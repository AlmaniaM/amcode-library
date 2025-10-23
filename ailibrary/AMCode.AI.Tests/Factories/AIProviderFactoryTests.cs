using AMCode.AI.Factories;
using AMCode.AI.Providers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AMCode.AI.Tests.Factories;

public class AIProviderFactoryTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ILogger<AIProviderFactory>> _mockLogger;
    private readonly Mock<ILogger<OpenAIGPTProvider>> _mockOpenAILogger;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AIProviderFactory _factory;

    public AIProviderFactoryTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLogger = new Mock<ILogger<AIProviderFactory>>();
        _mockOpenAILogger = new Mock<ILogger<OpenAIGPTProvider>>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup service provider mocks
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILogger<OpenAIGPTProvider>)))
            .Returns(_mockOpenAILogger.Object);
        _mockServiceProvider.Setup(sp => sp.GetRequiredService<ILogger<OpenAIGPTProvider>>())
            .Returns(_mockOpenAILogger.Object);
        _mockServiceProvider.Setup(sp => sp.GetRequiredService<IHttpClientFactory>())
            .Returns(_mockHttpClientFactory.Object);

        _factory = new AIProviderFactory(_mockServiceProvider.Object, _mockLogger.Object);
    }

    [Fact]
    public void RegisterProvider_ShouldAddProviderToRegistry()
    {
        // Arrange
        var providerName = "TestProvider";
        var factoryFunc = new Mock<Func<IServiceProvider, IAIProvider>>();

        // Act
        _factory.RegisterProvider<OpenAIGPTProvider>(providerName, factoryFunc.Object);

        // Assert
        _factory.IsProviderRegistered(providerName).Should().BeTrue();
        _factory.GetRegisteredProviders().Should().Contain(providerName);
    }

    [Fact]
    public void IsProviderRegistered_WithUnregisteredProvider_ShouldReturnFalse()
    {
        // Act
        var result = _factory.IsProviderRegistered("NonExistentProvider");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetRegisteredProviders_WithNoProviders_ShouldReturnEmptyList()
    {
        // Act
        var providers = _factory.GetRegisteredProviders();

        // Assert
        providers.Should().BeEmpty();
    }

    [Fact]
    public void GetRegisteredProviders_WithMultipleProviders_ShouldReturnAllProviders()
    {
        // Arrange
        var provider1 = "Provider1";
        var provider2 = "Provider2";
        var factoryFunc = new Mock<Func<IServiceProvider, IAIProvider>>();

        _factory.RegisterProvider<OpenAIGPTProvider>(provider1, factoryFunc.Object);
        _factory.RegisterProvider<OpenAIGPTProvider>(provider2, factoryFunc.Object);

        // Act
        var providers = _factory.GetRegisteredProviders();

        // Assert
        providers.Should().HaveCount(2);
        providers.Should().Contain(provider1);
        providers.Should().Contain(provider2);
    }

    [Fact]
    public void CreateProvider_WithValidType_ShouldCreateProvider()
    {
        // Arrange
        var providerName = "OpenAI";
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(s => s.Exists()).Returns(true);
        _mockConfiguration.Setup(c => c.GetSection($"AI:{providerName}"))
            .Returns(configSection.Object);

        // Act
        var provider = _factory.CreateProvider<OpenAIGPTProvider>(providerName, _mockConfiguration.Object);

        // Assert
        provider.Should().NotBeNull();
        provider.Should().BeOfType<OpenAIGPTProvider>();
    }

    [Fact]
    public void CreateCustomProvider_WithValidType_ShouldCreateProvider()
    {
        // Arrange
        var providerName = "CustomProvider";
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(s => s.Exists()).Returns(true);
        _mockConfiguration.Setup(c => c.GetSection($"AI:{providerName}"))
            .Returns(configSection.Object);

        // Act
        var provider = _factory.CreateCustomProvider(providerName, typeof(OpenAIGPTProvider), _mockConfiguration.Object);

        // Assert
        provider.Should().NotBeNull();
        provider.Should().BeOfType<OpenAIGPTProvider>();
    }

    [Fact]
    public void CreateCustomProvider_WithInvalidType_ShouldThrowException()
    {
        // Arrange
        var providerName = "InvalidProvider";
        var invalidType = typeof(string); // Not a GenericAIProvider

        // Act & Assert
        var action = () => _factory.CreateCustomProvider(providerName, invalidType, _mockConfiguration.Object);
        action.Should().Throw<ArgumentException>()
            .WithMessage("*must inherit from GenericAIProvider*");
    }

    [Fact]
    public void CreateProvider_WithConfigurationBinding_ShouldBindConfiguration()
    {
        // Arrange
        var providerName = "OpenAI";
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(s => s.Exists()).Returns(true);
        configSection.Setup(s => s["ApiKey"]).Returns("test-api-key");
        configSection.Setup(s => s["Model"]).Returns("gpt-4");
        
        _mockConfiguration.Setup(c => c.GetSection($"AI:{providerName}"))
            .Returns(configSection.Object);

        // Act
        var provider = _factory.CreateProvider<OpenAIGPTProvider>(providerName, _mockConfiguration.Object);

        // Assert
        provider.Should().NotBeNull();
        // Note: In a real test, you would verify the configuration was properly bound
        // This would require exposing the configuration or testing the provider's behavior
    }

    [Fact]
    public void CreateProvider_WithMissingConfiguration_ShouldStillCreateProvider()
    {
        // Arrange
        var providerName = "OpenAI";
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(s => s.Exists()).Returns(false);
        
        _mockConfiguration.Setup(c => c.GetSection($"AI:{providerName}"))
            .Returns(configSection.Object);

        // Act
        var provider = _factory.CreateProvider<OpenAIGPTProvider>(providerName, _mockConfiguration.Object);

        // Assert
        provider.Should().NotBeNull();
        provider.Should().BeOfType<OpenAIGPTProvider>();
    }

    [Fact]
    public void CreateProvider_WithServiceProviderFailure_ShouldThrowException()
    {
        // Arrange
        var providerName = "OpenAI";
        _mockServiceProvider.Setup(sp => sp.GetRequiredService<IHttpClientFactory>())
            .Throws(new InvalidOperationException("Service not found"));

        // Act & Assert
        var action = () => _factory.CreateProvider<OpenAIGPTProvider>(providerName, _mockConfiguration.Object);
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*Failed to create provider*");
    }
}
