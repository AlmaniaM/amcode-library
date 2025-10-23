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
using AMCode.Documents.Pdf.Domain.Interfaces;
using AMCode.Documents.Pdf.Infrastructure.Factories;
using AMCode.Documents.Pdf.Interfaces;
using AMCode.Documents.UnitTests.Performance;
using NUnit.Framework;

namespace AMCode.Documents.UnitTests.Performance
{
    /// <summary>
    /// Performance tests for concurrent document processing operations
    /// </summary>
    [TestFixture]
    public class ConcurrentProcessingTests
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

        #region Concurrent Document Creation Tests

        /// <summary>
        /// Test concurrent Excel workbook creation performance
        /// </summary>
        [Test]
        public async Task ConcurrentExcelCreation_ShouldHandleMultipleWorkbooks()
        {
            // Arrange
            const int concurrentTasks = 20;
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
                            var workbookResult = _workbookFactory.CreateWorkbook($"Concurrent{taskIndex}_{j}");
                            AssertResultSuccess(workbookResult);
                            var workbook = workbookResult.Value;

                            // Add some data
                            var addWorksheetResult = workbook.AddWorksheet($"Sheet{j}");
                            AssertResultSuccess(addWorksheetResult);
                            var worksheet = addWorksheetResult.Value;

                            var testData = CreateTestDataArray(50, 10);
                            var setDataResult = worksheet.SetData("A1", testData);
                            AssertResultSuccess(setDataResult);

                            // Save to file
                            var filePath = CreateTempFilePath(".xlsx");
                            var saveResult = workbook.SaveAs(filePath);
                            AssertResultSuccess(saveResult);

                            workbook.Dispose();
                        }
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Concurrent Excel Creation Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 3000, $"Average concurrent Excel creation time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 6000, $"Max concurrent Excel creation time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test concurrent DOCX document creation performance
        /// </summary>
        [Test]
        public async Task ConcurrentDocxCreation_ShouldHandleMultipleDocuments()
        {
            // Arrange
            const int concurrentTasks = 20;
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
                            var filePath = CreateTempFilePath(".docx");
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

            Console.WriteLine($"Concurrent DOCX Creation Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 2000, $"Average concurrent DOCX creation time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 4000, $"Max concurrent DOCX creation time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test concurrent PDF document creation performance
        /// </summary>
        [Test]
        public async Task ConcurrentPdfCreation_ShouldHandleMultipleDocuments()
        {
            // Arrange
            const int concurrentTasks = 20;
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
                            var filePath = CreateTempFilePath(".pdf");
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

            Console.WriteLine($"Concurrent PDF Creation Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 2500, $"Average concurrent PDF creation time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 5000, $"Max concurrent PDF creation time too high: {maxTime}ms");
        }

        #endregion

        #region Mixed Format Concurrent Tests

        /// <summary>
        /// Test concurrent creation across all document formats
        /// </summary>
        [Test]
        public async Task MixedFormatConcurrent_ShouldHandleAllFormats()
        {
            // Arrange
            const int tasksPerFormat = 10;
            var totalTasks = tasksPerFormat * 3; // Excel, DOCX, PDF

            // Act
            var tasks = new List<Task<long>>();

            // Excel tasks
            for (int i = 0; i < tasksPerFormat; i++)
            {
                var taskIndex = i;
                var task = Task.Run(() =>
                {
                    return MeasureExecutionTime(() =>
                    {
                        var workbookResult = _workbookFactory.CreateWorkbook($"MixedExcel{taskIndex}");
                        AssertResultSuccess(workbookResult);
                        var workbook = workbookResult.Value;

                        var addWorksheetResult = workbook.AddWorksheet("Data");
                        AssertResultSuccess(addWorksheetResult);
                        var worksheet = addWorksheetResult.Value;

                        var testData = CreateTestDataArray(100, 10);
                        var setDataResult = worksheet.SetData("A1", testData);
                        AssertResultSuccess(setDataResult);

                        var filePath = CreateTempFilePath(".xlsx");
                        var saveResult = workbook.SaveAs(filePath);
                        AssertResultSuccess(saveResult);

                        workbook.Dispose();
                    });
                });
                tasks.Add(task);
            }

            // DOCX tasks
            for (int i = 0; i < tasksPerFormat; i++)
            {
                var taskIndex = i;
                var task = Task.Run(() =>
                {
                    return MeasureExecutionTime(() =>
                    {
                        var documentResult = _documentFactory.CreateDocument($"MixedDocx{taskIndex}");
                        AssertResultSuccess(documentResult);
                        var document = documentResult.Value;

                        var paragraphResult = document.AddParagraph();
                        AssertResultSuccess(paragraphResult);
                        var paragraph = paragraphResult.Value;

                        var addTextResult = paragraph.AddText($"DOCX content {taskIndex}");
                        AssertResultSuccess(addTextResult);

                        var filePath = CreateTempFilePath(".docx");
                        var saveResult = document.SaveAs(filePath);
                        AssertResultSuccess(saveResult);

                        document.Dispose();
                    });
                });
                tasks.Add(task);
            }

            // PDF tasks
            for (int i = 0; i < tasksPerFormat; i++)
            {
                var taskIndex = i;
                var task = Task.Run(() =>
                {
                    return MeasureExecutionTime(() =>
                    {
                        var documentResult = _pdfFactory.CreateDocument($"MixedPdf{taskIndex}");
                        AssertResultSuccess(documentResult);
                        var document = documentResult.Value;

                        var addPageResult = document.AddPage();
                        AssertResultSuccess(addPageResult);
                        var page = addPageResult.Value;

                        var addTextResult = page.AddText($"PDF content {taskIndex}", 10, 10, 12);
                        AssertResultSuccess(addTextResult);

                        var filePath = CreateTempFilePath(".pdf");
                        var saveResult = document.SaveAs(filePath);
                        AssertResultSuccess(saveResult);

                        document.Dispose();
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Mixed Format Concurrent Performance:");
            Console.WriteLine($"  Total Tasks: {totalTasks}");
            Console.WriteLine($"  Excel Tasks: {tasksPerFormat}");
            Console.WriteLine($"  DOCX Tasks: {tasksPerFormat}");
            Console.WriteLine($"  PDF Tasks: {tasksPerFormat}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 3000, $"Average mixed format time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 6000, $"Max mixed format time too high: {maxTime}ms");
        }

        #endregion

        #region High Load Concurrent Tests

        /// <summary>
        /// Test high load concurrent processing
        /// </summary>
        [Test]
        public async Task HighLoadConcurrent_ShouldHandleManyOperations()
        {
            // Arrange
            const int concurrentTasks = 50;
            const int operationsPerTask = 3;

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
                            // Create Excel workbook
                            var workbookResult = _workbookFactory.CreateWorkbook($"HighLoad{taskIndex}_{j}");
                            AssertResultSuccess(workbookResult);
                            var workbook = workbookResult.Value;

                            var addWorksheetResult = workbook.AddWorksheet("Data");
                            AssertResultSuccess(addWorksheetResult);
                            var worksheet = addWorksheetResult.Value;

                            var testData = CreateTestDataArray(20, 5);
                            var setDataResult = worksheet.SetData("A1", testData);
                            AssertResultSuccess(setDataResult);

                            var excelFilePath = CreateTempFilePath(".xlsx");
                            var excelSaveResult = workbook.SaveAs(excelFilePath);
                            AssertResultSuccess(excelSaveResult);

                            workbook.Dispose();

                            // Create DOCX document
                            var documentResult = _documentFactory.CreateDocument($"HighLoad{taskIndex}_{j}");
                            AssertResultSuccess(documentResult);
                            var document = documentResult.Value;

                            var paragraphResult = document.AddParagraph();
                            AssertResultSuccess(paragraphResult);
                            var paragraph = paragraphResult.Value;

                            var addTextResult = paragraph.AddText($"High load content {taskIndex}_{j}");
                            AssertResultSuccess(addTextResult);

                            var docxFilePath = CreateTempFilePath(".docx");
                            var docxSaveResult = document.SaveAs(docxFilePath);
                            AssertResultSuccess(docxSaveResult);

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

            Console.WriteLine($"High Load Concurrent Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Total Operations: {concurrentTasks * operationsPerTask * 2}"); // 2 formats per operation
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 5000, $"Average high load time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 10000, $"Max high load time too high: {maxTime}ms");
        }

        #endregion

        #region Memory Usage Under Load Tests

        /// <summary>
        /// Test memory usage under concurrent load
        /// </summary>
        [Test]
        public async Task ConcurrentMemoryUsage_ShouldBeReasonable()
        {
            // Arrange
            const int concurrentTasks = 30;
            var memoryUsages = new List<long>();

            // Act
            var tasks = new List<Task<long>>();
            for (int i = 0; i < concurrentTasks; i++)
            {
                var taskIndex = i;
                var task = Task.Run(() =>
                {
                    return MeasureMemoryUsage(() =>
                    {
                        // Create and process documents
                        var workbookResult = _workbookFactory.CreateWorkbook($"MemoryTest{taskIndex}");
                        AssertResultSuccess(workbookResult);
                        var workbook = workbookResult.Value;

                        var addWorksheetResult = workbook.AddWorksheet("Data");
                        AssertResultSuccess(addWorksheetResult);
                        var worksheet = addWorksheetResult.Value;

                        var testData = CreateTestDataArray(100, 10);
                        var setDataResult = worksheet.SetData("A1", testData);
                        AssertResultSuccess(setDataResult);

                        var filePath = CreateTempFilePath(".xlsx");
                        var saveResult = workbook.SaveAs(filePath);
                        AssertResultSuccess(saveResult);

                        workbook.Dispose();
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageMemory = times.Average();
            var maxMemory = times.Max();

            Console.WriteLine($"Concurrent Memory Usage:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");

            Assert.IsTrue(averageMemory < 5 * 1024 * 1024, $"Average concurrent memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 10 * 1024 * 1024, $"Max concurrent memory usage too high: {maxMemory / 1024:F2}KB");
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
