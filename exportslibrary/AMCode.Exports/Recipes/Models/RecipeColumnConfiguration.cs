using System.Collections.Generic;

namespace AMCode.Exports.Recipes.Models
{
    /// <summary>
    /// Configuration for recipe export columns — controls which fields are included in the export.
    /// </summary>
    public class RecipeColumnConfiguration
    {
        public bool IncludeTitle { get; set; } = true;
        public bool IncludeAuthor { get; set; } = true;
        public bool IncludeCategory { get; set; } = true;
        public bool IncludePrepTime { get; set; } = true;
        public bool IncludeCookTime { get; set; } = true;
        public bool IncludeServings { get; set; } = true;
        public bool IncludeDietaryTags { get; set; } = true;
        public bool IncludeIngredients { get; set; } = true;
        public bool IncludeInstructions { get; set; } = true;
        public bool IncludeNutrition { get; set; } = false;
        public bool IncludeCreatedDate { get; set; } = false;
        public bool IncludeSource { get; set; } = false;

        // Legacy aliases for backwards compatibility
        public bool IncludeTiming
        {
            get => IncludePrepTime && IncludeCookTime;
            set { IncludePrepTime = value; IncludeCookTime = value; }
        }

        public bool IncludeTags
        {
            get => IncludeDietaryTags;
            set => IncludeDietaryTags = value;
        }

        public bool IncludeMetadata
        {
            get => IncludeCreatedDate;
            set => IncludeCreatedDate = value;
        }

        public bool IncludeDifficulty { get; set; } = false;
        public bool IncludeCuisine { get; set; } = false;
        public bool IncludeRating { get; set; } = false;
        public bool IncludeNotes { get; set; } = false;

        /// <summary>
        /// Optional custom ordering of column display names.
        /// </summary>
        public List<string> CustomColumnOrder { get; set; } = new List<string>();

        /// <summary>
        /// Default configuration — all main fields included.
        /// </summary>
        public static RecipeColumnConfiguration Default => new RecipeColumnConfiguration();

        /// <summary>
        /// Minimal configuration — excludes author, created date, and source.
        /// </summary>
        public static RecipeColumnConfiguration Minimal => new RecipeColumnConfiguration
        {
            IncludeAuthor = false,
            IncludeCreatedDate = false,
            IncludeSource = false
        };
    }
}
