using System.Text.Json.Serialization;

namespace AMCode.OCR.Models;

/// <summary>
/// Represents the result of an OCR operation
/// </summary>
public class OCRResult
{
    /// <summary>
    /// The extracted text from the image
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Individual text blocks with their positions and confidence scores
    /// </summary>
    public List<TextBlock> TextBlocks { get; set; } = new();

    /// <summary>
    /// Overall confidence score for the OCR result (0.0 to 1.0)
    /// </summary>
    public double Confidence { get; set; }

    /// <summary>
    /// Detected language code (e.g., "en", "es", "fr")
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// The provider that processed this result
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// Processing time for this OCR operation
    /// </summary>
    public TimeSpan ProcessingTime { get; set; }

    /// <summary>
    /// Cost of this OCR operation
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// Timestamp when the OCR was performed
    /// </summary>
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Additional metadata about the OCR result
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();

    /// <summary>
    /// Whether the result contains handwritten text
    /// </summary>
    public bool ContainsHandwriting { get; set; }

    /// <summary>
    /// Whether the result contains printed text
    /// </summary>
    public bool ContainsPrintedText { get; set; }

    /// <summary>
    /// The number of words detected
    /// </summary>
    public int WordCount => Text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

    /// <summary>
    /// The number of lines detected
    /// </summary>
    public int LineCount => Text.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length;

    /// <summary>
    /// Error message if the OCR operation failed
    /// </summary>
    public string Error { get; set; } = string.Empty;
}