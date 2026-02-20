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
    public class RecipeCsvBuilderTests
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
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes);

            var bytes = builder.Build();

            Assert.IsNotNull(bytes);
            Assert.Greater(bytes.Length, 0);
        }

        [Test]
        public void Build_IncludesUtf8Bom()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes);

            var bytes = builder.Build();

            // UTF-8 BOM is EF BB BF
            Assert.GreaterOrEqual(bytes.Length, 3);
            Assert.AreEqual(0xEF, bytes[0]);
            Assert.AreEqual(0xBB, bytes[1]);
            Assert.AreEqual(0xBF, bytes[2]);
        }

        [Test]
        public void Build_WithDefaultDelimiter_UseComma()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');

            Assert.IsTrue(csv.Contains(','));
        }

        [Test]
        public void WithDelimiter_Tab_UsesTabDelimiter()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes).WithDelimiter('\t');

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');
            var headerLine = csv.Split('\n')[0];

            Assert.IsTrue(headerLine.Contains('\t'));
        }

        [Test]
        public void WithDelimiter_Semicolon_UsesSemicolon()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes).WithDelimiter(';');

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');
            var headerLine = csv.Split('\n')[0];

            Assert.IsTrue(headerLine.Contains(';'));
        }

        [Test]
        public void Build_WithCommaInFieldValue_EscapesCorrectly()
        {
            var recipe = new RecipeExportData
            {
                Title = "Recipe, with comma",
                Category = "Test"
            };

            var builder = new RecipeCsvBuilder();
            builder.AddRecipe(recipe);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');

            // The field with comma should be wrapped in double quotes
            Assert.IsTrue(csv.Contains("\"Recipe, with comma\""));
        }

        [Test]
        public void Build_WithQuoteInFieldValue_EscapesCorrectly()
        {
            var recipe = new RecipeExportData
            {
                Title = "Recipe with \"quotes\"",
                Category = "Test"
            };

            var builder = new RecipeCsvBuilder();
            builder.AddRecipe(recipe);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');

            // Quotes should be doubled inside quoted field
            Assert.IsTrue(csv.Contains("\"Recipe with \"\"quotes\"\"\""));
        }

        [Test]
        public void Build_WithNewlineInFieldValue_EscapesCorrectly()
        {
            var recipe = new RecipeExportData
            {
                Title = "Recipe\nwith newline",
                Category = "Test"
            };

            var builder = new RecipeCsvBuilder();
            builder.AddRecipe(recipe);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');

            // Field with newline should be wrapped in double quotes
            Assert.IsTrue(csv.Contains('"'));
        }

        [Test]
        public void Build_WithNoRecipes_ReturnsHeaderOnly()
        {
            var builder = new RecipeCsvBuilder();

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');
            var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // Should have exactly 1 line (header row)
            Assert.AreEqual(1, lines.Length);
        }

        [Test]
        public void Build_WithRecipes_IncludesHeaderRow()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');
            var headerLine = csv.Split('\n')[0];

            Assert.IsTrue(headerLine.Contains("Recipe Name"));
        }

        [Test]
        public void Build_FlattenedFormat_OneRowPerRecipe()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes); // Default: flattened

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');
            var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            // 1 header + 1 row per recipe
            Assert.AreEqual(_testRecipes.Count + 1, lines.Length);
        }

        [Test]
        public void Build_ExpandedFormat_OneRowPerIngredient()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes).WithExpandedIngredients(true);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');
            var lines = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            var totalIngredients = _testRecipes.Sum(r => r.Ingredients.Count);
            // 1 header + total ingredients
            Assert.AreEqual(totalIngredients + 1, lines.Length);
        }

        [Test]
        public void WithColumns_MinimalConfig_ExcludesAuthorAndDate()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes).WithColumns(RecipeColumnConfiguration.Minimal);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');
            var headerLine = csv.Split('\n')[0];

            Assert.IsFalse(headerLine.Contains("Author"));
            Assert.IsFalse(headerLine.Contains("Created Date"));
        }

        [Test]
        public void AddRecipe_Null_DoesNotThrow()
        {
            var builder = new RecipeCsvBuilder();

            Assert.DoesNotThrow(() => builder.AddRecipe(null));
        }

        [Test]
        public void AddRecipes_Null_DoesNotThrow()
        {
            var builder = new RecipeCsvBuilder();

            Assert.DoesNotThrow(() => builder.AddRecipes(null));
        }

        [Test]
        public void BuildToStream_ReturnsReadableStream()
        {
            var builder = new RecipeCsvBuilder();
            builder.AddRecipes(_testRecipes);

            using var stream = builder.BuildToStream();

            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.CanRead);
            Assert.Greater(stream.Length, 0);
        }

        [Test]
        public void Build_IngredientsFlattened_JoinedWithSemicolon()
        {
            var recipe = new RecipeExportData
            {
                Title = "Test Recipe",
                Category = "Lunch",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Flour", Amount = "2", Unit = "cups" },
                    new Ingredient { Name = "Water", Amount = "1", Unit = "cup" }
                }
            };

            var builder = new RecipeCsvBuilder();
            builder.AddRecipe(recipe);

            var bytes = builder.Build();
            var csv = Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF');

            // Flattened ingredients should be joined with "; "
            Assert.IsTrue(csv.Contains(';'));
        }

        [Test]
        public void FluentChaining_ReturnsBuilder()
        {
            var builder = new RecipeCsvBuilder();

            var result = builder
                .AddRecipes(_testRecipes)
                .WithColumns(RecipeColumnConfiguration.Default)
                .WithDelimiter(',')
                .WithExpandedIngredients(false);

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RecipeCsvBuilder>(result);
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
                    DietaryTags = new List<string> { "Italian" },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Pasta", Amount = "400", Unit = "g" },
                        new Ingredient { Name = "Eggs", Amount = "4", Unit = "whole" }
                    },
                    InstructionSteps = new List<string> { "Boil pasta", "Mix eggs with cheese" },
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
                    DietaryTags = new List<string> { "Vegetarian" },
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = "Bread", Amount = "2", Unit = "slices" },
                        new Ingredient { Name = "Avocado", Amount = "1", Unit = "whole" }
                    },
                    InstructionSteps = new List<string> { "Toast bread", "Mash avocado" },
                    CreatedAt = new DateTime(2026, 2, 1)
                }
            };
        }
    }
}
