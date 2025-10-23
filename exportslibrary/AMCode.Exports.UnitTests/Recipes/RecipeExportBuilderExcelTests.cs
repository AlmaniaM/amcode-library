using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Exports.Recipes;
using AMCode.Exports.Recipes.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.Recipes
{
    [TestFixture]
    public class RecipeExportBuilderExcelTests
    {
        private Mock<ILogger<RecipeExportBuilder>> _mockLogger;
        private RecipeExportBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<RecipeExportBuilder>>();
            _builder = new RecipeExportBuilder(_mockLogger.Object);
        }

        [Test]
        public async Task ExportRecipesAsync_WithExcelFormat_ShouldReturnSuccessResult()
        {
            // Given
            var recipes = CreateSampleRecipes();
            var options = new RecipeExportOptions 
            { 
                Format = "excel", 
                Title = "Test Recipe Export" 
            };

            // When
            var result = await _builder.ExportRecipesAsync(recipes, options);

            // Then
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Length >= 0); // Basic validation that we get a stream
        }

        [Test]
        public async Task ExportRecipesAsync_WithCustomColumnsAndExcelFormat_ShouldReturnSuccessResult()
        {
            // Given
            var recipes = CreateSampleRecipes();
            var options = new RecipeExportOptions 
            { 
                Format = "excel", 
                Title = "Test Recipe Export" 
            };
            var columnConfig = new RecipeColumnConfiguration
            {
                IncludeTitle = true,
                IncludeCategory = true,
                IncludeTiming = true,
                IncludeServings = true,
                IncludeIngredients = true,
                IncludeInstructions = true,
                IncludeTags = false,
                IncludeMetadata = false
            };

            // When
            var result = await _builder.ExportRecipesAsync(recipes, options, columnConfig);

            // Then
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Length >= 0); // Basic validation that we get a stream
        }

        [Test]
        public async Task ExportShoppingListAsync_WithExcelFormat_ShouldReturnSuccessResult()
        {
            // Given
            var recipes = CreateSampleRecipes();
            var options = new RecipeExportOptions 
            { 
                Format = "excel", 
                Title = "Shopping List" 
            };

            // When
            var result = await _builder.ExportShoppingListAsync(recipes, options);

            // Then
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Length >= 0); // Basic validation that we get a stream
        }

        [Test]
        public async Task ExportRecipesAsync_WithUnsupportedFormat_ShouldReturnFailureResult()
        {
            // Given
            var recipes = CreateSampleRecipes();
            var options = new RecipeExportOptions 
            { 
                Format = "pdf", 
                Title = "Test Export" 
            };

            // When
            var result = await _builder.ExportRecipesAsync(recipes, options);

            // Then
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Error.Contains("Unsupported format"));
        }

        private List<Recipe> CreateSampleRecipes()
        {
            return new List<Recipe>
            {
                new Recipe
                {
                    Title = "Test Recipe 1",
                    Category = "Main Course",
                    PrepTimeMinutes = 15,
                    CookTimeMinutes = 30,
                    Servings = 4,
                    Difficulty = "Easy",
                    Cuisine = "Italian",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Pasta", Amount = "500g", Unit = "grams", IsOptional = false, Notes = "" },
                        new Ingredient { Name = "Tomato Sauce", Amount = "400ml", Unit = "ml", IsOptional = false, Notes = "" }
                    },
                    Instructions = "1. Boil pasta\n2. Heat sauce\n3. Combine",
                    Tags = new List<string> { "pasta", "italian" },
                    CreatedAt = DateTime.Now
                },
                new Recipe
                {
                    Title = "Test Recipe 2",
                    Category = "Dessert",
                    PrepTimeMinutes = 10,
                    CookTimeMinutes = 20,
                    Servings = 6,
                    Difficulty = "Medium",
                    Cuisine = "American",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Flour", Amount = "2 cups", Unit = "cups", IsOptional = false, Notes = "" },
                        new Ingredient { Name = "Sugar", Amount = "1 cup", Unit = "cups", IsOptional = false, Notes = "" }
                    },
                    Instructions = "1. Mix ingredients\n2. Bake at 350°F\n3. Cool",
                    Tags = new List<string> { "dessert", "baking" },
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            };
        }
    }
}
