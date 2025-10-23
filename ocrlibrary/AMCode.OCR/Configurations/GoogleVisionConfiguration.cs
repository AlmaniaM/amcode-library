using System.Text.Json.Serialization;

namespace AMCode.OCR.Configurations;

/// <summary>
/// Configuration for Google Cloud Vision OCR
/// </summary>
public class GoogleVisionConfiguration
{
    /// <summary>
    /// The Google Cloud project ID
    /// </summary>
    public string ProjectId { get; set; } = string.Empty;

    /// <summary>
    /// The Google Cloud service account key file path
    /// </summary>
    public string? ServiceAccountKeyPath { get; set; }

    /// <summary>
    /// The Google Cloud service account key JSON
    /// </summary>
    public string? ServiceAccountKeyJson { get; set; }

    /// <summary>
    /// The Google Cloud region
    /// </summary>
    public string Region { get; set; } = "us-central1";

    /// <summary>
    /// The API version
    /// </summary>
    public string ApiVersion { get; set; } = "v1";

    /// <summary>
    /// The maximum image size in MB
    /// </summary>
    public int MaxImageSizeMB { get; set; } = 20;

    /// <summary>
    /// The supported languages
    /// </summary>
    public string[] SupportedLanguages { get; set; } = new[] { "en", "es", "fr", "de", "it", "pt", "zh", "ja", "ko", "ar", "hi" };

    /// <summary>
    /// The cost per request in USD
    /// </summary>
    public decimal CostPerRequest { get; set; } = 0.001m;

    /// <summary>
    /// The average response time
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(1.5);

    /// <summary>
    /// The timeout for requests
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// The maximum number of retries
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// The retry delay
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Whether to enable language detection
    /// </summary>
    public bool EnableLanguageDetection { get; set; } = true;

    /// <summary>
    /// Whether to enable handwriting detection
    /// </summary>
    public bool EnableHandwritingDetection { get; set; } = true;

    /// <summary>
    /// Whether to enable table detection
    /// </summary>
    public bool EnableTableDetection { get; set; } = true;

    /// <summary>
    /// Whether to enable form detection
    /// </summary>
    public bool EnableFormDetection { get; set; } = true;

    /// <summary>
    /// The confidence threshold
    /// </summary>
    public double ConfidenceThreshold { get; set; } = 0.5;

    /// <summary>
    /// Whether to enable compression
    /// </summary>
    public bool EnableCompression { get; set; } = true;

    /// <summary>
    /// Whether to enable caching
    /// </summary>
    public bool EnableCaching { get; set; } = false;

    /// <summary>
    /// The cache expiration time
    /// </summary>
    public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromHours(1);

    /// <summary>
    /// Whether to enable logging
    /// </summary>
    public bool EnableLogging { get; set; } = true;

    /// <summary>
    /// The log level
    /// </summary>
    public string LogLevel { get; set; } = "Information";

    /// <summary>
    /// Whether to enable metrics
    /// </summary>
    public bool EnableMetrics { get; set; } = true;

    /// <summary>
    /// Whether to enable health checks
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// The health check interval
    /// </summary>
    public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Whether to enable rate limiting
    /// </summary>
    public bool EnableRateLimiting { get; set; } = true;

    /// <summary>
    /// The rate limit per minute
    /// </summary>
    public int RateLimitPerMinute { get; set; } = 100;

    /// <summary>
    /// Whether to enable circuit breaker
    /// </summary>
    public bool EnableCircuitBreaker { get; set; } = true;

    /// <summary>
    /// The circuit breaker failure threshold
    /// </summary>
    public int CircuitBreakerFailureThreshold { get; set; } = 5;

    /// <summary>
    /// The circuit breaker timeout
    /// </summary>
    public TimeSpan CircuitBreakerTimeout { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Whether to enable error reporting
    /// </summary>
    public bool EnableErrorReporting { get; set; } = true;

    /// <summary>
    /// The error reporting endpoint
    /// </summary>
    public string? ErrorReportingEndpoint { get; set; }

    /// <summary>
    /// Whether to enable telemetry
    /// </summary>
    public bool EnableTelemetry { get; set; } = true;

    /// <summary>
    /// The telemetry endpoint
    /// </summary>
    public string? TelemetryEndpoint { get; set; }

    /// <summary>
    /// Whether to enable debugging
    /// </summary>
    public bool EnableDebugging { get; set; } = false;

    /// <summary>
    /// The debug output directory
    /// </summary>
    public string? DebugOutputDirectory { get; set; }
}