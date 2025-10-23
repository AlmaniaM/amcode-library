namespace AMCode.AI.Models;

/// <summary>
/// Options and preferences for recipe parsing
/// </summary>
public class RecipeParsingOptions
{
    /// <summary>
    /// Maximum number of tokens to use for the response
    /// </summary>
    public int? MaxTokens { get; set; }
    
    /// <summary>
    /// Temperature for AI creativity (0.0 to 2.0)
    /// </summary>
    public float? Temperature { get; set; }
    
    /// <summary>
    /// Whether to require function calling for structured output
    /// </summary>
    public bool RequiresFunctionCalling { get; set; } = false;
    
    /// <summary>
    /// Whether to analyze images if present
    /// </summary>
    public bool RequiresVision { get; set; } = false;
    
    /// <summary>
    /// Language preference for parsing
    /// </summary>
    public string Language { get; set; } = "en";
    
    /// <summary>
    /// Whether to include confidence scores
    /// </summary>
    public bool IncludeConfidenceScores { get; set; } = true;
    
    /// <summary>
    /// Whether to include raw response for debugging
    /// </summary>
    public bool IncludeRawResponse { get; set; } = false;
    
    /// <summary>
    /// Custom prompt instructions
    /// </summary>
    public string CustomInstructions { get; set; } = string.Empty;
    
    /// <summary>
    /// Whether to extract nutritional information
    /// </summary>
    public bool ExtractNutritionalInfo { get; set; } = false;
    
    /// <summary>
    /// Whether to extract cooking techniques
    /// </summary>
    public bool ExtractCookingTechniques { get; set; } = false;
    
    /// <summary>
    /// Whether to extract equipment needed
    /// </summary>
    public bool ExtractEquipment { get; set; } = false;
    
    /// <summary>
    /// Maximum number of recipes to extract (for multi-recipe text)
    /// </summary>
    public int MaxRecipes { get; set; } = 1;
    
    /// <summary>
    /// Whether to validate parsed data
    /// </summary>
    public bool ValidateData { get; set; } = true;
}
