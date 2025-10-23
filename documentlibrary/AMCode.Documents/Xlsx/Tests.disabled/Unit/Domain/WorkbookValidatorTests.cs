using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Domain.Validators;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Domain
{
    /// <summary>
    /// Unit tests for WorkbookValidator class
    /// </summary>
    [TestFixture]
    public class WorkbookValidatorTests : UnitTestBase
    {
        private WorkbookValidator _validator;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _validator = new WorkbookValidator();
        }

        #region ValidateWorkbook Tests

        /// <summary>
        /// Test ValidateWorkbook with valid workbook should succeed
        /// </summary>
        [Test]
        public void ValidateWorkbook_WithValidWorkbook_ShouldSucceed()
        {
            // Arrange
            var workbook = CreateValidWorkbook();

            // Act
            var result = _validator.ValidateWorkbook(workbook);

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(result.Value);
        }

        /// <summary>
        /// Test ValidateWorkbook with null workbook should fail
        /// </summary>
        [Test]
        public void ValidateWorkbook_WithNullWorkbook_ShouldFail()
        {
            // Act
            var result = _validator.ValidateWorkbook(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Workbook cannot be null");
        }

        /// <summary>
        /// Test ValidateWorkbook with workbook having null content should fail
        /// </summary>
        [Test]
        public void ValidateWorkbook_WithNullContent_ShouldFail()
        {
            // Arrange
            var workbook = CreateWorkbookWithNullContent();

            // Act
            var result = _validator.ValidateWorkbook(workbook);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Workbook content cannot be null");
        }

        /// <summary>
        /// Test ValidateWorkbook with workbook having null metadata should fail
        /// </summary>
        [Test]
        public void ValidateWorkbook_WithNullMetadata_ShouldFail()
        {
            // Arrange
            var workbook = CreateWorkbookWithNullMetadata();

            // Act
            var result = _validator.ValidateWorkbook(workbook);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Workbook metadata cannot be null");
        }

        #endregion

        #region ValidateWorksheet Tests

        /// <summary>
        /// Test ValidateWorksheet with valid worksheet should succeed
        /// </summary>
        [Test]
        public void ValidateWorksheet_WithValidWorksheet_ShouldSucceed()
        {
            // Arrange
            var worksheet = CreateValidWorksheet();

            // Act
            var result = _validator.ValidateWorksheet(worksheet);

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(result.Value);
        }

        /// <summary>
        /// Test ValidateWorksheet with null worksheet should fail
        /// </summary>
        [Test]
        public void ValidateWorksheet_WithNullWorksheet_ShouldFail()
        {
            // Act
            var result = _validator.ValidateWorksheet(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet cannot be null");
        }

        /// <summary>
        /// Test ValidateWorksheet with worksheet having empty name should fail
        /// </summary>
        [Test]
        public void ValidateWorksheet_WithEmptyName_ShouldFail()
        {
            // Arrange
            var worksheet = CreateWorksheetWithEmptyName();

            // Act
            var result = _validator.ValidateWorksheet(worksheet);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test ValidateWorksheet with worksheet having invalid name should fail
        /// </summary>
        [Test]
        public void ValidateWorksheet_WithInvalidName_ShouldFail()
        {
            // Arrange
            var worksheet = CreateWorksheetWithInvalidName();

            // Act
            var result = _validator.ValidateWorksheet(worksheet);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name contains invalid characters");
        }

        #endregion

        #region ValidateRange Tests

        /// <summary>
        /// Test ValidateRange with valid range should succeed
        /// </summary>
        [Test]
        public void ValidateRange_WithValidRange_ShouldSucceed()
        {
            // Arrange
            var range = CreateValidRange();

            // Act
            var result = _validator.ValidateRange(range);

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(result.Value);
        }

        /// <summary>
        /// Test ValidateRange with null range should fail
        /// </summary>
        [Test]
        public void ValidateRange_WithNullRange_ShouldFail()
        {
            // Act
            var result = _validator.ValidateRange(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range cannot be null");
        }

        /// <summary>
        /// Test ValidateRange with range having empty address should fail
        /// </summary>
        [Test]
        public void ValidateRange_WithEmptyAddress_ShouldFail()
        {
            // Arrange
            var range = CreateRangeWithEmptyAddress();

            // Act
            var result = _validator.ValidateRange(range);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range address cannot be null or empty");
        }

        /// <summary>
        /// Test ValidateRange with range having invalid address should fail
        /// </summary>
        [Test]
        public void ValidateRange_WithInvalidAddress_ShouldFail()
        {
            // Arrange
            var range = CreateRangeWithInvalidAddress();

            // Act
            var result = _validator.ValidateRange(range);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range address format is invalid");
        }

        #endregion

        #region ValidateCellReference Tests

        /// <summary>
        /// Test ValidateCellReference with valid cell references should succeed
        /// </summary>
        [TestCase("A1")]
        [TestCase("B2")]
        [TestCase("Z26")]
        [TestCase("AA1")]
        [TestCase("AB100")]
        [TestCase("XFD1048576")]
        public void ValidateCellReference_WithValidReferences_ShouldSucceed(string cellReference)
        {
            // Act
            var result = _validator.ValidateCellReference(cellReference);

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(result.Value);
        }

        /// <summary>
        /// Test ValidateCellReference with null reference should fail
        /// </summary>
        [Test]
        public void ValidateCellReference_WithNullReference_ShouldFail()
        {
            // Act
            var result = _validator.ValidateCellReference(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Cell reference cannot be null or empty");
        }

        /// <summary>
        /// Test ValidateCellReference with empty reference should fail
        /// </summary>
        [Test]
        public void ValidateCellReference_WithEmptyReference_ShouldFail()
        {
            // Act
            var result = _validator.ValidateCellReference("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Cell reference cannot be null or empty");
        }

        /// <summary>
        /// Test ValidateCellReference with invalid references should fail
        /// </summary>
        [TestCase("1A")]  // Number before letter
        [TestCase("A")]   // No number
        [TestCase("1")]   // No letter
        [TestCase("A0")]  // Zero row
        [TestCase("A-1")] // Negative row
        [TestCase("A1.5")] // Decimal row
        [TestCase("A1A")] // Extra characters
        [TestCase("AA1B")] // Extra characters
        [TestCase("")]    // Empty
        [TestCase(" ")]   // Whitespace
        [TestCase("A1 ")] // Trailing whitespace
        [TestCase(" A1")] // Leading whitespace
        public void ValidateCellReference_WithInvalidReferences_ShouldFail(string cellReference)
        {
            // Act
            var result = _validator.ValidateCellReference(cellReference);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Cell reference format is invalid");
        }

        /// <summary>
        /// Test ValidateCellReference with out of bounds references should fail
        /// </summary>
        [TestCase("XFE1")]     // Column beyond XFD
        [TestCase("A1048577")] // Row beyond 1048576
        [TestCase("ZZZ1")]     // Invalid column
        public void ValidateCellReference_WithOutOfBoundsReferences_ShouldFail(string cellReference)
        {
            // Act
            var result = _validator.ValidateCellReference(cellReference);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Cell reference is out of bounds");
        }

        #endregion

        #region Edge Cases and Boundary Tests

        /// <summary>
        /// Test ValidateCellReference with maximum valid column should succeed
        /// </summary>
        [Test]
        public void ValidateCellReference_WithMaxValidColumn_ShouldSucceed()
        {
            // Act
            var result = _validator.ValidateCellReference("XFD1");

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(result.Value);
        }

        /// <summary>
        /// Test ValidateCellReference with maximum valid row should succeed
        /// </summary>
        [Test]
        public void ValidateCellReference_WithMaxValidRow_ShouldSucceed()
        {
            // Act
            var result = _validator.ValidateCellReference("A1048576");

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(result.Value);
        }

        /// <summary>
        /// Test ValidateCellReference with maximum valid cell should succeed
        /// </summary>
        [Test]
        public void ValidateCellReference_WithMaxValidCell_ShouldSucceed()
        {
            // Act
            var result = _validator.ValidateCellReference("XFD1048576");

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(result.Value);
        }

        /// <summary>
        /// Test ValidateWorksheet with very long name should fail
        /// </summary>
        [Test]
        public void ValidateWorksheet_WithVeryLongName_ShouldFail()
        {
            // Arrange
            var worksheet = CreateWorksheetWithVeryLongName();

            // Act
            var result = _validator.ValidateWorksheet(worksheet);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name is too long");
        }

        /// <summary>
        /// Test ValidateWorksheet with name containing only spaces should fail
        /// </summary>
        [Test]
        public void ValidateWorksheet_WithNameContainingOnlySpaces_ShouldFail()
        {
            // Arrange
            var worksheet = CreateWorksheetWithNameContainingOnlySpaces();

            // Act
            var result = _validator.ValidateWorksheet(worksheet);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a valid workbook for testing
        /// </summary>
        /// <returns>A valid workbook</returns>
        private IWorkbookDomain CreateValidWorkbook()
        {
            return new MockWorkbookDomain
            {
                Content = CreateValidWorkbookContent(),
                Metadata = CreateValidWorkbookMetadata()
            };
        }

        /// <summary>
        /// Creates a workbook with null content for testing
        /// </summary>
        /// <returns>A workbook with null content</returns>
        private IWorkbookDomain CreateWorkbookWithNullContent()
        {
            return new MockWorkbookDomain
            {
                Content = null,
                Metadata = CreateValidWorkbookMetadata()
            };
        }

        /// <summary>
        /// Creates a workbook with null metadata for testing
        /// </summary>
        /// <returns>A workbook with null metadata</returns>
        private IWorkbookDomain CreateWorkbookWithNullMetadata()
        {
            return new MockWorkbookDomain
            {
                Content = CreateValidWorkbookContent(),
                Metadata = null
            };
        }

        /// <summary>
        /// Creates a valid workbook content for testing
        /// </summary>
        /// <returns>A valid workbook content</returns>
        private IWorkbookContent CreateValidWorkbookContent()
        {
            return new MockWorkbookContent();
        }

        /// <summary>
        /// Creates a valid workbook metadata for testing
        /// </summary>
        /// <returns>A valid workbook metadata</returns>
        private IWorkbookMetadata CreateValidWorkbookMetadata()
        {
            return new MockWorkbookMetadata();
        }

        /// <summary>
        /// Creates a valid worksheet for testing
        /// </summary>
        /// <returns>A valid worksheet</returns>
        private IWorksheetContent CreateValidWorksheet()
        {
            return new MockWorksheetContent { Name = "ValidWorksheet" };
        }

        /// <summary>
        /// Creates a worksheet with empty name for testing
        /// </summary>
        /// <returns>A worksheet with empty name</returns>
        private IWorksheetContent CreateWorksheetWithEmptyName()
        {
            return new MockWorksheetContent { Name = "" };
        }

        /// <summary>
        /// Creates a worksheet with invalid name for testing
        /// </summary>
        /// <returns>A worksheet with invalid name</returns>
        private IWorksheetContent CreateWorksheetWithInvalidName()
        {
            return new MockWorksheetContent { Name = "Invalid/Name" };
        }

        /// <summary>
        /// Creates a worksheet with very long name for testing
        /// </summary>
        /// <returns>A worksheet with very long name</returns>
        private IWorksheetContent CreateWorksheetWithVeryLongName()
        {
            return new MockWorksheetContent { Name = new string('A', 1000) };
        }

        /// <summary>
        /// Creates a worksheet with name containing only spaces for testing
        /// </summary>
        /// <returns>A worksheet with name containing only spaces</returns>
        private IWorksheetContent CreateWorksheetWithNameContainingOnlySpaces()
        {
            return new MockWorksheetContent { Name = "   " };
        }

        /// <summary>
        /// Creates a valid range for testing
        /// </summary>
        /// <returns>A valid range</returns>
        private IRange CreateValidRange()
        {
            return new MockRange { Address = "A1:B2" };
        }

        /// <summary>
        /// Creates a range with empty address for testing
        /// </summary>
        /// <returns>A range with empty address</returns>
        private IRange CreateRangeWithEmptyAddress()
        {
            return new MockRange { Address = "" };
        }

        /// <summary>
        /// Creates a range with invalid address for testing
        /// </summary>
        /// <returns>A range with invalid address</returns>
        private IRange CreateRangeWithInvalidAddress()
        {
            return new MockRange { Address = "Invalid:Address" };
        }

        #endregion
    }

    #region Mock Implementations

    /// <summary>
    /// Mock implementation of IWorkbookDomain for testing
    /// </summary>
    public class MockWorkbookDomain : IWorkbookDomain
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastModified { get; set; } = DateTime.Now;
        public IWorkbookContent Content { get; set; }
        public IWorkbookMetadata Metadata { get; set; }

        public System.Collections.Generic.IEnumerable<IWorksheet> Worksheets => Content?.Worksheets ?? new List<IWorksheet>();
        public string Author => Metadata?.Author;
        public string Title => Metadata?.Title;
        public string Subject => Metadata?.Subject;
        public string Company => Metadata?.Company;
        public DateTime Created => Metadata?.Created ?? DateTime.MinValue;
        public DateTime Modified => Metadata?.Modified ?? DateTime.MinValue;

        public void Close()
        {
            // Mock implementation
        }

        public void Dispose()
        {
            // Mock implementation
        }

        public Result<string> GetProperty(string name)
        {
            return Metadata?.GetProperty(name) ?? Result<string>.Failure("Metadata is null");
        }

        public Result<bool> SaveAs(System.IO.Stream stream)
        {
            return Content?.SaveAs(stream) ?? Result<bool>.Failure("Content is null");
        }

        public Result<bool> SaveAs(string filePath)
        {
            return Content?.SaveAs(filePath) ?? Result<bool>.Failure("Content is null");
        }

        public Result<bool> SetProperty(string name, string value)
        {
            return Metadata?.SetProperty(name, value) ?? Result<bool>.Failure("Metadata is null");
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbookContent for testing
    /// </summary>
    public class MockWorkbookContent : IWorkbookContent
    {
        public System.Collections.Generic.IEnumerable<IWorksheet> Worksheets { get; } = new List<IWorksheet>();

        public Result<bool> SaveAs(System.IO.Stream stream)
        {
            return Result<bool>.Success(true);
        }

        public Result<bool> SaveAs(string filePath)
        {
            return Result<bool>.Success(true);
        }
    }

    /// <summary>
    /// Mock implementation of IWorkbookMetadata for testing
    /// </summary>
    public class MockWorkbookMetadata : IWorkbookMetadata
    {
        public string Author { get; set; } = "Test Author";
        public string Title { get; set; } = "Test Title";
        public string Subject { get; set; } = "Test Subject";
        public string Company { get; set; } = "Test Company";
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;

        public Result<string> GetProperty(string name)
        {
            return Result<string>.Success(name switch
            {
                "Author" => Author,
                "Title" => Title,
                "Subject" => Subject,
                "Company" => Company,
                _ => null
            });
        }

        public Result<bool> SetProperty(string name, string value)
        {
            return Result<bool>.Success(true);
        }
    }

    /// <summary>
    /// Mock implementation of IWorksheetContent for testing
    /// </summary>
    public class MockWorksheetContent : IWorksheetContent
    {
        public string Name { get; set; } = "TestWorksheet";
        public System.Collections.Generic.IEnumerable<ICell> Cells { get; } = new List<ICell>();
        public System.Collections.Generic.IEnumerable<IRow> Rows { get; } = new List<IRow>();
        public System.Collections.Generic.IEnumerable<IColumn> Columns { get; } = new List<IColumn>();
        public IRange UsedRange { get; } = new MockRange();

        public void Clear()
        {
            // Mock implementation
        }

        public object GetCellValue(int row, int column)
        {
            return "TestValue";
        }

        public IRange GetRange(string address)
        {
            return new MockRange { Address = address };
        }

        public void SetCellValue(int row, int column, object value)
        {
            // Mock implementation
        }
    }

    #endregion
}
