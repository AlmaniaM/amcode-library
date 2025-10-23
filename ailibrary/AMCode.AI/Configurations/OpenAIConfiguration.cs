namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for OpenAI provider
/// </summary>
public class OpenAIConfiguration
{
    /// <summary>
    /// OpenAI API key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// OpenAI organization ID
    /// </summary>
    public string OrganizationId { get; set; } = string.Empty;
    
    /// <summary>
    /// Model to use for requests
    /// </summary>
    public string Model { get; set; } = "gpt-4o";
    
    /// <summary>
    /// Base URL for OpenAI API
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.openai.com/v1";
    
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
    public decimal CostPerInputToken { get; set; } = 0.00003m;
    
    /// <summary>
    /// Cost per token for output
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.00006m;
}
