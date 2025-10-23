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
using NUnit.Framework;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AMCode.Documents.Xlsx.Tests.Performance
{
    /// <summary>
    /// Performance tests for Xlsx library operations
    /// </summary>
    [TestFixture]
    public class PerformanceTests : IntegrationTestBase
    {
        private IWorkbookFactory _workbookFactory;
        private IWorkbookEngine _workbookEngine;
        private IWorkbookLogger _workbookLogger;
        private IWorkbookValidator _workbookValidator;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            
            // Create real implementations for performance testing
            _workbookEngine = new OpenXmlWorkbookEngine();
            _workbookLogger = new TestWorkbookLogger();
            _workbookValidator = new TestWorkbookValidator();
            _workbookFactory = new WorkbookFactory(_workbookEngine, _workbookLogger, _workbookValidator);
        }

        #region Workbook Creation Performance Tests

        /// <summary>
        /// Test workbook creation performance
        /// </summary>
        [Test]
        public void WorkbookCreationPerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int iterations = 100;
            var times = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var time = MeasureExecutionTime(() =>
                {
                    var result = _workbookFactory.CreateWorkbook();
                    AssertResultSuccess(result);
                    result.Value.Dispose();
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();
            var minTime = times.Min();

            Console.WriteLine($"Workbook Creation Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");
            Console.WriteLine($"  Min Time: {minTime}ms");

            Assert.IsTrue(averageTime < 100, $"Average creation time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 500, $"Max creation time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test workbook creation with title performance
        /// </summary>
        [Test]
        public void WorkbookCreationWithTitlePerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int iterations = 100;
            var times = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var time = MeasureExecutionTime(() =>
                {
                    var result = _workbookFactory.CreateWorkbook($"Test Workbook {i}");
                    AssertResultSuccess(result);
                    result.Value.Dispose();
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Workbook Creation with Title Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 150, $"Average creation time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 600, $"Max creation time too high: {maxTime}ms");
        }

        #endregion

        #region Workbook Opening Performance Tests

        /// <summary>
        /// Test workbook opening performance
        /// </summary>
        [Test]
        public void WorkbookOpeningPerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int iterations = 50;
            var times = new List<long>();
            var testFiles = new List<string>();

            // Create test files
            for (int i = 0; i < iterations; i++)
            {
                var filePath = CreateValidExcelFile();
                testFiles.Add(filePath);
            }

            try
            {
                // Act
                foreach (var filePath in testFiles)
                {
                    var time = MeasureExecutionTime(() =>
                    {
                        var result = _workbookFactory.OpenWorkbook(filePath);
                        AssertResultSuccess(result);
                        result.Value.Dispose();
                    });
                    times.Add(time);
                }

                // Assert
                var averageTime = times.Average();
                var maxTime = times.Max();

                Console.WriteLine($"Workbook Opening Performance:");
                Console.WriteLine($"  Iterations: {iterations}");
                Console.WriteLine($"  Average Time: {averageTime:F2}ms");
                Console.WriteLine($"  Max Time: {maxTime}ms");

                Assert.IsTrue(averageTime < 200, $"Average opening time too high: {averageTime:F2}ms");
                Assert.IsTrue(maxTime < 800, $"Max opening time too high: {maxTime}ms");
            }
            finally
            {
                // Cleanup
                foreach (var filePath in testFiles)
                {
                    try
                    {
                        File.Delete(filePath);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        /// <summary>
        /// Test workbook opening from stream performance
        /// </summary>
        [Test]
        public void WorkbookOpeningFromStreamPerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int iterations = 50;
            var times = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                using var stream = CreateValidExcelStream();
                var time = MeasureExecutionTime(() =>
                {
                    var result = _workbookFactory.OpenWorkbook(stream);
                    AssertResultSuccess(result);
                    result.Value.Dispose();
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Workbook Opening from Stream Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 150, $"Average opening time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 600, $"Max opening time too high: {maxTime}ms");
        }

        #endregion

        #region Large Worksheet Operations Performance Tests

        /// <summary>
        /// Test large worksheet operations performance
        /// </summary>
        [Test]
        public void LargeWorksheetOperationsPerformance_ShouldBeAcceptable()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            using var workbook = workbookResult.Value;

            var addWorksheetResult = workbook.AddWorksheet("LargeSheet");
            AssertResultSuccess(addWorksheetResult);
            var worksheet = addWorksheetResult.Value;

            const int rows = 1000;
            const int columns = 10;

            // Test data setting performance
            var setDataTime = MeasureExecutionTime(() =>
            {
                var testData = CreateTestDataArray(rows, columns);
                var result = worksheet.SetData("A1", testData);
                AssertResultSuccess(result);
            });

            Console.WriteLine($"Large Worksheet Data Setting Performance:");
            Console.WriteLine($"  Rows: {rows}, Columns: {columns}");
            Console.WriteLine($"  Time: {setDataTime}ms");

            Assert.IsTrue(setDataTime < 5000, $"Data setting time too high: {setDataTime}ms");

            // Test cell value setting performance
            var setCellTime = MeasureExecutionTime(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    var result = worksheet.SetCellValue($"A{i}", $"Value {i}");
                    AssertResultSuccess(result);
                }
            });

            Console.WriteLine($"Cell Value Setting Performance:");
            Console.WriteLine($"  Cells: 100");
            Console.WriteLine($"  Time: {setCellTime}ms");

            Assert.IsTrue(setCellTime < 1000, $"Cell setting time too high: {setCellTime}ms");

            // Test range operations performance
            var rangeTime = MeasureExecutionTime(() =>
            {
                var rangeResult = worksheet.GetRange("A1:J100");
                AssertResultSuccess(rangeResult);
                var range = rangeResult.Value;

                var setRangeResult = range.SetValue("Range Value");
                AssertResultSuccess(setRangeResult);

                var clearRangeResult = range.Clear();
                AssertResultSuccess(clearRangeResult);
            });

            Console.WriteLine($"Range Operations Performance:");
            Console.WriteLine($"  Range: A1:J100");
            Console.WriteLine($"  Time: {rangeTime}ms");

            Assert.IsTrue(rangeTime < 2000, $"Range operations time too high: {rangeTime}ms");
        }

        /// <summary>
        /// Test large worksheet formatting performance
        /// </summary>
        [Test]
        public void LargeWorksheetFormattingPerformance_ShouldBeAcceptable()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            using var workbook = workbookResult.Value;

            var addWorksheetResult = workbook.AddWorksheet("FormattingSheet");
            AssertResultSuccess(addWorksheetResult);
            var worksheet = addWorksheetResult.Value;

            const int rows = 500;
            const int columns = 5;

            // Test column width setting performance
            var columnWidthTime = MeasureExecutionTime(() =>
            {
                for (int i = 0; i < columns; i++)
                {
                    var columnName = GetCellReference(1, (uint)(i + 1)).Replace("1", "");
                    var result = worksheet.SetColumnWidth(columnName, 20.0);
                    AssertResultSuccess(result);
                }
            });

            Console.WriteLine($"Column Width Setting Performance:");
            Console.WriteLine($"  Columns: {columns}");
            Console.WriteLine($"  Time: {columnWidthTime}ms");

            Assert.IsTrue(columnWidthTime < 1000, $"Column width setting time too high: {columnWidthTime}ms");

            // Test row height setting performance
            var rowHeightTime = MeasureExecutionTime(() =>
            {
                for (int i = 1; i <= rows; i++)
                {
                    var result = worksheet.SetRowHeight(i, 25.0);
                    AssertResultSuccess(result);
                }
            });

            Console.WriteLine($"Row Height Setting Performance:");
            Console.WriteLine($"  Rows: {rows}");
            Console.WriteLine($"  Time: {rowHeightTime}ms");

            Assert.IsTrue(rowHeightTime < 2000, $"Row height setting time too high: {rowHeightTime}ms");

            // Test auto-fit performance
            var autoFitTime = MeasureExecutionTime(() =>
            {
                for (int i = 0; i < columns; i++)
                {
                    var columnName = GetCellReference(1, (uint)(i + 1)).Replace("1", "");
                    var result = worksheet.AutoFitColumn(columnName);
                    AssertResultSuccess(result);
                }
            });

            Console.WriteLine($"Auto-Fit Performance:");
            Console.WriteLine($"  Columns: {columns}");
            Console.WriteLine($"  Time: {autoFitTime}ms");

            Assert.IsTrue(autoFitTime < 3000, $"Auto-fit time too high: {autoFitTime}ms");
        }

        #endregion

        #region Memory Usage Patterns Tests

        /// <summary>
        /// Test memory usage patterns
        /// </summary>
        [Test]
        public void MemoryUsagePatterns_ShouldBeReasonable()
        {
            // Arrange
            const int iterations = 10;
            var memoryUsages = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var memoryUsage = MeasureMemoryUsage(() =>
                {
                    var workbookResult = _workbookFactory.CreateWorkbook();
                    AssertResultSuccess(workbookResult);
                    var workbook = workbookResult.Value;

                    // Add some data
                    var addWorksheetResult = workbook.AddWorksheet($"Sheet{i}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    var testData = CreateTestDataArray(100, 10);
                    var setDataResult = worksheet.SetData("A1", testData);
                    AssertResultSuccess(setDataResult);

                    workbook.Dispose();
                });
                memoryUsages.Add(memoryUsage);
            }

            // Assert
            var averageMemory = memoryUsages.Average();
            var maxMemory = memoryUsages.Max();

            Console.WriteLine($"Memory Usage Patterns:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Memory: {averageMemory / 1024:F2}KB");
            Console.WriteLine($"  Max Memory: {maxMemory / 1024:F2}KB");

            Assert.IsTrue(averageMemory < 5 * 1024 * 1024, $"Average memory usage too high: {averageMemory / 1024:F2}KB");
            Assert.IsTrue(maxMemory < 10 * 1024 * 1024, $"Max memory usage too high: {maxMemory / 1024:F2}KB");
        }

        /// <summary>
        /// Test memory usage with large workbooks
        /// </summary>
        [Test]
        public void MemoryUsageWithLargeWorkbooks_ShouldBeReasonable()
        {
            // Arrange
            var largeFilePath = CreateLargeExcelFile(2000, 20);

            // Act
            var memoryUsage = MeasureMemoryUsage(() =>
            {
                var openResult = _workbookFactory.OpenWorkbook(largeFilePath);
                AssertResultSuccess(openResult);
                using var workbook = openResult.Value;

                // Perform some operations
                var addWorksheetResult = workbook.AddWorksheet("AdditionalSheet");
                AssertResultSuccess(addWorksheetResult);
                var worksheet = addWorksheetResult.Value;

                var testData = CreateTestDataArray(500, 10);
                var setDataResult = worksheet.SetData("A1", testData);
                AssertResultSuccess(setDataResult);
            });

            // Assert
            Console.WriteLine($"Memory Usage with Large Workbook:");
            Console.WriteLine($"  File: 2000x20 cells");
            Console.WriteLine($"  Memory: {memoryUsage / 1024:F2}KB");

            Assert.IsTrue(memoryUsage < 50 * 1024 * 1024, $"Memory usage too high: {memoryUsage / 1024:F2}KB");
        }

        #endregion

        #region Concurrent Operations Tests

        /// <summary>
        /// Test concurrent operations performance
        /// </summary>
        [Test]
        public async Task ConcurrentOperationsPerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int concurrentTasks = 10;
            const int operationsPerTask = 10;

            // Act
            var tasks = new List<Task<long>>();
            for (int i = 0; i < concurrentTasks; i++)
            {
                var task = Task.Run(() =>
                {
                    return MeasureExecutionTime(() =>
                    {
                        for (int j = 0; j < operationsPerTask; j++)
                        {
                            var workbookResult = _workbookFactory.CreateWorkbook();
                            AssertResultSuccess(workbookResult);
                            workbookResult.Value.Dispose();
                        }
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Concurrent Operations Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 2000, $"Average concurrent time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 5000, $"Max concurrent time too high: {maxTime}ms");
        }

        /// <summary>
        /// Test concurrent worksheet operations performance
        /// </summary>
        [Test]
        public async Task ConcurrentWorksheetOperationsPerformance_ShouldBeAcceptable()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            using var workbook = workbookResult.Value;

            const int concurrentTasks = 5;
            const int operationsPerTask = 20;

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
                            var addWorksheetResult = workbook.AddWorksheet($"Sheet{taskIndex}_{j}");
                            AssertResultSuccess(addWorksheetResult);
                            var worksheet = addWorksheetResult.Value;

                            var setCellResult = worksheet.SetCellValue("A1", $"Value{taskIndex}_{j}");
                            AssertResultSuccess(setCellResult);
                        }
                    });
                });
                tasks.Add(task);
            }

            var times = await Task.WhenAll(tasks);

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Concurrent Worksheet Operations Performance:");
            Console.WriteLine($"  Concurrent Tasks: {concurrentTasks}");
            Console.WriteLine($"  Operations per Task: {operationsPerTask}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 3000, $"Average concurrent worksheet time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 8000, $"Max concurrent worksheet time too high: {maxTime}ms");
        }

        #endregion

        #region Stress Scenarios Tests

        /// <summary>
        /// Test stress scenarios performance
        /// </summary>
        [Test]
        public void StressScenariosPerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int stressIterations = 1000;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                for (int i = 0; i < stressIterations; i++)
                {
                    var workbookResult = _workbookFactory.CreateWorkbook();
                    AssertResultSuccess(workbookResult);
                    var workbook = workbookResult.Value;

                    // Add worksheet
                    var addWorksheetResult = workbook.AddWorksheet($"StressSheet{i}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    // Set some data
                    var setCellResult = worksheet.SetCellValue("A1", $"Stress{i}");
                    AssertResultSuccess(setCellResult);

                    // Get range
                    var rangeResult = worksheet.GetRange("A1:C3");
                    AssertResultSuccess(rangeResult);
                    var range = rangeResult.Value;

                    // Set range value
                    var setRangeResult = range.SetValue($"Range{i}");
                    AssertResultSuccess(setRangeResult);

                    // Clear range
                    var clearRangeResult = range.Clear();
                    AssertResultSuccess(clearRangeResult);

                    workbook.Dispose();
                }
            });

            // Assert
            var averageTime = (double)time / stressIterations;

            Console.WriteLine($"Stress Scenarios Performance:");
            Console.WriteLine($"  Iterations: {stressIterations}");
            Console.WriteLine($"  Total Time: {time}ms");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");

            Assert.IsTrue(averageTime < 10, $"Average stress time too high: {averageTime:F2}ms");
            Assert.IsTrue(time < 30000, $"Total stress time too high: {time}ms");
        }

        #endregion

        #region Resource Cleanup Performance Tests

        /// <summary>
        /// Test resource cleanup performance
        /// </summary>
        [Test]
        public void ResourceCleanupPerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int iterations = 100;
            var times = new List<long>();

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var time = MeasureExecutionTime(() =>
                {
                    var workbookResult = _workbookFactory.CreateWorkbook();
                    AssertResultSuccess(workbookResult);
                    var workbook = workbookResult.Value;

                    // Add some data
                    var addWorksheetResult = workbook.AddWorksheet($"CleanupSheet{i}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    var testData = CreateTestDataArray(50, 5);
                    var setDataResult = worksheet.SetData("A1", testData);
                    AssertResultSuccess(setDataResult);

                    // Dispose
                    workbook.Dispose();
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Resource Cleanup Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 50, $"Average cleanup time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 200, $"Max cleanup time too high: {maxTime}ms");
        }

        #endregion

        #region Builder Pattern Performance Tests

        /// <summary>
        /// Test builder pattern performance
        /// </summary>
        [Test]
        public void BuilderPatternPerformance_ShouldBeAcceptable()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            using var workbook = workbookResult.Value;

            const int iterations = 100;

            // Act
            var times = new List<long>();
            for (int i = 0; i < iterations; i++)
            {
                var time = MeasureExecutionTime(() =>
                {
                    var addWorksheetResult = workbook.AddWorksheet($"BuilderSheet{i}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    var builderResult = worksheet.CreateBuilder();
                    AssertResultSuccess(builderResult);
                    var builder = builderResult.Value;

                    // Test fluent API chaining
                    var configuredBuilder = builder
                        .WithName($"Test Worksheet {i}")
                        .WithVisible(true)
                        .WithTabColor(0xFF0000)
                        .WithData("A1", CreateTestDataArray(10, 5))
                        .WithFormula("A11", "=SUM(A1:A10)")
                        .WithComment("A1", $"Test comment {i}")
                        .WithHyperlink("A2", "https://example.com", "Example Link")
                        .WithNamedRange($"TestRange{i}", "A1:E10")
                        .WithDataValidation("A1:A10", "List", "Option1,Option2,Option3")
                        .WithConditionalFormatting("A1:E10", "CellValue", "greaterThan", "5")
                        .WithPrintSettings(true, true, true, true)
                        .WithPageSetup(PaperSize.A4, PageOrientation.Portrait, 1.0, 1.0, 1.0, 1.0)
                        .WithHeaderFooter($"Test Header {i}", $"Test Footer {i}")
                        .WithMargins(1.0, 1.0, 1.0, 1.0, 1.0, 1.0)
                        .WithScaling(100, 100)
                        .WithPrintArea("A1:E10")
                        .WithPrintTitles("1:1", "A:A")
                        .WithGridlines(true)
                        .WithRowColumnHeaders(true)
                        .WithBlackAndWhite(false)
                        .WithDraftQuality(false)
                        .WithComments(PrintComments.None)
                        .WithErrorAs(PrintErrorAs.Blank)
                        .WithPageOrder(PrintPageOrder.DownThenOver)
                        .WithPrintQuality(600)
                        .WithFirstPageNumber(1)
                        .WithUseFirstPageNumber(true)
                        .WithHorizontalCentered(true)
                        .WithVerticalCentered(true)
                        .WithFitToPagesWide(1)
                        .WithFitToPagesTall(1)
                        .WithZoom(100)
                        .WithScale(100)
                        .WithResolution(600)
                        .WithColorMode(ColorMode.Color)
                        .WithDpi(600)
                        .WithPaperSize(PaperSize.A4)
                        .WithOrientation(PageOrientation.Portrait)
                        .WithCopies(1)
                        .WithCollate(true)
                        .WithPrintToFile(false)
                        .WithActivePrinter("Microsoft Print to PDF")
                        .WithPrinterName("Microsoft Print to PDF")
                        .WithDriverName("Microsoft Print to PDF")
                        .WithPortName("FILE:")
                        .WithPrintRange(PrintRange.All);

                    var buildResult = configuredBuilder.Build();
                    AssertResultSuccess(buildResult);
                });
                times.Add(time);
            }

            // Assert
            var averageTime = times.Average();
            var maxTime = times.Max();

            Console.WriteLine($"Builder Pattern Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");
            Console.WriteLine($"  Max Time: {maxTime}ms");

            Assert.IsTrue(averageTime < 100, $"Average builder time too high: {averageTime:F2}ms");
            Assert.IsTrue(maxTime < 500, $"Max builder time too high: {maxTime}ms");
        }

        #endregion

        #region Validation Performance Tests

        /// <summary>
        /// Test validation performance
        /// </summary>
        [Test]
        public void ValidationPerformance_ShouldBeAcceptable()
        {
            // Arrange
            var workbookResult = _workbookFactory.CreateWorkbook();
            AssertResultSuccess(workbookResult);
            using var workbook = workbookResult.Value;

            const int iterations = 1000;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    var addWorksheetResult = workbook.AddWorksheet($"ValidationSheet{i}");
                    AssertResultSuccess(addWorksheetResult);
                    var worksheet = addWorksheetResult.Value;

                    // Test validation operations
                    var rangeResult = worksheet.GetRange("A1:C3");
                    AssertResultSuccess(rangeResult);
                    var range = rangeResult.Value;

                    var setValueResult = range.SetValue($"Validation{i}");
                    AssertResultSuccess(setValueResult);

                    var getValueResult = range.GetValue();
                    AssertResultSuccess(getValueResult);
                }
            });

            // Assert
            var averageTime = (double)time / iterations;

            Console.WriteLine($"Validation Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Total Time: {time}ms");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");

            Assert.IsTrue(averageTime < 5, $"Average validation time too high: {averageTime:F2}ms");
            Assert.IsTrue(time < 10000, $"Total validation time too high: {time}ms");
        }

        #endregion

        #region Error Handling Performance Tests

        /// <summary>
        /// Test error handling performance
        /// </summary>
        [Test]
        public void ErrorHandlingPerformance_ShouldBeAcceptable()
        {
            // Arrange
            const int iterations = 1000;

            // Act
            var time = MeasureExecutionTime(() =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    // Test various error scenarios
                    var invalidWorkbookResult = _workbookFactory.CreateWorkbook(null);
                    AssertResultFailure(invalidWorkbookResult);

                    var invalidFileResult = _workbookFactory.OpenWorkbook("nonexistent.xlsx");
                    AssertResultFailure(invalidFileResult);

                    using var invalidStream = new MemoryStream();
                    var invalidStreamResult = _workbookFactory.OpenWorkbook(invalidStream);
                    AssertResultFailure(invalidStreamResult);
                }
            });

            // Assert
            var averageTime = (double)time / iterations;

            Console.WriteLine($"Error Handling Performance:");
            Console.WriteLine($"  Iterations: {iterations}");
            Console.WriteLine($"  Total Time: {time}ms");
            Console.WriteLine($"  Average Time: {averageTime:F2}ms");

            Assert.IsTrue(averageTime < 2, $"Average error handling time too high: {averageTime:F2}ms");
            Assert.IsTrue(time < 5000, $"Total error handling time too high: {time}ms");
        }

        #endregion
    }

    #region Test Helper Classes

    /// <summary>
    /// Test implementation of IWorkbookLogger for performance testing
    /// </summary>
    public class TestWorkbookLogger : IWorkbookLogger
    {
        public void LogWorkbookOperation(string operation, string details)
        {
            // Test implementation - minimal overhead for performance testing
        }

        public void LogError(string error, Exception exception = null)
        {
            // Test implementation - minimal overhead for performance testing
        }

        public void LogPerformance(string operation, long milliseconds)
        {
            // Test implementation - minimal overhead for performance testing
        }

        public void LogWarning(string warning, string details = null)
        {
            // Test implementation - minimal overhead for performance testing
        }
    }

    /// <summary>
    /// Test implementation of IWorkbookValidator for performance testing
    /// </summary>
    public class TestWorkbookValidator : IWorkbookValidator
    {
        public Result<bool> ValidateWorkbook(IWorkbookDomain workbook)
        {
            return Result<bool>.Success(true);
        }

        public Result<bool> ValidateWorksheet(IWorksheet worksheet)
        {
            return Result<bool>.Success(true);
        }

        public Result<bool> ValidateRange(IRange range)
        {
            return Result<bool>.Success(true);
        }

        public Result<bool> ValidateCellReference(string cellReference)
        {
            return Result<bool>.Success(true);
        }
    }

    #endregion
}
