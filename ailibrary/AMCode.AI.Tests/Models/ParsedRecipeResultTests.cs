using AMCode.AI.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMCode.AI.Tests.Models;

[TestClass]
public class ParsedRecipeResultTests
{
    [TestMethod]
    public void ParsedRecipeResult_ShouldInitializeWithDefaultValues()
    {
        // Act
        var result = new ParsedRecipeResult();
        
        // Assert
        result.Recipes.Should().NotBeNull().And.BeEmpty();
        result.Confidence.Should().Be(0.0);
        result.Source.Should().BeEmpty();
        result.ProcessingTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.Cost.Should().Be(0m);
        result.TokensUsed.Should().Be(0);
        result.Warnings.Should().NotBeNull().And.BeEmpty();
        result.RawResponse.Should().BeEmpty();
    }
    
    [TestMethod]
    public void ParsedRecipeResult_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var result = new ParsedRecipeResult();
        var now = DateTime.UtcNow;
        var recipe = new ParsedRecipe { Title = "Test Recipe" };
        
        // Act
        result.Recipes = new[] { recipe };
        result.Confidence = 0.95;
        result.Source = "OpenAI GPT";
        result.ProcessingTime = now;
        result.Cost = 0.05m;
        result.TokensUsed = 150;
        result.Warnings = new[] { "Low confidence on ingredient 3" };
        result.RawResponse = "{\"title\":\"Test Recipe\"}";
        
        // Assert
        result.Recipes.Should().Contain(recipe);
        result.Confidence.Should().Be(0.95);
        result.Source.Should().Be("OpenAI GPT");
        result.ProcessingTime.Should().Be(now);
        result.Cost.Should().Be(0.05m);
        result.TokensUsed.Should().Be(150);
        result.Warnings.Should().Contain("Low confidence on ingredient 3");
        result.RawResponse.Should().Be("{\"title\":\"Test Recipe\"}");
    }
}
