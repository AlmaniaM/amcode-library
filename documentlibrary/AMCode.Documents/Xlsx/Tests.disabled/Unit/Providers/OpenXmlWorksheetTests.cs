using System;
using System.IO;
using System.Linq;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Providers.OpenXml;
using AMCode.Documents.Xlsx.Interfaces;
using NUnit.Framework;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AMCode.Documents.Xlsx.Tests.Unit.Providers
{
    /// <summary>
    /// Unit tests for OpenXmlWorksheet class
    /// </summary>
    [TestFixture]
    public class OpenXmlWorksheetTests : UnitTestBase
    {
        private SpreadsheetDocument _spreadsheetDocument;
        private WorksheetPart _worksheetPart;
        private OpenXmlWorksheet _worksheet;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _spreadsheetDocument = CreateValidSpreadsheetDocument();
            _worksheetPart = CreateValidWorksheetPart();
            _worksheet = new OpenXmlWorksheet(_worksheetPart);
        }

        /// <summary>
        /// Teardown method called after each test
        /// </summary>
        [TearDown]
        public override void TearDown()
        {
            _worksheet?.Dispose();
            _worksheetPart?.Dispose();
            _spreadsheetDocument?.Dispose();
            base.TearDown();
        }

        #region Constructor Tests

        /// <summary>
        /// Test constructor with valid WorksheetPart should succeed
        /// </summary>
        [Test]
        public void Constructor_WithValidWorksheetPart_ShouldSucceed()
        {
            // Act & Assert
            Assert.IsNotNull(_worksheet);
        }

        /// <summary>
        /// Test constructor with null WorksheetPart should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullWorksheetPart_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OpenXmlWorksheet(null));
        }

        #endregion

        #region Name Property Tests

        /// <summary>
        /// Test Name property returns worksheet name
        /// </summary>
        [Test]
        public void Name_ShouldReturnWorksheetName()
        {
            // Act
            var name = _worksheet.Name;

            // Assert
            Assert.IsNotNull(name);
            Assert.IsNotEmpty(name);
        }

        /// <summary>
        /// Test Name property can be set
        /// </summary>
        [Test]
        public void Name_CanBeSet()
        {
            // Arrange
            var newName = "New Worksheet Name";

            // Act
            _worksheet.Name = newName;

            // Assert
            Assert.AreEqual(newName, _worksheet.Name);
        }

        #endregion

        #region Cells Property Tests

        /// <summary>
        /// Test Cells property returns collection of cells
        /// </summary>
        [Test]
        public void Cells_ShouldReturnCollectionOfCells()
        {
            // Act
            var cells = _worksheet.Cells;

            // Assert
            Assert.IsNotNull(cells);
            Assert.IsInstanceOf<System.Collections.Generic.IEnumerable<ICell>>(cells);
        }

        /// <summary>
        /// Test Cells property returns empty collection for new worksheet
        /// </summary>
        [Test]
        public void Cells_ForNewWorksheet_ShouldReturnEmptyCollection()
        {
            // Act
            var cells = _worksheet.Cells;

            // Assert
            Assert.IsNotNull(cells);
            Assert.AreEqual(0, cells.Count());
        }

        #endregion

        #region Rows Property Tests

        /// <summary>
        /// Test Rows property returns collection of rows
        /// </summary>
        [Test]
        public void Rows_ShouldReturnCollectionOfRows()
        {
            // Act
            var rows = _worksheet.Rows;

            // Assert
            Assert.IsNotNull(rows);
            Assert.IsInstanceOf<System.Collections.Generic.IEnumerable<IRow>>(rows);
        }

        /// <summary>
        /// Test Rows property returns empty collection for new worksheet
        /// </summary>
        [Test]
        public void Rows_ForNewWorksheet_ShouldReturnEmptyCollection()
        {
            // Act
            var rows = _worksheet.Rows;

            // Assert
            Assert.IsNotNull(rows);
            Assert.AreEqual(0, rows.Count());
        }

        #endregion

        #region Columns Property Tests

        /// <summary>
        /// Test Columns property returns collection of columns
        /// </summary>
        [Test]
        public void Columns_ShouldReturnCollectionOfColumns()
        {
            // Act
            var columns = _worksheet.Columns;

            // Assert
            Assert.IsNotNull(columns);
            Assert.IsInstanceOf<System.Collections.Generic.IEnumerable<IColumn>>(columns);
        }

        /// <summary>
        /// Test Columns property returns empty collection for new worksheet
        /// </summary>
        [Test]
        public void Columns_ForNewWorksheet_ShouldReturnEmptyCollection()
        {
            // Act
            var columns = _worksheet.Columns;

            // Assert
            Assert.IsNotNull(columns);
            Assert.AreEqual(0, columns.Count());
        }

        #endregion

        #region UsedRange Property Tests

        /// <summary>
        /// Test UsedRange property returns range
        /// </summary>
        [Test]
        public void UsedRange_ShouldReturnRange()
        {
            // Act
            var usedRange = _worksheet.UsedRange;

            // Assert
            Assert.IsNotNull(usedRange);
            Assert.IsInstanceOf<IRange>(usedRange);
        }

        #endregion

        #region GetRange Tests

        /// <summary>
        /// Test GetRange with valid address should succeed
        /// </summary>
        [Test]
        public void GetRange_WithValidAddress_ShouldSucceed()
        {
            // Arrange
            var address = "A1:B2";

            // Act
            var result = _worksheet.GetRange(address);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IRange>(result.Value);
        }

        /// <summary>
        /// Test GetRange with null address should fail
        /// </summary>
        [Test]
        public void GetRange_WithNullAddress_ShouldFail()
        {
            // Act
            var result = _worksheet.GetRange(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range address cannot be null or empty");
        }

        /// <summary>
        /// Test GetRange with empty address should fail
        /// </summary>
        [Test]
        public void GetRange_WithEmptyAddress_ShouldFail()
        {
            // Act
            var result = _worksheet.GetRange("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Range address cannot be null or empty");
        }

        /// <summary>
        /// Test GetRange with invalid address should fail
        /// </summary>
        [Test]
        public void GetRange_WithInvalidAddress_ShouldFail()
        {
            // Arrange
            var invalidAddress = "Invalid:Address";

            // Act
            var result = _worksheet.GetRange(invalidAddress);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid range address format");
        }

        #endregion

        #region SetCellValue Tests

        /// <summary>
        /// Test SetCellValue with valid parameters should succeed
        /// </summary>
        [Test]
        public void SetCellValue_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var row = 1;
            var column = 1;
            var value = "Test Value";

            // Act
            var result = _worksheet.SetCellValue(row, column, value);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetCellValue with invalid row should fail
        /// </summary>
        [Test]
        public void SetCellValue_WithInvalidRow_ShouldFail()
        {
            // Arrange
            var row = 0; // Invalid row
            var column = 1;
            var value = "Test Value";

            // Act
            var result = _worksheet.SetCellValue(row, column, value);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Row must be greater than 0");
        }

        /// <summary>
        /// Test SetCellValue with invalid column should fail
        /// </summary>
        [Test]
        public void SetCellValue_WithInvalidColumn_ShouldFail()
        {
            // Arrange
            var row = 1;
            var column = 0; // Invalid column
            var value = "Test Value";

            // Act
            var result = _worksheet.SetCellValue(row, column, value);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Column must be greater than 0");
        }

        /// <summary>
        /// Test SetCellValue with null value should succeed
        /// </summary>
        [Test]
        public void SetCellValue_WithNullValue_ShouldSucceed()
        {
            // Arrange
            var row = 1;
            var column = 1;
            object value = null;

            // Act
            var result = _worksheet.SetCellValue(row, column, value);

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region GetCellValue Tests

        /// <summary>
        /// Test GetCellValue with valid parameters should succeed
        /// </summary>
        [Test]
        public void GetCellValue_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var row = 1;
            var column = 1;

            // Act
            var result = _worksheet.GetCellValue(row, column);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test GetCellValue with invalid row should fail
        /// </summary>
        [Test]
        public void GetCellValue_WithInvalidRow_ShouldFail()
        {
            // Arrange
            var row = 0; // Invalid row
            var column = 1;

            // Act
            var result = _worksheet.GetCellValue(row, column);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Row must be greater than 0");
        }

        /// <summary>
        /// Test GetCellValue with invalid column should fail
        /// </summary>
        [Test]
        public void GetCellValue_WithInvalidColumn_ShouldFail()
        {
            // Arrange
            var row = 1;
            var column = 0; // Invalid column

            // Act
            var result = _worksheet.GetCellValue(row, column);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Column must be greater than 0");
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
            var result = _worksheet.Clear();

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region SetColumnWidth Tests

        /// <summary>
        /// Test SetColumnWidth with valid parameters should succeed
        /// </summary>
        [Test]
        public void SetColumnWidth_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var column = 1;
            var width = 15.0;

            // Act
            var result = _worksheet.SetColumnWidth(column, width);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetColumnWidth with invalid column should fail
        /// </summary>
        [Test]
        public void SetColumnWidth_WithInvalidColumn_ShouldFail()
        {
            // Arrange
            var column = 0; // Invalid column
            var width = 15.0;

            // Act
            var result = _worksheet.SetColumnWidth(column, width);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Column must be greater than 0");
        }

        /// <summary>
        /// Test SetColumnWidth with negative width should fail
        /// </summary>
        [Test]
        public void SetColumnWidth_WithNegativeWidth_ShouldFail()
        {
            // Arrange
            var column = 1;
            var width = -1.0;

            // Act
            var result = _worksheet.SetColumnWidth(column, width);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Width must be greater than 0");
        }

        #endregion

        #region SetRowHeight Tests

        /// <summary>
        /// Test SetRowHeight with valid parameters should succeed
        /// </summary>
        [Test]
        public void SetRowHeight_WithValidParameters_ShouldSucceed()
        {
            // Arrange
            var row = 1;
            var height = 20.0;

            // Act
            var result = _worksheet.SetRowHeight(row, height);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test SetRowHeight with invalid row should fail
        /// </summary>
        [Test]
        public void SetRowHeight_WithInvalidRow_ShouldFail()
        {
            // Arrange
            var row = 0; // Invalid row
            var height = 20.0;

            // Act
            var result = _worksheet.SetRowHeight(row, height);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Row must be greater than 0");
        }

        /// <summary>
        /// Test SetRowHeight with negative height should fail
        /// </summary>
        [Test]
        public void SetRowHeight_WithNegativeHeight_ShouldFail()
        {
            // Arrange
            var row = 1;
            var height = -1.0;

            // Act
            var result = _worksheet.SetRowHeight(row, height);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Height must be greater than 0");
        }

        #endregion

        #region AutoFitColumn Tests

        /// <summary>
        /// Test AutoFitColumn with valid column should succeed
        /// </summary>
        [Test]
        public void AutoFitColumn_WithValidColumn_ShouldSucceed()
        {
            // Arrange
            var column = 1;

            // Act
            var result = _worksheet.AutoFitColumn(column);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test AutoFitColumn with invalid column should fail
        /// </summary>
        [Test]
        public void AutoFitColumn_WithInvalidColumn_ShouldFail()
        {
            // Arrange
            var column = 0; // Invalid column

            // Act
            var result = _worksheet.AutoFitColumn(column);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Column must be greater than 0");
        }

        #endregion

        #region AutoFitRow Tests

        /// <summary>
        /// Test AutoFitRow with valid row should succeed
        /// </summary>
        [Test]
        public void AutoFitRow_WithValidRow_ShouldSucceed()
        {
            // Arrange
            var row = 1;

            // Act
            var result = _worksheet.AutoFitRow(row);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test AutoFitRow with invalid row should fail
        /// </summary>
        [Test]
        public void AutoFitRow_WithInvalidRow_ShouldFail()
        {
            // Arrange
            var row = 0; // Invalid row

            // Act
            var result = _worksheet.AutoFitRow(row);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Row must be greater than 0");
        }

        #endregion

        #region Activate/Deactivate Tests

        /// <summary>
        /// Test Activate method should succeed
        /// </summary>
        [Test]
        public void Activate_ShouldSucceed()
        {
            // Act
            var result = _worksheet.Activate();

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test Deactivate method should succeed
        /// </summary>
        [Test]
        public void Deactivate_ShouldSucceed()
        {
            // Act
            var result = _worksheet.Deactivate();

            // Assert
            AssertResultSuccess(result);
        }

        #endregion

        #region Disposal Tests

        /// <summary>
        /// Test that worksheet can be disposed multiple times without throwing
        /// </summary>
        [Test]
        public void Dispose_MultipleCalls_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _worksheet.Dispose();
                _worksheet.Dispose();
            });
        }

        /// <summary>
        /// Test that disposed worksheet throws on operations
        /// </summary>
        [Test]
        public void DisposedWorksheet_ShouldThrowOnOperations()
        {
            // Arrange
            _worksheet.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => { var _ = _worksheet.Name; });
            Assert.Throws<ObjectDisposedException>(() => { var _ = _worksheet.Cells; });
            Assert.Throws<ObjectDisposedException>(() => { var _ = _worksheet.Rows; });
            Assert.Throws<ObjectDisposedException>(() => { var _ = _worksheet.Columns; });
            Assert.Throws<ObjectDisposedException>(() => { var _ = _worksheet.UsedRange; });
            Assert.Throws<ObjectDisposedException>(() => _worksheet.GetRange("A1"));
            Assert.Throws<ObjectDisposedException>(() => _worksheet.SetCellValue(1, 1, "Value"));
            Assert.Throws<ObjectDisposedException>(() => _worksheet.GetCellValue(1, 1));
            Assert.Throws<ObjectDisposedException>(() => _worksheet.Clear());
            Assert.Throws<ObjectDisposedException>(() => _worksheet.SetColumnWidth(1, 15.0));
            Assert.Throws<ObjectDisposedException>(() => _worksheet.SetRowHeight(1, 20.0));
            Assert.Throws<ObjectDisposedException>(() => _worksheet.AutoFitColumn(1));
            Assert.Throws<ObjectDisposedException>(() => _worksheet.AutoFitRow(1));
            Assert.Throws<ObjectDisposedException>(() => _worksheet.Activate());
            Assert.Throws<ObjectDisposedException>(() => _worksheet.Deactivate());
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with cell operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithCellOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test set cell value
            var setResult = _worksheet.SetCellValue(1, 1, "Test Value");
            AssertResultSuccess(setResult);

            // Test get cell value
            var getResult = _worksheet.GetCellValue(1, 1);
            AssertResultSuccess(getResult);
            Assert.AreEqual("Test Value", getResult.Value);

            // Test get range
            var rangeResult = _worksheet.GetRange("A1");
            AssertResultSuccess(rangeResult);
            Assert.IsNotNull(rangeResult.Value);

            // Test clear
            var clearResult = _worksheet.Clear();
            AssertResultSuccess(clearResult);
        }

        /// <summary>
        /// Test complete workflow with formatting operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithFormattingOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test set column width
            var columnWidthResult = _worksheet.SetColumnWidth(1, 15.0);
            AssertResultSuccess(columnWidthResult);

            // Test set row height
            var rowHeightResult = _worksheet.SetRowHeight(1, 20.0);
            AssertResultSuccess(rowHeightResult);

            // Test auto fit column
            var autoFitColumnResult = _worksheet.AutoFitColumn(1);
            AssertResultSuccess(autoFitColumnResult);

            // Test auto fit row
            var autoFitRowResult = _worksheet.AutoFitRow(1);
            AssertResultSuccess(autoFitRowResult);
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
}
