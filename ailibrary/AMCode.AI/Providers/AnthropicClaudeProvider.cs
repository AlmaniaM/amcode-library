using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// Anthropic Claude provider for recipe parsing
/// </summary>
public class AnthropicClaudeProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly AnthropicConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    
    public override string ProviderName => "Anthropic Claude";
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _httpClient != null;
    
    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = true,
        SupportsFunctionCalling = true,
        SupportsVision = true,
        SupportsLongContext = true,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = 200000,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt", "zh", "ja", "ko" },
        CostPerToken = _config.CostPerInputToken,
        CostPerRequest = 0.008m,
        AverageResponseTime = TimeSpan.FromSeconds(2.5),
        SupportsCustomModels = false,
        SupportsFineTuning = false,
        SupportsEmbeddings = false,
        SupportsModeration = false,
        MaxRequestsPerMinute = 100,
        MaxRequestsPerDay = 1000
    };
    
    public AnthropicClaudeProvider(
        ILogger<AnthropicClaudeProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AnthropicConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        
        if (string.IsNullOrEmpty(_config.ApiKey))
        {
            _logger.LogWarning("Anthropic API key not configured");
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
                throw new InvalidOperationException("Anthropic client not initialized - check API key configuration");
            }
            
            _logger.LogInformation("Parsing recipe text with Anthropic Claude");
            
            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
            var request = CreateAnthropicRequest(prompt, options);
            
            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new AIException($"Anthropic API request failed: {response.StatusCode} - {errorContent}");
            }
            
            var result = await ProcessResponseAsync(response, cancellationToken);
            
            _logger.LogInformation("Anthropic Claude parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}", 
                result.Cost, result.TokensUsed);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Anthropic Claude parsing failed");
            throw new AIException($"Anthropic Claude parsing failed: {ex.Message}", ex);
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
                    ErrorMessage = "Anthropic client not initialized - check API key configuration"
                };
            }
            
            var startTime = DateTime.UtcNow;
            
            // Simple health check with a minimal request
            var request = CreateAnthropicRequest("Hello", new RecipeParsingOptions());
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/v1/messages")
            {
                Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
            };
            
            requestMessage.Headers.Add("x-api-key", _config.ApiKey);
            requestMessage.Headers.Add("anthropic-version", "2023-06-01");
            
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
            _logger.LogError(ex, "Failed to calculate cost estimate for Anthropic Claude");
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
        var request = CreateAnthropicRequest(prompt, options);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/v1/messages")
        {
            Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
        };
        
        requestMessage.Headers.Add("x-api-key", _config.ApiKey);
        requestMessage.Headers.Add("anthropic-version", "2023-06-01");
        
        return requestMessage;
    }
    
    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync();
        var anthropicResponse = JsonSerializer.Deserialize<AnthropicResponse>(content, _jsonOptions);
        
        if (anthropicResponse?.Content == null || !anthropicResponse.Content.Any())
        {
            throw new AIException("Invalid response from Anthropic Claude");
        }
        
        var responseText = anthropicResponse.Content[0].Text;
        var usage = anthropicResponse.Usage;
        
        var cost = CalculateCost(new ProviderUsage
        {
            InputTokens = usage.InputTokens,
            OutputTokens = usage.OutputTokens
        });
        
        return ParseJsonResponse(responseText, ProviderName, cost, usage.InputTokens + usage.OutputTokens);
    }
    
    protected override decimal CalculateCost(ProviderUsage usage)
    {
        var inputCost = usage.InputTokens * _config.CostPerInputToken;
        var outputCost = usage.OutputTokens * _config.CostPerOutputToken;
        return inputCost + outputCost;
    }
    
    private object CreateAnthropicRequest(string prompt, RecipeParsingOptions options)
    {
        return new
        {
            model = _config.Model,
            max_tokens = options.MaxTokens ?? _config.MaxTokens,
            temperature = options.Temperature ?? _config.Temperature,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = prompt
                }
            }
        };
    }
}

/// <summary>
/// Anthropic API response model
/// </summary>
public class AnthropicResponse
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public AnthropicContent[] Content { get; set; } = Array.Empty<AnthropicContent>();
    public AnthropicUsage Usage { get; set; } = new();
}

/// <summary>
/// Anthropic content model
/// </summary>
public class AnthropicContent
{
    public string Type { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

/// <summary>
/// Anthropic usage model
/// </summary>
public class AnthropicUsage
{
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
}
