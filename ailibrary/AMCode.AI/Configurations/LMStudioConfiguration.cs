namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for LM Studio local provider
/// </summary>
public class LMStudioConfiguration
{
    /// <summary>
    /// LM Studio server URL (default: http://localhost:1234)
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:1234";
    
    /// <summary>
    /// Model to use for requests (name of model loaded in LM Studio)
    /// </summary>
    public string Model { get; set; } = string.Empty;
    
    /// <summary>
    /// Maximum tokens for this provider
    /// </summary>
    public int MaxTokens { get; set; } = 4096;
    
    /// <summary>
    /// Default temperature
    /// </summary>
    public float Temperature { get; set; } = 0.1f;
    
    /// <summary>
    /// Request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(10);
    
    /// <summary>
    /// Whether to enable function calling (if model supports it)
    /// </summary>
    public bool EnableFunctionCalling { get; set; } = false;
    
    /// <summary>
    /// Whether to enable vision capabilities (if model supports it)
    /// </summary>
    public bool EnableVision { get; set; } = false;
    
    /// <summary>
    /// Cost per token (local, so 0)
    /// </summary>
    public decimal CostPerInputToken { get; set; } = 0m;
    
    /// <summary>
    /// Cost per token (local, so 0)
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0m;
    
    /// <summary>
    /// Average response time for this provider
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(5);
}

