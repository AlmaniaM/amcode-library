using AMCode.AI.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace AMCode.AI.Factories;

/// <summary>
/// Factory for creating AI providers dynamically
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

    public IAIProvider CreateProvider<T>(string name, IConfiguration configuration) where T : GenericAIProvider
    {
        _logger.LogInformation("Creating provider {ProviderType} with name {Name}", typeof(T).Name, name);

        try
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<T>>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();

            // Get configuration type
            var configType = GetConfigurationType<T>();
            var config = CreateConfiguration(configType, configuration, name);

            // Create provider instance
            var constructor = GetProviderConstructor<T>(configType);
            if (constructor == null)
            {
                throw new InvalidOperationException($"No suitable constructor found for {typeof(T).Name}");
            }

            var parameters = new object[] { logger, httpClientFactory, config };
            var provider = (IAIProvider)constructor.Invoke(parameters);

            _logger.LogInformation("Successfully created provider {ProviderType}", typeof(T).Name);
            return provider;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create provider {ProviderType}", typeof(T).Name);
            throw new InvalidOperationException($"Failed to create provider {typeof(T).Name}: {ex.Message}", ex);
        }
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
