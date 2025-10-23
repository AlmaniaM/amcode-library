using System.Text.Json.Serialization;

namespace AMCode.OCR.Models;

/// <summary>
/// Represents the health status of an OCR provider
/// </summary>
public class OCRProviderHealth
{
    /// <summary>
    /// Whether the provider is healthy
    /// </summary>
    public bool IsHealthy { get; set; }

    /// <summary>
    /// Status message
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Response time for the health check
    /// </summary>
    public TimeSpan ResponseTime { get; set; }

    /// <summary>
    /// When the health check was last performed
    /// </summary>
    public DateTime LastChecked { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Error message if the provider is unhealthy
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Additional health metrics
    /// </summary>
    public Dictionary<string, object> Metrics { get; set; } = new();

    /// <summary>
    /// Whether the provider is available for processing
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// The number of requests processed in the last hour
    /// </summary>
    public int RequestsPerHour { get; set; }

    /// <summary>
    /// The number of errors in the last hour
    /// </summary>
    public int ErrorsPerHour { get; set; }

    /// <summary>
    /// The success rate percentage
    /// </summary>
    public double SuccessRate { get; set; }

    /// <summary>
    /// The average processing time
    /// </summary>
    public TimeSpan AverageProcessingTime { get; set; }

    /// <summary>
    /// The last error that occurred
    /// </summary>
    public string? LastError { get; set; }

    /// <summary>
    /// When the last error occurred
    /// </summary>
    public DateTime? LastErrorTime { get; set; }

    /// <summary>
    /// Whether the provider is experiencing high load
    /// </summary>
    public bool IsHighLoad { get; set; }

    /// <summary>
    /// The current load percentage (0-100)
    /// </summary>
    public double LoadPercentage { get; set; }

    /// <summary>
    /// Whether the provider is in maintenance mode
    /// </summary>
    public bool IsInMaintenance { get; set; }

    /// <summary>
    /// When maintenance mode will end
    /// </summary>
    public DateTime? MaintenanceEndTime { get; set; }
}