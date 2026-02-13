using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Http;
using AMCode.OCR.Models;
using AMCode.OCR.Configurations;
using System.Text.Json;
using System.Text;

namespace AMCode.OCR.Providers;

/// <summary>
/// OpenAI OCR provider implementation using GPT-4o vision capabilities
/// </summary>
public class OpenAIOCRService : IOCRProvider
{
    private readonly HttpClient? _httpClient;
    private readonly ILogger<OpenAIOCRService> _logger;
    private readonly OpenAIOCRConfiguration _config;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Initializes a new instance of the OpenAIOCRService class
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="httpClientFactory">The HTTP client factory</param>
    /// <param name="config">The OpenAI OCR configuration</param>
    public OpenAIOCRService(
        ILogger<OpenAIOCRService> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<OpenAIOCRConfiguration> config)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));

        if (string.IsNullOrEmpty(_config.ApiKey))
        {
            _logger.LogWarning("OpenAI API key not configured. Provider will be marked as unavailable.");
            _httpClient = null;
        }
        else
        {
            _httpClient = httpClientFactory.CreateClient(nameof(OpenAIOCRService));
            _httpClient.BaseAddress = new Uri(_config.BaseUrl);
            _httpClient.Timeout = _config.Timeout;
        }
    }

    /// <summary>
    /// The name of the OCR provider
    /// </summary>
    public string ProviderName => "OpenAI GPT-4o OCR";

    /// <summary>
    /// Whether the provider requires internet connection
    /// </summary>
    public bool RequiresInternet => true;

    /// <summary>
    /// Whether the provider is currently available
    /// </summary>
    public bool IsAvailable => _httpClient != null && !string.IsNullOrEmpty(_config.ApiKey);

    /// <summary>
    /// The capabilities of this OCR provider
    /// </summary>
    public OCRProviderCapabilities Capabilities => new OCRProviderCapabilities
    {
        SupportsLanguageDetection = _config.EnableLanguageDetection,
        SupportsBoundingBoxes = false, // GPT-4o doesn't provide bounding boxes
        SupportsConfidenceScores = false, // GPT-4o doesn't provide confidence scores
        SupportsHandwriting = _config.EnableHandwritingDetection,
        SupportsPrintedText = true,
        SupportsTableDetection = _config.EnableTableDetection,
        SupportsFormDetection = _config.EnableFormDetection,
        MaxImageSizeMB = _config.MaxImageSizeMB,
        SupportedLanguages = _config.SupportedLanguages,
        CostPerRequest = 0, // Cost calculated per token
        AverageResponseTime = _config.AverageResponseTime,
        SupportsCustomModels = false,
        MaxPagesPerRequest = 1,
        SupportsBatchProcessing = false,
        SupportsRealTimeProcessing = true,
        SupportsImagePreprocessing = false,
        SupportsRotationDetection = true,
        SupportsSkewDetection = true
    };

    /// <summary>
    /// Processes an image stream and returns OCR result
    /// </summary>
    public async Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default)
    {
        return await ProcessImageAsync(imageStream, new OCRRequest(), cancellationToken);
    }

    /// <summary>
    /// Processes an image stream with custom options
    /// </summary>
    public async Task<OCRResult> ProcessImageAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default)
    {
        if (_httpClient == null)
        {
            throw new InvalidOperationException("OpenAI client is not available. Please check your API key configuration.");
        }

        if (imageStream == null)
        {
            throw new ArgumentNullException(nameof(imageStream));
        }

        var startTime = DateTime.UtcNow;

        try
        {
            _logger.LogInformation("Processing image with OpenAI GPT-4o OCR");

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

            // Read image bytes and convert to base64
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream, cancellationToken);
                imageBytes = memoryStream.ToArray();
            }

            // Detect image format
            var imageFormat = DetectImageFormat(imageBytes);
            var base64Image = Convert.ToBase64String(imageBytes);

            // Build OCR prompt
            var ocrPrompt = BuildOCRPrompt(options);

            // Create OpenAI API request
            var requestBody = new
            {
                model = _config.Model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = ocrPrompt
                            },
                            new
                            {
                                type = "image_url",
                                image_url = new
                                {
                                    url = $"data:{imageFormat};base64,{base64Image}",
                                    detail = _config.Detail
                                }
                            }
                        }
                    }
                },
                max_tokens = _config.MaxTokens,
                temperature = _config.Temperature
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/v1/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

            // Send request with retry logic
            HttpResponseMessage? response = null;
            Exception? lastException = null;

            for (int attempt = 0; attempt <= _config.MaxRetries; attempt++)
            {
                try
                {
                    if (attempt > 0)
                    {
                        await Task.Delay(_config.RetryDelay, cancellationToken);
                        _logger.LogInformation("Retrying OpenAI OCR request (attempt {Attempt}/{MaxRetries})", attempt + 1, _config.MaxRetries + 1);
                    }

                    response = await _httpClient.SendAsync(requestMessage, cancellationToken);
                    break;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    _logger.LogWarning(ex, "OpenAI OCR request failed (attempt {Attempt}/{MaxRetries})", attempt + 1, _config.MaxRetries + 1);

                    if (attempt == _config.MaxRetries)
                    {
                        throw;
                    }
                }
            }

            if (response == null)
            {
                throw lastException ?? new InvalidOperationException("Failed to get response from OpenAI API");
            }

            var processingTime = DateTime.UtcNow - startTime;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("OpenAI OCR API request failed: {StatusCode} - {Error}", response.StatusCode, errorContent);
                throw new OCRException($"OpenAI OCR API request failed: {response.StatusCode} - {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var openAIResponse = JsonSerializer.Deserialize<OpenAIResponse>(content, _jsonOptions);

            if (openAIResponse?.Choices == null || !openAIResponse.Choices.Any())
            {
                throw new OCRException("Invalid response from OpenAI API - no choices returned");
            }

            // Extract text from response
            var extractedText = openAIResponse.Choices[0].Message?.Content ?? string.Empty;
            var usage = openAIResponse.Usage;

            // Calculate cost based on token usage
            var cost = (usage.PromptTokens * _config.CostPerInputToken) +
                       (usage.CompletionTokens * _config.CostPerOutputToken);

            // Parse text into text blocks (simple line-based parsing)
            var textBlocks = ParseTextIntoBlocks(extractedText, options);

            var result = new OCRResult
            {
                Text = extractedText,
                TextBlocks = textBlocks,
                Confidence = 0.9, // GPT-4o provides high-quality OCR, use high default confidence
                Language = options.ExpectedLanguage ?? "en",
                Provider = ProviderName,
                ProcessingTime = processingTime,
                Cost = cost,
                ContainsHandwriting = _config.EnableHandwritingDetection,
                ContainsPrintedText = true,
                Metadata = new Dictionary<string, object>
                {
                    ["Model"] = _config.Model,
                    ["PromptTokens"] = usage.PromptTokens,
                    ["CompletionTokens"] = usage.CompletionTokens,
                    ["TotalTokens"] = usage.TotalTokens,
                    ["ImageSizeBytes"] = imageSizeBytes,
                    ["ImageSizeMB"] = imageSizeMB
                }
            };

            _logger.LogInformation("OpenAI GPT-4o OCR completed successfully in {ProcessingTime}ms. Cost: ${Cost:F6}, Tokens: {Tokens}",
                processingTime.TotalMilliseconds, cost, usage.TotalTokens);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "OpenAI GPT-4o OCR failed");
            throw new OCRException($"OpenAI GPT-4o OCR processing failed: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Checks the health of the OCR provider
    /// </summary>
    public async Task<OCRProviderHealth> CheckHealthAsync()
    {
        if (_httpClient == null || string.IsNullOrEmpty(_config.ApiKey))
        {
            return new OCRProviderHealth
            {
                IsHealthy = false,
                IsAvailable = false,
                Status = "Client not initialized",
                ResponseTime = TimeSpan.Zero,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = "OpenAI API key not configured",
                SuccessRate = 0.0,
                AverageProcessingTime = TimeSpan.Zero
            };
        }

        try
        {
            var startTime = DateTime.UtcNow;

            // Simple health check with a minimal request
            var requestBody = new
            {
                model = _config.Model,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = "Hello"
                    }
                },
                max_tokens = 10
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/v1/chat/completions")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody, _jsonOptions), Encoding.UTF8, "application/json")
            };

            requestMessage.Headers.Add("Authorization", $"Bearer {_config.ApiKey}");

            var response = await _httpClient.SendAsync(requestMessage, CancellationToken.None);
            var responseTime = DateTime.UtcNow - startTime;

            return new OCRProviderHealth
            {
                IsHealthy = response.IsSuccessStatusCode,
                IsAvailable = true,
                Status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                ResponseTime = responseTime,
                LastChecked = DateTime.UtcNow,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"HTTP {response.StatusCode}",
                SuccessRate = response.IsSuccessStatusCode ? 100.0 : 0.0,
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
    public Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null)
    {
        // Estimate tokens based on image size for GPT-4o vision
        // GPT-4o uses approximately 765 tokens per image tile (512x512)
        // Plus prompt tokens (~50) and response tokens (~500 for OCR text)
        var imageSizeMB = imageSizeBytes / (1024.0 * 1024.0);

        // Estimate number of tiles based on detail level
        var tilesEstimate = _config.Detail == "high"
            ? Math.Max(1, (int)(imageSizeMB * 4)) // High detail uses more tiles
            : 1; // Low detail uses single tile

        var estimatedInputTokens = (tilesEstimate * 765) + 50; // Image tiles + prompt
        var estimatedOutputTokens = 500; // Estimated OCR text output

        var estimatedCost = (estimatedInputTokens * _config.CostPerInputToken) +
                           (estimatedOutputTokens * _config.CostPerOutputToken);

        return Task.FromResult(estimatedCost);
    }

    /// <summary>
    /// Processes multiple images in batch
    /// </summary>
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
    public double GetReliabilityScore()
    {
        // OpenAI GPT-4o is very reliable
        return 0.97;
    }

    /// <summary>
    /// Gets the quality score for this provider
    /// </summary>
    public double GetQualityScore()
    {
        // OpenAI GPT-4o provides excellent OCR quality
        return 0.94;
    }

    /// <summary>
    /// Builds the OCR prompt for GPT-4o
    /// </summary>
    private string BuildOCRPrompt(OCRRequest options)
    {
        var prompt = new StringBuilder();
        prompt.AppendLine("Extract all text from this image with high accuracy. Please provide:");
        prompt.AppendLine("1. All text content EXACTLY as it appears in the image");
        prompt.AppendLine("2. Preserve line breaks and spacing where visible");
        prompt.AppendLine("3. Maintain the reading order (top to bottom, left to right)");

        if (options.RequiresHandwritingSupport || _config.EnableHandwritingDetection)
        {
            prompt.AppendLine("4. Include both printed and handwritten text");
        }

        if (options.RequiresTableDetection || _config.EnableTableDetection)
        {
            prompt.AppendLine("5. If tables are present, preserve their structure");
        }

        if (!string.IsNullOrEmpty(options.ExpectedLanguage))
        {
            prompt.AppendLine($"6. The text is primarily in {options.ExpectedLanguage}");
        }

        prompt.AppendLine("\nCRITICAL OCR ACCURACY RULES:");
        prompt.AppendLine("- Extract ALL text content without omission");
        prompt.AppendLine("- Carefully distinguish between similar-looking characters (1/l/I, 0/O, 2/z, 5/S)");
        prompt.AppendLine("- Expand common abbreviations to their full form when unambiguous");
        prompt.AppendLine("- Use context to resolve ambiguous characters or words");
        prompt.AppendLine("- Preserve the structure of lists, measurements, and multi-part entries");
        prompt.AppendLine("\nReturn only the extracted and expanded text, without any additional commentary or formatting.");

        return prompt.ToString();
    }

    /// <summary>
    /// Detects image format from bytes
    /// </summary>
    private string DetectImageFormat(byte[] imageBytes)
    {
        if (imageBytes.Length < 4)
        {
            return "image/png"; // Default fallback
        }

        // Check magic bytes for common image formats
        if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8 && imageBytes[2] == 0xFF)
        {
            return "image/jpeg";
        }
        else if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
        {
            return "image/png";
        }
        else if (imageBytes[0] == 0x47 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46)
        {
            return "image/gif";
        }
        else if (imageBytes[0] == 0x52 && imageBytes[1] == 0x49 && imageBytes[2] == 0x46 && imageBytes[3] == 0x46)
        {
            return "image/webp";
        }

        return "image/png"; // Default fallback
    }

    /// <summary>
    /// Parses extracted text into text blocks
    /// </summary>
    private List<TextBlock> ParseTextIntoBlocks(string text, OCRRequest options)
    {
        var textBlocks = new List<TextBlock>();
        var lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var textBlock = new TextBlock
            {
                Text = line,
                Confidence = 0.9, // High confidence for GPT-4o OCR
                BoundingBox = BoundingBox.FromCoordinates(0, i * 20, 100, 20), // Placeholder bounding box
                IsHandwritten = false, // GPT-4o doesn't distinguish
                IsPrinted = true,
                Language = options.ExpectedLanguage ?? "en",
                ReadingOrder = i
            };

            textBlocks.Add(textBlock);
        }

        return textBlocks;
    }
}

/// <summary>
/// OpenAI API response model
/// </summary>
internal class OpenAIResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public OpenAIChoice[] Choices { get; set; } = Array.Empty<OpenAIChoice>();
    public OpenAIUsage Usage { get; set; } = new();
}

/// <summary>
/// OpenAI choice model
/// </summary>
internal class OpenAIChoice
{
    public int Index { get; set; }
    public OpenAIMessage? Message { get; set; }
    public string FinishReason { get; set; } = string.Empty;
}

/// <summary>
/// OpenAI message model
/// </summary>
internal class OpenAIMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// OpenAI usage model
/// </summary>
internal class OpenAIUsage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}
