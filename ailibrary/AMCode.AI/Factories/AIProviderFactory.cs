using AMCode.AI.Configurations;
using AMCode.AI.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace AMCode.AI.Factories;

/// <summary>
/// Factory for creating AI providers dynamically.
/// Uses the AIProvider property from configuration sections to determine which provider implementation to use.
/// </summary>
public class AIProviderFactory : IAIProviderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AIProviderFactory> _logger;
    private readonly Dictionary<string, Func<IServiceProvider, IAIProvider>> _providers;

    public AIProviderFactory(IServiceProvider serviceProvider, ILogger<AIProviderFactory> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _providers = new Dictionary<string, Func<IServiceProvider, IAIProvider>>();
    }

    public IAIProvider CreateProvider()
    {
        // Get the default provider from registered providers or configuration
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var selectedProviderSection = configuration["AI:Provider"] ?? "OpenAI";

        _logger.LogInformation("Creating provider from section {SectionName}", selectedProviderSection);

        // Read the AIProvider property from the specific configuration section
        var aiProviderType = configuration[$"AI:{selectedProviderSection}:AIProvider"];

        if (string.IsNullOrWhiteSpace(aiProviderType))
        {
            _logger.LogWarning("AIProvider property not found in AI:{SectionName}, falling back to section name matching", selectedProviderSection);
            aiProviderType = selectedProviderSection;
        }
        else
        {
            _logger.LogDebug("Found AIProvider type '{AIProviderType}' in section AI:{SectionName}", aiProviderType, selectedProviderSection);
        }

        // Try to get from registered providers first
        if (_providers.TryGetValue(selectedProviderSection, out var factory))
        {
            return factory(_serviceProvider);
        }

        // Find provider using the AIProvider type mapping
        var provider = FindProviderByAIProviderType(aiProviderType, selectedProviderSection);

        if (provider == null)
        {
            throw new InvalidOperationException($"Provider for AIProvider type '{aiProviderType}' (section: '{selectedProviderSection}') not found");
        }

        return provider;
    }

    public IAIProvider CreateRecipeTextParserProvider()
    {
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var providerSection = configuration["AI:OCRTextParserProvider"] ?? "OCRTextParserAI";

        _logger.LogInformation("Creating recipe text parser provider from section {SectionName}", providerSection);

        // Read the AIProvider property from the specific configuration section
        var aiProviderType = configuration[$"AI:{providerSection}:AIProvider"];

        if (string.IsNullOrWhiteSpace(aiProviderType))
        {
            _logger.LogWarning("AIProvider property not found in AI:{SectionName}, falling back to section name matching", providerSection);
            aiProviderType = providerSection;
        }
        else
        {
            _logger.LogDebug("Found AIProvider type '{AIProviderType}' in section AI:{SectionName}", aiProviderType, providerSection);
        }

        // Try to get from registered providers first
        if (_providers.TryGetValue(providerSection, out var factory))
        {
            return factory(_serviceProvider);
        }

        // Find provider using the AIProvider type mapping
        var provider = FindProviderByAIProviderType(aiProviderType, providerSection);

        if (provider == null)
        {
            throw new InvalidOperationException($"Recipe text parser provider for AIProvider type '{aiProviderType}' (section: '{providerSection}') not found");
        }

        return provider;
    }

    /// <summary>
    /// Create the primary AI provider based on AI:Provider configuration.
    /// Uses the AIProvider property from the specified section to determine the provider type.
    /// </summary>
    /// <returns>Configured AI provider, or null if not found</returns>
    public IAIProvider? CreateAIProvider()
    {
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var providerSection = configuration["AI:Provider"] ?? configuration["AI:SelectedProvider"];

        if (string.IsNullOrWhiteSpace(providerSection))
        {
            _logger.LogWarning("AI:Provider is not configured");
            return null;
        }

        _logger.LogInformation("Creating AI provider from section {SectionName}", providerSection);

        // Read the AIProvider property from the specific configuration section
        var aiProviderType = configuration[$"AI:{providerSection}:AIProvider"];

        if (string.IsNullOrWhiteSpace(aiProviderType))
        {
            _logger.LogDebug("AIProvider property not found in AI:{SectionName}, falling back to section name matching", providerSection);
            aiProviderType = providerSection;
        }

        var provider = FindProviderByAIProviderType(aiProviderType, providerSection);
        if (provider == null)
        {
            _logger.LogWarning("AI provider for AIProvider type '{AIProviderType}' (section: '{SectionName}') not found", aiProviderType, providerSection);
        }

        return provider;
    }

    /// <summary>
    /// Create the fallback AI provider based on AI:FallbackProvider configuration.
    /// Uses the AIProvider property from the specified section to determine the provider type.
    /// </summary>
    /// <returns>Configured fallback AI provider, or null if not found</returns>
    public IAIProvider? CreateFallbackAIProvider()
    {
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var providerSection = configuration["AI:FallbackProvider"];

        if (string.IsNullOrWhiteSpace(providerSection))
        {
            _logger.LogDebug("AI:FallbackProvider is not configured");
            return null;
        }

        _logger.LogInformation("Creating fallback AI provider from section {SectionName}", providerSection);

        // Read the AIProvider property from the specific configuration section
        var aiProviderType = configuration[$"AI:{providerSection}:AIProvider"];

        if (string.IsNullOrWhiteSpace(aiProviderType))
        {
            _logger.LogDebug("AIProvider property not found in AI:{SectionName}, falling back to section name matching", providerSection);
            aiProviderType = providerSection;
        }

        var provider = FindProviderByAIProviderType(aiProviderType, providerSection);
        if (provider == null)
        {
            _logger.LogWarning("Fallback AI provider for AIProvider type '{AIProviderType}' (section: '{SectionName}') not found", aiProviderType, providerSection);
        }

        return provider;
    }

    /// <summary>
    /// Create the primary OCR text parser provider based on AI:OCRTextParserProvider configuration.
    /// Uses the AIProvider property from the specified section to determine the provider type.
    /// </summary>
    /// <returns>Configured OCR text parser provider, or null if not found</returns>
    public IAIProvider? CreateOCRTextParserProvider()
    {
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var providerSection = configuration["AI:OCRTextParserProvider"];

        if (string.IsNullOrWhiteSpace(providerSection))
        {
            _logger.LogWarning("AI:OCRTextParserProvider is not configured");
            return null;
        }

        _logger.LogInformation("Creating OCR text parser provider from section {SectionName}", providerSection);

        // Read the AIProvider property from the specific configuration section
        var aiProviderType = configuration[$"AI:{providerSection}:AIProvider"];

        if (string.IsNullOrWhiteSpace(aiProviderType))
        {
            _logger.LogDebug("AIProvider property not found in AI:{SectionName}, falling back to section name matching", providerSection);
            aiProviderType = providerSection;
        }

        var provider = FindProviderByAIProviderType(aiProviderType, providerSection);
        if (provider == null)
        {
            _logger.LogWarning("OCR text parser provider for AIProvider type '{AIProviderType}' (section: '{SectionName}') not found", aiProviderType, providerSection);
        }

        return provider;
    }

    /// <summary>
    /// Create the fallback OCR text parser provider based on AI:FallbackOCRTextParserProvider configuration.
    /// Uses the AIProvider property from the specified section to determine the provider type.
    /// </summary>
    /// <returns>Configured fallback OCR text parser provider, or null if not found</returns>
    public IAIProvider? CreateFallbackOCRTextParserProvider()
    {
        var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
        var providerSection = configuration["AI:FallbackOCRTextParserProvider"];

        if (string.IsNullOrWhiteSpace(providerSection))
        {
            _logger.LogDebug("AI:FallbackOCRTextParserProvider is not configured");
            return null;
        }

        _logger.LogInformation("Creating fallback OCR text parser provider from section {SectionName}", providerSection);

        // Read the AIProvider property from the specific configuration section
        var aiProviderType = configuration[$"AI:{providerSection}:AIProvider"];

        if (string.IsNullOrWhiteSpace(aiProviderType))
        {
            _logger.LogDebug("AIProvider property not found in AI:{SectionName}, falling back to section name matching", providerSection);
            aiProviderType = providerSection;
        }

        var provider = FindProviderByAIProviderType(aiProviderType, providerSection);
        if (provider == null)
        {
            _logger.LogWarning("Fallback OCR text parser provider for AIProvider type '{AIProviderType}' (section: '{SectionName}') not found", aiProviderType, providerSection);
        }

        return provider;
    }

    /// <summary>
    /// Finds an AI provider by its AIProvider type value from configuration.
    /// Uses the AIProviderRegistry.ProviderNameAliases to determine which provider names to look for.
    /// </summary>
    /// <param name="aiProviderType">The AIProvider type value (e.g., "openai", "azure-openai", "aws-bedrock")</param>
    /// <param name="configSectionName">The configuration section name for logging purposes</param>
    /// <returns>The matching AI provider, or null if not found</returns>
    private IAIProvider? FindProviderByAIProviderType(string aiProviderType, string configSectionName)
    {
        var providers = _serviceProvider.GetServices<IAIProvider>().ToList();
        if (providers.Count == 0)
        {
            _logger.LogWarning("No AI providers registered in service collection");
            return null;
        }

        // First, try to find using the AIProvider type mapping
        if (AIProviderRegistry.ProviderNameAliases.TryGetValue(aiProviderType, out var targetProviderNames))
        {
            foreach (var targetName in targetProviderNames)
            {
                var provider = providers.FirstOrDefault(p =>
                    p.ProviderName.Equals(targetName, StringComparison.OrdinalIgnoreCase) ||
                    p.ProviderName.Contains(targetName, StringComparison.OrdinalIgnoreCase));

                if (provider != null)
                {
                    _logger.LogDebug("Found provider '{ProviderName}' for AIProvider type '{AIProviderType}'",
                        provider.ProviderName, aiProviderType);
                    return provider;
                }
            }
        }

        // If no mapping found, try to match by config section name (backward compatibility)
        _logger.LogDebug("No mapping found for AIProvider type '{AIProviderType}', trying section name '{SectionName}'",
            aiProviderType, configSectionName);

        return FindProviderByName(configSectionName) ?? FindProviderByName(aiProviderType);
    }

    /// <summary>
    /// Finds an AI provider by name, supporting exact match, partial match, and common aliases
    /// </summary>
    /// <param name="providerName">The provider name to find</param>
    /// <returns>The matching AI provider, or null if not found</returns>
    private IAIProvider? FindProviderByName(string providerName)
    {
        // Try to get from registered providers first
        if (_providers.TryGetValue(providerName, out var factory))
        {
            return factory(_serviceProvider);
        }

        var providers = _serviceProvider.GetServices<IAIProvider>().ToList();
        if (providers.Count == 0)
        {
            _logger.LogWarning("No AI providers registered in service collection");
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
    private IAIProvider? FindProviderByAlias(string alias, List<IAIProvider> providers)
    {
        var normalizedAlias = alias.ToLowerInvariant().Trim();

        // Use registry instead of inline map
        if (!AIProviderRegistry.ProviderNameAliases.TryGetValue(normalizedAlias, out var targetNames))
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

    public IAIProvider CreateCustomProvider(string name, Type providerType, IConfiguration configuration)
    {
        _logger.LogInformation("Creating custom provider {ProviderType} with name {Name}", providerType.Name, name);

        if (!typeof(GenericAIProvider).IsAssignableFrom(providerType))
        {
            throw new ArgumentException($"Provider type must inherit from {nameof(GenericAIProvider)}");
        }

        try
        {
            var logger = _serviceProvider.GetRequiredService<ILogger>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            // Get configuration type
            var configType = GetConfigurationType(providerType);
            var config = CreateConfiguration(configType, configuration, name);

            // Create provider instance
            var constructor = GetProviderConstructor(providerType, configType);
            if (constructor == null)
            {
                throw new InvalidOperationException($"No suitable constructor found for {providerType.Name}");
            }

            var parameters = new object[] { logger, httpClientFactory, config };
            var provider = (IAIProvider)constructor.Invoke(parameters);

            _logger.LogInformation("Successfully created custom provider {ProviderType}", providerType.Name);
            return provider;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create custom provider {ProviderType}", providerType.Name);
            throw new InvalidOperationException($"Failed to create custom provider {providerType.Name}: {ex.Message}", ex);
        }
    }

    public void RegisterProvider<T>(string name, Func<IServiceProvider, IAIProvider> factory) where T : GenericAIProvider
    {
        _logger.LogInformation("Registering provider factory for {ProviderType} with name {Name}", typeof(T).Name, name);
        _providers[name] = factory;
    }

    public IEnumerable<string> GetRegisteredProviders()
    {
        return _providers.Keys.ToList();
    }

    public bool IsProviderRegistered(string name)
    {
        return _providers.ContainsKey(name);
    }

    /// <summary>
    /// Create a provider by name with optional model override.
    /// Reuses the existing FindProviderByName lookup strategy.
    /// </summary>
    public IAIProvider? CreateProviderByName(string providerName, string? modelOverride = null)
    {
        if (string.IsNullOrWhiteSpace(providerName))
        {
            return null;
        }

        _logger.LogDebug("Creating provider by name '{ProviderName}' with model override '{ModelOverride}'",
            providerName, modelOverride ?? "(none)");

        // Use the existing multi-strategy lookup
        var provider = FindProviderByName(providerName);

        if (provider == null)
        {
            // Also try the AIProvider type mapping (e.g., "openai" → OpenAI GPT)
            provider = FindProviderByAIProviderType(providerName, providerName);
        }

        return provider;
    }

    private Type GetConfigurationType<T>() where T : GenericAIProvider
    {
        return GetConfigurationType(typeof(T));
    }

    private Type GetConfigurationType(Type providerType)
    {
        // Look for configuration type in the same namespace
        var namespaceName = providerType.Namespace;
        var configTypeName = $"{providerType.Name}Configuration";
        var fullConfigTypeName = $"{namespaceName}.{configTypeName}";

        var configType = providerType.Assembly.GetType(fullConfigTypeName);
        if (configType != null)
        {
            return configType;
        }

        // Fallback: look for any type ending with "Configuration" in the same namespace
        var configurationTypes = providerType.Assembly.GetTypes()
            .Where(t => t.Namespace == namespaceName && t.Name.EndsWith("Configuration"))
            .ToList();

        if (configurationTypes.Count == 1)
        {
            return configurationTypes[0];
        }

        // If no specific configuration found, return a generic configuration type
        return typeof(object);
    }

    private object CreateConfiguration(Type configType, IConfiguration configuration, string name)
    {
        if (configType == typeof(object))
        {
            return new object();
        }

        var configInstance = Activator.CreateInstance(configType);
        if (configInstance == null)
        {
            throw new InvalidOperationException($"Failed to create instance of {configType.Name}");
        }

        // Bind configuration
        var configSection = configuration.GetSection($"AI:{name}");
        if (configSection.Exists())
        {
            configSection.Bind(configInstance);
        }

        return configInstance;
    }

    private ConstructorInfo? GetProviderConstructor<T>(Type configType) where T : GenericAIProvider
    {
        return GetProviderConstructor(typeof(T), configType);
    }

    private ConstructorInfo? GetProviderConstructor(Type providerType, Type configType)
    {
        // Look for constructor with ILogger, IHttpClientFactory, and configuration
        var constructors = providerType.GetConstructors();

        foreach (var constructor in constructors)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length >= 2)
            {
                var firstParam = parameters[0].ParameterType;
                var secondParam = parameters[1].ParameterType;

                // Check if first two parameters are ILogger and IHttpClientFactory
                if (IsLoggerType(firstParam) && secondParam == typeof(IHttpClientFactory))
                {
                    return constructor;
                }
            }
        }

        return null;
    }

    private static bool IsLoggerType(Type type)
    {
        return type.IsGenericType &&
               type.GetGenericTypeDefinition() == typeof(ILogger<>) ||
               type == typeof(ILogger);
    }
}
