using System;
using System.IO;
using System.Linq;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Interfaces;
using NUnit.Framework;

namespace AMCode.Documents.Xlsx.Tests.Unit.Infrastructure
{
    /// <summary>
    /// Unit tests for WorkbookContentAdapter class
    /// </summary>
    [TestFixture]
    public class WorkbookContentAdapterTests : UnitTestBase
    {
        private IWorkbook _mockWorkbook;
        private WorkbookContentAdapter _adapter;

        /// <summary>
        /// Setup method called before each test
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _mockWorkbook = CreateMockWorkbook();
            _adapter = new WorkbookContentAdapter(_mockWorkbook);
        }

        #region Constructor Tests

        /// <summary>
        /// Test constructor with valid IWorkbook should succeed
        /// </summary>
        [Test]
        public void Constructor_WithValidWorkbook_ShouldSucceed()
        {
            // Act & Assert
            Assert.IsNotNull(_adapter);
        }

        /// <summary>
        /// Test constructor with null IWorkbook should throw
        /// </summary>
        [Test]
        public void Constructor_WithNullWorkbook_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WorkbookContentAdapter(null));
        }

        #endregion

        #region Worksheets Property Tests

        /// <summary>
        /// Test Worksheets property delegates to underlying workbook
        /// </summary>
        [Test]
        public void Worksheets_ShouldDelegateToUnderlyingWorkbook()
        {
            // Act
            var worksheets = _adapter.Worksheets;

            // Assert
            Assert.IsNotNull(worksheets);
            Assert.AreEqual(_mockWorkbook.Worksheets, worksheets);
        }

        /// <summary>
        /// Test Worksheets property returns same instance as underlying workbook
        /// </summary>
        [Test]
        public void Worksheets_ShouldReturnSameInstanceAsUnderlyingWorkbook()
        {
            // Act
            var adapterWorksheets = _adapter.Worksheets;
            var workbookWorksheets = _mockWorkbook.Worksheets;

            // Assert
            Assert.AreSame(workbookWorksheets, adapterWorksheets);
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
            var result = _adapter.SaveAs(stream);

            // Assert
            AssertResultSuccess(result);
            AssertStreamHasContent(stream);
        }

        /// <summary>
        /// Test SaveAs with null stream should fail
        /// </summary>
        [Test]
        public void SaveAs_WithNullStream_ShouldFail()
        {
            // Act
            var result = _adapter.SaveAs((Stream)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream cannot be null");
        }

        /// <summary>
        /// Test SaveAs with disposed stream should fail
        /// </summary>
        [Test]
        public void SaveAs_WithDisposedStream_ShouldFail()
        {
            // Arrange
            var stream = CreateTempStream();
            stream.Dispose();

            // Act
            var result = _adapter.SaveAs(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream is disposed");
        }

        /// <summary>
        /// Test SaveAs with read-only stream should fail
        /// </summary>
        [Test]
        public void SaveAs_WithReadOnlyStream_ShouldFail()
        {
            // Arrange
            using var stream = new MemoryStream();
            stream.Close(); // Make it read-only

            // Act
            var result = _adapter.SaveAs(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Stream is not writable");
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
            var result = _adapter.SaveAs(filePath);

            // Assert
            AssertResultSuccess(result);
            AssertFileExistsAndHasContent(filePath);
        }

        /// <summary>
        /// Test SaveAs with null file path should fail
        /// </summary>
        [Test]
        public void SaveAs_WithNullFilePath_ShouldFail()
        {
            // Act
            var result = _adapter.SaveAs((string)null);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test SaveAs with empty file path should fail
        /// </summary>
        [Test]
        public void SaveAs_WithEmptyFilePath_ShouldFail()
        {
            // Act
            var result = _adapter.SaveAs("");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test SaveAs with whitespace file path should fail
        /// </summary>
        [Test]
        public void SaveAs_WithWhitespaceFilePath_ShouldFail()
        {
            // Act
            var result = _adapter.SaveAs("   ");

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path cannot be null or empty");
        }

        /// <summary>
        /// Test SaveAs with invalid file path should fail
        /// </summary>
        [Test]
        public void SaveAs_WithInvalidFilePath_ShouldFail()
        {
            // Arrange
            var invalidPath = "C:\\Invalid\\Path\\That\\Does\\Not\\Exist\\File.xlsx";

            // Act
            var result = _adapter.SaveAs(invalidPath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Invalid file path");
        }

        /// <summary>
        /// Test SaveAs with file path containing invalid characters should fail
        /// </summary>
        [Test]
        public void SaveAs_WithFilePathContainingInvalidCharacters_ShouldFail()
        {
            // Arrange
            var invalidPath = "C:\\Invalid<>|?*Path\\File.xlsx";

            // Act
            var result = _adapter.SaveAs(invalidPath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "File path contains invalid characters");
        }

        #endregion

        #region Error Handling Tests

        /// <summary>
        /// Test SaveAs with underlying workbook throwing exception should handle gracefully
        /// </summary>
        [Test]
        public void SaveAs_WithUnderlyingWorkbookThrowingException_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingWorkbook = new ExceptionThrowingWorkbook();
            var adapter = new WorkbookContentAdapter(exceptionThrowingWorkbook);
            using var stream = CreateTempStream();

            // Act
            var result = adapter.SaveAs(stream);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to save workbook");
        }

        /// <summary>
        /// Test SaveAs with underlying workbook throwing exception for file path should handle gracefully
        /// </summary>
        [Test]
        public void SaveAs_WithUnderlyingWorkbookThrowingExceptionForFilePath_ShouldHandleGracefully()
        {
            // Arrange
            var exceptionThrowingWorkbook = new ExceptionThrowingWorkbook();
            var adapter = new WorkbookContentAdapter(exceptionThrowingWorkbook);
            var filePath = CreateTempFilePath();

            // Act
            var result = adapter.SaveAs(filePath);

            // Assert
            AssertResultFailure(result);
            AssertResultFailureWithError(result, "Failed to save workbook");
        }

        #endregion

        #region Disposal Tests

        /// <summary>
        /// Test that adapter can be disposed multiple times without throwing
        /// </summary>
        [Test]
        public void Dispose_MultipleCalls_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _adapter.Dispose();
                _adapter.Dispose();
            });
        }

        /// <summary>
        /// Test that disposed adapter throws on operations
        /// </summary>
        [Test]
        public void DisposedAdapter_ShouldThrowOnOperations()
        {
            // Arrange
            _adapter.Dispose();

            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => { var _ = _adapter.Worksheets; });
            Assert.Throws<ObjectDisposedException>(() => _adapter.SaveAs(CreateTempStream()));
            Assert.Throws<ObjectDisposedException>(() => _adapter.SaveAs(CreateTempFilePath()));
        }

        #endregion

        #region Integration Tests

        /// <summary>
        /// Test complete workflow with valid operations
        /// </summary>
        [Test]
        public void CompleteWorkflow_WithValidOperations_ShouldSucceed()
        {
            // Arrange
            using var stream = CreateTempStream();
            var filePath = CreateTempFilePath();

            // Act & Assert
            // Test stream save
            var streamResult = _adapter.SaveAs(stream);
            AssertResultSuccess(streamResult);
            AssertStreamHasContent(stream);

            // Test file save
            var fileResult = _adapter.SaveAs(filePath);
            AssertResultSuccess(fileResult);
            AssertFileExistsAndHasContent(filePath);

            // Test worksheets access
            var worksheets = _adapter.Worksheets;
            Assert.IsNotNull(worksheets);
        }

        #endregion
    }

    #region Helper Classes

    /// <summary>
    /// Mock workbook that throws exceptions for testing error handling
    /// </summary>
    public class ExceptionThrowingWorkbook : IWorkbook
    {
        public System.Collections.Generic.IEnumerable<IWorksheet> Worksheets { get; } = new List<IWorksheet>();
        public IWorkbookProperties Properties { get; } = new MockWorkbookProperties();

        public IWorksheet AddWorksheet(string name)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public void RemoveWorksheet(string name)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public void RenameWorksheet(string oldName, string newName)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public void SaveAs(Stream stream)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public void SaveAs(string filePath)
        {
            throw new InvalidOperationException("Mock exception for testing");
        }

        public void Dispose()
        {
            // Mock implementation
        }
    }

    #endregion
}
