using AMCode.AI.Models;
using AMCode.AI.Configurations;
using AMCode.AI.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace AMCode.AI.Providers;

/// <summary>
/// Azure OpenAI Service provider using the official Azure.AI.OpenAI SDK
/// Based on: https://github.com/openai/openai-dotnet
/// </summary>
public class AzureOpenAISdkProvider : GenericAIProvider
{
    private readonly AzureOpenAIClient? _azureClient;
    private readonly ChatClient? _chatClient;
    private readonly AzureOpenAIConfiguration _config;
    private readonly PromptBuilderService _promptBuilder;

    public override string ProviderName => "Azure OpenAI SDK";
    public override bool RequiresInternet => true;
    public override bool IsAvailable => _azureClient != null && _chatClient != null;

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
        AverageResponseTime = _config.AverageResponseTime,
        SupportsCustomModels = true,
        SupportsFineTuning = true,
        SupportsEmbeddings = true,
        SupportsModeration = false,
        MaxRequestsPerMinute = 500,
        MaxRequestsPerDay = 10000
    };

    public AzureOpenAISdkProvider(
        ILogger<AzureOpenAISdkProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<AzureOpenAIConfiguration> config,
        PromptBuilderService promptBuilder)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
        _promptBuilder = promptBuilder ?? throw new ArgumentNullException(nameof(promptBuilder));

        if (string.IsNullOrEmpty(_config.ApiKey) || string.IsNullOrEmpty(_config.Endpoint) || string.IsNullOrEmpty(_config.DeploymentName))
        {
            _logger.LogWarning("Azure OpenAI SDK: API key, endpoint, or deployment name not configured");
            _azureClient = null;
            _chatClient = null;
        }
        else
        {
            try
            {
                // Create Azure OpenAI client using official SDK pattern
                // See: https://github.com/openai/openai-dotnet#how-to-work-with-azure-openai
                _azureClient = new AzureOpenAIClient(
                    new Uri(_config.Endpoint),
                    new AzureKeyCredential(_config.ApiKey));

                // Get chat client for the specific deployment
                _chatClient = _azureClient.GetChatClient(_config.DeploymentName);

                _logger.LogInformation("Azure OpenAI SDK client initialized successfully for deployment: {Deployment}", _config.DeploymentName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Azure OpenAI SDK client");
                _azureClient = null;
                _chatClient = null;
            }
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
            if (_chatClient == null)
            {
                throw new InvalidOperationException("Azure OpenAI SDK client not initialized - check API key, endpoint, and deployment name configuration");
            }

            _logger.LogInformation("Parsing recipe text with Azure OpenAI SDK");

            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);

            // Build chat messages using SDK types
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are an expert recipe parser. Extract structured recipe information from text. Always respond with valid JSON only."),
                new UserChatMessage(prompt)
            };

            // Create chat completion options
            var completionOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = options.MaxTokens ?? _config.MaxTokens,
                Temperature = options.Temperature ?? _config.Temperature,
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            };

            // Execute chat completion using SDK
            var completion = await _chatClient.CompleteChatAsync(messages, completionOptions, cancellationToken);

            // Extract response content
            var responseText = completion.Value.Content[0].Text;
            var usage = completion.Value.Usage;

            // Calculate cost based on token usage
            var cost = CalculateCost(new ProviderUsage
            {
                InputTokens = usage.InputTokenCount,
                OutputTokens = usage.OutputTokenCount
            });

            var result = ParseJsonResponse(responseText, ProviderName, cost, usage.TotalTokenCount);

            _logger.LogInformation("Azure OpenAI SDK parsing completed successfully. Cost: ${Cost:F6}, Tokens: {Tokens}",
                result.Cost, result.TokensUsed);

            return result;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Azure OpenAI SDK request failed with status: {Status}", ex.Status);
            throw new AIException($"Azure OpenAI SDK request failed: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex is not AIException)
        {
            _logger.LogError(ex, "Azure OpenAI SDK parsing failed");
            throw new AIException($"Azure OpenAI SDK parsing failed: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Parse recipe text with streaming response
    /// </summary>
    /// <param name="text">Recipe text to parse</param>
    /// <param name="options">Parsing options</param>
    /// <param name="onChunkReceived">Callback for each received chunk</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed recipe result</returns>
    public async Task<ParsedRecipeResult> ParseTextStreamingAsync(
        string text,
        RecipeParsingOptions options,
        Action<string>? onChunkReceived = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_chatClient == null)
            {
                throw new InvalidOperationException("Azure OpenAI SDK client not initialized");
            }

            _logger.LogInformation("Parsing recipe text with Azure OpenAI SDK (streaming)");

            var prompt = _promptBuilder.BuildRecipeParsingPrompt(text, options);

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are an expert recipe parser. Extract structured recipe information from text. Always respond with valid JSON only."),
                new UserChatMessage(prompt)
            };

            var completionOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = options.MaxTokens ?? _config.MaxTokens,
                Temperature = options.Temperature ?? _config.Temperature,
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            };

            // Use streaming API
            var responseBuilder = new System.Text.StringBuilder();
            int totalInputTokens = EstimateTokenCount(prompt);
            int totalOutputTokens = 0;

            await foreach (var update in _chatClient.CompleteChatStreamingAsync(messages, completionOptions, cancellationToken))
            {
                foreach (var contentPart in update.ContentUpdate)
                {
                    var chunk = contentPart.Text;
                    if (!string.IsNullOrEmpty(chunk))
                    {
                        responseBuilder.Append(chunk);
                        onChunkReceived?.Invoke(chunk);
                        totalOutputTokens += EstimateTokenCount(chunk);
                    }
                }
            }

            var responseText = responseBuilder.ToString();

            var cost = CalculateCost(new ProviderUsage
            {
                InputTokens = totalInputTokens,
                OutputTokens = totalOutputTokens
            });

            var result = ParseJsonResponse(responseText, ProviderName, cost, totalInputTokens + totalOutputTokens);

            _logger.LogInformation("Azure OpenAI SDK streaming parsing completed. Cost: ${Cost:F6}, Tokens: {Tokens}",
                result.Cost, result.TokensUsed);

            return result;
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Azure OpenAI SDK streaming request failed with status: {Status}", ex.Status);
            throw new AIException($"Azure OpenAI SDK streaming request failed: {ex.Message}", ex);
        }
        catch (Exception ex) when (ex is not AIException)
        {
            _logger.LogError(ex, "Azure OpenAI SDK streaming parsing failed");
            throw new AIException($"Azure OpenAI SDK streaming parsing failed: {ex.Message}", ex);
        }
    }

    public override async Task<AIProviderHealth> CheckHealthAsync()
    {
        try
        {
            if (_chatClient == null)
            {
                return new AIProviderHealth
                {
                    IsHealthy = false,
                    Status = "Client not initialized",
                    LastChecked = DateTime.UtcNow,
                    ErrorMessage = "Azure OpenAI SDK client not initialized - check API key, endpoint, and deployment name configuration"
                };
            }

            var startTime = DateTime.UtcNow;

            // Simple health check with a minimal request
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("Respond with 'ok'."),
                new UserChatMessage("Health check")
            };

            var completionOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = 10,
                Temperature = 0
            };

            var completion = await _chatClient.CompleteChatAsync(messages, completionOptions, CancellationToken.None);
            var responseTime = DateTime.UtcNow - startTime;

            var isHealthy = completion.Value.Content.Count > 0 && !string.IsNullOrEmpty(completion.Value.Content[0].Text);

            return new AIProviderHealth
            {
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = isHealthy ? string.Empty : "No response content received",
                RequestsPerMinute = 0,
                QuotaRemaining = int.MaxValue
            };
        }
        catch (RequestFailedException ex)
        {
            return new AIProviderHealth
            {
                IsHealthy = false,
                Status = "Unhealthy",
                ResponseTime = TimeSpan.Zero,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = $"Request failed with status {ex.Status}: {ex.Message}"
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
            var inputTokens = EstimateTokenCount(text);
            var outputTokens = options.MaxTokens ?? _config.MaxTokens;

            return CalculateCost(new ProviderUsage
            {
                InputTokens = inputTokens,
                OutputTokens = outputTokens
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to calculate cost estimate for Azure OpenAI SDK");
            return 0m;
        }
    }

    #region General AI Methods

    /// <summary>
    /// Send a chat request using the native Azure OpenAI SDK
    /// </summary>
    public override async Task<Models.AIChatResult> ChatAsync(Models.AIChatRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            if (_chatClient == null)
            {
                return Models.AIChatResult.Fail("Azure OpenAI SDK client not initialized", ProviderName);
            }

            _logger.LogInformation("Processing chat request with Azure OpenAI SDK");

            // Get messages including system instruction
            var allMessages = request.GetMessagesWithSystemInstruction();
            
            // Convert our chat messages to SDK format
            var messages = new List<ChatMessage>();
            foreach (var msg in allMessages)
            {
                ChatMessage chatMessage = msg.Role switch
                {
                    Models.AIChatRole.System => new SystemChatMessage(msg.Content),
                    Models.AIChatRole.User => new UserChatMessage(msg.Content),
                    Models.AIChatRole.Assistant => new AssistantChatMessage(msg.Content),
                    _ => new UserChatMessage(msg.Content)
                };
                messages.Add(chatMessage);
            }

            var completionOptions = new ChatCompletionOptions
            {
                MaxOutputTokenCount = request.MaxTokens ?? _config.MaxTokens,
                Temperature = request.Temperature ?? _config.Temperature
            };

            var completion = await _chatClient.CompleteChatAsync(messages, completionOptions, cancellationToken);
            stopwatch.Stop();

            var responseText = completion.Value.Content[0].Text;
            var usage = completion.Value.Usage;

            var cost = CalculateCost(new ProviderUsage
            {
                InputTokens = usage.InputTokenCount,
                OutputTokens = usage.OutputTokenCount
            });

            return new Models.AIChatResult
            {
                Message = Models.AIChatMessage.Assistant(responseText),
                Success = true,
                Provider = ProviderName,
                FinishReason = completion.Value.FinishReason.ToString(),
                Usage = CreateUsageStats(usage.InputTokenCount, usage.OutputTokenCount, _config.CostPerInputToken, _config.CostPerOutputToken),
                Duration = stopwatch.Elapsed,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(ex, "Azure OpenAI SDK chat request failed with status: {Status}", ex.Status);
            return Models.AIChatResult.Fail($"Request failed: {ex.Message}", ProviderName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Azure OpenAI SDK chat failed");
            return Models.AIChatResult.Fail(ex.Message, ProviderName);
        }
    }

    /// <summary>
    /// Stream chat responses using the native Azure OpenAI SDK
    /// </summary>
    public override async IAsyncEnumerable<Models.AIStreamChunk> ChatStreamingAsync(
        Models.AIChatRequest request,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (_chatClient == null)
        {
            yield return new Models.AIStreamChunk
            {
                Content = "Azure OpenAI SDK client not initialized",
                IsComplete = true,
                FinishReason = "error"
            };
            yield break;
        }

        _logger.LogInformation("Processing streaming chat request with Azure OpenAI SDK");

        // Get messages including system instruction
        var allMessages = request.GetMessagesWithSystemInstruction();
        
        // Convert our chat messages to SDK format
        var messages = new List<ChatMessage>();
        foreach (var msg in allMessages)
        {
            ChatMessage chatMessage = msg.Role switch
            {
                Models.AIChatRole.System => new SystemChatMessage(msg.Content),
                Models.AIChatRole.User => new UserChatMessage(msg.Content),
                Models.AIChatRole.Assistant => new AssistantChatMessage(msg.Content),
                _ => new UserChatMessage(msg.Content)
            };
            messages.Add(chatMessage);
        }

        var completionOptions = new ChatCompletionOptions
        {
            MaxOutputTokenCount = request.MaxTokens ?? _config.MaxTokens,
            Temperature = request.Temperature ?? _config.Temperature
        };

        int chunkIndex = 0;
        await foreach (var update in _chatClient.CompleteChatStreamingAsync(messages, completionOptions, cancellationToken))
        {
            foreach (var contentPart in update.ContentUpdate)
            {
                if (!string.IsNullOrEmpty(contentPart.Text))
                {
                    yield return new Models.AIStreamChunk
                    {
                        Content = contentPart.Text,
                        IsComplete = false,
                        Index = chunkIndex++
                    };
                }
            }

            // Check for finish reason
            if (update.FinishReason != null)
            {
                yield return new Models.AIStreamChunk
                {
                    Content = string.Empty,
                    IsComplete = true,
                    FinishReason = update.FinishReason.ToString(),
                    Index = chunkIndex
                };
            }
        }
    }

    #endregion

    protected override bool CheckAvailability()
    {
        return _azureClient != null &&
               _chatClient != null &&
               !string.IsNullOrEmpty(_config.ApiKey) &&
               !string.IsNullOrEmpty(_config.Endpoint) &&
               !string.IsNullOrEmpty(_config.DeploymentName);
    }

    protected override async Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken)
    {
        // This method is not used when using the SDK directly
        // Kept for interface compatibility
        throw new NotSupportedException("Azure OpenAI SDK provider uses SDK directly instead of HTTP requests. Use ParseTextAsync instead.");
    }

    protected override async Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        // This method is not used when using the SDK directly
        // Kept for interface compatibility
        throw new NotSupportedException("Azure OpenAI SDK provider uses SDK directly instead of HTTP requests. Use ParseTextAsync instead.");
    }

    protected override decimal CalculateCost(ProviderUsage usage)
    {
        var inputCost = usage.InputTokens * _config.CostPerInputToken;
        var outputCost = usage.OutputTokens * _config.CostPerOutputToken;
        return inputCost + outputCost;
    }
}

