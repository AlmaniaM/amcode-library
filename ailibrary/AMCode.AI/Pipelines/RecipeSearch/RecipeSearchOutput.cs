namespace AMCode.AI.Pipelines.RecipeSearch;

/// <summary>
/// A single recipe returned by any search provider, normalized to a common shape.
/// </summary>
public class SearchedRecipe
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SearchedIngredient> Ingredients { get; set; } = new();
    public List<string> Directions { get; set; } = new();
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public decimal EstimatedCostPerServing { get; set; }
    public string Cuisine { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public SearchedNutrition? Nutrition { get; set; }
    public List<string> AllergenWarnings { get; set; } = new();
    public List<string> SafetyNotes { get; set; } = new();
    public double Confidence { get; set; }
    public string? ImageUrl { get; set; }
    public string? SourceUrl { get; set; }

    /// <summary>
    /// Which provider found this recipe
    /// </summary>
    public RecipeSource Source { get; set; }

    /// <summary>
    /// External ID from the source API (e.g., Spoonacular recipe ID)
    /// </summary>
    public string? ExternalId { get; set; }

    /// <summary>
    /// Pre-computed hash for fast deduplication on DB ingest.
    /// SHA256(lowercase(title) + ":" + sorted(first 5 ingredient names))
    /// </summary>
    public string? SourceHash { get; set; }
}

public class SearchedIngredient
{
    public string Name { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string? Preparation { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SearchedNutrition
{
    public int? CaloriesPerServing { get; set; }
    public int? ProteinGrams { get; set; }
    public int? CarbsGrams { get; set; }
    public int? FatGrams { get; set; }
}

/// <summary>
/// Where a recipe came from
/// </summary>
public enum RecipeSource
{
    LocalDb = 0,
    RecipeNlg = 1,
    Spoonacular = 2,
    FatSecret = 3,
    AiGenerated = 4,
}

/// <summary>
/// Result from a single provider's search execution
/// </summary>
public class ProviderSearchResult
{
    public string ProviderName { get; set; } = string.Empty;
    public List<SearchedRecipe> Recipes { get; set; } = new();
    public TimeSpan SearchDuration { get; set; }
    public bool WasRateLimited { get; set; }
}

/// <summary>
/// Final aggregated output from the hybrid search system
/// </summary>
public class RecipeSearchOutput
{
    public List<SearchedRecipe> Recipes { get; set; } = new();
    public List<string> ProvidersSearched { get; set; } = new();
    public List<string> ProvidersSkipped { get; set; } = new();
    public int DuplicatesRemoved { get; set; }
    public TimeSpan TotalSearchDuration { get; set; }
}
