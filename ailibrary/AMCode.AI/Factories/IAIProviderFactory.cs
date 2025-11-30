using AMCode.AI.Providers;
using Microsoft.Extensions.Configuration;

namespace AMCode.AI.Factories;

/// <summary>
/// Factory interface for creating AI providers dynamically
/// </summary>
public interface IAIProviderFactory
{
    /// <summary>
    /// Create a provider by type and name
    /// </summary>
    /// <returns>Configured AI provider</returns>
    IAIProvider CreateProvider();

    /// <summary>
    /// Create the primary AI provider based on AI:Provider configuration
    /// </summary>
    /// <returns>Configured AI provider, or null if not found</returns>
    IAIProvider? CreateAIProvider();

    /// <summary>
    /// Create the fallback AI provider based on AI:FallbackProvider configuration
    /// </summary>
    /// <returns>Configured fallback AI provider, or null if not found</returns>
    IAIProvider? CreateFallbackAIProvider();

    /// <summary>
    /// Create an AI provider specifically for parsing extracted text from recipes.
    /// </summary>
    /// <returns>Configured OCR parser provider</returns>
    IAIProvider CreateRecipeTextParserProvider();

    /// <summary>
    /// Create the primary OCR text parser provider based on AI:OCRTextParserProvider configuration
    /// </summary>
    /// <returns>Configured OCR text parser provider, or null if not found</returns>
    IAIProvider? CreateOCRTextParserProvider();

    /// <summary>
    /// Create the fallback OCR text parser provider based on AI:FallbackOCRTextParserProvider configuration
    /// </summary>
    /// <returns>Configured fallback OCR text parser provider, or null if not found</returns>
    IAIProvider? CreateFallbackOCRTextParserProvider();

    /// <summary>
    /// Create a custom provider by type
    /// </summary>
    /// <param name="name">Provider name</param>
    /// <param name="providerType">Provider type</param>
    /// <param name="configuration">Configuration section</param>
    /// <returns>Configured AI provider</returns>
    IAIProvider CreateCustomProvider(string name, Type providerType, IConfiguration configuration);

    /// <summary>
    /// Register a provider factory
    /// </summary>
    /// <typeparam name="T">Provider type</typeparam>
    /// <param name="name">Provider name</param>
    /// <param name="factory">Factory function</param>
    void RegisterProvider<T>(string name, Func<IServiceProvider, IAIProvider> factory) where T : GenericAIProvider;

    /// <summary>
    /// Get all registered provider names
    /// </summary>
    /// <returns>List of registered provider names</returns>
    IEnumerable<string> GetRegisteredProviders();

    /// <summary>
    /// Check if a provider is registered
    /// </summary>
    /// <param name="name">Provider name</param>
    /// <returns>True if registered</returns>
    bool IsProviderRegistered(string name);
}
