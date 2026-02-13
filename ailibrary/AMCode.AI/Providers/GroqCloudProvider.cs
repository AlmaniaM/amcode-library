using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text;
using System.Diagnostics;

namespace AMCode.AI.Providers;

/// <summary>
/// Groq Cloud provider for ultra-fast AI inference.
/// Uses Groq's OpenAI-compatible API (api.groq.com).
/// Supports: Chat, Completion, Structured JSON, Vision (Llama 4 Scout).
/// Does NOT support: Embeddings, Function Calling.
/// </summary>
public class GroqCloudProvider : GenericAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly GroqCloudConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;

    public override string ProviderName => "Groq Cloud";
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _httpClient != null;

    public override AIProviderCapabilities Capabilities => new AIProviderCapabilities
    {
        SupportsStreaming = true,
        SupportsFunctionCalling = false,
        SupportsVision = _config.EnableVision,
        SupportsLongContext = true,
        MaxTokens = _config.MaxTokens,
        MaxContextLength = 128000,
        SupportedLanguages = new[] { "en", "es", "fr", "de", "it", "pt", "zh", "ja", "ko" },
        CostPerToken = _config.CostPerInputToken,
        CostPerRequest = 0.001m,
        AverageResponseTime = TimeSpan.FromMilliseconds(500),
        SupportsCustomModels = false,
        SupportsFineTuning = false,
        SupportsEmbeddings = false,
        SupportsModeration = false,
        MaxRequestsPerMinute = 30,
        MaxRequestsPerDay = 14400
    };

    public GroqCloudProvider(
        ILogger<GroqCloudProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<GroqCloudConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));

        if (string.IsNullOrEmpty(_config.ApiKey))
        {
            _logger.LogWarning("Groq Cloud API key not configured");
            _httpClient = null!;
        }
        else
        {
            _httpClient = httpClientFactory.CreateClient(ProviderName);
        }
    }

    #region Chat (Core Implementation)

    public override async Task<AIChatResult> ChatAsync(AIChatRequest request, CancellationToken cancellationToken = default)
    {
        if (_httpClient == null)
        {
            return AIChatResult.Fail("Groq Cloud client not initialized - check API key configuration", ProviderName);
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var messages = new List<object>();

            foreach (var msg in request.GetMessagesWithSystemInstruction())
            {
                messages.Add(new { role = msg.Role.ToString().ToLowerInvariant(), content = msg.Content });
            }

            var requestBody = new
            {
                model = _config.Model,
                messages,
                max_tokens = request.MaxTokens ?? _config.MaxTokens,
                temperature = request.Temperature ?? _config.Temperature,
                top_p = request.TopP,
                stop = request.StopSequences,
                stream = false
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Groq Cloud API error: {StatusCode} - {Error}", response.StatusCode, errorContent);
                return AIChatResult.Fail($"Groq Cloud API error: {response.StatusCode} - {errorContent}", ProviderName);
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var groqResponse = JsonSerializer.Deserialize<GroqCloudResponse>(content, _jsonOptions);

            if (groqResponse?.Choices == null || groqResponse.Choices.Length == 0)
            {
                return AIChatResult.Fail("Empty response from Groq Cloud", ProviderName);
            }

            var responseMessage = groqResponse.Choices[0].Message;
            var usage = groqResponse.Usage;
            var cost = CalculateCost(new ProviderUsage
            {
                InputTokens = usage.PromptTokens,
                OutputTokens = usage.CompletionTokens
            });

            return new AIChatResult
            {
                Success = true,
                Message = AIChatMessage.Assistant(responseMessage.Content),
                Provider = ProviderName,
                FinishReason = groqResponse.Choices[0].FinishReason,
                Usage = new AIUsageStats
                {
                    InputTokens = usage.PromptTokens,
                    OutputTokens = usage.CompletionTokens,
                    EstimatedCost = cost
                },
                Duration = stopwatch.Elapsed,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Groq Cloud chat failed");
            return AIChatResult.Fail($"Groq Cloud chat failed: {ex.Message}", ProviderName);
        }
    }

    #endregion

    #region Vision

    public override async Task<AIVisionResult> AnalyzeImageAsync(AIVisionRequest request, CancellationToken cancellationToken = default)
    {
        if (_httpClient == null)
        {
            return AIVisionResult.Fail("Groq Cloud client not initialized - check API key configuration", ProviderName);
        }

        if (!_config.EnableVision)
        {
            return AIVisionResult.Fail("Vision not enabled for Groq Cloud", ProviderName);
        }

        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Build content array with text and images
            var contentParts = new List<object>
            {
                new { type = "text", text = request.Prompt }
            };

            // Add image URLs
            if (request.ImageUrls != null)
            {
                foreach (var url in request.ImageUrls)
                {
                    contentParts.Add(new
                    {
                        type = "image_url",
                        image_url = new { url, detail = request.DetailLevel }
                    });
                }
            }

            // Add base64 images
            if (request.ImageBase64 != null)
            {
                foreach (var base64 in request.ImageBase64)
                {
                    contentParts.Add(new
                    {
                        type = "image_url",
                        image_url = new { url = $"data:image/jpeg;base64,{base64}", detail = request.DetailLevel }
                    });
                }
            }

            var requestBody = new
            {
                model = _config.VisionModel,
                messages = new[]
                {
                    new { role = "user", content = contentParts }
                },
                max_tokens = request.MaxTokens ?? _config.MaxTokens,
                temperature = 0.1f,
                stream = false
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                return AIVisionResult.Fail($"Groq Cloud vision error: {response.StatusCode} - {errorContent}", ProviderName);
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var groqResponse = JsonSerializer.Deserialize<GroqCloudResponse>(content, _jsonOptions);

            if (groqResponse?.Choices == null || groqResponse.Choices.Length == 0)
            {
                return AIVisionResult.Fail("Empty vision response from Groq Cloud", ProviderName);
            }

            var responseContent = groqResponse.Choices[0].Message.Content;
            var usage = groqResponse.Usage;

            return new AIVisionResult
            {
                Content = responseContent,
                Success = true,
                Provider = ProviderName,
                Usage = new AIUsageStats
                {
                    InputTokens = usage.PromptTokens,
                    OutputTokens = usage.CompletionTokens,
                    EstimatedCost = CalculateCost(new ProviderUsage
                    {
                        InputTokens = usage.PromptTokens,
                        OutputTokens = usage.CompletionTokens
                    })
                },
                Duration = stopwatch.Elapsed,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Groq Cloud vision analysis failed");
            return AIVisionResult.Fail($"Groq Cloud vision failed: {ex.Message}", ProviderName);
        }
    }

    #endregion

    #region Recipe Parsing (Required by GenericAIProvider)

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
                throw new InvalidOperationException("Groq Cloud client not initialized - check API key configuration");
            }

            _logger.LogInformation("Parsing recipe text with Groq Cloud");

            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);
            var requestMessage = await CreateRequestAsync(text, options, cancellationToken);
            var response = await _httpClient.SendAsync(requestMessage, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new AIException($"Groq Cloud API request failed: {response.StatusCode} - {errorContent}");
            }

            return await ProcessResponseAsync(response, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Groq Cloud recipe parsing failed");
            throw new AIException($"Groq Cloud parsing failed: {ex.Message}", ex);
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
                    ErrorMessage = "Groq Cloud client not initialized - check API key configuration"
                };
            }

            var startTime = DateTime.UtcNow;

            var requestBody = new
            {
                model = _config.Model,
                messages = new[] { new { role = "user", content = "Hi" } },
                max_tokens = 5,
                temperature = 0.0f,
                stream = false
            };

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

            var response = await _httpClient.SendAsync(httpRequest, CancellationToken.None);
            var responseTime = DateTime.UtcNow - startTime;

            return new AIProviderHealth
            {
                IsHealthy = response.IsSuccessStatusCode,
                Status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"HTTP {response.StatusCode}"
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

    public override Task<decimal> GetCostEstimateAsync(string text, RecipeParsingOptions options)
    {
        var inputTokens = EstimateTokenCount(text);
        var outputTokens = options.MaxTokens ?? _config.MaxTokens;

        var cost = CalculateCost(new ProviderUsage
        {
            InputTokens = inputTokens,
            OutputTokens = outputTokens
        });

        return Task.FromResult(cost);
    }

    #endregion

    #region Protected Overrides

    protected override bool CheckAvailability()
    {
        return _httpClient != null && !string.IsNullOrEmpty(_config.ApiKey);
    }

    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);

        var requestBody = new
        {
            model = _config.Model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            max_tokens = options.MaxTokens ?? _config.MaxTokens,
            temperature = options.Temperature ?? _config.Temperature,
            stream = false
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/chat/completions")
        {
            Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
        };
        httpRequest.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

        return httpRequest;
    }

    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var groqResponse = JsonSerializer.Deserialize<GroqCloudResponse>(content, _jsonOptions);

        if (groqResponse?.Choices == null || groqResponse.Choices.Length == 0)
        {
            throw new AIException("Invalid response from Groq Cloud");
        }

        var responseText = groqResponse.Choices[0].Message.Content;
        var usage = groqResponse.Usage;

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

    #endregion
}

#region Groq Cloud API Models

/// <summary>
/// Groq Cloud API response (OpenAI-compatible format)
/// </summary>
public class GroqCloudResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public GroqCloudChoice[] Choices { get; set; } = Array.Empty<GroqCloudChoice>();
    public GroqCloudUsage Usage { get; set; } = new();
}

public class GroqCloudChoice
{
    public int Index { get; set; }
    public GroqCloudMessage Message { get; set; } = new();
    public string FinishReason { get; set; } = string.Empty;
}

public class GroqCloudMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class GroqCloudUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

#endregion
