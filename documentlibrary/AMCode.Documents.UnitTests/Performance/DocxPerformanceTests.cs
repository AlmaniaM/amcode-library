using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Docx.Domain.Interfaces;
using AMCode.Documents.Docx.Infrastructure.Factories;
using AMCode.Documents.Docx.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.UnitTests.Performance
{
    /// <summary>
    /// Performance tests for DOCX document generation operations
    /// </summary>
    [TestFixture]
    public class DocxPerformanceTests
    {
        private IDocumentFactory _documentFactory;
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
            _documentFactory = new DocumentFactory();
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

        #region Large DOCX File Generation Tests

        /// <summary>
        /// Test large DOCX file generation performance
        /// </summary>
        [Test]
        public void ShouldGenerateLargeDocument()
        {
            // Arrange
            const int paragraphs = 1000;
            const int wordsPerParagraph = 50;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _documentFactory.CreateDocument("Large Document");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create many paragraphs with text
                for (int i = 0; i < paragraphs; i++)
                {
                    var paragraphResult = document.AddParagraph();
                    AssertResultSuccess(paragraphResult);
                    var paragraph = paragraphResult.Value;

                    // Add text to paragraph
                    var text = GenerateText(wordsPerParagraph);
                    var addTextResult = paragraph.AddText(text);
                    AssertResultSuccess(addTextResult);
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Document Generation Performance:");
            Console.WriteLine($"  Paragraphs: {paragraphs}");
            Console.WriteLine($"  Words per Paragraph: {wordsPerParagraph}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 30000, $"Large document generation time too high: {time}ms");
        }

        /// <summary>
        /// Test handling many tables performance
        /// </summary>
        [Test]
        public void ShouldHandleManyTables()
        {
            // Arrange
            const int tables = 100;
            const int rowsPerTable = 10;
            const int columnsPerTable = 5;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _documentFactory.CreateDocument("Many Tables");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create many tables
                for (int i = 0; i < tables; i++)
                {
                    var tableResult = document.AddTable(rowsPerTable, columnsPerTable);
                    AssertResultSuccess(tableResult);
                    var table = tableResult.Value;

                    // Fill table with data
                    for (int row = 0; row < rowsPerTable; row++)
                    {
                        for (int col = 0; col < columnsPerTable; col++)
                        {
                            var cellResult = table.GetCell(row, col);
                            AssertResultSuccess(cellResult);
                            var cell = cellResult.Value;

                            var addTextResult = cell.AddText($"Table{i}_R{row}_C{col}");
                            AssertResultSuccess(addTextResult);
                        }
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Many Tables Performance:");
            Console.WriteLine($"  Tables: {tables}");
            Console.WriteLine($"  Rows per Table: {rowsPerTable}, Columns per Table: {columnsPerTable}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 25000, $"Many tables creation time too high: {time}ms");
        }

        /// <summary>
        /// Test processing large tables performance
        /// </summary>
        [Test]
        public void ShouldProcessLargeTables()
        {
            // Arrange
            const int rows = 500;
            const int columns = 20;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _documentFactory.CreateDocument("Large Table");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                var tableResult = document.AddTable(rows, columns);
                AssertResultSuccess(tableResult);
                var table = tableResult.Value;

                // Fill large table with data
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        var cellResult = table.GetCell(row, col);
                        AssertResultSuccess(cellResult);
                        var cell = cellResult.Value;

                        var addTextResult = cell.AddText($"R{row}_C{col}");
                        AssertResultSuccess(addTextResult);
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Table Processing Performance:");
            Console.WriteLine($"  Rows: {rows}, Columns: {columns}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 20000, $"Large table processing time too high: {time}ms");
        }

        /// <summary>
        /// Test maintaining performance with formatting
        /// </summary>
        [Test]
        public void ShouldMaintainPerformanceWithFormatting()
        {
            // Arrange
            const int paragraphs = 500;
            const int wordsPerParagraph = 30;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var documentResult = _documentFactory.CreateDocument("Formatted Document");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create paragraphs with extensive formatting
                for (int i = 0; i < paragraphs; i++)
                {
                    var paragraphResult = document.AddParagraph();
                    AssertResultSuccess(paragraphResult);
                    var paragraph = paragraphResult.Value;

                    // Add text with formatting
                    var text = GenerateText(wordsPerParagraph);
                    var addTextResult = paragraph.AddText(text);
                    AssertResultSuccess(addTextResult);

                    // Apply formatting every 10th paragraph
                    if (i % 10 == 0)
                    {
                        var setBoldResult = paragraph.SetBold(true);
                        AssertResultSuccess(setBoldResult);

                        var setItalicResult = paragraph.SetItalic(true);
                        AssertResultSuccess(setItalicResult);

                        var setUnderlineResult = paragraph.SetUnderline(true);
                        AssertResultSuccess(setUnderlineResult);
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Formatted Document Performance:");
            Console.WriteLine($"  Paragraphs: {paragraphs}");
            Console.WriteLine($"  Words per Paragraph: {wordsPerParagraph}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 20000, $"Formatted document generation time too high: {time}ms");
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
                    var documentResult = _documentFactory.CreateDocument($"MemoryTest{i}");
                    AssertResultSuccess(documentResult);
                    var document = documentResult.Value;

                    // Add some content
                    var paragraphResult = document.AddParagraph();
                    AssertResultSuccess(paragraphResult);
                    var paragraph = paragraphResult.Value;

                    var addTextResult = paragraph.AddText($"Test content {i}");
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
            const int paragraphs = 2000;
            const int wordsPerParagraph = 100;

            // Act
            var memoryUsage = MeasureMemoryUsage(() =>
            {
                var documentResult = _documentFactory.CreateDocument("Large Memory Test");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Add large content
                for (int i = 0; i < paragraphs; i++)
                {
                    var paragraphResult = document.AddParagraph();
                    AssertResultSuccess(paragraphResult);
                    var paragraph = paragraphResult.Value;

                    var text = GenerateText(wordsPerParagraph);
                    var addTextResult = paragraph.AddText(text);
                    AssertResultSuccess(addTextResult);
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Document Memory Usage:");
            Console.WriteLine($"  Paragraphs: {paragraphs}");
            Console.WriteLine($"  Words per Paragraph: {wordsPerParagraph}");
            Console.WriteLine($"  Memory: {memoryUsage / 1024:F2}KB");

            Assert.IsTrue(memoryUsage < 25 * 1024 * 1024, $"Large document memory usage too high: {memoryUsage / 1024:F2}KB");
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
                            var documentResult = _documentFactory.CreateDocument($"Concurrent{taskIndex}_{j}");
                            AssertResultSuccess(documentResult);
                            var document = documentResult.Value;

                            // Add some content
                            var paragraphResult = document.AddParagraph();
                            AssertResultSuccess(paragraphResult);
                            var paragraph = paragraphResult.Value;

                            var addTextResult = paragraph.AddText($"Test content {taskIndex}_{j}");
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

            Assert.IsTrue(averageTime < 3000, $"Average concurrent time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 6000, $"Max concurrent time too high: {maxTime}ms");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a temporary file path for testing
        /// </summary>
        /// <param name="extension">File extension (default: .docx)</param>
        /// <returns>Path to temporary file</returns>
        private string CreateTempFilePath(string extension = ".docx")
        {
            var fileName = $"test_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);
            _createdTestFiles.Add(filePath);
            return filePath;
        }

        /// <summary>
        /// Generates test text with specified word count
        /// </summary>
        /// <param name="wordCount">Number of words to generate</param>
        /// <returns>Generated text</returns>
        private string GenerateText(int wordCount)
        {
            var words = new List<string>();
            var random = new Random();
            var sampleWords = new[] { "the", "quick", "brown", "fox", "jumps", "over", "lazy", "dog", "and", "runs", "fast", "through", "forest", "with", "great", "speed", "and", "agility", "while", "hunting", "for", "prey", "in", "the", "wild", "nature", "of", "the", "world" };

            for (int i = 0; i < wordCount; i++)
            {
                words.Add(sampleWords[random.Next(sampleWords.Length)]);
            }

            return string.Join(" ", words);
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
