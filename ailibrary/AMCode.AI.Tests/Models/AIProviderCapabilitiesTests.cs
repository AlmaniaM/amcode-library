using AMCode.AI.Models;
using FluentAssertions;
using Xunit;

namespace AMCode.AI.Tests.Models;

public class AIProviderCapabilitiesTests
{
    [Fact]
    public void AIProviderCapabilities_ShouldInitializeWithDefaultValues()
    {
        // Act
        var capabilities = new AIProviderCapabilities();
        
        // Assert
        capabilities.SupportsStreaming.Should().BeFalse();
        capabilities.SupportsFunctionCalling.Should().BeFalse();
        capabilities.SupportsVision.Should().BeFalse();
        capabilities.SupportsLongContext.Should().BeFalse();
        capabilities.MaxTokens.Should().Be(0);
        capabilities.MaxContextLength.Should().Be(0);
        capabilities.SupportedLanguages.Should().NotBeNull().And.BeEmpty();
        capabilities.CostPerToken.Should().Be(0m);
        capabilities.CostPerRequest.Should().Be(0m);
        capabilities.AverageResponseTime.Should().Be(TimeSpan.Zero);
        capabilities.SupportsCustomModels.Should().BeFalse();
        capabilities.SupportsFineTuning.Should().BeFalse();
        capabilities.SupportsEmbeddings.Should().BeFalse();
        capabilities.SupportsModeration.Should().BeFalse();
        capabilities.MaxRequestsPerMinute.Should().Be(0);
        capabilities.MaxRequestsPerDay.Should().Be(0);
    }
    
    [Fact]
    public void AIProviderCapabilities_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var capabilities = new AIProviderCapabilities();
        var supportedLanguages = new[] { "en", "es", "fr" };
        var responseTime = TimeSpan.FromSeconds(2.5);
        
        // Act
        capabilities.SupportsStreaming = true;
        capabilities.SupportsFunctionCalling = true;
        capabilities.SupportsVision = true;
        capabilities.SupportsLongContext = true;
        capabilities.MaxTokens = 4096;
        capabilities.MaxContextLength = 128000;
        capabilities.SupportedLanguages = supportedLanguages;
        capabilities.CostPerToken = 0.0001m;
        capabilities.CostPerRequest = 0.01m;
        capabilities.AverageResponseTime = responseTime;
        capabilities.SupportsCustomModels = true;
        capabilities.SupportsFineTuning = true;
        capabilities.SupportsEmbeddings = true;
        capabilities.SupportsModeration = true;
        capabilities.MaxRequestsPerMinute = 100;
        capabilities.MaxRequestsPerDay = 1000;
        
        // Assert
        capabilities.SupportsStreaming.Should().BeTrue();
        capabilities.SupportsFunctionCalling.Should().BeTrue();
        capabilities.SupportsVision.Should().BeTrue();
        capabilities.SupportsLongContext.Should().BeTrue();
        capabilities.MaxTokens.Should().Be(4096);
        capabilities.MaxContextLength.Should().Be(128000);
        capabilities.SupportedLanguages.Should().BeEquivalentTo(supportedLanguages);
        capabilities.CostPerToken.Should().Be(0.0001m);
        capabilities.CostPerRequest.Should().Be(0.01m);
        capabilities.AverageResponseTime.Should().Be(responseTime);
        capabilities.SupportsCustomModels.Should().BeTrue();
        capabilities.SupportsFineTuning.Should().BeTrue();
        capabilities.SupportsEmbeddings.Should().BeTrue();
        capabilities.SupportsModeration.Should().BeTrue();
        capabilities.MaxRequestsPerMinute.Should().Be(100);
        capabilities.MaxRequestsPerDay.Should().Be(1000);
    }
}
