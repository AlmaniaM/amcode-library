using System.Text.Json.Serialization;

namespace AMCode.OCR.Configurations;

/// <summary>
/// Configuration for Azure Computer Vision OCR
/// </summary>
public class AzureOCRConfiguration
{
    /// <summary>
    /// The Azure Computer Vision endpoint
    /// </summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// The Azure Computer Vision subscription key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The Azure region
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// The API version
    /// Current version: 2023-02-01-preview
    /// Note: Check Azure Computer Vision documentation for newer API versions (e.g., 2024-02-01-preview or later)
    /// Newer versions may provide improved accuracy and additional features
    /// </summary>
    public string ApiVersion { get; set; } = "2023-02-01-preview";

    /// <summary>
    /// The default language to use as a hint for OCR (e.g., "en", "es", "fr")
    /// Used when ExpectedLanguage is not provided in OCRRequest
    /// </summary>
    public string DefaultLanguage { get; set; } = "en";

    /// <summary>
    /// The model version
    /// </summary>
    public string ModelVersion { get; set; } = "latest";

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
    /// The maximum image size in MB
    /// </summary>
    public int MaxImageSizeMB { get; set; } = 50;

    /// <summary>
    /// The supported languages
    /// </summary>
    public string[] SupportedLanguages { get; set; } = new[] { "en", "es", "fr", "de", "it", "pt", "zh", "ja", "ko" };

    /// <summary>
    /// The cost per request in USD
    /// </summary>
    public decimal CostPerRequest { get; set; } = 0.001m;

    /// <summary>
    /// The average response time
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(2);

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
