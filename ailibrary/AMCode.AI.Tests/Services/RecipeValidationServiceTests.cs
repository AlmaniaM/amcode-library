using AMCode.AI.Models;
using AMCode.AI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AMCode.AI.Tests.Services;

public class RecipeValidationServiceTests
{
    private readonly Mock<ILogger<RecipeValidationService>> _mockLogger;
    private readonly RecipeValidationService _service;

    public RecipeValidationServiceTests()
    {
        _mockLogger = new Mock<ILogger<RecipeValidationService>>();
        _service = new RecipeValidationService(_mockLogger.Object);
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithValidRecipe_ShouldReturnValidResult()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> 
            { 
                new RecipeIngredient { Text = "1 cup flour" },
                new RecipeIngredient { Text = "2 eggs" },
                new RecipeIngredient { Text = "1 cup milk" }
            },
            Instructions = new List<string> { "Mix ingredients", "Bake for 30 minutes" },
            PrepTimeMinutes = 15,
            CookTimeMinutes = 30,
            Servings = 4
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Score.Should().BeGreaterThan(0.7);
        result.Issues.Should().BeEmpty();
        result.RecipeTitle.Should().Be("Test Recipe");
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithMissingTitle_ShouldReturnInvalidResult()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "1 cup flour" } },
            Instructions = new List<string> { "Mix ingredients" }
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Issues.Should().Contain(i => i.Type == "MissingTitle" && i.Severity == ValidationSeverity.Critical);
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithMissingIngredients_ShouldReturnInvalidResult()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient>(),
            Instructions = new List<string> { "Mix ingredients" }
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Issues.Should().Contain(i => i.Type == "MissingIngredients" && i.Severity == ValidationSeverity.Critical);
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithMissingInstructions_ShouldReturnInvalidResult()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "1 cup flour" } },
            Instructions = new List<string>()
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Issues.Should().Contain(i => i.Type == "MissingInstructions" && i.Severity == ValidationSeverity.Critical);
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithInvalidServings_ShouldReturnInvalidResult()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "1 cup flour" } },
            Instructions = new List<string> { "Mix ingredients" },
            Servings = -1
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Issues.Should().Contain(i => i.Type == "InvalidServings" && i.Severity == ValidationSeverity.Major);
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithWarnings_ShouldReturnValidResultWithWarnings()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "flour" } }, // Very short ingredient
            Instructions = new List<string> { "mix" }, // Very short instruction
            Servings = 100 // Unusually high servings
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue(); // Should still be valid despite warnings
        result.Warnings.Should().NotBeEmpty();
        result.Warnings.Should().Contain(w => w.Type == "IngredientTooShort");
        result.Warnings.Should().Contain(w => w.Type == "InstructionTooShort");
        result.Warnings.Should().Contain(w => w.Type == "HighServings");
    }

    [Fact]
    public async Task ValidateRecipesAsync_WithMultipleRecipes_ShouldReturnMultipleResults()
    {
        // Arrange
        var recipes = new List<ParsedRecipe>
        {
            new ParsedRecipe
            {
                Title = "Valid Recipe",
                Ingredients = new List<RecipeIngredient> 
                { 
                    new RecipeIngredient { Text = "1 cup flour" },
                    new RecipeIngredient { Text = "2 eggs" }
                },
                Instructions = new List<string> { "Mix ingredients", "Bake" }
            },
            new ParsedRecipe
            {
                Title = "",
                Ingredients = new List<RecipeIngredient>(),
                Instructions = new List<string>()
            }
        };

        // Act
        var results = await _service.ValidateRecipesAsync(recipes);

        // Assert
        results.Should().HaveCount(2);
        results.First().IsValid.Should().BeTrue();
        results.Last().IsValid.Should().BeFalse();
    }

    [Fact]
    public void GetValidationRules_ShouldReturnAllRules()
    {
        // Act
        var rules = _service.GetValidationRules();

        // Assert
        rules.Should().NotBeEmpty();
        rules.Should().ContainKey("MissingTitle");
        rules.Should().ContainKey("MissingIngredients");
        rules.Should().ContainKey("MissingInstructions");
        rules.Should().ContainKey("InvalidServings");
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithLongPrepTime_ShouldAddWarning()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "1 cup flour" } },
            Instructions = new List<string> { "Mix ingredients" },
            PrepTimeMinutes = 500 // Over 8 hours
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.Warnings.Should().Contain(w => w.Type == "LongPrepTime");
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithLongCookTime_ShouldAddWarning()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "1 cup flour" } },
            Instructions = new List<string> { "Mix ingredients" },
            CookTimeMinutes = 1500 // Over 24 hours
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.Warnings.Should().Contain(w => w.Type == "LongCookTime");
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithMissingTiming_ShouldAddWarning()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "1 cup flour" } },
            Instructions = new List<string> { "Mix ingredients" }
            // No timing information
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.Warnings.Should().Contain(w => w.Type == "MissingTiming");
    }

    [Fact]
    public async Task ValidateRecipeAsync_WithMissingServings_ShouldAddWarning()
    {
        // Arrange
        var recipe = new ParsedRecipe
        {
            Title = "Test Recipe",
            Ingredients = new List<RecipeIngredient> { new RecipeIngredient { Text = "1 cup flour" } },
            Instructions = new List<string> { "Mix ingredients" }
            // No servings information
        };

        // Act
        var result = await _service.ValidateRecipeAsync(recipe);

        // Assert
        result.Should().NotBeNull();
        result.Warnings.Should().Contain(w => w.Type == "MissingServings");
    }
}
