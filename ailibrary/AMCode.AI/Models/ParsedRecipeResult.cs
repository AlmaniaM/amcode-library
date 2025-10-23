namespace AMCode.AI.Models;

/// <summary>
/// Result of AI recipe parsing containing structured recipe data
/// </summary>
public class ParsedRecipeResult
{
    /// <summary>
    /// Array of parsed recipes (usually one, but can be multiple)
    /// </summary>
    public ParsedRecipe[] Recipes { get; set; } = Array.Empty<ParsedRecipe>();
    
    /// <summary>
    /// Overall confidence score for the parsing (0.0 to 1.0)
    /// </summary>
    public double Confidence { get; set; }
    
    /// <summary>
    /// Source AI provider that processed this request
    /// </summary>
    public string Source { get; set; } = string.Empty;
    
    /// <summary>
    /// Processing time for this request
    /// </summary>
    public DateTime ProcessingTime { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Cost of this AI operation (if applicable)
    /// </summary>
    public decimal Cost { get; set; }
    
    /// <summary>
    /// Number of tokens used for this request
    /// </summary>
    public int TokensUsed { get; set; }
    
    /// <summary>
    /// Any warnings or additional information
    /// </summary>
    public string[] Warnings { get; set; } = Array.Empty<string>();
    
    /// <summary>
    /// Raw response from the AI provider (for debugging)
    /// </summary>
    public string RawResponse { get; set; } = string.Empty;
}
