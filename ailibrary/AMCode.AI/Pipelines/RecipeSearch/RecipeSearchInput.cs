namespace AMCode.AI.Pipelines.RecipeSearch;

/// <summary>
/// Shared input for all recipe search providers.
/// Maps from the user's Get Creative form data.
/// </summary>
public class RecipeSearchInput
{
    public List<string> Ingredients { get; set; } = new();
    public string? Cuisine { get; set; }
    public List<string> MealTypes { get; set; } = new();
    public decimal? MaxBudgetPerServing { get; set; }
    public List<string> DietaryPreferences { get; set; } = new();
    public string? Allergies { get; set; }
    public string? CookingTimeRange { get; set; }
    public string? Difficulty { get; set; }
    public int Servings { get; set; } = 4;
    public string? AdditionalNotes { get; set; }
}
