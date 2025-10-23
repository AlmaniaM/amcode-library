using AMCode.AI.Models;

namespace AMCode.AI.Services;

/// <summary>
/// Interface for cost analysis and tracking
/// </summary>
public interface ICostAnalyzer
{
    /// <summary>
    /// Calculate cost for a request
    /// </summary>
    /// <param name="providerName">Name of the provider</param>
    /// <param name="inputTokens">Number of input tokens</param>
    /// <param name="outputTokens">Number of output tokens</param>
    /// <param name="capabilities">Provider capabilities</param>
    /// <returns>Calculated cost</returns>
    decimal CalculateCost(string providerName, int inputTokens, int outputTokens, AIProviderCapabilities capabilities);
    
    /// <summary>
    /// Record cost for a completed request
    /// </summary>
    /// <param name="providerName">Name of the provider</param>
    /// <param name="cost">Cost of the request</param>
    void RecordCost(string providerName, decimal cost);
    
    /// <summary>
    /// Get total cost for a provider
    /// </summary>
    /// <param name="providerName">Name of the provider</param>
    /// <returns>Total cost</returns>
    decimal GetTotalCost(string providerName);
    
    /// <summary>
    /// Get total cost across all providers
    /// </summary>
    /// <returns>Total cost</returns>
    decimal GetTotalCost();
    
    /// <summary>
    /// Get cost breakdown by provider
    /// </summary>
    /// <returns>Cost breakdown</returns>
    Dictionary<string, decimal> GetCostBreakdown();
    
    /// <summary>
    /// Get request count by provider
    /// </summary>
    /// <returns>Request count breakdown</returns>
    Dictionary<string, int> GetRequestCounts();
    
    /// <summary>
    /// Generate cost report
    /// </summary>
    /// <returns>Cost report</returns>
    CostReport GenerateCostReport();
    
    /// <summary>
    /// Reset all cost tracking
    /// </summary>
    void Reset();
}
