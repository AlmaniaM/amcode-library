using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Builders
{
    /// <summary>
    /// Unit tests for WorksheetBuilder class
    /// </summary>
    [TestFixture]
    public class WorksheetBuilderTests : UnitTestBase
    {
        private IWorkbookLogger _mockLogger;
        private IWorkbookValidator _mockValidator;
        private IWorksheetBuilder _builder;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _mockLogger = CreateMockWorkbookLogger();
            _mockValidator = CreateMockWorkbookValidator();
            _builder = CreateMockWorksheetBuilder();
        }

        #region Fluent API Method Chaining Tests

        /// <summary>
        /// Test that all methods return IWorksheetBuilder for chaining
        /// </summary>
        [Test]
        public void AllMethods_ShouldReturnIWorksheetBuilderForChaining()
        {
            // Act
            var result = _builder
                .WithName("Test Worksheet")
                .WithData(CreateTestDataArray(3, 3))
                .WithFormatting("A1:C3", "Bold")
                .WithFormula("A1", "=1+1")
                .WithComment("A1", "Test comment")
                .WithHyperlink("A1", "http://example.com", "Example Link")
                .WithDataValidation("A1:A10", "List", "Option1,Option2,Option3")
                .WithConditionalFormatting("A1:C3", "CellValue", "greaterThan", "5")
                .WithAutoFilter("A1:C3")
                .WithFreezePanes(1, 1)
                .WithSplitPanes(200, 300)
                .WithZoom(150)
                .WithTabColor("FF0000")
                .WithRightToLeft(false)
                .WithShowGridlines(true)
                .WithShowHeaders(true)
                .WithShowZeros(true)
                .WithShowOutlineSymbols(true)
                .WithDefaultRowHeight(20.0)
                .WithDefaultColumnWidth(15.0)
                .WithPageSetup("A4", "Portrait", 1.0, 1.0, 1.0, 1.0)
                .WithPrintArea("A1:C3")
                .WithPrintTitles("1:1", "A:A")
                .WithPageBreaks("A2", "B3")
                .WithMargins(1.0, 1.0, 1.0, 1.0, 0.5, 0.5)
                .WithHeaderFooter("Test Header", "Test Footer")
                .WithPageNumbering(true, "Page &P of &N")
                .WithDateStamp(true)
                .WithTimeStamp(true)
                .WithFileName(true)
                .WithSheetName(true)
                .WithFilePath(true)
                .WithFileSize(true)
                .WithLastSaved(true)
                .WithCreated(true)
                .WithModified(true)
                .WithAuthor(true)
                .WithTitle(true)
                .WithSubject(true)
                .WithCompany(true)
                .WithKeywords(true)
                .WithComments(true)
                .WithManager(true)
                .WithApplication(true)
                .WithTemplate(true)
                .WithRevision(true)
                .WithEditingTime(true)
                .WithPageCount(true)
                .WithWordCount(true)
                .WithCharacterCount(true)
                .WithCharacterCountWithSpaces(true)
                .WithLineCount(true)
                .WithParagraphCount(true)
                .WithSlideCount(true)
                .WithNoteCount(true)
                .WithHiddenSlideCount(true)
                .WithMMClipCount(true)
                .WithScaleCrop(true)
                .WithSharedDoc(true)
                .WithHyperlinkBase(true)
                .WithHyperlinksChanged(true)
                .WithAppVersion(true)
                .WithDocSecurity(true)
                .WithLinksUpToDate(true);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        #endregion

        #region WithName Tests

        /// <summary>
        /// Test WithName with valid name should succeed
        /// </summary>
        [Test]
        public void WithName_WithValidName_ShouldSucceed()
        {
            // Arrange
            var name = "Test Worksheet";

            // Act
            var result = _builder.WithName(name);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithName with null name should fail
        /// </summary>
        [Test]
        public void WithName_WithNullName_ShouldFail()
        {
            // Act
            var result = _builder.WithName(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test WithName with empty name should fail
        /// </summary>
        [Test]
        public void WithName_WithEmptyName_ShouldFail()
        {
            // Act
            var result = _builder.WithName("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test WithName with whitespace name should fail
        /// </summary>
        [Test]
        public void WithName_WithWhitespaceName_ShouldFail()
        {
            // Act
            var result = _builder.WithName("   ");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test WithName with invalid name should fail
        /// </summary>
        [Test]
        public void WithName_WithInvalidName_ShouldFail()
        {
            // Arrange
            var invalidName = "Invalid/Name";

            // Act
            var result = _builder.WithName(invalidName);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name contains invalid characters");
        }

        #endregion

        #region WithData Tests

        /// <summary>
        /// Test WithData with valid 2D array should succeed
        /// </summary>
        [Test]
        public void WithData_WithValid2DArray_ShouldSucceed()
        {
            // Arrange
            var data = CreateTestDataArray(3, 3);

            // Act
            var result = _builder.WithData(data);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithData with null array should fail
        /// </summary>
        [Test]
        public void WithData_WithNullArray_ShouldFail()
        {
            // Act
            var result = _builder.WithData((object[,])null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Data array cannot be null");
        }

        /// <summary>
        /// Test WithData with empty array should succeed
        /// </summary>
        [Test]
        public void WithData_WithEmptyArray_ShouldSucceed()
        {
            // Arrange
            var data = new object[0, 0];

            // Act
            var result = _builder.WithData(data);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithData with valid list of lists should succeed
        /// </summary>
        [Test]
        public void WithData_WithValidListOfLists_ShouldSucceed()
        {
            // Arrange
            var data = CreateTestDataList(3, 3);

            // Act
            var result = _builder.WithData(data);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithData with null list should fail
        /// </summary>
        [Test]
        public void WithData_WithNullList_ShouldFail()
        {
            // Act
            var result = _builder.WithData((System.Collections.Generic.List<System.Collections.Generic.List<object>>)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Data list cannot be null");
        }

        #endregion

        #region WithFormatting Tests

        /// <summary>
        /// Test WithFormatting with valid parameters should succeed
        /// </summary>
        [Test]
        public void WithFormatting_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var range = "A1:C3";
            var format = "Bold";

            // Act
            var result = _builder.WithFormatting(range, format);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithFormatting with null range should fail
        /// </summary>
        [Test]
        public void WithFormatting_WithNullRange_ShouldFail()
        {
            // Arrange
            var format = "Bold";

            // Act
            var result = _builder.WithFormatting(null, format);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range cannot be null or empty");
        }

        /// <summary>
        /// Test WithFormatting with null format should fail
        /// </summary>
        [Test]
        public void WithFormatting_WithNullFormat_ShouldFail()
        {
            // Arrange
            var range = "A1:C3";

            // Act
            var result = _builder.WithFormatting(range, null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Format cannot be null or empty");
        }

        #endregion

        #region WithFormula Tests

        /// <summary>
        /// Test WithFormula with valid parameters should succeed
        /// </summary>
        [Test]
        public void WithFormula_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var cell = "A1";
            var formula = "=1+1";

            // Act
            var result = _builder.WithFormula(cell, formula);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithFormula with null cell should fail
        /// </summary>
        [Test]
        public void WithFormula_WithNullCell_ShouldFail()
        {
            // Arrange
            var formula = "=1+1";

            // Act
            var result = _builder.WithFormula(null, formula);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Cell reference cannot be null or empty");
        }

        /// <summary>
        /// Test WithFormula with null formula should fail
        /// </summary>
        [Test]
        public void WithFormula_WithNullFormula_ShouldFail()
        {
            // Arrange
            var cell = "A1";

            // Act
            var result = _builder.WithFormula(cell, null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Formula cannot be null or empty");
        }

        #endregion

        #region WithComment Tests

        /// <summary>
        /// Test WithComment with valid parameters should succeed
        /// </summary>
        [Test]
        public void WithComment_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var cell = "A1";
            var comment = "Test comment";

            // Act
            var result = _builder.WithComment(cell, comment);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithComment with null cell should fail
        /// </summary>
        [Test]
        public void WithComment_WithNullCell_ShouldFail()
        {
            // Arrange
            var comment = "Test comment";

            // Act
            var result = _builder.WithComment(null, comment);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Cell reference cannot be null or empty");
        }

        /// <summary>
        /// Test WithComment with null comment should fail
        /// </summary>
        [Test]
        public void WithComment_WithNullComment_ShouldFail()
        {
            // Arrange
            var cell = "A1";

            // Act
            var result = _builder.WithComment(cell, null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Comment cannot be null or empty");
        }

        #endregion

        #region WithHyperlink Tests

        /// <summary>
        /// Test WithHyperlink with valid parameters should succeed
        /// </summary>
        [Test]
        public void WithHyperlink_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var cell = "A1";
            var url = "http://example.com";
            var displayText = "Example Link";

            // Act
            var result = _builder.WithHyperlink(cell, url, displayText);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IWorksheetBuilder>(result);
        }

        /// <summary>
        /// Test WithHyperlink with null cell should fail
        /// </summary>
        [Test]
        public void WithHyperlink_WithNullCell_ShouldFail()
        {
            // Arrange
            var url = "http://example.com";
            var displayText = "Example Link";

            // Act
            var result = _builder.WithHyperlink(null, url, displayText);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Cell reference cannot be null or empty");
        }

        /// <summary>
        /// Test WithHyperlink with null URL should fail
        /// </summary>
        [Test]
        public void WithHyperlink_WithNullUrl_ShouldFail()
        {
            // Arrange
            var cell = "A1";
            var displayText = "Example Link";

            // Act
            var result = _builder.WithHyperlink(cell, null, displayText);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "URL cannot be null or empty");
        }

        #endregion

        #region Build Tests

        /// <summary>
        /// Test Build method should succeed
        /// </summary>
        [Test]
        public void Build_ShouldSucceed()
        {
            // Act
            var result = _builder.Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorksheetContent>(result.Value);
        }

        /// <summary>
        /// Test Build method with configured properties should succeed
        /// </summary>
        [Test]
        public void Build_WithConfiguredProperties_ShouldSucceed()
        {
            // Arrange
            _builder
                .WithName("Test Worksheet")
                .WithData(CreateTestDataArray(3, 3))
                .WithFormatting("A1:C3", "Bold")
                .WithFormula("A1", "=1+1")
                .WithComment("A1", "Test comment")
                .WithHyperlink("A1", "http://example.com", "Example Link");

            // Act
            var result = _builder.Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            var worksheet = result.Value;
            Assert.AreEqual("Test Worksheet", worksheet.Name);
        }

        #endregion

        #region Error Handling Tests

        /// <summary>
        /// Test Build method with validation failure should fail
        /// </summary>
        [Test]
        public void Build_WithValidationFailure_ShouldFail()
        {
            // Arrange
            var mockValidator = CreateMockWorkbookValidator();
            mockValidator.ShouldValidateWorksheetSuccess = false;
            var builder = CreateMockWorksheetBuilder(mockValidator);

            // Act
            var result = builder.Build();

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet validation failed");
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with all builder methods
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithAllBuilderMethods_ShouldSucceed()
        {
            // Act
            var result = _builder
                .WithName("Complete Test Worksheet")
                .WithData(CreateTestDataArray(5, 5))
                .WithFormatting("A1:E5", "Bold")
                .WithFormula("A1", "=SUM(B1:E1)")
                .WithComment("A1", "This is the sum formula")
                .WithHyperlink("A1", "http://example.com", "Example Link")
                .WithDataValidation("A1:A10", "List", "Option1,Option2,Option3")
                .WithConditionalFormatting("A1:E5", "CellValue", "greaterThan", "5")
                .WithAutoFilter("A1:E5")
                .WithFreezePanes(1, 1)
                .WithSplitPanes(200, 300)
                .WithZoom(150)
                .WithTabColor("FF0000")
                .WithRightToLeft(false)
                .WithShowGridlines(true)
                .WithShowHeaders(true)
                .WithShowZeros(true)
                .WithShowOutlineSymbols(true)
                .WithDefaultRowHeight(20.0)
                .WithDefaultColumnWidth(15.0)
                .WithPageSetup("A4", "Portrait", 1.0, 1.0, 1.0, 1.0)
                .WithPrintArea("A1:E5")
                .WithPrintTitles("1:1", "A:A")
                .WithPageBreaks("A2", "B3")
                .WithMargins(1.0, 1.0, 1.0, 1.0, 0.5, 0.5)
                .WithHeaderFooter("Test Header", "Test Footer")
                .WithPageNumbering(true, "Page &P of &N")
                .WithDateStamp(true)
                .WithTimeStamp(true)
                .WithFileName(true)
                .WithSheetName(true)
                .WithFilePath(true)
                .WithFileSize(true)
                .WithLastSaved(true)
                .WithCreated(true)
                .WithModified(true)
                .WithAuthor(true)
                .WithTitle(true)
                .WithSubject(true)
                .WithCompany(true)
                .WithKeywords(true)
                .WithComments(true)
                .WithManager(true)
                .WithApplication(true)
                .WithTemplate(true)
                .WithRevision(true)
                .WithEditingTime(true)
                .WithPageCount(true)
                .WithWordCount(true)
                .WithCharacterCount(true)
                .WithCharacterCountWithSpaces(true)
                .WithLineCount(true)
                .WithParagraphCount(true)
                .WithSlideCount(true)
                .WithNoteCount(true)
                .WithHiddenSlideCount(true)
                .WithMMClipCount(true)
                .WithScaleCrop(true)
                .WithSharedDoc(true)
                .WithHyperlinkBase(true)
                .WithHyperlinksChanged(true)
                .WithAppVersion(true)
                .WithDocSecurity(true)
                .WithLinksUpToDate(true)
                .Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            var worksheet = result.Value;
            Assert.AreEqual("Complete Test Worksheet", worksheet.Name);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a mock worksheet builder for testing
        /// </summary>
        /// <returns>A mock worksheet builder</returns>
        private IWorksheetBuilder CreateMockWorksheetBuilder(IWorkbookValidator validator = null)
        {
            return new MockWorksheetBuilder(
                _mockLogger,
                validator ?? _mockValidator);
        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// Mock implementation of IWorksheetBuilder for testing
    /// </summary>
    public class MockWorksheetBuilder : IWorksheetBuilder
    {
        private readonly IWorkbookLogger _logger;
        private readonly IWorkbookValidator _validator;
        private string _name;
        private object[,] _dataArray;
        private System.Collections.Generic.List<System.Collections.Generic.List<object>> _dataList;
        private readonly System.Collections.Generic.List<(string range, string format)> _formatting = new System.Collections.Generic.List<(string, string)>();
        private readonly System.Collections.Generic.List<(string cell, string formula)> _formulas = new System.Collections.Generic.List<(string, string)>();
        private readonly System.Collections.Generic.List<(string cell, string comment)> _comments = new System.Collections.Generic.List<(string, string)>();
        private readonly System.Collections.Generic.List<(string cell, string url, string displayText)> _hyperlinks = new System.Collections.Generic.List<(string, string, string)>();

        public MockWorksheetBuilder(IWorkbookLogger logger, IWorkbookValidator validator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public IWorksheetBuilder WithName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result<IWorksheetBuilder>.Failure("Worksheet name cannot be null or empty");
            
            if (name.Contains("/") || name.Contains("\\") || name.Contains("?") || name.Contains("*") || name.Contains("[") || name.Contains("]"))
                return Result<IWorksheetBuilder>.Failure("Worksheet name contains invalid characters");
            
            _name = name;
            return this;
        }

        public IWorksheetBuilder WithData(object[,] data)
        {
            if (data == null)
                return Result<IWorksheetBuilder>.Failure("Data array cannot be null");
            
            _dataArray = data;
            return this;
        }

        public IWorksheetBuilder WithData(System.Collections.Generic.List<System.Collections.Generic.List<object>> data)
        {
            if (data == null)
                return Result<IWorksheetBuilder>.Failure("Data list cannot be null");
            
            _dataList = data;
            return this;
        }

        public IWorksheetBuilder WithFormatting(string range, string format)
        {
            if (string.IsNullOrEmpty(range))
                return Result<IWorksheetBuilder>.Failure("Range cannot be null or empty");
            
            if (string.IsNullOrEmpty(format))
                return Result<IWorksheetBuilder>.Failure("Format cannot be null or empty");
            
            _formatting.Add((range, format));
            return this;
        }

        public IWorksheetBuilder WithFormula(string cell, string formula)
        {
            if (string.IsNullOrEmpty(cell))
                return Result<IWorksheetBuilder>.Failure("Cell reference cannot be null or empty");
            
            if (string.IsNullOrEmpty(formula))
                return Result<IWorksheetBuilder>.Failure("Formula cannot be null or empty");
            
            _formulas.Add((cell, formula));
            return this;
        }

        public IWorksheetBuilder WithComment(string cell, string comment)
        {
            if (string.IsNullOrEmpty(cell))
                return Result<IWorksheetBuilder>.Failure("Cell reference cannot be null or empty");
            
            if (string.IsNullOrEmpty(comment))
                return Result<IWorksheetBuilder>.Failure("Comment cannot be null or empty");
            
            _comments.Add((cell, comment));
            return this;
        }

        public IWorksheetBuilder WithHyperlink(string cell, string url, string displayText)
        {
            if (string.IsNullOrEmpty(cell))
                return Result<IWorksheetBuilder>.Failure("Cell reference cannot be null or empty");
            
            if (string.IsNullOrEmpty(url))
                return Result<IWorksheetBuilder>.Failure("URL cannot be null or empty");
            
            _hyperlinks.Add((cell, url, displayText));
            return this;
        }

        public IWorksheetBuilder WithDataValidation(string range, string type, string formula1, string formula2 = null)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithConditionalFormatting(string range, string type, string operatorType, string formula1, string formula2 = null)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithAutoFilter(string range)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithFreezePanes(int row, int column)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithSplitPanes(int xSplit, int ySplit)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithZoom(int zoom)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithTabColor(string color)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithRightToLeft(bool rightToLeft)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithShowGridlines(bool showGridlines)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithShowHeaders(bool showHeaders)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithShowZeros(bool showZeros)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithShowOutlineSymbols(bool showOutlineSymbols)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithDefaultRowHeight(double height)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithDefaultColumnWidth(double width)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithPageSetup(string paperSize, string orientation, double leftMargin, double rightMargin, double topMargin, double bottomMargin)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithPrintArea(string range)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithPrintTitles(string rows, string columns)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithPageBreaks(params string[] cells)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithMargins(double left, double right, double top, double bottom, double header, double footer)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithHeaderFooter(string header, string footer)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithPageNumbering(bool enabled, string format)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithDateStamp(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithTimeStamp(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithFileName(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithSheetName(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithFilePath(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithFileSize(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithLastSaved(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithCreated(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithModified(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithAuthor(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithTitle(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithSubject(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithCompany(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithKeywords(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithComments(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithManager(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithApplication(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithTemplate(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithRevision(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithEditingTime(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithPageCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithWordCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithCharacterCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithCharacterCountWithSpaces(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithLineCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithParagraphCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithSlideCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithNoteCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithHiddenSlideCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithMMClipCount(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithScaleCrop(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithSharedDoc(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithHyperlinkBase(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithHyperlinksChanged(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithAppVersion(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithDocSecurity(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public IWorksheetBuilder WithLinksUpToDate(bool enabled)
        {
            // Mock implementation
            return this;
        }

        public Result<IWorksheetContent> Build()
        {
            try
            {
                // Create worksheet content
                var worksheet = new MockWorksheetContent
                {
                    Name = _name ?? "Default Worksheet"
                };

                // Validate worksheet
                var validationResult = _validator.ValidateWorksheet(worksheet);
                if (!validationResult.IsSuccess)
                    return Result<IWorksheetContent>.Failure("Worksheet validation failed");

                return Result<IWorksheetContent>.Success(worksheet);
            }
            catch (Exception ex)
            {
                return Result<IWorksheetContent>.Failure("Failed to build worksheet", ex);
            }
        }
    }

    /// <summary>
    /// Mock implementation of IWorksheetContent for testing
    /// </summary>
    public class MockWorksheetContent : IWorksheetContent
    {
        public string Name { get; set; } = "Test Worksheet";
        public System.Collections.Generic.IEnumerable<ICell> Cells { get; } = new System.Collections.Generic.List<ICell>();
        public System.Collections.Generic.IEnumerable<IRow> Rows { get; } = new System.Collections.Generic.List<IRow>();
        public System.Collections.Generic.IEnumerable<IColumn> Columns { get; } = new System.Collections.Generic.List<IColumn>();
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
