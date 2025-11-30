using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using AMCode.OCR.Providers;
using AMCode.OCR.Services;
using AMCode.OCR.Configurations;
using AMCode.OCR.Enums;
using AMCode.OCR.Factories;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Amazon.Textract;
using Google.Cloud.Vision.V1;
using System.Net.Http;
using System.Reflection;

namespace AMCode.OCR.Extensions;

/// <summary>
/// Extension methods for configuring OCR services
/// </summary>
public static class OCRServiceCollectionExtensions
{
    /// <summary>
    /// Adds multi-cloud OCR services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddMultiCloudOCR(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configurations
        services.Configure<OCRConfiguration>(configuration.GetSection("OCR"));
        services.Configure<AzureOCRConfiguration>(configuration.GetSection("OCR:Azure"));
        services.Configure<AWSTextractConfiguration>(configuration.GetSection("OCR:AWS"));
        services.Configure<GoogleVisionConfiguration>(configuration.GetSection("OCR:GCPVision"));
        services.Configure<AWSBedrockOCRConfiguration>(configuration.GetSection("OCR:AWSBedrock"));
        services.Configure<GCPDocumentAIConfiguration>(configuration.GetSection("OCR:GCPDocumentAI"));
        services.Configure<PaddleOCRConfiguration>(configuration.GetSection("OCR:PaddleOCR"));

        // Register HTTP clients
        services.AddHttpClient<AzureComputerVisionOCRService>();
        services.AddHttpClient<AWSTextractOCRService>();
        services.AddHttpClient<GoogleCloudVisionOCRService>();
        services.AddHttpClient<PaddleOCRProvider>();

        // Register Azure Computer Vision client
        services.AddSingleton<ComputerVisionClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<AzureOCRConfiguration>>().Value;
            var logger = provider.GetRequiredService<ILogger<AzureComputerVisionOCRService>>();

            try
            {
                if (string.IsNullOrEmpty(config.ApiKey) || string.IsNullOrEmpty(config.Endpoint))
                {
                    logger.LogWarning("Azure Computer Vision credentials not configured (ApiKey or Endpoint missing). Provider will be marked as unavailable.");
                    return null!; // Return null to allow provider initialization, but mark it as unavailable
                }

                var credentials = new ApiKeyServiceClientCredentials(config.ApiKey);
                return new ComputerVisionClient(credentials)
                {
                    Endpoint = config.Endpoint
                };
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to initialize Azure Computer Vision client. Provider will be marked as unavailable. Error: {Message}", ex.Message);
                return null!; // Return null to allow provider initialization, but mark it as unavailable
            }
        });

        // Register AWS Textract client
        services.AddSingleton<IAmazonTextract>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<AWSTextractConfiguration>>().Value;
            var logger = provider.GetRequiredService<ILogger<AWSTextractOCRService>>();

            try
            {
                if (string.IsNullOrEmpty(config.AccessKeyId) || string.IsNullOrEmpty(config.SecretAccessKey) || string.IsNullOrEmpty(config.Region))
                {
                    logger.LogWarning("AWS Textract credentials not configured (AccessKeyId, SecretAccessKey, or Region missing). Provider will be marked as unavailable.");
                    return null!; // Return null to allow provider initialization, but mark it as unavailable
                }

                var clientConfig = new AmazonTextractConfig
                {
                    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(config.Region)
                };
                return new AmazonTextractClient(config.AccessKeyId, config.SecretAccessKey, clientConfig);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to initialize AWS Textract client. Provider will be marked as unavailable. Error: {Message}", ex.Message);
                return null!; // Return null to allow provider initialization, but mark it as unavailable
            }
        });

        // Register Google Cloud Vision client
        services.AddSingleton<ImageAnnotatorClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<GoogleVisionConfiguration>>().Value;
            var logger = provider.GetRequiredService<ILogger<GoogleCloudVisionOCRService>>();

            try
            {
                // Use default credentials (Application Default Credentials)
                // If credentials are not found, this will throw and return null
                // The provider's IsAvailable check will then return false
                return ImageAnnotatorClient.Create();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to initialize Google Cloud Vision client. Provider will be marked as unavailable. Error: {Message}", ex.Message);
                // Return null to allow provider initialization, but mark it as unavailable
                // This allows fallback to other providers instead of failing completely
                return null!; // Use null-forgiving operator since provider handles null client
            }
        });

        // Register OCR providers
        services.AddSingleton<IOCRProvider, AzureComputerVisionOCRService>();
        services.AddSingleton<IOCRProvider, AWSTextractOCRService>();
        services.AddSingleton<IOCRProvider, GoogleCloudVisionOCRService>();
        services.AddSingleton<IOCRProvider, PaddleOCRProvider>();

        // Register OCR provider factory
        services.AddSingleton<IOCRProviderFactory, OCRProviderFactory>();

        // Register provider selector factory
        services.AddSingleton<IOCRProviderSelectorFactory, OCRProviderSelectorFactory>();

        // Register provider selector using factory
        services.AddSingleton<IOCRProviderSelector>(provider =>
        {
            var factory = provider.GetRequiredService<IOCRProviderSelectorFactory>();
            return factory.CreateSelector();
        });
        services.AddSingleton<IOCRService, EnhancedHybridOCRService>();

        // Discover and register dynamic providers from configuration
        DiscoverAndRegisterDynamicOCRProviders(services, configuration);

        return services;
    }

    /// <summary>
    /// Adds Azure Computer Vision OCR service
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddAzureOCR(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureOCRConfiguration>(configuration.GetSection("OCR:Azure"));
        services.AddHttpClient<AzureComputerVisionOCRService>();

        services.AddSingleton<ComputerVisionClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<AzureOCRConfiguration>>().Value;
            var credentials = new ApiKeyServiceClientCredentials(config.ApiKey);
            return new ComputerVisionClient(credentials)
            {
                Endpoint = config.Endpoint
            };
        });

        services.AddSingleton<IOCRProvider, AzureComputerVisionOCRService>();
        services.AddSingleton<IOCRService, EnhancedHybridOCRService>();

        return services;
    }

    /// <summary>
    /// Adds AWS Textract OCR service
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddAWSTextractOCR(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AWSTextractConfiguration>(configuration.GetSection("OCR:AWS"));
        services.AddHttpClient<AWSTextractOCRService>();

        services.AddSingleton<IAmazonTextract>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<AWSTextractConfiguration>>().Value;
            var clientConfig = new AmazonTextractConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(config.Region)
            };
            return new AmazonTextractClient(config.AccessKeyId, config.SecretAccessKey, clientConfig);
        });

        services.AddSingleton<IOCRProvider, AWSTextractOCRService>();
        services.AddSingleton<IOCRService, EnhancedHybridOCRService>();

        return services;
    }

    /// <summary>
    /// Adds Google Cloud Vision OCR service
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddGoogleVisionOCR(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GoogleVisionConfiguration>(configuration.GetSection("OCR:GCPVision"));
        services.AddHttpClient<GoogleCloudVisionOCRService>();

        services.AddSingleton<ImageAnnotatorClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<GoogleVisionConfiguration>>().Value;

            // Use default credentials for now
            return ImageAnnotatorClient.Create();
        });

        services.AddSingleton<IOCRProvider, GoogleCloudVisionOCRService>();
        services.AddSingleton<IOCRProviderSelector, SmartOCRProviderSelector>();
        services.AddSingleton<IOCRService, EnhancedHybridOCRService>();

        return services;
    }

    /// <summary>
    /// Adds a custom OCR provider
    /// </summary>
    /// <typeparam name="TProvider">The provider type</typeparam>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddCustomOCRProvider<TProvider>(this IServiceCollection services)
        where TProvider : class, IOCRProvider
    {
        services.AddSingleton<IOCRProvider, TProvider>();
        return services;
    }

    /// <summary>
    /// Adds OCR services with custom configuration
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configureOptions">Action to configure OCR options</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddOCR(this IServiceCollection services, Action<OCRConfiguration> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddSingleton<IOCRProviderSelector, SmartOCRProviderSelector>();
        services.AddSingleton<IOCRService, EnhancedHybridOCRService>();
        return services;
    }

    /// <summary>
    /// Adds OCR services with custom provider selection strategy
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="strategy">The provider selection strategy</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddOCRWithStrategy(this IServiceCollection services, OCRProviderSelectionStrategy strategy)
    {
        services.AddSingleton<IOCRProviderSelector>(provider =>
        {
            var providers = provider.GetServices<IOCRProvider>();
            var logger = provider.GetRequiredService<ILogger<SmartOCRProviderSelector>>();
            return new SmartOCRProviderSelector(providers, logger, strategy);
        });

        services.AddSingleton<IOCRService, EnhancedHybridOCRService>();
        return services;
    }

    /// <summary>
    /// Adds PaddleOCR service
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddPaddleOCR(this IServiceCollection services, IConfiguration configuration)
    {
        // Register configuration
        services.Configure<PaddleOCRConfiguration>(configuration.GetSection("OCR:PaddleOCR"));

        // Register HTTP client with timeout and retry policy
        // Use named client "PaddleOCR" to match ProviderName used in GetHttpClientAsync
        services.AddHttpClient("PaddleOCR", client =>
        {
            var config = configuration.GetSection("OCR:PaddleOCR").Get<PaddleOCRConfiguration>();
            if (config != null)
            {
                client.BaseAddress = new Uri(config.ServiceUrl);
                client.Timeout = config.Timeout;
            }
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            MaxConnectionsPerServer = 10
        });

        // Register provider
        services.AddSingleton<IOCRProvider, PaddleOCRProvider>();

        return services;
    }

    /// <summary>
    /// Discovers and registers dynamic OCR providers from configuration sections
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    private static void DiscoverAndRegisterDynamicOCRProviders(IServiceCollection services, IConfiguration configuration)
    {
        var ocrSection = configuration.GetSection("OCR");
        if (!ocrSection.Exists())
        {
            return;
        }

        // Known top-level properties to exclude from dynamic discovery
        var excludedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Provider",
            "FallbackProvider",
            "ProviderSelectionStrategy",
            "DefaultConfidenceThreshold",
            "MaxRetries",
            "DefaultTimeout",
            "EnableFallbackProviders",
            "MaxFallbackProviders",
            "EnableCostTracking",
            "EnablePerformanceMonitoring",
            "EnableHealthChecks"
        };

        var discoveredCount = 0;

        // Get all child sections
        foreach (var section in ocrSection.GetChildren())
        {
            var sectionName = section.Key;

            // Skip excluded properties and already statically registered providers
            if (excludedProperties.Contains(sectionName) || IsStaticallyRegisteredOCR(sectionName))
            {
                continue;
            }

            try
            {
                // Check if this section has an AIProvider property (or equivalent)
                // For OCR, we'll use AIProvider for consistency, but could also check section name patterns
                var aiProviderKey = section["AIProvider"];
                if (string.IsNullOrWhiteSpace(aiProviderKey))
                {
                    // Try to infer from section name (e.g., "MyPaddleOCR" -> "paddle-ocr")
                    // This is a fallback - prefer explicit AIProvider property
                    continue;
                }

                // Look up provider type from registry
                if (!OCRProviderRegistry.ProviderTypeMap.TryGetValue(aiProviderKey, out var providerType))
                {
                    continue;
                }

                // Get configuration type for this provider
                var configType = GetConfigurationTypeForOCRProvider(providerType);
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

                // Register HTTP client if needed (for providers that use HTTP)
                var httpClientMethod = typeof(HttpClientFactoryServiceCollectionExtensions)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == "AddHttpClient" && 
                                        m.GetParameters().Length == 1 &&
                                        m.IsGenericMethod);

                if (httpClientMethod != null && 
                    (providerType == typeof(PaddleOCRProvider) ||
                     providerType == typeof(AzureComputerVisionOCRService) ||
                     providerType == typeof(AWSTextractOCRService) ||
                     providerType == typeof(GoogleCloudVisionOCRService)))
                {
                    var genericHttpClientMethod = httpClientMethod.MakeGenericMethod(providerType);
                    genericHttpClientMethod.Invoke(null, new object[] { services });
                }

                // Register external clients for providers that need them
                RegisterExternalClientsForOCRProvider(services, providerType, configType, sectionName);

                // Register provider instance with section name as provider name
                services.AddSingleton<IOCRProvider>(serviceProvider =>
                {
                    return CreateDynamicOCRProviderInstance(serviceProvider, providerType, configType, sectionName);
                });

                discoveredCount++;
            }
            catch (Exception)
            {
                // Continue with other providers - don't fail startup
            }
        }
    }

    /// <summary>
    /// Checks if a provider section name is already statically registered
    /// </summary>
    private static bool IsStaticallyRegisteredOCR(string sectionName)
    {
        var staticSections = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Azure",
            "AWS",
            "GCPVision",
            "AWSBedrock",
            "GCPDocumentAI",
            "PaddleOCR"
        };

        return staticSections.Contains(sectionName);
    }

    /// <summary>
    /// Gets the configuration type for an OCR provider type
    /// </summary>
    private static Type? GetConfigurationTypeForOCRProvider(Type providerType)
    {
        // Map provider types to their configuration types
        var providerConfigMap = new Dictionary<Type, Type>
        {
            { typeof(PaddleOCRProvider), typeof(PaddleOCRConfiguration) },
            { typeof(AzureComputerVisionOCRService), typeof(AzureOCRConfiguration) },
            { typeof(AWSTextractOCRService), typeof(AWSTextractConfiguration) },
            { typeof(GoogleCloudVisionOCRService), typeof(GoogleVisionConfiguration) }
        };

        if (providerConfigMap.TryGetValue(providerType, out var configType))
        {
            return configType;
        }

        // Fallback: try to find configuration type by convention
        var namespaceName = providerType.Namespace?.Replace(".Providers", ".Configurations");
        if (namespaceName != null)
        {
            var providerName = providerType.Name;
            var patterns = new[]
            {
                providerName.Replace("Provider", "Configuration"),
                providerName.Replace("OCRService", "Configuration"),
                providerName.Replace("Service", "Configuration")
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
    /// Registers external clients for OCR providers that need them (Azure, AWS, GCP)
    /// </summary>
    private static void RegisterExternalClientsForOCRProvider(IServiceCollection services, Type providerType, Type configType, string sectionName)
    {
        // Register Azure Computer Vision client if needed
        if (providerType == typeof(AzureComputerVisionOCRService))
        {
            services.AddSingleton<ComputerVisionClient>(serviceProvider =>
            {
                var optionsMonitorType = typeof(IOptionsMonitor<>).MakeGenericType(configType);
                var optionsMonitor = serviceProvider.GetRequiredService(optionsMonitorType);
                var getMethod = optionsMonitorType.GetMethod("Get", new[] { typeof(string) });
                var configInstance = getMethod?.Invoke(optionsMonitor, new object[] { sectionName });

                // Use reflection to get ApiKey and Endpoint
                var apiKeyProp = configType.GetProperty("ApiKey");
                var endpointProp = configType.GetProperty("Endpoint");
                
                if (apiKeyProp != null && endpointProp != null)
                {
                    var apiKey = apiKeyProp.GetValue(configInstance) as string;
                    var endpoint = endpointProp.GetValue(configInstance) as string;

                    if (!string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(endpoint))
                    {
                        try
                        {
                            var credentials = new ApiKeyServiceClientCredentials(apiKey);
                            return new ComputerVisionClient(credentials) { Endpoint = endpoint };
                        }
                        catch
                        {
                            return null!;
                        }
                    }
                }

                return null!;
            });
        }

        // Register AWS Textract client if needed
        if (providerType == typeof(AWSTextractOCRService))
        {
            services.AddSingleton<IAmazonTextract>(serviceProvider =>
            {
                var optionsMonitorType = typeof(IOptionsMonitor<>).MakeGenericType(configType);
                var optionsMonitor = serviceProvider.GetRequiredService(optionsMonitorType);
                var getMethod = optionsMonitorType.GetMethod("Get", new[] { typeof(string) });
                var configInstance = getMethod?.Invoke(optionsMonitor, new object[] { sectionName });

                // Use reflection to get AccessKeyId, SecretAccessKey, and Region
                var accessKeyProp = configType.GetProperty("AccessKeyId");
                var secretKeyProp = configType.GetProperty("SecretAccessKey");
                var regionProp = configType.GetProperty("Region");

                if (accessKeyProp != null && secretKeyProp != null && regionProp != null)
                {
                    var accessKey = accessKeyProp.GetValue(configInstance) as string;
                    var secretKey = secretKeyProp.GetValue(configInstance) as string;
                    var region = regionProp.GetValue(configInstance) as string;

                    if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey) && !string.IsNullOrEmpty(region))
                    {
                        try
                        {
                            var clientConfig = new AmazonTextractConfig
                            {
                                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
                            };
                            return new AmazonTextractClient(accessKey, secretKey, clientConfig);
                        }
                        catch
                        {
                            return null!;
                        }
                    }
                }

                return null!;
            });
        }

        // Register Google Cloud Vision client if needed
        if (providerType == typeof(GoogleCloudVisionOCRService))
        {
            services.AddSingleton<ImageAnnotatorClient>(serviceProvider =>
            {
                try
                {
                    return ImageAnnotatorClient.Create();
                }
                catch
                {
                    return null!;
                }
            });
        }
    }

    /// <summary>
    /// Creates a dynamic OCR provider instance using reflection
    /// </summary>
    private static IOCRProvider CreateDynamicOCRProviderInstance(
        IServiceProvider serviceProvider,
        Type providerType,
        Type configType,
        string sectionName)
    {
        try
        {
            // Get required services
            var loggerType = typeof(ILogger<>).MakeGenericType(providerType);
            var loggerInstance = serviceProvider.GetRequiredService(loggerType);

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

            // Find constructor based on provider type
            var constructors = providerType.GetConstructors();
            ConstructorInfo? selectedConstructor = null;
            object[]? constructorParams = null;

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                // AzureComputerVisionOCRService: ComputerVisionClient, ILogger<T>, IOptions<TConfig>
                if (providerType == typeof(AzureComputerVisionOCRService) &&
                    parameters.Length == 3 &&
                    parameters[0].ParameterType == typeof(ComputerVisionClient) &&
                    IsLoggerType(parameters[1].ParameterType) &&
                    parameters[2].ParameterType.IsGenericType &&
                    parameters[2].ParameterType.GetGenericTypeDefinition() == typeof(IOptions<>))
                {
                    var client = serviceProvider.GetService<ComputerVisionClient>();
                    selectedConstructor = constructor;
                    constructorParams = new object[] { client!, loggerInstance, optionsWrapper };
                    break;
                }
                // AWSTextractOCRService: IAmazonTextract, ILogger<T>, IOptions<TConfig>
                else if (providerType == typeof(AWSTextractOCRService) &&
                         parameters.Length == 3 &&
                         parameters[0].ParameterType == typeof(IAmazonTextract) &&
                         IsLoggerType(parameters[1].ParameterType) &&
                         parameters[2].ParameterType.IsGenericType &&
                         parameters[2].ParameterType.GetGenericTypeDefinition() == typeof(IOptions<>))
                {
                    var client = serviceProvider.GetService<IAmazonTextract>();
                    selectedConstructor = constructor;
                    constructorParams = new object[] { client!, loggerInstance, optionsWrapper };
                    break;
                }
                // GoogleCloudVisionOCRService: ImageAnnotatorClient, ILogger<T>, IOptions<TConfig>
                else if (providerType == typeof(GoogleCloudVisionOCRService) &&
                         parameters.Length == 3 &&
                         parameters[0].ParameterType == typeof(ImageAnnotatorClient) &&
                         IsLoggerType(parameters[1].ParameterType) &&
                         parameters[2].ParameterType.IsGenericType &&
                         parameters[2].ParameterType.GetGenericTypeDefinition() == typeof(IOptions<>))
                {
                    var client = serviceProvider.GetService<ImageAnnotatorClient>();
                    selectedConstructor = constructor;
                    constructorParams = new object[] { client!, loggerInstance, optionsWrapper };
                    break;
                }
                // PaddleOCRProvider: ILogger<T>, IHttpClientFactory, IOptions<TConfig>
                else if (providerType == typeof(PaddleOCRProvider) &&
                         parameters.Length == 3 &&
                         IsLoggerType(parameters[0].ParameterType) &&
                         parameters[1].ParameterType == typeof(IHttpClientFactory) &&
                         parameters[2].ParameterType.IsGenericType &&
                         parameters[2].ParameterType.GetGenericTypeDefinition() == typeof(IOptions<>))
                {
                    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                    selectedConstructor = constructor;
                    constructorParams = new object[] { loggerInstance, httpClientFactory, optionsWrapper };
                    break;
                }
            }

            if (selectedConstructor == null || constructorParams == null)
            {
                throw new InvalidOperationException($"No suitable constructor found for {providerType.Name}");
            }

            var providerInstance = selectedConstructor.Invoke(constructorParams) as IOCRProvider;
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
                var loggerType = typeof(ILogger<>).MakeGenericType(typeof(OCRServiceCollectionExtensions));
                var loggerInstance = serviceProvider.GetService(loggerType) as ILogger;
                loggerInstance?.LogError(ex, "Failed to create dynamic OCR provider instance for {ProviderType} in section {SectionName}", providerType.Name, sectionName);
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
