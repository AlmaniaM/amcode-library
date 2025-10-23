using AMCode.Common.Models;
using AMCode.Exports.Recipes.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Exports.Recipes.Interfaces
{
    /// <summary>
    /// Interface for recipe export operations
    /// </summary>
    public interface IRecipeExportBuilder
    {
        /// <summary>
        /// Exports recipes to the specified format
        /// </summary>
        /// <param name="recipes">Collection of recipes to export</param>
        /// <param name="options">Export options</param>
        /// <returns>Result containing the export stream</returns>
        Task<Result<Stream>> ExportRecipesAsync(IEnumerable<Recipe> recipes, RecipeExportOptions options);
        
        /// <summary>
        /// Exports recipes with custom column configuration
        /// </summary>
        /// <param name="recipes">Collection of recipes to export</param>
        /// <param name="options">Export options</param>
        /// <param name="columnConfig">Custom column configuration</param>
        /// <returns>Result containing the export stream</returns>
        Task<Result<Stream>> ExportRecipesAsync(
            IEnumerable<Recipe> recipes, 
            RecipeExportOptions options,
            RecipeColumnConfiguration columnConfig);
        
        /// <summary>
        /// Exports recipe ingredients as a shopping list
        /// </summary>
        /// <param name="recipes">Collection of recipes</param>
        /// <param name="options">Export options</param>
        /// <returns>Result containing the shopping list export stream</returns>
        Task<Result<Stream>> ExportShoppingListAsync(IEnumerable<Recipe> recipes, RecipeExportOptions options);
    }
}
