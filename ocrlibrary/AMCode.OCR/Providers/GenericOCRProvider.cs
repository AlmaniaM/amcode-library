using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using AMCode.OCR.Models;
using System.Text.Json;

namespace AMCode.OCR.Providers;

/// <summary>
/// Generic base class for OCR providers
/// </summary>
public abstract class GenericOCRProvider : IOCRProvider
{
    protected readonly ILogger _logger;
    protected readonly IHttpClientFactory _httpClientFactory;
    protected readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the GenericOCRProvider class
    /// </summary>
    /// <param name="logger">The logger</param>
    /// <param name="httpClientFactory">The HTTP client factory</param>
    public GenericOCRProvider(
        ILogger logger,
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    /// <summary>
    /// The name of the OCR provider
    /// </summary>
    public abstract string ProviderName { get; }

    /// <summary>
    /// Whether the provider requires internet connection
    /// </summary>
    public abstract bool RequiresInternet { get; }

    /// <summary>
    /// The capabilities of this OCR provider
    /// </summary>
    public abstract OCRProviderCapabilities Capabilities { get; }

    /// <summary>
    /// Whether the provider is currently available
    /// </summary>
    public bool IsAvailable => CheckAvailability();

    /// <summary>
    /// Processes an image stream and returns OCR result
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public abstract Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default);

    /// <summary>
    /// Processes an image stream with custom options
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public abstract Task<OCRResult> ProcessImageAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks the health of the OCR provider
    /// </summary>
    /// <returns>Health status information</returns>
    public abstract Task<OCRProviderHealth> CheckHealthAsync();

    /// <summary>
    /// Gets the cost estimate for processing an image
    /// </summary>
    /// <param name="imageSizeBytes">Size of the image in bytes</param>
    /// <param name="options">OCR processing options</param>
    /// <returns>Estimated cost in USD</returns>
    public abstract Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null);

    /// <summary>
    /// Processes multiple images in batch
    /// </summary>
    /// <param name="imageStreams">Collection of image streams to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of OCR results</returns>
    public virtual async Task<IEnumerable<OCRResult>> ProcessBatchAsync(
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
                _logger.LogError(ex, "Failed to process image in batch with provider {Provider}", ProviderName);
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
    public virtual bool CanProcess(OCRRequest options)
    {
        if (options.ImageStream == null && string.IsNullOrEmpty(options.ImagePath))
            return false;

        if (options.MaxImageSizeMB > Capabilities.MaxImageSizeMB)
            return false;

        if (options.RequiresLanguageDetection && !Capabilities.SupportsLanguageDetection)
            return false;

        if (options.RequiresHandwritingSupport && !Capabilities.SupportsHandwriting)
            return false;

        if (options.RequiresTableDetection && !Capabilities.SupportsTableDetection)
            return false;

        if (options.RequiresFormDetection && !Capabilities.SupportsFormDetection)
            return false;

        return true;
    }

    /// <summary>
    /// Gets the estimated processing time for an image
    /// </summary>
    /// <param name="imageSizeBytes">Size of the image in bytes</param>
    /// <param name="options">OCR processing options</param>
    /// <returns>Estimated processing time</returns>
    public virtual TimeSpan GetEstimatedProcessingTime(long imageSizeBytes, OCRRequest? options = null)
    {
        // Base processing time plus additional time based on image size
        var baseTime = Capabilities.AverageResponseTime;
        var sizeFactor = Math.Max(1.0, imageSizeBytes / (1024.0 * 1024.0)); // Convert to MB
        var additionalTime = TimeSpan.FromMilliseconds(sizeFactor * 100); // 100ms per MB
        
        return baseTime.Add(additionalTime);
    }

    /// <summary>
    /// Gets the reliability score for this provider
    /// </summary>
    /// <returns>Reliability score (0.0 to 1.0)</returns>
    public virtual double GetReliabilityScore()
    {
        // Default reliability score, can be overridden by derived classes
        return 0.8;
    }

    /// <summary>
    /// Gets the quality score for this provider
    /// </summary>
    /// <returns>Quality score (0.0 to 1.0)</returns>
    public virtual double GetQualityScore()
    {
        // Default quality score, can be overridden by derived classes
        return 0.8;
    }

    /// <summary>
    /// Checks if the provider is available
    /// </summary>
    /// <returns>True if available, false otherwise</returns>
    protected abstract bool CheckAvailability();

    /// <summary>
    /// Gets an HTTP client for the provider
    /// </summary>
    /// <returns>HTTP client</returns>
    protected virtual async Task<HttpClient> GetHttpClientAsync()
    {
        var client = _httpClientFactory.CreateClient(ProviderName);
        await ConfigureHttpClientAsync(client);
        return client;
    }

    /// <summary>
    /// Configures the HTTP client for the provider
    /// </summary>
    /// <param name="client">The HTTP client to configure</param>
    /// <returns>Task representing the configuration</returns>
    protected virtual Task ConfigureHttpClientAsync(HttpClient client)
    {
        // Override in derived classes to configure headers, timeouts, etc.
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deserializes a JSON response
    /// </summary>
    /// <typeparam name="T">The type to deserialize to</typeparam>
    /// <param name="response">The HTTP response message</param>
    /// <returns>The deserialized object</returns>
    protected virtual async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _jsonOptions) ?? throw new InvalidOperationException("Failed to deserialize response");
    }

    /// <summary>
    /// Reads a stream to bytes
    /// </summary>
    /// <param name="stream">The stream to read</param>
    /// <returns>Byte array</returns>
    protected virtual async Task<byte[]> ReadStreamToBytesAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Calculates an estimated confidence score for text blocks
    /// </summary>
    /// <param name="textBlocks">The text blocks to analyze</param>
    /// <returns>Estimated confidence score</returns>
    protected virtual double CalculateEstimatedConfidence(IEnumerable<TextBlock> textBlocks)
    {
        if (!textBlocks.Any())
            return 0.0;

        // Simple heuristic: longer text blocks are more likely to be accurate
        var totalLength = textBlocks.Sum(tb => tb.Text.Length);
        var averageLength = totalLength / (double)textBlocks.Count();
        
        // Base confidence on average text length (longer = more confident)
        var baseConfidence = Math.Min(0.9, averageLength / 10.0);
        
        return Math.Max(0.1, baseConfidence);
    }

    /// <summary>
    /// Detects the language of text
    /// </summary>
    /// <param name="text">The text to analyze</param>
    /// <returns>Detected language code</returns>
    protected virtual string DetectLanguage(string text)
    {
        // Simple language detection based on common patterns
        if (string.IsNullOrWhiteSpace(text))
            return "en";

        var lowerText = text.ToLower();
        
        // Check for common English words
        var englishWords = new[] { "the", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with", "by" };
        var englishCount = englishWords.Count(word => lowerText.Contains(word));
        
        // Check for common Spanish words
        var spanishWords = new[] { "el", "la", "de", "que", "y", "a", "en", "un", "es", "se", "no", "te", "lo", "le", "da", "su", "por", "son", "con", "para", "al", "del", "los", "las", "una", "uno", "sobre", "entre", "hasta", "desde", "durante", "mediante", "según", "sin", "so", "bajo", "ante", "tras", "contra", "hacia", "hasta", "durante", "mediante", "según", "sin", "so", "bajo", "ante", "tras", "contra", "hacia" };
        var spanishCount = spanishWords.Count(word => lowerText.Contains(word));
        
        // Check for common French words
        var frenchWords = new[] { "le", "la", "de", "que", "et", "à", "en", "un", "est", "se", "ne", "te", "lo", "le", "da", "su", "par", "son", "con", "pour", "al", "del", "les", "las", "une", "un", "sur", "entre", "jusqu", "depuis", "pendant", "par", "selon", "sans", "so", "sous", "avant", "après", "contre", "vers", "jusqu", "pendant", "par", "selon", "sans", "so", "sous", "avant", "après", "contre", "vers" };
        var frenchCount = frenchWords.Count(word => lowerText.Contains(word));
        
        // Return the language with the most matches
        if (spanishCount > englishCount && spanishCount > frenchCount)
            return "es";
        if (frenchCount > englishCount && frenchCount > spanishCount)
            return "fr";
        
        return "en"; // Default to English
    }

    /// <summary>
    /// Creates a basic OCR result
    /// </summary>
    /// <param name="text">The extracted text</param>
    /// <param name="processingTime">The processing time</param>
    /// <param name="textBlocks">The text blocks</param>
    /// <returns>OCR result</returns>
    protected virtual OCRResult CreateOCRResult(string text, TimeSpan processingTime, List<TextBlock>? textBlocks = null)
    {
        return new OCRResult
        {
            Text = text,
            TextBlocks = textBlocks ?? new List<TextBlock>(),
            Confidence = textBlocks?.Any() == true ? textBlocks.Average(tb => tb.Confidence) : 0.8,
            Language = DetectLanguage(text),
            ProcessingTime = processingTime,
            ContainsHandwriting = false,
            ContainsPrintedText = true,
            Provider = ProviderName,
            Cost = Capabilities.CostPerRequest,
            Metadata = new Dictionary<string, object>
            {
                ["Provider"] = ProviderName,
                ["TextBlockCount"] = textBlocks?.Count ?? 0,
                ["LineCount"] = textBlocks?.Count ?? 0
            }
        };
    }
}
