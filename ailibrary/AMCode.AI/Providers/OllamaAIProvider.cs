using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// Ollama local AI provider for recipe parsing
/// </summary>
public class OllamaAIProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly OllamaConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    
    public override string ProviderName => "Ollama Local";
    public override bool RequiresInternet => false;
    public override bool IsAvailable => _httpClient != null;
    
    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = true,
        SupportsFunctionCalling = false,
        SupportsVision = false,
        SupportsLongContext = true,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = _config.MaxTokens * 2,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt" },
        CostPerToken = 0m, // Local, no cost
        CostPerRequest = 0m,
        AverageResponseTime = _config.AverageResponseTime,
        SupportsCustomModels = true,
        SupportsFineTuning = false,
        SupportsEmbeddings = false,
        SupportsModeration = false,
        MaxRequestsPerMinute = 1000, // Local, high limit
        MaxRequestsPerDay = int.MaxValue
    };
    
    public OllamaAIProvider(
        ILogger<OllamaAIProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<OllamaConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        
        if (string.IsNullOrEmpty(_config.BaseUrl))
        {
            _logger.LogWarning("Ollama base URL not configured");
            _httpClient = null!;
        }
        else
        {
            _httpClient = httpClientFactory.CreateClient(ProviderName);
        }
    }
    
    public override async Task<ParsedRecipeResult> ParseTextAsync(string text, CancellationToken cancellationToken = default)
    {
        return await ParseTextAsync(text, new RecipeParsingOptions(), cancellationToken);
    }
    
    public override async Task<ParsedRecipeResult> ParseTextAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_httpClient == null)
            {
                throw new InvalidOperationException("Ollama client not initialized - check base URL configuration");
            }
            
            _logger.LogInformation("Parsing recipe text with Ollama Local");
            
            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
            var request = CreateOllamaRequest(prompt, options);
            
            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new AIException($"Ollama API request failed: {response.StatusCode} - {errorContent}");
            }
            
            var result = await ProcessResponseAsync(response, cancellationToken);
            
            _logger.LogInformation("Ollama parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}", 
                result.Cost, result.TokensUsed);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ollama parsing failed");
            throw new AIException($"Ollama parsing failed: {ex.Message}", ex);
        }
    }
    
    public override async Task<AIProviderHealth> CheckHealthAsync()
    {
        try
        {
            if (_httpClient == null)
            {
                return new AIProviderHealth
                {
                    IsHealthy = false,
                    Status = "Client not initialized",
                    LastChecked = DateTime.UtcNow,
                    ErrorMessage = "Ollama client not initialized - check base URL configuration"
                };
            }
            
            var startTime = DateTime.UtcNow;
            
            // Simple health check with a minimal request
            var request = CreateOllamaRequest("Hello", new RecipeParsingOptions());
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/api/generate")
            {
                Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
            };
            
            var response = await _httpClient.SendAsync(requestMessage, CancellationToken.None);
            var responseTime = DateTime.UtcNow - startTime;
            
            return new AIProviderHealth
            {
                IsHealthy = response.IsSuccessStatusCode,
                Status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"HTTP {response.StatusCode}",
                RequestsPerMinute = 0,
                QuotaRemaining = int.MaxValue
            };
        }
        catch (Exception ex)
        {
            return new AIProviderHealth
            {
                IsHealthy = false,
                Status = "Unhealthy",
                ResponseTime = TimeSpan.Zero,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = ex.Message
            };
        }
    }
    
    public override async Task<decimal> GetCostEstimateAsync(string text, RecipeParsingOptions options)
    {
        // Ollama is local, so no cost
        return 0m;
    }
    
    protected override bool CheckAvailability()
    {
        return _httpClient != null && !string.IsNullOrEmpty(_config.BaseUrl);
    }
    
    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
        var request = CreateOllamaRequest(prompt, options);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/api/generate")
        {
            Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
        };
        
        return requestMessage;
    }
    
    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync();
        var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(content, _jsonOptions);
        
        if (ollamaResponse?.Response == null)
        {
            throw new AIException("Invalid response from Ollama");
        }
        
        var responseText = ollamaResponse.Response;
        var usage = ollamaResponse.Usage;
        
        var cost = CalculateCost(new ProviderUsage
        {
            InputTokens = usage.PromptEvalCount,
            OutputTokens = usage.EvalCount
        });
        
        return ParseJsonResponse(responseText, ProviderName, cost, usage.PromptEvalCount + usage.EvalCount);
    }
    
    protected override decimal CalculateCost(ProviderUsage usage)
    {
        // Ollama is local, so no cost
        return 0m;
    }
    
    private object CreateOllamaRequest(string prompt, RecipeParsingOptions options)
    {
        return new
        {
            model = _config.Model,
            prompt = prompt,
            stream = false,
            options = new
            {
                temperature = options.Temperature ?? _config.Temperature,
                num_predict = options.MaxTokens ?? _config.MaxTokens,
                num_threads = _config.NumThreads,
                use_gpu = _config.UseGpu
            }
        };
    }
}

/// <summary>
/// Ollama response model
/// </summary>
public class OllamaResponse
{
    public string Model { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Response { get; set; } = string.Empty;
    public bool Done { get; set; }
    public OllamaUsage Usage { get; set; } = new();
}

/// <summary>
/// Ollama usage model
/// </summary>
public class OllamaUsage
{
    public int PromptEvalCount { get; set; }
    public int EvalCount { get; set; }
    public int EvalDuration { get; set; }
    public int LoadDuration { get; set; }
    public int PromptEvalDuration { get; set; }
    public int TotalDuration { get; set; }
}
