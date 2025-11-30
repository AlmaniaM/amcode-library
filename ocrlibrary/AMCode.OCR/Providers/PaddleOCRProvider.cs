using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using AMCode.OCR.Models;
using AMCode.OCR.Configurations;
using System;
using System.Text.Json;
using System.Net.Http.Headers;
using System.IO;

namespace AMCode.OCR.Providers;

/// <summary>
/// PaddleOCR provider implementation that calls Python OCR service via HTTP
/// </summary>
public class PaddleOCRProvider : GenericOCRProvider
{
    private readonly PaddleOCRConfiguration _config;
    private bool? _availabilityCache;
    private DateTime _lastAvailabilityCheck = DateTime.MinValue;
    private static readonly TimeSpan AvailabilityCacheTimeout = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Initializes a new instance of the PaddleOCRProvider class
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="httpClientFactory">The HTTP client factory</param>
    /// <param name="config">The PaddleOCR configuration</param>
    public PaddleOCRProvider(
        ILogger<PaddleOCRProvider> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<PaddleOCRConfiguration> config)
        : base(logger, httpClientFactory)
    {
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// The name of the OCR provider
    /// </summary>
    public override string ProviderName => "PaddleOCR";

    /// <summary>
    /// Whether the provider requires internet connection
    /// </summary>
    public override bool RequiresInternet => true;

    /// <summary>
    /// The capabilities of this OCR provider
    /// </summary>
    public override OCRProviderCapabilities Capabilities => new OCRProviderCapabilities
    {
        SupportsLanguageDetection = _config.EnableLanguageDetection,
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
        SupportsBatchProcessing = true,
        SupportsRealTimeProcessing = true,
        SupportsImagePreprocessing = false,
        SupportsRotationDetection = true, // PaddleOCR supports angle classification
        SupportsSkewDetection = true
    };

    /// <summary>
    /// Processes an image stream and returns OCR result
    /// </summary>
    public override Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default)
    {
        return ProcessImageAsync(imageStream, new OCRRequest(), cancellationToken);
    }

    /// <summary>
    /// Processes an image stream with custom options
    /// </summary>
    public override async Task<OCRResult> ProcessImageAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default)
    {
        if (imageStream == null)
            throw new ArgumentNullException(nameof(imageStream));

        var startTime = DateTime.UtcNow;

        try
        {
            _logger.LogInformation("Processing image with PaddleOCR service at {ServiceUrl}", _config.ServiceUrl);

            // Validate image size
            var imageSizeBytes = imageStream.Length;
            var imageSizeMB = imageSizeBytes / (1024.0 * 1024.0);
            if (imageSizeMB > _config.MaxImageSizeMB)
            {
                throw new ArgumentException($"Image size ({imageSizeMB:F2} MB) exceeds maximum allowed size ({_config.MaxImageSizeMB} MB)");
            }

            // Reset stream position if needed
            if (imageStream.CanSeek && imageStream.Position > 0)
            {
                imageStream.Position = 0;
            }

            // Detect image format from stream magic bytes
            var (contentType, fileExtension) = DetectImageFormat(imageStream);

            // Reset stream position again after detection
            if (imageStream.CanSeek)
            {
                imageStream.Position = 0;
            }

            // Get HTTP client
            _logger.LogInformation("Getting HttpClient for PaddleOCR provider...");
            var client = await GetHttpClientAsync();
            _logger.LogInformation("HttpClient obtained. BaseAddress: {BaseAddress}, Timeout: {Timeout}",
                client.BaseAddress, client.Timeout);

            if (client.BaseAddress == null)
            {
                _logger.LogError("HttpClient BaseAddress is null! ServiceUrl: {ServiceUrl}", _config.ServiceUrl);
                throw new InvalidOperationException($"HttpClient BaseAddress is null. ServiceUrl configuration: {_config.ServiceUrl}");
            }

            // Use alternative /ocr-bytes endpoint by default (simpler, avoids multipart parsing issues)
            // Can be disabled by setting PADDLEOCR_USE_BYTES_ENDPOINT=false
            // The bytes endpoint is more reliable as it avoids multipart/form-data parsing complexity
            var useBytesEndpoint = Environment.GetEnvironmentVariable("PADDLEOCR_USE_BYTES_ENDPOINT")?.ToLower() != "false";

            HttpResponseMessage response;
            if (useBytesEndpoint)
            {
                // Use /ocr-bytes endpoint - send raw bytes with Content-Type header
                _logger.LogInformation("Using /ocr-bytes endpoint (raw bytes)");
                var ocrUrl = "/ocr-bytes";
                var fullUrl = $"{client.BaseAddress}ocr-bytes";

                // Read stream to bytes
                // Reset stream position if needed
                if (imageStream.CanSeek && imageStream.Position > 0)
                {
                    imageStream.Position = 0;
                }

                byte[] imageBytes;
                if (imageStream is MemoryStream ms && ms.CanRead)
                {
                    imageBytes = ms.ToArray();
                }
                else
                {
                    using var memoryStream = new MemoryStream();
                    await imageStream.CopyToAsync(memoryStream, cancellationToken);
                    imageBytes = memoryStream.ToArray();
                }

                _logger.LogInformation("Sending POST request to {OcrUrl} (full URL: {FullUrl})",
                    ocrUrl, fullUrl);
                _logger.LogInformation("Request details: ContentType={ContentType}, Size={Size} bytes",
                    contentType, imageBytes.Length);

                using var byteContent = new ByteArrayContent(imageBytes);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                _logger.LogInformation("Calling HttpClient.PostAsync with raw bytes...");
                var requestStartTime = DateTime.UtcNow;
                response = await client.PostAsync(fullUrl, byteContent, cancellationToken);
                var requestDuration = DateTime.UtcNow - requestStartTime;
                _logger.LogInformation("Received response: StatusCode={StatusCode}, Duration={Duration}ms",
                    response.StatusCode, requestDuration.TotalMilliseconds);
            }
            else
            {
                // Use /ocr endpoint - multipart/form-data (original approach)
                _logger.LogInformation("Using /ocr endpoint (multipart/form-data)");
                using var formData = new MultipartFormDataContent();
                using var imageContent = new StreamContent(imageStream);
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                formData.Add(imageContent, "image", $"image.{fileExtension}");

                // Send request to Python service
                // Use relative URL since BaseAddress is already set
                var ocrUrl = "/ocr";
                var fullUrl = $"{client.BaseAddress}{ocrUrl}";
                _logger.LogInformation("Sending POST request to {OcrUrl} (full URL: {FullUrl})",
                    ocrUrl, fullUrl);
                _logger.LogInformation("Request details: ContentType={ContentType}, Filename=image.{Extension}, StreamLength={StreamLength}",
                    contentType, fileExtension, imageStream.Length);

                _logger.LogInformation("Calling HttpClient.PostAsync...");
                var requestStartTime = DateTime.UtcNow;
                response = await client.PostAsync(ocrUrl, formData, cancellationToken);
                var requestDuration = DateTime.UtcNow - requestStartTime;
                _logger.LogInformation("Received response: StatusCode={StatusCode}, Duration={Duration}ms",
                    response.StatusCode, requestDuration.TotalMilliseconds);
            }

            var processingTime = DateTime.UtcNow - startTime;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("PaddleOCR service returned error: {StatusCode} - {Error}", response.StatusCode, errorContent);

                return new OCRResult
                {
                    Provider = ProviderName,
                    Error = $"HTTP {response.StatusCode}: {errorContent}",
                    ProcessingTime = processingTime,
                    ProcessedAt = DateTime.UtcNow,
                    Cost = _config.CostPerRequest
                };
            }

            // Deserialize response
            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var pythonResponse = JsonSerializer.Deserialize<PythonOCRResponse>(jsonContent, _jsonOptions);

            if (pythonResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize PaddleOCR response");
            }

            // Map Python response to OCRResult
            var result = MapPythonResponseToOCRResult(pythonResponse, processingTime);

            _logger.LogInformation("PaddleOCR processing completed successfully in {ProcessingTime}ms",
                processingTime.TotalMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PaddleOCR processing failed");
            var processingTime = DateTime.UtcNow - startTime;

            return new OCRResult
            {
                Provider = ProviderName,
                Error = ex.Message,
                ProcessingTime = processingTime,
                ProcessedAt = DateTime.UtcNow,
                Cost = _config.CostPerRequest
            };
        }
    }

    /// <summary>
    /// Checks the health of the OCR provider
    /// </summary>
    public override async Task<OCRProviderHealth> CheckHealthAsync()
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var client = await GetHttpClientAsync();
            var healthUrl = $"{_config.ServiceUrl.TrimEnd('/')}/health";
            var response = await client.GetAsync(healthUrl);

            var responseTime = DateTime.UtcNow - startTime;
            var isHealthy = response.IsSuccessStatusCode;

            return new OCRProviderHealth
            {
                IsHealthy = isHealthy,
                IsAvailable = isHealthy,
                Status = isHealthy ? "Healthy" : $"Unhealthy (HTTP {response.StatusCode})",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = isHealthy ? string.Empty : await response.Content.ReadAsStringAsync(),
                Metrics = new Dictionary<string, object>
                {
                    ["Provider"] = ProviderName,
                    ["ServiceUrl"] = _config.ServiceUrl,
                    ["StatusCode"] = (int)response.StatusCode
                }
            };
        }
        catch (Exception ex)
        {
            var responseTime = DateTime.UtcNow - startTime;
            _logger.LogError(ex, "Health check failed for PaddleOCR provider");

            return new OCRProviderHealth
            {
                IsHealthy = false,
                IsAvailable = false,
                Status = "Unhealthy",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = ex.Message,
                Metrics = new Dictionary<string, object>
                {
                    ["Provider"] = ProviderName,
                    ["ServiceUrl"] = _config.ServiceUrl,
                    ["Error"] = ex.GetType().Name
                }
            };
        }
    }

    /// <summary>
    /// Gets the cost estimate for processing an image
    /// </summary>
    public override Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null)
    {
        // PaddleOCR is CPU-based and free
        return Task.FromResult(_config.CostPerRequest);
    }

    /// <summary>
    /// Gets the estimated processing time for an image
    /// </summary>
    public override TimeSpan GetEstimatedProcessingTime(long imageSizeBytes, OCRRequest? options = null)
    {
        // Base time plus size factor
        var baseTime = _config.AverageResponseTime;
        var sizeFactor = Math.Max(1.0, imageSizeBytes / (1024.0 * 1024.0)); // Convert to MB
        var additionalTime = TimeSpan.FromMilliseconds(sizeFactor * 200); // 200ms per MB

        return baseTime.Add(additionalTime);
    }

    /// <summary>
    /// Gets the reliability score for this provider
    /// </summary>
    public override double GetReliabilityScore()
    {
        return 0.85; // High reliability for local/self-hosted service
    }

    /// <summary>
    /// Gets the quality score for this provider
    /// </summary>
    public override double GetQualityScore()
    {
        return 0.90; // Excellent quality, especially for handwriting
    }

    /// <summary>
    /// Checks if the provider is available
    /// </summary>
    protected override bool CheckAvailability()
    {
        // Cache availability check for 1 minute to avoid excessive health checks
        if (_availabilityCache.HasValue &&
            DateTime.UtcNow - _lastAvailabilityCheck < AvailabilityCacheTimeout)
        {
            return _availabilityCache.Value;
        }

        try
        {
            // Quick synchronous check - if service URL is configured, assume available
            // Full health check is done asynchronously via CheckHealthAsync
            var isAvailable = !string.IsNullOrWhiteSpace(_config.ServiceUrl);

            if (isAvailable)
            {
                _logger.LogDebug("PaddleOCR provider is available. Service URL: {ServiceUrl}", _config.ServiceUrl);
            }
            else
            {
                _logger.LogWarning("PaddleOCR provider is not available. ServiceUrl is not configured.");
            }

            _availabilityCache = isAvailable;
            _lastAvailabilityCheck = DateTime.UtcNow;
            return isAvailable;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking PaddleOCR availability");
            _availabilityCache = false;
            _lastAvailabilityCheck = DateTime.UtcNow;
            return false;
        }
    }

    /// <summary>
    /// Configures the HTTP client for the provider
    /// </summary>
    protected override Task ConfigureHttpClientAsync(HttpClient client)
    {
        client.BaseAddress = new Uri(_config.ServiceUrl);
        client.Timeout = _config.Timeout;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Detects image format from stream magic bytes
    /// </summary>
    private (string contentType, string extension) DetectImageFormat(Stream stream)
    {
        if (!stream.CanSeek || stream.Length < 4)
        {
            // Default to JPEG if we can't detect
            return ("image/jpeg", "jpg");
        }

        var originalPosition = stream.Position;
        try
        {
            stream.Position = 0;
            var header = new byte[12];
            var bytesRead = stream.Read(header, 0, Math.Min(header.Length, (int)stream.Length));

            if (bytesRead < 4)
            {
                return ("image/jpeg", "jpg");
            }

            // Check magic bytes for different image formats
            // JPEG: FF D8 FF
            if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
            {
                return ("image/jpeg", "jpg");
            }

            // PNG: 89 50 4E 47 0D 0A 1A 0A
            if (bytesRead >= 8 && header[0] == 0x89 && header[1] == 0x50 &&
                header[2] == 0x4E && header[3] == 0x47 && header[4] == 0x0D &&
                header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A)
            {
                return ("image/png", "png");
            }

            // BMP: 42 4D (BM)
            if (header[0] == 0x42 && header[1] == 0x4D)
            {
                return ("image/bmp", "bmp");
            }

            // WebP: RIFF...WEBP
            if (bytesRead >= 12 && header[0] == 0x52 && header[1] == 0x49 &&
                header[2] == 0x46 && header[3] == 0x46 && header[8] == 0x57 &&
                header[9] == 0x45 && header[10] == 0x42 && header[11] == 0x50)
            {
                return ("image/webp", "webp");
            }

            // GIF: 47 49 46 38 (GIF8)
            if (bytesRead >= 4 && header[0] == 0x47 && header[1] == 0x49 &&
                header[2] == 0x46 && header[3] == 0x38)
            {
                return ("image/gif", "gif");
            }

            // Default to JPEG if format is unknown
            _logger.LogWarning("Could not detect image format from magic bytes, defaulting to JPEG");
            return ("image/jpeg", "jpg");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error detecting image format, defaulting to JPEG");
            return ("image/jpeg", "jpg");
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = originalPosition;
            }
        }
    }

    /// <summary>
    /// Maps Python OCR response to .NET OCRResult
    /// </summary>
    private OCRResult MapPythonResponseToOCRResult(PythonOCRResponse pythonResponse, TimeSpan processingTime)
    {
        // Parse processing time string (HH:MM:SS.fff format)
        TimeSpan parsedProcessingTime = TimeSpan.Zero;
        if (!string.IsNullOrEmpty(pythonResponse.ProcessingTime))
        {
            if (TimeSpan.TryParse(pythonResponse.ProcessingTime, out var parsed))
            {
                parsedProcessingTime = parsed;
            }
        }

        // Map text blocks
        var textBlocks = pythonResponse.TextBlocks?.Select(tb => new TextBlock
        {
            Text = tb.Text ?? string.Empty,
            Confidence = tb.Confidence,
            BoundingBox = new BoundingBox
            {
                X = tb.BoundingBox?.X ?? 0,
                Y = tb.BoundingBox?.Y ?? 0,
                Width = tb.BoundingBox?.Width ?? 0,
                Height = tb.BoundingBox?.Height ?? 0
            },
            IsHandwritten = tb.IsHandwritten ?? false,
            IsPrinted = tb.IsPrinted ?? false,
            Language = tb.Language ?? pythonResponse.Language ?? "en",
            Properties = tb.Properties ?? new Dictionary<string, object>()
        }).ToList() ?? new List<TextBlock>();

        // Calculate overall confidence if not provided
        var overallConfidence = pythonResponse.Confidence;
        if (overallConfidence == 0 && textBlocks.Any())
        {
            overallConfidence = textBlocks.Average(tb => tb.Confidence);
        }

        // Detect handwriting and printed text
        var containsHandwriting = pythonResponse.ContainsHandwriting ?? textBlocks.Any(tb => tb.IsHandwritten);
        var containsPrintedText = pythonResponse.ContainsPrintedText ?? textBlocks.Any(tb => tb.IsPrinted);

        return new OCRResult
        {
            Text = pythonResponse.Text ?? string.Empty,
            TextBlocks = textBlocks,
            Confidence = overallConfidence,
            Language = pythonResponse.Language ?? "en",
            Provider = ProviderName,
            ProcessingTime = parsedProcessingTime != TimeSpan.Zero ? parsedProcessingTime : processingTime,
            Cost = _config.CostPerRequest,
            ProcessedAt = DateTime.UtcNow,
            ContainsHandwriting = containsHandwriting,
            ContainsPrintedText = containsPrintedText,
            Metadata = new Dictionary<string, object>
            {
                ["Provider"] = ProviderName,
                ["ServiceUrl"] = _config.ServiceUrl,
                ["TextBlockCount"] = textBlocks.Count,
                ["ProcessingTimeString"] = pythonResponse.ProcessingTime ?? string.Empty
            },
            Error = pythonResponse.Error ?? string.Empty
        };
    }

    /// <summary>
    /// Python OCR service response model (matches Python Pydantic models)
    /// </summary>
    private class PythonOCRResponse
    {
        public string? Text { get; set; }
        public List<PythonTextBlock>? TextBlocks { get; set; }
        public double Confidence { get; set; }
        public string? Language { get; set; }
        public string? ProcessingTime { get; set; }
        public bool? ContainsHandwriting { get; set; }
        public bool? ContainsPrintedText { get; set; }
        public string? Error { get; set; }
    }

    /// <summary>
    /// Python text block model
    /// </summary>
    private class PythonTextBlock
    {
        public string? Text { get; set; }
        public double Confidence { get; set; }
        public PythonBoundingBox? BoundingBox { get; set; }
        public bool? IsHandwritten { get; set; }
        public bool? IsPrinted { get; set; }
        public string? Language { get; set; }
        public Dictionary<string, object>? Properties { get; set; }
    }

    /// <summary>
    /// Python bounding box model
    /// </summary>
    private class PythonBoundingBox
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
    }
}
