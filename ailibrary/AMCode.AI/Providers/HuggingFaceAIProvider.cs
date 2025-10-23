using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// Hugging Face AI provider for recipe parsing
/// </summary>
public class HuggingFaceAIProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly HuggingFaceConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    
    public override string ProviderName => "Hugging Face";
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _httpClient != null;
    
    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = false,
        SupportsFunctionCalling = false,
        SupportsVision = false,
        SupportsLongContext = false,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = _config.MaxTokens,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt" },
        CostPerToken = _config.CostPerInputToken,
        CostPerRequest = _config.CostPerRequest,
        AverageResponseTime = TimeSpan.FromSeconds(5),
        SupportsCustomModels = true,
        SupportsFineTuning = true,
        SupportsEmbeddings = true,
        SupportsModeration = false,
        MaxRequestsPerMinute = 10,
        MaxRequestsPerDay = 100
    };
    
    public HuggingFaceAIProvider(
        ILogger<HuggingFaceAIProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<HuggingFaceConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        
        if (string.IsNullOrEmpty(_config.ApiKey))
        {
            _logger.LogWarning("Hugging Face API key not configured");
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
                throw new InvalidOperationException("Hugging Face client not initialized - check API key configuration");
            }
            
            _logger.LogInformation("Parsing recipe text with Hugging Face");
            
            var prompt = _promptBuilder.BuildSimpleRecipeParsingPrompt(text, options);
            var request = CreateHuggingFaceRequest(prompt, options);
            
            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new AIException($"Hugging Face API request failed: {response.StatusCode} - {errorContent}");
            }
            
            var result = await ProcessResponseAsync(response, cancellationToken);
            
            _logger.LogInformation("Hugging Face parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}", 
                result.Cost, result.TokensUsed);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hugging Face parsing failed");
            throw new AIException($"Hugging Face parsing failed: {ex.Message}", ex);
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
                    ErrorMessage = "Hugging Face client not initialized - check API key configuration"
                };
            }
            
            var startTime = DateTime.UtcNow;
            
            // Simple health check with a minimal request
            var request = CreateHuggingFaceRequest("Hello", new RecipeParsingOptions());
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/models/{_config.Model}")
            {
                Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
            };
            
            requestMessage.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");
            
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
        try
        {
            var estimatedTokens = EstimateTokenCount(text) + (options.MaxTokens ?? _config.MaxTokens);
            var inputTokens = EstimateTokenCount(text);
            var outputTokens = estimatedTokens - inputTokens;
            
            return CalculateCost(new ProviderUsage
            {
                InputTokens = inputTokens,
                OutputTokens = outputTokens
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to calculate cost estimate for Hugging Face");
            return 0m;
        }
    }
    
    protected override bool CheckAvailability()
    {
        return _httpClient != null && !string.IsNullOrEmpty(_config.ApiKey);
    }
    
    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        var prompt = _promptBuilder.BuildSimpleRecipeParsingPrompt(text, options);
        var request = CreateHuggingFaceRequest(prompt, options);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/models/{_config.Model}")
        {
            Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
        };
        
        requestMessage.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");
        
        return requestMessage;
    }
    
    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync();
        var huggingFaceResponse = JsonSerializer.Deserialize<HuggingFaceResponse>(content, _jsonOptions);
        
        if (huggingFaceResponse?.GeneratedText == null)
        {
            throw new AIException("Invalid response from Hugging Face");
        }
        
        var responseText = huggingFaceResponse.GeneratedText;
        var estimatedTokens = EstimateTokenCount(responseText);
        
        var cost = CalculateCost(new ProviderUsage
        {
            InputTokens = estimatedTokens / 2, // Rough estimate
            OutputTokens = estimatedTokens / 2
        });
        
        return ParseJsonResponse(responseText, ProviderName, cost, estimatedTokens);
    }
    
    protected override decimal CalculateCost(ProviderUsage usage)
    {
        var inputCost = usage.InputTokens * _config.CostPerInputToken;
        var outputCost = usage.OutputTokens * _config.CostPerOutputToken;
        return inputCost + outputCost;
    }
    
    private object CreateHuggingFaceRequest(string prompt, RecipeParsingOptions options)
    {
        return new
        {
            inputs = prompt,
            parameters = new
            {
                max_new_tokens = options.MaxTokens ?? _config.MaxTokens,
                temperature = options.Temperature ?? _config.Temperature,
                return_full_text = false
            }
        };
    }
}

/// <summary>
/// Hugging Face response model
/// </summary>
public class HuggingFaceResponse
{
    public string GeneratedText { get; set; } = string.Empty;
}
