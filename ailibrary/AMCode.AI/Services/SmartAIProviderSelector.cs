using AMCode.AI.Enums;
using AMCode.AI.Models;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Services;

/// <summary>
/// Interface for AI provider selection
/// </summary>
public interface IAIProviderSelector
{
    /// <summary>
    /// Select the best provider for a request
    /// </summary>
    /// <param name="request">AI request</param>
    /// <returns>Selected provider</returns>
    Task<IAIProvider> SelectOCRProvider(AIRequest request);
    
    /// <summary>
    /// Get all available providers
    /// </summary>
    /// <returns>Available providers</returns>
    Task<IEnumerable<IAIProvider>> GetAvailableProvidersAsync();
    
    /// <summary>
    /// Get health status for a provider
    /// </summary>
    /// <param name="providerName">Provider name</param>
    /// <returns>Provider health</returns>
    Task<AIProviderHealth> GetProviderHealthAsync(string providerName);
    
    /// <summary>
    /// Get cost estimate for a request
    /// </summary>
    /// <param name="request">AI request</param>
    /// <returns>Cost estimate</returns>
    Task<decimal> GetCostEstimateAsync(AIRequest request);
}

/// <summary>
/// Smart AI provider selector with multiple selection strategies
/// </summary>
public class SmartAIProviderSelector : IAIProviderSelector
{
    private readonly IEnumerable<IAIProvider> _providers;
    private readonly ILogger<SmartAIProviderSelector> _logger;
    private readonly AIProviderSelectionStrategy _strategy;
    private readonly ICostAnalyzer _costAnalyzer;
    private readonly Dictionary<string, AIProviderHealth> _healthCache = new();
    private readonly object _healthCacheLock = new();
    
    public SmartAIProviderSelector(
        IEnumerable<IAIProvider> providers,
        ILogger<SmartAIProviderSelector> logger,
        ICostAnalyzer costAnalyzer,
        AIProviderSelectionStrategy strategy = AIProviderSelectionStrategy.Balanced)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _costAnalyzer = costAnalyzer ?? throw new ArgumentNullException(nameof(costAnalyzer));
        _strategy = strategy;
    }
    
    public async Task<IAIProvider> SelectOCRProvider(AIRequest request)
    {
        try
        {
            _logger.LogDebug("Selecting best provider for request with strategy: {Strategy}", _strategy);
            
            var availableProviders = await GetAvailableProvidersAsync();
            if (!availableProviders.Any())
            {
                throw new InvalidOperationException("No AI providers are available");
            }
            
            var selectedProvider = _strategy switch
            {
                AIProviderSelectionStrategy.CostOptimized => SelectCostOptimizedProvider(availableProviders, request),
                AIProviderSelectionStrategy.PerformanceOptimized => SelectPerformanceOptimizedProvider(availableProviders, request),
                AIProviderSelectionStrategy.ReliabilityOptimized => SelectReliabilityOptimizedProvider(availableProviders, request),
                AIProviderSelectionStrategy.CapabilityOptimized => SelectCapabilityOptimizedProvider(availableProviders, request),
                AIProviderSelectionStrategy.QualityOptimized => SelectQualityOptimizedProvider(availableProviders, request),
                AIProviderSelectionStrategy.Balanced => SelectBalancedProvider(availableProviders, request),
                _ => availableProviders.First()
            };
            
            _logger.LogInformation("Selected provider: {Provider} for strategy: {Strategy}", 
                selectedProvider.ProviderName, _strategy);
            
            return selectedProvider;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to select best provider for request");
            throw;
        }
    }
    
    public async Task<IEnumerable<IAIProvider>> GetAvailableProvidersAsync()
    {
        try
        {
            var availableProviders = new List<IAIProvider>();
            
            foreach (var provider in _providers)
            {
                if (provider.IsAvailable)
                {
                    availableProviders.Add(provider);
                }
            }
            
            _logger.LogDebug("Found {Count} available providers", availableProviders.Count);
            return availableProviders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get available providers");
            return Enumerable.Empty<IAIProvider>();
        }
    }
    
    public async Task<AIProviderHealth> GetProviderHealthAsync(string providerName)
    {
        try
        {
            lock (_healthCacheLock)
            {
                if (_healthCache.TryGetValue(providerName, out var cachedHealth) && 
                    DateTime.UtcNow - cachedHealth.LastChecked < TimeSpan.FromMinutes(5))
                {
                    return cachedHealth;
                }
            }
            
            var provider = _providers.FirstOrDefault(p => p.ProviderName == providerName);
            if (provider == null)
            {
                return new AIProviderHealth
                {
                    IsHealthy = false,
                    Status = "Provider not found",
                    LastChecked = DateTime.UtcNow
                };
            }
            
            var health = await provider.CheckHealthAsync();
            
            lock (_healthCacheLock)
            {
                _healthCache[providerName] = health;
            }
            
            return health;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get health for provider {Provider}", providerName);
            return new AIProviderHealth
            {
                IsHealthy = false,
                Status = "Health check failed",
                ErrorMessage = ex.Message,
                LastChecked = DateTime.UtcNow
            };
        }
    }
    
    public async Task<decimal> GetCostEstimateAsync(AIRequest request)
    {
        try
        {
            var availableProviders = await GetAvailableProvidersAsync();
            if (!availableProviders.Any())
            {
                return 0m;
            }
            
            var provider = SelectCostOptimizedProvider(availableProviders, request);
            var costEstimate = await provider.GetCostEstimateAsync(request.Text, request.Options);
            
            _logger.LogDebug("Cost estimate for request: ${Cost:F6} using provider {Provider}", 
                costEstimate, provider.ProviderName);
            
            return costEstimate;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cost estimate for request");
            return 0m;
        }
    }
    
    private IAIProvider SelectCostOptimizedProvider(IEnumerable<IAIProvider> providers, AIRequest request)
    {
        return providers
            .Where(p => p.Capabilities.MaxTokens >= request.EstimatedTokens)
            .OrderBy(p => p.Capabilities.CostPerToken)
            .First();
    }
    
    private IAIProvider SelectPerformanceOptimizedProvider(IEnumerable<IAIProvider> providers, AIRequest request)
    {
        return providers
            .Where(p => p.Capabilities.MaxTokens >= request.EstimatedTokens)
            .OrderBy(p => p.Capabilities.AverageResponseTime)
            .First();
    }
    
    private IAIProvider SelectReliabilityOptimizedProvider(IEnumerable<IAIProvider> providers, AIRequest request)
    {
        return providers
            .Where(p => p.Capabilities.MaxTokens >= request.EstimatedTokens)
            .OrderByDescending(p => GetProviderReliabilityScore(p))
            .First();
    }
    
    private IAIProvider SelectCapabilityOptimizedProvider(IEnumerable<IAIProvider> providers, AIRequest request)
    {
        return providers
            .Where(p => p.Capabilities.MaxTokens >= request.EstimatedTokens)
            .Where(p => p.Capabilities.SupportsFunctionCalling == request.RequiresFunctionCalling)
            .Where(p => p.Capabilities.SupportsVision == request.RequiresVision)
            .OrderByDescending(p => GetProviderCapabilityScore(p, request))
            .First();
    }
    
    private IAIProvider SelectQualityOptimizedProvider(IEnumerable<IAIProvider> providers, AIRequest request)
    {
        return providers
            .Where(p => p.Capabilities.MaxTokens >= request.EstimatedTokens)
            .OrderByDescending(p => GetProviderQualityScore(p))
            .First();
    }
    
    private IAIProvider SelectBalancedProvider(IEnumerable<IAIProvider> providers, AIRequest request)
    {
        return providers
            .Where(p => p.Capabilities.MaxTokens >= request.EstimatedTokens)
            .OrderBy(p => GetProviderBalancedScore(p, request))
            .First();
    }
    
    private double GetProviderReliabilityScore(IAIProvider provider)
    {
        // Simple reliability score based on capabilities
        var score = 0.0;
        
        if (provider.Capabilities.SupportsLongContext) score += 0.3;
        if (provider.Capabilities.SupportsFunctionCalling) score += 0.2;
        if (provider.Capabilities.SupportsVision) score += 0.1;
        if (provider.Capabilities.MaxTokens > 4000) score += 0.2;
        if (provider.Capabilities.AverageResponseTime < TimeSpan.FromSeconds(3)) score += 0.2;
        
        return score;
    }
    
    private double GetProviderCapabilityScore(IAIProvider provider, AIRequest request)
    {
        var score = 0.0;
        
        if (provider.Capabilities.SupportsFunctionCalling && request.RequiresFunctionCalling) score += 0.4;
        if (provider.Capabilities.SupportsVision && request.RequiresVision) score += 0.3;
        if (provider.Capabilities.SupportsLongContext) score += 0.2;
        if (provider.Capabilities.MaxTokens >= request.EstimatedTokens * 2) score += 0.1;
        
        return score;
    }
    
    private double GetProviderQualityScore(IAIProvider provider)
    {
        // Quality score based on provider capabilities and performance
        var score = 0.0;
        
        if (provider.Capabilities.SupportsFunctionCalling) score += 0.3;
        if (provider.Capabilities.SupportsLongContext) score += 0.2;
        if (provider.Capabilities.MaxTokens > 8000) score += 0.2;
        if (provider.Capabilities.AverageResponseTime < TimeSpan.FromSeconds(2)) score += 0.2;
        if (provider.Capabilities.SupportsVision) score += 0.1;
        
        return score;
    }
    
    private double GetProviderBalancedScore(IAIProvider provider, AIRequest request)
    {
        // Balanced score considering cost, performance, and capabilities
        var costScore = 1.0 / (1.0 + (double)provider.Capabilities.CostPerToken * 1000);
        var performanceScore = 1.0 / (1.0 + provider.Capabilities.AverageResponseTime.TotalSeconds);
        var capabilityScore = GetProviderCapabilityScore(provider, request);
        
        return (costScore * 0.4) + (performanceScore * 0.3) + (capabilityScore * 0.3);
    }
}
