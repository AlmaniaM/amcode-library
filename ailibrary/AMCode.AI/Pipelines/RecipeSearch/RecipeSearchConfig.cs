namespace AMCode.AI.Pipelines.RecipeSearch;

/// <summary>
/// Configuration for hybrid recipe search.
/// Bound from appsettings.json AI:RecipeSearch section.
/// </summary>
public class RecipeSearchConfig
{
    /// <summary>
    /// Maximum total results to return to the caller
    /// </summary>
    public int MaxResults { get; set; } = 5;

    /// <summary>
    /// Minimum number of AI-generated recipes in every result set.
    /// AI always contributes at least this many recipes.
    /// </summary>
    public int MinAiRecipes { get; set; } = 1;

    /// <summary>
    /// Maximum number of AI-generated recipes per search
    /// </summary>
    public int MaxAiRecipes { get; set; } = 3;

    /// <summary>
    /// Timeout for each individual provider search
    /// </summary>
    public TimeSpan ProviderTimeout { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Fuzzy title similarity threshold for deduplication (0.0–1.0).
    /// Recipes with normalized Levenshtein similarity above this are considered duplicates.
    /// </summary>
    public double DeduplicationTitleThreshold { get; set; } = 0.85;

    /// <summary>
    /// Minimum ingredient overlap percentage for deduplication (0.0–1.0).
    /// Used alongside title similarity to confirm a duplicate.
    /// </summary>
    public double DeduplicationIngredientOverlap { get; set; } = 0.60;

    /// <summary>
    /// Whether to cache external API results into local DB
    /// </summary>
    public bool CacheExternalResults { get; set; } = true;

    /// <summary>
    /// Per-provider enable/disable toggles
    /// </summary>
    public ProviderToggles Providers { get; set; } = new();
}

public class ProviderToggles
{
    public bool LocalDb { get; set; } = true;
    public bool RecipeNlg { get; set; } = true;
    public bool Spoonacular { get; set; } = true;
    public bool FatSecret { get; set; } = true;
    public bool AiGeneration { get; set; } = true;
}
