using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// Anthropic Claude provider for recipe parsing.
/// 
/// <para>
/// Model Selection: This provider supports selection from Anthropic's pre-built models via the Model configuration property.
/// Available models include: claude-3-opus, claude-3-sonnet, claude-3-haiku, claude-3-5-sonnet, claude-3-5-haiku.
/// Users can configure their preferred model in appsettings.json under "AI:Anthropic:Model".
/// </para>
/// 
/// <para>
/// Custom Models: Anthropic does NOT support custom models or fine-tuning for general users.
/// Custom model creation is only available to enterprise customers for specialized use cases (e.g., "Claude Gov" for government applications).
/// The SupportsCustomModels capability is correctly set to false.
/// </para>
/// </summary>
public class AnthropicClaudeProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly AnthropicConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    
    public override string ProviderName => "Anthropic Claude";
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _httpClient != null;
    
    /// <summary>
    /// Provider capabilities for Anthropic Claude.
    /// Note: SupportsCustomModels is false because Anthropic only offers pre-built models (no custom model creation for general users).
    /// Model selection is available via the Model configuration property.
    /// </summary>
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
        SupportsCustomModels = false, // Anthropic does not support custom models for general users
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

    #region General AI Methods

    /// <summary>
    /// Send a chat request using native Anthropic Messages API.
    /// Note: Anthropic uses a separate "system" field rather than including it in messages.
    /// </summary>
    public override async Task<Models.AIChatResult> ChatAsync(Models.AIChatRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            if (_httpClient == null)
            {
                return Models.AIChatResult.Fail("Anthropic client not initialized", ProviderName);
            }

            _logger.LogInformation("Processing chat request with Anthropic Claude");

            // Get all messages including system instruction
            var allMessages = request.GetMessagesWithSystemInstruction();
            
            // Anthropic requires system message as a separate field, not in messages array
            string? systemInstruction = null;
            var messages = new List<object>();
            
            foreach (var msg in allMessages)
            {
                if (msg.Role == Models.AIChatRole.System)
                {
                    // Anthropic supports only one system message - use the first one (which is the SystemInstruction)
                    if (systemInstruction == null)
                    {
                        systemInstruction = msg.Content;
                    }
                }
                else
                {
                    messages.Add(new
                    {
                        role = msg.Role switch
                        {
                            Models.AIChatRole.User => "user",
                            Models.AIChatRole.Assistant => "assistant",
                            _ => "user"
                        },
                        content = msg.Content
                    });
                }
            }

            var requestBody = new Dictionary<string, object>
            {
                { "model", _config.Model },
                { "max_tokens", request.MaxTokens ?? _config.MaxTokens },
                { "temperature", request.Temperature ?? _config.Temperature },
                { "messages", messages }
            };

            if (!string.IsNullOrEmpty(systemInstruction))
            {
                requestBody["system"] = systemInstruction;
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/v1/messages")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("x-api-key", _config.ApiKey);
            requestMessage.Headers.Add("anthropic-version", "2023-06-01");

            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Models.AIChatResult.Fail($"API request failed: {response.StatusCode} - {errorContent}", ProviderName);
            }

            var content = await response.Content.ReadAsStringAsync();
            var anthropicResponse = JsonSerializer.Deserialize<AnthropicResponse>(content, _jsonOptions);

            if (anthropicResponse?.Content == null || !anthropicResponse.Content.Any())
            {
                return Models.AIChatResult.Fail("Invalid response from Anthropic", ProviderName);
            }

            var responseText = anthropicResponse.Content[0].Text;
            var usage = anthropicResponse.Usage;

            var cost = CalculateCost(new ProviderUsage
            {
                InputTokens = usage.InputTokens,
                OutputTokens = usage.OutputTokens
            });

            return new Models.AIChatResult
            {
                Message = Models.AIChatMessage.Assistant(responseText),
                Success = true,
                Provider = ProviderName,
                FinishReason = "end_turn",
                Usage = CreateUsageStats(usage.InputTokens, usage.OutputTokens, _config.CostPerInputToken, _config.CostPerOutputToken),
                Duration = stopwatch.Elapsed,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Anthropic Claude chat failed");
            return Models.AIChatResult.Fail(ex.Message, ProviderName);
        }
    }

    #endregion

    #region Vision/Image Analysis

    /// <summary>
    /// Analyze images using Anthropic Claude's vision capabilities.
    /// Supports base64-encoded images and system prompts for structured output.
    /// </summary>
    public override async Task<AIVisionResult> AnalyzeImageAsync(AIVisionRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            if (_httpClient == null)
            {
                return AIVisionResult.Fail("Anthropic client not initialized - check API key configuration", ProviderName);
            }

            if (request.ImageBase64 == null || !request.ImageBase64.Any())
            {
                return AIVisionResult.Fail("No base64 images provided in request", ProviderName);
            }

            _logger.LogInformation("Analyzing image with Anthropic Claude vision API");

            // Use first image (Anthropic supports multiple images, but we'll use the first for now)
            var base64Image = request.ImageBase64[0];
            
            // Detect image format from base64 data
            var imageBytes = Convert.FromBase64String(base64Image);
            var imageFormat = DetectImageFormat(imageBytes);

            // Build messages with image and text prompt
            var messages = new List<object>
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "image",
                            source = new
                            {
                                type = "base64",
                                media_type = imageFormat,
                                data = base64Image
                            }
                        },
                        new
                        {
                            type = "text",
                            text = request.Prompt
                        }
                    }
                }
            };

            // Build request body
            var requestBody = new Dictionary<string, object>
            {
                { "model", _config.Model },
                { "max_tokens", request.MaxTokens ?? _config.MaxTokens },
                { "temperature", _config.Temperature },
                { "messages", messages }
            };

            // Note: AIVisionRequest doesn't have SystemMessage property yet
            // For now, system instructions can be included in the prompt itself
            // Future enhancement: Add SystemMessage property to AIVisionRequest

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/v1/messages")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };
            
            requestMessage.Headers.Add("x-api-key", _config.ApiKey);
            requestMessage.Headers.Add("anthropic-version", "2023-06-01");

            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return AIVisionResult.Fail($"API request failed: {response.StatusCode} - {errorContent}", ProviderName);
            }

            var content = await response.Content.ReadAsStringAsync();
            var anthropicResponse = JsonSerializer.Deserialize<AnthropicResponse>(content, _jsonOptions);

            if (anthropicResponse?.Content == null || !anthropicResponse.Content.Any())
            {
                return AIVisionResult.Fail("Invalid response from Anthropic", ProviderName);
            }

            var responseText = anthropicResponse.Content[0].Text;
            var usage = anthropicResponse.Usage;

            var cost = CalculateCost(new ProviderUsage
            {
                InputTokens = usage.InputTokens,
                OutputTokens = usage.OutputTokens
            });

            var usageStats = CreateUsageStats(usage.InputTokens, usage.OutputTokens, _config.CostPerInputToken, _config.CostPerOutputToken);

            _logger.LogInformation("Anthropic Claude vision analysis completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}", 
                cost, usage.InputTokens + usage.OutputTokens);

            var result = AIVisionResult.Ok(responseText, ProviderName, usageStats);
            result.Duration = stopwatch.Elapsed;
            result.FinishReason = "end_turn";
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Anthropic Claude vision analysis failed");
            return AIVisionResult.Fail($"Vision analysis failed: {ex.Message}", ProviderName);
        }
    }

    /// <summary>
    /// Detect image format from byte array using magic bytes
    /// </summary>
    private string DetectImageFormat(byte[] imageBytes)
    {
        if (imageBytes.Length < 4)
        {
            return "image/png"; // Default fallback
        }

        // Check magic bytes for common image formats
        if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
        {
            return "image/jpeg";
        }
        else if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
        {
            return "image/png";
        }
        else if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x38)
        {
            return "image/gif";
        }
        else if (imageBytes[0] == 0x52 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x46)
        {
            // RIFF header - could be WebP
            return "image/webp";
        }

        // Default to PNG if format cannot be determined
        return "image/png";
    }

    #endregion
    
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
