namespace AMCode.Exports.Recipes.Models
{
    /// <summary>
    /// Options for recipe export operations
    /// </summary>
    public class RecipeExportOptions
    {
        public string Format { get; set; } = "excel";
        public string Title { get; set; } = "Recipe Export";
        public bool IncludeImages { get; set; } = false;
        public bool IncludeMetadata { get; set; } = true;
        public string SortBy { get; set; } = "Title";
        public bool SortAscending { get; set; } = true;
        public bool IncludeNutritionInfo { get; set; } = false;
        public bool IncludeCookingTips { get; set; } = false;
        public string GroupBy { get; set; } = "Category";
        public bool ConsolidateIngredients { get; set; } = false;
    }
}
