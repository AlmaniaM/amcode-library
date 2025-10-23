using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using AMCode.OCR.Services;
using AMCode.OCR.Models;
using AMCode.OCR.Providers;

namespace AMCode.OCR.Tests.Services;

[TestClass]
public class CostAnalyzerTests
{
    private Mock<ILogger<CostAnalyzer>> _mockLogger = null!;
    private CostAnalyzer _costAnalyzer = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<CostAnalyzer>>();
        _costAnalyzer = new CostAnalyzer(_mockLogger.Object);
    }
    
    [TestMethod]
    public async Task CalculateCostAsync_ShouldReturnBaseCost()
    {
        // Arrange
        var mockProvider = new Mock<IOCRProvider>();
        mockProvider.Setup(p => p.Capabilities).Returns(new OCRProviderCapabilities
        {
            CostPerRequest = 0.001m
        });
        
        var request = new OCRRequest
        {
            ImageStream = new MemoryStream(new byte[1024]) // 1KB image
        };
        
        // Act
        var cost = await _costAnalyzer.CalculateCostAsync(mockProvider.Object, request);
        
        // Assert
        cost.Should().Be(0.001m);
    }
    
    [TestMethod]
    public async Task CalculateCostAsync_ShouldAddSizeBasedCost()
    {
        // Arrange
        var mockProvider = new Mock<IOCRProvider>();
        mockProvider.Setup(p => p.Capabilities).Returns(new OCRProviderCapabilities
        {
            CostPerRequest = 0.001m
        });
        
        // 2MB image (2 * 1024 * 1024 bytes)
        var imageSize = 2 * 1024 * 1024;
        var request = new OCRRequest
        {
            ImageStream = new MemoryStream(new byte[imageSize])
        };
        
        // Act
        var cost = await _costAnalyzer.CalculateCostAsync(mockProvider.Object, request);
        
        // Assert
        // Base cost + (2MB - 1MB) * 0.0001 = 0.001 + 0.0001 = 0.0011
        cost.Should().BeApproximately(0.0011m, 0.00001m);
    }
    
    [TestMethod]
    public async Task RecordCostAsync_ShouldRecordCost()
    {
        // Arrange
        var providerName = "Test Provider";
        var cost = 0.005m;
        var timestamp = DateTime.UtcNow;
        
        // Act
        await _costAnalyzer.RecordCostAsync(providerName, cost, timestamp);
        
        // Assert
        var totalCost = await _costAnalyzer.GetTotalCostAsync(TimeSpan.FromMinutes(1));
        totalCost.Should().Be(cost);
    }
    
    [TestMethod]
    public async Task GenerateCostReportAsync_ShouldReturnCorrectReport()
    {
        // Arrange
        var providerName = "Test Provider";
        var cost1 = 0.001m;
        var cost2 = 0.002m;
        var timestamp = DateTime.UtcNow;
        
        await _costAnalyzer.RecordCostAsync(providerName, cost1, timestamp);
        await _costAnalyzer.RecordCostAsync(providerName, cost2, timestamp);
        
        // Act
        var report = await _costAnalyzer.GenerateCostReportAsync(TimeSpan.FromMinutes(1));
        
        // Assert
        report.TotalCost.Should().Be(0.003m);
        report.TotalRequests.Should().Be(2);
        report.AverageCostPerRequest.Should().Be(0.0015m);
        report.ProviderBreakdown.Should().ContainKey(providerName);
        report.ProviderBreakdown[providerName].Should().Be(0.003m);
    }
    
    [TestMethod]
    public async Task GetCostBreakdownAsync_ShouldReturnCorrectBreakdown()
    {
        // Arrange
        var provider1 = "Provider 1";
        var provider2 = "Provider 2";
        var cost1 = 0.001m;
        var cost2 = 0.002m;
        var timestamp = DateTime.UtcNow;
        
        await _costAnalyzer.RecordCostAsync(provider1, cost1, timestamp);
        await _costAnalyzer.RecordCostAsync(provider2, cost2, timestamp);
        
        // Act
        var breakdown = await _costAnalyzer.GetCostBreakdownAsync(TimeSpan.FromMinutes(1));
        var breakdownList = breakdown.ToList();
        
        // Assert
        breakdownList.Should().HaveCount(2);
        breakdownList.Should().Contain(b => b.ProviderName == provider1 && b.TotalCost == cost1);
        breakdownList.Should().Contain(b => b.ProviderName == provider2 && b.TotalCost == cost2);
    }
}
