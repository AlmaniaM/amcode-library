using AMCode.AI.Models;

namespace AMCode.AI.Pipelines.RecipeSearch;

/// <summary>
/// Interface for a recipe search provider.
/// Each source (local DB, Spoonacular, FatSecret, RecipeNLG, AI generation)
/// implements this interface to participate in hybrid search aggregation.
/// </summary>
public interface IRecipeSearchProvider
{
    /// <summary>
    /// Unique name of this provider (e.g., "LocalDb", "Spoonacular", "AiGeneration")
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Priority for result ordering. Lower = higher priority.
    /// LocalDb=0, RecipeNLG=1, Spoonacular=2, FatSecret=3, AiGeneration=10
    /// </summary>
    int Priority { get; }

    /// <summary>
    /// Whether this provider is currently available (not rate-limited, configured, etc.)
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// Search for recipes matching the given input.
    /// </summary>
    /// <param name="input">Search criteria (ingredients, cuisine, dietary, etc.)</param>
    /// <param name="maxResults">Maximum number of results to return</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result containing provider search results or an error</returns>
    Task<Result<ProviderSearchResult>> SearchAsync(
        RecipeSearchInput input,
        int maxResults,
        CancellationToken cancellationToken = default);
}
