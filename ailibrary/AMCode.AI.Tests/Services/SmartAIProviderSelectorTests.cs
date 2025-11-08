using AMCode.AI.Enums;
using AMCode.AI.Models;
using AMCode.AI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace AMCode.AI.Tests.Services;

public class SmartAIProviderSelectorTests
{
    private Mock<ILogger<SmartAIProviderSelector>> _mockLogger;
    private Mock<ICostAnalyzer> _mockCostAnalyzer;
    private List<IAIProvider> _providers;
    private SmartAIProviderSelector _selector;
    
    public SmartAIProviderSelectorTests()
    {
        _mockLogger = new Mock<ILogger<SmartAIProviderSelector>>();
        _mockCostAnalyzer = new Mock<ICostAnalyzer>();
        _providers = new List<IAIProvider>();
        
        // Create mock providers with different capabilities
        var provider1 = CreateMockProvider("Provider1", 0.0001m, TimeSpan.FromSeconds(1), 4000);
        var provider2 = CreateMockProvider("Provider2", 0.0002m, TimeSpan.FromSeconds(2), 8000);
        var provider3 = CreateMockProvider("Provider3", 0.00005m, TimeSpan.FromSeconds(3), 2000);
        
        _providers.Add(provider1);
        _providers.Add(provider2);
        _providers.Add(provider3);
        
        _selector = new SmartAIProviderSelector(_providers, _mockLogger.Object, _mockCostAnalyzer.Object, AIProviderSelectionStrategy.Balanced);
    }
    
    [Fact]
    public async Task SelectBestProviderAsync_CostOptimized_ShouldSelectCheapestProvider()
    {
        // Arrange
        var selector = new SmartAIProviderSelector(_providers, _mockLogger.Object, _mockCostAnalyzer.Object, AIProviderSelectionStrategy.CostOptimized);
        var request = new AIRequest
        {
            Text = "Test recipe",
            EstimatedTokens = 1000,
            Options = new RecipeParsingOptions()
        };
        
        // Act
        var result = await selector.SelectBestProviderAsync(request);
        
        // Assert
        result.ProviderName.Should().Be("Provider3"); // Cheapest
    }
    
    [Fact]
    public async Task SelectBestProviderAsync_PerformanceOptimized_ShouldSelectFastestProvider()
    {
        // Arrange
        var selector = new SmartAIProviderSelector(_providers, _mockLogger.Object, _mockCostAnalyzer.Object, AIProviderSelectionStrategy.PerformanceOptimized);
        var request = new AIRequest
        {
            Text = "Test recipe",
            EstimatedTokens = 1000,
            Options = new RecipeParsingOptions()
        };
        
        // Act
        var result = await selector.SelectBestProviderAsync(request);
        
        // Assert
        result.ProviderName.Should().Be("Provider1"); // Fastest
    }
    
    [Fact]
    public async Task SelectBestProviderAsync_CapabilityOptimized_ShouldSelectProviderWithRequiredCapabilities()
    {
        // Arrange
        var selector = new SmartAIProviderSelector(_providers, _mockLogger.Object, _mockCostAnalyzer.Object, AIProviderSelectionStrategy.CapabilityOptimized);
        var request = new AIRequest
        {
            Text = "Test recipe",
            EstimatedTokens = 1000,
            RequiresFunctionCalling = true,
            Options = new RecipeParsingOptions()
        };
        
        // Act
        var result = await selector.SelectBestProviderAsync(request);
        
        // Assert
        result.ProviderName.Should().Be("Provider1"); // Has function calling
    }
    
    [Fact]
    public async Task GetAvailableProvidersAsync_ShouldReturnAllAvailableProviders()
    {
        // Act
        var result = await _selector.GetAvailableProvidersAsync();
        
        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(p => p.ProviderName == "Provider1");
        result.Should().Contain(p => p.ProviderName == "Provider2");
        result.Should().Contain(p => p.ProviderName == "Provider3");
    }
    
    [Fact]
    public async Task GetProviderHealthAsync_ShouldReturnHealthStatus()
    {
        // Arrange
        var providerName = "Provider1";
        var expectedHealth = new AIProviderHealth
        {
            IsHealthy = true,
            Status = "Healthy",
            LastChecked = DateTime.UtcNow
        };
        
        var provider = _providers.First(p => p.ProviderName == providerName);
        var mockProvider = Mock.Get(provider);
        mockProvider.Setup(p => p.CheckHealthAsync()).ReturnsAsync(expectedHealth);
        
        // Act
        var result = await _selector.GetProviderHealthAsync(providerName);
        
        // Assert
        result.Should().NotBeNull();
        result.IsHealthy.Should().BeTrue();
        result.Status.Should().Be("Healthy");
    }
    
    [Fact]
    public async Task GetProviderHealthAsync_WithNonExistentProvider_ShouldReturnUnhealthy()
    {
        // Act
        var result = await _selector.GetProviderHealthAsync("NonExistentProvider");
        
        // Assert
        result.Should().NotBeNull();
        result.IsHealthy.Should().BeFalse();
        result.Status.Should().Be("Provider not found");
    }
    
    [Fact]
    public async Task GetCostEstimateAsync_ShouldReturnCostEstimate()
    {
        // Arrange
        var request = new AIRequest
        {
            Text = "Test recipe",
            EstimatedTokens = 1000,
            Options = new RecipeParsingOptions()
        };
        
        var expectedCost = 0.05m;
        var provider = _providers.First(p => p.ProviderName == "Provider3");
        var mockProvider = Mock.Get(provider);
        mockProvider.Setup(p => p.GetCostEstimateAsync(It.IsAny<string>(), It.IsAny<RecipeParsingOptions>()))
                   .ReturnsAsync(expectedCost);
        
        // Act
        var result = await _selector.GetCostEstimateAsync(request);
        
        // Assert
        result.Should().Be(expectedCost);
    }
    
    private IAIProvider CreateMockProvider(string name, decimal costPerToken, TimeSpan responseTime, int maxTokens)
    {
        var mockProvider = new Mock<IAIProvider>();
        mockProvider.Setup(p => p.ProviderName).Returns(name);
        mockProvider.Setup(p => p.IsAvailable).Returns(true);
        mockProvider.Setup(p => p.RequiresInternet).Returns(true);
        mockProvider.Setup(p => p.Capabilities).Returns(new AIProviderCapabilities
        {
            CostPerToken = costPerToken,
            AverageResponseTime = responseTime,
            MaxTokens = maxTokens,
            SupportsFunctionCalling = name == "Provider1",
            SupportsVision = false,
            SupportsLongContext = true
        });
        mockProvider.Setup(p => p.CheckHealthAsync()).ReturnsAsync(new AIProviderHealth
        {
            IsHealthy = true,
            Status = "Healthy",
            LastChecked = DateTime.UtcNow
        });
        mockProvider.Setup(p => p.GetCostEstimateAsync(It.IsAny<string>(), It.IsAny<RecipeParsingOptions>()))
                   .ReturnsAsync(costPerToken * 1000); // Simple cost calculation
        
        return mockProvider.Object;
    }
}
