using AMCode.Common.Models;
using AMCode.Storage.ImageUtilities;
using AMCode.Storage.ImageUtilities.Models;
using AMCode.Storage.Interfaces;
using AMCode.Storage.Recipes.Interfaces;
using AMCode.Storage.Recipes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Storage.Recipes
{
    /// <summary>
    /// Service for managing recipe images and related files.
    /// Uses IImageUtility to optimize images on upload (strip EXIF, compress, generate thumbnails).
    /// </summary>
    public class RecipeImageStorageService : IRecipeImageStorageService
    {
        private readonly ISimpleFileStorage _fileStorage;
        private readonly IStorageLogger _logger;
        private readonly RecipeImageStorageOptions _options;
        private readonly IImageUtility _imageUtility;

        public RecipeImageStorageService(
            ISimpleFileStorage fileStorage,
            IStorageLogger logger,
            RecipeImageStorageOptions options,
            IImageUtility imageUtility = null)
        {
            _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _imageUtility = imageUtility; // Optional — when null, images are stored as-is
        }

        /// <summary>
        /// Stores a recipe image, optionally optimizing it via IImageUtility.
        /// When IImageUtility is provided:
        ///   1. Strips EXIF metadata (preserves orientation)
        ///   2. Compresses to configured quality
        ///   3. Generates multi-variant thumbnails
        /// </summary>
        public async Task<Result<string>> StoreRecipeImageAsync(
            string recipeId,
            Stream imageStream,
            string fileName,
            RecipeImageType imageType = RecipeImageType.Main)
        {
            try
            {
                _logger.LogInformation("Storing recipe image for recipe {0}, type: {1}", recipeId, imageType);

                Stream processedStream = imageStream;
                bool ownProcessedStream = false;

                if (_imageUtility != null && imageType == RecipeImageType.Main)
                {
                    // 1. Strip EXIF (keep orientation)
                    var stripped = await _imageUtility.StripMetadataAsync(imageStream);
                    processedStream = stripped.Data;
                    ownProcessedStream = true;

                    // 2. Compress
                    var compressed = await _imageUtility.CompressAsync(processedStream, new CompressionOptions
                    {
                        Quality = _options.ImageQuality
                    });
                    if (ownProcessedStream) processedStream.Dispose();
                    processedStream = compressed.Data;

                    // 3. Generate and store thumbnails if configured
                    if (_options.GenerateThumbnails)
                    {
                        processedStream.Position = 0;
                        await StoreThumbnailsAsync(recipeId, processedStream, fileName);
                        processedStream.Position = 0;
                    }
                }

                var filePath = GenerateImagePath(recipeId, fileName, imageType);
                var result = await _fileStorage.StoreFileAsync(processedStream, filePath);

                if (ownProcessedStream) processedStream.Dispose();

                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to store recipe image: {0}", result.Error);
                    return Result<string>.Failure(result.Error);
                }

                _logger.LogInformation("Successfully stored recipe image at: {0}", filePath);
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
        public async Task<Result<Stream>> GetRecipeImageAsync(string recipeId, RecipeImageType imageType = RecipeImageType.Main)
        {
            try
            {
                _logger.LogInformation("Retrieving recipe image for recipe {0}, type: {1}", recipeId, imageType);

                var filePath = GenerateImagePath(recipeId, string.Empty, imageType);
                var result = await _fileStorage.GetFileAsync(filePath);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Recipe image not found: {0}", filePath);
                    return Result<Stream>.Failure(result.Error);
                }

                _logger.LogInformation("Successfully retrieved recipe image from: {0}", filePath);
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
        public async Task<Result> DeleteRecipeImageAsync(string recipeId, RecipeImageType imageType = RecipeImageType.Main)
        {
            try
            {
                _logger.LogInformation("Deleting recipe image for recipe {0}, type: {1}", recipeId, imageType);

                var filePath = GenerateImagePath(recipeId, string.Empty, imageType);
                var result = await _fileStorage.DeleteFileAsync(filePath);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to delete recipe image: {0}", filePath);
                    return Result.Failure(result.Error);
                }

                _logger.LogInformation("Successfully deleted recipe image: {0}", filePath);
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
        public async Task<Result<List<string>>> ListRecipeImagesAsync(string recipeId)
        {
            try
            {
                _logger.LogInformation("Listing images for recipe {0}", recipeId);

                var recipePath = Path.Combine(_options.BasePath, "recipes", recipeId);
                var result = await _fileStorage.ListFilesAsync(recipePath);

                if (!result.IsSuccess)
                {
                    _logger.LogWarning("Failed to list recipe images: {0}", result.Error);
                    return Result<List<string>>.Failure(result.Error);
                }

                var imageFiles = result.Value
                    .Where(f => _options.SupportedImageExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                    .ToList();

                _logger.LogInformation("Found {0} images for recipe {1}", imageFiles.Count, recipeId);
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
        public async Task<Result<string>> StoreRecipeDocumentAsync(
            string recipeId,
            Stream documentStream,
            string fileName,
            RecipeDocumentType documentType = RecipeDocumentType.PDF)
        {
            try
            {
                _logger.LogInformation("Storing recipe document for recipe {0}, type: {1}", recipeId, documentType);

                var filePath = GenerateDocumentPath(recipeId, fileName, documentType);
                var result = await _fileStorage.StoreFileAsync(documentStream, filePath);

                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to store recipe document: {0}", result.Error);
                    return Result<string>.Failure(result.Error);
                }

                _logger.LogInformation("Successfully stored recipe document at: {0}", filePath);
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
        public async Task<Result<string>> StoreRecipeExportAsync(
            string recipeId,
            Stream exportStream,
            string fileName,
            RecipeExportType exportType = RecipeExportType.Excel)
        {
            try
            {
                _logger.LogInformation("Storing recipe export for recipe {0}, type: {1}", recipeId, exportType);

                var filePath = GenerateExportPath(recipeId, fileName, exportType);
                var result = await _fileStorage.StoreFileAsync(exportStream, filePath);

                if (!result.IsSuccess)
                {
                    _logger.LogError("Failed to store recipe export: {0}", result.Error);
                    return Result<string>.Failure(result.Error);
                }

                _logger.LogInformation("Successfully stored recipe export at: {0}", filePath);
                return Result<string>.Success(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while storing recipe export");
                return Result<string>.Failure($"Failed to store recipe export: {ex.Message}");
            }
        }

        // ─── Private helpers ─────────────────────────────────────────────────────

        private async Task StoreThumbnailsAsync(string recipeId, Stream imageStream, string fileName)
        {
            try
            {
                var thumbnailOptions = new ThumbnailOptions
                {
                    Variants = new System.Collections.Generic.List<ThumbnailVariant>
                    {
                        new ThumbnailVariant(_options.ThumbnailWidth, "thumb_sm"),
                        new ThumbnailVariant(_options.ThumbnailHeight, "thumb_md"),
                    },
                    Quality = _options.ImageQuality,
                    StripMetadata = true
                };

                var thumbnails = await _imageUtility.GenerateThumbnailsAsync(imageStream, thumbnailOptions);

                foreach (var thumbnail in thumbnails)
                {
                    var thumbFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{thumbnail.VariantSuffix}.webp";
                    var thumbPath = GenerateImagePath(recipeId, thumbFileName, RecipeImageType.Thumbnail);
                    var storeResult = await _fileStorage.StoreFileAsync(thumbnail.Data, thumbPath);

                    if (!storeResult.IsSuccess)
                    {
                        _logger.LogWarning("Failed to store thumbnail variant {0}: {1}", thumbnail.VariantSuffix, storeResult.Error);
                    }
                    else
                    {
                        _logger.LogInformation("Stored thumbnail variant {0} at {1}", thumbnail.VariantSuffix, thumbPath);
                    }

                    thumbnail.Data.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while generating thumbnails for recipe {0}", recipeId);
                // Non-fatal: main image store continues
            }
        }

        private string GenerateImagePath(string recipeId, string fileName, RecipeImageType imageType)
        {
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                extension = ".jpg";
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
