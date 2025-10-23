using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Providers.OpenXml;
using AMCode.Documents.Xlsx.Interfaces;
using NUnit.Framework;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AMCode.Documents.Xlsx.Tests.Unit.Providers
{
    /// <summary>
    /// Unit tests for OpenXmlRange class
    /// </summary>
    [TestFixture]
    public class OpenXmlRangeTests : UnitTestBase
    {
        private SpreadsheetDocument _spreadsheetDocument;
        private WorksheetPart _worksheetPart;
        private OpenXmlRange _range;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _spreadsheetDocument = CreateValidSpreadsheetDocument();
            _worksheetPart = CreateValidWorksheetPart();
            _range = new OpenXmlRange(_worksheetPart, "A1:B2");
        }

        /// <summary>
        /// Teardown method called after each test
        /// </summary>
        [TearDown]
        public override void TearDown()
        {
            _range?.Dispose();
            _worksheetPart?.Dispose();
            _spreadsheetDocument?.Dispose();
            base.TearDown();
        }

        #region Constructor Tests

        /// <summary>
        /// Test constructor with valid parameters should succeed
        /// </summary>
        [Test]
        public void Constructor_WithValidParameters_ShouldSucceed()
        {
            // Act & Assert
            Assert.IsNotNull(_range);
        }

        /// <summary>
        /// Test constructor with null WorksheetPart should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullWorksheetPart_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OpenXmlRange(null, "A1:B2"));
        }

        /// <summary>
        /// Test constructor with null address should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullAddress_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OpenXmlRange(_worksheetPart, null));
        }

        /// <summary>
        /// Test constructor with empty address should throw
        /// </summary>
        [Test]
        public void Constructor_WithEmptyAddress_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new OpenXmlRange(_worksheetPart, ""));
        }

        /// <summary>
        /// Test constructor with invalid address should throw
        /// </summary>
        [Test]
        public void Constructor_WithInvalidAddress_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new OpenXmlRange(_worksheetPart, "Invalid:Address"));
        }

        #endregion

        #region Address Property Tests

        /// <summary>
        /// Test Address property returns correct address
        /// </summary>
        [Test]
        public void Address_ShouldReturnCorrectAddress()
        {
            // Act
            var address = _range.Address;

            // Assert
            Assert.AreEqual("A1:B2", address);
        }

        /// <summary>
        /// Test Address property can be set
        /// </summary>
        [Test]
        public void Address_CanBeSet()
        {
            // Arrange
            var newAddress = "C3:D4";

            // Act
            _range.Address = newAddress;

            // Assert
            Assert.AreEqual(newAddress, _range.Address);
        }

        /// <summary>
        /// Test Address property with null value should throw
        /// </summary>
        [Test]
        public void Address_WithNullValue_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _range.Address = null);
        }

        /// <summary>
        /// Test Address property with empty value should throw
        /// </summary>
        [Test]
        public void Address_WithEmptyValue_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _range.Address = "");
        }

        /// <summary>
        /// Test Address property with invalid value should throw
        /// </summary>
        [Test]
        public void Address_WithInvalidValue_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _range.Address = "Invalid:Address");
        }

        #endregion

        #region Value Property Tests

        /// <summary>
        /// Test Value property can be set and retrieved
        /// </summary>
        [Test]
        public void Value_CanBeSetAndRetrieved()
        {
            // Arrange
            var testValue = "Test Value";

            // Act
            _range.Value = testValue;

            // Assert
            Assert.AreEqual(testValue, _range.Value);
        }

        /// <summary>
        /// Test Value property with null value should succeed
        /// </summary>
        [Test]
        public void Value_WithNullValue_ShouldSucceed()
        {
            // Act
            _range.Value = null;

            // Assert
            Assert.IsNull(_range.Value);
        }

        /// <summary>
        /// Test Value property with different types should succeed
        /// </summary>
        [TestCase("String Value")]
        [TestCase(123)]
        [TestCase(123.45)]
        [TestCase(true)]
        [TestCase(false)]
        public void Value_WithDifferentTypes_ShouldSucceed(object value)
        {
            // Act
            _range.Value = value;

            // Assert
            Assert.AreEqual(value, _range.Value);
        }

        #endregion

        #region Formula Property Tests

        /// <summary>
        /// Test Formula property can be set and retrieved
        /// </summary>
        [Test]
        public void Formula_CanBeSetAndRetrieved()
        {
            // Arrange
            var testFormula = "=1+1";

            // Act
            _range.Formula = testFormula;

            // Assert
            Assert.AreEqual(testFormula, _range.Formula);
        }

        /// <summary>
        /// Test Formula property with null value should succeed
        /// </summary>
        [Test]
        public void Formula_WithNullValue_ShouldSucceed()
        {
            // Act
            _range.Formula = null;

            // Assert
            Assert.IsNull(_range.Formula);
        }

        /// <summary>
        /// Test Formula property with empty value should succeed
        /// </summary>
        [Test]
        public void Formula_WithEmptyValue_ShouldSucceed()
        {
            // Act
            _range.Formula = "";

            // Assert
            Assert.AreEqual("", _range.Formula);
        }

        #endregion

        #region GetCell Tests

        /// <summary>
        /// Test GetCell with valid parameters should succeed
        /// </summary>
        [Test]
        public void GetCell_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var row = 1;
            var column = 1;

            // Act
            var result = _range.GetCell(row, column);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<ICell>(result.Value);
        }

        /// <summary>
        /// Test GetCell with invalid row should fail
        /// </summary>
        [Test]
        public void GetCell_WithInvalidRow_ShouldFail()
        {
            // Arrange
            var row = 0; // Invalid row
            var column = 1;

            // Act
            var result = _range.GetCell(row, column);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Row must be greater than 0");
        }

        /// <summary>
        /// Test GetCell with invalid column should fail
        /// </summary>
        [Test]
        public void GetCell_WithInvalidColumn_ShouldFail()
        {
            // Arrange
            var row = 1;
            var column = 0; // Invalid column

            // Act
            var result = _range.GetCell(row, column);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Column must be greater than 0");
        }

        /// <summary>
        /// Test GetCell with out of bounds row should fail
        /// </summary>
        [Test]
        public void GetCell_WithOutOfBoundsRow_ShouldFail()
        {
            // Arrange
            var row = 1000; // Out of bounds
            var column = 1;

            // Act
            var result = _range.GetCell(row, column);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Row is out of range");
        }

        /// <summary>
        /// Test GetCell with out of bounds column should fail
        /// </summary>
        [Test]
        public void GetCell_WithOutOfBoundsColumn_ShouldFail()
        {
            // Arrange
            var row = 1;
            var column = 1000; // Out of bounds

            // Act
            var result = _range.GetCell(row, column);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Column is out of range");
        }

        #endregion

        #region SetValue Tests

        /// <summary>
        /// Test SetValue with valid value should succeed
        /// </summary>
        [Test]
        public void SetValue_WithValidValue_ShouldSucceed()
        {
            // Arrange
            var value = "Test Value";

            // Act
            var result = _range.SetValue(value);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetValue with null value should succeed
        /// </summary>
        [Test]
        public void SetValue_WithNullValue_ShouldSucceed()
        {
            // Act
            var result = _range.SetValue(null);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetValue with different types should succeed
        /// </summary>
        [TestCase("String Value")]
        [TestCase(123)]
        [TestCase(123.45)]
        [TestCase(true)]
        [TestCase(false)]
        public void SetValue_WithDifferentTypes_ShouldSucceed(object value)
        {
            // Act
            var result = _range.SetValue(value);

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region GetValue Tests

        /// <summary>
        /// Test GetValue should succeed
        /// </summary>
        [Test]
        public void GetValue_ShouldSucceed()
        {
            // Act
            var result = _range.GetValue();

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region SetFormula Tests

        /// <summary>
        /// Test SetFormula with valid formula should succeed
        /// </summary>
        [Test]
        public void SetFormula_WithValidFormula_ShouldSucceed()
        {
            // Arrange
            var formula = "=1+1";

            // Act
            var result = _range.SetFormula(formula);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetFormula with null formula should succeed
        /// </summary>
        [Test]
        public void SetFormula_WithNullFormula_ShouldSucceed()
        {
            // Act
            var result = _range.SetFormula(null);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetFormula with empty formula should succeed
        /// </summary>
        [Test]
        public void SetFormula_WithEmptyFormula_ShouldSucceed()
        {
            // Act
            var result = _range.SetFormula("");

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region GetFormula Tests

        /// <summary>
        /// Test GetFormula should succeed
        /// </summary>
        [Test]
        public void GetFormula_ShouldSucceed()
        {
            // Act
            var result = _range.GetFormula();

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region Clear Tests

        /// <summary>
        /// Test Clear method should succeed
        /// </summary>
        [Test]
        public void Clear_ShouldSucceed()
        {
            // Act
            var result = _range.Clear();

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region Copy Tests

        /// <summary>
        /// Test Copy with valid destination should succeed
        /// </summary>
        [Test]
        public void Copy_WithValidDestination_ShouldSucceed()
        {
            // Arrange
            var destination = new MockRange();

            // Act
            var result = _range.Copy(destination);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test Copy with null destination should fail
        /// </summary>
        [Test]
        public void Copy_WithNullDestination_ShouldFail()
        {
            // Act
            var result = _range.Copy(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Destination range cannot be null");
        }

        #endregion

        #region AutoFit Tests

        /// <summary>
        /// Test AutoFit method should succeed
        /// </summary>
        [Test]
        public void AutoFit_ShouldSucceed()
        {
            // Act
            var result = _range.AutoFit();

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region Merge Tests

        /// <summary>
        /// Test Merge method should succeed
        /// </summary>
        [Test]
        public void Merge_ShouldSucceed()
        {
            // Act
            var result = _range.Merge();

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region GetSubRange Tests

        /// <summary>
        /// Test GetSubRange with valid parameters should succeed
        /// </summary>
        [Test]
        public void GetSubRange_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var startRow = 1;
            var startColumn = 1;
            var endRow = 2;
            var endColumn = 2;

            // Act
            var result = _range.GetSubRange(startRow, startColumn, endRow, endColumn);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IRange>(result.Value);
        }

        /// <summary>
        /// Test GetSubRange with invalid start row should fail
        /// </summary>
        [Test]
        public void GetSubRange_WithInvalidStartRow_ShouldFail()
        {
            // Arrange
            var startRow = 0; // Invalid
            var startColumn = 1;
            var endRow = 2;
            var endColumn = 2;

            // Act
            var result = _range.GetSubRange(startRow, startColumn, endRow, endColumn);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Start row must be greater than 0");
        }

        /// <summary>
        /// Test GetSubRange with invalid start column should fail
        /// </summary>
        [Test]
        public void GetSubRange_WithInvalidStartColumn_ShouldFail()
        {
            // Arrange
            var startRow = 1;
            var startColumn = 0; // Invalid
            var endRow = 2;
            var endColumn = 2;

            // Act
            var result = _range.GetSubRange(startRow, startColumn, endRow, endColumn);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Start column must be greater than 0");
        }

        /// <summary>
        /// Test GetSubRange with invalid end row should fail
        /// </summary>
        [Test]
        public void GetSubRange_WithInvalidEndRow_ShouldFail()
        {
            // Arrange
            var startRow = 1;
            var startColumn = 1;
            var endRow = 0; // Invalid
            var endColumn = 2;

            // Act
            var result = _range.GetSubRange(startRow, startColumn, endRow, endColumn);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "End row must be greater than 0");
        }

        /// <summary>
        /// Test GetSubRange with invalid end column should fail
        /// </summary>
        [Test]
        public void GetSubRange_WithInvalidEndColumn_ShouldFail()
        {
            // Arrange
            var startRow = 1;
            var startColumn = 1;
            var endRow = 2;
            var endColumn = 0; // Invalid

            // Act
            var result = _range.GetSubRange(startRow, startColumn, endRow, endColumn);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "End column must be greater than 0");
        }

        /// <summary>
        /// Test GetSubRange with start greater than end should fail
        /// </summary>
        [Test]
        public void GetSubRange_WithStartGreaterThanEnd_ShouldFail()
        {
            // Arrange
            var startRow = 3;
            var startColumn = 3;
            var endRow = 1; // Less than start
            var endColumn = 1;

            // Act
            var result = _range.GetSubRange(startRow, startColumn, endRow, endColumn);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Start position must be less than or equal to end position");
        }

        #endregion

        #region Disposal Tests

        /// <summary>
        /// Test that range can be disposed multiple times without throwing
        /// </summary>
        [Test]
        public void Dispose_MultipleCalls_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _range.Dispose();
                _range.Dispose();
            });
        }

        /// <summary>
        /// Test that disposed range throws on operations
        /// </summary>
        [Test]
        public void DisposedRange_ShouldThrowOnOperations()
        {
            // Arrange
            _range.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => { var _ = _range.Address; });
            Assert.Throws<ObjectDisposedException>(() => { var _ = _range.Value; });
            Assert.Throws<ObjectDisposedException>(() => { var _ = _range.Formula; });
            Assert.Throws<ObjectDisposedException>(() => _range.GetCell(1, 1));
            Assert.Throws<ObjectDisposedException>(() => _range.SetValue("Value"));
            Assert.Throws<ObjectDisposedException>(() => _range.GetValue());
            Assert.Throws<ObjectDisposedException>(() => _range.SetFormula("=1+1"));
            Assert.Throws<ObjectDisposedException>(() => _range.GetFormula());
            Assert.Throws<ObjectDisposedException>(() => _range.Clear());
            Assert.Throws<ObjectDisposedException>(() => _range.Copy(new MockRange()));
            Assert.Throws<ObjectDisposedException>(() => _range.AutoFit());
            Assert.Throws<ObjectDisposedException>(() => _range.Merge());
            Assert.Throws<ObjectDisposedException>(() => _range.GetSubRange(1, 1, 2, 2));
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with value operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithValueOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test set value
            var setValueResult = _range.SetValue("Test Value");
            AssertResultSuccess(setValueResult);

            // Test get value
            var getValueResult = _range.GetValue();
            AssertResultSuccess(getValueResult);
            Assert.AreEqual("Test Value", getValueResult.Value);

            // Test set formula
            var setFormulaResult = _range.SetFormula("=1+1");
            AssertResultSuccess(setFormulaResult);

            // Test get formula
            var getFormulaResult = _range.GetFormula();
            AssertResultSuccess(getFormulaResult);
            Assert.AreEqual("=1+1", getFormulaResult.Value);

            // Test clear
            var clearResult = _range.Clear();
            AssertResultSuccess(clearResult);
        }

        /// <summary>
        /// Test complete workflow with cell operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithCellOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test get cell
            var getCellResult = _range.GetCell(1, 1);
            AssertResultSuccess(getCellResult);
            Assert.IsNotNull(getCellResult.Value);

            // Test get sub range
            var getSubRangeResult = _range.GetSubRange(1, 1, 2, 2);
            AssertResultSuccess(getSubRangeResult);
            Assert.IsNotNull(getSubRangeResult.Value);
        }

        /// <summary>
        /// Test complete workflow with range operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithRangeOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test copy
            var destination = new MockRange();
            var copyResult = _range.Copy(destination);
            AssertResultSuccess(copyResult);

            // Test auto fit
            var autoFitResult = _range.AutoFit();
            AssertResultSuccess(autoFitResult);

            // Test merge
            var mergeResult = _range.Merge();
            AssertResultSuccess(mergeResult);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a valid SpreadsheetDocument for testing
        /// </summary>
        /// <returns>A valid SpreadsheetDocument</returns>
        private SpreadsheetDocument CreateValidSpreadsheetDocument()
        {
            var stream = new MemoryStream();
            var document = SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
            
            // Add workbook part
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
            workbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
            
            return document;
        }

        /// <summary>
        /// Creates a valid WorksheetPart for testing
        /// </summary>
        /// <returns>A valid WorksheetPart</returns>
        private WorksheetPart CreateValidWorksheetPart()
        {
            var workbookPart = _spreadsheetDocument.WorkbookPart;
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();
            worksheetPart.Worksheet.AppendChild(new SheetData());
            
            return worksheetPart;
        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// Mock implementation of IRange for testing
    /// </summary>
    public class MockRange : IRange
    {
        public string Address { get; set; } = "A1";
        public object Value { get; set; } = "Test Value";
        public string Formula { get; set; } = "=1+1";

        public ICell GetCell(int row, int column)
        {
            return new MockCell { Row = row, Column = column };
        }

        public void SetValue(object value)
        {
            Value = value;
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetFormula(string formula)
        {
            Formula = formula;
        }

        public string GetFormula()
        {
            return Formula;
        }

        public void Clear()
        {
            Value = null;
            Formula = null;
        }

        public void Copy(IRange destination)
        {
            // Mock implementation
        }

        public void AutoFit()
        {
            // Mock implementation
        }

        public void Merge()
        {
            // Mock implementation
        }

        public IRange GetSubRange(int startRow, int startColumn, int endRow, int endColumn)
        {
            return new MockRange { Address = $"{startRow}:{startColumn}:{endRow}:{endColumn}" };
        }
    }

    /// <summary>
    /// Mock implementation of ICell for testing
    /// </summary>
    public class MockCell : ICell
    {
        public int Row { get; set; } = 1;
        public int Column { get; set; } = 1;
        public object Value { get; set; } = "Test Cell Value";
        public string Formula { get; set; } = "=1+1";

        public void SetValue(object value)
        {
            Value = value;
        }

        public object GetValue()
        {
            return Value;
        }

        public void SetFormula(string formula)
        {
            Formula = formula;
        }

        public string GetFormula()
        {
            return Formula;
        }

        public void Clear()
        {
            Value = null;
            Formula = null;
        }
    }

    #endregion
}
