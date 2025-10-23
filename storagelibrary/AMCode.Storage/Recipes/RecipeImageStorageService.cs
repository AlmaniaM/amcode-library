using AMCode.Common.Models;
using AMCode.Storage.Components.Storage;
using AMCode.Storage.Interfaces;
using AMCode.Storage.Recipes.Interfaces;
using AMCode.Storage.Recipes.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AMCode.Storage.Recipes
{
    /// <summary>
    /// Service for managing recipe images and related files
    /// </summary>
    public class RecipeImageStorageService : IRecipeImageStorageService
    {
        private readonly ISimpleFileStorage _fileStorage;
        private readonly IStorageLogger _logger;
        private readonly RecipeImageStorageOptions _options;
        
        public RecipeImageStorageService(
            ISimpleFileStorage fileStorage,
            IStorageLogger logger,
            RecipeImageStorageOptions options)
        {
            _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        
        /// <summary>
        /// Stores a recipe image
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="imageStream">The image stream</param>
        /// <param name="fileName">The original file name</param>
        /// <param name="imageType">The type of image (main, thumbnail, etc.)</param>
        /// <returns>Result containing the stored file path</returns>
        public async Task<Result<string>> StoreRecipeImageAsync(
            string recipeId, 
            Stream imageStream, 
            string fileName, 
            RecipeImageType imageType = RecipeImageType.Main)
        {
            try
            {
                _logger.LogInformation("Storing recipe image for recipe {RecipeId}, type: {ImageType}", recipeId, imageType);
                
                var filePath = GenerateImagePath(recipeId, fileName, imageType);
                var result = await _fileStorage.StoreFileAsync(imageStream, filePath);
                
                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to store recipe image: {Error}", result.Error);
                    return Result<string>.Failure(result.Error);
                }
                
                _logger.LogInformation("Successfully stored recipe image at: {FilePath}", filePath);
                return Result<string>.Success(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while storing recipe image");
                return Result<string>.Failure($"Failed to store recipe image: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Retrieves a recipe image
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="imageType">The type of image to retrieve</param>
        /// <returns>Result containing the image stream</returns>
        public async Task<Result<Stream>> GetRecipeImageAsync(string recipeId, RecipeImageType imageType = RecipeImageType.Main)
        {
            try
            {
                _logger.LogInformation("Retrieving recipe image for recipe {RecipeId}, type: {ImageType}", recipeId, imageType);
                
                var filePath = GenerateImagePath(recipeId, string.Empty, imageType);
                var result = await _fileStorage.GetFileAsync(filePath);
                
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Recipe image not found: {FilePath}", filePath);
                    return Result<Stream>.Failure(result.Error);
                }
                
                _logger.LogInformation("Successfully retrieved recipe image from: {FilePath}", filePath);
                return Result<Stream>.Success(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving recipe image");
                return Result<Stream>.Failure($"Failed to retrieve recipe image: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Deletes a recipe image
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="imageType">The type of image to delete</param>
        /// <returns>Result indicating success or failure</returns>
        public async Task<Result> DeleteRecipeImageAsync(string recipeId, RecipeImageType imageType = RecipeImageType.Main)
        {
            try
            {
                _logger.LogInformation("Deleting recipe image for recipe {RecipeId}, type: {ImageType}", recipeId, imageType);
                
                var filePath = GenerateImagePath(recipeId, string.Empty, imageType);
                var result = await _fileStorage.DeleteFileAsync(filePath);
                
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to delete recipe image: {FilePath}", filePath);
                    return Result.Failure(result.Error);
                }
                
                _logger.LogInformation("Successfully deleted recipe image: {FilePath}", filePath);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting recipe image");
                return Result.Failure($"Failed to delete recipe image: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Lists all images for a recipe
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <returns>Result containing the list of image paths</returns>
        public async Task<Result<List<string>>> ListRecipeImagesAsync(string recipeId)
        {
            try
            {
                _logger.LogInformation("Listing images for recipe {RecipeId}", recipeId);
                
                var recipePath = Path.Combine(_options.BasePath, "recipes", recipeId);
                var result = await _fileStorage.ListFilesAsync(recipePath);
                
                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to list recipe images: {Error}", result.Error);
                    return Result<List<string>>.Failure(result.Error);
                }
                
                var imageFiles = result.Value
                    .Where(f => _options.SupportedImageExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                    .ToList();
                
                _logger.LogInformation("Found {Count} images for recipe {RecipeId}", imageFiles.Count, recipeId);
                return Result<List<string>>.Success(imageFiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while listing recipe images");
                return Result<List<string>>.Failure($"Failed to list recipe images: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Stores a recipe document (PDF, DOCX, etc.)
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="documentStream">The document stream</param>
        /// <param name="fileName">The original file name</param>
        /// <param name="documentType">The type of document</param>
        /// <returns>Result containing the stored file path</returns>
        public async Task<Result<string>> StoreRecipeDocumentAsync(
            string recipeId, 
            Stream documentStream, 
            string fileName, 
            RecipeDocumentType documentType = RecipeDocumentType.PDF)
        {
            try
            {
                _logger.LogInformation("Storing recipe document for recipe {RecipeId}, type: {DocumentType}", recipeId, documentType);
                
                var filePath = GenerateDocumentPath(recipeId, fileName, documentType);
                var result = await _fileStorage.StoreFileAsync(documentStream, filePath);
                
                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to store recipe document: {Error}", result.Error);
                    return Result<string>.Failure(result.Error);
                }
                
                _logger.LogInformation("Successfully stored recipe document at: {FilePath}", filePath);
                return Result<string>.Success(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while storing recipe document");
                return Result<string>.Failure($"Failed to store recipe document: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Stores recipe export files
        /// </summary>
        /// <param name="recipeId">The recipe ID</param>
        /// <param name="exportStream">The export stream</param>
        /// <param name="fileName">The export file name</param>
        /// <param name="exportType">The type of export</param>
        /// <returns>Result containing the stored file path</returns>
        public async Task<Result<string>> StoreRecipeExportAsync(
            string recipeId, 
            Stream exportStream, 
            string fileName, 
            RecipeExportType exportType = RecipeExportType.Excel)
        {
            try
            {
                _logger.LogInformation("Storing recipe export for recipe {RecipeId}, type: {ExportType}", recipeId, exportType);
                
                var filePath = GenerateExportPath(recipeId, fileName, exportType);
                var result = await _fileStorage.StoreFileAsync(exportStream, filePath);
                
                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to store recipe export: {Error}", result.Error);
                    return Result<string>.Failure(result.Error);
                }
                
                _logger.LogInformation("Successfully stored recipe export at: {FilePath}", filePath);
                return Result<string>.Success(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while storing recipe export");
                return Result<string>.Failure($"Failed to store recipe export: {ex.Message}");
            }
        }
        
        private string GenerateImagePath(string recipeId, string fileName, RecipeImageType imageType)
        {
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                extension = ".jpg"; // Default extension
            }
            
            var imageTypeFolder = imageType switch
            {
                RecipeImageType.Main => "main",
                RecipeImageType.Thumbnail => "thumbnails",
                RecipeImageType.Step => "steps",
                _ => "images"
            };
            
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrEmpty(fileNameWithoutExtension))
            {
                fileNameWithoutExtension = $"{imageType}_{DateTime.UtcNow:yyyyMMddHHmmss}";
            }
            
            return Path.Combine(_options.BasePath, "recipes", recipeId, imageTypeFolder, $"{fileNameWithoutExtension}{extension}");
        }
        
        private string GenerateDocumentPath(string recipeId, string fileName, RecipeDocumentType documentType)
        {
            var extension = documentType switch
            {
                RecipeDocumentType.PDF => ".pdf",
                RecipeDocumentType.DOCX => ".docx",
                RecipeDocumentType.TXT => ".txt",
                _ => Path.GetExtension(fileName)
            };
            
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrEmpty(fileNameWithoutExtension))
            {
                fileNameWithoutExtension = $"recipe_{DateTime.UtcNow:yyyyMMddHHmmss}";
            }
            
            return Path.Combine(_options.BasePath, "recipes", recipeId, "documents", $"{fileNameWithoutExtension}{extension}");
        }
        
        private string GenerateExportPath(string recipeId, string fileName, RecipeExportType exportType)
        {
            var extension = exportType switch
            {
                RecipeExportType.Excel => ".xlsx",
                RecipeExportType.CSV => ".csv",
                RecipeExportType.PDF => ".pdf",
                _ => Path.GetExtension(fileName)
            };
            
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.IsNullOrEmpty(fileNameWithoutExtension))
            {
                fileNameWithoutExtension = $"export_{DateTime.UtcNow:yyyyMMddHHmmss}";
            }
            
            return Path.Combine(_options.BasePath, "recipes", recipeId, "exports", $"{fileNameWithoutExtension}{extension}");
        }
    }
}
