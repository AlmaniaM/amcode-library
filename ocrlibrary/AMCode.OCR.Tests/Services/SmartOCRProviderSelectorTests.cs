using NUnit.Framework;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using AMCode.OCR.Services;
using AMCode.OCR.Models;
using AMCode.OCR.Providers;
using AMCode.OCR.Enums;

namespace AMCode.OCR.Tests.Services;

[TestFixture]
public class SmartOCRProviderSelectorTests
{
    private Mock<ILogger<SmartOCRProviderSelector>> _mockLogger = null!;
    private List<IOCRProvider> _providers = null!;
    private SmartOCRProviderSelector _selector = null!;
    
    [SetUp]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger<SmartOCRProviderSelector>>();
        
        // Create mock providers
        var azureProvider = CreateMockProvider("Azure Computer Vision", 0.0005m, TimeSpan.FromSeconds(2), true, true);
        var awsProvider = CreateMockProvider("AWS Textract", 0.0015m, TimeSpan.FromSeconds(3), false, true);
        var googleProvider = CreateMockProvider("Google Cloud Vision", 0.001m, TimeSpan.FromSeconds(1.5), true, false);
        
        _providers = new List<IOCRProvider> { azureProvider.Object, awsProvider.Object, googleProvider.Object };
        _selector = new SmartOCRProviderSelector(_providers, _mockLogger.Object, OCRProviderSelectionStrategy.PerformanceOptimized);
    }
    
    [Test]
    public async Task SelectBestProviderAsync_PerformanceOptimized_ShouldSelectFastestProvider()
    {
        // Arrange
        var request = new OCRRequest
        {
            RequiresLanguageDetection = true,
            RequiresHandwritingSupport = false
        };
        
        // Act
        var selectedProvider = await _selector.SelectBestProviderAsync(request);
        
        // Assert
        selectedProvider.ProviderName.Should().Be("Google Cloud Vision"); // Fastest response time
    }
    
    [Test]
    public async Task SelectBestProviderAsync_CostOptimized_ShouldSelectCheapestProvider()
    {
        // Arrange
        var selector = new SmartOCRProviderSelector(_providers, _mockLogger.Object, OCRProviderSelectionStrategy.CostOptimized);
        var request = new OCRRequest
        {
            RequiresLanguageDetection = true,
            RequiresHandwritingSupport = true
        };
        
        // Act
        var selectedProvider = await selector.SelectBestProviderAsync(request);
        
        // Assert
        selectedProvider.ProviderName.Should().Be("Azure Computer Vision"); // Cheapest cost
    }
    
    [Test]
    public async Task SelectBestProviderAsync_CapabilityOptimized_ShouldSelectProviderWithRequiredCapabilities()
    {
        // Arrange
        var selector = new SmartOCRProviderSelector(_providers, _mockLogger.Object, OCRProviderSelectionStrategy.CapabilityOptimized);
        var request = new OCRRequest
        {
            RequiresLanguageDetection = true,
            RequiresHandwritingSupport = true
        };
        
        // Act
        var selectedProvider = await selector.SelectBestProviderAsync(request);
        
        // Assert
        selectedProvider.ProviderName.Should().Be("Azure Computer Vision"); // Only one with both capabilities
    }
    
    [Test]
    public async Task GetAvailableProvidersAsync_ShouldReturnAllAvailableProviders()
    {
        // Act
        var availableProviders = await _selector.GetAvailableProvidersAsync();
        
        // Assert
        availableProviders.Should().HaveCount(3);
        availableProviders.Should().Contain(p => p.ProviderName == "Azure Computer Vision");
        availableProviders.Should().Contain(p => p.ProviderName == "AWS Textract");
        availableProviders.Should().Contain(p => p.ProviderName == "Google Cloud Vision");
    }
    
    [Test]
    public async Task GetProviderHealthAsync_ShouldReturnHealthStatus()
    {
        // Arrange
        var providerName = "Azure Computer Vision";
        
        // Act
        var health = await _selector.GetProviderHealthAsync(providerName);
        
        // Assert
        health.Should().NotBeNull();
        health.IsHealthy.Should().BeTrue();
    }
    
    [Test]
    public async Task GetProviderHealthAsync_WithNonExistentProvider_ShouldReturnUnhealthy()
    {
        // Arrange
        var providerName = "Non-existent Provider";
        
        // Act
        var health = await _selector.GetProviderHealthAsync(providerName);
        
        // Assert
        health.Should().NotBeNull();
        health.IsHealthy.Should().BeFalse();
        health.Status.Should().Be("Provider not found");
    }
    
    private Mock<IOCRProvider> CreateMockProvider(string name, decimal cost, TimeSpan responseTime, bool supportsLanguageDetection, bool supportsHandwriting)
    {
        var mockProvider = new Mock<IOCRProvider>();
        mockProvider.Setup(p => p.ProviderName).Returns(name);
        mockProvider.Setup(p => p.IsAvailable).Returns(true);
        mockProvider.Setup(p => p.RequiresInternet).Returns(true);
        mockProvider.Setup(p => p.Capabilities).Returns(new OCRProviderCapabilities
        {
            SupportsLanguageDetection = supportsLanguageDetection,
            SupportsHandwriting = supportsHandwriting,
            SupportsBoundingBoxes = true,
            SupportsConfidenceScores = true,
            SupportsPrintedText = true,
            MaxImageSizeMB = 10,
            SupportedLanguages = new[] { "en", "es", "fr" },
            CostPerRequest = cost,
            AverageResponseTime = responseTime
        });
        
        mockProvider.Setup(p => p.CheckHealthAsync()).ReturnsAsync(new OCRProviderHealth
        {
            IsHealthy = true,
            Status = "Healthy",
            ResponseTime = responseTime,
            LastChecked = DateTime.UtcNow
        });
        
        // Setup CanProcess to return true for any request
        mockProvider.Setup(p => p.CanProcess(It.IsAny<OCRRequest>())).Returns(true);
        
        return mockProvider;
    }
}
