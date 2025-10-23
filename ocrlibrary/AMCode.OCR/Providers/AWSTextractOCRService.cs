using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMCode.OCR.Models;
using AMCode.OCR.Configurations;
using System.Text.Json;

namespace AMCode.OCR.Providers;

/// <summary>
/// AWS Textract OCR provider implementation
/// </summary>
public class AWSTextractOCRService : IOCRProvider
{
    private readonly IAmazonTextract _textractClient;
    private readonly ILogger<AWSTextractOCRService> _logger;
    private readonly AWSTextractConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the AWSTextractOCRService class
    /// </summary>
    /// <param name="textractClient">The AWS Textract client</param>
    /// <param name="logger">The logger</param>
    /// <param name="config">The configuration</param>
    public AWSTextractOCRService(
        IAmazonTextract textractClient,
        ILogger<AWSTextractOCRService> logger,
        IOptions<AWSTextractConfiguration> config)
    {
        _textractClient = textractClient ?? throw new ArgumentNullException(nameof(textractClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// The name of the OCR provider
    /// </summary>
    public string ProviderName => "AWS Textract";

    /// <summary>
    /// Whether the provider requires internet connection
    /// </summary>
    public bool RequiresInternet => true;

    /// <summary>
    /// Whether the provider is currently available
    /// </summary>
    public bool IsAvailable => _textractClient != null;

    /// <summary>
    /// The capabilities of this OCR provider
    /// </summary>
    public OCRProviderCapabilities Capabilities => new OCRProviderCapabilities
    {
        SupportsLanguageDetection = false,
        SupportsBoundingBoxes = true,
        SupportsConfidenceScores = true,
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
            _logger.LogInformation("Processing image with AWS Textract");

            var startTime = DateTime.UtcNow;
            var imageBytes = await ReadStreamToBytesAsync(imageStream);
            
            var request = new DetectDocumentTextRequest
            {
                Document = new Document
                {
                    Bytes = new MemoryStream(imageBytes)
                }
            };

            var response = await _textractClient.DetectDocumentTextAsync(request, cancellationToken);
            var processingTime = DateTime.UtcNow - startTime;

            var result = ProcessTextractResponse(response, processingTime);
            result.Provider = ProviderName;
            result.Cost = _config.CostPerRequest;
            
            _logger.LogInformation("AWS Textract OCR completed successfully in {ProcessingTime}ms", 
                processingTime.TotalMilliseconds);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AWS Textract OCR failed");
            throw new OCRException($"AWS Textract processing failed: {ex.Message}", ex);
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
            
            // Try to detect document text with a minimal request
            var request = new DetectDocumentTextRequest
            {
                Document = new Document
                {
                    Bytes = new MemoryStream(new byte[0])
                }
            };
            
            var response = await _textractClient.DetectDocumentTextAsync(request, CancellationToken.None);
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
        // AWS Textract charges per request, not by image size
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

        if (options.RequiresLanguageDetection)
            return false; // AWS Textract doesn't support language detection

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
        var additionalTime = TimeSpan.FromMilliseconds(sizeFactor * 150); // 150ms per MB
        
        return baseTime.Add(additionalTime);
    }

    /// <summary>
    /// Gets the reliability score for this provider
    /// </summary>
    /// <returns>Reliability score (0.0 to 1.0)</returns>
    public double GetReliabilityScore()
    {
        // AWS Textract is generally very reliable
        return 0.92;
    }

    /// <summary>
    /// Gets the quality score for this provider
    /// </summary>
    /// <returns>Quality score (0.0 to 1.0)</returns>
    public double GetQualityScore()
    {
        // AWS Textract provides high-quality OCR results, especially for forms and tables
        return 0.88;
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
    /// Processes the AWS Textract response
    /// </summary>
    /// <param name="response">The AWS Textract response</param>
    /// <param name="processingTime">The processing time</param>
    /// <returns>OCR result</returns>
    private OCRResult ProcessTextractResponse(DetectDocumentTextResponse response, TimeSpan processingTime)
    {
        var textBlocks = new List<TextBlock>();
        var fullText = new List<string>();

        foreach (var block in response.Blocks)
        {
            if (block.BlockType == BlockType.LINE)
            {
                var textBlock = new TextBlock
                {
                    Text = block.Text ?? string.Empty,
                    Confidence = (double)block.Confidence,
                    BoundingBox = AMCode.OCR.Models.BoundingBox.FromCoordinates(
                        block.Geometry?.BoundingBox?.Left ?? 0,
                        block.Geometry?.BoundingBox?.Top ?? 0,
                        block.Geometry?.BoundingBox?.Width ?? 0,
                        block.Geometry?.BoundingBox?.Height ?? 0
                    ),
                    IsHandwritten = false, // AWS Textract doesn't distinguish between handwritten and printed
                    IsPrinted = true,
                    Language = "en", // AWS Textract doesn't provide language detection
                    ReadingOrder = textBlocks.Count
                };

                textBlocks.Add(textBlock);
                fullText.Add(block.Text ?? string.Empty);
            }
        }

        var result = new OCRResult
        {
            Text = string.Join(" ", fullText),
            TextBlocks = textBlocks,
            Confidence = textBlocks.Any() ? textBlocks.Average(tb => tb.Confidence) : 0.0,
            Language = "en", // AWS Textract doesn't provide language detection
            ProcessingTime = processingTime,
            ContainsHandwriting = false,
            ContainsPrintedText = true,
            Metadata = new Dictionary<string, object>
            {
                ["Provider"] = ProviderName,
                ["ModelVersion"] = "latest",
                ["TextBlockCount"] = textBlocks.Count,
                ["LineCount"] = textBlocks.Count,
                ["BlockCount"] = response.Blocks.Count
            }
        };

        return result;
    }
}