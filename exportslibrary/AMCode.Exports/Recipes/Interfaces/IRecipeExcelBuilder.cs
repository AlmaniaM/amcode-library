using System.Collections.Generic;
using System.IO;
using AMCode.Exports.Recipes.Models;

namespace AMCode.Exports.Recipes.Interfaces
{
    /// <summary>
    /// Interface for building multi-sheet Excel recipe exports.
    /// </summary>
    public interface IRecipeExcelBuilder
    {
        /// <summary>
        /// Add a single recipe to the export.
        /// </summary>
        IRecipeExcelBuilder AddRecipe(RecipeExportData recipe);

        /// <summary>
        /// Add multiple recipes to the export.
        /// </summary>
        IRecipeExcelBuilder AddRecipes(IEnumerable<RecipeExportData> recipes);

        /// <summary>
        /// Configure custom column selection.
        /// </summary>
        IRecipeExcelBuilder WithColumns(RecipeColumnConfiguration columns);

        /// <summary>
        /// Configure Excel formatting options.
        /// </summary>
        IRecipeExcelBuilder WithFormatting(ExcelFormattingOptions options);

        /// <summary>
        /// Whether to include a Nutrition sheet (only written if recipe data has nutrition info).
        /// </summary>
        IRecipeExcelBuilder IncludeNutritionSheet(bool include = true);

        /// <summary>
        /// Build and return the workbook as a byte array.
        /// </summary>
        byte[] Build();

        /// <summary>
        /// Build and return the workbook as a <see cref="Stream"/>.
        /// </summary>
        Stream BuildToStream();
    }
}
