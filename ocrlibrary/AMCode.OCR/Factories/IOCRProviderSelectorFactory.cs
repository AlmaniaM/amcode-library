using AMCode.OCR.Services;

namespace AMCode.OCR.Factories;

/// <summary>
/// Factory interface for creating OCR provider selectors
/// </summary>
public interface IOCRProviderSelectorFactory
{
    /// <summary>
    /// Creates the appropriate OCR provider selector based on configuration
    /// </summary>
    /// <returns>OCR provider selector instance</returns>
    IOCRProviderSelector CreateSelector();
}

