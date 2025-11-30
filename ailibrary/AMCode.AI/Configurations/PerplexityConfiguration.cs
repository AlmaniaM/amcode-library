namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for Perplexity provider
/// </summary>
public class PerplexityConfiguration
{
    /// <summary>
    /// Perplexity API key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Base URL for Perplexity API
    /// </summary>
    public string BaseUrl { get; set; } = "https://api.perplexity.ai";
    
    /// <summary>
    /// Model to use for requests
    /// </summary>
    public string Model { get; set; } = "llama-3.1-sonar-large-128k-online";
    
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
    public decimal CostPerInputToken { get; set; } = 0.0000002m;
    
    /// <summary>
    /// Cost per token for output
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.0000002m;
    
    /// <summary>
    /// Average response time for this provider
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(2);
}

