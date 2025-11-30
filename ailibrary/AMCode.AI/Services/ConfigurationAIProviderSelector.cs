using AMCode.AI.Models;
using AMCode.AI.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMCode.AI.Configurations;

namespace AMCode.AI.Services;

/// <summary>
/// Configuration-based AI provider selector that uses a manually configured provider
/// </summary>
public class ConfigurationAIProviderSelector : IAIProviderSelector
{
    private readonly IEnumerable<IAIProvider> _providers;
    private readonly ILogger<ConfigurationAIProviderSelector> _logger;
    private readonly AIConfiguration _config;

    public ConfigurationAIProviderSelector(
        IEnumerable<IAIProvider> providers,
        ILogger<ConfigurationAIProviderSelector> logger,
        IOptions<AIConfiguration> config)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<IAIProvider> SelectOCRProvider(AIRequest request)
    {
        try
        {
            _logger.LogDebug("Selecting provider from configuration: {SelectedProvider}", _config.SelectedProvider);

            var availableProviders = await GetAvailableProvidersAsync();
            if (!availableProviders.Any())
            {
                throw new InvalidOperationException("No AI providers are available");
            }

            // If no provider is configured, use first available
            if (string.IsNullOrWhiteSpace(_config.SelectedProvider))
            {
                _logger.LogWarning("No provider specified in configuration (AI:SelectedProvider). Using first available provider.");
                return availableProviders.First();
            }

            // Try to find exact match first
            var selectedProvider = availableProviders.FirstOrDefault(p =>
                p.ProviderName.Equals(_config.SelectedProvider, StringComparison.OrdinalIgnoreCase));

            // If no exact match, try partial match
            if (selectedProvider == null)
            {
                selectedProvider = availableProviders.FirstOrDefault(p =>
                    p.ProviderName.Contains(_config.SelectedProvider, StringComparison.OrdinalIgnoreCase));
            }

            // If still no match, try common name mappings
            if (selectedProvider == null)
            {
                selectedProvider = _config.SelectedProvider.ToLower() switch
                {
                    "ollama" or "ollama local" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Ollama", StringComparison.OrdinalIgnoreCase)),
                    "lmstudio" or "lm studio" or "lm studio local" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("LM Studio", StringComparison.OrdinalIgnoreCase)),
                    "openai" or "gpt" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("OpenAI", StringComparison.OrdinalIgnoreCase) ||
                        p.ProviderName.Contains("GPT", StringComparison.OrdinalIgnoreCase)),
                    "anthropic" or "claude" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Anthropic", StringComparison.OrdinalIgnoreCase) ||
                        p.ProviderName.Contains("Claude", StringComparison.OrdinalIgnoreCase)),
                    "azure" or "azure openai" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Azure", StringComparison.OrdinalIgnoreCase)),
                    "aws" or "bedrock" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("AWS", StringComparison.OrdinalIgnoreCase) ||
                        p.ProviderName.Contains("Bedrock", StringComparison.OrdinalIgnoreCase)),
                    "perplexity" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Perplexity", StringComparison.OrdinalIgnoreCase)),
                    "grok" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Grok", StringComparison.OrdinalIgnoreCase)),
                    "huggingface" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("HuggingFace", StringComparison.OrdinalIgnoreCase)),
                    "openaigpt4omini" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains(AIProviderNames.OCRTextParserAI, StringComparison.OrdinalIgnoreCase)),
                    _ => null
                };
            }

            if (selectedProvider == null)
            {
                _logger.LogWarning(
                    "Configured provider '{ConfiguredProvider}' not found. Available providers: {AvailableProviders}. Using first available provider.",
                    _config.SelectedProvider,
                    string.Join(", ", availableProviders.Select(p => p.ProviderName)));
                return availableProviders.First();
            }

            _logger.LogInformation("Selected provider from configuration: {ProviderName}", selectedProvider.ProviderName);
            return selectedProvider;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to select provider from configuration");
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

            return await provider.CheckHealthAsync();
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
            var selectedProvider = await SelectOCRProvider(request);
            return await selectedProvider.GetCostEstimateAsync(request.Text, request.Options);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cost estimate for request");
            return 0m;
        }
    }
}

