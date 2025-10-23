using AMCode.AI.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMCode.AI.Tests.Models;

[TestClass]
public class AIRequestTests
{
    [TestMethod]
    public void AIRequest_ShouldInitializeWithDefaultValues()
    {
        // Act
        var request = new AIRequest();
        
        // Assert
        request.Text.Should().BeEmpty();
        request.Options.Should().NotBeNull();
        request.EstimatedTokens.Should().Be(0);
        request.RequiresFunctionCalling.Should().BeFalse();
        request.RequiresVision.Should().BeFalse();
        request.MaxRetries.Should().Be(3);
        request.ConfidenceThreshold.Should().Be(0.7);
        request.Priority.Should().Be(5);
        request.Timeout.Should().Be(TimeSpan.FromMinutes(5));
        request.Metadata.Should().NotBeNull().And.BeEmpty();
    }
    
    [TestMethod]
    public void AIRequest_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var request = new AIRequest();
        var options = new RecipeParsingOptions { MaxTokens = 1000 };
        var metadata = new Dictionary<string, object> { { "key", "value" } };
        var timeout = TimeSpan.FromMinutes(10);
        
        // Act
        request.Text = "Test recipe text";
        request.Options = options;
        request.EstimatedTokens = 500;
        request.RequiresFunctionCalling = true;
        request.RequiresVision = true;
        request.MaxRetries = 5;
        request.ConfidenceThreshold = 0.8;
        request.Priority = 8;
        request.Timeout = timeout;
        request.Metadata = metadata;
        
        // Assert
        request.Text.Should().Be("Test recipe text");
        request.Options.Should().Be(options);
        request.EstimatedTokens.Should().Be(500);
        request.RequiresFunctionCalling.Should().BeTrue();
        request.RequiresVision.Should().BeTrue();
        request.MaxRetries.Should().Be(5);
        request.ConfidenceThreshold.Should().Be(0.8);
        request.Priority.Should().Be(8);
        request.Timeout.Should().Be(timeout);
        request.Metadata.Should().BeEquivalentTo(metadata);
    }
}
