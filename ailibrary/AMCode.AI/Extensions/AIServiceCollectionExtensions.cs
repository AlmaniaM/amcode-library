using AMCode.AI.Configurations;
using AMCode.AI.Enums;
using AMCode.AI.Factories;
using AMCode.AI.Providers;
using AMCode.AI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace AMCode.AI.Extensions;

/// <summary>
/// Extension methods for registering AI services
/// </summary>
public static class AIServiceCollectionExtensions
{
    /// <summary>
    /// Add multi-cloud AI services to the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddMultiCloudAI(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configurations
        services.Configure<AIConfiguration>(configuration.GetSection("AI"));
        services.Configure<OpenAIConfiguration>(configuration.GetSection("AI:OpenAI"));
        services.Configure<OpenAIConfiguration>("OCRTextParserAI", configuration.GetSection("AI:OCRTextParserAI"));
        services.Configure<AnthropicConfiguration>(configuration.GetSection("AI:Anthropic"));
        services.Configure<GrokConfiguration>(configuration.GetSection("AI:Grok"));
        services.Configure<AWSBedrockConfiguration>(configuration.GetSection("AI:AWSBedrock"));
        services.Configure<OllamaConfiguration>(configuration.GetSection("AI:Ollama"));
        services.Configure<LMStudioConfiguration>(configuration.GetSection("AI:LMStudio"));
        services.Configure<HuggingFaceConfiguration>(configuration.GetSection("AI:HuggingFace"));
        services.Configure<AzureOpenAIConfiguration>("AzureOpenAIGPT5Nano", configuration.GetSection("AI:AzureOpenAIGPT5Nano"));
        services.Configure<PerplexityConfiguration>(configuration.GetSection("AI:Perplexity"));

        // Register HTTP clients
        services.AddHttpClient<OpenAIGPTProvider>();
        services.AddHttpClient<AnthropicClaudeProvider>();
        services.AddHttpClient<GrokProvider>();
        services.AddHttpClient<AWSBedrockProvider>();
        services.AddHttpClient<OllamaAIProvider>();
        services.AddHttpClient<LMStudioAIProvider>();
        services.AddHttpClient<HuggingFaceAIProvider>();
        services.AddHttpClient<AzureOpenAIProvider>();
        services.AddHttpClient<AzureOpenAISdkProvider>();
        services.AddHttpClient<PerplexityProvider>();

        // Register providers
        services.AddSingleton<IAIProvider>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<OpenAIGPTProvider>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var config = provider.GetRequiredService<IOptions<OpenAIConfiguration>>().Value;
            var promptBuilder = provider.GetRequiredService<PromptBuilderService>();
            return new OpenAIGPTProvider(logger, httpClientFactory, Options.Create(config), promptBuilder, null);
        });

        // Register OCRTextParserAI provider (second instance with different configuration)
        services.AddSingleton<IAIProvider>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<OpenAIGPTProvider>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var configMonitor = provider.GetRequiredService<IOptionsMonitor<OpenAIConfiguration>>();
            var config = configMonitor.Get("OCRTextParserAI");
            var promptBuilder = provider.GetRequiredService<PromptBuilderService>();
            return new OpenAIGPTProvider(logger, httpClientFactory, Options.Create(config), promptBuilder, "OpenAI GPT-4o Mini");
        });

        services.AddSingleton<IAIProvider>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<AzureComputerVisionProvider>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var config = provider.GetRequiredService<IOptions<AzureComputerVisionConfiguration>>().Value;
            var promptBuilder = provider.GetRequiredService<PromptBuilderService>();
            return new AzureComputerVisionProvider(logger, httpClientFactory, Options.Create(config), promptBuilder);
        });

        // Register AzureOpenAISdkProvider with AzureOpenAIGPT5Nano configuration (named options)
        services.AddSingleton<IAIProvider>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<AzureOpenAISdkProvider>>();
            var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
            var configMonitor = provider.GetRequiredService<IOptionsMonitor<AzureOpenAIConfiguration>>();
            var config = configMonitor.Get("AzureOpenAIGPT5Nano");
            var promptBuilder = provider.GetRequiredService<PromptBuilderService>();
            return new AzureOpenAISdkProvider(logger, httpClientFactory, Options.Create(config), promptBuilder);
        });

        services.AddSingleton<IAIProvider, AnthropicClaudeProvider>();
        services.AddSingleton<IAIProvider, GrokProvider>();
        services.AddSingleton<IAIProvider, AWSBedrockProvider>();
        services.AddSingleton<IAIProvider, OllamaAIProvider>();
        services.AddSingleton<IAIProvider, LMStudioAIProvider>();
        services.AddSingleton<IAIProvider, HuggingFaceAIProvider>();
        services.AddSingleton<IAIProvider, AzureOpenAIProvider>();
        services.AddSingleton<IAIProvider, PerplexityProvider>();
        services.AddSingleton<IAIProvider, AzureComputerVisionProvider>();

        // Register services
        services.AddSingleton<PromptBuilderService>();
        services.AddSingleton<ICostAnalyzer, CostAnalyzer>();
        services.AddSingleton<IRecipeParserService, EnhancedHybridAIService>();
        services.AddSingleton<IRecipeValidationService, RecipeValidationService>();

        // Register AI provider factory
        services.AddSingleton<IAIProviderFactory>(provider =>
        {
            var serviceProvider = provider.GetRequiredService<IServiceProvider>();
            var logger = provider.GetRequiredService<ILogger<AIProviderFactory>>();
            return new AIProviderFactory(serviceProvider, logger);
        });

        // Discover and register dynamic providers from configuration
        // Note: This must be called before building the service provider
        DiscoverAndRegisterDynamicAIProviders(services, configuration);

        return services;
    }

    /// <summary>
    /// Add custom AI provider
    /// </summary>
    /// <typeparam name="T">Provider type</typeparam>
    /// <param name="services">Service collection</param>
    /// <param name="name">Provider name</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddCustomAIProvider<T>(this IServiceCollection services, string name, IConfiguration configuration)
        where T : class, IAIProvider
    {
        services.AddHttpClient<T>();
        services.AddSingleton<IAIProvider, T>();

        return services;
    }

    /// <summary>
    /// Add AI provider with custom factory
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="factory">Provider factory</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddAIProvider(this IServiceCollection services, Func<IServiceProvider, IAIProvider> factory)
    {
        services.AddSingleton<IAIProvider>(factory);
        return services;
    }

    /// <summary>
    /// Configure AI provider selection strategy
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="strategy">Selection strategy</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection ConfigureAIProviderSelection(this IServiceCollection services, AIProviderSelectionStrategy strategy)
    {
        services.Configure<AIConfiguration>(config => config.ProviderSelectionStrategy = strategy.ToString());
        return services;
    }

    /// <summary>
    /// Configure AI cost tracking
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="enableTracking">Enable cost tracking</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection ConfigureAICostTracking(this IServiceCollection services, bool enableTracking = true)
    {
        services.Configure<AIConfiguration>(config => config.EnableCostTracking = enableTracking);
        return services;
    }

    /// <summary>
    /// Configure AI health monitoring
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="enableMonitoring">Enable health monitoring</param>
    /// <param name="checkInterval">Health check interval</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection ConfigureAIHealthMonitoring(this IServiceCollection services, bool enableMonitoring = true, TimeSpan? checkInterval = null)
    {
        services.Configure<AIConfiguration>(config =>
        {
            config.EnableHealthMonitoring = enableMonitoring;
            if (checkInterval.HasValue)
            {
                config.HealthCheckInterval = checkInterval.Value;
            }
        });
        return services;
    }

    /// <summary>
    /// Configure AI fallback providers
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="enableFallback">Enable fallback providers</param>
    /// <param name="maxAttempts">Maximum fallback attempts</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection ConfigureAIFallbackProviders(this IServiceCollection services, bool enableFallback = true, int maxAttempts = 2)
    {
        services.Configure<AIConfiguration>(config =>
        {
            config.EnableFallbackProviders = enableFallback;
            config.MaxFallbackAttempts = maxAttempts;
        });
        return services;
    }

    /// <summary>
    /// Discovers and registers dynamic AI providers from configuration sections
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    private static void DiscoverAndRegisterDynamicAIProviders(IServiceCollection services, IConfiguration configuration)
    {
        var aiSection = configuration.GetSection("AI");
        if (!aiSection.Exists())
        {
            return;
        }

        // Known top-level properties to exclude from dynamic discovery
        var excludedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Provider",
            "FallbackProvider",
            "OCRTextParserProvider",
            "FallbackOCRTextParserProvider",
            "MaxRetries",
            "ConfidenceThreshold",
            "DefaultTimeout",
            "DefaultMaxTokens",
            "DefaultTemperature",
            "EnableCostTracking",
            "EnableHealthMonitoring",
            "HealthCheckInterval",
            "ProviderSelectionStrategy",
            "SmartSelectionStrategy",
            "SelectedProvider",
            "EnableFallbackProviders",
            "MaxFallbackAttempts"
        };

        // Use a simple logging approach - we can't get logger from service provider yet
        // Logging will be done via console or we'll skip it for now
        var discoveredCount = 0;

        // Get all child sections
        foreach (var section in aiSection.GetChildren())
        {
            var sectionName = section.Key;

            // Skip excluded properties and already statically registered providers
            if (excludedProperties.Contains(sectionName) || IsStaticallyRegistered(sectionName))
            {
                continue;
            }

            try
            {
                // Check if this section has an AIProvider property
                var aiProviderKey = section["AIProvider"];
                if (string.IsNullOrWhiteSpace(aiProviderKey))
                {
                    continue; // Not a provider configuration section
                }

                // Look up provider type from registry
                if (!AIProviderRegistry.ProviderTypeMap.TryGetValue(aiProviderKey, out var providerType))
                {
                    // Log warning - can't use logger yet, so we'll skip silently or use Console
                    continue;
                }

                // Get configuration type for this provider
                var configType = GetConfigurationTypeForProvider(providerType);
                if (configType == null)
                {
                    continue;
                }

                // Register configuration with named options
                var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == "Configure" &&
                                        m.GetParameters().Length == 3 &&
                                        m.GetParameters()[1].ParameterType == typeof(string) &&
                                        m.GetParameters()[2].ParameterType == typeof(IConfiguration));

                if (configureMethod != null)
                {
                    var genericMethod = configureMethod.MakeGenericMethod(configType);
                    genericMethod.Invoke(null, new object[] { services, sectionName, section });
                }

                // Ensure HTTP client is registered for this provider type (if not already)
                var httpClientMethod = typeof(HttpClientFactoryServiceCollectionExtensions)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == "AddHttpClient" &&
                                        m.GetParameters().Length == 1 &&
                                        m.IsGenericMethod);

                if (httpClientMethod != null)
                {
                    var genericHttpClientMethod = httpClientMethod.MakeGenericMethod(providerType);
                    genericHttpClientMethod.Invoke(null, new object[] { services });
                }

                // Register provider instance with section name as provider name
                services.AddSingleton<IAIProvider>(serviceProvider =>
                {
                    return CreateDynamicProviderInstance(serviceProvider, providerType, configType, sectionName);
                });

                discoveredCount++;
            }
            catch (Exception)
            {
                // Continue with other providers - don't fail startup
                // Error logging will happen when provider is actually instantiated
            }
        }
    }

    /// <summary>
    /// Checks if a provider section name is already statically registered
    /// </summary>
    private static bool IsStaticallyRegistered(string sectionName)
    {
        var staticSections = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "OpenAI",
            "OCRTextParserAI",
            "Anthropic",
            "Grok",
            "AWSBedrock",
            "Ollama",
            "LMStudio",
            "HuggingFace",
            "AzureOpenAIGPT5Nano",
            "Perplexity",
            "AzureComputerVision"
        };

        return staticSections.Contains(sectionName);
    }

    /// <summary>
    /// Gets the configuration type for a provider type
    /// </summary>
    private static Type? GetConfigurationTypeForProvider(Type providerType)
    {
        // Map provider types to their configuration types
        var providerConfigMap = new Dictionary<Type, Type>
        {
            { typeof(OpenAIGPTProvider), typeof(OpenAIConfiguration) },
            { typeof(AzureOpenAIProvider), typeof(AzureOpenAIConfiguration) },
            { typeof(AzureOpenAISdkProvider), typeof(AzureOpenAIConfiguration) },
            { typeof(AnthropicClaudeProvider), typeof(AnthropicConfiguration) },
            { typeof(GrokProvider), typeof(GrokConfiguration) },
            { typeof(AWSBedrockProvider), typeof(AWSBedrockConfiguration) },
            { typeof(OllamaAIProvider), typeof(OllamaConfiguration) },
            { typeof(LMStudioAIProvider), typeof(LMStudioConfiguration) },
            { typeof(HuggingFaceAIProvider), typeof(HuggingFaceConfiguration) },
            { typeof(PerplexityProvider), typeof(PerplexityConfiguration) },
            { typeof(AzureComputerVisionProvider), typeof(AzureComputerVisionConfiguration) }
        };

        if (providerConfigMap.TryGetValue(providerType, out var configType))
        {
            return configType;
        }

        // Fallback: try to find configuration type by convention
        var namespaceName = providerType.Namespace?.Replace(".Providers", ".Configurations");
        if (namespaceName != null)
        {
            // Try different naming patterns
            var providerName = providerType.Name;
            var patterns = new[]
            {
                providerName.Replace("Provider", "Configuration"),
                providerName.Replace("AIProvider", "Configuration"),
                providerName.Replace("Provider", "") + "Configuration"
            };

            foreach (var pattern in patterns)
            {
                var fullName = $"{namespaceName}.{pattern}";
                var configTypeFound = providerType.Assembly.GetType(fullName);
                if (configTypeFound != null)
                {
                    return configTypeFound;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Creates a dynamic provider instance using reflection
    /// </summary>
    private static IAIProvider CreateDynamicProviderInstance(
        IServiceProvider serviceProvider,
        Type providerType,
        Type configType,
        string sectionName,
        ILogger? logger = null)
    {
        try
        {
            // Get required services
            var loggerType = typeof(ILogger<>).MakeGenericType(providerType);
            var loggerInstance = serviceProvider.GetRequiredService(loggerType);
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var promptBuilder = serviceProvider.GetRequiredService<PromptBuilderService>();

            // Get configuration using named options
            var optionsMonitorType = typeof(IOptionsMonitor<>).MakeGenericType(configType);
            var optionsMonitor = serviceProvider.GetRequiredService(optionsMonitorType);
            var getMethod = optionsMonitorType.GetMethod("Get", new[] { typeof(string) });
            var configInstance = getMethod?.Invoke(optionsMonitor, new object[] { sectionName });

            // Create IOptions<T> wrapper
            var optionsType = typeof(IOptions<>).MakeGenericType(configType);
            var optionsCreateMethod = typeof(Options).GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
            var genericOptionsCreate = optionsCreateMethod?.MakeGenericMethod(configType);
            var optionsWrapper = genericOptionsCreate?.Invoke(null, new object[] { configInstance });

            // Find constructor
            var constructors = providerType.GetConstructors();
            ConstructorInfo? selectedConstructor = null;
            object[]? constructorParams = null;

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                // Look for constructor with: ILogger<T>, IHttpClientFactory, IOptions<TConfig>, PromptBuilderService
                if (parameters.Length >= 4 &&
                    IsLoggerType(parameters[0].ParameterType) &&
                    parameters[1].ParameterType == typeof(IHttpClientFactory) &&
                    parameters[2].ParameterType.IsGenericType &&
                    parameters[2].ParameterType.GetGenericTypeDefinition() == typeof(IOptions<>) &&
                    parameters[3].ParameterType == typeof(PromptBuilderService))
                {
                    selectedConstructor = constructor;
                    constructorParams = new object[] { loggerInstance, httpClientFactory, optionsWrapper, promptBuilder };
                    break;
                }
                // Look for constructor with: ILogger<T>, IHttpClientFactory, IOptions<TConfig>, PromptBuilderService, string? (providerName)
                else if (parameters.Length == 5 &&
                         IsLoggerType(parameters[0].ParameterType) &&
                         parameters[1].ParameterType == typeof(IHttpClientFactory) &&
                         parameters[2].ParameterType.IsGenericType &&
                         parameters[2].ParameterType.GetGenericTypeDefinition() == typeof(IOptions<>) &&
                         parameters[3].ParameterType == typeof(PromptBuilderService) &&
                         parameters[4].ParameterType == typeof(string))
                {
                    selectedConstructor = constructor;
                    constructorParams = new object[] { loggerInstance, httpClientFactory, optionsWrapper, promptBuilder, sectionName };
                    break;
                }
            }

            if (selectedConstructor == null || constructorParams == null)
            {
                throw new InvalidOperationException($"No suitable constructor found for {providerType.Name}");
            }

            var providerInstance = selectedConstructor.Invoke(constructorParams) as IAIProvider;
            if (providerInstance == null)
            {
                throw new InvalidOperationException($"Failed to create instance of {providerType.Name}");
            }

            return providerInstance;
        }
        catch (Exception ex)
        {
            // Get logger from service provider if available
            try
            {
                var loggerType = typeof(ILogger<>).MakeGenericType(typeof(AIServiceCollectionExtensions));
                var loggerInstance = serviceProvider.GetService(loggerType) as ILogger;
                loggerInstance?.LogError(ex, "Failed to create dynamic provider instance for {ProviderType} in section {SectionName}", providerType.Name, sectionName);
            }
            catch
            {
                // If we can't get logger, just throw the original exception
            }
            throw;
        }
    }

    /// <summary>
    /// Checks if a type is a logger type
    /// </summary>
    private static bool IsLoggerType(Type type)
    {
        return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ILogger<>)) ||
               type == typeof(ILogger);
    }
}
