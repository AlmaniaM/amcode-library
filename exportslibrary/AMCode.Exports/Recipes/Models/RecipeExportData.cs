using System;
using System.Collections.Generic;

namespace AMCode.Exports.Recipes.Models
{
    /// <summary>
    /// Data transfer object for recipe exports — contains all data fields that may be included in an export.
    /// </summary>
    public class RecipeExportData
    {
        /// <summary>
        /// Recipe title/name.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Author or creator of the recipe.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Recipe category (e.g., Breakfast, Lunch, Dinner, Dessert).
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Preparation time in minutes.
        /// </summary>
        public int PrepTimeMinutes { get; set; }

        /// <summary>
        /// Cooking time in minutes.
        /// </summary>
        public int CookTimeMinutes { get; set; }

        /// <summary>
        /// Number of servings.
        /// </summary>
        public int Servings { get; set; }

        /// <summary>
        /// Difficulty level (e.g., Easy, Medium, Hard).
        /// </summary>
        public string Difficulty { get; set; } = string.Empty;

        /// <summary>
        /// Cuisine type (e.g., Italian, Mexican, Asian).
        /// </summary>
        public string Cuisine { get; set; } = string.Empty;

        /// <summary>
        /// Dietary tags (e.g., Vegetarian, Gluten-Free, Vegan).
        /// </summary>
        public List<string> DietaryTags { get; set; } = new List<string>();

        /// <summary>
        /// List of ingredients.
        /// </summary>
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        /// <summary>
        /// Cooking instructions as ordered steps.
        /// </summary>
        public List<string> InstructionSteps { get; set; } = new List<string>();

        /// <summary>
        /// Nutritional information (optional).
        /// </summary>
        public RecipeNutritionData Nutrition { get; set; }

        /// <summary>
        /// Source of the recipe (e.g., Manual, Imported, AI-Generated).
        /// </summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// When the recipe was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Additional notes about the recipe.
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Creates a <see cref="RecipeExportData"/> from an existing <see cref="Recipe"/> model.
        /// </summary>
        public static RecipeExportData FromRecipe(Recipe recipe)
        {
            if (recipe == null) return null;

            var data = new RecipeExportData
            {
                Title = recipe.Title,
                Category = recipe.Category,
                PrepTimeMinutes = recipe.PrepTimeMinutes,
                CookTimeMinutes = recipe.CookTimeMinutes,
                Servings = recipe.Servings,
                Difficulty = recipe.Difficulty,
                Cuisine = recipe.Cuisine,
                DietaryTags = recipe.Tags ?? new List<string>(),
                Ingredients = recipe.Ingredients ?? new List<Ingredient>(),
                Notes = recipe.Notes,
                CreatedAt = recipe.CreatedAt,
            };

            // Split instructions string into steps if needed
            if (!string.IsNullOrEmpty(recipe.Instructions))
            {
                data.InstructionSteps = new List<string>(
                    recipe.Instructions.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                );
            }

            return data;
        }
    }

    /// <summary>
    /// Nutritional information for a recipe.
    /// </summary>
    public class RecipeNutritionData
    {
        public decimal? CaloriesPerServing { get; set; }
        public decimal? FatGrams { get; set; }
        public decimal? CarbGrams { get; set; }
        public decimal? ProteinGrams { get; set; }
        public decimal? FiberGrams { get; set; }
        public decimal? SodiumMg { get; set; }
    }
}
