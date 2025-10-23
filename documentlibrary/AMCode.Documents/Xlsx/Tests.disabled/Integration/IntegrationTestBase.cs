using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Integration
{
    /// <summary>
    /// Base class for integration tests providing common setup, teardown, and helper methods
    /// </summary>
    public abstract class IntegrationTestBase
    {
        /// <summary>
        /// Gets the test data directory path
        /// </summary>
        protected string TestDataDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData");

        /// <summary>
        /// Gets the test output directory path
        /// </summary>
        protected string TestOutputDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "TestOutput");

        /// <summary>
        /// Gets the integration test data directory path
        /// </summary>
        protected string IntegrationTestDataDirectory => Path.Combine(TestDataDirectory, "Integration");

        /// <summary>
        /// Gets the performance test data directory path
        /// </summary>
        protected string PerformanceTestDataDirectory => Path.Combine(TestDataDirectory, "Performance");

        /// <summary>
        /// List of test files created during tests for cleanup
        /// </summary>
        protected List<string> CreatedTestFiles { get; private set; }

        /// <summary>
        /// List of test streams created during tests for cleanup
        /// </summary>
        protected List<Stream> CreatedTestStreams { get; private set; }

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            // Initialize cleanup lists
            CreatedTestFiles = new List<string>();
            CreatedTestStreams = new List<Stream>();

            // Ensure test directories exist
            Directory.CreateDirectory(TestDataDirectory);
            Directory.CreateDirectory(TestOutputDirectory);
            Directory.CreateDirectory(IntegrationTestDataDirectory);
            Directory.CreateDirectory(PerformanceTestDataDirectory);
        }

        /// <summary>
        /// Teardown method called after each test
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            // Dispose all created streams
            foreach (var stream in CreatedTestStreams)
            {
                try
                {
                    stream?.Dispose();
                }
                catch
                {
                    // Ignore disposal errors
                }
            }
            CreatedTestStreams.Clear();

            // Clean up all created test files
            foreach (var filePath in CreatedTestFiles)
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
            CreatedTestFiles.Clear();

            // Clean up test output files
            if (Directory.Exists(TestOutputDirectory))
            {
                var files = Directory.GetFiles(TestOutputDirectory);
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        #region Test File Creation Helpers

        /// <summary>
        /// Creates a temporary file path for testing
        /// </summary>
        /// <param name="extension">File extension (default: .xlsx)</param>
        /// <returns>Path to temporary file</returns>
        protected string CreateTempFilePath(string extension = ".xlsx")
        {
            var fileName = $"test_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(TestOutputDirectory, fileName);
            CreatedTestFiles.Add(filePath);
            return filePath;
        }

        /// <summary>
        /// Creates a temporary file path in integration test data directory
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Path to temporary file</returns>
        protected string CreateIntegrationTestFilePath(string fileName)
        {
            var filePath = Path.Combine(IntegrationTestDataDirectory, fileName);
            CreatedTestFiles.Add(filePath);
            return filePath;
        }

        /// <summary>
        /// Creates a temporary file path in performance test data directory
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Path to temporary file</returns>
        protected string CreatePerformanceTestFilePath(string fileName)
        {
            var filePath = Path.Combine(PerformanceTestDataDirectory, fileName);
            CreatedTestFiles.Add(filePath);
            return filePath;
        }

        #endregion

        #region Test Stream Creation Helpers

        /// <summary>
        /// Creates a valid Excel stream for testing
        /// </summary>
        /// <returns>MemoryStream containing valid Excel data</returns>
        protected Stream CreateValidExcelStream()
        {
            var stream = new MemoryStream();
            CreatedTestStreams.Add(stream);

            // Create a minimal valid Excel file
            using (var package = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = package.AddWorkbookPart();
                workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                var worksheetPart = workbookPart.AddNewPart<DocumentFormat.OpenXml.Spreadsheet.WorksheetPart>();
                worksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(new DocumentFormat.OpenXml.Spreadsheet.SheetData());
                workbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
                var sheets = workbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                var sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet1"
                };
                sheets.AppendChild(sheet);
            }

            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Creates a valid Excel file for testing
        /// </summary>
        /// <param name="filePath">Path where to create the file</param>
        /// <returns>Path to created file</returns>
        protected string CreateValidExcelFile(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = CreateTempFilePath();
            }

            using (var stream = CreateValidExcelStream())
            {
                using (var fileStream = File.Create(filePath))
                {
                    stream.CopyTo(fileStream);
                }
            }

            CreatedTestFiles.Add(filePath);
            return filePath;
        }

        /// <summary>
        /// Creates a large Excel file for performance testing
        /// </summary>
        /// <param name="rows">Number of rows to create</param>
        /// <param name="columns">Number of columns to create</param>
        /// <returns>Path to created file</returns>
        protected string CreateLargeExcelFile(int rows = 1000, int columns = 10)
        {
            var filePath = CreatePerformanceTestFilePath($"large_{rows}x{columns}.xlsx");

            using (var package = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(filePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = package.AddWorkbookPart();
                workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                var worksheetPart = workbookPart.AddNewPart<DocumentFormat.OpenXml.Spreadsheet.WorksheetPart>();
                var worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();

                // Create rows and cells with data
                for (uint rowIndex = 1; rowIndex <= rows; rowIndex++)
                {
                    var row = new DocumentFormat.OpenXml.Spreadsheet.Row() { RowIndex = rowIndex };
                    
                    for (uint columnIndex = 1; columnIndex <= columns; columnIndex++)
                    {
                        var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell()
                        {
                            CellReference = GetCellReference(rowIndex, columnIndex),
                            DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String,
                            CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue($"R{rowIndex}C{columnIndex}")
                        };
                        row.AppendChild(cell);
                    }
                    
                    sheetData.AppendChild(row);
                }

                worksheet.AppendChild(sheetData);
                worksheetPart.Worksheet = worksheet;

                workbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
                var sheets = workbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                var sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "LargeSheet"
                };
                sheets.AppendChild(sheet);
            }

            CreatedTestFiles.Add(filePath);
            return filePath;
        }

        #endregion

        #region Test Data Generation Helpers

        /// <summary>
        /// Creates test data array for worksheet testing
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>2D array with test data</returns>
        protected object[,] CreateTestDataArray(int rows, int columns)
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
        /// Creates test data list for worksheet testing
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>List of lists with test data</returns>
        protected List<List<object>> CreateTestDataList(int rows, int columns)
        {
            var data = new List<List<object>>();
            for (int i = 0; i < rows; i++)
            {
                var row = new List<object>();
                for (int j = 0; j < columns; j++)
                {
                    row.Add($"R{i + 1}C{j + 1}");
                }
                data.Add(row);
            }
            return data;
        }

        /// <summary>
        /// Creates test formulas for worksheet testing
        /// </summary>
        /// <param name="count">Number of formulas to create</param>
        /// <returns>Array of test formulas</returns>
        protected string[] CreateTestFormulas(int count)
        {
            var formulas = new string[count];
            for (int i = 0; i < count; i++)
            {
                formulas[i] = $"=A{i + 1}+B{i + 1}";
            }
            return formulas;
        }

        #endregion

        #region Validation Helpers

        /// <summary>
        /// Validates that a Result is successful
        /// </summary>
        /// <typeparam name="T">Result value type</typeparam>
        /// <param name="result">Result to validate</param>
        protected void AssertResultSuccess<T>(Result<T> result)
        {
            Assert.IsTrue(result.IsSuccess, $"Expected success but got failure: {result.Error}");
        }

        /// <summary>
        /// Validates that a Result is successful and has a value
        /// </summary>
        /// <typeparam name="T">Result value type</typeparam>
        /// <param name="result">Result to validate</param>
        /// <param name="expectedValue">Expected value</param>
        protected void AssertResultSuccessWithValue<T>(Result<T> result, T expectedValue)
        {
            AssertResultSuccess(result);
            Assert.AreEqual(expectedValue, result.Value);
        }

        /// <summary>
        /// Validates that a Result is a failure
        /// </summary>
        /// <typeparam name="T">Result value type</typeparam>
        /// <param name="result">Result to validate</param>
        protected void AssertResultFailure<T>(Result<T> result)
        {
            Assert.IsFalse(result.IsSuccess, "Expected failure but got success");
        }

        /// <summary>
        /// Validates that a Result is a failure with specific error
        /// </summary>
        /// <typeparam name="T">Result value type</typeparam>
        /// <param name="result">Result to validate</param>
        /// <param name="expectedError">Expected error message</param>
        protected void AssertResultFailureWithError<T>(Result<T> result, string expectedError)
        {
            AssertResultFailure(result);
            Assert.IsTrue(result.Error.Contains(expectedError), $"Expected error containing '{expectedError}' but got '{result.Error}'");
        }

        /// <summary>
        /// Validates that a file exists and is not empty
        /// </summary>
        /// <param name="filePath">Path to file to validate</param>
        protected void AssertFileExistsAndNotEmpty(string filePath)
        {
            Assert.IsTrue(File.Exists(filePath), $"File does not exist: {filePath}");
            var fileInfo = new FileInfo(filePath);
            Assert.IsTrue(fileInfo.Length > 0, $"File is empty: {filePath}");
        }

        /// <summary>
        /// Validates that a stream is valid and not empty
        /// </summary>
        /// <param name="stream">Stream to validate</param>
        protected void AssertStreamIsValidAndNotEmpty(Stream stream)
        {
            Assert.IsNotNull(stream, "Stream is null");
            Assert.IsTrue(stream.Length > 0, "Stream is empty");
            Assert.IsTrue(stream.CanRead, "Stream is not readable");
        }

        #endregion

        #region Performance Testing Helpers

        /// <summary>
        /// Measures execution time of an action
        /// </summary>
        /// <param name="action">Action to measure</param>
        /// <returns>Execution time in milliseconds</returns>
        protected long MeasureExecutionTime(Action action)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Measures execution time of an async action
        /// </summary>
        /// <param name="action">Async action to measure</param>
        /// <returns>Execution time in milliseconds</returns>
        protected async Task<long> MeasureExecutionTimeAsync(Func<Task> action)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            await action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Measures memory usage before and after an action
        /// </summary>
        /// <param name="action">Action to measure</param>
        /// <returns>Memory usage difference in bytes</returns>
        protected long MeasureMemoryUsage(Action action)
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

        #region Utility Methods

        /// <summary>
        /// Gets cell reference from row and column indices
        /// </summary>
        /// <param name="row">Row index (1-based)</param>
        /// <param name="column">Column index (1-based)</param>
        /// <returns>Cell reference (e.g., "A1", "B2")</returns>
        protected string GetCellReference(uint row, uint column)
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
        /// Waits for a specified amount of time
        /// </summary>
        /// <param name="milliseconds">Milliseconds to wait</param>
        protected void Wait(int milliseconds)
        {
            System.Threading.Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// Waits for a specified amount of time asynchronously
        /// </summary>
        /// <param name="milliseconds">Milliseconds to wait</param>
        protected async Task WaitAsync(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }

        #endregion
    }
}
