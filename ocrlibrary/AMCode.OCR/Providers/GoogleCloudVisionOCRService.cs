using Google.Cloud.Vision.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMCode.OCR.Models;
using AMCode.OCR.Configurations;
using System.Text.Json;

namespace AMCode.OCR.Providers;

/// <summary>
/// Google Cloud Vision OCR provider implementation
/// </summary>
public class GoogleCloudVisionOCRService : IOCRProvider
{
    private readonly ImageAnnotatorClient _client;
    private readonly ILogger<GoogleCloudVisionOCRService> _logger;
    private readonly GoogleVisionConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the GoogleCloudVisionOCRService class
    /// </summary>
    /// <param name="client">The Google Cloud Vision client</param>
    /// <param name="logger">The logger</param>
    /// <param name="config">The configuration</param>
    public GoogleCloudVisionOCRService(
        ImageAnnotatorClient client,
        ILogger<GoogleCloudVisionOCRService> logger,
        IOptions<GoogleVisionConfiguration> config)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// The name of the OCR provider
    /// </summary>
    public string ProviderName => "Google Cloud Vision";

    /// <summary>
    /// Whether the provider requires internet connection
    /// </summary>
    public bool RequiresInternet => true;

    /// <summary>
    /// Whether the provider is currently available
    /// </summary>
    public bool IsAvailable => _client != null;

    /// <summary>
    /// The capabilities of this OCR provider
    /// </summary>
    public OCRProviderCapabilities Capabilities => new OCRProviderCapabilities
    {
        SupportsLanguageDetection = _config.EnableLanguageDetection,
        SupportsBoundingBoxes = true,
        SupportsConfidenceScores = false, // Google Cloud Vision doesn't provide confidence scores
        SupportsHandwriting = _config.EnableHandwritingDetection,
        SupportsPrintedText = true,
        SupportsTableDetection = _config.EnableTableDetection,
        SupportsFormDetection = _config.EnableFormDetection,
        MaxImageSizeMB = _config.MaxImageSizeMB,
        SupportedLanguages = _config.SupportedLanguages,
        CostPerRequest = _config.CostPerRequest,
        AverageResponseTime = _config.AverageResponseTime,
        SupportsCustomModels = false,
        MaxPagesPerRequest = 1,
        SupportsBatchProcessing = false,
        SupportsRealTimeProcessing = true,
        SupportsImagePreprocessing = true,
        SupportsRotationDetection = true,
        SupportsSkewDetection = true
    };

    /// <summary>
    /// Processes an image stream and returns OCR result
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public async Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default)
    {
        return await ProcessImageAsync(imageStream, new OCRRequest(), cancellationToken);
    }

    /// <summary>
    /// Processes an image stream with custom options
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public async Task<OCRResult> ProcessImageAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Processing image with Google Cloud Vision");

            var startTime = DateTime.UtcNow;
            var imageBytes = await ReadStreamToBytesAsync(imageStream);
            
            var image = Image.FromBytes(imageBytes);
            var response = await _client.DetectTextAsync(image);
            var processingTime = DateTime.UtcNow - startTime;

            var result = ProcessGoogleVisionResponse(response, processingTime);
            result.Provider = ProviderName;
            result.Cost = _config.CostPerRequest;
            
            _logger.LogInformation("Google Cloud Vision OCR completed successfully in {ProcessingTime}ms", 
                processingTime.TotalMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google Cloud Vision OCR failed");
            throw new OCRException($"Google Cloud Vision processing failed: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Checks the health of the OCR provider
    /// </summary>
    /// <returns>Health status information</returns>
    public async Task<OCRProviderHealth> CheckHealthAsync()
    {
        try
        {
            var startTime = DateTime.UtcNow;
            
            // Try to detect text with a minimal image
            var image = Image.FromBytes(new byte[0]);
            var response = await _client.DetectTextAsync(image);
            var responseTime = DateTime.UtcNow - startTime;
            
            return new OCRProviderHealth
            {
                IsHealthy = true,
                IsAvailable = true,
                Status = "Healthy",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
                SuccessRate = 100.0,
                AverageProcessingTime = _config.AverageResponseTime
            };
        }
        catch (Exception ex)
        {
            return new OCRProviderHealth
            {
                IsHealthy = false,
                IsAvailable = false,
                Status = "Unhealthy",
                ResponseTime = TimeSpan.Zero,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = ex.Message,
                LastError = ex.Message,
                LastErrorTime = DateTime.UtcNow,
                SuccessRate = 0.0
            };
        }
    }

    /// <summary>
    /// Gets the cost estimate for processing an image
    /// </summary>
    /// <param name="imageSizeBytes">Size of the image in bytes</param>
    /// <param name="options">OCR processing options</param>
    /// <returns>Estimated cost in USD</returns>
    public Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null)
    {
        // Google Cloud Vision charges per request, not by image size
        return Task.FromResult(_config.CostPerRequest);
    }

    /// <summary>
    /// Processes multiple images in batch
    /// </summary>
    /// <param name="imageStreams">Collection of image streams to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of OCR results</returns>
    public async Task<IEnumerable<OCRResult>> ProcessBatchAsync(
        IEnumerable<Stream> imageStreams, 
        OCRRequest? options = null, 
        CancellationToken cancellationToken = default)
    {
        var results = new List<OCRResult>();
        
        foreach (var imageStream in imageStreams)
        {
            try
            {
                var result = await ProcessImageAsync(imageStream, options ?? new OCRRequest(), cancellationToken);
                results.Add(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process image in batch");
                results.Add(new OCRResult
                {
                    Provider = ProviderName,
                    Error = ex.Message,
                    ProcessingTime = TimeSpan.Zero,
                    ProcessedAt = DateTime.UtcNow
                });
            }
        }
        
        return results;
    }

    /// <summary>
    /// Validates if the provider can process the given request
    /// </summary>
    /// <param name="options">OCR processing options</param>
    /// <returns>True if the provider can process the request, false otherwise</returns>
    public bool CanProcess(OCRRequest options)
    {
        if (options.ImageStream == null && string.IsNullOrEmpty(options.ImagePath))
            return false;

        if (options.MaxImageSizeMB > _config.MaxImageSizeMB)
            return false;

        if (options.RequiresLanguageDetection && !_config.EnableLanguageDetection)
            return false;

        if (options.RequiresHandwritingSupport && !_config.EnableHandwritingDetection)
            return false;

        if (options.RequiresTableDetection && !_config.EnableTableDetection)
            return false;

        if (options.RequiresFormDetection && !_config.EnableFormDetection)
            return false;

        return true;
    }

    /// <summary>
    /// Gets the estimated processing time for an image
    /// </summary>
    /// <param name="imageSizeBytes">Size of the image in bytes</param>
    /// <param name="options">OCR processing options</param>
    /// <returns>Estimated processing time</returns>
    public TimeSpan GetEstimatedProcessingTime(long imageSizeBytes, OCRRequest? options = null)
    {
        // Base processing time plus additional time based on image size
        var baseTime = _config.AverageResponseTime;
        var sizeFactor = Math.Max(1.0, imageSizeBytes / (1024.0 * 1024.0)); // Convert to MB
        var additionalTime = TimeSpan.FromMilliseconds(sizeFactor * 80); // 80ms per MB
        
        return baseTime.Add(additionalTime);
    }

    /// <summary>
    /// Gets the reliability score for this provider
    /// </summary>
    /// <returns>Reliability score (0.0 to 1.0)</returns>
    public double GetReliabilityScore()
    {
        // Google Cloud Vision is generally very reliable
        return 0.94;
    }

    /// <summary>
    /// Gets the quality score for this provider
    /// </summary>
    /// <returns>Quality score (0.0 to 1.0)</returns>
    public double GetQualityScore()
    {
        // Google Cloud Vision provides high-quality OCR results with good language detection
        return 0.92;
    }

    /// <summary>
    /// Reads a stream to bytes
    /// </summary>
    /// <param name="stream">The stream to read</param>
    /// <returns>Byte array</returns>
    private async Task<byte[]> ReadStreamToBytesAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Processes the Google Cloud Vision response
    /// </summary>
    /// <param name="response">The Google Cloud Vision response</param>
    /// <param name="processingTime">The processing time</param>
    /// <returns>OCR result</returns>
    private OCRResult ProcessGoogleVisionResponse(IReadOnlyList<EntityAnnotation> response, TimeSpan processingTime)
    {
        var textBlocks = new List<TextBlock>();
        var fullText = new List<string>();

        foreach (var annotation in response)
        {
            var textBlock = new TextBlock
            {
                Text = annotation.Description ?? string.Empty,
                Confidence = 0.8, // Google Cloud Vision doesn't provide confidence scores, use default
                BoundingBox = BoundingBox.FromCoordinates(
                    annotation.BoundingPoly?.Vertices?.FirstOrDefault()?.X ?? 0,
                    annotation.BoundingPoly?.Vertices?.FirstOrDefault()?.Y ?? 0,
                    (annotation.BoundingPoly?.Vertices?.LastOrDefault()?.X ?? 0) - (annotation.BoundingPoly?.Vertices?.FirstOrDefault()?.X ?? 0),
                    (annotation.BoundingPoly?.Vertices?.LastOrDefault()?.Y ?? 0) - (annotation.BoundingPoly?.Vertices?.FirstOrDefault()?.Y ?? 0)
                ),
                IsHandwritten = false, // Google Cloud Vision doesn't distinguish between handwritten and printed
                IsPrinted = true,
                Language = annotation.Locale ?? "en",
                ReadingOrder = textBlocks.Count
            };

            textBlocks.Add(textBlock);
            fullText.Add(annotation.Description ?? string.Empty);
        }

        var result = new OCRResult
        {
            Text = string.Join(" ", fullText),
            TextBlocks = textBlocks,
            Confidence = textBlocks.Any() ? textBlocks.Average(tb => tb.Confidence) : 0.0,
            Language = textBlocks.FirstOrDefault()?.Language ?? "en",
            ProcessingTime = processingTime,
            ContainsHandwriting = false,
            ContainsPrintedText = true,
            Metadata = new Dictionary<string, object>
            {
                ["Provider"] = ProviderName,
                ["ModelVersion"] = "latest",
                ["TextBlockCount"] = textBlocks.Count,
                ["LineCount"] = textBlocks.Count,
                ["AnnotationCount"] = response.Count
            }
        };

        return result;
    }
}