using Microsoft.Extensions.Logging;
using AMCode.OCR.Models;
using AMCode.OCR.Enums;

namespace AMCode.OCR.Services;

/// <summary>
/// Interface for OCR provider selection
/// </summary>
public interface IOCRProviderSelector
{
    /// <summary>
    /// Selects the best OCR provider for a request
    /// </summary>
    /// <param name="request">The OCR request</param>
    /// <returns>The best OCR provider</returns>
    Task<IOCRProvider> SelectBestProviderAsync(OCRRequest request);

    /// <summary>
    /// Gets all available OCR providers
    /// </summary>
    /// <returns>Collection of available providers</returns>
    Task<IEnumerable<IOCRProvider>> GetAvailableProvidersAsync();

    /// <summary>
    /// Gets the health status of a specific provider
    /// </summary>
    /// <param name="providerName">The provider name</param>
    /// <returns>Health status information</returns>
    Task<OCRProviderHealth> GetProviderHealthAsync(string providerName);

    /// <summary>
    /// Gets the cost estimate for a request across all providers
    /// </summary>
    /// <param name="request">The OCR request</param>
    /// <returns>Cost estimate</returns>
    Task<decimal> GetCostEstimateAsync(OCRRequest request);
}

/// <summary>
/// Smart OCR provider selector implementation
/// </summary>
public class SmartOCRProviderSelector : IOCRProviderSelector
{
    private readonly IEnumerable<IOCRProvider> _providers;
    private readonly ILogger<SmartOCRProviderSelector> _logger;
    private readonly OCRProviderSelectionStrategy _strategy;
    private readonly Dictionary<string, OCRProviderHealth> _providerHealthCache;
    private readonly Dictionary<string, DateTime> _lastHealthCheck;

    /// <summary>
    /// Initializes a new instance of the SmartOCRProviderSelector class
    /// </summary>
    /// <param name="providers">The available OCR providers</param>
    /// <param name="logger">The logger</param>
    /// <param name="strategy">The selection strategy</param>
    public SmartOCRProviderSelector(
        IEnumerable<IOCRProvider> providers,
        ILogger<SmartOCRProviderSelector> logger,
        OCRProviderSelectionStrategy strategy = OCRProviderSelectionStrategy.Balanced)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _strategy = strategy;
        _providerHealthCache = new Dictionary<string, OCRProviderHealth>();
        _lastHealthCheck = new Dictionary<string, DateTime>();
    }

    /// <summary>
    /// Selects the best OCR provider for a request
    /// </summary>
    /// <param name="request">The OCR request</param>
    /// <returns>The best OCR provider</returns>
    public async Task<IOCRProvider> SelectBestProviderAsync(OCRRequest request)
    {
        _logger.LogInformation("Selecting best OCR provider for request using strategy: {Strategy}", _strategy);

        var availableProviders = await GetAvailableProvidersAsync();
        var compatibleProviders = availableProviders.Where(p => p.CanProcess(request)).ToList();

        if (!compatibleProviders.Any())
        {
            throw new InvalidOperationException("No compatible OCR providers available for the request");
        }

        var selectedProvider = _strategy switch
        {
            OCRProviderSelectionStrategy.CostOptimized => SelectCostOptimizedProvider(compatibleProviders, request),
            OCRProviderSelectionStrategy.PerformanceOptimized => SelectPerformanceOptimizedProvider(compatibleProviders, request),
            OCRProviderSelectionStrategy.ReliabilityOptimized => SelectReliabilityOptimizedProvider(compatibleProviders, request),
            OCRProviderSelectionStrategy.CapabilityOptimized => SelectCapabilityOptimizedProvider(compatibleProviders, request),
            OCRProviderSelectionStrategy.QualityOptimized => SelectQualityOptimizedProvider(compatibleProviders, request),
            OCRProviderSelectionStrategy.Balanced => SelectBalancedProvider(compatibleProviders, request),
            OCRProviderSelectionStrategy.LoadBalanced => SelectLoadBalancedProvider(compatibleProviders, request),
            OCRProviderSelectionStrategy.GeographicOptimized => SelectGeographicOptimizedProvider(compatibleProviders, request),
            _ => compatibleProviders.First()
        };

        _logger.LogInformation("Selected provider: {Provider} for request", selectedProvider.ProviderName);
        return selectedProvider;
    }

    /// <summary>
    /// Gets all available OCR providers
    /// </summary>
    /// <returns>Collection of available providers</returns>
    public async Task<IEnumerable<IOCRProvider>> GetAvailableProvidersAsync()
    {
        var availableProviders = new List<IOCRProvider>();

        foreach (var provider in _providers)
        {
            try
            {
                if (provider.IsAvailable)
                {
                    availableProviders.Add(provider);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Provider {Provider} is not available", provider.ProviderName);
            }
        }

        return availableProviders;
    }

    /// <summary>
    /// Gets the health status of a specific provider
    /// </summary>
    /// <param name="providerName">The provider name</param>
    /// <returns>Health status information</returns>
    public async Task<OCRProviderHealth> GetProviderHealthAsync(string providerName)
    {
        var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
        if (provider == null)
        {
            return new OCRProviderHealth
            {
                IsHealthy = false,
                IsAvailable = false,
                Status = "Provider not found",
                LastChecked = DateTime.UtcNow,
                ErrorMessage = $"Provider '{providerName}' not found"
            };
        }

        // Check if we have a cached health status that's still valid
        if (_providerHealthCache.TryGetValue(providerName, out var cachedHealth) &&
            _lastHealthCheck.TryGetValue(providerName, out var lastCheck) &&
            DateTime.UtcNow - lastCheck < TimeSpan.FromMinutes(5))
        {
            return cachedHealth;
        }

        try
        {
            var health = await provider.CheckHealthAsync();
            _providerHealthCache[providerName] = health;
            _lastHealthCheck[providerName] = DateTime.UtcNow;
            return health;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check health for provider {Provider}", providerName);
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
    /// Gets the cost estimate for a request across all providers
    /// </summary>
    /// <param name="request">The OCR request</param>
    /// <returns>Cost estimate</returns>
    public async Task<decimal> GetCostEstimateAsync(OCRRequest request)
    {
        var availableProviders = await GetAvailableProvidersAsync();
        var compatibleProviders = availableProviders.Where(p => p.CanProcess(request));

        if (!compatibleProviders.Any())
            return 0;

        var costs = new List<decimal>();
        foreach (var provider in compatibleProviders)
        {
            try
            {
                var cost = await provider.GetCostEstimateAsync(0, request); // 0 bytes for estimation
                costs.Add(cost);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get cost estimate for provider {Provider}", provider.ProviderName);
            }
        }

        return costs.Any() ? costs.Min() : 0;
    }

    /// <summary>
    /// Selects the cost-optimized provider
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectCostOptimizedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        return providers
            .OrderBy(p => p.Capabilities.CostPerRequest)
            .First();
    }

    /// <summary>
    /// Selects the performance-optimized provider
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectPerformanceOptimizedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        return providers
            .OrderBy(p => p.Capabilities.AverageResponseTime)
            .First();
    }

    /// <summary>
    /// Selects the reliability-optimized provider
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectReliabilityOptimizedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        return providers
            .OrderByDescending(p => p.GetReliabilityScore())
            .First();
    }

    /// <summary>
    /// Selects the capability-optimized provider
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectCapabilityOptimizedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        return providers
            .Where(p => p.Capabilities.SupportsLanguageDetection == request.RequiresLanguageDetection)
            .Where(p => p.Capabilities.SupportsHandwriting == request.RequiresHandwritingSupport)
            .Where(p => p.Capabilities.SupportsTableDetection == request.RequiresTableDetection)
            .Where(p => p.Capabilities.SupportsFormDetection == request.RequiresFormDetection)
            .OrderByDescending(p => GetProviderCapabilityScore(p, request))
            .First();
    }

    /// <summary>
    /// Selects the quality-optimized provider
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectQualityOptimizedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        return providers
            .OrderByDescending(p => p.GetQualityScore())
            .First();
    }

    /// <summary>
    /// Selects a balanced provider considering multiple factors
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectBalancedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        return providers
            .OrderByDescending(p => GetBalancedScore(p, request))
            .First();
    }

    /// <summary>
    /// Selects a load-balanced provider
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectLoadBalancedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        // Simple round-robin selection based on provider name hash
        var providerList = providers.ToList();
        var index = Math.Abs(request.GetHashCode()) % providerList.Count;
        return providerList[index];
    }

    /// <summary>
    /// Selects a geographically optimized provider
    /// </summary>
    /// <param name="providers">Available providers</param>
    /// <param name="request">The OCR request</param>
    /// <returns>Selected provider</returns>
    private IOCRProvider SelectGeographicOptimizedProvider(IEnumerable<IOCRProvider> providers, OCRRequest request)
    {
        // For now, just return the first available provider
        // In a real implementation, this would consider geographic proximity
        return providers.First();
    }

    /// <summary>
    /// Calculates the capability score for a provider
    /// </summary>
    /// <param name="provider">The provider</param>
    /// <param name="request">The request</param>
    /// <returns>Capability score</returns>
    private double GetProviderCapabilityScore(IOCRProvider provider, OCRRequest request)
    {
        var score = 0.0;
        var capabilities = provider.Capabilities;

        if (request.RequiresLanguageDetection && capabilities.SupportsLanguageDetection)
            score += 0.3;
        if (request.RequiresHandwritingSupport && capabilities.SupportsHandwriting)
            score += 0.3;
        if (request.RequiresTableDetection && capabilities.SupportsTableDetection)
            score += 0.2;
        if (request.RequiresFormDetection && capabilities.SupportsFormDetection)
            score += 0.2;

        return score;
    }

    /// <summary>
    /// Calculates the balanced score for a provider
    /// </summary>
    /// <param name="provider">The provider</param>
    /// <param name="request">The request</param>
    /// <returns>Balanced score</returns>
    private double GetBalancedScore(IOCRProvider provider, OCRRequest request)
    {
        var costScore = 1.0 - (double)(provider.Capabilities.CostPerRequest / 0.01m); // Normalize cost
        var performanceScore = 1.0 - (provider.Capabilities.AverageResponseTime.TotalSeconds / 10.0); // Normalize performance
        var reliabilityScore = provider.GetReliabilityScore();
        var qualityScore = provider.GetQualityScore();
        var capabilityScore = GetProviderCapabilityScore(provider, request);

        // Weighted average
        return (costScore * 0.2) + (performanceScore * 0.2) + (reliabilityScore * 0.2) + (qualityScore * 0.2) + (capabilityScore * 0.2);
    }
}