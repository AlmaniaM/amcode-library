using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Factories;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Infrastructure
{
    /// <summary>
    /// Unit tests for WorkbookFactory class
    /// </summary>
    [TestFixture]
    public class WorkbookFactoryTests : UnitTestBase
    {
        private IWorkbookEngine _mockEngine;
        private IWorkbookLogger _mockLogger;
        private IWorkbookValidator _mockValidator;
        private WorkbookFactory _factory;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _mockEngine = CreateMockWorkbookEngine();
            _mockLogger = CreateMockWorkbookLogger();
            _mockValidator = CreateMockWorkbookValidator();
            _factory = new WorkbookFactory(_mockEngine, _mockLogger, _mockValidator);
        }

        #region Constructor Tests

        /// <summary>
        /// Test constructor with valid dependencies should succeed
        /// </summary>
        [Test]
        public void Constructor_WithValidDependencies_ShouldSucceed()
        {
            // Act & Assert
            Assert.IsNotNull(_factory);
        }

        /// <summary>
        /// Test constructor with null engine should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullEngine_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new WorkbookFactory(null, _mockLogger, _mockValidator));
        }

        /// <summary>
        /// Test constructor with null logger should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullLogger_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new WorkbookFactory(_mockEngine, null, _mockValidator));
        }

        /// <summary>
        /// Test constructor with null validator should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullValidator_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new WorkbookFactory(_mockEngine, _mockLogger, null));
        }

        #endregion

        #region CreateWorkbook Tests

        /// <summary>
        /// Test CreateWorkbook should succeed
        /// </summary>
        [Test]
        public void CreateWorkbook_ShouldSucceed()
        {
            // Act
            var result = _factory.CreateWorkbook();

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);
        }

        /// <summary>
        /// Test CreateWorkbook with valid title should succeed
        /// </summary>
        [Test]
        public void CreateWorkbook_WithValidTitle_ShouldSucceed()
        {
            // Arrange
            var title = "Test Workbook";

            // Act
            var result = _factory.CreateWorkbook(title);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);
        }

        /// <summary>
        /// Test CreateWorkbook with null title should succeed
        /// </summary>
        [Test]
        public void CreateWorkbook_WithNullTitle_ShouldSucceed()
        {
            // Act
            var result = _factory.CreateWorkbook(null);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        /// <summary>
        /// Test CreateWorkbook with empty title should succeed
        /// </summary>
        [Test]
        public void CreateWorkbook_WithEmptyTitle_ShouldSucceed()
        {
            // Act
            var result = _factory.CreateWorkbook("");

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        /// <summary>
        /// Test CreateWorkbook with whitespace title should succeed
        /// </summary>
        [Test]
        public void CreateWorkbook_WithWhitespaceTitle_ShouldSucceed()
        {
            // Act
            var result = _factory.CreateWorkbook("   ");

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        /// <summary>
        /// Test CreateWorkbook with very long title should succeed
        /// </summary>
        [Test]
        public void CreateWorkbook_WithVeryLongTitle_ShouldSucceed()
        {
            // Arrange
            var longTitle = new string('A', 1000);

            // Act
            var result = _factory.CreateWorkbook(longTitle);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
        }

        #endregion

        #region OpenWorkbook(Stream) Tests

        /// <summary>
        /// Test OpenWorkbook with valid stream should succeed
        /// </summary>
        [Test]
        public void OpenWorkbook_WithValidStream_ShouldSucceed()
        {
            // Arrange
            using var stream = CreateValidExcelStream();

            // Act
            var result = _factory.OpenWorkbook(stream);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);
        }

        /// <summary>
        /// Test OpenWorkbook with null stream should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithNullStream_ShouldFail()
        {
            // Act
            var result = _factory.OpenWorkbook((Stream)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream cannot be null");
        }

        /// <summary>
        /// Test OpenWorkbook with disposed stream should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithDisposedStream_ShouldFail()
        {
            // Arrange
            var stream = CreateValidExcelStream();
            stream.Dispose();

            // Act
            var result = _factory.OpenWorkbook(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream is disposed");
        }

        /// <summary>
        /// Test OpenWorkbook with empty stream should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithEmptyStream_ShouldFail()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            var result = _factory.OpenWorkbook(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream is empty");
        }

        /// <summary>
        /// Test OpenWorkbook with invalid Excel stream should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithInvalidExcelStream_ShouldFail()
        {
            // Arrange
            using var stream = new MemoryStream();
            var data = System.Text.Encoding.UTF8.GetBytes("Not an Excel file");
            stream.Write(data, 0, data.Length);
            stream.Position = 0;

            // Act
            var result = _factory.OpenWorkbook(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid Excel file format");
        }

        #endregion

        #region OpenWorkbook(string) Tests

        /// <summary>
        /// Test OpenWorkbook with valid file path should succeed
        /// </summary>
        [Test]
        public void OpenWorkbook_WithValidFilePath_ShouldSucceed()
        {
            // Arrange
            var filePath = CreateValidExcelFile();

            // Act
            var result = _factory.OpenWorkbook(filePath);

            // Assert
            AssertResultSuccess(result);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IWorkbookDomain>(result.Value);
        }

        /// <summary>
        /// Test OpenWorkbook with null file path should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithNullFilePath_ShouldFail()
        {
            // Act
            var result = _factory.OpenWorkbook((string)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test OpenWorkbook with empty file path should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithEmptyFilePath_ShouldFail()
        {
            // Act
            var result = _factory.OpenWorkbook("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test OpenWorkbook with whitespace file path should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithWhitespaceFilePath_ShouldFail()
        {
            // Act
            var result = _factory.OpenWorkbook("   ");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test OpenWorkbook with non-existent file path should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithNonExistentFilePath_ShouldFail()
        {
            // Arrange
            var filePath = Path.Combine(TestOutputDirectory, "NonExistentFile.xlsx");

            // Act
            var result = _factory.OpenWorkbook(filePath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File does not exist");
        }

        /// <summary>
        /// Test OpenWorkbook with invalid file path should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithInvalidFilePath_ShouldFail()
        {
            // Arrange
            var invalidPath = "C:\\Invalid<>|?*Path\\File.xlsx";

            // Act
            var result = _factory.OpenWorkbook(invalidPath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid file path");
        }

        /// <summary>
        /// Test OpenWorkbook with non-Excel file should fail
        /// </summary>
        [Test]
        public void OpenWorkbook_WithNonExcelFile_ShouldFail()
        {
            // Arrange
            var filePath = CreateTempFilePath(".txt");
            File.WriteAllText(filePath, "This is not an Excel file");

            // Act
            var result = _factory.OpenWorkbook(filePath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid Excel file format");
        }

        #endregion

        #region Error Handling Tests

        /// <summary>
        /// Test CreateWorkbook with engine throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void CreateWorkbook_WithEngineThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingEngine = new ExceptionThrowingWorkbookEngine();
            var factory = new WorkbookFactory(exceptionThrowingEngine, _mockLogger, _mockValidator);

            // Act
            var result = factory.CreateWorkbook();

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to create workbook");
        }

        /// <summary>
        /// Test OpenWorkbook with engine throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void OpenWorkbook_WithEngineThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingEngine = new ExceptionThrowingWorkbookEngine();
            var factory = new WorkbookFactory(exceptionThrowingEngine, _mockLogger, _mockValidator);
            using var stream = CreateValidExcelStream();

            // Act
            var result = factory.OpenWorkbook(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to open workbook");
        }

        /// <summary>
        /// Test CreateWorkbook with validator throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void CreateWorkbook_WithValidatorThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingValidator = new ExceptionThrowingWorkbookValidator();
            var factory = new WorkbookFactory(_mockEngine, _mockLogger, exceptionThrowingValidator);

            // Act
            var result = factory.CreateWorkbook();

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to validate workbook");
        }

        #endregion

        #region Logging Tests

        /// <summary>
        /// Test that successful operations are logged
        /// </summary>
        [Test]
        public void SuccessfulOperations_ShouldBeLogged()
        {
            // Arrange
            var mockLogger = CreateMockWorkbookLogger();
            var factory = new WorkbookFactory(_mockEngine, mockLogger, _mockValidator);

            // Act
            var result = factory.CreateWorkbook();

            // Assert
            AssertResultSuccess(result);
            Assert.IsTrue(mockLogger.LoggedOperations.Count > 0);
        }

        /// <summary>
        /// Test that failed operations are logged
        /// </summary>
        [Test]
        public void FailedOperations_ShouldBeLogged()
        {
            // Arrange
            var mockLogger = CreateMockWorkbookLogger();
            var exceptionThrowingEngine = new ExceptionThrowingWorkbookEngine();
            var factory = new WorkbookFactory(exceptionThrowingEngine, mockLogger, _mockValidator);

            // Act
            var result = factory.CreateWorkbook();

            // Assert
            AssertResultFailure(result);
            Assert.IsTrue(mockLogger.LoggedErrors.Count > 0);
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with all operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithAllOperations_ShouldSucceed()
        {
            // Act & Assert
            // Test create workbook
            var createResult = _factory.CreateWorkbook("Test Workbook");
            AssertResultSuccess(createResult);
            Assert.IsNotNull(createResult.Value);

            // Test open workbook from stream
            using var stream = CreateValidExcelStream();
            var openStreamResult = _factory.OpenWorkbook(stream);
            AssertResultSuccess(openStreamResult);
            Assert.IsNotNull(openStreamResult.Value);

            // Test open workbook from file
            var filePath = CreateValidExcelFile();
            var openFileResult = _factory.OpenWorkbook(filePath);
            AssertResultSuccess(openFileResult);
            Assert.IsNotNull(openFileResult.Value);
        }

        /// <summary>
        /// Test that created workbook can be used for operations
        /// </summary>
        [Test]
        public void CreatedWorkbook_CanBeUsedForOperations()
        {
            // Arrange
            var result = _factory.CreateWorkbook("Test Workbook");
            AssertResultSuccess(result);
            var workbook = result.Value;

            // Act & Assert
            // Test basic operations
            Assert.IsNotNull(workbook.Id);
            Assert.IsNotNull(workbook.Title);
            Assert.IsNotNull(workbook.Worksheets);
            Assert.IsNotNull(workbook.Author);
            Assert.IsNotNull(workbook.Subject);
            Assert.IsNotNull(workbook.Company);

            // Test save operations
            using var stream = new MemoryStream();
            var saveStreamResult = workbook.SaveAs(stream);
            AssertResultSuccess(saveStreamResult);

            var filePath = CreateTempFilePath();
            var saveFileResult = workbook.SaveAs(filePath);
            AssertResultSuccess(saveFileResult);
            AssertFileExistsAndHasContent(filePath);
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
    /// Mock workbook engine that throws exceptions for testing error handling
    /// </summary>
    public class ExceptionThrowingWorkbookEngine : IWorkbookEngine
    {
        public Result<object> Create()
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public Result<object> Open(Stream stream)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public Result<object> Open(string filePath)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }
    }

    /// <summary>
    /// Mock workbook validator that throws exceptions for testing error handling
    /// </summary>
    public class ExceptionThrowingWorkbookValidator : IWorkbookValidator
    {
        public Result<bool> ValidateWorkbook(IWorkbookDomain workbook)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public Result<bool> ValidateWorksheet(IWorksheetContent worksheet)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public Result<bool> ValidateRange(IRange range)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public Result<bool> ValidateCellReference(string cellReference)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }
    }

    #endregion
}
