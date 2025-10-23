namespace AMCode.Exports.Recipes.Models
{
    /// <summary>
    /// Configuration for recipe export columns
    /// </summary>
    public class RecipeColumnConfiguration
    {
        public bool IncludeTitle { get; set; } = true;
        public bool IncludeCategory { get; set; } = true;
        public bool IncludeTiming { get; set; } = true;
        public bool IncludeServings { get; set; } = true;
        public bool IncludeIngredients { get; set; } = true;
        public bool IncludeInstructions { get; set; } = true;
        public bool IncludeTags { get; set; } = true;
        public bool IncludeMetadata { get; set; } = true;
        public bool IncludeDifficulty { get; set; } = false;
        public bool IncludeCuisine { get; set; } = false;
        public bool IncludeRating { get; set; } = false;
        public bool IncludeNotes { get; set; } = false;
    }
}
