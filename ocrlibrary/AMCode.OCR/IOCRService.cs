using AMCode.OCR.Models;

namespace AMCode.OCR;

/// <summary>
/// Main interface for OCR services
/// </summary>
public interface IOCRService
{
    /// <summary>
    /// Extracts text from an image stream
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    Task<Result<OCRResult>> ExtractTextAsync(Stream imageStream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts text from an image file
    /// </summary>
    /// <param name="imagePath">The path to the image file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    Task<Result<OCRResult>> ExtractTextAsync(string imagePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts text from an image with custom options
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    Task<Result<OCRResult>> ExtractTextAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extracts text from an image file with custom options
    /// </summary>
    /// <param name="imagePath">The path to the image file</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    Task<Result<OCRResult>> ExtractTextAsync(string imagePath, OCRRequest options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the OCR service is available
    /// </summary>
    /// <returns>True if the service is available, false otherwise</returns>
    Task<bool> IsAvailableAsync();

    /// <summary>
    /// Gets the health status of the OCR service
    /// </summary>
    /// <returns>Health status information</returns>
    Task<OCRProviderHealth> GetHealthAsync();

    /// <summary>
    /// Gets the capabilities of the OCR service
    /// </summary>
    /// <returns>Service capabilities</returns>
    OCRProviderCapabilities GetCapabilities();

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
    Task<Result<IEnumerable<OCRResult>>> ProcessBatchAsync(
        IEnumerable<Stream> imageStreams, 
        OCRRequest? options = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes multiple image files in batch
    /// </summary>
    /// <param name="imagePaths">Collection of image file paths to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of OCR results</returns>
    Task<Result<IEnumerable<OCRResult>>> ProcessBatchAsync(
        IEnumerable<string> imagePaths, 
        OCRRequest? options = null, 
        CancellationToken cancellationToken = default);
}