using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using AMCode.Documents.Xlsx;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit
{
    /// <summary>
    /// Base class for unit tests providing common setup, teardown, and helper methods
    /// </summary>
    public abstract class UnitTestBase
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
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            // Ensure test directories exist
            Directory.CreateDirectory(TestDataDirectory);
            Directory.CreateDirectory(TestOutputDirectory);
        }

        /// <summary>
        /// Teardown method called after each test
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
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

        /// <summary>
        /// Creates a mock IWorkbookEngine for testing
        /// </summary>
        /// <returns>A mock IWorkbookEngine</returns>
        protected IWorkbookEngine CreateMockWorkbookEngine()
        {
            return new MockWorkbookEngine();
        }

        /// <summary>
        /// Creates a mock IWorkbookLogger for testing
        /// </summary>
        /// <returns>A mock IWorkbookLogger</returns>
        protected IWorkbookLogger CreateMockWorkbookLogger()
        {
            return new MockWorkbookLogger();
        }

        /// <summary>
        /// Creates a mock IWorkbookValidator for testing
        /// </summary>
        /// <returns>A mock IWorkbookValidator</returns>
        protected IWorkbookValidator CreateMockWorkbookValidator()
        {
            return new MockWorkbookValidator();
        }

        /// <summary>
        /// Creates a mock IWorkbook for testing
        /// </summary>
        /// <returns>A mock IWorkbook</returns>
        protected IWorkbook CreateMockWorkbook()
        {
            return new MockWorkbook();
        }

        /// <summary>
        /// Creates a mock IWorkbookProperties for testing
        /// </summary>
        /// <returns>A mock IWorkbookProperties</returns>
        protected IWorkbookProperties CreateMockWorkbookProperties()
        {
            return new MockWorkbookProperties();
        }

        /// <summary>
        /// Creates a mock IWorksheet for testing
        /// </summary>
        /// <returns>A mock IWorksheet</returns>
        protected IWorksheet CreateMockWorksheet()
        {
            return new MockWorksheet();
        }

        /// <summary>
        /// Creates a mock IRange for testing
        /// </summary>
        /// <returns>A mock IRange</returns>
        protected IRange CreateMockRange()
        {
            return new MockRange();
        }

        /// <summary>
        /// Asserts that a Result is successful
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="result">The result to assert</param>
        /// <param name="message">Optional message for the assertion</param>
        protected void AssertResultSuccess<T>(Result<T> result, string message = null)
        {
            Assert.IsTrue(result.IsSuccess, message ?? $"Expected success but got error: {result.Error}");
        }

        /// <summary>
        /// Asserts that a Result is a failure
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="result">The result to assert</param>
        /// <param name="message">Optional message for the assertion</param>
        protected void AssertResultFailure<T>(Result<T> result, string message = null)
        {
            Assert.IsFalse(result.IsSuccess, message ?? "Expected failure but got success");
        }

        /// <summary>
        /// Asserts that a Result is successful and returns the expected value
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="result">The result to assert</param>
        /// <param name="expectedValue">The expected value</param>
        /// <param name="message">Optional message for the assertion</param>
        protected void AssertResultSuccessWithValue<T>(Result<T> result, T expectedValue, string message = null)
        {
            AssertResultSuccess(result, message);
            Assert.AreEqual(expectedValue, result.Value, message ?? "Result value does not match expected value");
        }

        /// <summary>
        /// Asserts that a Result is a failure with the expected error message
        /// </summary>
        /// <typeparam name="T">The result type</typeparam>
        /// <param name="result">The result to assert</param>
        /// <param name="expectedError">The expected error message</param>
        /// <param name="message">Optional message for the assertion</param>
        protected void AssertResultFailureWithError<T>(Result<T> result, string expectedError, string message = null)
        {
            AssertResultFailure(result, message);
            Assert.IsTrue(result.Error.Contains(expectedError), 
                message ?? $"Expected error to contain '{expectedError}' but got '{result.Error}'");
        }

        /// <summary>
        /// Creates test data for a 2D array
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        /// <returns>A 2D array with test data</returns>
        protected object[,] CreateTestDataArray(int rows, int cols)
        {
            var data = new object[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    data[row, col] = $"Row{row + 1}Col{col + 1}";
                }
            }
            return data;
        }

        /// <summary>
        /// Creates test data for a list of lists
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        /// <returns>A list of lists with test data</returns>
        protected List<List<object>> CreateTestDataList(int rows, int cols)
        {
            var data = new List<List<object>>();
            for (int row = 0; row < rows; row++)
            {
                var rowData = new List<object>();
                for (int col = 0; col < cols; col++)
                {
                    rowData.Add($"Row{row + 1}Col{col + 1}");
                }
                data.Add(rowData);
            }
            return data;
        }

        /// <summary>
        /// Creates a temporary file path for testing
        /// </summary>
        /// <param name="extension">File extension (default: .xlsx)</param>
        /// <returns>A temporary file path</returns>
        protected string CreateTempFilePath(string extension = ".xlsx")
        {
            var fileName = $"Test_{Guid.NewGuid():N}{extension}";
            return Path.Combine(TestOutputDirectory, fileName);
        }

        /// <summary>
        /// Creates a temporary memory stream for testing
        /// </summary>
        /// <returns>A temporary memory stream</returns>
        protected MemoryStream CreateTempStream()
        {
            return new MemoryStream();
        }

        /// <summary>
        /// Validates that a file exists and has content
        /// </summary>
        /// <param name="filePath">The file path to validate</param>
        /// <param name="minSize">Minimum file size in bytes (default: 0)</param>
        protected void AssertFileExistsAndHasContent(string filePath, long minSize = 0)
        {
            Assert.IsTrue(File.Exists(filePath), $"File does not exist: {filePath}");
            var fileInfo = new FileInfo(filePath);
            Assert.IsTrue(fileInfo.Length > minSize, $"File size {fileInfo.Length} is not greater than {minSize} bytes");
        }

        /// <summary>
        /// Validates that a stream has content
        /// </summary>
        /// <param name="stream">The stream to validate</param>
        /// <param name="minSize">Minimum stream size in bytes (default: 0)</param>
        protected void AssertStreamHasContent(Stream stream, long minSize = 0)
        {
            Assert.IsNotNull(stream, "Stream is null");
            Assert.IsTrue(stream.Length > minSize, $"Stream size {stream.Length} is not greater than {minSize} bytes");
        }
    }

    #region Mock Implementations

    /// <summary>
    /// Mock implementation of IWorkbookEngine for testing
    /// </summary>
    public class MockWorkbookEngine : IWorkbookEngine
    {
        public Result<object> Create()
        {
            return Result<object>.Success(new object());
        }

        public Result<object> Open(Stream stream)
        {
            if (stream == null)
                return Result<object>.Failure("Stream is null");
            
            return Result<object>.Success(new object());
        }

        public Result<object> Open(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Result<object>.Failure("File path is null or empty");
            
            return Result<object>.Success(new object());
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbookLogger for testing
    /// </summary>
    public class MockWorkbookLogger : IWorkbookLogger
    {
        public List<string> LoggedOperations { get; } = new List<string>();
        public List<string> LoggedErrors { get; } = new List<string>();
        public List<string> LoggedWarnings { get; } = new List<string>();
        public List<(string operation, TimeSpan duration)> LoggedPerformance { get; } = new List<(string, TimeSpan)>();

        public void LogInformation(string message, Guid workbookId)
        {
            LoggedOperations.Add($"INFO: {message} (Workbook: {workbookId})");
        }

        public void LogWarning(string message, Guid workbookId)
        {
            LoggedWarnings.Add($"WARNING: {message} (Workbook: {workbookId})");
        }

        public void LogError(string message, Exception exception, Guid workbookId)
        {
            LoggedErrors.Add($"ERROR: {message} - {exception?.Message} (Workbook: {workbookId})");
        }

        public void LogWorkbookOperation(string operation, Guid workbookId)
        {
            LoggedOperations.Add($"OPERATION: {operation} (Workbook: {workbookId})");
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbookValidator for testing
    /// </summary>
    public class MockWorkbookValidator : IWorkbookValidator
    {
        public bool ShouldValidateWorkbookSuccess { get; set; } = true;
        public bool ShouldValidateWorksheetSuccess { get; set; } = true;
        public bool ShouldValidateRangeSuccess { get; set; } = true;
        public bool ShouldValidateCellReferenceSuccess { get; set; } = true;

        public Result<ValidationResult> ValidateWorkbook(IWorkbookDomain workbook)
        {
            return ShouldValidateWorkbookSuccess 
                ? Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock validation passed" }) 
                : Result<ValidationResult>.Failure("Mock validation failed");
        }

        public Result<ValidationResult> ValidateWorksheet(IWorksheet worksheet)
        {
            return ShouldValidateWorksheetSuccess 
                ? Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock worksheet validation passed" }) 
                : Result<ValidationResult>.Failure("Mock worksheet validation failed");
        }

        public Result<ValidationResult> ValidateRange(IRange range)
        {
            return ShouldValidateRangeSuccess 
                ? Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock range validation passed" }) 
                : Result<ValidationResult>.Failure("Mock range validation failed");
        }

        public Result<ValidationResult> ValidateCellReference(string cellReference)
        {
            return ShouldValidateCellReferenceSuccess 
                ? Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock cell reference validation passed" }) 
                : Result<ValidationResult>.Failure("Mock cell reference validation failed");
        }

        public Result<ValidationResult> ValidateRangeReference(string rangeReference)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock range reference validation passed" });
        }

        public Result<ValidationResult> ValidateWorksheetName(string worksheetName)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock worksheet name validation passed" });
        }

        public Result<ValidationResult> ValidateFilePath(string filePath)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock file path validation passed" });
        }

        public Result<ValidationResult> ValidateFormula(string formula)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock formula validation passed" });
        }

        public Result<ValidationResult> ValidateCellValue(object value, Type expectedType)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock cell value validation passed" });
        }

        public Result<ValidationResult> ValidateMetadata(IWorkbookMetadata metadata)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock metadata validation passed" });
        }

        public Result<ValidationResult> ValidateProtection(IWorkbookDomain workbook)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock protection validation passed" });
        }

        public Result<ValidationResult> ValidateCalculationSettings(IWorkbookDomain workbook)
        {
            return Result<ValidationResult>.Success(new ValidationResult { IsValid = true, Message = "Mock calculation settings validation passed" });
        }

        public Result<IEnumerable<ValidationRule>> GetValidationRules(ValidationType validationType)
        {
            return Result<IEnumerable<ValidationRule>>.Success(new List<ValidationRule>());
        }

        public Result SetValidationRules(ValidationType validationType, IEnumerable<ValidationRule> rules)
        {
            return Result.Success();
        }

        public Result ClearValidationRules()
        {
            return Result.Success();
        }

        public Result<IEnumerable<ValidationResult>> ValidateBatch(IEnumerable<object> items, ValidationType validationType)
        {
            return Result<IEnumerable<ValidationResult>>.Success(new List<ValidationResult>());
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbook for testing
    /// </summary>
    public class MockWorkbook : IWorkbook
    {
        public List<IWorksheet> Worksheets { get; } = new List<IWorksheet>();
        public IWorkbookProperties Properties { get; } = new MockWorkbookProperties();

        public IWorksheet AddWorksheet(string name)
        {
            var worksheet = new MockWorksheet { Name = name };
            Worksheets.Add(worksheet);
            return worksheet;
        }

        public void RemoveWorksheet(string name)
        {
            Worksheets.RemoveAll(w => w.Name == name);
        }

        public void RenameWorksheet(string oldName, string newName)
        {
            var worksheet = Worksheets.Find(w => w.Name == oldName);
            if (worksheet != null)
            {
                worksheet.Name = newName;
            }
        }

        public void SaveAs(Stream stream)
        {
            // Mock implementation - just write some data
            var data = System.Text.Encoding.UTF8.GetBytes("Mock Excel Data");
            stream.Write(data, 0, data.Length);
        }

        public void SaveAs(string filePath)
        {
            File.WriteAllText(filePath, "Mock Excel Data");
        }

        public void Dispose()
        {
            // Mock disposal
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbookProperties for testing
    /// </summary>
    public class MockWorkbookProperties : IWorkbookProperties
    {
        public string Author { get; set; } = "Test Author";
        public string Title { get; set; } = "Test Title";
        public string Subject { get; set; } = "Test Subject";
        public string Company { get; set; } = "Test Company";
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;

        public void SetProperty(string name, string value)
        {
            // Mock implementation
        }

        public string GetProperty(string name)
        {
            return name switch
            {
                "Author" => Author,
                "Title" => Title,
                "Subject" => Subject,
                "Company" => Company,
                _ => null
            };
        }
    }

    /// <summary>
    /// Mock implementation of IWorksheet for testing
    /// </summary>
    public class MockWorksheet : IWorksheet
    {
        public string Name { get; set; } = "Test Worksheet";
        public List<ICell> Cells { get; } = new List<ICell>();
        public List<IRow> Rows { get; } = new List<IRow>();
        public List<IColumn> Columns { get; } = new List<IColumn>();
        public IRange UsedRange { get; } = new MockRange();

        public IRange GetRange(string address)
        {
            return new MockRange { Address = address };
        }

        public void SetCellValue(int row, int column, object value)
        {
            // Mock implementation
        }

        public object GetCellValue(int row, int column)
        {
            return $"Cell{row},{column}";
        }

        public void Clear()
        {
            // Mock implementation
        }

        public void SetColumnWidth(int column, double width)
        {
            // Mock implementation
        }

        public void SetRowHeight(int row, double height)
        {
            // Mock implementation
        }

        public void AutoFitColumn(int column)
        {
            // Mock implementation
        }

        public void AutoFitRow(int row)
        {
            // Mock implementation
        }

        public void Activate()
        {
            // Mock implementation
        }

        public void Deactivate()
        {
            // Mock implementation
        }
    }

    /// <summary>
    /// Mock implementation of IRange for testing
    /// </summary>
    public class MockRange : IRange
    {
        public string Address { get; set; } = "A1";
        public IWorksheet Worksheet { get; set; } = new MockWorksheet();
        public int RowCount { get; set; } = 1;
        public int ColumnCount { get; set; } = 1;
        public int FirstRow { get; set; } = 1;
        public int LastRow { get; set; } = 1;
        public int FirstColumn { get; set; } = 1;
        public int LastColumn { get; set; } = 1;
        public IEnumerable<ICell> Cells { get; set; } = new List<ICell>();
        public IEnumerable<IRow> Rows { get; set; } = new List<IRow>();
        public IEnumerable<IColumn> Columns { get; set; } = new List<IColumn>();

        public Result<ICell> GetCell(int rowIndex, int columnIndex)
        {
            return Result<ICell>.Success(new MockCell { Row = rowIndex, Column = columnIndex });
        }

        public Result<ICell> GetCell(string cellAddress)
        {
            return Result<ICell>.Success(new MockCell { Address = cellAddress });
        }

        public Result<IRow> GetRow(int rowIndex)
        {
            return Result<IRow>.Success(new MockRow { Index = rowIndex });
        }

        public Result<IColumn> GetColumn(int columnIndex)
        {
            return Result<IColumn>.Success(new MockColumn { Index = columnIndex });
        }

        public Result<IRange> GetSubRange(int startRow, int startColumn, int endRow, int endColumn)
        {
            return Result<IRange>.Success(new MockRange { Address = $"{startRow}:{startColumn}:{endRow}:{endColumn}" });
        }

        public Result<IRange> GetSubRange(string address)
        {
            return Result<IRange>.Success(new MockRange { Address = address });
        }

        public Result SetValue(object value)
        {
            return Result.Success();
        }

        public Result SetValue<T>(T value)
        {
            return Result.Success();
        }

        public Result Clear()
        {
            return Result.Success();
        }

        public Result ClearFormatting()
        {
            return Result.Success();
        }

        public Result ClearAll()
        {
            return Result.Success();
        }

        public Result CopyTo(IRange destination)
        {
            return Result.Success();
        }

        public Result CopyTo(IRange destination, CopyOptions options)
        {
            return Result.Success();
        }

        public bool IntersectsWith(IRange other)
        {
            return false;
        }

        public bool Contains(IRange other)
        {
            return false;
        }

        public bool Contains(string address)
        {
            return false;
        }

        public bool Contains(int row, int column)
        {
            return false;
        }

        public Result<IRange> Intersect(IRange other)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> Union(IRange other)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> Offset(int rowOffset, int columnOffset)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> Resize(int rowCount, int columnCount)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> GetUsedRange()
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> GetEntireRow(int rowIndex)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> GetEntireColumn(int columnIndex)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> GetEntireRow(string rowReference)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public Result<IRange> GetEntireColumn(string columnReference)
        {
            return Result<IRange>.Success(new MockRange());
        }

        public string GetAddress()
        {
            return Address;
        }

        public string GetAddressR1C1()
        {
            return Address;
        }

        public string GetAddress(AddressOptions options)
        {
            return Address;
        }

        public string GetAddressR1C1(AddressOptions options)
        {
            return Address;
        }
    }

    /// <summary>
    /// Mock implementation of IRow for testing
    /// </summary>
    public class MockRow : IRow
    {
        public int Index { get; set; } = 1;
        public double Height { get; set; } = 15.0;
        public bool IsVisible { get; set; } = true;
        public IEnumerable<ICell> Cells { get; set; } = new List<ICell>();

        public Result<ICell> GetCell(int columnIndex)
        {
            return Result<ICell>.Success(new MockCell { Row = Index, Column = columnIndex });
        }

        public Result<ICell> GetCell(string columnReference)
        {
            return Result<ICell>.Success(new MockCell { Address = $"{columnReference}{Index}" });
        }

        public Result SetHeight(double height)
        {
            Height = height;
            return Result.Success();
        }

        public Result<double> GetHeight()
        {
            return Result<double>.Success(Height);
        }

        public Result AutoFit()
        {
            return Result.Success();
        }

        public Result SetVisibility(bool isVisible)
        {
            IsVisible = isVisible;
            return Result.Success();
        }

        public Result<bool> GetVisibility()
        {
            return Result<bool>.Success(IsVisible);
        }
    }

    /// <summary>
    /// Mock implementation of IColumn for testing
    /// </summary>
    public class MockColumn : IColumn
    {
        public int Index { get; set; } = 1;
        public double Width { get; set; } = 8.43;
        public bool IsVisible { get; set; } = true;
        public IEnumerable<ICell> Cells { get; set; } = new List<ICell>();

        public Result<ICell> GetCell(int rowIndex)
        {
            return Result<ICell>.Success(new MockCell { Row = rowIndex, Column = Index });
        }

        public Result<ICell> GetCell(string rowReference)
        {
            return Result<ICell>.Success(new MockCell { Address = $"{GetColumnLetter(Index)}{rowReference}" });
        }

        public Result SetWidth(double width)
        {
            Width = width;
            return Result.Success();
        }

        public Result<double> GetWidth()
        {
            return Result<double>.Success(Width);
        }

        public Result AutoFit()
        {
            return Result.Success();
        }

        public Result SetVisibility(bool isVisible)
        {
            IsVisible = isVisible;
            return Result.Success();
        }

        public Result<bool> GetVisibility()
        {
            return Result<bool>.Success(IsVisible);
        }

        private string GetColumnLetter(int columnIndex)
        {
            string columnLetter = "";
            while (columnIndex > 0)
            {
                columnIndex--;
                columnLetter = (char)('A' + columnIndex % 26) + columnLetter;
                columnIndex /= 26;
            }
            return columnLetter;
        }
    }

    /// <summary>
    /// Mock implementation of ICell for testing
    /// </summary>
    public class MockCell : ICell
    {
        public string Address { get; set; } = "A1";
        public int Row { get; set; } = 1;
        public int Column { get; set; } = 1;
        public object Value { get; set; } = "Test Cell Value";
        public string Formula { get; set; } = "=1+1";
        public CellDataType DataType { get; set; } = CellDataType.String;
        public bool HasFormula { get; set; } = false;
        public bool IsEmpty { get; set; } = false;
        public bool IsMerged { get; set; } = false;
        public IWorksheet Worksheet { get; set; } = new MockWorksheet();

        public Result Clear()
        {
            Value = null;
            Formula = null;
            return Result.Success();
        }

        public Result ClearFormatting()
        {
            return Result.Success();
        }

        public Result ClearAll()
        {
            Value = null;
            Formula = null;
            return Result.Success();
        }
    }

    #endregion

    #region Supporting Types and Enums

    /// <summary>
    /// Mock implementation of CopyOptions for testing
    /// </summary>
    public class CopyOptions
    {
        public bool CopyValues { get; set; } = true;
        public bool CopyFormulas { get; set; } = true;
        public bool CopyFormatting { get; set; } = true;
    }

    /// <summary>
    /// Mock implementation of AddressOptions for testing
    /// </summary>
    public class AddressOptions
    {
        public bool IncludeSheetName { get; set; } = false;
        public bool UseR1C1Notation { get; set; } = false;
        public bool AbsoluteReference { get; set; } = false;
    }

    #endregion
}
