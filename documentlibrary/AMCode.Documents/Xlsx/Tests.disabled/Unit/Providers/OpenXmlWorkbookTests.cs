using System;
using System.IO;
using System.Linq;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Providers.OpenXml;
using AMCode.Documents.Xlsx.Interfaces;
using NUnit.Framework;
using DocumentFormat.OpenXml.Packaging;

namespace AMCode.Documents.Xlsx.Tests.Unit.Providers
{
    /// <summary>
    /// Unit tests for OpenXmlWorkbook class
    /// </summary>
    [TestFixture]
    public class OpenXmlWorkbookTests : UnitTestBase
    {
        private SpreadsheetDocument _spreadsheetDocument;
        private OpenXmlWorkbook _workbook;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _spreadsheetDocument = CreateValidSpreadsheetDocument();
            _workbook = new OpenXmlWorkbook(_spreadsheetDocument);
        }

        /// <summary>
        /// Teardown method called after each test
        /// </summary>
        [TearDown]
        public override void TearDown()
        {
            _workbook?.Dispose();
            _spreadsheetDocument?.Dispose();
            base.TearDown();
        }

        #region Constructor Tests

        /// <summary>
        /// Test constructor with valid SpreadsheetDocument should succeed
        /// </summary>
        [Test]
        public void Constructor_WithValidSpreadsheetDocument_ShouldSucceed()
        {
            // Act & Assert
            Assert.IsNotNull(_workbook);
        }

        /// <summary>
        /// Test constructor with null SpreadsheetDocument should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullSpreadsheetDocument_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new OpenXmlWorkbook(null));
        }

        #endregion

        #region Worksheets Property Tests

        /// <summary>
        /// Test Worksheets property returns collection of worksheets
        /// </summary>
        [Test]
        public void Worksheets_ShouldReturnCollectionOfWorksheets()
        {
            // Act
            var worksheets = _workbook.Worksheets;

            // Assert
            Assert.IsNotNull(worksheets);
            Assert.IsInstanceOf<System.Collections.Generic.IEnumerable<IWorksheet>>(worksheets);
        }

        /// <summary>
        /// Test Worksheets property returns empty collection for new workbook
        /// </summary>
        [Test]
        public void Worksheets_ForNewWorkbook_ShouldReturnEmptyCollection()
        {
            // Act
            var worksheets = _workbook.Worksheets;

            // Assert
            Assert.IsNotNull(worksheets);
            Assert.AreEqual(0, worksheets.Count());
        }

        #endregion

        #region Properties Property Tests

        /// <summary>
        /// Test Properties property returns workbook properties
        /// </summary>
        [Test]
        public void Properties_ShouldReturnWorkbookProperties()
        {
            // Act
            var properties = _workbook.Properties;

            // Assert
            Assert.IsNotNull(properties);
            Assert.IsInstanceOf<IWorkbookProperties>(properties);
        }

        #endregion

        #region AddWorksheet Tests

        /// <summary>
        /// Test AddWorksheet with valid name should succeed
        /// </summary>
        [Test]
        public void AddWorksheet_WithValidName_ShouldSucceed()
        {
            // Arrange
            var worksheetName = "Test Worksheet";

            // Act
            var result = _workbook.AddWorksheet(worksheetName);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorksheet>(result.Value);
        }

        /// <summary>
        /// Test AddWorksheet with null name should fail
        /// </summary>
        [Test]
        public void AddWorksheet_WithNullName_ShouldFail()
        {
            // Act
            var result = _workbook.AddWorksheet(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test AddWorksheet with empty name should fail
        /// </summary>
        [Test]
        public void AddWorksheet_WithEmptyName_ShouldFail()
        {
            // Act
            var result = _workbook.AddWorksheet("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test AddWorksheet with whitespace name should fail
        /// </summary>
        [Test]
        public void AddWorksheet_WithWhitespaceName_ShouldFail()
        {
            // Act
            var result = _workbook.AddWorksheet("   ");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test AddWorksheet with duplicate name should fail
        /// </summary>
        [Test]
        public void AddWorksheet_WithDuplicateName_ShouldFail()
        {
            // Arrange
            var worksheetName = "Test Worksheet";
            _workbook.AddWorksheet(worksheetName);

            // Act
            var result = _workbook.AddWorksheet(worksheetName);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet with name already exists");
        }

        /// <summary>
        /// Test AddWorksheet with invalid name should fail
        /// </summary>
        [Test]
        public void AddWorksheet_WithInvalidName_ShouldFail()
        {
            // Arrange
            var invalidName = "Invalid/Name";

            // Act
            var result = _workbook.AddWorksheet(invalidName);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name contains invalid characters");
        }

        #endregion

        #region RemoveWorksheet Tests

        /// <summary>
        /// Test RemoveWorksheet with existing worksheet should succeed
        /// </summary>
        [Test]
        public void RemoveWorksheet_WithExistingWorksheet_ShouldSucceed()
        {
            // Arrange
            var worksheetName = "Test Worksheet";
            _workbook.AddWorksheet(worksheetName);

            // Act
            var result = _workbook.RemoveWorksheet(worksheetName);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test RemoveWorksheet with non-existent worksheet should fail
        /// </summary>
        [Test]
        public void RemoveWorksheet_WithNonExistentWorksheet_ShouldFail()
        {
            // Arrange
            var worksheetName = "Non Existent Worksheet";

            // Act
            var result = _workbook.RemoveWorksheet(worksheetName);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet not found");
        }

        /// <summary>
        /// Test RemoveWorksheet with null name should fail
        /// </summary>
        [Test]
        public void RemoveWorksheet_WithNullName_ShouldFail()
        {
            // Act
            var result = _workbook.RemoveWorksheet(null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test RemoveWorksheet with empty name should fail
        /// </summary>
        [Test]
        public void RemoveWorksheet_WithEmptyName_ShouldFail()
        {
            // Act
            var result = _workbook.RemoveWorksheet("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet name cannot be null or empty");
        }

        #endregion

        #region RenameWorksheet Tests

        /// <summary>
        /// Test RenameWorksheet with valid names should succeed
        /// </summary>
        [Test]
        public void RenameWorksheet_WithValidNames_ShouldSucceed()
        {
            // Arrange
            var oldName = "Old Name";
            var newName = "New Name";
            _workbook.AddWorksheet(oldName);

            // Act
            var result = _workbook.RenameWorksheet(oldName, newName);

            // Assert
            AssertResultSuccess(result);
        }

        /// <summary>
        /// Test RenameWorksheet with non-existent old name should fail
        /// </summary>
        [Test]
        public void RenameWorksheet_WithNonExistentOldName_ShouldFail()
        {
            // Arrange
            var oldName = "Non Existent";
            var newName = "New Name";

            // Act
            var result = _workbook.RenameWorksheet(oldName, newName);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet not found");
        }

        /// <summary>
        /// Test RenameWorksheet with null old name should fail
        /// </summary>
        [Test]
        public void RenameWorksheet_WithNullOldName_ShouldFail()
        {
            // Arrange
            var newName = "New Name";

            // Act
            var result = _workbook.RenameWorksheet(null, newName);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Old worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test RenameWorksheet with null new name should fail
        /// </summary>
        [Test]
        public void RenameWorksheet_WithNullNewName_ShouldFail()
        {
            // Arrange
            var oldName = "Old Name";
            _workbook.AddWorksheet(oldName);

            // Act
            var result = _workbook.RenameWorksheet(oldName, null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "New worksheet name cannot be null or empty");
        }

        /// <summary>
        /// Test RenameWorksheet with duplicate new name should fail
        /// </summary>
        [Test]
        public void RenameWorksheet_WithDuplicateNewName_ShouldFail()
        {
            // Arrange
            var oldName = "Old Name";
            var newName = "Existing Name";
            _workbook.AddWorksheet(oldName);
            _workbook.AddWorksheet(newName);

            // Act
            var result = _workbook.RenameWorksheet(oldName, newName);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Worksheet with new name already exists");
        }

        #endregion

        #region SaveAs(Stream) Tests

        /// <summary>
        /// Test SaveAs with valid stream should succeed
        /// </summary>
        [Test]
        public void SaveAs_WithValidStream_ShouldSucceed()
        {
            // Arrange
            using var stream = CreateTempStream();

            // Act
            _workbook.SaveAs(stream);

            // Assert
            AssertStreamHasContent(stream);
        }

        /// <summary>
        /// Test SaveAs with null stream should throw
        /// </summary>
        [Test]
        public void SaveAs_WithNullStream_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _workbook.SaveAs((Stream)null));
        }

        /// <summary>
        /// Test SaveAs with disposed stream should throw
        /// </summary>
        [Test]
        public void SaveAs_WithDisposedStream_ShouldThrow()
        {
            // Arrange
            var stream = CreateTempStream();
            stream.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => _workbook.SaveAs(stream));
        }

        #endregion

        #region SaveAs(string) Tests

        /// <summary>
        /// Test SaveAs with valid file path should succeed
        /// </summary>
        [Test]
        public void SaveAs_WithValidFilePath_ShouldSucceed()
        {
            // Arrange
            var filePath = CreateTempFilePath();

            // Act
            _workbook.SaveAs(filePath);

            // Assert
            AssertFileExistsAndHasContent(filePath);
        }

        /// <summary>
        /// Test SaveAs with null file path should throw
        /// </summary>
        [Test]
        public void SaveAs_WithNullFilePath_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _workbook.SaveAs((string)null));
        }

        /// <summary>
        /// Test SaveAs with empty file path should throw
        /// </summary>
        [Test]
        public void SaveAs_WithEmptyFilePath_ShouldThrowArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _workbook.SaveAs(""));
        }

        /// <summary>
        /// Test SaveAs with invalid file path should throw
        /// </summary>
        [Test]
        public void SaveAs_WithInvalidFilePath_ShouldThrow()
        {
            // Arrange
            var invalidPath = "C:\\Invalid<>|?*Path\\File.xlsx";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _workbook.SaveAs(invalidPath));
        }

        #endregion

        #region Disposal Tests

        /// <summary>
        /// Test that workbook can be disposed multiple times without throwing
        /// </summary>
        [Test]
        public void Dispose_MultipleCalls_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _workbook.Dispose();
                _workbook.Dispose();
            });
        }

        /// <summary>
        /// Test that disposed workbook throws on operations
        /// </summary>
        [Test]
        public void DisposedWorkbook_ShouldThrowOnOperations()
        {
            // Arrange
            _workbook.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => { var _ = _workbook.Worksheets; });
            Assert.Throws<ObjectDisposedException>(() => { var _ = _workbook.Properties; });
            Assert.Throws<ObjectDisposedException>(() => _workbook.AddWorksheet("Test"));
            Assert.Throws<ObjectDisposedException>(() => _workbook.RemoveWorksheet("Test"));
            Assert.Throws<ObjectDisposedException>(() => _workbook.RenameWorksheet("Old", "New"));
            Assert.Throws<ObjectDisposedException>(() => _workbook.SaveAs(CreateTempStream()));
            Assert.Throws<ObjectDisposedException>(() => _workbook.SaveAs(CreateTempFilePath()));
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with worksheet operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithWorksheetOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test add worksheet
            var addResult = _workbook.AddWorksheet("Test Worksheet");
            AssertResultSuccess(addResult);
            Assert.IsNotNull(addResult.Value);

            // Test worksheets collection
            var worksheets = _workbook.Worksheets;
            Assert.AreEqual(1, worksheets.Count());

            // Test rename worksheet
            var renameResult = _workbook.RenameWorksheet("Test Worksheet", "Renamed Worksheet");
            AssertResultSuccess(renameResult);

            // Test remove worksheet
            var removeResult = _workbook.RemoveWorksheet("Renamed Worksheet");
            AssertResultSuccess(removeResult);

            // Test worksheets collection after removal
            var worksheetsAfterRemoval = _workbook.Worksheets;
            Assert.AreEqual(0, worksheetsAfterRemoval.Count());
        }

        /// <summary>
        /// Test complete workflow with save operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithSaveOperations_ShouldSucceed()
        {
            // Arrange
            _workbook.AddWorksheet("Test Worksheet");

            // Act & Assert
            // Test save to stream
            using var stream = CreateTempStream();
            _workbook.SaveAs(stream);
            AssertStreamHasContent(stream);

            // Test save to file
            var filePath = CreateTempFilePath();
            _workbook.SaveAs(filePath);
            AssertFileExistsAndHasContent(filePath);
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

        #endregion
    }
}
