using AMCode.AI.Models;
using Microsoft.Extensions.Logging;

namespace AMCode.AI.Services;

/// <summary>
/// Service for analyzing and tracking AI operation costs
/// </summary>
public class CostAnalyzer : ICostAnalyzer
{
    private readonly ILogger<CostAnalyzer> _logger;
    private readonly Dictionary<string, decimal> _costsByProvider = new();
    private readonly Dictionary<string, int> _requestsByProvider = new();
    private readonly object _lock = new();
    
    public CostAnalyzer(ILogger<CostAnalyzer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    /// <summary>
    /// Calculate cost for a request
    /// </summary>
    /// <param name="providerName">Name of the provider</param>
    /// <param name="inputTokens">Number of input tokens</param>
    /// <param name="outputTokens">Number of output tokens</param>
    /// <param name="capabilities">Provider capabilities</param>
    /// <returns>Calculated cost</returns>
    public decimal CalculateCost(string providerName, int inputTokens, int outputTokens, AIProviderCapabilities capabilities)
    {
        try
        {
            _logger.LogDebug("Calculating cost for {Provider}: {InputTokens} input, {OutputTokens} output", 
                providerName, inputTokens, outputTokens);
            
            var inputCost = inputTokens * capabilities.CostPerToken;
            var outputCost = outputTokens * capabilities.CostPerToken;
            var baseCost = capabilities.CostPerRequest;
            
            var totalCost = inputCost + outputCost + baseCost;
            
            _logger.LogDebug("Cost calculation for {Provider}: Input=${InputCost:F6}, Output=${OutputCost:F6}, Base=${BaseCost:F6}, Total=${TotalCost:F6}", 
                providerName, inputCost, outputCost, baseCost, totalCost);
            
            return totalCost;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to calculate cost for provider {Provider}", providerName);
            return 0m;
        }
    }
    
    /// <summary>
    /// Record cost for a completed request
    /// </summary>
    /// <param name="providerName">Name of the provider</param>
    /// <param name="cost">Cost of the request</param>
    public void RecordCost(string providerName, decimal cost)
    {
        try
        {
            lock (_lock)
            {
                if (_costsByProvider.ContainsKey(providerName))
                {
                    _costsByProvider[providerName] += cost;
                    _requestsByProvider[providerName]++;
                }
                else
                {
                    _costsByProvider[providerName] = cost;
                    _requestsByProvider[providerName] = 1;
                }
            }
            
            _logger.LogDebug("Recorded cost for {Provider}: ${Cost:F6}", providerName, cost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to record cost for provider {Provider}", providerName);
        }
    }
    
    /// <summary>
    /// Get total cost for a provider
    /// </summary>
    /// <param name="providerName">Name of the provider</param>
    /// <returns>Total cost</returns>
    public decimal GetTotalCost(string providerName)
    {
        lock (_lock)
        {
            return _costsByProvider.TryGetValue(providerName, out var cost) ? cost : 0m;
        }
    }
    
    /// <summary>
    /// Get total cost across all providers
    /// </summary>
    /// <returns>Total cost</returns>
    public decimal GetTotalCost()
    {
        lock (_lock)
        {
            return _costsByProvider.Values.Sum();
        }
    }
    
    /// <summary>
    /// Get cost breakdown by provider
    /// </summary>
    /// <returns>Cost breakdown</returns>
    public Dictionary<string, decimal> GetCostBreakdown()
    {
        lock (_lock)
        {
            return new Dictionary<string, decimal>(_costsByProvider);
        }
    }
    
    /// <summary>
    /// Get request count by provider
    /// </summary>
    /// <returns>Request count breakdown</returns>
    public Dictionary<string, int> GetRequestCounts()
    {
        lock (_lock)
        {
            return new Dictionary<string, int>(_requestsByProvider);
        }
    }
    
    /// <summary>
    /// Generate cost report
    /// </summary>
    /// <returns>Cost report</returns>
    public CostReport GenerateCostReport()
    {
        try
        {
            lock (_lock)
            {
                var report = new CostReport
                {
                    GeneratedAt = DateTime.UtcNow,
                    TotalCost = _costsByProvider.Values.Sum(),
                    TotalRequests = _requestsByProvider.Values.Sum(),
                    ProviderBreakdown = _costsByProvider.ToDictionary(
                        kvp => kvp.Key,
                        kvp => new ProviderCostInfo
                        {
                            ProviderName = kvp.Key,
                            TotalCost = kvp.Value,
                            RequestCount = _requestsByProvider.TryGetValue(kvp.Key, out var count) ? count : 0,
                            AverageCostPerRequest = count > 0 ? kvp.Value / count : 0m
                        }
                    )
                };
                
                _logger.LogInformation("Generated cost report: Total=${TotalCost:F6}, Requests={TotalRequests}", 
                    report.TotalCost, report.TotalRequests);
                
                return report;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate cost report");
            return new CostReport
            {
                GeneratedAt = DateTime.UtcNow,
                TotalCost = 0m,
                TotalRequests = 0,
                ProviderBreakdown = new Dictionary<string, ProviderCostInfo>()
            };
        }
    }
    
    /// <summary>
    /// Reset all cost tracking
    /// </summary>
    public void Reset()
    {
        lock (_lock)
        {
            _costsByProvider.Clear();
            _requestsByProvider.Clear();
        }
        
        _logger.LogInformation("Cost tracking reset");
    }
}

/// <summary>
/// Cost report information
/// </summary>
public class CostReport
{
    /// <summary>
    /// When the report was generated
    /// </summary>
    public DateTime GeneratedAt { get; set; }
    
    /// <summary>
    /// Total cost across all providers
    /// </summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>
    /// Total number of requests
    /// </summary>
    public int TotalRequests { get; set; }
    
    /// <summary>
    /// Cost breakdown by provider
    /// </summary>
    public Dictionary<string, ProviderCostInfo> ProviderBreakdown { get; set; } = new();
}

/// <summary>
/// Provider cost information
/// </summary>
public class ProviderCostInfo
{
    /// <summary>
    /// Provider name
    /// </summary>
    public string ProviderName { get; set; } = string.Empty;
    
    /// <summary>
    /// Total cost for this provider
    /// </summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>
    /// Number of requests for this provider
    /// </summary>
    public int RequestCount { get; set; }
    
    /// <summary>
    /// Average cost per request
    /// </summary>
    public decimal AverageCostPerRequest { get; set; }
}
