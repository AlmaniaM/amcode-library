using AMCode.AI.Models;
using System.Text.Json;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AMCode.AI.Providers;

/// <summary>
/// Generic base class for AI providers implementing common functionality
/// </summary>
public abstract class GenericAIProvider : IAIProvider
{
    protected readonly ILogger _logger;
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly JsonSerializerOptions _jsonOptions;

    public abstract string ProviderName { get; }
    public abstract bool RequiresInternet { get; }
    public abstract AIProviderCapabilities Capabilities { get; }

    public virtual bool IsAvailable => CheckAvailability();

    protected GenericAIProvider(
        ILogger logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            Converters = { new RecipeIngredientJsonConverter() }
        };
    }

    #region Abstract Methods (Must be implemented by derived classes)

    public abstract Task<ParsedRecipeResult> ParseTextAsync(string text, CancellationToken cancellationToken = default);
    public abstract Task<ParsedRecipeResult> ParseTextAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default);
    public abstract Task<AIProviderHealth> CheckHealthAsync();
    public abstract Task<decimal> GetCostEstimateAsync(string text, RecipeParsingOptions options);

    protected abstract bool CheckAvailability();
    protected abstract Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken);
    protected abstract Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken);
    protected abstract decimal CalculateCost(ProviderUsage usage);

    #endregion

    #region General Text Completion

    /// <summary>
    /// Send a prompt and get a text completion response
    /// </summary>
    public virtual async Task<AICompletionResult> CompleteAsync(string prompt, CancellationToken cancellationToken = default)
    {
        return await CompleteAsync(new AICompletionRequest { Prompt = prompt }, cancellationToken);
    }

    /// <summary>
    /// Send a completion request with full options.
    /// Default implementation uses chat API with a single user message.
    /// </summary>
    public virtual async Task<AICompletionResult> CompleteAsync(AICompletionRequest request, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.LogInformation("Completing prompt with {Provider}", ProviderName);

            // Default implementation: use chat with single message
            var chatRequest = new AIChatRequest
            {
                SystemInstruction = request.SystemMessage,
                Messages = new List<AIChatMessage> { AIChatMessage.User(request.Prompt) },
                MaxTokens = request.MaxTokens,
                Temperature = request.Temperature,
                TopP = request.TopP,
                StopSequences = request.StopSequences,
                Timeout = request.Timeout,
                Metadata = request.Metadata
            };

            var chatResult = await ChatAsync(chatRequest, cancellationToken);
            stopwatch.Stop();

            if (!chatResult.Success)
            {
                return AICompletionResult.Fail(chatResult.ErrorMessage ?? "Unknown error", ProviderName);
            }

            return new AICompletionResult
            {
                Content = chatResult.Message.Content,
                Success = true,
                Provider = ProviderName,
                FinishReason = chatResult.FinishReason,
                Usage = chatResult.Usage,
                Duration = stopwatch.Elapsed,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Completion failed for {Provider}", ProviderName);
            return AICompletionResult.Fail(ex.Message, ProviderName);
        }
    }

    /// <summary>
    /// Stream a text completion response. Default implementation falls back to non-streaming.
    /// </summary>
    public virtual async IAsyncEnumerable<AIStreamChunk> CompleteStreamingAsync(
        AICompletionRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("{Provider} does not support streaming - falling back to non-streaming", ProviderName);

        var result = await CompleteAsync(request, cancellationToken);

        yield return new AIStreamChunk
        {
            Content = result.Content,
            IsComplete = true,
            FinishReason = result.FinishReason,
            Index = 0
        };
    }

    #endregion

    #region Structured JSON Responses

    /// <summary>
    /// Send a prompt and get a structured JSON response
    /// </summary>
    public virtual async Task<AIJsonResult<T>> CompleteJsonAsync<T>(string prompt, CancellationToken cancellationToken = default) where T : class
    {
        return await CompleteJsonAsync<T>(new AIJsonRequest { Prompt = prompt }, cancellationToken);
    }

    /// <summary>
    /// Send a JSON request with full options.
    /// Default implementation adds JSON instructions to the prompt.
    /// </summary>
    public virtual async Task<AIJsonResult<T>> CompleteJsonAsync<T>(AIJsonRequest request, CancellationToken cancellationToken = default) where T : class
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            _logger.LogInformation("Completing JSON request with {Provider}", ProviderName);

            // Build a prompt that requests JSON output
            var jsonPrompt = BuildJsonPrompt(request.Prompt, request.JsonSchema, request.ExampleResponse);

            var completionRequest = new AICompletionRequest
            {
                Prompt = jsonPrompt,
                SystemMessage = request.SystemMessage ?? "You are a helpful assistant that responds only with valid JSON. Do not include any text outside the JSON object.",
                MaxTokens = request.MaxTokens,
                Temperature = request.Temperature ?? 0.1f, // Lower temperature for more consistent JSON
                TopP = request.TopP,
                StopSequences = request.StopSequences,
                Timeout = request.Timeout,
                Metadata = request.Metadata
            };

            var completionResult = await CompleteAsync(completionRequest, cancellationToken);
            stopwatch.Stop();

            if (!completionResult.Success)
            {
                return AIJsonResult<T>.Fail(completionResult.ErrorMessage ?? "Unknown error", ProviderName);
            }

            // Parse the JSON response
            var jsonContent = ExtractJsonFromResponse(completionResult.Content);

            try
            {
                var data = JsonSerializer.Deserialize<T>(jsonContent, _jsonOptions);
                if (data == null)
                {
                    return AIJsonResult<T>.Fail("Deserialized result was null", ProviderName, jsonContent);
                }

                return new AIJsonResult<T>
                {
                    Data = data,
                    Success = true,
                    Provider = ProviderName,
                    RawJson = jsonContent,
                    Usage = completionResult.Usage,
                    Duration = stopwatch.Elapsed,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to parse JSON response from {Provider}", ProviderName);
                return AIJsonResult<T>.Fail($"JSON parsing failed: {jsonEx.Message}", ProviderName, jsonContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "JSON completion failed for {Provider}", ProviderName);
            return AIJsonResult<T>.Fail(ex.Message, ProviderName);
        }
    }

    #endregion

    #region Chat Conversation

    /// <summary>
    /// Send a single message and get a response
    /// </summary>
    public virtual async Task<AIChatResult> ChatAsync(string message, string? systemMessage = null, CancellationToken cancellationToken = default)
    {
        var request = AIChatRequest.Create(message, systemMessage);
        return await ChatAsync(request, cancellationToken);
    }

    /// <summary>
    /// Continue a multi-turn conversation.
    /// Default implementation throws NotSupportedException - providers should override.
    /// </summary>
    public virtual Task<AIChatResult> ChatAsync(AIChatRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("{Provider} does not have a native chat implementation", ProviderName);
        return Task.FromResult(AIChatResult.Fail($"{ProviderName} does not support chat - override ChatAsync in derived class", ProviderName));
    }

    /// <summary>
    /// Stream a chat response. Default implementation falls back to non-streaming.
    /// </summary>
    public virtual async IAsyncEnumerable<AIStreamChunk> ChatStreamingAsync(
        AIChatRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("{Provider} does not support streaming chat - falling back to non-streaming", ProviderName);

        var result = await ChatAsync(request, cancellationToken);

        yield return new AIStreamChunk
        {
            Content = result.Message.Content,
            IsComplete = true,
            FinishReason = result.FinishReason,
            Index = 0
        };
    }

    #endregion

    #region Embeddings

    /// <summary>
    /// Generate embeddings for a single text
    /// </summary>
    public virtual async Task<AIEmbeddingResult> GetEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        return await GetEmbeddingsAsync(new AIEmbeddingRequest { Texts = new List<string> { text } }, cancellationToken);
    }

    /// <summary>
    /// Generate embeddings for multiple texts.
    /// Default implementation returns not supported - providers should override if they support embeddings.
    /// </summary>
    public virtual Task<AIEmbeddingResult> GetEmbeddingsAsync(AIEmbeddingRequest request, CancellationToken cancellationToken = default)
    {
        if (!Capabilities.SupportsEmbeddings)
        {
            return Task.FromResult(AIEmbeddingResult.Fail($"{ProviderName} does not support embeddings", ProviderName));
        }

        _logger.LogWarning("{Provider} embeddings not implemented - override GetEmbeddingsAsync", ProviderName);
        return Task.FromResult(AIEmbeddingResult.Fail($"{ProviderName} embeddings not implemented", ProviderName));
    }

    #endregion

    #region Vision/Image Analysis

    /// <summary>
    /// Analyze an image with a prompt
    /// </summary>
    public virtual async Task<AIVisionResult> AnalyzeImageAsync(string prompt, string imageUrl, CancellationToken cancellationToken = default)
    {
        return await AnalyzeImageAsync(new AIVisionRequest
        {
            Prompt = prompt,
            ImageUrls = new List<string> { imageUrl }
        }, cancellationToken);
    }

    /// <summary>
    /// Analyze images with full options.
    /// Default implementation returns not supported - providers should override if they support vision.
    /// </summary>
    public virtual Task<AIVisionResult> AnalyzeImageAsync(AIVisionRequest request, CancellationToken cancellationToken = default)
    {
        if (!Capabilities.SupportsVision)
        {
            return Task.FromResult(AIVisionResult.Fail($"{ProviderName} does not support vision/image analysis", ProviderName));
        }

        _logger.LogWarning("{Provider} vision not implemented - override AnalyzeImageAsync", ProviderName);
        return Task.FromResult(AIVisionResult.Fail($"{ProviderName} vision not implemented", ProviderName));
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Get HTTP client for this provider
    /// </summary>
    protected virtual async Task<HttpClient> GetHttpClientAsync()
    {
        var client = _httpClientFactory.CreateClient(ProviderName);
        await ConfigureHttpClientAsync(client);
        return client;
    }

    /// <summary>
    /// Configure HTTP client with provider-specific settings
    /// </summary>
    protected virtual Task ConfigureHttpClientAsync(HttpClient client)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deserialize JSON response
    /// </summary>
    protected virtual async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions) ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    /// <summary>
    /// Build a standard recipe parsing prompt
    /// </summary>
    protected virtual string BuildPrompt(string text, RecipeParsingOptions options)
    {
        return $@"
Please parse the following recipe text and extract structured information. Return the result as JSON in the following format:

{{
  ""title"": ""Recipe Title"",
  ""description"": ""Optional description"",
  ""ingredients"": [
    {{
      ""name"": ""ingredient name"",
      ""amount"": ""amount"",
      ""unit"": ""unit"",
      ""text"": ""full ingredient text"",
      ""notes"": ""optional notes""
    }}
  ],
  ""instructions"": [""step 1"", ""step 2""],
  ""prepTimeMinutes"": 15,
  ""cookTimeMinutes"": 30,
  ""totalTimeMinutes"": 45,
  ""servings"": 4,
  ""category"": ""Main Course"",
  ""tags"": [""tag1"", ""tag2""],
  ""difficulty"": 3,
  ""confidence"": 0.95,
  ""notes"": ""Additional notes or tips""
}}

Rules:
- Extract ingredients as structured objects with name, amount, unit, and full text
- Extract instructions as step-by-step strings
- Convert times to minutes (e.g., ""30 minutes"" becomes 30)
- Estimate confidence based on text clarity (0.0 to 1.0)
- If information is missing, use null or empty string
- Be conservative with confidence scores
- Extract category and tags when possible
- Estimate difficulty on a 1-5 scale

Recipe text:
{text}

JSON response:";
    }

    /// <summary>
    /// Build a prompt that requests JSON output
    /// </summary>
    protected virtual string BuildJsonPrompt(string prompt, string? schema, string? example)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine(prompt);
        sb.AppendLine();
        sb.AppendLine("Respond with valid JSON only. Do not include any explanations, markdown, or text outside the JSON.");

        if (!string.IsNullOrEmpty(schema))
        {
            sb.AppendLine();
            sb.AppendLine("Expected JSON schema:");
            sb.AppendLine(schema);
        }

        if (!string.IsNullOrEmpty(example))
        {
            sb.AppendLine();
            sb.AppendLine("Example response format:");
            sb.AppendLine(example);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Extract JSON from a response that might contain extra text
    /// </summary>
    protected virtual string ExtractJsonFromResponse(string response)
    {
        if (string.IsNullOrWhiteSpace(response))
            return "{}";

        var trimmed = response.Trim();

        // If it starts with { or [, assume it's pure JSON
        if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
        {
            // Find the matching closing bracket
            int depth = 0;
            char openBracket = trimmed[0];
            char closeBracket = openBracket == '{' ? '}' : ']';

            for (int i = 0; i < trimmed.Length; i++)
            {
                if (trimmed[i] == openBracket) depth++;
                else if (trimmed[i] == closeBracket) depth--;

                if (depth == 0)
                {
                    return trimmed.Substring(0, i + 1);
                }
            }
            return trimmed;
        }

        // Try to find JSON in markdown code blocks
        var jsonMatch = System.Text.RegularExpressions.Regex.Match(response, @"```(?:json)?\s*([\s\S]*?)```");
        if (jsonMatch.Success)
        {
            return jsonMatch.Groups[1].Value.Trim();
        }

        // Try to find JSON object or array anywhere in the response
        var objectMatch = System.Text.RegularExpressions.Regex.Match(response, @"\{[\s\S]*\}");
        if (objectMatch.Success)
        {
            return objectMatch.Value;
        }

        var arrayMatch = System.Text.RegularExpressions.Regex.Match(response, @"\[[\s\S]*\]");
        if (arrayMatch.Success)
        {
            return arrayMatch.Value;
        }

        return response;
    }

    /// <summary>
    /// Estimate token count for text
    /// </summary>
    protected virtual int EstimateTokenCount(string text)
    {
        // Simple estimation: ~4 characters per token
        return Math.Max(1, text.Length / 4);
    }

    /// <summary>
    /// Parse JSON response into ParsedRecipeResult
    /// </summary>
    protected virtual ParsedRecipeResult ParseJsonResponse(string jsonResponse, string providerName, decimal cost, int tokensUsed)
    {
        try
        {
            Debug.WriteLine($"Extracted LLM text for parsed recipe: {jsonResponse}");
            var recipe = JsonSerializer.Deserialize<ParsedRecipe>(jsonResponse, _jsonOptions);
            if (recipe == null)
            {
                throw new InvalidOperationException("Failed to deserialize recipe from JSON response");
            }

            return new ParsedRecipeResult
            {
                Recipes = new[] { recipe },
                Confidence = recipe.Confidence,
                Source = providerName,
                ProcessingTime = DateTime.UtcNow,
                Cost = cost,
                TokensUsed = tokensUsed,
                RawResponse = jsonResponse
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to parse JSON response from {Provider}", providerName);
            throw new InvalidOperationException($"Failed to parse JSON response from {providerName}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Create usage stats from provider-specific usage info
    /// </summary>
    protected virtual AIUsageStats CreateUsageStats(int inputTokens, int outputTokens, decimal? costPerInput = null, decimal? costPerOutput = null)
    {
        var usage = new AIUsageStats
        {
            InputTokens = inputTokens,
            OutputTokens = outputTokens,
            CostPerInputToken = costPerInput,
            CostPerOutputToken = costPerOutput
        };

        if (costPerInput.HasValue && costPerOutput.HasValue)
        {
            usage.EstimatedCost = (inputTokens * costPerInput.Value) + (outputTokens * costPerOutput.Value);
        }

        return usage;
    }

    #endregion
}

/// <summary>
/// Provider usage information for cost calculation
/// </summary>
public class ProviderUsage
{
    /// <summary>
    /// Number of input tokens
    /// </summary>
    public int InputTokens { get; set; }

    /// <summary>
    /// Number of output tokens
    /// </summary>
    public int OutputTokens { get; set; }

    /// <summary>
    /// Total tokens used
    /// </summary>
    public int TotalTokens => InputTokens + OutputTokens;

    /// <summary>
    /// Request timestamp
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
