using AMCode.OCR;
using AMCode.OCR.Models;
using AMCode.OCR.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AMCode.OCR.Examples;

/// <summary>
/// Comprehensive examples demonstrating AMCode.OCR usage
/// </summary>
public class OCRExample
{
    private readonly IOCRService _ocrService;
    private readonly ILogger<OCRExample> _logger;

    public OCRExample(IOCRService ocrService, ILogger<OCRExample> logger)
    {
        _ocrService = ocrService;
        _logger = logger;
    }

    /// <summary>
    /// Basic OCR example - extract text from a single image
    /// </summary>
    public async Task BasicOCRExampleAsync()
    {
        _logger.LogInformation("Starting basic OCR example");

        try
        {
            // Example 1: Extract text from image file
            var imagePath = "sample-image.jpg";
            var result = await _ocrService.ExtractTextAsync(imagePath);

            if (result.IsSuccess)
            {
                _logger.LogInformation("OCR successful!");
                _logger.LogInformation("Extracted text: {Text}", result.Value.Text);
                _logger.LogInformation("Confidence: {Confidence}", result.Value.Confidence);
                _logger.LogInformation("Language: {Language}", result.Value.Language);
                _logger.LogInformation("Provider: {Provider}", result.Value.Provider);
                _logger.LogInformation("Processing time: {ProcessingTime}", result.Value.ProcessingTime);
            }
            else
            {
                _logger.LogError("OCR failed: {Error}", result.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Basic OCR example failed");
        }
    }

    /// <summary>
    /// Advanced OCR example with custom options
    /// </summary>
    public async Task AdvancedOCRExampleAsync()
    {
        _logger.LogInformation("Starting advanced OCR example");

        try
        {
            using var imageStream = File.OpenRead("sample-image.jpg");
            
            // Create custom OCR request
            var options = new OCRRequest
            {
                ConfidenceThreshold = 0.8,
                RequiresLanguageDetection = true,
                ReturnBoundingBoxes = true,
                PreferredLanguage = "en",
                MaxImageSizeMB = 5,
                PreprocessImage = true
            };

            var result = await _ocrService.ExtractTextAsync(imageStream, options);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Advanced OCR successful!");
                _logger.LogInformation("Text: {Text}", result.Value.Text);
                _logger.LogInformation("Confidence: {Confidence}", result.Value.Confidence);
                _logger.LogInformation("Language: {Language}", result.Value.Language);
                _logger.LogInformation("Text blocks count: {Count}", result.Value.TextBlocks.Count);

                // Process individual text blocks
                foreach (var textBlock in result.Value.TextBlocks)
                {
                    _logger.LogInformation("Block: '{Text}' (Confidence: {Confidence}, Language: {Language})",
                        textBlock.Text, textBlock.Confidence, textBlock.Language);
                    
                    if (textBlock.BoundingBox != null)
                    {
                        _logger.LogInformation("  Position: X={X}, Y={Y}, Width={Width}, Height={Height}",
                            textBlock.BoundingBox.X, textBlock.BoundingBox.Y,
                            textBlock.BoundingBox.Width, textBlock.BoundingBox.Height);
                    }
                }
            }
            else
            {
                _logger.LogError("Advanced OCR failed: {Error}", result.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Advanced OCR example failed");
        }
    }

    /// <summary>
    /// Batch processing example
    /// </summary>
    public async Task BatchProcessingExampleAsync()
    {
        _logger.LogInformation("Starting batch processing example");

        try
        {
            var imagePaths = new[]
            {
                "image1.jpg",
                "image2.jpg",
                "image3.jpg"
            };

            var options = new OCRRequest
            {
                ConfidenceThreshold = 0.7,
                RequiresLanguageDetection = true
            };

            var results = await _ocrService.ProcessBatchAsync(imagePaths, options);

            if (results.IsSuccess)
            {
                _logger.LogInformation("Batch processing successful!");
                _logger.LogInformation("Processed {Count} images", results.Value.Count());

                var index = 1;
                foreach (var result in results.Value)
                {
                    _logger.LogInformation("Image {Index}: '{Text}' (Confidence: {Confidence})",
                        index++, result.Text, result.Confidence);
                }
            }
            else
            {
                _logger.LogError("Batch processing failed: {Error}", results.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Batch processing example failed");
        }
    }

    /// <summary>
    /// Health monitoring example
    /// </summary>
    public async Task HealthMonitoringExampleAsync()
    {
        _logger.LogInformation("Starting health monitoring example");

        try
        {
            // Check service availability
            var isAvailable = await _ocrService.IsAvailableAsync();
            _logger.LogInformation("OCR service available: {Available}", isAvailable);

            if (isAvailable)
            {
                // Get health status
                var health = await _ocrService.GetHealthAsync();
                _logger.LogInformation("Health status: {Status}", health.Status);
                _logger.LogInformation("Is healthy: {IsHealthy}", health.IsHealthy);
                _logger.LogInformation("Success rate: {SuccessRate}%", health.SuccessRate);
                _logger.LogInformation("Average processing time: {ProcessingTime}", health.AverageProcessingTime);

                // Get capabilities
                var capabilities = _ocrService.GetCapabilities();
                _logger.LogInformation("Supports language detection: {SupportsLanguageDetection}", 
                    capabilities.SupportsLanguageDetection);
                _logger.LogInformation("Supports bounding boxes: {SupportsBoundingBoxes}", 
                    capabilities.SupportsBoundingBoxes);
                _logger.LogInformation("Supports confidence scores: {SupportsConfidenceScores}", 
                    capabilities.SupportsConfidenceScores);
                _logger.LogInformation("Max image size: {MaxImageSizeMB}MB", capabilities.MaxImageSizeMB);
                _logger.LogInformation("Supported languages: {Languages}", 
                    string.Join(", ", capabilities.SupportedLanguages));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health monitoring example failed");
        }
    }

    /// <summary>
    /// Cost analysis example
    /// </summary>
    public async Task CostAnalysisExampleAsync()
    {
        _logger.LogInformation("Starting cost analysis example");

        try
        {
            var imageSizeBytes = 1024 * 1024; // 1MB
            var options = new OCRRequest
            {
                ConfidenceThreshold = 0.8,
                RequiresLanguageDetection = true
            };

            var costEstimate = await _ocrService.GetCostEstimateAsync(imageSizeBytes, options);
            _logger.LogInformation("Estimated cost for {SizeMB}MB image: ${Cost:F4}", 
                imageSizeBytes / (1024.0 * 1024.0), costEstimate);

            // Process image and get actual cost
            using var imageStream = new MemoryStream(new byte[imageSizeBytes]);
            var result = await _ocrService.ExtractTextAsync(imageStream, options);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Actual cost: ${Cost:F4}", result.Value.Cost);
                _logger.LogInformation("Processing time: {ProcessingTime}", result.Value.ProcessingTime);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cost analysis example failed");
        }
    }

    /// <summary>
    /// Error handling example
    /// </summary>
    public async Task ErrorHandlingExampleAsync()
    {
        _logger.LogInformation("Starting error handling example");

        try
        {
            // Example 1: Invalid image path
            var result1 = await _ocrService.ExtractTextAsync("nonexistent-image.jpg");
            if (!result1.IsSuccess)
            {
                _logger.LogWarning("Expected error for nonexistent image: {Error}", result1.Error);
            }

            // Example 2: Empty image stream
            using var emptyStream = new MemoryStream();
            var result2 = await _ocrService.ExtractTextAsync(emptyStream);
            if (!result2.IsSuccess)
            {
                _logger.LogWarning("Expected error for empty stream: {Error}", result2.Error);
            }

            // Example 3: Very large image
            var largeOptions = new OCRRequest
            {
                MaxImageSizeMB = 1 // Very small limit
            };
            using var largeStream = new MemoryStream(new byte[2 * 1024 * 1024]); // 2MB
            var result3 = await _ocrService.ExtractTextAsync(largeStream, largeOptions);
            if (!result3.IsSuccess)
            {
                _logger.LogWarning("Expected error for oversized image: {Error}", result3.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling example failed");
        }
    }

    /// <summary>
    /// Recipe OCR example - specialized for recipe text extraction
    /// </summary>
    public async Task RecipeOCRExampleAsync()
    {
        _logger.LogInformation("Starting recipe OCR example");

        try
        {
            var recipeImagePath = "recipe-image.jpg";
            var options = new OCRRequest
            {
                ConfidenceThreshold = 0.8,
                RequiresLanguageDetection = true,
                ReturnBoundingBoxes = true,
                PreferredLanguage = "en",
                PreprocessImage = true
            };

            var result = await _ocrService.ExtractTextAsync(recipeImagePath, options);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Recipe OCR successful!");
                
                // Parse recipe components
                var recipe = ParseRecipe(result.Value.Text);
                
                _logger.LogInformation("Recipe Title: {Title}", recipe.Title);
                _logger.LogInformation("Servings: {Servings}", recipe.Servings);
                _logger.LogInformation("Prep Time: {PrepTime}", recipe.PrepTime);
                _logger.LogInformation("Cook Time: {CookTime}", recipe.CookTime);
                
                _logger.LogInformation("Ingredients:");
                foreach (var ingredient in recipe.Ingredients)
                {
                    _logger.LogInformation("  - {Ingredient}", ingredient);
                }
                
                _logger.LogInformation("Instructions:");
                for (int i = 0; i < recipe.Instructions.Count; i++)
                {
                    _logger.LogInformation("  {Step}. {Instruction}", i + 1, recipe.Instructions[i]);
                }
            }
            else
            {
                _logger.LogError("Recipe OCR failed: {Error}", result.Error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Recipe OCR example failed");
        }
    }

    /// <summary>
    /// Parse recipe from extracted text
    /// </summary>
    private Recipe ParseRecipe(string text)
    {
        // Simple recipe parsing logic
        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        var recipe = new Recipe
        {
            Title = "Extracted Recipe",
            Servings = "Unknown",
            PrepTime = "Unknown",
            CookTime = "Unknown",
            Ingredients = new List<string>(),
            Instructions = new List<string>()
        };

        bool inIngredients = false;
        bool inInstructions = false;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            
            if (string.IsNullOrWhiteSpace(trimmedLine))
                continue;

            if (trimmedLine.ToLower().Contains("ingredients"))
            {
                inIngredients = true;
                inInstructions = false;
                continue;
            }
            
            if (trimmedLine.ToLower().Contains("instructions") || 
                trimmedLine.ToLower().Contains("directions"))
            {
                inIngredients = false;
                inInstructions = true;
                continue;
            }

            if (inIngredients && !trimmedLine.ToLower().Contains("ingredients"))
            {
                recipe.Ingredients.Add(trimmedLine);
            }
            else if (inInstructions && !trimmedLine.ToLower().Contains("instructions"))
            {
                recipe.Instructions.Add(trimmedLine);
            }
        }

        return recipe;
    }

    /// <summary>
    /// Recipe data model
    /// </summary>
    private class Recipe
    {
        public string Title { get; set; } = string.Empty;
        public string Servings { get; set; } = string.Empty;
        public string PrepTime { get; set; } = string.Empty;
        public string CookTime { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public List<string> Instructions { get; set; } = new();
    }
}

// Example usage:
// var ocrExample = new OCRExample(ocrService, logger);
// await ocrExample.BasicOCRExampleAsync();
