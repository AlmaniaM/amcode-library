using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using NUnit.Framework;

namespace AMCode.Documents.UnitTests.Performance
{
    /// <summary>
    /// Performance tests for recipe processing operations
    /// </summary>
    [TestFixture]
    public class RecipeProcessingPerformanceTests
    {
        private List<string> _createdTestFiles;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Initialize cleanup list
            _createdTestFiles = new List<string>();
        }

        /// <summary>
        /// Teardown method called after each test
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Clean up all created test files
            foreach (var filePath in _createdTestFiles)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
            _createdTestFiles.Clear();
        }

        #region OCR Performance Tests

        /// <summary>
        /// Test OCR processing performance with large images
        /// </summary>
        [Test]
        public void OCRProcessing_ShouldHandleLargeImages()
        {
            // Arrange
            const int iterations = 10;
            var times = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var time = MeasureExecutionTime(() =>
                {
                    // Simulate OCR processing with large image
                    var imageData = GenerateLargeImageData(2048, 1536); // 2MP image
                    var ocrResult = SimulateOCRProcessing(imageData);
                    Assert.IsTrue(ocrResult.IsSuccess, "OCR processing should succeed");
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"OCR Processing Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Image Size: 2048x1536 (2MP)");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 5000, $"Average OCR processing time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 10000, $"Max OCR processing time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test OCR processing performance with multiple images
        /// </summary>
        [Test]
        public void OCRProcessing_ShouldHandleMultipleImages()
        {
            // Arrange
            const int imageCount = 20;
            const int imageWidth = 1024;
            const int imageHeight = 768;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var tasks = new List<Task<Result<string>>>();
                
                for (int i = 0; i < imageCount; i++)
                {
                    var task = Task.Run(() =>
                    {
                        var imageData = GenerateImageData(imageWidth, imageHeight);
                        return SimulateOCRProcessing(imageData);
                    });
                    tasks.Add(task);
                }

                var results = Task.WhenAll(tasks).Result;
                
                // Verify all OCR operations succeeded
                foreach (var result in results)
                {
                    Assert.IsTrue(result.IsSuccess, "All OCR operations should succeed");
                }
            });

            // Assert
            Console.WriteLine($"Multiple Images OCR Performance:");
            Console.WriteLine($"  Image Count: {imageCount}");
            Console.WriteLine($"  Image Size: {imageWidth}x{imageHeight}");
            Console.WriteLine($"  Total Time: {time}ms");
            Console.WriteLine($"  Average per Image: {time / (double)imageCount:F2}ms");

            Assert.IsTrue(time < 30000, $"Multiple images OCR processing time too high: {time}ms");
        }

        #endregion

        #region AI Parsing Performance Tests

        /// <summary>
        /// Test AI parsing performance with complex recipes
        /// </summary>
        [Test]
        public void AIParsing_ShouldHandleComplexRecipes()
        {
            // Arrange
            const int iterations = 10;
            var times = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var time = MeasureExecutionTime(() =>
                {
                    var complexRecipeText = GenerateComplexRecipeText();
                    var parsingResult = SimulateAIParsing(complexRecipeText);
                    Assert.IsTrue(parsingResult.IsSuccess, "AI parsing should succeed");
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"AI Parsing Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Recipe Complexity: High (multiple sections, ingredients, instructions)");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 3000, $"Average AI parsing time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 6000, $"Max AI parsing time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test AI parsing performance with batch processing
        /// </summary>
        [Test]
        public void AIParsing_ShouldHandleBatchProcessing()
        {
            // Arrange
            const int recipeCount = 50;
            var recipeTexts = new List<string>();

            for (int i = 0; i < recipeCount; i++)
            {
                recipeTexts.Add(GenerateRecipeText(i));
            }

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var tasks = new List<Task<Result<object>>>();
                
                foreach (var recipeText in recipeTexts)
                {
                    var task = Task.Run(() => SimulateAIParsing(recipeText));
                    tasks.Add(task);
                }

                var results = Task.WhenAll(tasks).Result;
                
                // Verify all parsing operations succeeded
                foreach (var result in results)
                {
                    Assert.IsTrue(result.IsSuccess, "All AI parsing operations should succeed");
                }
            });

            // Assert
            Console.WriteLine($"Batch AI Parsing Performance:");
            Console.WriteLine($"  Recipe Count: {recipeCount}");
            Console.WriteLine($"  Total Time: {time}ms");
            Console.WriteLine($"  Average per Recipe: {time / (double)recipeCount:F2}ms");

            Assert.IsTrue(time < 60000, $"Batch AI parsing time too high: {time}ms");
        }

        #endregion

        #region Document Export Performance Tests

        /// <summary>
        /// Test document export performance with large recipes
        /// </summary>
        [Test]
        public void DocumentExport_ShouldHandleLargeRecipes()
        {
            // Arrange
            const int iterations = 10;
            var times = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var time = MeasureExecutionTime(() =>
                {
                    var largeRecipe = GenerateLargeRecipe();
                    var exportResult = SimulateDocumentExport(largeRecipe, "xlsx");
                    Assert.IsTrue(exportResult.IsSuccess, "Document export should succeed");
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Document Export Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Recipe Size: Large (100+ ingredients, 50+ instructions)");
            Console.WriteLine($"  Format: Excel");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 2000, $"Average document export time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 4000, $"Max document export time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test document export performance across multiple formats
        /// </summary>
        [Test]
        public void DocumentExport_ShouldHandleMultipleFormats()
        {
            // Arrange
            var recipe = GenerateMediumRecipe();
            var formats = new[] { "xlsx", "docx", "pdf" };
            var times = new Dictionary<string, long>();

            // Act
            foreach (var format in formats)
            {
                var time = MeasureExecutionTime(() =>
                {
                    var exportResult = SimulateDocumentExport(recipe, format);
                    Assert.IsTrue(exportResult.IsSuccess, $"Document export to {format} should succeed");
                });
                times[format] = time;
            }

            // Assert
            Console.WriteLine($"Multi-Format Export Performance:");
            Console.WriteLine($"  Recipe Size: Medium (50 ingredients, 25 instructions)");
            foreach (var kvp in times)
            {
                Console.WriteLine($"  {kvp.Key.ToUpper()}: {kvp.Value}ms");
                Assert.IsTrue(kvp.Value < 3000, $"{kvp.Key} export time too high: {kvp.Value}ms");
            }
        }

        #endregion

        #region Concurrent Processing Tests

        /// <summary>
        /// Test concurrent recipe processing performance
        /// </summary>
        [Test]
        public async Task ConcurrentProcessing_ShouldHandleMultipleRecipes()
        {
            // Arrange
            const int concurrentTasks = 10;
            const int operationsPerTask = 5;

            // Act
            var tasks = new List<Task<long>>();
            for (int i = 0; i < concurrentTasks; i++)
            {
                var taskIndex = i;
                var task = Task.Run(() =>
                {
                    return MeasureExecutionTime(() =>
                    {
                        for (int j = 0; j < operationsPerTask; j++)
                        {
                            // Simulate full recipe processing pipeline
                            var imageData = GenerateImageData(1024, 768);
                            var ocrResult = SimulateOCRProcessing(imageData);
                            Assert.IsTrue(ocrResult.IsSuccess, "OCR should succeed");

                            var parsingResult = SimulateAIParsing(ocrResult.Value);
                            Assert.IsTrue(parsingResult.IsSuccess, "AI parsing should succeed");

                            var exportResult = SimulateDocumentExport(parsingResult.Value, "xlsx");
                            Assert.IsTrue(exportResult.IsSuccess, "Document export should succeed");
                        }
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Concurrent Recipe Processing Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 15000, $"Average concurrent processing time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 30000, $"Max concurrent processing time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test concurrent document export performance
        /// </summary>
        [Test]
        public async Task ConcurrentExport_ShouldHandleMultipleExports()
        {
            // Arrange
            const int concurrentTasks = 20;
            var recipes = new List<object>();
            
            for (int i = 0; i < concurrentTasks; i++)
            {
                recipes.Add(GenerateMediumRecipe());
            }

            // Act
            var tasks = new List<Task<long>>();
            for (int i = 0; i < concurrentTasks; i++)
            {
                var taskIndex = i;
                var task = Task.Run(() =>
                {
                    return MeasureExecutionTime(() =>
                    {
                        var exportResult = SimulateDocumentExport(recipes[taskIndex], "xlsx");
                        Assert.IsTrue(exportResult.IsSuccess, "Document export should succeed");
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Concurrent Document Export Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 2000, $"Average concurrent export time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 5000, $"Max concurrent export time too high: {maxTime}ms");
        }

        #endregion

        #region Memory Usage Tests

        /// <summary>
        /// Test memory usage during recipe processing
        /// </summary>
        [Test]
        public void RecipeProcessing_ShouldHaveReasonableMemoryUsage()
        {
            // Arrange
            const int iterations = 20;
            var memoryUsages = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var memoryUsage = MeasureMemoryUsage(() =>
                {
                    // Simulate full recipe processing pipeline
                    var imageData = GenerateImageData(1024, 768);
                    var ocrResult = SimulateOCRProcessing(imageData);
                    var parsingResult = SimulateAIParsing(ocrResult.Value);
                    var exportResult = SimulateDocumentExport(parsingResult.Value, "xlsx");
                    
                    Assert.IsTrue(ocrResult.IsSuccess, "OCR should succeed");
                    Assert.IsTrue(parsingResult.IsSuccess, "AI parsing should succeed");
                    Assert.IsTrue(exportResult.IsSuccess, "Document export should succeed");
                });
                memoryUsages.Add(memoryUsage);
            }

            // Assert
            var averageMemory = memoryUsages.Average();
            var maxMemory = memoryUsages.Max();

            Console.WriteLine($"Recipe Processing Memory Usage:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");

            Assert.IsTrue(averageMemory < 10 * 1024 * 1024, $"Average memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 20 * 1024 * 1024, $"Max memory usage too high: {maxMemory / 1024:F2}KB");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a temporary file path for testing
        /// </summary>
        /// <param name="extension">File extension</param>
        /// <returns>Path to temporary file</returns>
        private string CreateTempFilePath(string extension)
        {
            var fileName = $"test_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            _createdTestFiles.Add(filePath);
            return filePath;
        }

        /// <summary>
        /// Generates large image data for testing
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <returns>Image data</returns>
        private byte[] GenerateLargeImageData(int width, int height)
        {
            // Simulate large image data (3 bytes per pixel for RGB)
            var dataSize = width * height * 3;
            var data = new byte[dataSize];
            var random = new Random();
            random.NextBytes(data);
            return data;
        }

        /// <summary>
        /// Generates image data for testing
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <returns>Image data</returns>
        private byte[] GenerateImageData(int width, int height)
        {
            // Simulate image data (3 bytes per pixel for RGB)
            var dataSize = width * height * 3;
            var data = new byte[dataSize];
            var random = new Random();
            random.NextBytes(data);
            return data;
        }

        /// <summary>
        /// Generates complex recipe text for testing
        /// </summary>
        /// <returns>Complex recipe text</returns>
        private string GenerateComplexRecipeText()
        {
            return @"
                RECIPE TITLE: Complex Multi-Course Meal
                
                INGREDIENTS:
                For the main course:
                - 2 lbs beef tenderloin, trimmed
                - 1/4 cup olive oil
                - 4 cloves garlic, minced
                - 2 sprigs fresh rosemary
                - Salt and pepper to taste
                
                For the side dish:
                - 1 lb baby potatoes, halved
                - 2 tbsp butter
                - 1 tbsp fresh thyme
                - 1/2 cup heavy cream
                
                For the sauce:
                - 1 cup red wine
                - 2 cups beef stock
                - 1 shallot, minced
                - 2 tbsp butter
                - 1 tbsp flour
                
                INSTRUCTIONS:
                1. Preheat oven to 400°F
                2. Season beef with salt, pepper, and garlic
                3. Heat oil in large skillet over medium-high heat
                4. Sear beef on all sides until golden brown
                5. Transfer to oven and roast for 15-20 minutes
                6. Let rest for 10 minutes before slicing
                7. Meanwhile, prepare potatoes by boiling until tender
                8. Drain and toss with butter, thyme, and cream
                9. For the sauce, reduce wine by half
                10. Add stock and shallot, simmer until thickened
                11. Whisk in butter and flour to finish
                12. Serve beef with potatoes and sauce
                
                COOKING TIME: 45 minutes
                SERVINGS: 4
                DIFFICULTY: Intermediate
            ";
        }

        /// <summary>
        /// Generates recipe text for testing
        /// </summary>
        /// <param name="index">Recipe index</param>
        /// <returns>Recipe text</returns>
        private string GenerateRecipeText(int index)
        {
            return $@"
                RECIPE TITLE: Test Recipe {index}
                
                INGREDIENTS:
                - 2 cups flour
                - 1 cup sugar
                - 3 eggs
                - 1/2 cup butter
                - 1 tsp vanilla
                
                INSTRUCTIONS:
                1. Mix dry ingredients
                2. Beat wet ingredients
                3. Combine and bake at 350°F for 30 minutes
                
                SERVINGS: 8
            ";
        }

        /// <summary>
        /// Generates large recipe data for testing
        /// </summary>
        /// <returns>Large recipe data</returns>
        private object GenerateLargeRecipe()
        {
            return new
            {
                Title = "Large Test Recipe",
                Ingredients = Enumerable.Range(1, 100).Select(i => $"Ingredient {i}").ToList(),
                Instructions = Enumerable.Range(1, 50).Select(i => $"Step {i}: Do something").ToList(),
                Servings = 20,
                CookingTime = 120
            };
        }

        /// <summary>
        /// Generates medium recipe data for testing
        /// </summary>
        /// <returns>Medium recipe data</returns>
        private object GenerateMediumRecipe()
        {
            return new
            {
                Title = "Medium Test Recipe",
                Ingredients = Enumerable.Range(1, 50).Select(i => $"Ingredient {i}").ToList(),
                Instructions = Enumerable.Range(1, 25).Select(i => $"Step {i}: Do something").ToList(),
                Servings = 10,
                CookingTime = 60
            };
        }

        /// <summary>
        /// Simulates OCR processing
        /// </summary>
        /// <param name="imageData">Image data</param>
        /// <returns>OCR result</returns>
        private Result<string> SimulateOCRProcessing(byte[] imageData)
        {
            // Simulate OCR processing time based on image size
            var processingTime = imageData.Length / 1000; // 1ms per KB
            System.Threading.Thread.Sleep(Math.Min(processingTime, 100)); // Cap at 100ms
            
            return Result<string>.Success("Simulated OCR text output");
        }

        /// <summary>
        /// Simulates AI parsing
        /// </summary>
        /// <param name="text">Text to parse</param>
        /// <returns>Parsing result</returns>
        private Result<object> SimulateAIParsing(string text)
        {
            // Simulate AI parsing time based on text length
            var processingTime = text.Length / 100; // 1ms per 100 characters
            System.Threading.Thread.Sleep(Math.Min(processingTime, 50)); // Cap at 50ms
            
            return Result<object>.Success(new { Parsed = true, Text = text });
        }

        /// <summary>
        /// Simulates document export
        /// </summary>
        /// <param name="recipe">Recipe data</param>
        /// <param name="format">Export format</param>
        /// <returns>Export result</returns>
        private Result<string> SimulateDocumentExport(object recipe, string format)
        {
            // Simulate document export time
            System.Threading.Thread.Sleep(100); // 100ms base time
            
            var filePath = CreateTempFilePath($".{format}");
            return Result<string>.Success(filePath);
        }

        /// <summary>
        /// Measures execution time of an action
        /// </summary>
        /// <param name="action">Action to measure</param>
        /// <returns>Execution time in milliseconds</returns>
        private long MeasureExecutionTime(Action action)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Measures memory usage before and after an action
        /// </summary>
        /// <param name="action">Action to measure</param>
        /// <returns>Memory usage difference in bytes</returns>
        private long MeasureMemoryUsage(Action action)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var beforeMemory = GC.GetTotalMemory(false);
            action();
            var afterMemory = GC.GetTotalMemory(false);

            return afterMemory - beforeMemory;
        }

        #endregion
    }
}
