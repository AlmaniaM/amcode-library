namespace AMCode.AI.Enums;

/// <summary>
/// Strategy for selecting the best AI provider
/// </summary>
public enum AIProviderSelectionStrategy
{
    /// <summary>
    /// Select provider based on cost optimization
    /// </summary>
    CostOptimized,
    
    /// <summary>
    /// Select provider based on performance optimization
    /// </summary>
    PerformanceOptimized,
    
    /// <summary>
    /// Select provider based on reliability optimization
    /// </summary>
    ReliabilityOptimized,
    
    /// <summary>
    /// Select provider based on capability optimization
    /// </summary>
    CapabilityOptimized,
    
    /// <summary>
    /// Select provider based on quality optimization
    /// </summary>
    QualityOptimized,
    
    /// <summary>
    /// Select provider based on balanced criteria
    /// </summary>
    Balanced
}
