using AMCode.AI.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AMCode.AI.Tests.Models;

[TestClass]
public class ParsedRecipeTests
{
    [TestMethod]
    public void ParsedRecipe_ShouldInitializeWithDefaultValues()
    {
        // Act
        var recipe = new ParsedRecipe();
        
        // Assert
        recipe.Title.Should().BeEmpty();
        recipe.Description.Should().BeEmpty();
        recipe.Ingredients.Should().NotBeNull().And.BeEmpty();
        recipe.Instructions.Should().NotBeNull().And.BeEmpty();
        recipe.PrepTimeMinutes.Should().BeNull();
        recipe.CookTimeMinutes.Should().BeNull();
        recipe.TotalTimeMinutes.Should().BeNull();
        recipe.Servings.Should().BeNull();
        recipe.Category.Should().BeEmpty();
        recipe.Tags.Should().NotBeNull().And.BeEmpty();
        recipe.Difficulty.Should().BeNull();
        recipe.Confidence.Should().Be(0.0);
        recipe.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        recipe.Notes.Should().BeEmpty();
    }
    
    [TestMethod]
    public void ParsedRecipe_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var recipe = new ParsedRecipe();
        var now = DateTime.UtcNow;
        
        // Act
        recipe.Title = "Test Recipe";
        recipe.Description = "A test recipe";
        recipe.PrepTimeMinutes = 15;
        recipe.CookTimeMinutes = 30;
        recipe.TotalTimeMinutes = 45;
        recipe.Servings = 4;
        recipe.Category = "Main Course";
        recipe.Tags = new List<string> { "test", "example" };
        recipe.Difficulty = 3;
        recipe.Confidence = 0.95;
        recipe.CreatedAt = now;
        recipe.Notes = "Test notes";
        
        // Assert
        recipe.Title.Should().Be("Test Recipe");
        recipe.Description.Should().Be("A test recipe");
        recipe.PrepTimeMinutes.Should().Be(15);
        recipe.CookTimeMinutes.Should().Be(30);
        recipe.TotalTimeMinutes.Should().Be(45);
        recipe.Servings.Should().Be(4);
        recipe.Category.Should().Be("Main Course");
        recipe.Tags.Should().Contain("test").And.Contain("example");
        recipe.Difficulty.Should().Be(3);
        recipe.Confidence.Should().Be(0.95);
        recipe.CreatedAt.Should().Be(now);
        recipe.Notes.Should().Be("Test notes");
    }
    
    [TestMethod]
    public void RecipeIngredient_ShouldInitializeWithDefaultValues()
    {
        // Act
        var ingredient = new RecipeIngredient();
        
        // Assert
        ingredient.Name.Should().BeEmpty();
        ingredient.Amount.Should().BeEmpty();
        ingredient.Unit.Should().BeEmpty();
        ingredient.Text.Should().BeEmpty();
        ingredient.Notes.Should().BeEmpty();
    }
    
    [TestMethod]
    public void RecipeIngredient_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var ingredient = new RecipeIngredient();
        
        // Act
        ingredient.Name = "Flour";
        ingredient.Amount = "2";
        ingredient.Unit = "cups";
        ingredient.Text = "2 cups all-purpose flour";
        ingredient.Notes = "sifted";
        
        // Assert
        ingredient.Name.Should().Be("Flour");
        ingredient.Amount.Should().Be("2");
        ingredient.Unit.Should().Be("cups");
        ingredient.Text.Should().Be("2 cups all-purpose flour");
        ingredient.Notes.Should().Be("sifted");
    }
}
