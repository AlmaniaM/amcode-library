using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// LM Studio local AI provider for recipe parsing.
/// Uses OpenAI-compatible API format on localhost (default port 1234).
/// </summary>
public class LMStudioAIProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly LMStudioConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    
    public override string ProviderName => "LM Studio Local";
    public override bool RequiresInternet => false;
    public override bool IsAvailable => _httpClient != null;
    
    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = true,
        SupportsFunctionCalling = _config.EnableFunctionCalling,
        SupportsVision = _config.EnableVision,
        SupportsLongContext = true,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = _config.MaxTokens * 2,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt", "zh", "ja", "ko" },
        CostPerToken = 0m, // Local, no cost
        CostPerRequest = 0m,
        AverageResponseTime = _config.AverageResponseTime,
        SupportsCustomModels = true, // Can use any model loaded in LM Studio
        SupportsFineTuning = false,
        SupportsEmbeddings = false,
        SupportsModeration = false,
        MaxRequestsPerMinute = 1000, // Local, high limit
        MaxRequestsPerDay = int.MaxValue
    };
    
    public LMStudioAIProvider(
        ILogger<LMStudioAIProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<LMStudioConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        
        if (string.IsNullOrEmpty(_config.BaseUrl))
        {
            _logger.LogWarning("LM Studio base URL not configured");
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
                throw new InvalidOperationException("LM Studio client not initialized - check base URL configuration");
            }
            
            if (string.IsNullOrEmpty(_config.Model))
            {
                throw new InvalidOperationException("LM Studio model not configured - specify model name in configuration");
            }
            
            _logger.LogInformation("Parsing recipe text with LM Studio Local (Model: {Model})", _config.Model);
            
            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
            var request = CreateLMStudioRequest(prompt, options);
            
            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new AIException($"LM Studio API request failed: {response.StatusCode} - {errorContent}");
            }
            
            var result = await ProcessResponseAsync(response, cancellationToken);
            
            _logger.LogInformation("LM Studio parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}", 
                result.Cost, result.TokensUsed);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LM Studio parsing failed");
            throw new AIException($"LM Studio parsing failed: {ex.Message}", ex);
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
                    ErrorMessage = "LM Studio client not initialized - check base URL configuration"
                };
            }
            
            var startTime = DateTime.UtcNow;
            
            // Simple health check with a minimal request
            var request = CreateLMStudioRequest("Hello", new RecipeParsingOptions());
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/v1/chat/completions")
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
        // LM Studio is local, so no cost
        return 0m;
    }
    
    protected override bool CheckAvailability()
    {
        return _httpClient != null && !string.IsNullOrEmpty(_config.BaseUrl) && !string.IsNullOrEmpty(_config.Model);
    }
    
    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
        var request = CreateLMStudioRequest(prompt, options);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/v1/chat/completions")
        {
            Content = new StringContent(JsonSerializer.Serialize(request, _jsonOptions), Encoding.UTF8, "application/json")
        };
        
        // LM Studio doesn't require API key, but some setups might use it
        // No Authorization header needed for standard LM Studio setup
        
        return requestMessage;
    }
    
    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync();
        var lmStudioResponse = JsonSerializer.Deserialize<OpenAIResponse>(content, _jsonOptions);
        
        if (lmStudioResponse?.Choices == null || !lmStudioResponse.Choices.Any())
        {
            throw new AIException("Invalid response from LM Studio");
        }
        
        var responseText = lmStudioResponse.Choices[0].Message.Content;
        var usage = lmStudioResponse.Usage;
        
        var cost = CalculateCost(new ProviderUsage
        {
            InputTokens = usage.PromptTokens,
            OutputTokens = usage.CompletionTokens
        });
        
        return ParseJsonResponse(responseText, ProviderName, cost, usage.TotalTokens);
    }
    
    protected override decimal CalculateCost(ProviderUsage usage)
    {
        // LM Studio is local, so no cost
        return 0m;
    }
    
    private object CreateLMStudioRequest(string prompt, RecipeParsingOptions options)
    {
        return new
        {
            model = _config.Model,
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You are an expert recipe parser. Extract structured recipe information from text."
                },
                new
                {
                    role = "user",
                    content = prompt
                }
            },
            max_tokens = options.MaxTokens ?? _config.MaxTokens,
            temperature = options.Temperature ?? _config.Temperature,
            response_format = new { type = "json_object" }
        };
    }
}

