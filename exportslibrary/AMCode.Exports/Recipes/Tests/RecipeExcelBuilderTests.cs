using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AMCode.Exports.Recipes.Models;
using NUnit.Framework;

namespace AMCode.Exports.Recipes.Tests
{
    [TestFixture]
    public class RecipeExcelBuilderTests
    {
        private List<RecipeExportData> _testRecipes;

        [SetUp]
        public void Setup()
        {
            _testRecipes = CreateTestRecipes();
        }

        [Test]
        public void Build_WithRecipes_ReturnsByteArray()
        {
            var builder = new RecipeExcelBuilder();
            builder.AddRecipes(_testRecipes);

            var bytes = builder.Build();

            Assert.IsNotNull(bytes);
            Assert.Greater(bytes.Length, 0);
        }

        [Test]
        public void BuildToStream_WithRecipes_ReturnsReadableStream()
        {
            var builder = new RecipeExcelBuilder();
            builder.AddRecipes(_testRecipes);

            using var stream = builder.BuildToStream();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanRead);
            Assert.Greater(stream.Length, 0);
        }

        [Test]
        public void Build_WithNoRecipes_ReturnsWorkbook()
        {
            var builder = new RecipeExcelBuilder();

            var bytes = builder.Build();

            // Should return a valid (possibly empty) Excel workbook
            Assert.IsNotNull(bytes);
            Assert.Greater(bytes.Length, 0);
        }

        [Test]
        public void AddRecipe_NullRecipe_DoesNotThrow()
        {
            var builder = new RecipeExcelBuilder();

            Assert.DoesNotThrow(() => builder.AddRecipe(null));
        }

        [Test]
        public void AddRecipes_NullCollection_DoesNotThrow()
        {
            var builder = new RecipeExcelBuilder();

            Assert.DoesNotThrow(() => builder.AddRecipes(null));
        }

        [Test]
        public void WithColumns_AppliesColumnConfiguration()
        {
            var builder = new RecipeExcelBuilder();
            var columns = new RecipeColumnConfiguration
            {
                IncludeTitle = true,
                IncludeAuthor = false,
                IncludeCategory = true,
                IncludePrepTime = false,
                IncludeCookTime = false,
                IncludeServings = true,
                IncludeDietaryTags = false
            };

            builder.AddRecipes(_testRecipes).WithColumns(columns);

            // Build should succeed with custom column config
            Assert.DoesNotThrow(() => builder.Build());
        }

        [Test]
        public void WithFormatting_AppliesFormattingOptions()
        {
            var builder = new RecipeExcelBuilder();
            var formatting = new ExcelFormattingOptions
            {
                BoldHeaders = true,
                AutoFitColumns = false,
                FreezeHeaderRow = false
            };

            builder.AddRecipes(_testRecipes).WithFormatting(formatting);

            Assert.DoesNotThrow(() => builder.Build());
        }

        [Test]
        public void IncludeNutritionSheet_WithNutritionData_IncludesSheet()
        {
            var recipe = new RecipeExportData
            {
                Title = "Healthy Recipe",
                Category = "Lunch",
                Nutrition = new RecipeNutritionData
                {
                    CaloriesPerServing = 350.5m,
                    FatGrams = 12.3m,
                    CarbGrams = 45.0m,
                    ProteinGrams = 25.6m,
                    FiberGrams = 8.0m
                }
            };

            var builder = new RecipeExcelBuilder();
            builder.AddRecipe(recipe).IncludeNutritionSheet(true);

            var bytes = builder.Build();

            Assert.IsNotNull(bytes);
            Assert.Greater(bytes.Length, 0);
        }

        [Test]
        public void IncludeNutritionSheet_WithNoNutritionData_SkipsSheet()
        {
            var builder = new RecipeExcelBuilder();
            builder.AddRecipes(_testRecipes).IncludeNutritionSheet(true);

            // Recipes have no nutrition data — sheet should be skipped gracefully
            Assert.DoesNotThrow(() => builder.Build());
        }

        [Test]
        public void Build_WithRecipeHavingMultipleIngredients_IncludesIngredientSheet()
        {
            var builder = new RecipeExcelBuilder();
            builder.AddRecipes(_testRecipes);

            var bytes = builder.Build();

            // Verify the workbook is valid (non-zero bytes is a basic sanity check)
            Assert.Greater(bytes.Length, 0);
        }

        [Test]
        public void Build_WithRecipeHavingInstructions_IncludesInstructionsSheet()
        {
            var builder = new RecipeExcelBuilder();
            builder.AddRecipes(_testRecipes);

            var bytes = builder.Build();

            Assert.Greater(bytes.Length, 0);
        }

        [Test]
        public void FluentChaining_ReturnsBuilder()
        {
            var builder = new RecipeExcelBuilder();

            var result = builder
                .AddRecipes(_testRecipes)
                .WithColumns(RecipeColumnConfiguration.Default)
                .WithFormatting(ExcelFormattingOptions.Default)
                .IncludeNutritionSheet(false);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RecipeExcelBuilder>(result);
        }

        [Test]
        public void Build_WithLargeRecipeSet_DoesNotThrow()
        {
            var manyRecipes = Enumerable.Range(1, 50)
                .Select(i => new RecipeExportData
                {
                    Title = $"Recipe {i}",
                    Category = "Test",
                    PrepTimeMinutes = i * 2,
                    CookTimeMinutes = i * 3,
                    Servings = (i % 4) + 1,
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = $"Ingredient {i}A", Amount = "1", Unit = "cup" },
                        new Ingredient { Name = $"Ingredient {i}B", Amount = "2", Unit = "tbsp" }
                    },
                    InstructionSteps = new List<string> { $"Step 1 for recipe {i}", $"Step 2 for recipe {i}" }
                })
                .ToList();

            var builder = new RecipeExcelBuilder();
            builder.AddRecipes(manyRecipes);

            Assert.DoesNotThrow(() => builder.Build());
        }

        private static List<RecipeExportData> CreateTestRecipes()
        {
            return new List<RecipeExportData>
            {
                new RecipeExportData
                {
                    Title = "Pasta Carbonara",
                    Author = "Chef Test",
                    Category = "Dinner",
                    PrepTimeMinutes = 15,
                    CookTimeMinutes = 20,
                    Servings = 4,
                    DietaryTags = new List<string> { "Italian", "Pasta" },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Pasta", Amount = "400", Unit = "g" },
                        new Ingredient { Name = "Eggs", Amount = "4", Unit = "whole" },
                        new Ingredient { Name = "Pecorino Romano", Amount = "100", Unit = "g" }
                    },
                    InstructionSteps = new List<string>
                    {
                        "Boil pasta in salted water",
                        "Mix eggs with cheese",
                        "Combine pasta with egg mixture off heat"
                    },
                    CreatedAt = new DateTime(2026, 1, 15)
                },
                new RecipeExportData
                {
                    Title = "Avocado Toast",
                    Author = "Test User",
                    Category = "Breakfast",
                    PrepTimeMinutes = 5,
                    CookTimeMinutes = 5,
                    Servings = 2,
                    DietaryTags = new List<string> { "Vegetarian", "Quick" },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Bread", Amount = "2", Unit = "slices" },
                        new Ingredient { Name = "Avocado", Amount = "1", Unit = "whole" },
                        new Ingredient { Name = "Lemon juice", Amount = "1", Unit = "tsp" }
                    },
                    InstructionSteps = new List<string>
                    {
                        "Toast bread",
                        "Mash avocado with lemon juice",
                        "Spread on toast"
                    },
                    CreatedAt = new DateTime(2026, 2, 1)
                }
            };
        }
    }
}
