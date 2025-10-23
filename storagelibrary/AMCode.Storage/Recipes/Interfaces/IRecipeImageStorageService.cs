using AMCode.Common.Models;
using AMCode.Storage.Recipes.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Storage.Recipes.Interfaces
{
    /// <summary>
    /// Interface for recipe image storage operations
    /// </summary>
    public interface IRecipeImageStorageService
    {
        /// <summary>
        /// Stores a recipe image
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="imageStream">The image stream</param>
        /// <param name="fileName">The original file name</param>
        /// <param name="imageType">The type of image (main, thumbnail, etc.)</param>
        /// <returns>Result containing the stored file path</returns>
        Task<Result<string>> StoreRecipeImageAsync(string recipeId, Stream imageStream, string fileName, RecipeImageType imageType = RecipeImageType.Main);
        
        /// <summary>
        /// Retrieves a recipe image
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="imageType">The type of image to retrieve</param>
        /// <returns>Result containing the image stream</returns>
        Task<Result<Stream>> GetRecipeImageAsync(string recipeId, RecipeImageType imageType = RecipeImageType.Main);
        
        /// <summary>
        /// Deletes a recipe image
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="imageType">The type of image to delete</param>
        /// <returns>Result indicating success or failure</returns>
        Task<Result> DeleteRecipeImageAsync(string recipeId, RecipeImageType imageType = RecipeImageType.Main);
        
        /// <summary>
        /// Lists all images for a recipe
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <returns>Result containing the list of image paths</returns>
        Task<Result<List<string>>> ListRecipeImagesAsync(string recipeId);
        
        /// <summary>
        /// Stores a recipe document (PDF, DOCX, etc.)
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="documentStream">The document stream</param>
        /// <param name="fileName">The original file name</param>
        /// <param name="documentType">The type of document</param>
        /// <returns>Result containing the stored file path</returns>
        Task<Result<string>> StoreRecipeDocumentAsync(string recipeId, Stream documentStream, string fileName, RecipeDocumentType documentType = RecipeDocumentType.PDF);
        
        /// <summary>
        /// Stores recipe export files
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="exportStream">The export stream</param>
        /// <param name="fileName">The export file name</param>
        /// <param name="exportType">The type of export</param>
        /// <returns>Result containing the stored file path</returns>
        Task<Result<string>> StoreRecipeExportAsync(string recipeId, Stream exportStream, string fileName, RecipeExportType exportType = RecipeExportType.Excel);
    }
}
