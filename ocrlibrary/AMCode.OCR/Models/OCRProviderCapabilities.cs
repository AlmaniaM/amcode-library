using System.Text.Json.Serialization;

namespace AMCode.OCR.Models;

/// <summary>
/// Represents the capabilities of an OCR provider
/// </summary>
public class OCRProviderCapabilities
{
    /// <summary>
    /// Whether the provider supports language detection
    /// </summary>
    public bool SupportsLanguageDetection { get; set; }

    /// <summary>
    /// Whether the provider supports bounding box coordinates
    /// </summary>
    public bool SupportsBoundingBoxes { get; set; }

    /// <summary>
    /// Whether the provider supports confidence scores
    /// </summary>
    public bool SupportsConfidenceScores { get; set; }

    /// <summary>
    /// Whether the provider supports handwriting recognition
    /// </summary>
    public bool SupportsHandwriting { get; set; }

    /// <summary>
    /// Whether the provider supports printed text recognition
    /// </summary>
    public bool SupportsPrintedText { get; set; }

    /// <summary>
    /// Maximum image size in MB
    /// </summary>
    public int MaxImageSizeMB { get; set; }

    /// <summary>
    /// Supported languages
    /// </summary>
    public string[] SupportedLanguages { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Cost per request in USD
    /// </summary>
    public decimal CostPerRequest { get; set; }

    /// <summary>
    /// Average response time
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; }

    /// <summary>
    /// Whether the provider supports table detection
    /// </summary>
    public bool SupportsTableDetection { get; set; }

    /// <summary>
    /// Whether the provider supports form detection
    /// </summary>
    public bool SupportsFormDetection { get; set; }

    /// <summary>
    /// Whether the provider supports custom models
    /// </summary>
    public bool SupportsCustomModels { get; set; }

    /// <summary>
    /// Maximum number of pages per request
    /// </summary>
    public int MaxPagesPerRequest { get; set; } = 1;

    /// <summary>
    /// Whether the provider supports batch processing
    /// </summary>
    public bool SupportsBatchProcessing { get; set; }

    /// <summary>
    /// Whether the provider supports real-time processing
    /// </summary>
    public bool SupportsRealTimeProcessing { get; set; }

    /// <summary>
    /// Whether the provider supports image preprocessing
    /// </summary>
    public bool SupportsImagePreprocessing { get; set; }

    /// <summary>
    /// Whether the provider supports rotation detection
    /// </summary>
    public bool SupportsRotationDetection { get; set; }

    /// <summary>
    /// Whether the provider supports skew detection
    /// </summary>
    public bool SupportsSkewDetection { get; set; }
}