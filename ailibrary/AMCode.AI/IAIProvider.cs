using AMCode.AI.Models;

namespace AMCode.AI;

/// <summary>
/// AI provider interface for different AI services
/// </summary>
public interface IAIProvider
{
    /// <summary>
    /// Parse text using default options
    /// </summary>
    /// <param name="text">The text to parse</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed recipe result</returns>
    Task<ParsedRecipeResult> ParseTextAsync(string text, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Parse text with custom options
    /// </summary>
    /// <param name="text">The text to parse</param>
    /// <param name="options">Parsing options and preferences</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Parsed recipe result</returns>
    Task<ParsedRecipeResult> ParseTextAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Provider name for identification
    /// </summary>
    string ProviderName { get; }
    
    /// <summary>
    /// Whether this provider requires internet connection
    /// </summary>
    bool RequiresInternet { get; }
    
    /// <summary>
    /// Whether this provider is currently available
    /// </summary>
    bool IsAvailable { get; }
    
    /// <summary>
    /// Provider capabilities and features
    /// </summary>
    AIProviderCapabilities Capabilities { get; }
    
    /// <summary>
    /// Check provider health status
    /// </summary>
    /// <returns>Provider health information</returns>
    Task<AIProviderHealth> CheckHealthAsync();
    
    /// <summary>
    /// Get cost estimate for a request
    /// </summary>
    /// <param name="text">Text to be processed</param>
    /// <param name="options">Parsing options</param>
    /// <returns>Estimated cost</returns>
    Task<decimal> GetCostEstimateAsync(string text, RecipeParsingOptions options);
}
