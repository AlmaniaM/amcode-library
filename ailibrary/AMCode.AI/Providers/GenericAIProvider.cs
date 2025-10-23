using AMCode.AI.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Providers;

/// <summary>
/// Generic base class for AI providers
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
            WriteIndented = false
        };
    }
    
    public abstract Task<ParsedRecipeResult> ParseTextAsync(string text, CancellationToken cancellationToken = default);
    public abstract Task<ParsedRecipeResult> ParseTextAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default);
    public abstract Task<AIProviderHealth> CheckHealthAsync();
    public abstract Task<decimal> GetCostEstimateAsync(string text, RecipeParsingOptions options);
    
    protected abstract bool CheckAvailability();
    protected abstract Task<HttpRequestMessage> CreateRequestAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken);
    protected abstract Task<ParsedRecipeResult> ProcessResponseAsync(HttpResponseMessage response, CancellationToken cancellationToken);
    protected abstract decimal CalculateCost(ProviderUsage usage);
    
    /// <summary>
    /// Get HTTP client for this provider
    /// </summary>
    /// <returns>Configured HTTP client</returns>
    protected virtual async Task<HttpClient> GetHttpClientAsync()
    {
        var client = _httpClientFactory.CreateClient(ProviderName);
        await ConfigureHttpClientAsync(client);
        return client;
    }
    
    /// <summary>
    /// Configure HTTP client with provider-specific settings
    /// </summary>
    /// <param name="client">HTTP client to configure</param>
    /// <returns>Task</returns>
    protected virtual Task ConfigureHttpClientAsync(HttpClient client)
    {
        // Override in derived classes to configure headers, timeouts, etc.
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Deserialize JSON response
    /// </summary>
    /// <typeparam name="T">Type to deserialize to</typeparam>
    /// <param name="response">HTTP response</param>
    /// <returns>Deserialized object</returns>
    protected virtual async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions) ?? throw new InvalidOperationException("Failed to deserialize response");
    }
    
    /// <summary>
    /// Build a standard recipe parsing prompt
    /// </summary>
    /// <param name="text">Recipe text</param>
    /// <param name="options">Parsing options</param>
    /// <returns>Formatted prompt</returns>
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
    /// Estimate token count for text
    /// </summary>
    /// <param name="text">Text to estimate</param>
    /// <returns>Estimated token count</returns>
    protected virtual int EstimateTokenCount(string text)
    {
        // Simple estimation: ~4 characters per token
        return Math.Max(1, text.Length / 4);
    }
    
    /// <summary>
    /// Parse JSON response into ParsedRecipeResult
    /// </summary>
    /// <param name="jsonResponse">JSON response string</param>
    /// <param name="providerName">Provider name</param>
    /// <param name="cost">Operation cost</param>
    /// <param name="tokensUsed">Tokens used</param>
    /// <returns>Parsed recipe result</returns>
    protected virtual ParsedRecipeResult ParseJsonResponse(string jsonResponse, string providerName, decimal cost, int tokensUsed)
    {
        try
        {
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
