using AMCode.AI.Models;

namespace AMCode.AI.Models;

/// <summary>
/// Request object for AI processing
/// </summary>
public class AIRequest
{
    /// <summary>
    /// Text to be processed
    /// </summary>
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    /// Parsing options and preferences
    /// </summary>
    public RecipeParsingOptions Options { get; set; } = new();
    
    /// <summary>
    /// Estimated number of tokens for this request
    /// </summary>
    public int EstimatedTokens { get; set; }
    
    /// <summary>
    /// Whether function calling is required
    /// </summary>
    public bool RequiresFunctionCalling { get; set; }
    
    /// <summary>
    /// Whether vision/image analysis is required
    /// </summary>
    public bool RequiresVision { get; set; }
    
    /// <summary>
    /// Maximum number of retries
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    
    /// <summary>
    /// Minimum confidence threshold
    /// </summary>
    public double ConfidenceThreshold { get; set; } = 0.7;
    
    /// <summary>
    /// Request priority (1-10, higher is more important)
    /// </summary>
    public int Priority { get; set; } = 5;
    
    /// <summary>
    /// Request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// Custom metadata for this request
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
