namespace AMCode.AI.Models;

/// <summary>
/// Structured recipe data parsed from text
/// </summary>
public class ParsedRecipe
{
    /// <summary>
    /// Recipe title
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Recipe description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// List of ingredients
    /// </summary>
    public List<RecipeIngredient> Ingredients { get; set; } = new();
    
    /// <summary>
    /// List of cooking instructions
    /// </summary>
    public List<string> Instructions { get; set; } = new();
    
    /// <summary>
    /// Preparation time in minutes
    /// </summary>
    public int? PrepTimeMinutes { get; set; }
    
    /// <summary>
    /// Cooking time in minutes
    /// </summary>
    public int? CookTimeMinutes { get; set; }
    
    /// <summary>
    /// Total time in minutes
    /// </summary>
    public int? TotalTimeMinutes { get; set; }
    
    /// <summary>
    /// Number of servings
    /// </summary>
    public int? Servings { get; set; }
    
    /// <summary>
    /// Recipe category (e.g., "Dessert", "Main Course")
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// Recipe tags for categorization
    /// </summary>
    public List<string> Tags { get; set; } = new();
    
    /// <summary>
    /// Difficulty level (1-5 scale)
    /// </summary>
    public int? Difficulty { get; set; }
    
    /// <summary>
    /// Confidence score for this specific recipe (0.0 to 1.0)
    /// </summary>
    public double Confidence { get; set; }
    
    /// <summary>
    /// Recipe creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Additional notes or tips
    /// </summary>
    public string Notes { get; set; } = string.Empty;
}

/// <summary>
/// Recipe ingredient with structured information
/// </summary>
public class RecipeIngredient
{
    /// <summary>
    /// Ingredient name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Ingredient amount (e.g., "2", "1/2", "1.5")
    /// </summary>
    public string Amount { get; set; } = string.Empty;
    
    /// <summary>
    /// Ingredient unit (e.g., "cups", "tablespoons", "pounds")
    /// </summary>
    public string Unit { get; set; } = string.Empty;
    
    /// <summary>
    /// Full ingredient text as originally written
    /// </summary>
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    /// Additional notes or preparation instructions
    /// </summary>
    public string Notes { get; set; } = string.Empty;
}
