using AMCode.AI.Configurations;
using AMCode.AI.Enums;
using AMCode.AI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AMCode.AI.Factories;

/// <summary>
/// Factory for creating AI provider selectors based on configuration
/// </summary>
public class AIProviderSelectorFactory : IAIProviderSelectorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AIProviderSelectorFactory> _logger;
    private readonly IOptions<AIConfiguration> _config;

    public AIProviderSelectorFactory(
        IServiceProvider serviceProvider,
        ILogger<AIProviderSelectorFactory> logger,
        IOptions<AIConfiguration> config)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public IAIProviderSelector CreateSelector()
    {
        var providers = _serviceProvider.GetServices<IAIProvider>();
        var selectionMode = (_config.Value.ProviderSelectionStrategy ?? "smart").ToLower();

        _logger.LogDebug("Creating AI provider selector with mode: {Mode}", selectionMode);

        if (selectionMode == "configuration")
        {
            // Use ConfigurationAIProviderSelector for manual provider selection
            var logger = _serviceProvider.GetRequiredService<ILogger<ConfigurationAIProviderSelector>>();
            _logger.LogInformation("Creating ConfigurationAIProviderSelector with provider: {Provider}", _config.Value.SelectedProvider);
            return new ConfigurationAIProviderSelector(providers, logger, _config);
        }
        else
        {
            // Use SmartAIProviderSelector with smart selection strategy
            var logger = _serviceProvider.GetRequiredService<ILogger<SmartAIProviderSelector>>();
            var costAnalyzer = _serviceProvider.GetRequiredService<ICostAnalyzer>();

            // Parse SmartSelectionStrategy (defaults to Balanced if not set or invalid)
            var strategy = Enum.TryParse<AIProviderSelectionStrategy>(_config.Value.SmartSelectionStrategy, out var parsedStrategy)
                ? parsedStrategy
                : AIProviderSelectionStrategy.Balanced;

            _logger.LogInformation("Creating SmartAIProviderSelector with strategy: {Strategy}", strategy);
            return new SmartAIProviderSelector(providers, logger, costAnalyzer, strategy);
        }
    }
}

