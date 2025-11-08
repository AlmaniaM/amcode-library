using AMCode.AI.Models;
using AMCode.AI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace AMCode.AI.Tests.Services;

public class PromptBuilderServiceTests
{
    private Mock<ILogger<PromptBuilderService>> _mockLogger;
    private PromptBuilderService _service;
    
    public PromptBuilderServiceTests()
    {
        _mockLogger = new Mock<ILogger<PromptBuilderService>>();
        _service = new PromptBuilderService(_mockLogger.Object);
    }
    
    [Fact]
    public void BuildRecipeParsingPrompt_WithDefaultOptions_ShouldReturnFormattedPrompt()
    {
        // Arrange
        var text = "Chocolate Chip Cookies\n2 cups flour\n1 cup sugar\nMix ingredients and bake at 350°F for 12 minutes";
        
        // Act
        var result = _service.BuildRecipeParsingPrompt(text);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Please parse the following recipe text");
        result.Should().Contain("Return the result as JSON");
        result.Should().Contain(text);
        result.Should().Contain("JSON response:");
    }
    
    [Fact]
    public void BuildRecipeParsingPrompt_WithCustomOptions_ShouldIncludeOptionsInPrompt()
    {
        // Arrange
        var text = "Test recipe";
        var options = new RecipeParsingOptions
        {
            MaxTokens = 2000,
            Temperature = 0.5f,
            Language = "es",
            ExtractNutritionalInfo = true,
            CustomInstructions = "Focus on cooking techniques"
        };
        
        // Act
        var result = _service.BuildRecipeParsingPrompt(text, options);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Limit response to 2000 tokens");
        result.Should().Contain("Use temperature 0.5 for creativity");
        result.Should().Contain("Parse in es language");
        result.Should().Contain("Extract nutritional information if available");
        result.Should().Contain("Focus on cooking techniques");
    }
    
    [Fact]
    public void BuildSimpleRecipeParsingPrompt_ShouldReturnSimplifiedPrompt()
    {
        // Arrange
        var text = "Simple recipe text";
        
        // Act
        var result = _service.BuildSimpleRecipeParsingPrompt(text);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Parse this recipe and return JSON");
        result.Should().Contain("title");
        result.Should().Contain("ingredients");
        result.Should().Contain("instructions");
        result.Should().Contain(text);
        result.Should().Contain("JSON:");
    }
    
    [Fact]
    public void BuildFunctionCallingPrompt_ShouldReturnFunctionCallingPrompt()
    {
        // Arrange
        var text = "Function calling recipe";
        var options = new RecipeParsingOptions
        {
            RequiresFunctionCalling = true
        };
        
        // Act
        var result = _service.BuildFunctionCallingPrompt(text, options);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Parse the following recipe text");
        result.Should().Contain("Use the provided function");
        result.Should().Contain("Title");
        result.Should().Contain("Ingredients");
        result.Should().Contain("Instructions");
        result.Should().Contain(text);
    }
    
    [Fact]
    public void BuildRecipeParsingPrompt_WithNullOptions_ShouldNotThrow()
    {
        // Arrange
        var text = "Test recipe";
        
        // Act & Assert
        var action = () => _service.BuildRecipeParsingPrompt(text, null);
        action.Should().NotThrow();
    }
    
    [Fact]
    public void BuildRecipeParsingPrompt_WithEmptyText_ShouldReturnValidPrompt()
    {
        // Arrange
        var text = "";
        
        // Act
        var result = _service.BuildRecipeParsingPrompt(text);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Please parse the following recipe text");
    }
    
    [Fact]
    public void BuildRecipeParsingPrompt_WithAllOptions_ShouldIncludeAllOptions()
    {
        // Arrange
        var text = "Test recipe";
        var options = new RecipeParsingOptions
        {
            RequiresFunctionCalling = true,
            RequiresVision = true,
            MaxTokens = 1000,
            Temperature = 0.3f,
            Language = "fr",
            ExtractNutritionalInfo = true,
            ExtractCookingTechniques = true,
            ExtractEquipment = true,
            MaxRecipes = 3,
            CustomInstructions = "Be very detailed"
        };
        
        // Act
        var result = _service.BuildRecipeParsingPrompt(text, options);
        
        // Assert
        result.Should().Contain("Use function calling if available");
        result.Should().Contain("Analyze any images if present");
        result.Should().Contain("Limit response to 1000 tokens");
        result.Should().Contain("Use temperature 0.3 for creativity");
        result.Should().Contain("Parse in fr language");
        result.Should().Contain("Extract nutritional information if available");
        result.Should().Contain("Extract cooking techniques and methods");
        result.Should().Contain("Extract required equipment and tools");
        result.Should().Contain("Extract up to 3 recipes if multiple are present");
        result.Should().Contain("Be very detailed");
    }
}
