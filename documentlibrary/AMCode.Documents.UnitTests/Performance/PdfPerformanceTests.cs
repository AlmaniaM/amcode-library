using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Pdf.Domain.Interfaces;
using AMCode.Documents.Pdf.Infrastructure.Factories;
using AMCode.Documents.Pdf.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.UnitTests.Performance
{
    /// <summary>
    /// Performance tests for PDF document generation operations
    /// </summary>
    [TestFixture]
    public class PdfPerformanceTests
    {
        private IPdfFactory _pdfFactory;
        private List<string> _createdTestFiles;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Initialize cleanup list
            _createdTestFiles = new List<string>();

            // Create real implementations for performance testing
            _pdfFactory = new PdfFactory();
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

        #region Large PDF File Generation Tests

        /// <summary>
        /// Test large PDF file generation performance
        /// </summary>
        [Test]
        public void ShouldGenerateLargeDocument()
        {
            // Arrange
            const int pages = 100;
            const int elementsPerPage = 50;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _pdfFactory.CreateDocument("Large PDF");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create many pages with content
                for (int pageIndex = 0; pageIndex < pages; pageIndex++)
                {
                    var addPageResult = document.AddPage();
                    AssertResultSuccess(addPageResult);
                    var page = addPageResult.Value;

                    // Add multiple elements to each page
                    for (int elementIndex = 0; elementIndex < elementsPerPage; elementIndex++)
                    {
                        var addTextResult = page.AddText($"Page {pageIndex + 1}, Element {elementIndex + 1}", 10, 10 + elementIndex * 15, 12);
                        AssertResultSuccess(addTextResult);
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large PDF Generation Performance:");
            Console.WriteLine($"  Pages: {pages}");
            Console.WriteLine($"  Elements per Page: {elementsPerPage}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 30000, $"Large PDF generation time too high: {time}ms");
        }

        /// <summary>
        /// Test handling many pages performance
        /// </summary>
        [Test]
        public void ShouldHandleManyPages()
        {
            // Arrange
            const int pages = 200;
            const int elementsPerPage = 20;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _pdfFactory.CreateDocument("Many Pages");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create many pages
                for (int i = 0; i < pages; i++)
                {
                    var addPageResult = document.AddPage();
                    AssertResultSuccess(addPageResult);
                    var page = addPageResult.Value;

                    // Add some content to each page
                    for (int j = 0; j < elementsPerPage; j++)
                    {
                        var addTextResult = page.AddText($"Page {i + 1}, Text {j + 1}", 10, 10 + j * 20, 10);
                        AssertResultSuccess(addTextResult);
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Many Pages Performance:");
            Console.WriteLine($"  Pages: {pages}");
            Console.WriteLine($"  Elements per Page: {elementsPerPage}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 25000, $"Many pages creation time too high: {time}ms");
        }

        /// <summary>
        /// Test processing large content performance
        /// </summary>
        [Test]
        public void ShouldProcessLargeContent()
        {
            // Arrange
            const int pages = 50;
            const int elementsPerPage = 100;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _pdfFactory.CreateDocument("Large Content");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create pages with large content
                for (int pageIndex = 0; pageIndex < pages; pageIndex++)
                {
                    var addPageResult = document.AddPage();
                    AssertResultSuccess(addPageResult);
                    var page = addPageResult.Value;

                    // Add many elements to each page
                    for (int elementIndex = 0; elementIndex < elementsPerPage; elementIndex++)
                    {
                        var addTextResult = page.AddText($"Content {pageIndex + 1}_{elementIndex + 1}", 10, 10 + elementIndex * 12, 10);
                        AssertResultSuccess(addTextResult);

                        // Add some shapes every 10th element
                        if (elementIndex % 10 == 0)
                        {
                            var addRectangleResult = page.AddRectangle(10, 10 + elementIndex * 12, 100, 10);
                            AssertResultSuccess(addRectangleResult);
                        }
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Content Processing Performance:");
            Console.WriteLine($"  Pages: {pages}");
            Console.WriteLine($"  Elements per Page: {elementsPerPage}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 20000, $"Large content processing time too high: {time}ms");
        }

        /// <summary>
        /// Test maintaining performance with complex layouts
        /// </summary>
        [Test]
        public void ShouldMaintainPerformanceWithComplexLayouts()
        {
            // Arrange
            const int pages = 30;
            const int elementsPerPage = 50;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _pdfFactory.CreateDocument("Complex Layout");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create pages with complex layouts
                for (int pageIndex = 0; pageIndex < pages; pageIndex++)
                {
                    var addPageResult = document.AddPage();
                    AssertResultSuccess(addPageResult);
                    var page = addPageResult.Value;

                    // Add various types of content
                    for (int elementIndex = 0; elementIndex < elementsPerPage; elementIndex++)
                    {
                        var x = 10 + (elementIndex % 10) * 50;
                        var y = 10 + (elementIndex / 10) * 20;

                        // Add text
                        var addTextResult = page.AddText($"Text {elementIndex}", x, y, 10);
                        AssertResultSuccess(addTextResult);

                        // Add rectangles
                        var addRectangleResult = page.AddRectangle(x, y + 10, 40, 5);
                        AssertResultSuccess(addRectangleResult);

                        // Add circles every 5th element
                        if (elementIndex % 5 == 0)
                        {
                            var addCircleResult = page.AddCircle(x + 20, y + 15, 5);
                            AssertResultSuccess(addCircleResult);
                        }
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Complex Layout Performance:");
            Console.WriteLine($"  Pages: {pages}");
            Console.WriteLine($"  Elements per Page: {elementsPerPage}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 15000, $"Complex layout generation time too high: {time}ms");
        }

        #endregion

        #region Memory Usage Tests

        /// <summary>
        /// Test memory consumption during generation
        /// </summary>
        [Test]
        public void ShouldHaveReasonableMemoryUsage()
        {
            // Arrange
            const int iterations = 10;
            var memoryUsages = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var memoryUsage = MeasureMemoryUsage(() =>
                {
                    var documentResult = _pdfFactory.CreateDocument($"MemoryTest{i}");
                    AssertResultSuccess(documentResult);
                    var document = documentResult.Value;

                    // Add some content
                    var addPageResult = document.AddPage();
                    AssertResultSuccess(addPageResult);
                    var page = addPageResult.Value;

                    var addTextResult = page.AddText($"Test content {i}", 10, 10, 12);
                    AssertResultSuccess(addTextResult);

                    // Save to file
                    var filePath = CreateTempFilePath();
                    var saveResult = document.SaveAs(filePath);
                    AssertResultSuccess(saveResult);

                    document.Dispose();
                });
                memoryUsages.Add(memoryUsage);
            }

            // Assert
            var averageMemory = memoryUsages.Average();
            var maxMemory = memoryUsages.Max();

            Console.WriteLine($"Memory Usage Test:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");

            Assert.IsTrue(averageMemory < 5 * 1024 * 1024, $"Average memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 10 * 1024 * 1024, $"Max memory usage too high: {maxMemory / 1024:F2}KB");
        }

        /// <summary>
        /// Test memory usage with large documents
        /// </summary>
        [Test]
        public void ShouldHandleLargeDocumentMemoryUsage()
        {
            // Arrange
            const int pages = 100;
            const int elementsPerPage = 100;

            // Act
            var memoryUsage = MeasureMemoryUsage(() =>
            {
                var documentResult = _pdfFactory.CreateDocument("Large Memory Test");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Add large content
                for (int pageIndex = 0; pageIndex < pages; pageIndex++)
                {
                    var addPageResult = document.AddPage();
                    AssertResultSuccess(addPageResult);
                    var page = addPageResult.Value;

                    for (int elementIndex = 0; elementIndex < elementsPerPage; elementIndex++)
                    {
                        var addTextResult = page.AddText($"Content {pageIndex}_{elementIndex}", 10, 10 + elementIndex * 10, 10);
                        AssertResultSuccess(addTextResult);
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Document Memory Usage:");
            Console.WriteLine($"  Pages: {pages}");
            Console.WriteLine($"  Elements per Page: {elementsPerPage}");
            Console.WriteLine($"  Memory: {memoryUsage / 1024:F2}KB");

            Assert.IsTrue(memoryUsage < 30 * 1024 * 1024, $"Large document memory usage too high: {memoryUsage / 1024:F2}KB");
        }

        #endregion

        #region Concurrent Operations Tests

        /// <summary>
        /// Test concurrent document creation performance
        /// </summary>
        [Test]
        public async Task ShouldHandleConcurrentDocumentCreation()
        {
            // Arrange
            const int concurrentTasks = 5;
            const int operationsPerTask = 10;

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
                            var documentResult = _pdfFactory.CreateDocument($"Concurrent{taskIndex}_{j}");
                            AssertResultSuccess(documentResult);
                            var document = documentResult.Value;

                            // Add some content
                            var addPageResult = document.AddPage();
                            AssertResultSuccess(addPageResult);
                            var page = addPageResult.Value;

                            var addTextResult = page.AddText($"Test content {taskIndex}_{j}", 10, 10, 12);
                            AssertResultSuccess(addTextResult);

                            // Save to file
                            var filePath = CreateTempFilePath();
                            var saveResult = document.SaveAs(filePath);
                            AssertResultSuccess(saveResult);

                            document.Dispose();
                        }
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Concurrent Document Creation Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 4000, $"Average concurrent time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 8000, $"Max concurrent time too high: {maxTime}ms");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a temporary file path for testing
        /// </summary>
        /// <param name="extension">File extension (default: .pdf)</param>
        /// <returns>Path to temporary file</returns>
        private string CreateTempFilePath(string extension = ".pdf")
        {
            var fileName = $"test_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            _createdTestFiles.Add(filePath);
            return filePath;
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

        /// <summary>
        /// Validates that a Result is successful
        /// </summary>
        /// <typeparam name="T">Result value type</typeparam>
        /// <param name="result">Result to validate</param>
        private void AssertResultSuccess<T>(Result<T> result)
        {
            Assert.IsTrue(result.IsSuccess, $"Expected success but got failure: {result.Error}");
        }

        #endregion
    }
}
