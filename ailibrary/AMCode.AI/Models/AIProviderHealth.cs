namespace AMCode.AI.Models;

/// <summary>
/// Health status information for an AI provider
/// </summary>
public class AIProviderHealth
{
    /// <summary>
    /// Whether the provider is currently healthy
    /// </summary>
    public bool IsHealthy { get; set; }
    
    /// <summary>
    /// Health status description
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Response time for health check
    /// </summary>
    public TimeSpan ResponseTime { get; set; }
    
    /// <summary>
    /// When the health was last checked
    /// </summary>
    public DateTime LastChecked { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Error message if unhealthy
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;
    
    /// <summary>
    /// Current cost for this provider
    /// </summary>
    public decimal CurrentCost { get; set; }
    
    /// <summary>
    /// Requests per minute rate
    /// </summary>
    public int RequestsPerMinute { get; set; }
    
    /// <summary>
    /// Available quota remaining
    /// </summary>
    public int QuotaRemaining { get; set; }
    
    /// <summary>
    /// Quota reset time
    /// </summary>
    public DateTime? QuotaResetTime { get; set; }
    
    /// <summary>
    /// Provider-specific health metrics
    /// </summary>
    public Dictionary<string, object> Metrics { get; set; } = new();
}
