using System.Text.Json.Serialization;

namespace AMCode.AI.Pipelines.RecipeGeneration;

/// <summary>
/// Input for the recipe generation pipeline.
/// Maps from user form data (ingredients, preferences, constraints).
/// </summary>
public class RecipeGenerationInput
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
    public int RecipeCount { get; set; } = 3;
}

/// <summary>
/// Output from the LLM — structured JSON containing generated recipes.
/// </summary>
public class RecipeGenerationOutput
{
    [JsonPropertyName("recipes")]
    public List<GeneratedRecipeItem> Recipes { get; set; } = new();
}

public class GeneratedRecipeItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("ingredients")]
    public List<GeneratedIngredient> Ingredients { get; set; } = new();

    [JsonPropertyName("directions")]
    public List<string> Directions { get; set; } = new();

    [JsonPropertyName("prepTimeMinutes")]
    public int PrepTimeMinutes { get; set; }

    [JsonPropertyName("cookTimeMinutes")]
    public int CookTimeMinutes { get; set; }

    [JsonPropertyName("servings")]
    public int Servings { get; set; }

    [JsonPropertyName("difficulty")]
    public string Difficulty { get; set; } = string.Empty;

    [JsonPropertyName("estimatedCostPerServing")]
    public decimal EstimatedCostPerServing { get; set; }

    [JsonPropertyName("cuisine")]
    public string Cuisine { get; set; } = string.Empty;

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

    [JsonPropertyName("nutrition")]
    public NutritionEstimate? Nutrition { get; set; }

    [JsonPropertyName("allergenWarnings")]
    public List<string> AllergenWarnings { get; set; } = new();

    [JsonPropertyName("safetyNotes")]
    public List<string> SafetyNotes { get; set; } = new();

    [JsonPropertyName("sourceInspiration")]
    public string? SourceInspiration { get; set; }

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}

public class GeneratedIngredient
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("amount")]
    public string Amount { get; set; } = string.Empty;

    [JsonPropertyName("unit")]
    public string Unit { get; set; } = string.Empty;

    [JsonPropertyName("preparation")]
    public string? Preparation { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

public class NutritionEstimate
{
    [JsonPropertyName("caloriesPerServing")]
    public int? CaloriesPerServing { get; set; }

    [JsonPropertyName("proteinGrams")]
    public int? ProteinGrams { get; set; }

    [JsonPropertyName("carbsGrams")]
    public int? CarbsGrams { get; set; }

    [JsonPropertyName("fatGrams")]
    public int? FatGrams { get; set; }
}
