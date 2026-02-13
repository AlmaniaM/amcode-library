using AMCode.AI.Pipelines;

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
    /// Default provider selection mode: "smart" (uses SmartAIProviderSelector) or "configuration" (uses ConfigurationAIProviderSelector)
    /// </summary>
    public string ProviderSelectionStrategy { get; set; } = "smart";

    /// <summary>
    /// Smart selection strategy when ProviderSelectionStrategy is "smart"
    /// Options: CostOptimized, PerformanceOptimized, ReliabilityOptimized, CapabilityOptimized, QualityOptimized, Balanced
    /// </summary>
    public string SmartSelectionStrategy { get; set; } = "Balanced";

    /// <summary>
    /// Manually selected provider name when ProviderSelectionStrategy is "configuration"
    /// </summary>
    public string SelectedProvider { get; set; } = string.Empty;

    /// <summary>
    /// Provider name for general AI usage (e.g., "OpenAI", "Anthropic", "AzureOpenAIGPT5Nano")
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// Provider name specifically for parsing OCR-extracted text (e.g., "OCRTextParserAI", "Anthropic")
    /// </summary>
    public string OCRTextParserProvider { get; set; } = string.Empty;

    /// <summary>
    /// Fallback provider name for general AI usage (e.g., "OpenAI", "Anthropic", "AzureOpenAIGPT5Nano")
    /// </summary>
    public string? FallbackProvider { get; set; }

    /// <summary>
    /// Fallback provider name specifically for parsing OCR-extracted text (e.g., "OCRTextParserAI", "Anthropic")
    /// </summary>
    public string? FallbackOCRTextParserProvider { get; set; }

    /// <summary>
    /// Whether to enable fallback providers
    /// </summary>
    public bool EnableFallbackProviders { get; set; } = true;

    /// <summary>
    /// Maximum number of fallback attempts
    /// </summary>
    public int MaxFallbackAttempts { get; set; } = 2;

    /// <summary>
    /// Per-pipeline configuration. Keys are pipeline names (e.g., "RecipeExtraction", "GroceryList").
    /// Bound from AI:Pipelines section in appsettings.json.
    /// </summary>
    public Dictionary<string, PipelineConfiguration> Pipelines { get; set; } = new();
}
