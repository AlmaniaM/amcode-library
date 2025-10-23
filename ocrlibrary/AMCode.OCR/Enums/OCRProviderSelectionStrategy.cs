namespace AMCode.OCR.Enums;

/// <summary>
/// Strategy for selecting OCR providers
/// </summary>
public enum OCRProviderSelectionStrategy
{
    /// <summary>
    /// Select the provider with the lowest cost
    /// </summary>
    CostOptimized,

    /// <summary>
    /// Select the provider with the best performance (fastest response time)
    /// </summary>
    PerformanceOptimized,

    /// <summary>
    /// Select the provider with the highest reliability
    /// </summary>
    ReliabilityOptimized,

    /// <summary>
    /// Select the provider with the best capabilities for the request
    /// </summary>
    CapabilityOptimized,

    /// <summary>
    /// Select the provider with the best quality output
    /// </summary>
    QualityOptimized,

    /// <summary>
    /// Select the provider with the best balance of all factors
    /// </summary>
    Balanced,

    /// <summary>
    /// Select the provider based on current load and availability
    /// </summary>
    LoadBalanced,

    /// <summary>
    /// Select the provider based on geographic proximity
    /// </summary>
    GeographicOptimized,

    /// <summary>
    /// Select the provider based on custom criteria
    /// </summary>
    Custom
}