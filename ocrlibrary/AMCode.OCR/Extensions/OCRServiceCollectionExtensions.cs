using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using AMCode.OCR.Providers;
using AMCode.OCR.Services;
using AMCode.OCR.Configurations;
using AMCode.OCR.Enums;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Amazon.Textract;
using Google.Cloud.Vision.V1;

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
        services.Configure<GoogleVisionConfiguration>(configuration.GetSection("OCR:Google"));

        // Register HTTP clients
        services.AddHttpClient<AzureComputerVisionOCRService>();
        services.AddHttpClient<AWSTextractOCRService>();
        services.AddHttpClient<GoogleCloudVisionOCRService>();

        // Register Azure Computer Vision client
        services.AddSingleton<ComputerVisionClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<AzureOCRConfiguration>>().Value;
            var credentials = new ApiKeyServiceClientCredentials(config.SubscriptionKey);
            return new ComputerVisionClient(credentials)
            {
                Endpoint = config.Endpoint
            };
        });

        // Register AWS Textract client
        services.AddSingleton<IAmazonTextract>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<AWSTextractConfiguration>>().Value;
            var clientConfig = new AmazonTextractConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(config.Region)
            };
            return new AmazonTextractClient(config.AccessKeyId, config.SecretAccessKey, clientConfig);
        });

        // Register Google Cloud Vision client
        services.AddSingleton<ImageAnnotatorClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<GoogleVisionConfiguration>>().Value;
            
            // Use default credentials for now
            return ImageAnnotatorClient.Create();
        });

        // Register OCR providers
        services.AddSingleton<IOCRProvider, AzureComputerVisionOCRService>();
        services.AddSingleton<IOCRProvider, AWSTextractOCRService>();
        services.AddSingleton<IOCRProvider, GoogleCloudVisionOCRService>();

        // Register services
        services.AddSingleton<IOCRProviderSelector, SmartOCRProviderSelector>();
        services.AddSingleton<IOCRService, EnhancedHybridOCRService>();

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
            var credentials = new ApiKeyServiceClientCredentials(config.SubscriptionKey);
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
        services.Configure<GoogleVisionConfiguration>(configuration.GetSection("OCR:Google"));
        services.AddHttpClient<GoogleCloudVisionOCRService>();
        
        services.AddSingleton<ImageAnnotatorClient>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<GoogleVisionConfiguration>>().Value;
            
            // Use default credentials for now
            return ImageAnnotatorClient.Create();
        });

        services.AddSingleton<IOCRProvider, GoogleCloudVisionOCRService>();
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
}