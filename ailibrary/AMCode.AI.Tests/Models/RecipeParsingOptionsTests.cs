using AMCode.AI.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMCode.AI.Tests.Models;

[TestClass]
public class RecipeParsingOptionsTests
{
    [TestMethod]
    public void RecipeParsingOptions_ShouldInitializeWithDefaultValues()
    {
        // Act
        var options = new RecipeParsingOptions();
        
        // Assert
        options.MaxTokens.Should().BeNull();
        options.Temperature.Should().BeNull();
        options.RequiresFunctionCalling.Should().BeFalse();
        options.RequiresVision.Should().BeFalse();
        options.Language.Should().Be("en");
        options.IncludeConfidenceScores.Should().BeTrue();
        options.IncludeRawResponse.Should().BeFalse();
        options.CustomInstructions.Should().BeEmpty();
        options.ExtractNutritionalInfo.Should().BeFalse();
        options.ExtractCookingTechniques.Should().BeFalse();
        options.ExtractEquipment.Should().BeFalse();
        options.MaxRecipes.Should().Be(1);
        options.ValidateData.Should().BeTrue();
    }
    
    [TestMethod]
    public void RecipeParsingOptions_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var options = new RecipeParsingOptions();
        
        // Act
        options.MaxTokens = 2000;
        options.Temperature = 0.5f;
        options.RequiresFunctionCalling = true;
        options.RequiresVision = true;
        options.Language = "es";
        options.IncludeConfidenceScores = false;
        options.IncludeRawResponse = true;
        options.CustomInstructions = "Be very detailed";
        options.ExtractNutritionalInfo = true;
        options.ExtractCookingTechniques = true;
        options.ExtractEquipment = true;
        options.MaxRecipes = 3;
        options.ValidateData = false;
        
        // Assert
        options.MaxTokens.Should().Be(2000);
        options.Temperature.Should().Be(0.5f);
        options.RequiresFunctionCalling.Should().BeTrue();
        options.RequiresVision.Should().BeTrue();
        options.Language.Should().Be("es");
        options.IncludeConfidenceScores.Should().BeFalse();
        options.IncludeRawResponse.Should().BeTrue();
        options.CustomInstructions.Should().Be("Be very detailed");
        options.ExtractNutritionalInfo.Should().BeTrue();
        options.ExtractCookingTechniques.Should().BeTrue();
        options.ExtractEquipment.Should().BeTrue();
        options.MaxRecipes.Should().Be(3);
        options.ValidateData.Should().BeFalse();
    }
}
