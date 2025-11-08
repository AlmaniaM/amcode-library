using AMCode.AI.Models;
using AMCode.AI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace AMCode.AI.Tests.Services;

public class CostAnalyzerTests
{
    private Mock<ILogger<CostAnalyzer>> _mockLogger;
    private CostAnalyzer _costAnalyzer;
    
    public CostAnalyzerTests()
    {
        _mockLogger = new Mock<ILogger<CostAnalyzer>>();
        _costAnalyzer = new CostAnalyzer(_mockLogger.Object);
    }
    
    [Fact]
    public void CalculateCost_ShouldReturnCorrectCost()
    {
        // Arrange
        var providerName = "TestProvider";
        var inputTokens = 100;
        var outputTokens = 50;
        var capabilities = new AIProviderCapabilities
        {
            CostPerToken = 0.0001m,
            CostPerRequest = 0.01m
        };
        
        // Act
        var cost = _costAnalyzer.CalculateCost(providerName, inputTokens, outputTokens, capabilities);
        
        // Assert
        var expectedCost = (inputTokens * capabilities.CostPerToken) + (outputTokens * capabilities.CostPerToken) + capabilities.CostPerRequest;
        cost.Should().Be(expectedCost);
    }
    
    [Fact]
    public void RecordCost_ShouldRecordCostCorrectly()
    {
        // Arrange
        var providerName = "TestProvider";
        var cost = 0.05m;
        
        // Act
        _costAnalyzer.RecordCost(providerName, cost);
        
        // Assert
        var totalCost = _costAnalyzer.GetTotalCost(providerName);
        totalCost.Should().Be(cost);
    }
    
    [Fact]
    public void RecordCost_MultipleTimes_ShouldAccumulateCost()
    {
        // Arrange
        var providerName = "TestProvider";
        var cost1 = 0.05m;
        var cost2 = 0.03m;
        
        // Act
        _costAnalyzer.RecordCost(providerName, cost1);
        _costAnalyzer.RecordCost(providerName, cost2);
        
        // Assert
        var totalCost = _costAnalyzer.GetTotalCost(providerName);
        totalCost.Should().Be(cost1 + cost2);
    }
    
    [Fact]
    public void GetTotalCost_WithNoRecordedCosts_ShouldReturnZero()
    {
        // Act
        var totalCost = _costAnalyzer.GetTotalCost();
        
        // Assert
        totalCost.Should().Be(0m);
    }
    
    [Fact]
    public void GetCostBreakdown_ShouldReturnCorrectBreakdown()
    {
        // Arrange
        var provider1 = "Provider1";
        var provider2 = "Provider2";
        var cost1 = 0.05m;
        var cost2 = 0.03m;
        
        // Act
        _costAnalyzer.RecordCost(provider1, cost1);
        _costAnalyzer.RecordCost(provider2, cost2);
        var breakdown = _costAnalyzer.GetCostBreakdown();
        
        // Assert
        breakdown.Should().ContainKey(provider1).WhoseValue.Should().Be(cost1);
        breakdown.Should().ContainKey(provider2).WhoseValue.Should().Be(cost2);
    }
    
    [Fact]
    public void GetRequestCounts_ShouldReturnCorrectCounts()
    {
        // Arrange
        var provider1 = "Provider1";
        var provider2 = "Provider2";
        
        // Act
        _costAnalyzer.RecordCost(provider1, 0.05m);
        _costAnalyzer.RecordCost(provider1, 0.03m);
        _costAnalyzer.RecordCost(provider2, 0.02m);
        var counts = _costAnalyzer.GetRequestCounts();
        
        // Assert
        counts.Should().ContainKey(provider1).WhoseValue.Should().Be(2);
        counts.Should().ContainKey(provider2).WhoseValue.Should().Be(1);
    }
    
    [Fact]
    public void GenerateCostReport_ShouldReturnCorrectReport()
    {
        // Arrange
        var provider1 = "Provider1";
        var provider2 = "Provider2";
        var cost1 = 0.05m;
        var cost2 = 0.03m;
        
        // Act
        _costAnalyzer.RecordCost(provider1, cost1);
        _costAnalyzer.RecordCost(provider2, cost2);
        var report = _costAnalyzer.GenerateCostReport();
        
        // Assert
        report.Should().NotBeNull();
        report.TotalCost.Should().Be(cost1 + cost2);
        report.TotalRequests.Should().Be(2);
        report.ProviderBreakdown.Should().HaveCount(2);
        report.GeneratedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void Reset_ShouldClearAllCosts()
    {
        // Arrange
        var provider1 = "Provider1";
        var provider2 = "Provider2";
        _costAnalyzer.RecordCost(provider1, 0.05m);
        _costAnalyzer.RecordCost(provider2, 0.03m);
        
        // Act
        _costAnalyzer.Reset();
        
        // Assert
        _costAnalyzer.GetTotalCost().Should().Be(0m);
        _costAnalyzer.GetCostBreakdown().Should().BeEmpty();
        _costAnalyzer.GetRequestCounts().Should().BeEmpty();
    }
}
