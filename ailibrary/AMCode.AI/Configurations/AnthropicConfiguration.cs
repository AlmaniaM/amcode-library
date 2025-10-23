namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for Anthropic Claude provider
/// </summary>
public class AnthropicConfiguration
{
    /// <summary>
    /// Anthropic API key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Model to use for requests
    /// </summary>
    public string Model { get; set; } = "claude-3-5-sonnet-20241022";
    
    /// <summary>
    /// Base URL for Anthropic API
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.anthropic.com";
    
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
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// Whether to enable function calling
    /// </summary>
    public bool EnableFunctionCalling { get; set; } = true;
    
    /// <summary>
    /// Whether to enable vision capabilities
    /// </summary>
    public bool EnableVision { get; set; } = true;
    
    /// <summary>
    /// Cost per token for input
    /// </summary>
    public decimal CostPerInputToken { get; set; } = 0.000025m;
    
    /// <summary>
    /// Cost per token for output
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.000125m;
}
