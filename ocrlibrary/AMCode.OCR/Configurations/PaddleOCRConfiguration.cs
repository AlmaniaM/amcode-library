using System.ComponentModel.DataAnnotations;

namespace AMCode.OCR.Configurations;

/// <summary>
/// Configuration for PaddleOCR Python service
/// </summary>
public class PaddleOCRConfiguration
{
    /// <summary>
    /// The base URL of the Python OCR service
    /// </summary>
    [Required]
    [Url]
    public string ServiceUrl { get; set; } = "http://localhost:8000";

    /// <summary>
    /// The timeout for HTTP requests
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// The maximum number of retries
    /// </summary>
    [Range(0, 10)]
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// The retry delay between attempts
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The maximum image size in MB
    /// </summary>
    [Range(1, 50)]
    public int MaxImageSizeMB { get; set; } = 10;

    /// <summary>
    /// The supported languages
    /// </summary>
    public string[] SupportedLanguages { get; set; } = new[] { "en" };

    /// <summary>
    /// The cost per request in USD (free for CPU-based PaddleOCR)
    /// </summary>
    public decimal CostPerRequest { get; set; } = 0m;

    /// <summary>
    /// The average response time
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Whether to enable language detection
    /// </summary>
    public bool EnableLanguageDetection { get; set; } = false;

    /// <summary>
    /// Whether to enable handwriting detection
    /// </summary>
    public bool EnableHandwritingDetection { get; set; } = true;

    /// <summary>
    /// Whether to enable table detection
    /// </summary>
    public bool EnableTableDetection { get; set; } = false;

    /// <summary>
    /// Whether to enable form detection
    /// </summary>
    public bool EnableFormDetection { get; set; } = false;

    /// <summary>
    /// The confidence threshold
    /// </summary>
    [Range(0.0, 1.0)]
    public double ConfidenceThreshold { get; set; } = 0.5;

    /// <summary>
    /// Whether to enable health checks
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// The health check interval
    /// </summary>
    public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromMinutes(5);
}
