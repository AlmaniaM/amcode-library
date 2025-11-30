using AMCode.OCR;

namespace AMCode.OCR.Factories;

/// <summary>
/// Factory interface for creating OCR providers dynamically based on configuration
/// </summary>
public interface IOCRProviderFactory
{
    /// <summary>
    /// Creates the primary OCR provider based on OCR:Provider configuration
    /// </summary>
    /// <returns>Configured OCR provider, or null if not found</returns>
    IOCRProvider? CreateProvider();

    /// <summary>
    /// Creates the fallback OCR provider based on OCR:FallbackProvider configuration
    /// </summary>
    /// <returns>Configured fallback OCR provider, or null if not found</returns>
    IOCRProvider? CreateFallbackProvider();
}

