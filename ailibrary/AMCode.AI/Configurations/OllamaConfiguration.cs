namespace AMCode.AI.Configurations;

/// <summary>
/// Configuration for Ollama local provider
/// </summary>
public class OllamaConfiguration
{
    /// <summary>
    /// Ollama server URL
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:11434";
    
    /// <summary>
    /// Model to use for requests
    /// </summary>
    public string Model { get; set; } = "llama3.1:8b";
    
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
    /// Whether to enable function calling
    /// </summary>
    public bool EnableFunctionCalling { get; set; } = false;
    
    /// <summary>
    /// Whether to enable vision capabilities
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
    /// Whether to use GPU acceleration
    /// </summary>
    public bool UseGpu { get; set; } = true;
    
    /// <summary>
    /// Number of threads to use
    /// </summary>
    public int NumThreads { get; set; } = 4;
    
    /// <summary>
    /// Average response time for this provider
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(5);
}
