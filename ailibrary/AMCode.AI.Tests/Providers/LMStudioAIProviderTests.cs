using AMCode.AI.Configurations;
using AMCode.AI.Models;
using AMCode.AI.Providers;
using AMCode.AI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace AMCode.AI.Tests.Providers;

/// <summary>
/// Unit tests for LM Studio AI provider
/// </summary>
public class LMStudioAIProviderTests
{
    private readonly Mock<ILogger<LMStudioAIProvider>> _loggerMock;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<PromptBuilderService> _promptBuilderMock;
    private readonly LMStudioConfiguration _config;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    public LMStudioAIProviderTests()
    {
        _loggerMock = new Mock<ILogger<LMStudioAIProvider>>();
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _promptBuilderMock = new Mock<PromptBuilderService>(
            Mock.Of<ILogger<PromptBuilderService>>());
        
        _config = new LMStudioConfiguration
        {
            BaseUrl = "http://localhost:1234",
            Model = "test-model",
            MaxTokens = 4096,
            Temperature = 0.1f,
            Timeout = TimeSpan.FromMinutes(10),
            AverageResponseTime = TimeSpan.FromSeconds(5)
        };

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        
        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri(_config.BaseUrl)
        };
        
        _httpClientFactoryMock
            .Setup(x => x.CreateClient("LM Studio Local"))
            .Returns(httpClient);
    }

    [Fact]
    public void ProviderName_ShouldReturnCorrectName()
    {
        // Arrange
        var provider = CreateProvider();

        // Act
        var name = provider.ProviderName;

        // Assert
        name.Should().Be("LM Studio Local");
    }

    [Fact]
    public void RequiresInternet_ShouldReturnFalse()
    {
        // Arrange
        var provider = CreateProvider();

        // Act
        var requiresInternet = provider.RequiresInternet;

        // Assert
        requiresInternet.Should().BeFalse();
    }

    [Fact]
    public void Capabilities_ShouldReturnCorrectValues()
    {
        // Arrange
        var provider = CreateProvider();

        // Act
        var capabilities = provider.Capabilities;

        // Assert
        capabilities.SupportsStreaming.Should().BeTrue();
        capabilities.SupportsCustomModels.Should().BeTrue();
        capabilities.CostPerToken.Should().Be(0m);
        capabilities.CostPerRequest.Should().Be(0m);
        capabilities.MaxRequestsPerMinute.Should().Be(1000);
        capabilities.MaxRequestsPerDay.Should().Be(int.MaxValue);
    }

    [Fact]
    public void IsAvailable_ShouldReturnFalse_WhenBaseUrlNotConfigured()
    {
        // Arrange
        var emptyConfig = new LMStudioConfiguration
        {
            BaseUrl = string.Empty,
            Model = "test-model"
        };
        var provider = new LMStudioAIProvider(
            _loggerMock.Object,
            _httpClientFactoryMock.Object,
            Options.Create(emptyConfig),
            _promptBuilderMock.Object);

        // Act
        var isAvailable = provider.IsAvailable;

        // Assert
        isAvailable.Should().BeFalse();
    }

    [Fact]
    public void IsAvailable_ShouldReturnFalse_WhenModelNotConfigured()
    {
        // Arrange
        var emptyConfig = new LMStudioConfiguration
        {
            BaseUrl = "http://localhost:1234",
            Model = string.Empty
        };
        var provider = new LMStudioAIProvider(
            _loggerMock.Object,
            _httpClientFactoryMock.Object,
            Options.Create(emptyConfig),
            _promptBuilderMock.Object);

        // Act
        var isAvailable = provider.IsAvailable;

        // Assert
        isAvailable.Should().BeFalse();
    }

    [Fact]
    public void IsAvailable_ShouldReturnTrue_WhenProperlyConfigured()
    {
        // Arrange
        var provider = CreateProvider();

        // Act
        var isAvailable = provider.IsAvailable;

        // Assert
        isAvailable.Should().BeTrue();
    }

    [Fact]
    public async Task GetCostEstimateAsync_ShouldReturnZero_ForLocalProvider()
    {
        // Arrange
        var provider = CreateProvider();
        var options = new RecipeParsingOptions();

        // Act
        var cost = await provider.GetCostEstimateAsync("test text", options);

        // Assert
        cost.Should().Be(0m);
    }

    [Fact]
    public async Task CheckHealthAsync_ShouldReturnHealthy_WhenServiceResponds()
    {
        // Arrange
        var provider = CreateProvider();
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(new
            {
                choices = new[]
                {
                    new { message = new { role = "assistant", content = "Hello" } }
                }
            }), Encoding.UTF8, "application/json")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        // Act
        var health = await provider.CheckHealthAsync();

        // Assert
        health.IsHealthy.Should().BeTrue();
        health.Status.Should().Be("Healthy");
    }

    [Fact]
    public async Task CheckHealthAsync_ShouldReturnUnhealthy_WhenServiceFails()
    {
        // Arrange
        var provider = CreateProvider();
        var mockResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        // Act
        var health = await provider.CheckHealthAsync();

        // Assert
        health.IsHealthy.Should().BeFalse();
        health.Status.Should().Be("Unhealthy");
    }

    [Fact]
    public async Task CheckHealthAsync_ShouldReturnUnhealthy_WhenClientNotInitialized()
    {
        // Arrange
        var emptyConfig = new LMStudioConfiguration
        {
            BaseUrl = string.Empty,
            Model = "test-model"
        };
        var provider = new LMStudioAIProvider(
            _loggerMock.Object,
            _httpClientFactoryMock.Object,
            Options.Create(emptyConfig),
            _promptBuilderMock.Object);

        // Act
        var health = await provider.CheckHealthAsync();

        // Assert
        health.IsHealthy.Should().BeFalse();
        health.Status.Should().Be("Client not initialized");
        health.ErrorMessage.Should().Contain("LM Studio client not initialized");
    }

    [Fact]
    public void ParseTextAsync_ShouldThrow_WhenModelNotConfigured()
    {
        // Arrange
        var emptyConfig = new LMStudioConfiguration
        {
            BaseUrl = "http://localhost:1234",
            Model = string.Empty
        };
        var provider = new LMStudioAIProvider(
            _loggerMock.Object,
            _httpClientFactoryMock.Object,
            Options.Create(emptyConfig),
            _promptBuilderMock.Object);

        // Act & Assert
        var action = async () => await provider.ParseTextAsync("test");
        action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*model not configured*");
    }

    private LMStudioAIProvider CreateProvider()
    {
        return new LMStudioAIProvider(
            _loggerMock.Object,
            _httpClientFactoryMock.Object,
            Options.Create(_config),
            _promptBuilderMock.Object);
    }
}

