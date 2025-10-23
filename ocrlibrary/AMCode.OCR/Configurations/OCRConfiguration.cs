using System.Text.Json.Serialization;
using AMCode.OCR.Enums;

namespace AMCode.OCR.Configurations;

/// <summary>
/// Main configuration for OCR services
/// </summary>
public class OCRConfiguration
{
    /// <summary>
    /// The default selection strategy
    /// </summary>
    public OCRProviderSelectionStrategy DefaultSelectionStrategy { get; set; } = OCRProviderSelectionStrategy.Balanced;

    /// <summary>
    /// The default confidence threshold
    /// </summary>
    public double DefaultConfidenceThreshold { get; set; } = 0.5;

    /// <summary>
    /// The maximum number of retries
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// The default timeout for requests
    /// </summary>
    public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Whether to enable fallback providers
    /// </summary>
    public bool EnableFallbackProviders { get; set; } = true;

    /// <summary>
    /// The maximum number of fallback providers to try
    /// </summary>
    public int MaxFallbackProviders { get; set; } = 3;

    /// <summary>
    /// Whether to enable cost tracking
    /// </summary>
    public bool EnableCostTracking { get; set; } = true;

    /// <summary>
    /// Whether to enable performance monitoring
    /// </summary>
    public bool EnablePerformanceMonitoring { get; set; } = true;

    /// <summary>
    /// Whether to enable health checks
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// The interval for health checks
    /// </summary>
    public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Whether to enable image preprocessing
    /// </summary>
    public bool EnableImagePreprocessing { get; set; } = true;

    /// <summary>
    /// Whether to enable batch processing
    /// </summary>
    public bool EnableBatchProcessing { get; set; } = true;

    /// <summary>
    /// The maximum batch size
    /// </summary>
    public int MaxBatchSize { get; set; } = 10;

    /// <summary>
    /// The maximum image size in MB
    /// </summary>
    public int MaxImageSizeMB { get; set; } = 50;

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
    /// Whether to enable metrics collection
    /// </summary>
    public bool EnableMetrics { get; set; } = true;

    /// <summary>
    /// The metrics collection interval
    /// </summary>
    public TimeSpan MetricsInterval { get; set; } = TimeSpan.FromMinutes(1);

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
    /// Whether to enable retry policies
    /// </summary>
    public bool EnableRetryPolicies { get; set; } = true;

    /// <summary>
    /// The retry delay
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The retry backoff multiplier
    /// </summary>
    public double RetryBackoffMultiplier { get; set; } = 2.0;

    /// <summary>
    /// The maximum retry delay
    /// </summary>
    public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Whether to enable compression
    /// </summary>
    public bool EnableCompression { get; set; } = true;

    /// <summary>
    /// The compression level
    /// </summary>
    public int CompressionLevel { get; set; } = 6;

    /// <summary>
    /// Whether to enable encryption
    /// </summary>
    public bool EnableEncryption { get; set; } = false;

    /// <summary>
    /// The encryption key
    /// </summary>
    public string? EncryptionKey { get; set; }

    /// <summary>
    /// Whether to enable data validation
    /// </summary>
    public bool EnableDataValidation { get; set; } = true;

    /// <summary>
    /// Whether to enable result validation
    /// </summary>
    public bool EnableResultValidation { get; set; } = true;

    /// <summary>
    /// The result validation timeout
    /// </summary>
    public TimeSpan ResultValidationTimeout { get; set; } = TimeSpan.FromSeconds(30);

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

    /// <summary>
    /// Whether to enable profiling
    /// </summary>
    public bool EnableProfiling { get; set; } = false;

    /// <summary>
    /// The profiling output directory
    /// </summary>
    public string? ProfilingOutputDirectory { get; set; }
}