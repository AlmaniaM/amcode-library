using AMCode.AI.Pipelines.RecipeGeneration;
using FluentAssertions;
using Xunit;

namespace AMCode.AI.Tests.Pipelines.RecipeGeneration;

public class RecipeSafetyValidatorTests
{
    #region Allergen Detection Tests

    [Fact]
    public void ValidateAndEnrich_WithPeanutAllergy_ShouldFlagPeanutIngredients()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Pad Thai",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "peanuts", Text = "1/4 cup crushed peanuts" },
                new() { Name = "rice noodles", Text = "8 oz rice noodles" },
            },
            Directions = new List<string> { "Cook noodles" },
        });
        var input = CreateInput(allergies: "peanuts");

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].AllergenWarnings.Should().NotBeEmpty();
        output.Recipes[0].AllergenWarnings.Should().Contain(w => w.Contains("peanut", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithDairyAllergy_ShouldFlagHiddenDairySources()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Pasta",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "parmesan cheese", Text = "1/2 cup grated parmesan cheese" },
                new() { Name = "butter", Text = "2 tbsp butter" },
                new() { Name = "pasta", Text = "1 lb spaghetti" },
            },
            Directions = new List<string> { "Cook pasta" },
        });
        var input = CreateInput(allergies: "dairy");

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].AllergenWarnings.Should().HaveCountGreaterThanOrEqualTo(2);
        output.Recipes[0].AllergenWarnings.Should().Contain(w => w.Contains("parmesan", StringComparison.OrdinalIgnoreCase));
        output.Recipes[0].AllergenWarnings.Should().Contain(w => w.Contains("butter", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithGlutenAllergy_ShouldFlagSoySauce()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Stir Fry",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "soy sauce", Text = "2 tbsp soy sauce" },
                new() { Name = "chicken", Text = "1 lb chicken breast" },
            },
            Directions = new List<string> { "Stir fry" },
        });
        var input = CreateInput(allergies: "gluten");

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].AllergenWarnings.Should().Contain(w => w.Contains("soy sauce", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithMultipleAllergies_ShouldFlagAll()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Mixed Plate",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "shrimp", Text = "1 lb shrimp" },
                new() { Name = "eggs", Text = "2 eggs" },
                new() { Name = "rice", Text = "2 cups rice" },
            },
            Directions = new List<string> { "Cook everything" },
        });
        var input = CreateInput(allergies: "shellfish, egg");

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].AllergenWarnings.Should().Contain(w => w.Contains("shrimp", StringComparison.OrdinalIgnoreCase));
        output.Recipes[0].AllergenWarnings.Should().Contain(w => w.Contains("egg", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithNoAllergies_ShouldNotAddWarnings()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Simple Salad",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "lettuce", Text = "1 head lettuce" },
                new() { Name = "tomato", Text = "2 tomatoes" },
            },
            Directions = new List<string> { "Toss salad" },
        });
        var input = CreateInput(allergies: null);

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].AllergenWarnings.Should().BeEmpty();
    }

    #endregion

    #region Cooking Temperature Tests

    [Fact]
    public void ValidateAndEnrich_WithChickenIngredient_ShouldAddTemperatureWarning()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Roast Chicken",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "chicken breast", Text = "2 lbs chicken breast" },
            },
            Directions = new List<string> { "Roast in oven at 375F for 45 minutes" },
        });
        var input = CreateInput();

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].SafetyNotes.Should().Contain(n => n.Contains("165°F", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithPorkIngredient_ShouldAddTemperatureWarning()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Pork Chops",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "pork chops", Text = "4 pork chops" },
            },
            Directions = new List<string> { "Grill pork chops" },
        });
        var input = CreateInput();

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].SafetyNotes.Should().Contain(n => n.Contains("145°F", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithGroundBeef_ShouldAddHigherTemperatureWarning()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Hamburgers",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "ground beef", Text = "1 lb ground beef" },
            },
            Directions = new List<string> { "Form patties and grill" },
        });
        var input = CreateInput();

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].SafetyNotes.Should().Contain(n => n.Contains("160°F", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithVegetarianRecipe_ShouldNotAddMeatTemperatureWarnings()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Vegetable Stir Fry",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "broccoli", Text = "2 cups broccoli" },
                new() { Name = "carrots", Text = "1 cup carrots" },
                new() { Name = "tofu", Text = "1 block tofu" },
            },
            Directions = new List<string> { "Stir fry vegetables" },
        });
        var input = CreateInput();

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].SafetyNotes.Should().NotContain(n => n.Contains("165°F"));
        output.Recipes[0].SafetyNotes.Should().NotContain(n => n.Contains("160°F"));
    }

    #endregion

    #region General Safety Notes Tests

    [Fact]
    public void ValidateAndEnrich_WithFreshProduce_ShouldAddWashReminder()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Fresh Salad",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "lettuce", Text = "1 head lettuce" },
                new() { Name = "spinach", Text = "2 cups spinach" },
            },
            Directions = new List<string> { "Toss salad" },
            Servings = 2,
            CookTimeMinutes = 5,
        });
        var input = CreateInput();

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].SafetyNotes.Should().Contain(n => n.Contains("Wash", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidateAndEnrich_WithLargeServings_ShouldAddLeftoverReminder()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Big Batch Stew",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "beef", Text = "3 lbs beef" },
            },
            Directions = new List<string> { "Slow cook for hours" },
            Servings = 8,
            CookTimeMinutes = 120,
        });
        var input = CreateInput();

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert
        output.Recipes[0].SafetyNotes.Should().Contain(n => n.Contains("Refrigerate leftovers", StringComparison.OrdinalIgnoreCase));
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ValidateAndEnrich_WithNullOutput_ShouldNotThrow()
    {
        // Arrange
        var input = CreateInput();

        // Act & Assert
        var act = () => RecipeSafetyValidator.ValidateAndEnrich(null!, input);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateAndEnrich_WithEmptyRecipeList_ShouldNotThrow()
    {
        // Arrange
        var output = new RecipeGenerationOutput { Recipes = new List<GeneratedRecipeItem>() };
        var input = CreateInput();

        // Act & Assert
        var act = () => RecipeSafetyValidator.ValidateAndEnrich(output, input);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateAndEnrich_ShouldNotDuplicateWarnings()
    {
        // Arrange
        var output = CreateOutput(new GeneratedRecipeItem
        {
            Title = "Chicken Parm",
            Ingredients = new List<GeneratedIngredient>
            {
                new() { Name = "chicken breast", Text = "2 chicken breasts" },
                new() { Name = "chicken thigh", Text = "2 chicken thighs" },
            },
            Directions = new List<string> { "Cook chicken" },
        });
        var input = CreateInput();

        // Act
        RecipeSafetyValidator.ValidateAndEnrich(output, input);

        // Assert — should only have one chicken temperature warning
        var chickenNotes = output.Recipes[0].SafetyNotes.Count(n => n.Contains("chicken", StringComparison.OrdinalIgnoreCase));
        chickenNotes.Should().Be(1);
    }

    #endregion

    #region Helpers

    private static RecipeGenerationInput CreateInput(string? allergies = null)
    {
        return new RecipeGenerationInput
        {
            Ingredients = new List<string> { "chicken" },
            Servings = 4,
            Allergies = allergies,
        };
    }

    private static RecipeGenerationOutput CreateOutput(GeneratedRecipeItem recipe)
    {
        return new RecipeGenerationOutput
        {
            Recipes = new List<GeneratedRecipeItem> { recipe },
        };
    }

    #endregion
}
