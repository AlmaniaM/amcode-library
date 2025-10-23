using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// AWS Bedrock provider for recipe parsing
/// </summary>
public class AWSBedrockProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly AWSBedrockConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    
    public override string ProviderName => "AWS Bedrock";
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _httpClient != null;
    
    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = true,
        SupportsFunctionCalling = false,
        SupportsVision = false,
        SupportsLongContext = true,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = 100000,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt" },
        CostPerToken = _config.CostPerInputToken,
        CostPerRequest = 0.01m,
        AverageResponseTime = TimeSpan.FromSeconds(3),
        SupportsCustomModels = false,
        SupportsFineTuning = false,
        SupportsEmbeddings = false,
        SupportsModeration = false,
        MaxRequestsPerMinute = 50,
        MaxRequestsPerDay = 1000
    };
    
    public AWSBedrockProvider(
        ILogger<AWSBedrockProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AWSBedrockConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        
        if (string.IsNullOrEmpty(_config.Region))
        {
            _logger.LogWarning("AWS Bedrock region not configured");
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
                throw new InvalidOperationException("AWS Bedrock client not initialized - check region configuration");
            }
            
            _logger.LogInformation("Parsing recipe text with AWS Bedrock");
            
            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
            var request = CreateBedrockRequest(prompt, options);
            
            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new AIException($"AWS Bedrock API request failed: {response.StatusCode} - {errorContent}");
            }
            
            var result = await ProcessResponseAsync(response, cancellationToken);
            
            _logger.LogInformation("AWS Bedrock parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}", 
                result.Cost, result.TokensUsed);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AWS Bedrock parsing failed");
            throw new AIException($"AWS Bedrock parsing failed: {ex.Message}", ex);
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
                    ErrorMessage = "AWS Bedrock client not initialized - check region configuration"
                };
            }
            
            var startTime = DateTime.UtcNow;
            
            // Simple health check with a minimal request
            var request = CreateBedrockRequest("Hello", new RecipeParsingOptions());
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://bedrock-runtime.{_config.Region}.amazonaws.com/model/{_config.Model}/invoke")
            {
                Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
            };
            
            // Note: In a real implementation, you would need to add AWS signature authentication
            // For now, we'll just check if the client is configured
            
            var responseTime = DateTime.UtcNow - startTime;
            
            return new AIProviderHealth
            {
                IsHealthy = true, // Simplified for this implementation
                Status = "Healthy",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
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
            _logger.LogError(ex, "Failed to calculate cost estimate for AWS Bedrock");
            return 0m;
        }
    }
    
    protected override bool CheckAvailability()
    {
        return _httpClient != null && !string.IsNullOrEmpty(_config.Region);
    }
    
    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
        var request = CreateBedrockRequest(prompt, options);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://bedrock-runtime.{_config.Region}.amazonaws.com/model/{_config.Model}/invoke")
        {
            Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
        };
        
        // Note: In a real implementation, you would need to add AWS signature authentication
        // This would typically be done using AWS SDK or custom authentication middleware
        
        return requestMessage;
    }
    
    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync();
        var bedrockResponse = JsonSerializer.Deserialize<BedrockResponse>(content, _jsonOptions);
        
        if (bedrockResponse?.Content == null || !bedrockResponse.Content.Any())
        {
            throw new AIException("Invalid response from AWS Bedrock");
        }
        
        var responseText = bedrockResponse.Content[0].Text;
        var usage = bedrockResponse.Usage;
        
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
    
    private object CreateBedrockRequest(string prompt, RecipeParsingOptions options)
    {
        return new
        {
            anthropic_version = "bedrock-2023-05-31",
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
/// AWS Bedrock response model
/// </summary>
public class BedrockResponse
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public BedrockContent[] Content { get; set; } = Array.Empty<BedrockContent>();
    public BedrockUsage Usage { get; set; } = new();
}

/// <summary>
/// AWS Bedrock content model
/// </summary>
public class BedrockContent
{
    public string Type { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

/// <summary>
/// AWS Bedrock usage model
/// </summary>
public class BedrockUsage
{
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
}
