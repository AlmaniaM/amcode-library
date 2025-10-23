using Microsoft.Extensions.Logging;
using AMCode.OCR.Models;

namespace AMCode.OCR.Services;

/// <summary>
/// Interface for analyzing and tracking OCR costs
/// </summary>
public interface ICostAnalyzer
{
    /// <summary>
    /// Calculate the cost for an OCR operation
    /// </summary>
    /// <param name="provider">OCR provider</param>
    /// <param name="request">OCR request</param>
    /// <returns>Estimated cost in USD</returns>
    Task<decimal> CalculateCostAsync(IOCRProvider provider, OCRRequest request);
    
    /// <summary>
    /// Generate a cost report for a given period
    /// </summary>
    /// <param name="period">Time period to analyze</param>
    /// <returns>Cost report with breakdown</returns>
    Task<CostReport> GenerateCostReportAsync(TimeSpan period);
    
    /// <summary>
    /// Get total cost for a given period
    /// </summary>
    /// <param name="period">Time period to analyze</param>
    /// <returns>Total cost in USD</returns>
    Task<decimal> GetTotalCostAsync(TimeSpan period);
    
    /// <summary>
    /// Get cost breakdown by provider
    /// </summary>
    /// <param name="period">Time period to analyze</param>
    /// <returns>Cost breakdown by provider</returns>
    Task<IEnumerable<CostBreakdown>> GetCostBreakdownAsync(TimeSpan period);
    
    /// <summary>
    /// Record a cost for tracking
    /// </summary>
    /// <param name="providerName">Name of the provider</param>
    /// <param name="cost">Cost amount</param>
    /// <param name="timestamp">When the cost was incurred</param>
    Task RecordCostAsync(string providerName, decimal cost, DateTime timestamp);
}

/// <summary>
/// Cost analysis and tracking service
/// </summary>
public class CostAnalyzer : ICostAnalyzer
{
    private readonly ILogger<CostAnalyzer> _logger;
    private readonly List<CostRecord> _costRecords;
    private readonly object _lock = new object();
    
    public CostAnalyzer(ILogger<CostAnalyzer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _costRecords = new List<CostRecord>();
    }
    
    public async Task<decimal> CalculateCostAsync(IOCRProvider provider, OCRRequest request)
    {
        // Base cost per request
        var cost = provider.Capabilities.CostPerRequest;
        
        // Additional cost based on image size (if applicable)
        if (request.ImageStream.CanSeek)
        {
            var imageSize = request.ImageStream.Length;
            var imageSizeMB = imageSize / (1024.0 * 1024.0);
            
            // Some providers charge based on image size
            if (imageSizeMB > 1.0)
            {
                cost += (decimal)(imageSizeMB - 1.0) * 0.0001m; // $0.0001 per MB over 1MB
            }
        }
        
        return cost;
    }
    
    public async Task<CostReport> GenerateCostReportAsync(TimeSpan period)
    {
        var cutoffTime = DateTime.UtcNow - period;
        
        lock (_lock)
        {
            var costs = _costRecords.Where(c => c.Timestamp >= cutoffTime).ToList();
            
            return new CostReport
            {
                Period = period,
                TotalCost = costs.Sum(c => c.Cost),
                ProviderBreakdown = costs.GroupBy(c => c.ProviderName)
                    .ToDictionary(g => g.Key, g => g.Sum(c => c.Cost)),
                DailyBreakdown = costs.GroupBy(c => c.Timestamp.Date)
                    .ToDictionary(g => g.Key, g => g.Sum(c => c.Cost)),
                TotalRequests = costs.Count,
                AverageCostPerRequest = costs.Any() ? costs.Average(c => c.Cost) : 0
            };
        }
    }
    
    public async Task<decimal> GetTotalCostAsync(TimeSpan period)
    {
        var report = await GenerateCostReportAsync(period);
        return report.TotalCost;
    }
    
    public async Task<IEnumerable<CostBreakdown>> GetCostBreakdownAsync(TimeSpan period)
    {
        var cutoffTime = DateTime.UtcNow - period;
        
        lock (_lock)
        {
            var costs = _costRecords.Where(c => c.Timestamp >= cutoffTime).ToList();
            
            return costs.GroupBy(c => c.ProviderName)
                .Select(g => new CostBreakdown
                {
                    ProviderName = g.Key,
                    TotalCost = g.Sum(c => c.Cost),
                    RequestCount = g.Count(),
                    AverageCostPerRequest = g.Average(c => c.Cost),
                    Percentage = costs.Any() ? (g.Sum(c => c.Cost) / costs.Sum(c => c.Cost)) * 100 : 0
                })
                .OrderByDescending(b => b.TotalCost);
        }
    }
    
    public async Task RecordCostAsync(string providerName, decimal cost, DateTime timestamp)
    {
        lock (_lock)
        {
            _costRecords.Add(new CostRecord
            {
                ProviderName = providerName,
                Cost = cost,
                Timestamp = timestamp
            });
        }
        
        _logger.LogInformation("Recorded cost: {Provider} - ${Cost:F4} at {Timestamp}", 
            providerName, cost, timestamp);
    }
}

/// <summary>
/// Cost report data
/// </summary>
public class CostReport
{
    public TimeSpan Period { get; set; }
    public decimal TotalCost { get; set; }
    public Dictionary<string, decimal> ProviderBreakdown { get; set; } = new();
    public Dictionary<DateTime, decimal> DailyBreakdown { get; set; } = new();
    public int TotalRequests { get; set; }
    public decimal AverageCostPerRequest { get; set; }
}

/// <summary>
/// Cost breakdown by provider
/// </summary>
public class CostBreakdown
{
    public string ProviderName { get; set; } = string.Empty;
    public decimal TotalCost { get; set; }
    public int RequestCount { get; set; }
    public decimal AverageCostPerRequest { get; set; }
    public decimal Percentage { get; set; }
}

/// <summary>
/// Individual cost record
/// </summary>
internal class CostRecord
{
    public string ProviderName { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public DateTime Timestamp { get; set; }
}
