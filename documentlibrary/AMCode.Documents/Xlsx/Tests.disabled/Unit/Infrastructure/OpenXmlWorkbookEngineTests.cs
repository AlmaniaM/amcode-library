using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Infrastructure
{
    /// <summary>
    /// Unit tests for OpenXmlWorkbookEngine class
    /// </summary>
    [TestFixture]
    public class OpenXmlWorkbookEngineTests : UnitTestBase
    {
        private OpenXmlWorkbookEngine _engine;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _engine = new OpenXmlWorkbookEngine();
        }

        #region Create Tests

        /// <summary>
        /// Test Create method should succeed
        /// </summary>
        [Test]
        public void Create_ShouldSucceed()
        {
            // Act
            var result = _engine.Create();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        /// <summary>
        /// Test Create method returns valid SpreadsheetDocument
        /// </summary>
        [Test]
        public void Create_ShouldReturnValidSpreadsheetDocument()
        {
            // Act
            var result = _engine.Create();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<DocumentFormat.OpenXml.Packaging.SpreadsheetDocument>(result.Value);
        }

        #endregion

        #region Open(Stream) Tests

        /// <summary>
        /// Test Open with valid stream should succeed
        /// </summary>
        [Test]
        public void Open_WithValidStream_ShouldSucceed()
        {
            // Arrange
            using var stream = CreateValidExcelStream();

            // Act
            var result = _engine.Open(stream);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        /// <summary>
        /// Test Open with null stream should fail
        /// </summary>
        [Test]
        public void Open_WithNullStream_ShouldFail()
        {
            // Act
            var result = _engine.Open((Stream)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream cannot be null");
        }

        /// <summary>
        /// Test Open with disposed stream should fail
        /// </summary>
        [Test]
        public void Open_WithDisposedStream_ShouldFail()
        {
            // Arrange
            var stream = CreateValidExcelStream();
            stream.Dispose();

            // Act
            var result = _engine.Open(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream is disposed");
        }

        /// <summary>
        /// Test Open with empty stream should fail
        /// </summary>
        [Test]
        public void Open_WithEmptyStream_ShouldFail()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            var result = _engine.Open(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream is empty");
        }

        /// <summary>
        /// Test Open with invalid Excel stream should fail
        /// </summary>
        [Test]
        public void Open_WithInvalidExcelStream_ShouldFail()
        {
            // Arrange
            using var stream = new MemoryStream();
            var data = System.Text.Encoding.UTF8.GetBytes("Not an Excel file");
            stream.Write(data, 0, data.Length);
            stream.Position = 0;

            // Act
            var result = _engine.Open(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid Excel file format");
        }

        /// <summary>
        /// Test Open with non-readable stream should fail
        /// </summary>
        [Test]
        public void Open_WithNonReadableStream_ShouldFail()
        {
            // Arrange
            using var stream = new MemoryStream();
            stream.Close(); // Make it non-readable

            // Act
            var result = _engine.Open(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream is not readable");
        }

        #endregion

        #region Open(string) Tests

        /// <summary>
        /// Test Open with valid file path should succeed
        /// </summary>
        [Test]
        public void Open_WithValidFilePath_ShouldSucceed()
        {
            // Arrange
            var filePath = CreateValidExcelFile();

            // Act
            var result = _engine.Open(filePath);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        /// <summary>
        /// Test Open with null file path should fail
        /// </summary>
        [Test]
        public void Open_WithNullFilePath_ShouldFail()
        {
            // Act
            var result = _engine.Open((string)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test Open with empty file path should fail
        /// </summary>
        [Test]
        public void Open_WithEmptyFilePath_ShouldFail()
        {
            // Act
            var result = _engine.Open("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test Open with whitespace file path should fail
        /// </summary>
        [Test]
        public void Open_WithWhitespaceFilePath_ShouldFail()
        {
            // Act
            var result = _engine.Open("   ");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test Open with non-existent file path should fail
        /// </summary>
        [Test]
        public void Open_WithNonExistentFilePath_ShouldFail()
        {
            // Arrange
            var filePath = Path.Combine(TestOutputDirectory, "NonExistentFile.xlsx");

            // Act
            var result = _engine.Open(filePath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File does not exist");
        }

        /// <summary>
        /// Test Open with invalid file path should fail
        /// </summary>
        [Test]
        public void Open_WithInvalidFilePath_ShouldFail()
        {
            // Arrange
            var invalidPath = "C:\\Invalid<>|?*Path\\File.xlsx";

            // Act
            var result = _engine.Open(invalidPath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid file path");
        }

        /// <summary>
        /// Test Open with file path containing invalid characters should fail
        /// </summary>
        [Test]
        public void Open_WithFilePathContainingInvalidCharacters_ShouldFail()
        {
            // Arrange
            var invalidPath = "C:\\Invalid<>|?*Path\\File.xlsx";

            // Act
            var result = _engine.Open(invalidPath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid file path");
        }

        /// <summary>
        /// Test Open with non-Excel file should fail
        /// </summary>
        [Test]
        public void Open_WithNonExcelFile_ShouldFail()
        {
            // Arrange
            var filePath = CreateTempFilePath(".txt");
            File.WriteAllText(filePath, "This is not an Excel file");

            // Act
            var result = _engine.Open(filePath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid Excel file format");
        }

        #endregion

        #region Error Handling Tests

        /// <summary>
        /// Test Create with underlying OpenXml throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void Create_WithUnderlyingOpenXmlThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingEngine = new ExceptionThrowingOpenXmlWorkbookEngine();

            // Act
            var result = exceptionThrowingEngine.Create();

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to create SpreadsheetDocument");
        }

        /// <summary>
        /// Test Open with underlying OpenXml throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void Open_WithUnderlyingOpenXmlThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingEngine = new ExceptionThrowingOpenXmlWorkbookEngine();
            using var stream = CreateValidExcelStream();

            // Act
            var result = exceptionThrowingEngine.Open(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to open SpreadsheetDocument");
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with create and open operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithCreateAndOpenOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test create
            var createResult = _engine.Create();
            AssertResultSuccess(createResult);
            Assert.IsNotNull(createResult.Value);

            // Test open from stream
            using var stream = CreateValidExcelStream();
            var openStreamResult = _engine.Open(stream);
            AssertResultSuccess(openStreamResult);
            Assert.IsNotNull(openStreamResult.Value);

            // Test open from file
            var filePath = CreateValidExcelFile();
            var openFileResult = _engine.Open(filePath);
            AssertResultSuccess(openFileResult);
            Assert.IsNotNull(openFileResult.Value);
        }

        /// <summary>
        /// Test that created document can be saved and reopened
        /// </summary>
        [Test]
        public void CreatedDocument_CanBeSavedAndReopened()
        {
            // Arrange
            var filePath = CreateTempFilePath();

            // Act
            // Create document
            var createResult = _engine.Create();
            AssertResultSuccess(createResult);

            // Save document (this would be done by the workbook)
            using (var document = createResult.Value as DocumentFormat.OpenXml.Packaging.SpreadsheetDocument)
            {
                // Mock save operation
                using var stream = new FileStream(filePath, FileMode.Create);
                document.SaveAs(stream);
            }

            // Reopen document
            var openResult = _engine.Open(filePath);

            // Assert
            AssertResultSuccess(openResult);
            Assert.IsNotNull(openResult.Value);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a valid Excel stream for testing
        /// </summary>
        /// <returns>A valid Excel stream</returns>
        private MemoryStream CreateValidExcelStream()
        {
            var stream = new MemoryStream();
            using (var document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                // Add workbook part
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
            }
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Creates a valid Excel file for testing
        /// </summary>
        /// <returns>A valid Excel file path</returns>
        private string CreateValidExcelFile()
        {
            var filePath = CreateTempFilePath();
            using (var document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(filePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                // Add workbook part
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
            }
            return filePath;
        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// Mock OpenXml workbook engine that throws exceptions for testing error handling
    /// </summary>
    public class ExceptionThrowingOpenXmlWorkbookEngine : OpenXmlWorkbookEngine
    {
        public override Result<object> Create()
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public override Result<object> Open(Stream stream)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public override Result<object> Open(string filePath)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }
    }

    #endregion
}
