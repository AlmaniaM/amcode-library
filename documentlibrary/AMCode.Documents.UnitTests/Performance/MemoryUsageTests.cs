using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Infrastructure.Factories;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using AMCode.Documents.Xlsx.Providers.OpenXml;
using AMCode.Documents.Xlsx.Interfaces;
using AMCode.Documents.Docx.Domain.Interfaces;
using AMCode.Documents.Docx.Infrastructure.Factories;
using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.UnitTests.Performance;
using AMCode.Documents.Pdf.Domain.Interfaces;
using AMCode.Documents.Pdf.Infrastructure.Factories;
using AMCode.Documents.Pdf.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.UnitTests.Performance
{
    /// <summary>
    /// Performance tests for memory consumption during document generation
    /// </summary>
    [TestFixture]
    public class MemoryUsageTests
    {
        private IWorkbookFactory _workbookFactory;
        private IDocumentFactory _documentFactory;
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
            var workbookEngine = new OpenXmlWorkbookEngine();
            var workbookLogger = new TestWorkbookLogger();
            var workbookValidator = new TestWorkbookValidator();
            _workbookFactory = new WorkbookFactory(workbookEngine, workbookLogger, workbookValidator);
            _documentFactory = new DocumentFactory();
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

        #region Excel Memory Usage Tests

        /// <summary>
        /// Test Excel memory consumption during generation
        /// </summary>
        [Test]
        public void ExcelGeneration_ShouldHaveReasonableMemoryUsage()
        {
            // Arrange
            const int iterations = 20;
            var memoryUsages = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var memoryUsage = MeasureMemoryUsage(() =>
                {
                    var workbookResult = _workbookFactory.CreateWorkbook($"MemoryTest{i}");
                    AssertResultSuccess(workbookResult);
                    var workbook = workbookResult.Value;

                    // Add worksheet with data
                    var addWorksheetResult = workbook.AddWorksheet($"Sheet{i}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    var testData = CreateTestDataArray(100, 10);
                    var setDataResult = worksheet.SetData("A1", testData);
                    AssertResultSuccess(setDataResult);

                    // Save to file
                    var filePath = CreateTempFilePath(".xlsx");
                    var saveResult = workbook.SaveAs(filePath);
                    AssertResultSuccess(saveResult);

                    workbook.Dispose();
                });
                memoryUsages.Add(memoryUsage);
            }

            // Assert
            var averageMemory = memoryUsages.Average();
            var maxMemory = memoryUsages.Max();

            Console.WriteLine($"Excel Memory Usage Test:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");

            Assert.IsTrue(averageMemory < 5 * 1024 * 1024, $"Average Excel memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 10 * 1024 * 1024, $"Max Excel memory usage too high: {maxMemory / 1024:F2}KB");
        }

        /// <summary>
        /// Test Excel memory usage with large workbooks
        /// </summary>
        [Test]
        public void ExcelLargeWorkbook_ShouldHandleMemoryEfficiently()
        {
            // Arrange
            const int rows = 2000;
            const int columns = 20;
            const int worksheets = 5;

            // Act
            var memoryUsage = MeasureMemoryUsage(() =>
            {
                var workbookResult = _workbookFactory.CreateWorkbook("Large Excel");
                AssertResultSuccess(workbookResult);
                using var workbook = workbookResult.Value;

                // Create multiple worksheets with large datasets
                for (int sheetIndex = 0; sheetIndex < worksheets; sheetIndex++)
                {
                    var addWorksheetResult = workbook.AddWorksheet($"Sheet{sheetIndex + 1}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    var testData = CreateTestDataArray(rows, columns);
                    var setDataResult = worksheet.SetData("A1", testData);
                    AssertResultSuccess(setDataResult);
                }

                // Save to file
                var filePath = CreateTempFilePath(".xlsx");
                var saveResult = workbook.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Excel Large Workbook Memory Usage:");
            Console.WriteLine($"  Rows: {rows}, Columns: {columns}, Worksheets: {worksheets}");
            Console.WriteLine($"  Memory: {memoryUsage / 1024:F2}KB");

            Assert.IsTrue(memoryUsage < 30 * 1024 * 1024, $"Large Excel workbook memory usage too high: {memoryUsage / 1024:F2}KB");
        }

        #endregion

        #region DOCX Memory Usage Tests

        /// <summary>
        /// Test DOCX memory consumption during generation
        /// </summary>
        [Test]
        public void DocxGeneration_ShouldHaveReasonableMemoryUsage()
        {
            // Arrange
            const int iterations = 20;
            var memoryUsages = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var memoryUsage = MeasureMemoryUsage(() =>
                {
                    var documentResult = _documentFactory.CreateDocument($"MemoryTest{i}");
                    AssertResultSuccess(documentResult);
                    var document = documentResult.Value;

                    // Add paragraph with text
                    var paragraphResult = document.AddParagraph();
                    AssertResultSuccess(paragraphResult);
                    var paragraph = paragraphResult.Value;

                    var addTextResult = paragraph.AddText($"Test content {i}");
                    AssertResultSuccess(addTextResult);

                    // Save to file
                    var filePath = CreateTempFilePath(".docx");
                    var saveResult = document.SaveAs(filePath);
                    AssertResultSuccess(saveResult);

                    document.Dispose();
                });
                memoryUsages.Add(memoryUsage);
            }

            // Assert
            var averageMemory = memoryUsages.Average();
            var maxMemory = memoryUsages.Max();

            Console.WriteLine($"DOCX Memory Usage Test:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");

            Assert.IsTrue(averageMemory < 3 * 1024 * 1024, $"Average DOCX memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 6 * 1024 * 1024, $"Max DOCX memory usage too high: {maxMemory / 1024:F2}KB");
        }

        /// <summary>
        /// Test DOCX memory usage with large documents
        /// </summary>
        [Test]
        public void DocxLargeDocument_ShouldHandleMemoryEfficiently()
        {
            // Arrange
            const int paragraphs = 1000;
            const int wordsPerParagraph = 50;

            // Act
            var memoryUsage = MeasureMemoryUsage(() =>
            {
                var documentResult = _documentFactory.CreateDocument("Large DOCX");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                // Create many paragraphs with text
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
                var filePath = CreateTempFilePath(".docx");
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"DOCX Large Document Memory Usage:");
            Console.WriteLine($"  Paragraphs: {paragraphs}");
            Console.WriteLine($"  Words per Paragraph: {wordsPerParagraph}");
            Console.WriteLine($"  Memory: {memoryUsage / 1024:F2}KB");

            Assert.IsTrue(memoryUsage < 20 * 1024 * 1024, $"Large DOCX document memory usage too high: {memoryUsage / 1024:F2}KB");
        }

        #endregion

        #region PDF Memory Usage Tests

        /// <summary>
        /// Test PDF memory consumption during generation
        /// </summary>
        [Test]
        public void PdfGeneration_ShouldHaveReasonableMemoryUsage()
        {
            // Arrange
            const int iterations = 20;
            var memoryUsages = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var memoryUsage = MeasureMemoryUsage(() =>
                {
                    var documentResult = _pdfFactory.CreateDocument($"MemoryTest{i}");
                    AssertResultSuccess(documentResult);
                    var document = documentResult.Value;

                    // Add page with content
                    var addPageResult = document.AddPage();
                    AssertResultSuccess(addPageResult);
                    var page = addPageResult.Value;

                    var addTextResult = page.AddText($"Test content {i}", 10, 10, 12);
                    AssertResultSuccess(addTextResult);

                    // Save to file
                    var filePath = CreateTempFilePath(".pdf");
                    var saveResult = document.SaveAs(filePath);
                    AssertResultSuccess(saveResult);

                    document.Dispose();
                });
                memoryUsages.Add(memoryUsage);
            }

            // Assert
            var averageMemory = memoryUsages.Average();
            var maxMemory = memoryUsages.Max();

            Console.WriteLine($"PDF Memory Usage Test:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");

            Assert.IsTrue(averageMemory < 3 * 1024 * 1024, $"Average PDF memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 6 * 1024 * 1024, $"Max PDF memory usage too high: {maxMemory / 1024:F2}KB");
        }

        /// <summary>
        /// Test PDF memory usage with large documents
        /// </summary>
        [Test]
        public void PdfLargeDocument_ShouldHandleMemoryEfficiently()
        {
            // Arrange
            const int pages = 200;
            const int elementsPerPage = 50;

            // Act
            var memoryUsage = MeasureMemoryUsage(() =>
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

                    for (int elementIndex = 0; elementIndex < elementsPerPage; elementIndex++)
                    {
                        var addTextResult = page.AddText($"Content {pageIndex}_{elementIndex}", 10, 10 + elementIndex * 10, 10);
                        AssertResultSuccess(addTextResult);
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath(".pdf");
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"PDF Large Document Memory Usage:");
            Console.WriteLine($"  Pages: {pages}");
            Console.WriteLine($"  Elements per Page: {elementsPerPage}");
            Console.WriteLine($"  Memory: {memoryUsage / 1024:F2}KB");

            Assert.IsTrue(memoryUsage < 25 * 1024 * 1024, $"Large PDF document memory usage too high: {memoryUsage / 1024:F2}KB");
        }

        #endregion

        #region Cross-Format Memory Comparison Tests

        /// <summary>
        /// Test memory usage comparison across all formats
        /// </summary>
        [Test]
        public void CrossFormatMemoryComparison_ShouldBeReasonable()
        {
            // Arrange
            const int contentSize = 1000; // 1000 elements per document

            // Act
            var excelMemory = MeasureMemoryUsage(() =>
            {
                var workbookResult = _workbookFactory.CreateWorkbook("Memory Comparison Excel");
                AssertResultSuccess(workbookResult);
                using var workbook = workbookResult.Value;

                var addWorksheetResult = workbook.AddWorksheet("Data");
                AssertResultSuccess(addWorksheetResult);
                var worksheet = addWorksheetResult.Value;

                var testData = CreateTestDataArray(contentSize, 1);
                var setDataResult = worksheet.SetData("A1", testData);
                AssertResultSuccess(setDataResult);

                var filePath = CreateTempFilePath(".xlsx");
                var saveResult = workbook.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            var docxMemory = MeasureMemoryUsage(() =>
            {
                var documentResult = _documentFactory.CreateDocument("Memory Comparison DOCX");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                for (int i = 0; i < contentSize; i++)
                {
                    var paragraphResult = document.AddParagraph();
                    AssertResultSuccess(paragraphResult);
                    var paragraph = paragraphResult.Value;

                    var addTextResult = paragraph.AddText($"Content {i}");
                    AssertResultSuccess(addTextResult);
                }

                var filePath = CreateTempFilePath(".docx");
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            var pdfMemory = MeasureMemoryUsage(() =>
            {
                var documentResult = _pdfFactory.CreateDocument("Memory Comparison PDF");
                AssertResultSuccess(documentResult);
                using var document = documentResult.Value;

                var addPageResult = document.AddPage();
                AssertResultSuccess(addPageResult);
                var page = addPageResult.Value;

                for (int i = 0; i < contentSize; i++)
                {
                    var addTextResult = page.AddText($"Content {i}", 10, 10 + i * 10, 10);
                    AssertResultSuccess(addTextResult);
                }

                var filePath = CreateTempFilePath(".pdf");
                var saveResult = document.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Cross-Format Memory Comparison:");
            Console.WriteLine($"  Content Size: {contentSize} elements");
            Console.WriteLine($"  Excel Memory: {excelMemory / 1024:F2}KB");
            Console.WriteLine($"  DOCX Memory: {docxMemory / 1024:F2}KB");
            Console.WriteLine($"  PDF Memory: {pdfMemory / 1024:F2}KB");

            // All formats should have reasonable memory usage
            Assert.IsTrue(excelMemory < 15 * 1024 * 1024, $"Excel memory usage too high: {excelMemory / 1024:F2}KB");
            Assert.IsTrue(docxMemory < 15 * 1024 * 1024, $"DOCX memory usage too high: {docxMemory / 1024:F2}KB");
            Assert.IsTrue(pdfMemory < 15 * 1024 * 1024, $"PDF memory usage too high: {pdfMemory / 1024:F2}KB");
        }

        #endregion

        #region Memory Leak Detection Tests

        /// <summary>
        /// Test for memory leaks during repeated operations
        /// </summary>
        [Test]
        public void MemoryLeakDetection_ShouldNotHaveLeaks()
        {
            // Arrange
            const int iterations = 100;
            var memoryUsages = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var memoryUsage = MeasureMemoryUsage(() =>
                {
                    // Create and dispose Excel workbook
                    var workbookResult = _workbookFactory.CreateWorkbook($"LeakTest{i}");
                    AssertResultSuccess(workbookResult);
                    var workbook = workbookResult.Value;
                    workbook.Dispose();

                    // Create and dispose DOCX document
                    var documentResult = _documentFactory.CreateDocument($"LeakTest{i}");
                    AssertResultSuccess(documentResult);
                    var document = documentResult.Value;
                    document.Dispose();

                    // Create and dispose PDF document
                    var pdfResult = _pdfFactory.CreateDocument($"LeakTest{i}");
                    AssertResultSuccess(pdfResult);
                    var pdfDocument = pdfResult.Value;
                    pdfDocument.Dispose();
                });
                memoryUsages.Add(memoryUsage);

                // Force garbage collection every 10 iterations
                if (i % 10 == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }

            // Assert
            var averageMemory = memoryUsages.Average();
            var maxMemory = memoryUsages.Max();
            var memoryGrowth = memoryUsages.Last() - memoryUsages.First();

            Console.WriteLine($"Memory Leak Detection Test:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");
            Console.WriteLine($"  Memory Growth: {memoryGrowth / 1024:F2}KB");

            // Memory growth should be minimal (less than 1MB)
            Assert.IsTrue(memoryGrowth < 1024 * 1024, $"Memory leak detected: {memoryGrowth / 1024:F2}KB growth");
            Assert.IsTrue(averageMemory < 5 * 1024 * 1024, $"Average memory usage too high: {averageMemory / 1024:F2}KB");
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
        /// Creates test data array for worksheet testing
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>2D array with test data</returns>
        private object[,] CreateTestDataArray(int rows, int columns)
        {
            var data = new object[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    data[i, j] = $"R{i + 1}C{j + 1}";
                }
            }
            return data;
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
