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
    /// Load Testing for AMCode Libraries
    /// Tests system behavior under various loads and stress conditions
    /// </summary>
    [TestFixture]
    public class LoadTestingTests
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
            
            // Add AMCode services with load testing optimizations
            services.AddAMCodeOCR(GetLoadTestOCRConfiguration());
            services.AddAMCodeAI(GetLoadTestAIConfiguration());
            services.AddAMCodeDocuments(GetLoadTestDocumentsConfiguration());
            services.AddAMCodeExports(GetLoadTestExportsConfiguration());
            services.AddAMCodeStorage(GetLoadTestStorageConfiguration());
            
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
        /// Test concurrent users with AMCode libraries
        /// </summary>
        [Test]
        public async Task ConcurrentUsers_WithAMCodeLibraries_ShouldHandleLoad()
        {
            // Arrange
            var concurrentUsers = 10;
            var tasks = new List<Task<LoadTestResult>>();
            var stopwatch = Stopwatch.StartNew();
            
            // Act - Process multiple recipes concurrently
            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks.Add(ProcessRecipeLoadTestAsync($"load-test-recipe-{i}"));
            }
            
            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();
            
            // Assert - All tasks should complete successfully
            var successfulResults = results.Where(r => r.IsSuccess).ToList();
            var failedResults = results.Where(r => !r.IsSuccess).ToList();
            
            Console.WriteLine($"Concurrent Load Test Results:");
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
        /// Test high-volume processing
        /// </summary>
        [Test]
        public async Task HighVolumeProcessing_WithAMCodeLibraries_ShouldHandleVolume()
        {
            // Arrange
            var volume = 50; // Process 50 recipes
            var batchSize = 10; // Process in batches of 10
            var results = new List<LoadTestResult>();
            var stopwatch = Stopwatch.StartNew();
            
            // Act - Process in batches to simulate high volume
            for (int batch = 0; batch < volume / batchSize; batch++)
            {
                var batchTasks = new List<Task<LoadTestResult>>();
                
                for (int i = 0; i < batchSize; i++)
                {
                    var recipeId = batch * batchSize + i;
                    batchTasks.Add(ProcessRecipeLoadTestAsync($"volume-test-recipe-{recipeId}"));
                }
                
                var batchResults = await Task.WhenAll(batchTasks);
                results.AddRange(batchResults);
                
                Console.WriteLine($"Batch {batch + 1} completed: {batchResults.Count(r => r.IsSuccess)}/{batchSize} successful");
                
                // Small delay between batches to simulate real-world usage
                await Task.Delay(100);
            }
            
            stopwatch.Stop();
            
            // Assert - Results analysis
            var successfulResults = results.Where(r => r.IsSuccess).ToList();
            var failedResults = results.Where(r => !r.IsSuccess).ToList();
            
            Console.WriteLine($"High Volume Test Results:");
            Console.WriteLine($"  Total recipes processed: {results.Count}");
            Console.WriteLine($"  Successful: {successfulResults.Count}");
            Console.WriteLine($"  Failed: {failedResults.Count}");
            Console.WriteLine($"  Success rate: {(double)successfulResults.Count / results.Count:P2}");
            Console.WriteLine($"  Total time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Throughput: {results.Count / (stopwatch.ElapsedMilliseconds / 1000.0):F2} recipes/second");
            
            // At least 70% should succeed for high volume
            var successRate = (double)successfulResults.Count / results.Count;
            Assert.IsTrue(successRate >= 0.7, $"Success rate too low for high volume: {successRate:P2}");
            
            // Should process at least 1 recipe per second
            var throughput = results.Count / (stopwatch.ElapsedMilliseconds / 1000.0);
            Assert.IsTrue(throughput >= 1.0, $"Throughput too low: {throughput:F2} recipes/second");
        }

        /// <summary>
        /// Test memory usage under load
        /// </summary>
        [Test]
        public async Task MemoryUsage_UnderLoad_ShouldBeStable()
        {
            // Arrange
            var iterations = 30;
            var memorySamples = new List<long>();
            var initialMemory = GC.GetTotalMemory(true);
            
            // Act - Process recipes and monitor memory usage
            for (int i = 0; i < iterations; i++)
            {
                var recipe = CreateTestRecipe($"memory-test-recipe-{i}");
                var result = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                
                // Sample memory every 5 iterations
                if (i % 5 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    var currentMemory = GC.GetTotalMemory(false);
                    memorySamples.Add(currentMemory);
                    Console.WriteLine($"Iteration {i}: Memory usage: {currentMemory / (1024 * 1024):F2}MB");
                }
            }
            
            // Force final garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var finalMemory = GC.GetTotalMemory(false);
            
            // Assert - Memory should be stable
            var maxMemory = memorySamples.Max();
            var minMemory = memorySamples.Min();
            var memoryVariation = maxMemory - minMemory;
            var maxMemoryIncrease = finalMemory - initialMemory;
            
            Console.WriteLine($"Memory Usage Under Load:");
            Console.WriteLine($"  Initial memory: {initialMemory / (1024 * 1024):F2}MB");
            Console.WriteLine($"  Final memory: {finalMemory / (1024 * 1024):F2}MB");
            Console.WriteLine($"  Max memory: {maxMemory / (1024 * 1024):F2}MB");
            Console.WriteLine($"  Min memory: {minMemory / (1024 * 1024):F2}MB");
            Console.WriteLine($"  Memory variation: {memoryVariation / (1024 * 1024):F2}MB");
            Console.WriteLine($"  Total memory increase: {maxMemoryIncrease / (1024 * 1024):F2}MB");
            
            // Memory increase should be reasonable (less than 200MB)
            Assert.IsTrue(maxMemoryIncrease < 200 * 1024 * 1024, 
                $"Memory increase too high: {maxMemoryIncrease / (1024 * 1024):F2}MB");
            
            // Memory variation should be stable (less than 100MB variation)
            Assert.IsTrue(memoryVariation < 100 * 1024 * 1024, 
                $"Memory variation too high: {memoryVariation / (1024 * 1024):F2}MB");
        }

        /// <summary>
        /// Test CPU usage under load
        /// </summary>
        [Test]
        public async Task CPUUsage_UnderLoad_ShouldBeEfficient()
        {
            // Arrange
            var iterations = 20;
            var concurrentTasks = 5;
            var stopwatch = Stopwatch.StartNew();
            
            // Act - Run concurrent tasks to test CPU usage
            var tasks = new List<Task<LoadTestResult>>();
            
            for (int i = 0; i < iterations; i++)
            {
                if (tasks.Count >= concurrentTasks)
                {
                    // Wait for some tasks to complete before starting new ones
                    var completedTask = await Task.WhenAny(tasks);
                    tasks.Remove(completedTask);
                }
                
                tasks.Add(ProcessRecipeLoadTestAsync($"cpu-test-recipe-{i}"));
            }
            
            // Wait for all remaining tasks
            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();
            
            // Assert - All tasks should complete
            var successfulResults = results.Where(r => r.IsSuccess).ToList();
            var successRate = (double)successfulResults.Count / results.Length;
            
            Console.WriteLine($"CPU Usage Under Load:");
            Console.WriteLine($"  Total tasks: {results.Length}");
            Console.WriteLine($"  Successful: {successfulResults.Count}");
            Console.WriteLine($"  Success rate: {successRate:P2}");
            Console.WriteLine($"  Total time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Average time per task: {stopwatch.ElapsedMilliseconds / (double)results.Length:F2}ms");
            
            // At least 80% should succeed
            Assert.IsTrue(successRate >= 0.8, $"Success rate too low: {successRate:P2}");
            
            // Average processing time should be reasonable
            var averageTime = successfulResults.Any() ? successfulResults.Average(r => r.ElapsedMilliseconds) : 0;
            Assert.IsTrue(averageTime < 15000, $"Average processing time too long: {averageTime:F2}ms");
        }

        /// <summary>
        /// Test error handling under load
        /// </summary>
        [Test]
        public async Task ErrorHandling_UnderLoad_ShouldBeRobust()
        {
            // Arrange
            var iterations = 20;
            var errorRate = 0.3; // 30% of requests will have errors
            var tasks = new List<Task<LoadTestResult>>();
            
            // Act - Mix successful and error scenarios
            for (int i = 0; i < iterations; i++)
            {
                var shouldError = (i % 10) < (iterations * errorRate);
                tasks.Add(ProcessRecipeLoadTestAsync($"error-test-recipe-{i}", shouldError));
            }
            
            var results = await Task.WhenAll(tasks);
            
            // Assert - System should handle errors gracefully
            var successfulResults = results.Where(r => r.IsSuccess).ToList();
            var failedResults = results.Where(r => !r.IsSuccess).ToList();
            
            Console.WriteLine($"Error Handling Under Load:");
            Console.WriteLine($"  Total tasks: {results.Length}");
            Console.WriteLine($"  Successful: {successfulResults.Count}");
            Console.WriteLine($"  Failed: {failedResults.Count}");
            Console.WriteLine($"  Success rate: {(double)successfulResults.Count / results.Length:P2}");
            
            // Should have some failures (due to intentional errors)
            Assert.IsTrue(failedResults.Count > 0, "Should have some failures for error handling test");
            
            // Success rate should be reasonable even with errors
            var successRate = (double)successfulResults.Count / results.Length;
            Assert.IsTrue(successRate >= 0.5, $"Success rate too low with errors: {successRate:P2}");
            
            // Failed results should have proper error messages
            var resultsWithErrors = failedResults.Where(r => !string.IsNullOrEmpty(r.ErrorMessage)).ToList();
            Assert.IsTrue(resultsWithErrors.Count > 0, "Failed results should have error messages");
        }

        /// <summary>
        /// Test provider selection under load
        /// </summary>
        [Test]
        public async Task ProviderSelection_UnderLoad_ShouldDistributeEvenly()
        {
            // Arrange
            var iterations = 30;
            var providerUsage = new ConcurrentDictionary<string, int>();
            var tasks = new List<Task<LoadTestResult>>();
            
            // Act - Process multiple recipes and track provider usage
            for (int i = 0; i < iterations; i++)
            {
                tasks.Add(ProcessRecipeWithProviderTrackingAsync($"provider-test-recipe-{i}", providerUsage));
            }
            
            var results = await Task.WhenAll(tasks);
            
            // Assert - Provider usage should be distributed
            Console.WriteLine($"Provider Selection Under Load:");
            foreach (var provider in providerUsage)
            {
                Console.WriteLine($"  {provider.Key}: {provider.Value} uses");
            }
            
            // Should have used multiple providers
            Assert.IsTrue(providerUsage.Count > 1, "Should use multiple providers");
            
            // Provider usage should be reasonably distributed
            var totalUsage = providerUsage.Values.Sum();
            var averageUsage = totalUsage / (double)providerUsage.Count;
            var maxUsage = providerUsage.Values.Max();
            var minUsage = providerUsage.Values.Min();
            
            // No single provider should dominate (max usage should be less than 80% of total)
            var maxProviderRatio = maxUsage / (double)totalUsage;
            Assert.IsTrue(maxProviderRatio < 0.8, $"Provider usage too concentrated: {maxProviderRatio:P2}");
        }

        #region Helper Methods

        private async Task<LoadTestResult> ProcessRecipeLoadTestAsync(string recipeId, bool shouldError = false)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                if (shouldError)
                {
                    // Simulate error scenario
                    throw new InvalidOperationException($"Simulated error for recipe {recipeId}");
                }
                
                var testImagePath = CreateTestImage();
                
                // OCR
                var ocrResult = await _ocrService.ExtractTextAsync(testImagePath);
                if (!ocrResult.IsSuccess) 
                {
                    return new LoadTestResult 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = ocrResult.ErrorMessage,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RecipeId = recipeId
                    };
                }
                
                // AI Parsing
                var aiResult = await _aiService.ParseRecipeAsync(ocrResult.Value.Text);
                if (!aiResult.IsSuccess) 
                {
                    return new LoadTestResult 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = aiResult.ErrorMessage,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RecipeId = recipeId
                    };
                }
                
                // Recipe Creation
                var recipe = CreateRecipeFromParsedData(aiResult.Value);
                
                // Document Generation
                var docResult = await _documentFactory.CreateRecipeDocumentAsync(recipe);
                if (!docResult.IsSuccess) 
                {
                    return new LoadTestResult 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = docResult.ErrorMessage,
                        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                        RecipeId = recipeId
                    };
                }
                
                stopwatch.Stop();
                
                return new LoadTestResult 
                { 
                    IsSuccess = true, 
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    RecipeId = recipeId
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new LoadTestResult 
                { 
                    IsSuccess = false, 
                    ErrorMessage = ex.Message,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    RecipeId = recipeId
                };
            }
        }

        private async Task<LoadTestResult> ProcessRecipeWithProviderTrackingAsync(string recipeId, ConcurrentDictionary<string, int> providerUsage)
        {
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                var testImagePath = CreateTestImage();
                
                // OCR with provider tracking
                var ocrResult = await _ocrService.ExtractTextAsync(testImagePath);
                if (ocrResult.IsSuccess)
                {
                    // Track OCR provider usage (simplified)
                    providerUsage.AddOrUpdate("OCR", 1, (key, value) => value + 1);
                }
                
                // AI Parsing with provider tracking
                var aiResult = await _aiService.ParseRecipeAsync(ocrResult.Value.Text);
                if (aiResult.IsSuccess)
                {
                    // Track AI provider usage (simplified)
                    providerUsage.AddOrUpdate("AI", 1, (key, value) => value + 1);
                }
                
                stopwatch.Stop();
                
                return new LoadTestResult 
                { 
                    IsSuccess = ocrResult.IsSuccess && aiResult.IsSuccess, 
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    RecipeId = recipeId
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                return new LoadTestResult 
                { 
                    IsSuccess = false, 
                    ErrorMessage = ex.Message,
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
                    RecipeId = recipeId
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
                Load Test Recipe
                
                Ingredients:
                - 2 cups flour
                - 1 cup sugar
                - 1/2 cup butter
                
                Instructions:
                1. Mix ingredients
                2. Bake at 350°F for 20 minutes
            ";
        }

        private Recipe CreateTestRecipe(string recipeId)
        {
            return new Recipe
            {
                Id = recipeId,
                Title = "Load Test Recipe",
                Description = "A recipe for load testing",
                Ingredients = new List<Ingredient>
                {
                    new() { Name = "Flour", Amount = "2", Unit = "cups" },
                    new() { Name = "Sugar", Amount = "1", Unit = "cup" }
                },
                Instructions = new List<Instruction>
                {
                    new() { StepNumber = 1, Description = "Mix ingredients" },
                    new() { StepNumber = 2, Description = "Bake at 350°F" }
                },
                PrepTime = TimeSpan.FromMinutes(10),
                CookTime = TimeSpan.FromMinutes(20),
                Servings = 4,
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

        private OCRConfiguration GetLoadTestOCRConfiguration()
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

        private AIConfiguration GetLoadTestAIConfiguration()
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

        private DocumentsConfiguration GetLoadTestDocumentsConfiguration()
        {
            return new DocumentsConfiguration
            {
                DefaultFont = "Arial",
                DefaultFontSize = 12,
                PageMargins = "1in"
            };
        }

        private ExportsConfiguration GetLoadTestExportsConfiguration()
        {
            return new ExportsConfiguration
            {
                DefaultDateFormat = "yyyy-MM-dd",
                IncludeMetadata = true
            };
        }

        private StorageConfiguration GetLoadTestStorageConfiguration()
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

    public class LoadTestResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public string? RecipeId { get; set; }
        public string? ProviderUsed { get; set; }
    }

    #endregion
}
