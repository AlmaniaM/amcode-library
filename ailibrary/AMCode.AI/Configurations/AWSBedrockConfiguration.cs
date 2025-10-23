namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for AWS Bedrock provider
/// </summary>
public class AWSBedrockConfiguration
{
    /// <summary>
    /// AWS region
    /// </summary>
    public string Region { get; set; } = "us-east-1";
    
    /// <summary>
    /// Model to use for requests
    /// </summary>
    public string Model { get; set; } = "anthropic.claude-3-5-sonnet-20241022-v2:0";
    
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
    public bool EnableFunctionCalling { get; set; } = false;
    
    /// <summary>
    /// Whether to enable vision capabilities
    /// </summary>
    public bool EnableVision { get; set; } = false;
    
    /// <summary>
    /// Cost per token for input
    /// </summary>
    public decimal CostPerInputToken { get; set; } = 0.00003m;
    
    /// <summary>
    /// Cost per token for output
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.00015m;
}
