using NUnit.Framework;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using AMCode.OCR.Services;
using AMCode.OCR.Providers;
using AMCode.OCR.Configurations;
using AMCode.OCR.Enums;
using AMCode.OCR.Factories;

namespace AMCode.OCR.Tests.Factories;

[TestFixture]
public class OCRProviderSelectorFactoryTests
{
    private Mock<IServiceProvider> _mockServiceProvider = null!;
    private Mock<ILogger<OCRProviderSelectorFactory>> _mockLogger = null!;
    private Mock<IOptions<OCRConfiguration>> _mockOptions = null!;
    private Mock<ILogger<SmartOCRProviderSelector>> _mockSelectorLogger = null!;
    private List<IOCRProvider> _providers = null!;
    private OCRProviderSelectorFactory _factory = null!;

    [SetUp]
    public void Setup()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLogger = new Mock<ILogger<OCRProviderSelectorFactory>>();
        _mockOptions = new Mock<IOptions<OCRConfiguration>>();
        _mockSelectorLogger = new Mock<ILogger<SmartOCRProviderSelector>>();
        _providers = new List<IOCRProvider>();

        // Create mock providers
        var provider1 = CreateMockProvider("Azure Computer Vision");
        var provider2 = CreateMockProvider("AWS Textract");
        var provider3 = CreateMockProvider("Google Cloud Vision");

        _providers.Add(provider1.Object);
        _providers.Add(provider2.Object);
        _providers.Add(provider3.Object);

        // Setup service provider — GetServices<T>() calls GetService(typeof(IEnumerable<T>)) internally
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IEnumerable<IOCRProvider>)))
            .Returns(_providers);
        // GetRequiredService<T>() calls GetService(typeof(T)) internally
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILogger<SmartOCRProviderSelector>)))
            .Returns(_mockSelectorLogger.Object);

        // Setup default configuration
        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.Balanced,
            Provider = null
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        _factory = new OCRProviderSelectorFactory(
            _mockServiceProvider.Object,
            _mockLogger.Object,
            _mockOptions.Object);
    }

    [Test]
    public void CreateSelector_WithDefaultConfiguration_ShouldCreateSmartOCRProviderSelector()
    {
        // Arrange
        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.Balanced,
            Provider = null
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        selector.Should().BeOfType<SmartOCRProviderSelector>();
    }

    [Test]
    public void CreateSelector_WithCustomStrategy_ShouldCreateSelectorWithStrategy()
    {
        // Arrange
        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.PerformanceOptimized,
            Provider = "Azure Computer Vision"
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        selector.Should().BeOfType<SmartOCRProviderSelector>();
    }

    [Test]
    public void CreateSelector_WithDefaultProvider_ShouldCreateSelectorWithProvider()
    {
        // Arrange
        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.Balanced,
            Provider = "Azure Computer Vision"
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        selector.Should().BeOfType<SmartOCRProviderSelector>();
    }

    [Test]
    public void CreateSelector_ShouldUseAllProviders()
    {
        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(IEnumerable<IOCRProvider>)), Times.Once);
    }

    [Test]
    public void CreateSelector_ShouldGetLogger()
    {
        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(ILogger<SmartOCRProviderSelector>)), Times.Once);
    }

    [Test]
    public void CreateSelector_WithCostOptimizedStrategy_ShouldCreateSelector()
    {
        // Arrange
        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.CostOptimized,
            Provider = null
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        selector.Should().BeOfType<SmartOCRProviderSelector>();
    }

    [Test]
    public void CreateSelector_WithReliabilityOptimizedStrategy_ShouldCreateSelector()
    {
        // Arrange
        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.ReliabilityOptimized,
            Provider = null
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        selector.Should().BeOfType<SmartOCRProviderSelector>();
    }

    [Test]
    public void CreateSelector_WithConfigurationStrategy_ShouldCreateConfigurationOCRProviderSelector()
    {
        // Arrange
        var configLogger = new Mock<ILogger<ConfigurationOCRProviderSelector>>();
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILogger<ConfigurationOCRProviderSelector>)))
            .Returns(configLogger.Object);

        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.Configuration,
            Provider = "Azure Computer Vision"
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        selector.Should().BeOfType<ConfigurationOCRProviderSelector>();
    }

    [Test]
    public void CreateSelector_WithConfigurationStrategy_ShouldGetConfigurationLogger()
    {
        // Arrange
        var configLogger = new Mock<ILogger<ConfigurationOCRProviderSelector>>();
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILogger<ConfigurationOCRProviderSelector>)))
            .Returns(configLogger.Object);

        var config = new OCRConfiguration
        {
            ProviderSelectionStrategy = OCRProviderSelectionStrategy.Configuration,
            Provider = "Azure Computer Vision"
        };
        _mockOptions.Setup(o => o.Value).Returns(config);

        // Act
        var selector = _factory.CreateSelector();

        // Assert
        selector.Should().NotBeNull();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(ILogger<ConfigurationOCRProviderSelector>)), Times.Once);
    }

    private Mock<IOCRProvider> CreateMockProvider(string name)
    {
        var mockProvider = new Mock<IOCRProvider>();
        mockProvider.Setup(p => p.ProviderName).Returns(name);
        mockProvider.Setup(p => p.IsAvailable).Returns(true);
        return mockProvider;
    }
}

