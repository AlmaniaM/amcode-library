using System.Collections.Generic;
using System.IO;
using AMCode.Exports.Recipes.Models;

namespace AMCode.Exports.Recipes.Interfaces
{
    /// <summary>
    /// Interface for building CSV recipe exports with proper escaping and configurable formatting.
    /// </summary>
    public interface IRecipeCsvBuilder
    {
        /// <summary>
        /// Add a single recipe to the export.
        /// </summary>
        IRecipeCsvBuilder AddRecipe(RecipeExportData recipe);

        /// <summary>
        /// Add multiple recipes to the export.
        /// </summary>
        IRecipeCsvBuilder AddRecipes(IEnumerable<RecipeExportData> recipes);

        /// <summary>
        /// Configure custom column selection.
        /// </summary>
        IRecipeCsvBuilder WithColumns(RecipeColumnConfiguration columns);

        /// <summary>
        /// Set the field delimiter. Default: comma.
        /// </summary>
        IRecipeCsvBuilder WithDelimiter(char delimiter);

        /// <summary>
        /// When true, outputs one row per ingredient per recipe. When false, ingredients are joined.
        /// </summary>
        IRecipeCsvBuilder WithExpandedIngredients(bool expanded = true);

        /// <summary>
        /// Build and return the CSV as a byte array (includes UTF-8 BOM for Excel compatibility).
        /// </summary>
        byte[] Build();

        /// <summary>
        /// Build and return the CSV as a <see cref="Stream"/> (includes UTF-8 BOM for Excel compatibility).
        /// </summary>
        Stream BuildToStream();
    }
}
