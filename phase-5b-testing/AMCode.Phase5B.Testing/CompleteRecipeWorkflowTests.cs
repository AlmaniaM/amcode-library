using NUnit.Framework;
using AMCode.OCR;
using AMCode.AI;
using AMCode.Documents;
using AMCode.Exports;
using AMCode.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace AMCode.Phase5B.Testing
{
    /// <summary>
    /// End-to-End Testing for Complete Recipe Processing Workflow
    /// Tests the complete flow: Image → OCR → AI Parsing → Recipe Creation → Document Generation → Export → Storage
    /// </summary>
    [TestFixture]
    public class CompleteRecipeWorkflowTests
    {
        private ServiceProvider _serviceProvider;
        private IOCRService _ocrService;
        private IAIRecipeParsingService _aiService;
        private IRecipeDocumentFactory _documentFactory;
        private IRecipeExportBuilder _exportBuilder;
        private IRecipeImageStorageService _storageService;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            
            // Add logging
            services.AddLogging(builder => builder.AddConsole());
            
            // Add AMCode services
            services.AddAMCodeOCR(GetOCRConfiguration());
            services.AddAMCodeAI(GetAIConfiguration());
            services.AddAMCodeDocuments(GetDocumentsConfiguration());
            services.AddAMCodeExports(GetExportsConfiguration());
            services.AddAMCodeStorage(GetStorageConfiguration());
            
            _serviceProvider = services.BuildServiceProvider();
            
            // Get services
            _ocrService = _serviceProvider.GetRequiredService<IOCRService>();
            _aiService = _serviceProvider.GetRequiredService<IAIRecipeParsingService>();
            _documentFactory = _serviceProvider.GetRequiredService<IRecipeDocumentFactory>();
            _exportBuilder = _serviceProvider.GetRequiredService<IRecipeExportBuilder>();
            _storageService = _serviceProvider.GetRequiredService<IRecipeImageStorageService>();
        }

        [TearDown]
        public void Cleanup()
        {
            _serviceProvider?.Dispose();
        }

        /// <summary>
        /// Test complete recipe processing workflow with AMCode libraries
        /// </summary>
        [Test]
        public async Task CompleteRecipeWorkflow_WithAMCodeLibraries_ShouldWorkEndToEnd()
        {
            // Arrange
            var testImagePath = CreateTestImage();
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                // Act - Step 1: OCR text extraction
                Console.WriteLine("Step 1: Starting OCR text extraction...");
                var ocrResult = await _ocrService.ExtractTextAsync(testImagePath);
                Assert.IsTrue(ocrResult.IsSuccess, $"OCR failed: {ocrResult.ErrorMessage}");
                Console.WriteLine($"OCR completed in {stopwatch.ElapsedMilliseconds}ms");
                Console.WriteLine($"Extracted text: {ocrResult.Value.Text.Substring(0, Math.Min(100, ocrResult.Value.Text.Length))}...");
                
                // Act - Step 2: AI recipe parsing
                Console.WriteLine("Step 2: Starting AI recipe parsing...");
                var aiStopwatch = Stopwatch.StartNew();
                var aiResult = await _aiService.ParseRecipeAsync(ocrResult.Value.Text);
                aiStopwatch.Stop();
                Assert.IsTrue(aiResult.IsSuccess, $"AI parsing failed: {aiResult.ErrorMessage}");
                Console.WriteLine($"AI parsing completed in {aiStopwatch.ElapsedMilliseconds}ms");
                
                // Act - Step 3: Recipe creation
                Console.WriteLine("Step 3: Creating recipe from parsed data...");
                var recipe = CreateRecipeFromParsedData(aiResult.Value);
                Assert.IsNotNull(recipe, "Recipe creation failed");
                Console.WriteLine($"Recipe created: {recipe.Title}");
                
                // Act - Step 4: Document generation
                Console.WriteLine("Step 4: Generating recipe document...");
                var docStopwatch = Stopwatch.StartNew();
                var documentResult = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                docStopwatch.Stop();
                Assert.IsTrue(documentResult.IsSuccess, $"Document generation failed: {documentResult.ErrorMessage}");
                Console.WriteLine($"Document generation completed in {docStopwatch.ElapsedMilliseconds}ms");
                
                // Act - Step 5: Export generation
                Console.WriteLine("Step 5: Generating export...");
                var exportStopwatch = Stopwatch.StartNew();
                var exportResult = await _exportBuilder.ExportRecipeAsync(recipe, ExportFormat.Pdf);
                exportStopwatch.Stop();
                Assert.IsTrue(exportResult.IsSuccess, $"Export generation failed: {exportResult.ErrorMessage}");
                Console.WriteLine($"Export generation completed in {exportStopwatch.ElapsedMilliseconds}ms");
                
                // Act - Step 6: Storage
                Console.WriteLine("Step 6: Storing files...");
                var storageStopwatch = Stopwatch.StartNew();
                var storageResult = await _storageService.StoreRecipeFilesAsync(recipe, documentResult.Value, exportResult.Value);
                storageStopwatch.Stop();
                Assert.IsTrue(storageResult.IsSuccess, $"Storage failed: {storageResult.ErrorMessage}");
                Console.WriteLine($"Storage completed in {storageStopwatch.ElapsedMilliseconds}ms");
                
                stopwatch.Stop();
                
                // Assert - Overall performance
                Console.WriteLine($"Complete workflow completed in {stopwatch.ElapsedMilliseconds}ms");
                Assert.IsTrue(stopwatch.ElapsedMilliseconds < 60000, $"Workflow took too long: {stopwatch.ElapsedMilliseconds}ms");
                
                // Assert - All steps succeeded
                Assert.IsTrue(ocrResult.IsSuccess, "OCR step failed");
                Assert.IsTrue(aiResult.IsSuccess, "AI parsing step failed");
                Assert.IsNotNull(recipe, "Recipe creation step failed");
                Assert.IsTrue(documentResult.IsSuccess, "Document generation step failed");
                Assert.IsTrue(exportResult.IsSuccess, "Export generation step failed");
                Assert.IsTrue(storageResult.IsSuccess, "Storage step failed");
            }
            finally
            {
                // Cleanup test image
                if (File.Exists(testImagePath))
                {
                    File.Delete(testImagePath);
                }
            }
        }

        /// <summary>
        /// Test error handling and recovery mechanisms
        /// </summary>
        [Test]
        public async Task CompleteRecipeWorkflow_WithErrorHandling_ShouldRecoverGracefully()
        {
            // Arrange
            var invalidImagePath = "nonexistent-image.jpg";
            
            // Act & Assert - OCR should handle invalid image gracefully
            var ocrResult = await _ocrService.ExtractTextAsync(invalidImagePath);
            Assert.IsFalse(ocrResult.IsSuccess, "OCR should fail for invalid image");
            Assert.IsNotNull(ocrResult.ErrorMessage, "Error message should be provided");
            
            // Test with empty text for AI parsing
            var aiResult = await _aiService.ParseRecipeAsync("");
            Assert.IsFalse(aiResult.IsSuccess, "AI parsing should fail for empty text");
            Assert.IsNotNull(aiResult.ErrorMessage, "Error message should be provided");
        }

        /// <summary>
        /// Test performance benchmarks
        /// </summary>
        [Test]
        public async Task CompleteRecipeWorkflow_Performance_ShouldMeetBenchmarks()
        {
            // Arrange
            var testImagePath = CreateTestImage();
            var performanceResults = new Dictionary<string, long>();
            
            try
            {
                // Act - Measure OCR performance
                var ocrStopwatch = Stopwatch.StartNew();
                var ocrResult = await _ocrService.ExtractTextAsync(testImagePath);
                ocrStopwatch.Stop();
                performanceResults["OCR"] = ocrStopwatch.ElapsedMilliseconds;
                
                if (ocrResult.IsSuccess)
                {
                    // Measure AI parsing performance
                    var aiStopwatch = Stopwatch.StartNew();
                    var aiResult = await _aiService.ParseRecipeAsync(ocrResult.Value.Text);
                    aiStopwatch.Stop();
                    performanceResults["AI"] = aiStopwatch.ElapsedMilliseconds;
                    
                    if (aiResult.IsSuccess)
                    {
                        var recipe = CreateRecipeFromParsedData(aiResult.Value);
                        
                        // Measure document generation performance
                        var docStopwatch = Stopwatch.StartNew();
                        var docResult = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                        docStopwatch.Stop();
                        performanceResults["Document"] = docStopwatch.ElapsedMilliseconds;
                    }
                }
                
                // Assert - Performance benchmarks
                Assert.IsTrue(performanceResults["OCR"] < 10000, $"OCR took too long: {performanceResults["OCR"]}ms");
                if (performanceResults.ContainsKey("AI"))
                {
                    Assert.IsTrue(performanceResults["AI"] < 15000, $"AI parsing took too long: {performanceResults["AI"]}ms");
                }
                if (performanceResults.ContainsKey("Document"))
                {
                    Assert.IsTrue(performanceResults["Document"] < 5000, $"Document generation took too long: {performanceResults["Document"]}ms");
                }
                
                Console.WriteLine("Performance Results:");
                foreach (var result in performanceResults)
                {
                    Console.WriteLine($"  {result.Key}: {result.Value}ms");
                }
            }
            finally
            {
                if (File.Exists(testImagePath))
                {
                    File.Delete(testImagePath);
                }
            }
        }

        #region Helper Methods

        private string CreateTestImage()
        {
            // Create a simple test image with recipe text
            var testImagePath = Path.GetTempFileName() + ".jpg";
            
            // For testing purposes, we'll create a simple text file that simulates an image
            // In a real scenario, this would be an actual image file
            var recipeText = @"
                Chocolate Chip Cookies
                
                Ingredients:
                - 2 cups all-purpose flour
                - 1 cup butter, softened
                - 3/4 cup brown sugar
                - 1/2 cup white sugar
                - 2 large eggs
                - 2 tsp vanilla extract
                - 1 tsp baking soda
                - 1 tsp salt
                - 2 cups chocolate chips
                
                Instructions:
                1. Preheat oven to 375°F
                2. Mix butter and sugars until creamy
                3. Beat in eggs and vanilla
                4. Combine flour, baking soda, and salt
                5. Gradually add to butter mixture
                6. Stir in chocolate chips
                7. Drop by rounded tablespoons onto ungreased cookie sheets
                8. Bake 9-11 minutes until golden brown
            ";
            
            File.WriteAllText(testImagePath, recipeText);
            return testImagePath;
        }

        private Recipe CreateRecipeFromParsedData(ParsedRecipeData parsedData)
        {
            return new Recipe
            {
                Id = Guid.NewGuid().ToString(),
                Title = parsedData.Title ?? "Unknown Recipe",
                Description = parsedData.Description ?? "",
                Ingredients = parsedData.Ingredients?.Select(i => new Ingredient
                {
                    Name = i.Name,
                    Amount = i.Amount,
                    Unit = i.Unit
                }).ToList() ?? new List<Ingredient>(),
                Instructions = parsedData.Instructions?.Select((instruction, index) => new Instruction
                {
                    StepNumber = index + 1,
                    Description = instruction
                }).ToList() ?? new List<Instruction>(),
                PrepTime = parsedData.PrepTime ?? TimeSpan.Zero,
                CookTime = parsedData.CookTime ?? TimeSpan.Zero,
                Servings = parsedData.Servings ?? 1,
                CreatedAt = DateTime.UtcNow
            };
        }

        private OCRConfiguration GetOCRConfiguration()
        {
            return new OCRConfiguration
            {
                DefaultProvider = "Azure",
                Providers = new Dictionary<string, OCRProviderConfiguration>
                {
                    ["Azure"] = new OCRProviderConfiguration
                    {
                        IsEnabled = true,
                        ApiKey = "test-key",
                        Endpoint = "https://test.cognitiveservices.azure.com/"
                    }
                }
            };
        }

        private AIConfiguration GetAIConfiguration()
        {
            return new AIConfiguration
            {
                DefaultProvider = "OpenAI",
                Providers = new Dictionary<string, AIProviderConfiguration>
                {
                    ["OpenAI"] = new AIProviderConfiguration
                    {
                        IsEnabled = true,
                        ApiKey = "test-key",
                        Model = "gpt-3.5-turbo"
                    }
                }
            };
        }

        private DocumentsConfiguration GetDocumentsConfiguration()
        {
            return new DocumentsConfiguration
            {
                DefaultFont = "Arial",
                DefaultFontSize = 12,
                PageMargins = "1in"
            };
        }

        private ExportsConfiguration GetExportsConfiguration()
        {
            return new ExportsConfiguration
            {
                DefaultDateFormat = "yyyy-MM-dd",
                IncludeMetadata = true
            };
        }

        private StorageConfiguration GetStorageConfiguration()
        {
            return new StorageConfiguration
            {
                BasePath = Path.GetTempPath(),
                MaxImageSizeMB = 10,
                MaxDocumentSizeMB = 50,
                GenerateThumbnails = true,
                ThumbnailWidth = 300,
                ThumbnailHeight = 300,
                CompressImages = true,
                ImageQuality = 85
            };
        }

        #endregion
    }

    #region Data Models

    public class Recipe
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<Instruction> Instructions { get; set; } = new();
        public TimeSpan PrepTime { get; set; }
        public TimeSpan CookTime { get; set; }
        public int Servings { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Ingredient
    {
        public string Name { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
    }

    public class Instruction
    {
        public int StepNumber { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class ParsedRecipeData
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<ParsedIngredient>? Ingredients { get; set; }
        public List<string>? Instructions { get; set; }
        public TimeSpan? PrepTime { get; set; }
        public TimeSpan? CookTime { get; set; }
        public int? Servings { get; set; }
    }

    public class ParsedIngredient
    {
        public string Name { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
    }

    public enum ExportFormat
    {
        Pdf,
        Docx,
        Csv,
        Excel
    }

    #endregion

    #region Configuration Classes

    public class OCRConfiguration
    {
        public string DefaultProvider { get; set; } = string.Empty;
        public Dictionary<string, OCRProviderConfiguration> Providers { get; set; } = new();
    }

    public class OCRProviderConfiguration
    {
        public bool IsEnabled { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
    }

    public class AIConfiguration
    {
        public string DefaultProvider { get; set; } = string.Empty;
        public Dictionary<string, AIProviderConfiguration> Providers { get; set; } = new();
    }

    public class AIProviderConfiguration
    {
        public bool IsEnabled { get; set; }
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
    }

    public class DocumentsConfiguration
    {
        public string DefaultFont { get; set; } = string.Empty;
        public int DefaultFontSize { get; set; }
        public string PageMargins { get; set; } = string.Empty;
    }

    public class ExportsConfiguration
    {
        public string DefaultDateFormat { get; set; } = string.Empty;
        public bool IncludeMetadata { get; set; }
    }

    public class StorageConfiguration
    {
        public string BasePath { get; set; } = string.Empty;
        public int MaxImageSizeMB { get; set; }
        public int MaxDocumentSizeMB { get; set; }
        public bool GenerateThumbnails { get; set; }
        public int ThumbnailWidth { get; set; }
        public int ThumbnailHeight { get; set; }
        public bool CompressImages { get; set; }
        public int ImageQuality { get; set; }
    }

    #endregion
}
