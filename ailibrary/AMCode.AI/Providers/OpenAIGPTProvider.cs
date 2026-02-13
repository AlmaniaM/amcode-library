using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;

namespace AMCode.AI.Providers;

/// <summary>
/// OpenAI GPT provider for recipe parsing
/// </summary>
public class OpenAIGPTProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly OpenAIConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;
    private readonly string _providerName;

    public override string ProviderName => _providerName;
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _httpClient != null;

    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = true,
        SupportsFunctionCalling = true,
        SupportsVision = true,
        SupportsLongContext = true,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = 128000,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt", "zh", "ja", "ko" },
        CostPerToken = _config.CostPerInputToken,
        CostPerRequest = 0.01m,
        AverageResponseTime = TimeSpan.FromSeconds(2),
        SupportsCustomModels = true,
        SupportsFineTuning = true,
        SupportsEmbeddings = true,
        SupportsModeration = true,
        MaxRequestsPerMinute = 500,
        MaxRequestsPerDay = 10000
    };

    public OpenAIGPTProvider(
        ILogger<OpenAIGPTProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<OpenAIConfiguration> config,
        PromptBuilderService promptBuilder,
        string? providerName = null)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));
        _providerName = providerName ?? "OpenAI GPT";

        if (string.IsNullOrEmpty(_config.ApiKey))
        {
            _logger.LogWarning("OpenAI API key not configured");
            _httpClient = null!;
        }
        else
        {
            _httpClient = httpClientFactory.CreateClient(_providerName);
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
                throw new InvalidOperationException("OpenAI client not initialized - check API key configuration");
            }

            _logger.LogInformation("Parsing recipe text with OpenAI GPT");

            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
            var request = CreateOpenAIRequest(prompt, options);

            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new AIException($"OpenAI API request failed: {response.StatusCode} - {errorContent}");
            }

            var result = await ProcessResponseAsync(response, cancellationToken);

            _logger.LogInformation("OpenAI GPT parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}",
                result.Cost, result.TokensUsed);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenAI GPT parsing failed");
            throw new AIException($"OpenAI GPT parsing failed: {ex.Message}", ex);
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
                    ErrorMessage = "OpenAI client not initialized - check API key configuration"
                };
            }

            var startTime = DateTime.UtcNow;

            // Simple health check with a minimal request
            var request = CreateOpenAIRequest("Hello", new RecipeParsingOptions());
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
            _logger.LogError(ex, "Failed to calculate cost estimate for OpenAI GPT");
            return 0m;
        }
    }

    #region General AI Methods

    /// <summary>
    /// Send a chat request using native OpenAI chat completions API
    /// </summary>
    public override async Task<Models.AIChatResult> ChatAsync(Models.AIChatRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            if (_httpClient == null)
            {
                return Models.AIChatResult.Fail("OpenAI client not initialized", ProviderName);
            }

            _logger.LogInformation("Processing chat request with OpenAI GPT");

            // Get messages including system instruction
            var allMessages = request.GetMessagesWithSystemInstruction();

            // Build messages array for OpenAI API
            var messages = allMessages.Select(m => new
            {
                role = m.Role switch
                {
                    Models.AIChatRole.System => "system",
                    Models.AIChatRole.User => "user",
                    Models.AIChatRole.Assistant => "assistant",
                    _ => "user"
                },
                content = m.Content
            }).ToArray();

            var maxTokens = request.MaxTokens ?? _config.MaxTokens;
            var requestBodyDict = new Dictionary<string, object>
            {
                { "model", _config.Model },
                { "messages", messages }
            };

            // Only include temperature if the model supports custom values
            if (!RequiresDefaultTemperature(_config.Model))
            {
                requestBodyDict["temperature"] = request.Temperature ?? _config.Temperature;
            }

            // Use max_completion_tokens for models that require it (e.g., gpt-5-nano)
            if (RequiresMaxCompletionTokens(_config.Model))
            {
                requestBodyDict["max_completion_tokens"] = maxTokens;
            }
            else
            {
                requestBodyDict["max_tokens"] = maxTokens;
            }

            var requestBody = requestBodyDict;

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Models.AIChatResult.Fail($"API request failed: {response.StatusCode} - {errorContent}", ProviderName);
            }

            var content = await response.Content.ReadAsStringAsync();
            var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(content, _jsonOptions);

            if (openAIResponse?.Choices == null || !openAIResponse.Choices.Any())
            {
                return Models.AIChatResult.Fail("Invalid response from OpenAI", ProviderName);
            }

            var responseText = openAIResponse.Choices[0].Message.Content;
            var usage = openAIResponse.Usage;

            var cost = CalculateCost(new ProviderUsage
            {
                InputTokens = usage.PromptTokens,
                OutputTokens = usage.CompletionTokens
            });

            return new Models.AIChatResult
            {
                Message = Models.AIChatMessage.Assistant(responseText),
                Success = true,
                Provider = ProviderName,
                FinishReason = openAIResponse.Choices[0].FinishReason,
                Usage = CreateUsageStats(usage.PromptTokens, usage.CompletionTokens, _config.CostPerInputToken, _config.CostPerOutputToken),
                Duration = stopwatch.Elapsed,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenAI GPT chat failed");
            return Models.AIChatResult.Fail(ex.Message, ProviderName);
        }
    }

    #endregion

    protected override bool CheckAvailability()
    {
        return _httpClient != null && !string.IsNullOrEmpty(_config.ApiKey);
    }

    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
        var request = CreateOpenAIRequest(prompt, options);

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
        var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(content, _jsonOptions);

        if (openAIResponse?.Choices == null || !openAIResponse.Choices.Any())
        {
            throw new AIException("Invalid response from OpenAI");
        }

        var responseText = openAIResponse.Choices[0].Message.Content;
        var usage = openAIResponse.Usage;

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

    private object CreateOpenAIRequest(string prompt, RecipeParsingOptions options)
    {
            var maxTokens = options.MaxTokens ?? _config.MaxTokens;
        var requestBody = new Dictionary<string, object>
        {
            { "model", _config.Model },
            { "messages", new[]
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
                }
            },
            { "response_format", new { type = "json_object" } }
        };

        // Only include temperature if the model supports custom values
        if (!RequiresDefaultTemperature(_config.Model))
        {
            requestBody["temperature"] = options.Temperature ?? _config.Temperature;
        }

        // Use max_completion_tokens for models that require it (e.g., gpt-5-nano)
        if (RequiresMaxCompletionTokens(_config.Model))
        {
            requestBody["max_completion_tokens"] = maxTokens;
        }
        else
        {
            requestBody["max_tokens"] = maxTokens;
        }

        return requestBody;
    }

    /// <summary>
    /// Checks if the model requires max_completion_tokens instead of max_tokens
    ///
    /// Models requiring max_completion_tokens:
    /// - GPT-5 series (gpt-5, gpt-5-nano, gpt-5-mini): Use max_completion_tokens
    /// - O1 series (o1, o1-preview, o1-mini): Use max_completion_tokens
    ///
    /// Reference: https://platform.openai.com/docs/guides/gpt-5
    /// </summary>
    private static bool RequiresMaxCompletionTokens(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
            return false;

        // Models that require max_completion_tokens (typically newer models)
        var modelsRequiringMaxCompletionTokens = new[]
        {
            "gpt-5-nano",
            "gpt-5-mini",
            "gpt-5",
            "o1",
            "o1-preview",
            "o1-mini"
        };

        return modelsRequiringMaxCompletionTokens.Any(m =>
            model.Contains(m, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Checks if the model doesn't support custom temperature values
    ///
    /// For these models, the temperature parameter must be omitted entirely from the request:
    /// - Reasoning models (o1, o3 series): Only support default temperature (1), cannot be customized
    /// - GPT-5 series (gpt-5, gpt-5-nano, gpt-5-mini): Don't support temperature parameter at all
    ///
    /// Note: GPT-5.1 with reasoning_effort=none DOES support temperature, but other GPT-5 models do not.
    ///
    /// Reference: https://platform.openai.com/docs/guides/gpt-5
    /// </summary>
    private static bool RequiresDefaultTemperature(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
            return false;

        // Models that don't support custom temperature (temperature parameter must be omitted)
        var modelsRequiringDefaultTemperature = new[]
        {
            // Reasoning models - only support default temperature (1)
            "o1",
            "o1-preview",
            "o1-mini",
            "o3",
            "o3-mini",
            // GPT-5 series - don't support temperature parameter at all
            "gpt-5-nano",
            "gpt-5-mini",
            "gpt-5"
        };

        return modelsRequiringDefaultTemperature.Any(m =>
            model.Contains(m, StringComparison.OrdinalIgnoreCase));
    }

    #region Vision/Image Analysis

    /// <summary>
    /// Analyze images using OpenAI's vision capabilities (GPT-4o, GPT-4o-mini).
    /// Sends base64-encoded images via the Chat Completions API.
    /// </summary>
    public override async Task<AIVisionResult> AnalyzeImageAsync(AIVisionRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            if (_httpClient == null)
            {
                return AIVisionResult.Fail("OpenAI client not initialized - check API key configuration", ProviderName);
            }

            if ((request.ImageBase64 == null || !request.ImageBase64.Any()) &&
                (request.ImageUrls == null || !request.ImageUrls.Any()))
            {
                return AIVisionResult.Fail("No images provided in request", ProviderName);
            }

            _logger.LogInformation("Analyzing image with OpenAI vision API (model: {Model})", _config.Model);

            // Build content array with images and text prompt
            var contentParts = new List<object>();

            // Add base64 images
            if (request.ImageBase64 != null)
            {
                foreach (var base64Image in request.ImageBase64)
                {
                    var imageBytes = Convert.FromBase64String(base64Image);
                    var mediaType = DetectImageFormat(imageBytes);

                    contentParts.Add(new
                    {
                        type = "image_url",
                        image_url = new
                        {
                            url = $"data:{mediaType};base64,{base64Image}",
                            detail = request.DetailLevel ?? "auto"
                        }
                    });
                }
            }

            // Add URL images
            if (request.ImageUrls != null)
            {
                foreach (var imageUrl in request.ImageUrls)
                {
                    contentParts.Add(new
                    {
                        type = "image_url",
                        image_url = new
                        {
                            url = imageUrl,
                            detail = request.DetailLevel ?? "auto"
                        }
                    });
                }
            }

            // Add text prompt
            contentParts.Add(new
            {
                type = "text",
                text = request.Prompt
            });

            var messages = new List<object>
            {
                new { role = "user", content = contentParts }
            };

            var maxTokens = request.MaxTokens ?? _config.MaxTokens;
            var requestBodyDict = new Dictionary<string, object>
            {
                { "model", _config.Model },
                { "messages", messages }
            };

            if (!RequiresDefaultTemperature(_config.Model))
            {
                requestBodyDict["temperature"] = _config.Temperature;
            }

            if (RequiresMaxCompletionTokens(_config.Model))
            {
                requestBodyDict["max_completion_tokens"] = maxTokens;
            }
            else
            {
                requestBodyDict["max_tokens"] = maxTokens;
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBodyDict, _jsonOptions), Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return AIVisionResult.Fail($"API request failed: {response.StatusCode} - {errorContent}", ProviderName);
            }

            var content = await response.Content.ReadAsStringAsync();
            var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(content, _jsonOptions);

            if (openAIResponse?.Choices == null || !openAIResponse.Choices.Any())
            {
                return AIVisionResult.Fail("Invalid response from OpenAI vision API", ProviderName);
            }

            var responseText = openAIResponse.Choices[0].Message.Content;
            var usage = openAIResponse.Usage;

            var usageStats = CreateUsageStats(usage.PromptTokens, usage.CompletionTokens, _config.CostPerInputToken, _config.CostPerOutputToken);

            _logger.LogInformation("OpenAI vision analysis completed successfully. Tokens: {Tokens}", usage.TotalTokens);

            var result = AIVisionResult.Ok(responseText, ProviderName, usageStats);
            result.Duration = stopwatch.Elapsed;
            result.FinishReason = openAIResponse.Choices[0].FinishReason;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenAI vision analysis failed");
            return AIVisionResult.Fail($"Vision analysis failed: {ex.Message}", ProviderName);
        }
    }

    /// <summary>
    /// Detect image format from byte array using magic bytes
    /// </summary>
    private static string DetectImageFormat(byte[] imageBytes)
    {
        if (imageBytes.Length < 4)
            return "image/png";

        if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
            return "image/jpeg";

        if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
            return "image/png";

        if (imageBytes[0] == 0x52 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x46)
            return "image/webp";

        if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46)
            return "image/gif";

        return "image/png";
    }

    #endregion
}

/// <summary>
/// OpenAI API response model
/// </summary>
public class OpenAIResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public OpenAIChoice[] Choices { get; set; } = Array.Empty<OpenAIChoice>();
    public OpenAIUsage Usage { get; set; } = new();
}

/// <summary>
/// OpenAI choice model
/// </summary>
public class OpenAIChoice
{
    public int Index { get; set; }
    public OpenAIMessage Message { get; set; } = new();
    public string FinishReason { get; set; } = string.Empty;
}

/// <summary>
/// OpenAI message model
/// </summary>
public class OpenAIMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// OpenAI usage model
/// </summary>
public class OpenAIUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

/// <summary>
/// AI exception for provider-specific errors
/// </summary>
public class AIException : Exception
{
    public AIException(string message) : base(message) { }
    public AIException(string message, Exception innerException) : base(message, innerException) { }
}
