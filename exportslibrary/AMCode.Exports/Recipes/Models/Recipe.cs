using System;
using System.Collections.Generic;

namespace AMCode.Exports.Recipes.Models
{
    /// <summary>
    /// Represents a recipe with all its properties
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// The title of the recipe
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The category of the recipe (e.g., "Dinner", "Dessert", "Appetizer")
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// Preparation time in minutes
        /// </summary>
        public int PrepTimeMinutes { get; set; }

        /// <summary>
        /// Cooking time in minutes
        /// </summary>
        public int CookTimeMinutes { get; set; }

        /// <summary>
        /// Number of servings
        /// </summary>
        public int Servings { get; set; }

        /// <summary>
        /// Difficulty level (e.g., "Easy", "Medium", "Hard")
        /// </summary>
        public string Difficulty { get; set; } = string.Empty;

        /// <summary>
        /// Cuisine type (e.g., "Italian", "Mexican", "Asian")
        /// </summary>
        public string Cuisine { get; set; } = string.Empty;

        /// <summary>
        /// List of ingredients
        /// </summary>
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        /// <summary>
        /// Cooking instructions
        /// </summary>
        public string Instructions { get; set; } = string.Empty;

        /// <summary>
        /// Tags associated with the recipe
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// When the recipe was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// When the recipe was last modified
        /// </summary>
        public DateTime ModifiedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Additional notes about the recipe
        /// </summary>
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents an ingredient in a recipe
    /// </summary>
    public class Ingredient
    {
        /// <summary>
        /// The name of the ingredient
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The amount of the ingredient
        /// </summary>
        public string Amount { get; set; } = string.Empty;

        /// <summary>
        /// The unit of measurement
        /// </summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>
        /// Additional notes about the ingredient
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Whether this ingredient is optional
        /// </summary>
        public bool IsOptional { get; set; } = false;
    }
}
