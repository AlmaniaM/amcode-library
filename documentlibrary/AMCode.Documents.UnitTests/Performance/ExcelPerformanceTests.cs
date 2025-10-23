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
using AMCode.Documents.UnitTests.Performance;
using NUnit.Framework;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AMCode.Documents.UnitTests.Performance
{
    /// <summary>
    /// Performance tests for Excel document generation operations
    /// </summary>
    [TestFixture]
    public class ExcelPerformanceTests
    {
        private IWorkbookFactory _workbookFactory;
        private IWorkbookEngine _workbookEngine;
        private IWorkbookLogger _workbookLogger;
        private IWorkbookValidator _workbookValidator;
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
            _workbookEngine = new OpenXmlWorkbookEngine();
            _workbookLogger = new TestWorkbookLogger();
            _workbookValidator = new TestWorkbookValidator();
            _workbookFactory = new WorkbookFactory(_workbookEngine, _workbookLogger, _workbookValidator);
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

        #region Large Excel File Generation Tests

        /// <summary>
        /// Test large Excel file generation performance
        /// </summary>
        [Test]
        public void ShouldGenerateLargeWorkbook()
        {
            // Arrange
            const int rows = 5000;
            const int columns = 20;
            const int worksheets = 5;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var workbookResult = _workbookFactory.CreateWorkbook("Large Workbook");
                AssertResultSuccess(workbookResult);
                using var workbook = workbookResult.Value;

                // Create multiple worksheets with large datasets
                for (int sheetIndex = 0; sheetIndex < worksheets; sheetIndex++)
                {
                    var addWorksheetResult = workbook.AddWorksheet($"Sheet{sheetIndex + 1}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    // Generate large dataset
                    var testData = CreateTestDataArray(rows, columns);
                    var setDataResult = worksheet.SetData("A1", testData);
                    AssertResultSuccess(setDataResult);

                    // Add some formatting
                    for (int col = 0; col < columns; col++)
                    {
                        var columnName = GetCellReference(1, (uint)(col + 1)).Replace("1", "");
                        var setColumnWidthResult = worksheet.SetColumnWidth(columnName, 15.0);
                        AssertResultSuccess(setColumnWidthResult);
                    }
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = workbook.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Workbook Generation Performance:");
            Console.WriteLine($"  Rows: {rows}, Columns: {columns}, Worksheets: {worksheets}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 30000, $"Large workbook generation time too high: {time}ms");
        }

        /// <summary>
        /// Test handling many worksheets performance
        /// </summary>
        [Test]
        public void ShouldHandleManyWorksheets()
        {
            // Arrange
            const int worksheets = 50;
            const int rowsPerSheet = 100;
            const int columnsPerSheet = 10;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var workbookResult = _workbookFactory.CreateWorkbook("Many Worksheets");
                AssertResultSuccess(workbookResult);
                using var workbook = workbookResult.Value;

                // Create many worksheets
                for (int i = 0; i < worksheets; i++)
                {
                    var addWorksheetResult = workbook.AddWorksheet($"Worksheet{i + 1}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    // Add some data to each worksheet
                    var testData = CreateTestDataArray(rowsPerSheet, columnsPerSheet);
                    var setDataResult = worksheet.SetData("A1", testData);
                    AssertResultSuccess(setDataResult);
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = workbook.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Many Worksheets Performance:");
            Console.WriteLine($"  Worksheets: {worksheets}");
            Console.WriteLine($"  Rows per Sheet: {rowsPerSheet}, Columns per Sheet: {columnsPerSheet}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 20000, $"Many worksheets creation time too high: {time}ms");
        }

        /// <summary>
        /// Test processing large datasets performance
        /// </summary>
        [Test]
        public void ShouldProcessLargeDataSets()
        {
            // Arrange
            const int rows = 10000;
            const int columns = 50;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var workbookResult = _workbookFactory.CreateWorkbook("Large Dataset");
                AssertResultSuccess(workbookResult);
                using var workbook = workbookResult.Value;

                var addWorksheetResult = workbook.AddWorksheet("LargeData");
                AssertResultSuccess(addWorksheetResult);
                var worksheet = addWorksheetResult.Value;

                // Process large dataset in chunks to avoid memory issues
                const int chunkSize = 1000;
                for (int startRow = 0; startRow < rows; startRow += chunkSize)
                {
                    var currentChunkSize = Math.Min(chunkSize, rows - startRow);
                    var testData = CreateTestDataArray(currentChunkSize, columns);
                    var setDataResult = worksheet.SetData($"A{startRow + 1}", testData);
                    AssertResultSuccess(setDataResult);
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = workbook.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Dataset Processing Performance:");
            Console.WriteLine($"  Rows: {rows}, Columns: {columns}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 60000, $"Large dataset processing time too high: {time}ms");
        }

        /// <summary>
        /// Test maintaining performance with styles
        /// </summary>
        [Test]
        public void ShouldMaintainPerformanceWithStyles()
        {
            // Arrange
            const int rows = 2000;
            const int columns = 15;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                var workbookResult = _workbookFactory.CreateWorkbook("Styled Workbook");
                AssertResultSuccess(workbookResult);
                using var workbook = workbookResult.Value;

                var addWorksheetResult = workbook.AddWorksheet("StyledSheet");
                AssertResultSuccess(addWorksheetResult);
                var worksheet = addWorksheetResult.Value;

                // Add data
                var testData = CreateTestDataArray(rows, columns);
                var setDataResult = worksheet.SetData("A1", testData);
                AssertResultSuccess(setDataResult);

                // Apply extensive styling
                for (int row = 1; row <= rows; row += 10) // Style every 10th row
                {
                    for (int col = 1; col <= columns; col++)
                    {
                        var cellRef = GetCellReference((uint)row, (uint)col);
                        var setCellValueResult = worksheet.SetCellValue(cellRef, $"Styled{row}_{col}");
                        AssertResultSuccess(setCellValueResult);
                    }
                }

                // Set column widths
                for (int col = 1; col <= columns; col++)
                {
                    var columnName = GetCellReference(1, (uint)col).Replace("1", "");
                    var setColumnWidthResult = worksheet.SetColumnWidth(columnName, 20.0);
                    AssertResultSuccess(setColumnWidthResult);
                }

                // Set row heights
                for (int row = 1; row <= rows; row += 5) // Set every 5th row height
                {
                    var setRowHeightResult = worksheet.SetRowHeight(row, 25.0);
                    AssertResultSuccess(setRowHeightResult);
                }

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = workbook.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Styled Workbook Performance:");
            Console.WriteLine($"  Rows: {rows}, Columns: {columns}");
            Console.WriteLine($"  Time: {time}ms");

            Assert.IsTrue(time < 25000, $"Styled workbook generation time too high: {time}ms");
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
                    var workbookResult = _workbookFactory.CreateWorkbook($"MemoryTest{i}");
                    AssertResultSuccess(workbookResult);
                    var workbook = workbookResult.Value;

                    // Add some data
                    var addWorksheetResult = workbook.AddWorksheet($"Sheet{i}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    var testData = CreateTestDataArray(500, 10);
                    var setDataResult = worksheet.SetData("A1", testData);
                    AssertResultSuccess(setDataResult);

                    // Save to file
                    var filePath = CreateTempFilePath();
                    var saveResult = workbook.SaveAs(filePath);
                    AssertResultSuccess(saveResult);

                    workbook.Dispose();
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

            Assert.IsTrue(averageMemory < 10 * 1024 * 1024, $"Average memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 20 * 1024 * 1024, $"Max memory usage too high: {maxMemory / 1024:F2}KB");
        }

        /// <summary>
        /// Test memory usage with large workbooks
        /// </summary>
        [Test]
        public void ShouldHandleLargeWorkbookMemoryUsage()
        {
            // Arrange
            const int rows = 5000;
            const int columns = 20;

            // Act
            var memoryUsage = MeasureMemoryUsage(() =>
            {
                var workbookResult = _workbookFactory.CreateWorkbook("Large Memory Test");
                AssertResultSuccess(workbookResult);
                using var workbook = workbookResult.Value;

                var addWorksheetResult = workbook.AddWorksheet("LargeSheet");
                AssertResultSuccess(addWorksheetResult);
                var worksheet = addWorksheetResult.Value;

                // Add large dataset
                var testData = CreateTestDataArray(rows, columns);
                var setDataResult = worksheet.SetData("A1", testData);
                AssertResultSuccess(setDataResult);

                // Save to file
                var filePath = CreateTempFilePath();
                var saveResult = workbook.SaveAs(filePath);
                AssertResultSuccess(saveResult);
            });

            // Assert
            Console.WriteLine($"Large Workbook Memory Usage:");
            Console.WriteLine($"  Rows: {rows}, Columns: {columns}");
            Console.WriteLine($"  Memory: {memoryUsage / 1024:F2}KB");

            Assert.IsTrue(memoryUsage < 50 * 1024 * 1024, $"Large workbook memory usage too high: {memoryUsage / 1024:F2}KB");
        }

        #endregion

        #region Concurrent Operations Tests

        /// <summary>
        /// Test concurrent workbook creation performance
        /// </summary>
        [Test]
        public async Task ShouldHandleConcurrentWorkbookCreation()
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
                            var workbookResult = _workbookFactory.CreateWorkbook($"Concurrent{taskIndex}_{j}");
                            AssertResultSuccess(workbookResult);
                            var workbook = workbookResult.Value;

                            // Add some data
                            var addWorksheetResult = workbook.AddWorksheet($"Sheet{j}");
                            AssertResultSuccess(addWorksheetResult);
                            var worksheet = addWorksheetResult.Value;

                            var testData = CreateTestDataArray(100, 5);
                            var setDataResult = worksheet.SetData("A1", testData);
                            AssertResultSuccess(setDataResult);

                            // Save to file
                            var filePath = CreateTempFilePath();
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

            Console.WriteLine($"Concurrent Workbook Creation Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 5000, $"Average concurrent time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 10000, $"Max concurrent time too high: {maxTime}ms");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a temporary file path for testing
        /// </summary>
        /// <param name="extension">File extension (default: .xlsx)</param>
        /// <returns>Path to temporary file</returns>
        private string CreateTempFilePath(string extension = ".xlsx")
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
        /// Gets cell reference from row and column indices
        /// </summary>
        /// <param name="row">Row index (1-based)</param>
        /// <param name="column">Column index (1-based)</param>
        /// <returns>Cell reference (e.g., "A1", "B2")</returns>
        private string GetCellReference(uint row, uint column)
        {
            var columnName = "";
            while (column > 0)
            {
                column--;
                columnName = (char)('A' + column % 26) + columnName;
                column /= 26;
            }
            return $"{columnName}{row}";
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
