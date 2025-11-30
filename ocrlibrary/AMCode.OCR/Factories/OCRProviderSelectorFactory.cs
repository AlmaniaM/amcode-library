using AMCode.OCR.Configurations;
using AMCode.OCR.Enums;
using AMCode.OCR.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMCode.OCR.Factories;

/// <summary>
/// Factory for creating OCR provider selectors based on configuration
/// </summary>
public class OCRProviderSelectorFactory : IOCRProviderSelectorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OCRProviderSelectorFactory> _logger;
    private readonly IOptions<OCRConfiguration> _config;

    public OCRProviderSelectorFactory(
        IServiceProvider serviceProvider,
        ILogger<OCRProviderSelectorFactory> logger,
        IOptions<OCRConfiguration> config)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public IOCRProviderSelector CreateSelector()
    {
        var providers = _serviceProvider.GetServices<IOCRProvider>();
        var strategy = _config.Value.ProviderSelectionStrategy;

        _logger.LogDebug("Creating OCR provider selector with strategy: {Strategy}", strategy);

        if (strategy == OCRProviderSelectionStrategy.Configuration)
        {
            // Use ConfigurationOCRProviderSelector for manual provider selection
            var logger = _serviceProvider.GetRequiredService<ILogger<ConfigurationOCRProviderSelector>>();
            _logger.LogInformation("Creating ConfigurationOCRProviderSelector with provider: {Provider}", _config.Value.Provider);
            return new ConfigurationOCRProviderSelector(providers, logger, _config);
        }
        else
        {
            // Use SmartOCRProviderSelector with smart selection strategy
            var logger = _serviceProvider.GetRequiredService<ILogger<SmartOCRProviderSelector>>();
            var defaultProvider = _config.Value.Provider;

            _logger.LogInformation(
                "Creating SmartOCRProviderSelector with strategy: {Strategy}, defaultProvider: {Provider}",
                strategy,
                defaultProvider);

            return new SmartOCRProviderSelector(providers, logger, strategy, defaultProvider);
        }
    }
}
