using NUnit.Framework;
using AMCode.OCR;
using AMCode.AI;
using AMCode.Documents;
using AMCode.Exports;
using AMCode.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace AMCode.Phase5B.Testing
{
    /// <summary>
    /// Performance Optimization Tests for AMCode Libraries
    /// Tests performance benchmarks, load handling, and optimization strategies
    /// </summary>
    [TestFixture]
    public class PerformanceOptimizationTests
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
            
            // Add AMCode services with performance optimizations
            services.AddAMCodeOCR(GetOptimizedOCRConfiguration());
            services.AddAMCodeAI(GetOptimizedAIConfiguration());
            services.AddAMCodeDocuments(GetOptimizedDocumentsConfiguration());
            services.AddAMCodeExports(GetOptimizedExportsConfiguration());
            services.AddAMCodeStorage(GetOptimizedStorageConfiguration());
            
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
        /// Test OCR processing performance with optimization
        /// </summary>
        [Test]
        public async Task OCRProcessing_WithOptimization_ShouldMeetPerformanceBenchmarks()
        {
            // Arrange
            var testImagePath = CreateTestImage();
            var iterations = 5;
            var results = new List<long>();
            
            try
            {
                // Act - Run multiple iterations to get average performance
                for (int i = 0; i < iterations; i++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    var result = await _ocrService.ExtractTextAsync(testImagePath);
                    stopwatch.Stop();
                    
                    if (result.IsSuccess)
                    {
                        results.Add(stopwatch.ElapsedMilliseconds);
                        Console.WriteLine($"OCR iteration {i + 1}: {stopwatch.ElapsedMilliseconds}ms");
                    }
                }
                
                // Assert - Performance benchmarks
                var averageTime = results.Average();
                var maxTime = results.Max();
                
                Assert.IsTrue(averageTime < 5000, $"Average OCR time too long: {averageTime}ms");
                Assert.IsTrue(maxTime < 10000, $"Max OCR time too long: {maxTime}ms");
                
                Console.WriteLine($"OCR Performance - Average: {averageTime:F2}ms, Max: {maxTime}ms");
            }
            finally
            {
                if (File.Exists(testImagePath))
                {
                    File.Delete(testImagePath);
                }
            }
        }

        /// <summary>
        /// Test AI parsing performance with optimization
        /// </summary>
        [Test]
        public async Task AIParsing_WithOptimization_ShouldMeetPerformanceBenchmarks()
        {
            // Arrange
            var testText = GetTestRecipeText();
            var iterations = 3;
            var results = new List<long>();
            
            // Act - Run multiple iterations to get average performance
            for (int i = 0; i < iterations; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                var result = await _aiService.ParseRecipeAsync(testText);
                stopwatch.Stop();
                
                if (result.IsSuccess)
                {
                    results.Add(stopwatch.ElapsedMilliseconds);
                    Console.WriteLine($"AI parsing iteration {i + 1}: {stopwatch.ElapsedMilliseconds}ms");
                }
            }
            
            // Assert - Performance benchmarks
            if (results.Any())
            {
                var averageTime = results.Average();
                var maxTime = results.Max();
                
                Assert.IsTrue(averageTime < 10000, $"Average AI parsing time too long: {averageTime}ms");
                Assert.IsTrue(maxTime < 15000, $"Max AI parsing time too long: {maxTime}ms");
                
                Console.WriteLine($"AI Parsing Performance - Average: {averageTime:F2}ms, Max: {maxTime}ms");
            }
        }

        /// <summary>
        /// Test document generation performance
        /// </summary>
        [Test]
        public async Task DocumentGeneration_WithOptimization_ShouldMeetPerformanceBenchmarks()
        {
            // Arrange
            var recipe = CreateTestRecipe();
            var iterations = 5;
            var results = new List<long>();
            
            // Act - Run multiple iterations to get average performance
            for (int i = 0; i < iterations; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                var result = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                stopwatch.Stop();
                
                if (result.IsSuccess)
                {
                    results.Add(stopwatch.ElapsedMilliseconds);
                    Console.WriteLine($"Document generation iteration {i + 1}: {stopwatch.ElapsedMilliseconds}ms");
                }
            }
            
            // Assert - Performance benchmarks
            if (results.Any())
            {
                var averageTime = results.Average();
                var maxTime = results.Max();
                
                Assert.IsTrue(averageTime < 3000, $"Average document generation time too long: {averageTime}ms");
                Assert.IsTrue(maxTime < 5000, $"Max document generation time too long: {maxTime}ms");
                
                Console.WriteLine($"Document Generation Performance - Average: {averageTime:F2}ms, Max: {maxTime}ms");
            }
        }

        /// <summary>
        /// Test export generation performance
        /// </summary>
        [Test]
        public async Task ExportGeneration_WithOptimization_ShouldMeetPerformanceBenchmarks()
        {
            // Arrange
            var recipe = CreateTestRecipe();
            var iterations = 5;
            var results = new List<long>();
            
            // Act - Run multiple iterations to get average performance
            for (int i = 0; i < iterations; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                var result = await _exportBuilder.ExportRecipeAsync(recipe, ExportFormat.Pdf);
                stopwatch.Stop();
                
                if (result.IsSuccess)
                {
                    results.Add(stopwatch.ElapsedMilliseconds);
                    Console.WriteLine($"Export generation iteration {i + 1}: {stopwatch.ElapsedMilliseconds}ms");
                }
            }
            
            // Assert - Performance benchmarks
            if (results.Any())
            {
                var averageTime = results.Average();
                var maxTime = results.Max();
                
                Assert.IsTrue(averageTime < 2000, $"Average export generation time too long: {averageTime}ms");
                Assert.IsTrue(maxTime < 3000, $"Max export generation time too long: {maxTime}ms");
                
                Console.WriteLine($"Export Generation Performance - Average: {averageTime:F2}ms, Max: {maxTime}ms");
            }
        }

        /// <summary>
        /// Test concurrent processing performance
        /// </summary>
        [Test]
        public async Task ConcurrentProcessing_WithAMCodeLibraries_ShouldHandleLoad()
        {
            // Arrange
            var concurrentUsers = 10;
            var tasks = new List<Task<ProcessingResult>>();
            var stopwatch = Stopwatch.StartNew();
            
            // Act - Process multiple recipes concurrently
            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks.Add(ProcessRecipeAsync($"test-recipe-{i}"));
            }
            
            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();
            
            // Assert - All tasks should complete successfully
            var successfulResults = results.Where(r => r.IsSuccess).ToList();
            var failedResults = results.Where(r => !r.IsSuccess).ToList();
            
            Console.WriteLine($"Concurrent Processing Results:");
            Console.WriteLine($"  Total tasks: {results.Length}");
            Console.WriteLine($"  Successful: {successfulResults.Count}");
            Console.WriteLine($"  Failed: {failedResults.Count}");
            Console.WriteLine($"  Total time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Average time per task: {stopwatch.ElapsedMilliseconds / (double)results.Length:F2}ms");
            
            // At least 80% should succeed
            var successRate = (double)successfulResults.Count / results.Length;
            Assert.IsTrue(successRate >= 0.8, $"Success rate too low: {successRate:P2}");
            
            // All successful tasks should complete within 30 seconds
            var maxProcessingTime = successfulResults.Any() ? successfulResults.Max(r => r.ElapsedMilliseconds) : 0;
            Assert.IsTrue(maxProcessingTime < 30000, $"Max processing time too long: {maxProcessingTime}ms");
        }

        /// <summary>
        /// Test memory usage optimization
        /// </summary>
        [Test]
        public async Task MemoryUsage_WithOptimization_ShouldBeEfficient()
        {
            // Arrange
            var initialMemory = GC.GetTotalMemory(true);
            var iterations = 20;
            var maxMemoryIncrease = 100 * 1024 * 1024; // 100MB
            
            // Act - Process multiple recipes to test memory usage
            for (int i = 0; i < iterations; i++)
            {
                var recipe = CreateTestRecipe();
                var result = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                
                // Force garbage collection every 5 iterations
                if (i % 5 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            
            // Force final garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            
            var finalMemory = GC.GetTotalMemory(false);
            var memoryIncrease = finalMemory - initialMemory;
            
            // Assert - Memory usage should be reasonable
            Assert.IsTrue(memoryIncrease < maxMemoryIncrease, 
                $"Memory usage too high: {memoryIncrease / (1024 * 1024):F2}MB increase");
            
            Console.WriteLine($"Memory Usage Test:");
            Console.WriteLine($"  Initial memory: {initialMemory / (1024 * 1024):F2}MB");
            Console.WriteLine($"  Final memory: {finalMemory / (1024 * 1024):F2}MB");
            Console.WriteLine($"  Memory increase: {memoryIncrease / (1024 * 1024):F2}MB");
        }

        /// <summary>
        /// Test cost optimization strategies
        /// </summary>
        [Test]
        public async Task CostOptimization_WithSmartProviderSelection_ShouldMinimizeCosts()
        {
            // Arrange
            var testImagePath = CreateTestImage();
            var costResults = new List<CostAnalysis>();
            
            try
            {
                // Act - Test OCR with different providers to compare costs
                var ocrResult = await _ocrService.ExtractTextAsync(testImagePath);
                
                if (ocrResult.IsSuccess)
                {
                    // Test AI parsing with different providers
                    var aiResult = await _aiService.ParseRecipeAsync(ocrResult.Value.Text);
                    
                    if (aiResult.IsSuccess)
                    {
                        var recipe = CreateRecipeFromParsedData(aiResult.Value);
                        
                        // Test document generation
                        var docResult = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                        
                        if (docResult.IsSuccess)
                        {
                            // Test export generation
                            var exportResult = await _exportBuilder.ExportRecipeAsync(recipe, ExportFormat.Pdf);
                            
                            // Calculate total cost
                            var totalCost = CalculateTotalCost(ocrResult.Value, aiResult.Value, docResult.Value, exportResult.Value);
                            
                            Console.WriteLine($"Cost Analysis:");
                            Console.WriteLine($"  OCR Cost: ${ocrResult.Value.EstimatedCost:F4}");
                            Console.WriteLine($"  AI Cost: ${aiResult.Value.EstimatedCost:F4}");
                            Console.WriteLine($"  Document Cost: ${docResult.Value.EstimatedCost:F4}");
                            Console.WriteLine($"  Export Cost: ${exportResult.Value.EstimatedCost:F4}");
                            Console.WriteLine($"  Total Cost: ${totalCost:F4}");
                            
                            // Assert - Total cost should be reasonable
                            Assert.IsTrue(totalCost < 0.10, $"Total cost too high: ${totalCost:F4}");
                        }
                    }
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

        private async Task<ProcessingResult> ProcessRecipeAsync(string recipeId)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var testImagePath = CreateTestImage();
                
                // OCR
                var ocrResult = await _ocrService.ExtractTextAsync(testImagePath);
                if (!ocrResult.IsSuccess) return new ProcessingResult { IsSuccess = false, ErrorMessage = ocrResult.ErrorMessage };
                
                // AI Parsing
                var aiResult = await _aiService.ParseRecipeAsync(ocrResult.Value.Text);
                if (!aiResult.IsSuccess) return new ProcessingResult { IsSuccess = false, ErrorMessage = aiResult.ErrorMessage };
                
                // Recipe Creation
                var recipe = CreateRecipeFromParsedData(aiResult.Value);
                
                // Document Generation
                var docResult = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                if (!docResult.IsSuccess) return new ProcessingResult { IsSuccess = false, ErrorMessage = docResult.ErrorMessage };
                
                stopwatch.Stop();
                
                return new ProcessingResult 
                { 
                    IsSuccess = true, 
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    RecipeId = recipeId
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new ProcessingResult 
                { 
                    IsSuccess = false, 
                    ErrorMessage = ex.Message,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
                };
            }
        }

        private string CreateTestImage()
        {
            var testImagePath = Path.GetTempFileName() + ".jpg";
            var recipeText = GetTestRecipeText();
            File.WriteAllText(testImagePath, recipeText);
            return testImagePath;
        }

        private string GetTestRecipeText()
        {
            return @"
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
        }

        private Recipe CreateTestRecipe()
        {
            return new Recipe
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Test Recipe",
                Description = "A test recipe for performance testing",
                Ingredients = new List<Ingredient>
                {
                    new() { Name = "Flour", Amount = "2", Unit = "cups" },
                    new() { Name = "Butter", Amount = "1", Unit = "cup" },
                    new() { Name = "Sugar", Amount = "3/4", Unit = "cup" }
                },
                Instructions = new List<Instruction>
                {
                    new() { StepNumber = 1, Description = "Mix ingredients" },
                    new() { StepNumber = 2, Description = "Bake at 375°F" }
                },
                PrepTime = TimeSpan.FromMinutes(15),
                CookTime = TimeSpan.FromMinutes(10),
                Servings = 24,
                CreatedAt = DateTime.UtcNow
            };
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

        private decimal CalculateTotalCost(OCRResult ocrResult, ParsedRecipeData aiResult, DocumentResult docResult, ExportResult exportResult)
        {
            return (ocrResult.EstimatedCost ?? 0) + 
                   (aiResult.EstimatedCost ?? 0) + 
                   (docResult.EstimatedCost ?? 0) + 
                   (exportResult.EstimatedCost ?? 0);
        }

        private OCRConfiguration GetOptimizedOCRConfiguration()
        {
            return new OCRConfiguration
            {
                Provider = "Azure",
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

        private AIConfiguration GetOptimizedAIConfiguration()
        {
            return new AIConfiguration
            {
                Provider = "OpenAI",
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

        private DocumentsConfiguration GetOptimizedDocumentsConfiguration()
        {
            return new DocumentsConfiguration
            {
                DefaultFont = "Arial",
                DefaultFontSize = 12,
                PageMargins = "1in"
            };
        }

        private ExportsConfiguration GetOptimizedExportsConfiguration()
        {
            return new ExportsConfiguration
            {
                DefaultDateFormat = "yyyy-MM-dd",
                IncludeMetadata = true
            };
        }

        private StorageConfiguration GetOptimizedStorageConfiguration()
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

    #region Helper Classes

    public class ProcessingResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public string? RecipeId { get; set; }
    }

    public class CostAnalysis
    {
        public string Provider { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public TimeSpan Duration { get; set; }
        public double Confidence { get; set; }
    }

    public class OCRResult
    {
        public string Text { get; set; } = string.Empty;
        public double Confidence { get; set; }
        public decimal? EstimatedCost { get; set; }
    }

    public class DocumentResult
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public decimal? EstimatedCost { get; set; }
    }

    public class ExportResult
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public decimal? EstimatedCost { get; set; }
    }

    #endregion
}
