using System.Text.Json.Serialization;

namespace AMCode.OCR.Models;

/// <summary>
/// Represents a request for OCR processing
/// </summary>
public class OCRRequest
{
    /// <summary>
    /// The image stream to process
    /// </summary>
    [JsonIgnore]
    public Stream? ImageStream { get; set; }

    /// <summary>
    /// The image path to process
    /// </summary>
    public string? ImagePath { get; set; }

    /// <summary>
    /// The image bytes to process
    /// </summary>
    [JsonIgnore]
    public byte[]? ImageBytes { get; set; }

    /// <summary>
    /// Whether language detection is required
    /// </summary>
    public bool RequiresLanguageDetection { get; set; }

    /// <summary>
    /// Whether handwriting support is required
    /// </summary>
    public bool RequiresHandwritingSupport { get; set; }

    /// <summary>
    /// Whether table detection is required
    /// </summary>
    public bool RequiresTableDetection { get; set; }

    /// <summary>
    /// Whether form detection is required
    /// </summary>
    public bool RequiresFormDetection { get; set; }

    /// <summary>
    /// The expected language code (e.g., "en", "es", "fr")
    /// </summary>
    public string? ExpectedLanguage { get; set; }

    /// <summary>
    /// The confidence threshold for results (0.0 to 1.0)
    /// </summary>
    public double ConfidenceThreshold { get; set; } = 0.5;

    /// <summary>
    /// The maximum number of retries
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// The timeout for the request
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Whether to preprocess the image
    /// </summary>
    public bool PreprocessImage { get; set; } = true;

    /// <summary>
    /// Image preprocessing options
    /// </summary>
    public ImagePreprocessingOptions PreprocessingOptions { get; set; } = new();

    /// <summary>
    /// Additional options for the OCR request
    /// </summary>
    public Dictionary<string, object> Options { get; set; } = new();

    /// <summary>
    /// The priority of the request (1-10, 10 being highest)
    /// </summary>
    public int Priority { get; set; } = 5;

    /// <summary>
    /// Whether to return detailed text blocks
    /// </summary>
    public bool ReturnDetailedTextBlocks { get; set; } = true;

    /// <summary>
    /// Whether to return bounding boxes
    /// </summary>
    public bool ReturnBoundingBoxes { get; set; } = true;

    /// <summary>
    /// Whether to return confidence scores
    /// </summary>
    public bool ReturnConfidenceScores { get; set; } = true;

    /// <summary>
    /// The maximum number of pages to process
    /// </summary>
    public int MaxPages { get; set; } = 1;

    /// <summary>
    /// The maximum image size in MB
    /// </summary>
    public int MaxImageSizeMB { get; set; } = 50;

    /// <summary>
    /// Whether to process the image in real-time
    /// </summary>
    public bool ProcessInRealTime { get; set; } = false;

    /// <summary>
    /// The callback URL for async processing
    /// </summary>
    public string? CallbackUrl { get; set; }

    /// <summary>
    /// The correlation ID for tracking
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// The user ID for tracking
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The session ID for tracking
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// The preferred language for OCR processing
    /// </summary>
    public string? PreferredLanguage { get; set; }

    /// <summary>
    /// The maximum cost per request in USD
    /// </summary>
    public decimal MaxCostPerRequest { get; set; } = 1.0m;
}

/// <summary>
/// Represents image preprocessing options
/// </summary>
public class ImagePreprocessingOptions
{
    /// <summary>
    /// Whether to enhance contrast
    /// </summary>
    public bool EnhanceContrast { get; set; } = true;

    /// <summary>
    /// Whether to enhance brightness
    /// </summary>
    public bool EnhanceBrightness { get; set; } = true;

    /// <summary>
    /// Whether to remove noise
    /// </summary>
    public bool RemoveNoise { get; set; } = true;

    /// <summary>
    /// Whether to detect and correct skew
    /// </summary>
    public bool DetectSkew { get; set; } = true;

    /// <summary>
    /// Whether to detect and correct rotation
    /// </summary>
    public bool DetectRotation { get; set; } = true;

    /// <summary>
    /// Whether to resize the image
    /// </summary>
    public bool ResizeImage { get; set; } = false;

    /// <summary>
    /// The maximum width for resizing
    /// </summary>
    public int MaxWidth { get; set; } = 2048;

    /// <summary>
    /// The maximum height for resizing
    /// </summary>
    public int MaxHeight { get; set; } = 2048;

    /// <summary>
    /// Whether to convert to grayscale
    /// </summary>
    public bool ConvertToGrayscale { get; set; } = false;

    /// <summary>
    /// Whether to apply sharpening
    /// </summary>
    public bool ApplySharpening { get; set; } = true;

    /// <summary>
    /// The sharpening factor (0.0 to 2.0)
    /// </summary>
    public double SharpeningFactor { get; set; } = 1.0;

    /// <summary>
    /// Whether to apply edge detection
    /// </summary>
    public bool ApplyEdgeDetection { get; set; } = false;

    /// <summary>
    /// The edge detection threshold
    /// </summary>
    public double EdgeDetectionThreshold { get; set; } = 0.5;
}