using AMCode.OCR.Models;

namespace AMCode.OCR;

/// <summary>
/// Interface for OCR providers
/// </summary>
public interface IOCRProvider
{
    /// <summary>
    /// The name of the OCR provider
    /// </summary>
    string ProviderName { get; }

    /// <summary>
    /// Whether the provider requires internet connection
    /// </summary>
    bool RequiresInternet { get; }

    /// <summary>
    /// Whether the provider is currently available
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// The capabilities of this OCR provider
    /// </summary>
    OCRProviderCapabilities Capabilities { get; }

    /// <summary>
    /// Processes an image stream and returns OCR result
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes an image stream with custom options
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    Task<OCRResult> ProcessImageAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks the health of the OCR provider
    /// </summary>
    /// <returns>Health status information</returns>
    Task<OCRProviderHealth> CheckHealthAsync();

    /// <summary>
    /// Gets the cost estimate for processing an image
    /// </summary>
    /// <param name="imageSizeBytes">Size of the image in bytes</param>
    /// <param name="options">OCR processing options</param>
    /// <returns>Estimated cost in USD</returns>
    Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null);

    /// <summary>
    /// Processes multiple images in batch
    /// </summary>
    /// <param name="imageStreams">Collection of image streams to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of OCR results</returns>
    Task<IEnumerable<OCRResult>> ProcessBatchAsync(
        IEnumerable<Stream> imageStreams, 
        OCRRequest? options = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if the provider can process the given request
    /// </summary>
    /// <param name="options">OCR processing options</param>
    /// <returns>True if the provider can process the request, false otherwise</returns>
    bool CanProcess(OCRRequest options);

    /// <summary>
    /// Gets the estimated processing time for an image
    /// </summary>
    /// <param name="imageSizeBytes">Size of the image in bytes</param>
    /// <param name="options">OCR processing options</param>
    /// <returns>Estimated processing time</returns>
    TimeSpan GetEstimatedProcessingTime(long imageSizeBytes, OCRRequest? options = null);

    /// <summary>
    /// Gets the reliability score for this provider
    /// </summary>
    /// <returns>Reliability score (0.0 to 1.0)</returns>
    double GetReliabilityScore();

    /// <summary>
    /// Gets the quality score for this provider
    /// </summary>
    /// <returns>Quality score (0.0 to 1.0)</returns>
    double GetQualityScore();
}