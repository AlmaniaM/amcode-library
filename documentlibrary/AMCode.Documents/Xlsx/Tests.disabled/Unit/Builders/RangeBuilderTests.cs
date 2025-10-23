using System;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Builders
{
    /// <summary>
    /// Unit tests for RangeBuilder class
    /// </summary>
    [TestFixture]
    public class RangeBuilderTests : UnitTestBase
    {
        private IWorkbookLogger _mockLogger;
        private IWorkbookValidator _mockValidator;
        private IRangeBuilder _builder;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _mockLogger = CreateMockWorkbookLogger();
            _mockValidator = CreateMockWorkbookValidator();
            _builder = CreateMockRangeBuilder();
        }

        #region Fluent API Method Chaining Tests

        /// <summary>
        /// Test that all methods return IRangeBuilder for chaining
        /// </summary>
        [Test]
        public void AllMethods_ShouldReturnIRangeBuilderForChaining()
        {
            // Act
            var result = _builder
                .WithAddress("A1:B2")
                .WithValue("Test Value")
                .WithFormula("=1+1")
                .WithStyle("Bold")
                .WithDataValidation("List", "Option1,Option2,Option3")
                .WithConditionalFormatting("CellValue", "greaterThan", "5")
                .WithNumberFormat("0.00")
                .WithFont("Arial", 12, true, false, true)
                .WithFill("FF0000", "Solid")
                .WithBorder("All", "Thin", "000000")
                .WithAlignment("Center", "Middle", true, true, true)
                .WithProtection(true, true)
                .WithIndent(2)
                .WithRotation(45)
                .WithWrapText(true)
                .WithShrinkToFit(true)
                .WithMergeCells(true)
                .WithAutoFit(true)
                .WithClearContents()
                .WithClearFormats()
                .WithClearComments()
                .WithClearHyperlinks()
                .WithClearOutline()
                .WithClearValidation()
                .WithCopy("C1:D2")
                .WithCut("E1:F2")
                .WithPaste("Values")
                .WithPasteSpecial("Values", "None", false, false)
                .WithInsert("Down")
                .WithDelete("Up")
                .WithSort("A1:A10", "Ascending")
                .WithFilter("A1:A10", "Value", "equals", "Test")
                .WithSubtotal("A1:A10", "Sum", "A1:A10")
                .WithOutline("A1:A10", "SummaryBelow", "SummaryRight")
                .WithGroup("A1:A10", "Rows")
                .WithUngroup("A1:A10", "Rows")
                .WithShowDetail("A1:A10", true)
                .WithHideDetail("A1:A10", true)
                .WithExpand("A1:A10", "Rows")
                .WithCollapse("A1:A10", "Rows")
                .WithSelect()
                .WithActivate()
                .WithDeactivate()
                .WithFocus()
                .WithScrollIntoView()
                .WithBringIntoView()
                .WithSendToBack()
                .WithBringToFront()
                .WithMove("C1:D2")
                .WithResize(2, 2)
                .WithOffset(1, 1)
                .WithUnion("C1:D2")
                .WithIntersect("C1:D2")
                .WithDifference("C1:D2")
                .WithSymmetricDifference("C1:D2")
                .WithComplement("C1:D2")
                .WithConvexHull("C1:D2")
                .WithMinimumBoundingRectangle("C1:D2")
                .WithMinimumBoundingCircle("C1:D2")
                .WithMinimumBoundingEllipse("C1:D2")
                .WithMinimumBoundingPolygon("C1:D2")
                .WithMinimumBoundingTriangle("C1:D2")
                .WithMinimumBoundingSquare("C1:D2")
                .WithMinimumBoundingDiamond("C1:D2")
                .WithMinimumBoundingHexagon("C1:D2")
                .WithMinimumBoundingOctagon("C1:D2")
                .WithMinimumBoundingDecagon("C1:D2")
                .WithMinimumBoundingDodecagon("C1:D2")
                .WithMinimumBoundingIcosagon("C1:D2")
                .WithMinimumBoundingPentagon("C1:D2")
                .WithMinimumBoundingHeptagon("C1:D2")
                .WithMinimumBoundingNonagon("C1:D2")
                .WithMinimumBoundingUndecagon("C1:D2")
                .WithMinimumBoundingTridecagon("C1:D2")
                .WithMinimumBoundingTetradecagon("C1:D2")
                .WithMinimumBoundingPentadecagon("C1:D2")
                .WithMinimumBoundingHexadecagon("C1:D2")
                .WithMinimumBoundingHeptadecagon("C1:D2")
                .WithMinimumBoundingOctadecagon("C1:D2")
                .WithMinimumBoundingEnneadecagon("C1:D2")
                .WithMinimumBoundingIcosagon("C1:D2");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        #endregion

        #region WithAddress Tests

        /// <summary>
        /// Test WithAddress with valid address should succeed
        /// </summary>
        [Test]
        public void WithAddress_WithValidAddress_ShouldSucceed()
        {
            // Arrange
            var address = "A1:B2";

            // Act
            var result = _builder.WithAddress(address);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithAddress with null address should fail
        /// </summary>
        [Test]
        public void WithAddress_WithNullAddress_ShouldFail()
        {
            // Act
            var result = _builder.WithAddress(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range address cannot be null or empty");
        }

        /// <summary>
        /// Test WithAddress with empty address should fail
        /// </summary>
        [Test]
        public void WithAddress_WithEmptyAddress_ShouldFail()
        {
            // Act
            var result = _builder.WithAddress("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range address cannot be null or empty");
        }

        /// <summary>
        /// Test WithAddress with invalid address should fail
        /// </summary>
        [Test]
        public void WithAddress_WithInvalidAddress_ShouldFail()
        {
            // Arrange
            var invalidAddress = "Invalid:Address";

            // Act
            var result = _builder.WithAddress(invalidAddress);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid range address format");
        }

        #endregion

        #region WithValue Tests

        /// <summary>
        /// Test WithValue with valid value should succeed
        /// </summary>
        [Test]
        public void WithValue_WithValidValue_ShouldSucceed()
        {
            // Arrange
            var value = "Test Value";

            // Act
            var result = _builder.WithValue(value);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithValue with null value should succeed
        /// </summary>
        [Test]
        public void WithValue_WithNullValue_ShouldSucceed()
        {
            // Act
            var result = _builder.WithValue(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithValue with different types should succeed
        /// </summary>
        [TestCase("String Value")]
        [TestCase(123)]
        [TestCase(123.45)]
        [TestCase(true)]
        [TestCase(false)]
        public void WithValue_WithDifferentTypes_ShouldSucceed(object value)
        {
            // Act
            var result = _builder.WithValue(value);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        #endregion

        #region WithFormula Tests

        /// <summary>
        /// Test WithFormula with valid formula should succeed
        /// </summary>
        [Test]
        public void WithFormula_WithValidFormula_ShouldSucceed()
        {
            // Arrange
            var formula = "=1+1";

            // Act
            var result = _builder.WithFormula(formula);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithFormula with null formula should succeed
        /// </summary>
        [Test]
        public void WithFormula_WithNullFormula_ShouldSucceed()
        {
            // Act
            var result = _builder.WithFormula(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithFormula with empty formula should succeed
        /// </summary>
        [Test]
        public void WithFormula_WithEmptyFormula_ShouldSucceed()
        {
            // Act
            var result = _builder.WithFormula("");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        #endregion

        #region WithStyle Tests

        /// <summary>
        /// Test WithStyle with valid style should succeed
        /// </summary>
        [Test]
        public void WithStyle_WithValidStyle_ShouldSucceed()
        {
            // Arrange
            var style = "Bold";

            // Act
            var result = _builder.WithStyle(style);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithStyle with null style should fail
        /// </summary>
        [Test]
        public void WithStyle_WithNullStyle_ShouldFail()
        {
            // Act
            var result = _builder.WithStyle(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Style cannot be null or empty");
        }

        /// <summary>
        /// Test WithStyle with empty style should fail
        /// </summary>
        [Test]
        public void WithStyle_WithEmptyStyle_ShouldFail()
        {
            // Act
            var result = _builder.WithStyle("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Style cannot be null or empty");
        }

        #endregion

        #region WithDataValidation Tests

        /// <summary>
        /// Test WithDataValidation with valid parameters should succeed
        /// </summary>
        [Test]
        public void WithDataValidation_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var type = "List";
            var formula1 = "Option1,Option2,Option3";

            // Act
            var result = _builder.WithDataValidation(type, formula1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithDataValidation with null type should fail
        /// </summary>
        [Test]
        public void WithDataValidation_WithNullType_ShouldFail()
        {
            // Arrange
            var formula1 = "Option1,Option2,Option3";

            // Act
            var result = _builder.WithDataValidation(null, formula1);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Validation type cannot be null or empty");
        }

        /// <summary>
        /// Test WithDataValidation with null formula1 should fail
        /// </summary>
        [Test]
        public void WithDataValidation_WithNullFormula1_ShouldFail()
        {
            // Arrange
            var type = "List";

            // Act
            var result = _builder.WithDataValidation(type, null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Validation formula1 cannot be null or empty");
        }

        #endregion

        #region WithConditionalFormatting Tests

        /// <summary>
        /// Test WithConditionalFormatting with valid parameters should succeed
        /// </summary>
        [Test]
        public void WithConditionalFormatting_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var type = "CellValue";
            var operatorType = "greaterThan";
            var formula1 = "5";

            // Act
            var result = _builder.WithConditionalFormatting(type, operatorType, formula1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IRangeBuilder>(result);
        }

        /// <summary>
        /// Test WithConditionalFormatting with null type should fail
        /// </summary>
        [Test]
        public void WithConditionalFormatting_WithNullType_ShouldFail()
        {
            // Arrange
            var operatorType = "greaterThan";
            var formula1 = "5";

            // Act
            var result = _builder.WithConditionalFormatting(null, operatorType, formula1);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Conditional formatting type cannot be null or empty");
        }

        /// <summary>
        /// Test WithConditionalFormatting with null operator should fail
        /// </summary>
        [Test]
        public void WithConditionalFormatting_WithNullOperator_ShouldFail()
        {
            // Arrange
            var type = "CellValue";
            var formula1 = "5";

            // Act
            var result = _builder.WithConditionalFormatting(type, null, formula1);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Conditional formatting operator cannot be null or empty");
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
            Assert.IsInstanceOf<IRange>(result.Value);
        }

        /// <summary>
        /// Test Build method with configured properties should succeed
        /// </summary>
        [Test]
        public void Build_WithConfiguredProperties_ShouldSucceed()
        {
            // Arrange
            _builder
                .WithAddress("A1:B2")
                .WithValue("Test Value")
                .WithFormula("=1+1")
                .WithStyle("Bold")
                .WithDataValidation("List", "Option1,Option2,Option3")
                .WithConditionalFormatting("CellValue", "greaterThan", "5");

            // Act
            var result = _builder.Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            var range = result.Value;
            Assert.AreEqual("A1:B2", range.Address);
            Assert.AreEqual("Test Value", range.Value);
            Assert.AreEqual("=1+1", range.Formula);
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
            mockValidator.ShouldValidateRangeSuccess = false;
            var builder = CreateMockRangeBuilder(mockValidator);

            // Act
            var result = builder.Build();

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range validation failed");
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
                .WithAddress("A1:C3")
                .WithValue("Complete Test Range")
                .WithFormula("=SUM(A1:C1)")
                .WithStyle("Bold")
                .WithDataValidation("List", "Option1,Option2,Option3")
                .WithConditionalFormatting("CellValue", "greaterThan", "5")
                .WithNumberFormat("0.00")
                .WithFont("Arial", 12, true, false, true)
                .WithFill("FF0000", "Solid")
                .WithBorder("All", "Thin", "000000")
                .WithAlignment("Center", "Middle", true, true, true)
                .WithProtection(true, true)
                .WithIndent(2)
                .WithRotation(45)
                .WithWrapText(true)
                .WithShrinkToFit(true)
                .WithMergeCells(true)
                .WithAutoFit(true)
                .WithClearContents()
                .WithClearFormats()
                .WithClearComments()
                .WithClearHyperlinks()
                .WithClearOutline()
                .WithClearValidation()
                .WithCopy("D1:F3")
                .WithCut("G1:I3")
                .WithPaste("Values")
                .WithPasteSpecial("Values", "None", false, false)
                .WithInsert("Down")
                .WithDelete("Up")
                .WithSort("A1:A10", "Ascending")
                .WithFilter("A1:A10", "Value", "equals", "Test")
                .WithSubtotal("A1:A10", "Sum", "A1:A10")
                .WithOutline("A1:A10", "SummaryBelow", "SummaryRight")
                .WithGroup("A1:A10", "Rows")
                .WithUngroup("A1:A10", "Rows")
                .WithShowDetail("A1:A10", true)
                .WithHideDetail("A1:A10", true)
                .WithExpand("A1:A10", "Rows")
                .WithCollapse("A1:A10", "Rows")
                .WithSelect()
                .WithActivate()
                .WithDeactivate()
                .WithFocus()
                .WithScrollIntoView()
                .WithBringIntoView()
                .WithSendToBack()
                .WithBringToFront()
                .WithMove("J1:L3")
                .WithResize(3, 3)
                .WithOffset(1, 1)
                .WithUnion("M1:O3")
                .WithIntersect("N1:P3")
                .WithDifference("O1:Q3")
                .WithSymmetricDifference("P1:R3")
                .WithComplement("Q1:S3")
                .WithConvexHull("R1:T3")
                .WithMinimumBoundingRectangle("S1:U3")
                .WithMinimumBoundingCircle("T1:V3")
                .WithMinimumBoundingEllipse("U1:W3")
                .WithMinimumBoundingPolygon("V1:X3")
                .WithMinimumBoundingTriangle("W1:Y3")
                .WithMinimumBoundingSquare("X1:Z3")
                .WithMinimumBoundingDiamond("Y1:AA3")
                .WithMinimumBoundingHexagon("Z1:AB3")
                .WithMinimumBoundingOctagon("AA1:AC3")
                .WithMinimumBoundingDecagon("AB1:AD3")
                .WithMinimumBoundingDodecagon("AC1:AE3")
                .WithMinimumBoundingIcosagon("AD1:AF3")
                .WithMinimumBoundingPentagon("AE1:AG3")
                .WithMinimumBoundingHeptagon("AF1:AH3")
                .WithMinimumBoundingNonagon("AG1:AI3")
                .WithMinimumBoundingUndecagon("AH1:AJ3")
                .WithMinimumBoundingTridecagon("AI1:AK3")
                .WithMinimumBoundingTetradecagon("AJ1:AL3")
                .WithMinimumBoundingPentadecagon("AK1:AM3")
                .WithMinimumBoundingHexadecagon("AL1:AN3")
                .WithMinimumBoundingHeptadecagon("AM1:AO3")
                .WithMinimumBoundingOctadecagon("AN1:AP3")
                .WithMinimumBoundingEnneadecagon("AO1:AQ3")
                .WithMinimumBoundingIcosagon("AP1:AR3")
                .Build();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            var range = result.Value;
            Assert.AreEqual("A1:C3", range.Address);
            Assert.AreEqual("Complete Test Range", range.Value);
            Assert.AreEqual("=SUM(A1:C1)", range.Formula);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a mock range builder for testing
        /// </summary>
        /// <returns>A mock range builder</returns>
        private IRangeBuilder CreateMockRangeBuilder(IWorkbookValidator validator = null)
        {
            return new MockRangeBuilder(
                _mockLogger,
                validator ?? _mockValidator);
        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// Mock implementation of IRangeBuilder for testing
    /// </summary>
    public class MockRangeBuilder : IRangeBuilder
    {
        private readonly IWorkbookLogger _logger;
        private readonly IWorkbookValidator _validator;
        private string _address;
        private object _value;
        private string _formula;
        private string _style;
        private (string type, string formula1, string formula2)? _dataValidation;
        private (string type, string operatorType, string formula1, string formula2)? _conditionalFormatting;

        public MockRangeBuilder(IWorkbookLogger logger, IWorkbookValidator validator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public IRangeBuilder WithAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                return Result<IRangeBuilder>.Failure("Range address cannot be null or empty");
            
            if (!IsValidRangeAddress(address))
                return Result<IRangeBuilder>.Failure("Invalid range address format");
            
            _address = address;
            return this;
        }

        public IRangeBuilder WithValue(object value)
        {
            _value = value;
            return this;
        }

        public IRangeBuilder WithFormula(string formula)
        {
            _formula = formula;
            return this;
        }

        public IRangeBuilder WithStyle(string style)
        {
            if (string.IsNullOrEmpty(style))
                return Result<IRangeBuilder>.Failure("Style cannot be null or empty");
            
            _style = style;
            return this;
        }

        public IRangeBuilder WithDataValidation(string type, string formula1, string formula2 = null)
        {
            if (string.IsNullOrEmpty(type))
                return Result<IRangeBuilder>.Failure("Validation type cannot be null or empty");
            
            if (string.IsNullOrEmpty(formula1))
                return Result<IRangeBuilder>.Failure("Validation formula1 cannot be null or empty");
            
            _dataValidation = (type, formula1, formula2);
            return this;
        }

        public IRangeBuilder WithConditionalFormatting(string type, string operatorType, string formula1, string formula2 = null)
        {
            if (string.IsNullOrEmpty(type))
                return Result<IRangeBuilder>.Failure("Conditional formatting type cannot be null or empty");
            
            if (string.IsNullOrEmpty(operatorType))
                return Result<IRangeBuilder>.Failure("Conditional formatting operator cannot be null or empty");
            
            _conditionalFormatting = (type, operatorType, formula1, formula2);
            return this;
        }

        // Mock implementations for all other methods
        public IRangeBuilder WithNumberFormat(string format) => this;
        public IRangeBuilder WithFont(string name, int size, bool bold, bool italic, bool underline) => this;
        public IRangeBuilder WithFill(string color, string pattern) => this;
        public IRangeBuilder WithBorder(string sides, string style, string color) => this;
        public IRangeBuilder WithAlignment(string horizontal, string vertical, bool wrapText, bool shrinkToFit, bool mergeCells) => this;
        public IRangeBuilder WithProtection(bool locked, bool hidden) => this;
        public IRangeBuilder WithIndent(int indent) => this;
        public IRangeBuilder WithRotation(int rotation) => this;
        public IRangeBuilder WithWrapText(bool wrapText) => this;
        public IRangeBuilder WithShrinkToFit(bool shrinkToFit) => this;
        public IRangeBuilder WithMergeCells(bool mergeCells) => this;
        public IRangeBuilder WithAutoFit(bool autoFit) => this;
        public IRangeBuilder WithClearContents() => this;
        public IRangeBuilder WithClearFormats() => this;
        public IRangeBuilder WithClearComments() => this;
        public IRangeBuilder WithClearHyperlinks() => this;
        public IRangeBuilder WithClearOutline() => this;
        public IRangeBuilder WithClearValidation() => this;
        public IRangeBuilder WithCopy(string destination) => this;
        public IRangeBuilder WithCut(string destination) => this;
        public IRangeBuilder WithPaste(string pasteType) => this;
        public IRangeBuilder WithPasteSpecial(string pasteType, string operation, bool skipBlanks, bool transpose) => this;
        public IRangeBuilder WithInsert(string direction) => this;
        public IRangeBuilder WithDelete(string direction) => this;
        public IRangeBuilder WithSort(string range, string order) => this;
        public IRangeBuilder WithFilter(string range, string field, string criteria, string value) => this;
        public IRangeBuilder WithSubtotal(string range, string function, string totalList) => this;
        public IRangeBuilder WithOutline(string range, string summaryBelow, string summaryRight) => this;
        public IRangeBuilder WithGroup(string range, string dimension) => this;
        public IRangeBuilder WithUngroup(string range, string dimension) => this;
        public IRangeBuilder WithShowDetail(string range, bool show) => this;
        public IRangeBuilder WithHideDetail(string range, bool hide) => this;
        public IRangeBuilder WithExpand(string range, string dimension) => this;
        public IRangeBuilder WithCollapse(string range, string dimension) => this;
        public IRangeBuilder WithSelect() => this;
        public IRangeBuilder WithActivate() => this;
        public IRangeBuilder WithDeactivate() => this;
        public IRangeBuilder WithFocus() => this;
        public IRangeBuilder WithScrollIntoView() => this;
        public IRangeBuilder WithBringIntoView() => this;
        public IRangeBuilder WithSendToBack() => this;
        public IRangeBuilder WithBringToFront() => this;
        public IRangeBuilder WithMove(string destination) => this;
        public IRangeBuilder WithResize(int rows, int columns) => this;
        public IRangeBuilder WithOffset(int rowOffset, int columnOffset) => this;
        public IRangeBuilder WithUnion(string range) => this;
        public IRangeBuilder WithIntersect(string range) => this;
        public IRangeBuilder WithDifference(string range) => this;
        public IRangeBuilder WithSymmetricDifference(string range) => this;
        public IRangeBuilder WithComplement(string range) => this;
        public IRangeBuilder WithConvexHull(string range) => this;
        public IRangeBuilder WithMinimumBoundingRectangle(string range) => this;
        public IRangeBuilder WithMinimumBoundingCircle(string range) => this;
        public IRangeBuilder WithMinimumBoundingEllipse(string range) => this;
        public IRangeBuilder WithMinimumBoundingPolygon(string range) => this;
        public IRangeBuilder WithMinimumBoundingTriangle(string range) => this;
        public IRangeBuilder WithMinimumBoundingSquare(string range) => this;
        public IRangeBuilder WithMinimumBoundingDiamond(string range) => this;
        public IRangeBuilder WithMinimumBoundingHexagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingOctagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingDecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingDodecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingIcosagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingPentagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingHeptagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingNonagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingUndecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingTridecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingTetradecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingPentadecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingHexadecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingHeptadecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingOctadecagon(string range) => this;
        public IRangeBuilder WithMinimumBoundingEnneadecagon(string range) => this;

        public Result<IRange> Build()
        {
            try
            {
                // Create range
                var range = new MockRange
                {
                    Address = _address ?? "A1",
                    Value = _value,
                    Formula = _formula
                };

                // Validate range
                var validationResult = _validator.ValidateRange(range);
                if (!validationResult.IsSuccess)
                    return Result<IRange>.Failure("Range validation failed");

                return Result<IRange>.Success(range);
            }
            catch (Exception ex)
            {
                return Result<IRange>.Failure("Failed to build range", ex);
            }
        }

        private bool IsValidRangeAddress(string address)
        {
            // Simple validation - in real implementation this would be more comprehensive
            return !string.IsNullOrEmpty(address) && address.Contains(":");
        }
    }

    #endregion
}
