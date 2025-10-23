using AMCode.AI.Models;

namespace AMCode.AI;

/// <summary>
/// Main recipe parser service interface for AI-powered recipe text parsing
/// </summary>
public interface IRecipeParserService
{
    /// <summary>
    /// Parse recipe text using default options
    /// </summary>
    /// <param name="text">The recipe text to parse</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing parsed recipe data or error</returns>
    Task<Result<ParsedRecipeResult>> ParseRecipeAsync(string text, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Parse recipe text with custom options
    /// </summary>
    /// <param name="text">The recipe text to parse</param>
    /// <param name="options">Parsing options and preferences</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing parsed recipe data or error</returns>
    Task<Result<ParsedRecipeResult>> ParseRecipeAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if any AI providers are available
    /// </summary>
    /// <returns>True if at least one provider is available</returns>
    Task<bool> IsAvailableAsync();
}
