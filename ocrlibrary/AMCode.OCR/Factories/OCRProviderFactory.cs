using AMCode.OCR;
using AMCode.OCR.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMCode.OCR.Factories;

/// <summary>
/// Factory for creating OCR providers dynamically based on configuration
/// </summary>
public class OCRProviderFactory : IOCRProviderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OCRProviderFactory> _logger;
    private readonly OCRConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the OCRProviderFactory class
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <param name="options">The OCR configuration options</param>
    /// <param name="logger">The logger</param>
    public OCRProviderFactory(
        IServiceProvider serviceProvider,
        IOptions<OCRConfiguration> options,
        ILogger<OCRProviderFactory> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Creates the primary OCR provider based on OCR:Provider configuration
    /// </summary>
    /// <returns>Configured OCR provider, or null if not found</returns>
    public IOCRProvider? CreateProvider()
    {
        if (string.IsNullOrWhiteSpace(_configuration.Provider))
        {
            _logger.LogWarning("OCR:Provider is not configured");
            return null;
        }

        var providerName = _configuration.Provider;
        _logger.LogInformation("Creating OCR provider {ProviderName}", providerName);

        var provider = FindProviderByName(providerName);
        if (provider == null)
        {
            _logger.LogWarning("OCR provider '{ProviderName}' not found", providerName);
        }

        return provider;
    }

    /// <summary>
    /// Creates the fallback OCR provider based on OCR:FallbackProvider configuration
    /// </summary>
    /// <returns>Configured fallback OCR provider, or null if not found</returns>
    public IOCRProvider? CreateFallbackProvider()
    {
        if (string.IsNullOrWhiteSpace(_configuration.FallbackProvider))
        {
            _logger.LogDebug("OCR:FallbackProvider is not configured");
            return null;
        }

        var providerName = _configuration.FallbackProvider;
        _logger.LogInformation("Creating fallback OCR provider {ProviderName}", providerName);

        var provider = FindProviderByName(providerName);
        if (provider == null)
        {
            _logger.LogWarning("Fallback OCR provider '{ProviderName}' not found", providerName);
        }

        return provider;
    }

    /// <summary>
    /// Finds an OCR provider by name, supporting exact match, partial match, and common aliases
    /// </summary>
    /// <param name="providerName">The provider name to find</param>
    /// <returns>The matching OCR provider, or null if not found</returns>
    private IOCRProvider? FindProviderByName(string providerName)
    {
        var providers = _serviceProvider.GetServices<IOCRProvider>().ToList();
        if (providers.Count == 0)
        {
            _logger.LogWarning("No OCR providers registered in service collection");
            return null;
        }

        // Try exact match first (case-insensitive)
        var provider = providers.FirstOrDefault(p =>
            p.ProviderName.Equals(providerName, StringComparison.OrdinalIgnoreCase));

        if (provider != null)
        {
            return provider;
        }

        // Try partial match (provider name contains the search term)
        provider = providers.FirstOrDefault(p =>
            p.ProviderName.Contains(providerName, StringComparison.OrdinalIgnoreCase));

        if (provider != null)
        {
            return provider;
        }

        // Try reverse partial match (search term contains provider name)
        provider = providers.FirstOrDefault(p =>
            providerName.Contains(p.ProviderName, StringComparison.OrdinalIgnoreCase));

        if (provider != null)
        {
            return provider;
        }

        // Try common aliases
        provider = FindProviderByAlias(providerName, providers);
        if (provider != null)
        {
            return provider;
        }

        return null;
    }

    /// <summary>
    /// Finds a provider using common aliases
    /// </summary>
    /// <param name="alias">The alias to search for</param>
    /// <param name="providers">The list of available providers</param>
    /// <returns>The matching provider, or null if not found</returns>
    private IOCRProvider? FindProviderByAlias(string alias, List<IOCRProvider> providers)
    {
        var normalizedAlias = alias.ToLowerInvariant().Trim();

        // Use registry instead of inline map
        if (!OCRProviderRegistry.ProviderNameAliases.TryGetValue(normalizedAlias, out var targetNames))
        {
            return null;
        }

        foreach (var targetName in targetNames)
        {
            var provider = providers.FirstOrDefault(p =>
                p.ProviderName.Equals(targetName, StringComparison.OrdinalIgnoreCase) ||
                p.ProviderName.Contains(targetName, StringComparison.OrdinalIgnoreCase));

            if (provider != null)
            {
                return provider;
            }
        }

        return null;
    }
}

