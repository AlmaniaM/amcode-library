namespace AMCode.AI.Configurations;

/// <summary>
/// Base configuration for AI services
/// </summary>
public class AIConfiguration
{
    /// <summary>
    /// Default maximum number of retries
    /// </summary>
    public int MaxRetries { get; set; } = 3;
    
    /// <summary>
    /// Default confidence threshold
    /// </summary>
    public double ConfidenceThreshold { get; set; } = 0.7;
    
    /// <summary>
    /// Default timeout for requests
    /// </summary>
    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// Default maximum tokens
    /// </summary>
    public int DefaultMaxTokens { get; set; } = 4096;
    
    /// <summary>
    /// Default temperature
    /// </summary>
    public float DefaultTemperature { get; set; } = 0.1f;
    
    /// <summary>
    /// Whether to enable cost tracking
    /// </summary>
    public bool EnableCostTracking { get; set; } = true;
    
    /// <summary>
    /// Whether to enable health monitoring
    /// </summary>
    public bool EnableHealthMonitoring { get; set; } = true;
    
    /// <summary>
    /// Health check interval
    /// </summary>
    public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromMinutes(5);
    
    /// <summary>
    /// Default provider selection strategy
    /// </summary>
    public string DefaultSelectionStrategy { get; set; } = "Balanced";
    
    /// <summary>
    /// Whether to enable fallback providers
    /// </summary>
    public bool EnableFallbackProviders { get; set; } = true;
    
    /// <summary>
    /// Maximum number of fallback attempts
    /// </summary>
    public int MaxFallbackAttempts { get; set; } = 2;
}
