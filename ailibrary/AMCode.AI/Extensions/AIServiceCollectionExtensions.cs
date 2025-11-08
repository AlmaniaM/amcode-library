using AMCode.AI.Configurations;
using AMCode.AI.Enums;
using AMCode.AI.Factories;
using AMCode.AI.Providers;
using AMCode.AI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

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
        services.Configure<AnthropicConfiguration>(configuration.GetSection("AI:Anthropic"));
        services.Configure<GrokConfiguration>(configuration.GetSection("AI:Grok"));
        services.Configure<AWSBedrockConfiguration>(configuration.GetSection("AI:AWSBedrock"));
        services.Configure<OllamaConfiguration>(configuration.GetSection("AI:Ollama"));
        services.Configure<LMStudioConfiguration>(configuration.GetSection("AI:LMStudio"));
        services.Configure<HuggingFaceConfiguration>(configuration.GetSection("AI:HuggingFace"));
        
        // Register HTTP clients
        services.AddHttpClient<OpenAIGPTProvider>();
        services.AddHttpClient<AnthropicClaudeProvider>();
        services.AddHttpClient<GrokProvider>();
        services.AddHttpClient<AWSBedrockProvider>();
        services.AddHttpClient<OllamaAIProvider>();
        services.AddHttpClient<LMStudioAIProvider>();
        services.AddHttpClient<HuggingFaceAIProvider>();
        
        // Register providers
        services.AddSingleton<IAIProvider, OpenAIGPTProvider>();
        services.AddSingleton<IAIProvider, AnthropicClaudeProvider>();
        services.AddSingleton<IAIProvider, GrokProvider>();
        services.AddSingleton<IAIProvider, AWSBedrockProvider>();
        services.AddSingleton<IAIProvider, OllamaAIProvider>();
        services.AddSingleton<IAIProvider, LMStudioAIProvider>();
        services.AddSingleton<IAIProvider, HuggingFaceAIProvider>();
        
        // Register services
        services.AddSingleton<PromptBuilderService>();
        services.AddSingleton<ICostAnalyzer, CostAnalyzer>();
        services.AddSingleton<IAIProviderSelector, SmartAIProviderSelector>();
        services.AddSingleton<IRecipeParserService, EnhancedHybridAIService>();
        services.AddSingleton<IRecipeValidationService, RecipeValidationService>();
        services.AddSingleton<IAIProviderFactory, AIProviderFactory>();
        
        // Register provider selector with strategy
        services.AddSingleton<IAIProviderSelector>(provider =>
        {
            var providers = provider.GetServices<IAIProvider>();
            var logger = provider.GetRequiredService<ILogger<SmartAIProviderSelector>>();
            var costAnalyzer = provider.GetRequiredService<ICostAnalyzer>();
            var config = provider.GetRequiredService<IOptions<AIConfiguration>>().Value;
            
            var strategy = Enum.TryParse<AIProviderSelectionStrategy>(config.DefaultSelectionStrategy, out var parsedStrategy) 
                ? parsedStrategy 
                : AIProviderSelectionStrategy.Balanced;
            
            return new SmartAIProviderSelector(providers, logger, costAnalyzer, strategy);
        });
        
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
        services.Configure<AIConfiguration>(config => config.DefaultSelectionStrategy = strategy.ToString());
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
}
