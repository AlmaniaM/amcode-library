using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMCode.OCR.Models;
using AMCode.OCR.Configurations;
using AMCode.OCR.Enums;

namespace AMCode.OCR.Services;

/// <summary>
/// Enhanced hybrid OCR service implementation
/// </summary>
public class EnhancedHybridOCRService : IOCRService
{
    private readonly IEnumerable<IOCRProvider> _providers;
    private readonly IOCRProviderSelector _providerSelector;
    private readonly ILogger<EnhancedHybridOCRService> _logger;
    private readonly OCRConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the EnhancedHybridOCRService class
    /// </summary>
    /// <param name="providers">The available OCR providers</param>
    /// <param name="providerSelector">The provider selector</param>
    /// <param name="logger">The logger</param>
    /// <param name="config">The configuration</param>
    public EnhancedHybridOCRService(
        IEnumerable<IOCRProvider> providers,
        IOCRProviderSelector providerSelector,
        ILogger<EnhancedHybridOCRService> logger,
        IOptions<OCRConfiguration> config)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _providerSelector = providerSelector ?? throw new ArgumentNullException(nameof(providerSelector));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }

    /// <summary>
    /// Extracts text from an image stream
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public async Task<Result<OCRResult>> ExtractTextAsync(Stream imageStream, CancellationToken cancellationToken = default)
    {
        return await ExtractTextAsync(imageStream, new OCRRequest(), cancellationToken);
    }

    /// <summary>
    /// Extracts text from an image file
    /// </summary>
    /// <param name="imagePath">The path to the image file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public async Task<Result<OCRResult>> ExtractTextAsync(string imagePath, CancellationToken cancellationToken = default)
    {
        return await ExtractTextAsync(imagePath, new OCRRequest(), cancellationToken);
    }

    /// <summary>
    /// Extracts text from an image with custom options
    /// </summary>
    /// <param name="imageStream">The image stream to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public async Task<Result<OCRResult>> ExtractTextAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting enhanced hybrid OCR processing");

        try
        {
            // Validate the request
            if (imageStream == null)
            {
                return Result.Failure<OCRResult>("Image stream cannot be null");
            }

            if (imageStream.Length == 0)
            {
                return Result.Failure<OCRResult>("Image stream cannot be empty");
            }

            // Set default options if not provided
            var requestOptions = options ?? new OCRRequest();
            requestOptions.ImageStream = imageStream;
            requestOptions.ConfidenceThreshold = requestOptions.ConfidenceThreshold > 0 ? requestOptions.ConfidenceThreshold : _config.DefaultConfidenceThreshold;
            requestOptions.MaxRetries = requestOptions.MaxRetries > 0 ? requestOptions.MaxRetries : _config.MaxRetries;

            // Try primary provider first
            try
            {
                var primaryProvider = await _providerSelector.SelectBestProviderAsync(requestOptions);
                var result = await ProcessWithProvider(primaryProvider, requestOptions, cancellationToken);

                if (result.Confidence >= requestOptions.ConfidenceThreshold)
                {
                    _logger.LogInformation("OCR successful with primary provider: {Provider}", primaryProvider.ProviderName);
                    return Result.Success(result);
                }
                else
                {
                    _logger.LogWarning("Primary provider {Provider} returned low confidence: {Confidence}", 
                        primaryProvider.ProviderName, result.Confidence);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Primary provider failed, trying fallback providers");
            }

            // Try fallback providers if enabled
            if (_config.EnableFallbackProviders)
            {
                var fallbackProviders = await _providerSelector.GetAvailableProvidersAsync();
                var fallbackCount = 0;

                foreach (var provider in fallbackProviders)
                {
                    if (fallbackCount >= _config.MaxFallbackProviders)
                        break;

                    try
                    {
                        var result = await ProcessWithProvider(provider, requestOptions, cancellationToken);

                        if (result.Confidence >= requestOptions.ConfidenceThreshold)
                        {
                            _logger.LogInformation("OCR successful with fallback provider: {Provider}", provider.ProviderName);
                            return Result.Success(result);
                        }
                        else
                        {
                            _logger.LogWarning("Fallback provider {Provider} returned low confidence: {Confidence}", 
                                provider.ProviderName, result.Confidence);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Fallback provider {Provider} failed", provider.ProviderName);
                    }

                    fallbackCount++;
                }
            }

            return Result.Failure<OCRResult>("All OCR providers failed or returned low confidence results");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Enhanced hybrid OCR processing failed");
            return Result.Failure<OCRResult>($"OCR processing failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Extracts text from an image file with custom options
    /// </summary>
    /// <param name="imagePath">The path to the image file</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result containing extracted text and metadata</returns>
    public async Task<Result<OCRResult>> ExtractTextAsync(string imagePath, OCRRequest options, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return Result.Failure<OCRResult>("Image path cannot be null or empty");
        }

        if (!File.Exists(imagePath))
        {
            return Result.Failure<OCRResult>($"Image file not found: {imagePath}");
        }

        try
        {
            using var stream = File.OpenRead(imagePath);
            return await ExtractTextAsync(stream, options, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read image file: {ImagePath}", imagePath);
            return Result.Failure<OCRResult>($"Failed to read image file: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks if the OCR service is available
    /// </summary>
    /// <returns>True if the service is available, false otherwise</returns>
    public async Task<bool> IsAvailableAsync()
    {
        try
        {
            var availableProviders = await _providerSelector.GetAvailableProvidersAsync();
            return availableProviders.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check OCR service availability");
            return false;
        }
    }

    /// <summary>
    /// Gets the health status of the OCR service
    /// </summary>
    /// <returns>Health status information</returns>
    public async Task<OCRProviderHealth> GetHealthAsync()
    {
        try
        {
            var availableProviders = await _providerSelector.GetAvailableProvidersAsync();
            var providerHealths = new List<OCRProviderHealth>();

            foreach (var provider in availableProviders)
            {
                try
                {
                    var health = await provider.CheckHealthAsync();
                    providerHealths.Add(health);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to get health for provider {Provider}", provider.ProviderName);
                }
            }

            if (!providerHealths.Any())
            {
                return new OCRProviderHealth
                {
                    IsHealthy = false,
                    IsAvailable = false,
                    Status = "No providers available",
                    LastChecked = DateTime.UtcNow,
                    ErrorMessage = "No OCR providers are available"
                };
            }

            var healthyProviders = providerHealths.Count(h => h.IsHealthy);
            var totalProviders = providerHealths.Count;
            var successRate = (double)healthyProviders / totalProviders * 100;

            return new OCRProviderHealth
            {
                IsHealthy = healthyProviders > 0,
                IsAvailable = true,
                Status = healthyProviders > 0 ? "Healthy" : "Unhealthy",
                LastChecked = DateTime.UtcNow,
                SuccessRate = successRate,
                AverageProcessingTime = providerHealths.Any() ? 
                    TimeSpan.FromMilliseconds(providerHealths.Average(h => h.AverageProcessingTime.TotalMilliseconds)) : 
                    TimeSpan.Zero,
                Metrics = new Dictionary<string, object>
                {
                    ["TotalProviders"] = totalProviders,
                    ["HealthyProviders"] = healthyProviders,
                    ["UnhealthyProviders"] = totalProviders - healthyProviders
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get OCR service health");
            return new OCRProviderHealth
            {
                IsHealthy = false,
                IsAvailable = false,
                Status = "Health check failed",
                LastChecked = DateTime.UtcNow,
                ErrorMessage = ex.Message
            };
        }
    }

    /// <summary>
    /// Gets the capabilities of the OCR service
    /// </summary>
    /// <returns>Service capabilities</returns>
    public OCRProviderCapabilities GetCapabilities()
    {
        var capabilities = new OCRProviderCapabilities
        {
            SupportsLanguageDetection = _providers.Any(p => p.Capabilities.SupportsLanguageDetection),
            SupportsBoundingBoxes = _providers.Any(p => p.Capabilities.SupportsBoundingBoxes),
            SupportsConfidenceScores = _providers.Any(p => p.Capabilities.SupportsConfidenceScores),
            SupportsHandwriting = _providers.Any(p => p.Capabilities.SupportsHandwriting),
            SupportsPrintedText = _providers.Any(p => p.Capabilities.SupportsPrintedText),
            SupportsTableDetection = _providers.Any(p => p.Capabilities.SupportsTableDetection),
            SupportsFormDetection = _providers.Any(p => p.Capabilities.SupportsFormDetection),
            MaxImageSizeMB = _providers.Any() ? _providers.Max(p => p.Capabilities.MaxImageSizeMB) : 0,
            SupportedLanguages = _providers.SelectMany(p => p.Capabilities.SupportedLanguages).Distinct().ToArray(),
            CostPerRequest = _providers.Any() ? _providers.Min(p => p.Capabilities.CostPerRequest) : 0,
            AverageResponseTime = _providers.Any() ? 
                TimeSpan.FromMilliseconds(_providers.Average(p => p.Capabilities.AverageResponseTime.TotalMilliseconds)) : 
                TimeSpan.Zero,
            SupportsCustomModels = _providers.Any(p => p.Capabilities.SupportsCustomModels),
            MaxPagesPerRequest = _providers.Any() ? _providers.Max(p => p.Capabilities.MaxPagesPerRequest) : 0,
            SupportsBatchProcessing = _providers.Any(p => p.Capabilities.SupportsBatchProcessing),
            SupportsRealTimeProcessing = _providers.Any(p => p.Capabilities.SupportsRealTimeProcessing),
            SupportsImagePreprocessing = _providers.Any(p => p.Capabilities.SupportsImagePreprocessing),
            SupportsRotationDetection = _providers.Any(p => p.Capabilities.SupportsRotationDetection),
            SupportsSkewDetection = _providers.Any(p => p.Capabilities.SupportsSkewDetection)
        };

        return capabilities;
    }

    /// <summary>
    /// Gets the cost estimate for processing an image
    /// </summary>
    /// <param name="imageSizeBytes">Size of the image in bytes</param>
    /// <param name="options">OCR processing options</param>
    /// <returns>Estimated cost in USD</returns>
    public async Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null)
    {
        try
        {
            var requestOptions = options ?? new OCRRequest();
            return await _providerSelector.GetCostEstimateAsync(requestOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cost estimate");
            return 0;
        }
    }

    /// <summary>
    /// Processes multiple images in batch
    /// </summary>
    /// <param name="imageStreams">Collection of image streams to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of OCR results</returns>
    public async Task<Result<IEnumerable<OCRResult>>> ProcessBatchAsync(
        IEnumerable<Stream> imageStreams, 
        OCRRequest? options = null, 
        CancellationToken cancellationToken = default)
    {
        if (!_config.EnableBatchProcessing)
        {
            return Result.Failure<IEnumerable<OCRResult>>("Batch processing is not enabled");
        }

        var imageStreamList = imageStreams.ToList();
        if (imageStreamList.Count > _config.MaxBatchSize)
        {
            return Result.Failure<IEnumerable<OCRResult>>($"Batch size exceeds maximum allowed: {_config.MaxBatchSize}");
        }

        try
        {
            var results = new List<OCRResult>();
            var requestOptions = options ?? new OCRRequest();

            foreach (var imageStream in imageStreamList)
            {
                var result = await ExtractTextAsync(imageStream, requestOptions, cancellationToken);
                if (result.IsSuccess)
                {
                    results.Add(result.Value);
                }
                else
                {
                    _logger.LogWarning("Failed to process image in batch: {Error}", result.Error);
                }
            }

            return Result.Success<IEnumerable<OCRResult>>(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Batch processing failed");
            return Result.Failure<IEnumerable<OCRResult>>($"Batch processing failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Processes multiple image files in batch
    /// </summary>
    /// <param name="imagePaths">Collection of image file paths to process</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of OCR results</returns>
    public async Task<Result<IEnumerable<OCRResult>>> ProcessBatchAsync(
        IEnumerable<string> imagePaths, 
        OCRRequest? options = null, 
        CancellationToken cancellationToken = default)
    {
        var imagePathList = imagePaths.ToList();
        if (imagePathList.Count > _config.MaxBatchSize)
        {
            return Result.Failure<IEnumerable<OCRResult>>($"Batch size exceeds maximum allowed: {_config.MaxBatchSize}");
        }

        try
        {
            var results = new List<OCRResult>();
            var requestOptions = options ?? new OCRRequest();

            foreach (var imagePath in imagePathList)
            {
                var result = await ExtractTextAsync(imagePath, requestOptions, cancellationToken);
                if (result.IsSuccess)
                {
                    results.Add(result.Value);
                }
                else
                {
                    _logger.LogWarning("Failed to process image file {ImagePath} in batch: {Error}", imagePath, result.Error);
                }
            }

            return Result.Success<IEnumerable<OCRResult>>(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Batch processing failed");
            return Result.Failure<IEnumerable<OCRResult>>($"Batch processing failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Processes an image with a specific provider
    /// </summary>
    /// <param name="provider">The OCR provider</param>
    /// <param name="options">OCR processing options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OCR result</returns>
    private async Task<OCRResult> ProcessWithProvider(IOCRProvider provider, OCRRequest options, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing with provider: {Provider}", provider.ProviderName);

        var result = await provider.ProcessImageAsync(options.ImageStream!, options, cancellationToken);

        // Post-process result based on provider capabilities
        if (!provider.Capabilities.SupportsConfidenceScores)
        {
            result.Confidence = CalculateEstimatedConfidence(result);
        }

        if (!provider.Capabilities.SupportsLanguageDetection)
        {
            result.Language = DetectLanguage(result.Text);
        }

        return result;
    }

    /// <summary>
    /// Calculates an estimated confidence score for text blocks
    /// </summary>
    /// <param name="result">The OCR result</param>
    /// <returns>Estimated confidence score</returns>
    private double CalculateEstimatedConfidence(OCRResult result)
    {
        if (!result.TextBlocks.Any())
            return 0.5;

        // Simple heuristic: longer text blocks are more likely to be accurate
        var totalLength = result.TextBlocks.Sum(tb => tb.Text.Length);
        var averageLength = totalLength / (double)result.TextBlocks.Count;
        
        // Base confidence on average text length (longer = more confident)
        var baseConfidence = Math.Min(0.9, averageLength / 10.0);
        
        return Math.Max(0.1, baseConfidence);
    }

    /// <summary>
    /// Detects the language of text
    /// </summary>
    /// <param name="text">The text to analyze</param>
    /// <returns>Detected language code</returns>
    private string DetectLanguage(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "en";

        var lowerText = text.ToLower();
        
        // Check for common English words
        var englishWords = new[] { "the", "and", "or", "but", "in", "on", "at", "to", "for", "of", "with", "by" };
        var englishCount = englishWords.Count(word => lowerText.Contains(word));
        
        // Check for common Spanish words
        var spanishWords = new[] { "el", "la", "de", "que", "y", "a", "en", "un", "es", "se", "no", "te", "lo", "le", "da", "su", "por", "son", "con", "para", "al", "del", "los", "las", "una", "uno" };
        var spanishCount = spanishWords.Count(word => lowerText.Contains(word));
        
        // Check for common French words
        var frenchWords = new[] { "le", "la", "de", "que", "et", "à", "en", "un", "est", "se", "ne", "te", "lo", "le", "da", "su", "par", "son", "con", "pour", "al", "del", "les", "las", "une", "un" };
        var frenchCount = frenchWords.Count(word => lowerText.Contains(word));
        
        // Return the language with the most matches
        if (spanishCount > englishCount && spanishCount > frenchCount)
            return "es";
        if (frenchCount > englishCount && frenchCount > spanishCount)
            return "fr";
        
        return "en"; // Default to English
    }
}