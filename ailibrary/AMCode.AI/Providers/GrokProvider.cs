using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// Grok (X.AI) provider for recipe parsing
/// </summary>
public class GrokProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly GrokConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    
    public override string ProviderName => "Grok (X.AI)";
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _httpClient != null;
    
    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = true,
        SupportsFunctionCalling = false,
        SupportsVision = false,
        SupportsLongContext = true,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = 128000,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt" },
        CostPerToken = _config.CostPerInputToken,
        CostPerRequest = 0.005m,
        AverageResponseTime = TimeSpan.FromSeconds(1.5),
        SupportsCustomModels = false,
        SupportsFineTuning = false,
        SupportsEmbeddings = false,
        SupportsModeration = false,
        MaxRequestsPerMinute = 200,
        MaxRequestsPerDay = 5000
    };
    
    public GrokProvider(
        ILogger<GrokProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<GrokConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        
        if (string.IsNullOrEmpty(_config.ApiKey))
        {
            _logger.LogWarning("Grok API key not configured");
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
                throw new InvalidOperationException("Grok client not initialized - check API key configuration");
            }
            
            _logger.LogInformation("Parsing recipe text with Grok (X.AI)");
            
            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
            var request = CreateGrokRequest(prompt, options);
            
            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new AIException($"Grok API request failed: {response.StatusCode} - {errorContent}");
            }
            
            var result = await ProcessResponseAsync(response, cancellationToken);
            
            _logger.LogInformation("Grok parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}", 
                result.Cost, result.TokensUsed);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Grok parsing failed");
            throw new AIException($"Grok parsing failed: {ex.Message}", ex);
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
                    ErrorMessage = "Grok client not initialized - check API key configuration"
                };
            }
            
            var startTime = DateTime.UtcNow;
            
            // Simple health check with a minimal request
            var request = CreateGrokRequest("Hello", new RecipeParsingOptions());
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
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
            _logger.LogError(ex, "Failed to calculate cost estimate for Grok");
            return 0m;
        }
    }
    
    protected override bool CheckAvailability()
    {
        return _httpClient != null && !string.IsNullOrEmpty(_config.ApiKey);
    }
    
    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
        var request = CreateGrokRequest(prompt, options);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
        {
            Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
        };
        
        requestMessage.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");
        
        return requestMessage;
    }
    
    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync();
        var grokResponse = JsonSerializer.Deserialize<GrokResponse>(content, _jsonOptions);
        
        if (grokResponse?.Choices == null || !grokResponse.Choices.Any())
        {
            throw new AIException("Invalid response from Grok");
        }
        
        var responseText = grokResponse.Choices[0].Message.Content;
        var usage = grokResponse.Usage;
        
        var cost = CalculateCost(new ProviderUsage
        {
            InputTokens = usage.PromptTokens,
            OutputTokens = usage.CompletionTokens
        });
        
        return ParseJsonResponse(responseText, ProviderName, cost, usage.TotalTokens);
    }
    
    protected override decimal CalculateCost(ProviderUsage usage)
    {
        var inputCost = usage.InputTokens * _config.CostPerInputToken;
        var outputCost = usage.OutputTokens * _config.CostPerOutputToken;
        return inputCost + outputCost;
    }
    
    private object CreateGrokRequest(string prompt, RecipeParsingOptions options)
    {
        return new
        {
            model = _config.Model,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = prompt
                }
            },
            max_tokens = options.MaxTokens ?? _config.MaxTokens,
            temperature = options.Temperature ?? _config.Temperature,
            stream = false
        };
    }
}

/// <summary>
/// Grok API response model
/// </summary>
public class GrokResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public GrokChoice[] Choices { get; set; } = Array.Empty<GrokChoice>();
    public GrokUsage Usage { get; set; } = new();
}

/// <summary>
/// Grok choice model
/// </summary>
public class GrokChoice
{
    public int Index { get; set; }
    public GrokMessage Message { get; set; } = new();
    public string FinishReason { get; set; } = string.Empty;
}

/// <summary>
/// Grok message model
/// </summary>
public class GrokMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// Grok usage model
/// </summary>
public class GrokUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}
