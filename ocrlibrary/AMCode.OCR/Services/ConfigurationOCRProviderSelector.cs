using AMCode.OCR.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AMCode.OCR.Configurations;

namespace AMCode.OCR.Services;

/// <summary>
/// Configuration-based OCR provider selector that uses a manually configured provider
/// </summary>
public class ConfigurationOCRProviderSelector : IOCRProviderSelector
{
    private readonly IEnumerable<IOCRProvider> _providers;
    private readonly ILogger<ConfigurationOCRProviderSelector> _logger;
    private readonly OCRConfiguration _config;

    public ConfigurationOCRProviderSelector(
        IEnumerable<IOCRProvider> providers,
        ILogger<ConfigurationOCRProviderSelector> logger,
        IOptions<OCRConfiguration> config)
    {
        _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<IOCRProvider> SelectOCRProvider(OCRRequest request)
    {
        try
        {
            _logger.LogDebug("Selecting provider from configuration: {Provider}", _config.Provider);

            var availableProviders = await GetAvailableProvidersAsync();
            if (!availableProviders.Any())
            {
                throw new InvalidOperationException("No OCR providers are available");
            }

            // If no provider is configured, use first available
            if (string.IsNullOrWhiteSpace(_config.Provider))
            {
                _logger.LogWarning("No provider specified in configuration (OCR:Provider). Using first available provider.");
                return availableProviders.First();
            }

            // Try to find exact match first
            var selectedProvider = availableProviders.FirstOrDefault(p =>
                p.ProviderName.Equals(_config.Provider, StringComparison.OrdinalIgnoreCase));

            // If no exact match, try partial match
            if (selectedProvider == null)
            {
                selectedProvider = availableProviders.FirstOrDefault(p =>
                    p.ProviderName.Contains(_config.Provider, StringComparison.OrdinalIgnoreCase));
            }

            // If still no match, try common name mappings
            if (selectedProvider == null)
            {
                selectedProvider = _config.Provider.ToLower() switch
                {
                    "azure" or "azure computer vision" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Azure", StringComparison.OrdinalIgnoreCase)),
                    "aws" or "textract" or "awsbedrock" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("AWS", StringComparison.OrdinalIgnoreCase) ||
                        p.ProviderName.Contains("Textract", StringComparison.OrdinalIgnoreCase)),
                    "google" or "gcp" or "google cloud vision" or "gcpvision" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Google", StringComparison.OrdinalIgnoreCase) ||
                        p.ProviderName.Contains("GCP", StringComparison.OrdinalIgnoreCase)),
                    "paddle" or "paddleocr" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Paddle", StringComparison.OrdinalIgnoreCase)),
                    "bedrock" or "aws bedrock" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("Bedrock", StringComparison.OrdinalIgnoreCase)),
                    "documentai" or "gcp document ai" => availableProviders.FirstOrDefault(p =>
                        p.ProviderName.Contains("DocumentAI", StringComparison.OrdinalIgnoreCase)),
                    _ => null
                };
            }

            if (selectedProvider == null)
            {
                _logger.LogWarning(
                    "Configured provider '{ConfiguredProvider}' not found. Available providers: {AvailableProviders}. Using first available provider.",
                    _config.Provider,
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

    public async Task<IEnumerable<IOCRProvider>> GetAvailableProvidersAsync()
    {
        try
        {
            var availableProviders = new List<IOCRProvider>();

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
            return Enumerable.Empty<IOCRProvider>();
        }
    }

    public async Task<OCRProviderHealth> GetProviderHealthAsync(string providerName)
    {
        try
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

            return await provider.CheckHealthAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get health for provider {Provider}", providerName);
            return new OCRProviderHealth
            {
                IsHealthy = false,
                IsAvailable = false,
                Status = "Health check failed",
                ErrorMessage = ex.Message,
                LastChecked = DateTime.UtcNow
            };
        }
    }

    public async Task<decimal> GetCostEstimateAsync(OCRRequest request)
    {
        try
        {
            var selectedProvider = await SelectOCRProvider(request);
            // Use 0 bytes for estimation (actual size not needed for cost estimation)
            return await selectedProvider.GetCostEstimateAsync(0, request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cost estimate for request");
            return 0m;
        }
    }
}

