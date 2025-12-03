using System.ComponentModel.DataAnnotations;

namespace AMCode.OCR.Configurations;

/// <summary>
/// Configuration for Anthropic Claude OCR provider
/// </summary>
public class AnthropicOCRConfiguration
{
    /// <summary>
    /// Anthropic API key
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Model to use for OCR requests (e.g., claude-3-5-sonnet-20241022, claude-3-opus-20240229)
    /// </summary>
    public string Model { get; set; } = "claude-3-5-sonnet-20241022";

    /// <summary>
    /// Base URL for Anthropic API
    /// </summary>
    [Url]
    public string BaseUrl { get; set; } = "https://api.anthropic.com";

    /// <summary>
    /// Maximum tokens for OCR response
    /// </summary>
    [Range(1, 8192)]
    public int MaxTokens { get; set; } = 4096;

    /// <summary>
    /// Temperature for OCR processing (lower = more deterministic)
    /// </summary>
    [Range(0.0, 2.0)]
    public double Temperature { get; set; } = 0.1;

    /// <summary>
    /// Request timeout
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Maximum number of retries
    /// </summary>
    [Range(0, 10)]
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Retry delay between attempts
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Maximum image size in MB
    /// </summary>
    [Range(1, 50)]
    public int MaxImageSizeMB { get; set; } = 20;

    /// <summary>
    /// Supported languages
    /// </summary>
    public string[] SupportedLanguages { get; set; } = new[] 
    { 
        "en", "es", "fr", "de", "it", "pt", "nl", "pl", "ru", "zh", "ja", "ko" 
    };

    /// <summary>
    /// Cost per input token in USD
    /// </summary>
    public decimal CostPerInputToken { get; set; } = 0.000025m;

    /// <summary>
    /// Cost per output token in USD
    /// </summary>
    public decimal CostPerOutputToken { get; set; } = 0.000125m;

    /// <summary>
    /// Average response time
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; } = TimeSpan.FromSeconds(3);

    /// <summary>
    /// Whether to enable handwriting detection
    /// </summary>
    public bool EnableHandwritingDetection { get; set; } = true;

    /// <summary>
    /// Whether to enable language detection
    /// </summary>
    public bool EnableLanguageDetection { get; set; } = true;

    /// <summary>
    /// Whether to enable table detection
    /// </summary>
    public bool EnableTableDetection { get; set; } = true;

    /// <summary>
    /// Whether to enable form detection
    /// </summary>
    public bool EnableFormDetection { get; set; } = false;

    /// <summary>
    /// Confidence threshold for results
    /// </summary>
    [Range(0.0, 1.0)]
    public double ConfidenceThreshold { get; set; } = 0.7;

    /// <summary>
    /// Whether to enable health checks
    /// </summary>
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// Health check interval
    /// </summary>
    public TimeSpan HealthCheckInterval { get; set; } = TimeSpan.FromMinutes(5);
}

