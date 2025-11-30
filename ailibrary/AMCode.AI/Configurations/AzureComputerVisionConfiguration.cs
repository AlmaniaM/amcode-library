namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for Azure Computer Vision provider
/// </summary>
public class AzureComputerVisionConfiguration
{
    /// <summary>
    /// Azure Computer Vision subscription key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure Computer Vision endpoint
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure region
    /// </summary>
    public string Region { get; set; } = string.Empty;
    
    /// <summary>
    /// Model to use for requests
    /// </summary>
    public string Model { get; set; } = "gpt-4o";
    
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
    /// Cost per token for input
    /// </summary>
    public decimal CostPerInputToken { get; set; } = 0.00001m;
    
    /// <summary>
    /// Cost per token for output
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.00003m;
    
    /// <summary>
    /// Average response time for this provider
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(3);
}

