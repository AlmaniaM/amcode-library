using AMCode.AI.Services;

namespace AMCode.AI.Factories;

/// <summary>
/// Factory interface for creating AI provider selectors
/// </summary>
public interface IAIProviderSelectorFactory
{
    /// <summary>
    /// Creates the appropriate AI provider selector based on configuration
    /// </summary>
    /// <returns>AI provider selector instance</returns>
    IAIProviderSelector CreateSelector();
}

