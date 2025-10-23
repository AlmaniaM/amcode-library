namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for Hugging Face provider
/// </summary>
public class HuggingFaceConfiguration
{
    /// <summary>
    /// Hugging Face API key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Model to use for requests
    /// </summary>
    public string Model { get; set; } = "microsoft/DialoGPT-medium";
    
    /// <summary>
    /// Base URL for Hugging Face API
    /// </summary>
    public string BaseUrl { get; set; } = "https://api-inference.huggingface.co";
    
    /// <summary>
    /// Maximum tokens for this provider
    /// </summary>
    public int MaxTokens { get; set; } = 1024;
    
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
    public decimal CostPerInputToken { get; set; } = 0.00001m;
    
    /// <summary>
    /// Cost per token for output
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.00001m;
    
    /// <summary>
    /// Base cost per request
    /// </summary>
    public decimal CostPerRequest { get; set; } = 0.001m;
    
    /// <summary>
    /// Whether to use the free tier
    /// </summary>
    public bool UseFreeTier { get; set; } = true;
}
