using AMCode.AI.Pipelines.RecipeGeneration;
using FluentAssertions;
using Xunit;

namespace AMCode.AI.Tests.Pipelines.RecipeGeneration;

public class RecipeGenerationPromptBuilderTests
{
    [Fact]
    public void BuildSystemPrompt_ShouldContainSafetyCriticalRules()
    {
        // Act
        var prompt = RecipeGenerationPromptBuilder.BuildSystemPrompt();

        // Assert
        prompt.Should().Contain("165°F");
        prompt.Should().Contain("NEVER");
        prompt.Should().Contain("allergen");
        prompt.Should().Contain("SAFE for human consumption");
    }

    [Fact]
    public void BuildUserPrompt_WithAllFilters_ShouldIncludeAllSections()
    {
        // Arrange
        var input = new RecipeGenerationInput
        {
            Ingredients = new List<string> { "chicken", "rice", "tomatoes" },
            Cuisine = "Italian",
            MealTypes = new List<string> { "Dinner" },
            MaxBudgetPerServing = 15m,
            DietaryPreferences = new List<string> { "Gluten-Free" },
            Allergies = "peanuts, shellfish",
            CookingTimeRange = "30-60m",
            Difficulty = "Medium",
            Servings = 4,
            AdditionalNotes = "Low sodium preferred",
            RecipeCount = 3,
        };

        // Act
        var prompt = RecipeGenerationPromptBuilder.BuildUserPrompt(input);

        // Assert
        prompt.Should().Contain("chicken, rice, tomatoes");
        prompt.Should().Contain("Italian");
        prompt.Should().Contain("Dinner");
        prompt.Should().Contain("$15");
        prompt.Should().Contain("Gluten-Free");
        prompt.Should().Contain("peanuts, shellfish");
        prompt.Should().Contain("30-60m");
        prompt.Should().Contain("Medium");
        prompt.Should().Contain("SERVINGS: 4");
        prompt.Should().Contain("Low sodium preferred");
        prompt.Should().Contain("3 recipe(s)");
    }

    [Fact]
    public void BuildUserPrompt_WithMinimalInput_ShouldStillBeValid()
    {
        // Arrange
        var input = new RecipeGenerationInput
        {
            Ingredients = new List<string> { "eggs" },
            Servings = 2,
            RecipeCount = 1,
        };

        // Act
        var prompt = RecipeGenerationPromptBuilder.BuildUserPrompt(input);

        // Assert
        prompt.Should().Contain("eggs");
        prompt.Should().Contain("SERVINGS: 2");
        prompt.Should().Contain("1 recipe(s)");
        prompt.Should().NotContain("CUISINE:");
        prompt.Should().NotContain("ALLERGIES");
    }

    [Fact]
    public void BuildUserPrompt_WithAllergies_ShouldEmphasizeCriticalWarning()
    {
        // Arrange
        var input = new RecipeGenerationInput
        {
            Ingredients = new List<string> { "chicken" },
            Allergies = "nuts",
            Servings = 4,
        };

        // Act
        var prompt = RecipeGenerationPromptBuilder.BuildUserPrompt(input);

        // Assert
        prompt.Should().Contain("CRITICAL");
        prompt.Should().Contain("DO NOT INCLUDE");
        prompt.Should().Contain("nuts");
    }

    [Fact]
    public void BuildUserPrompt_WithDietaryPreferences_ShouldMarkAsStrict()
    {
        // Arrange
        var input = new RecipeGenerationInput
        {
            Ingredients = new List<string> { "tofu" },
            DietaryPreferences = new List<string> { "Vegan", "Gluten-Free" },
            Servings = 4,
        };

        // Act
        var prompt = RecipeGenerationPromptBuilder.BuildUserPrompt(input);

        // Assert
        prompt.Should().Contain("STRICT");
        prompt.Should().Contain("Vegan, Gluten-Free");
    }

    [Fact]
    public void BuildUserPrompt_ShouldIncludeJsonSchema()
    {
        // Arrange
        var input = new RecipeGenerationInput
        {
            Ingredients = new List<string> { "chicken" },
            Servings = 4,
        };

        // Act
        var prompt = RecipeGenerationPromptBuilder.BuildUserPrompt(input);

        // Assert
        prompt.Should().Contain("\"recipes\"");
        prompt.Should().Contain("\"title\"");
        prompt.Should().Contain("\"ingredients\"");
        prompt.Should().Contain("\"directions\"");
        prompt.Should().Contain("\"allergenWarnings\"");
        prompt.Should().Contain("\"safetyNotes\"");
    }
}
