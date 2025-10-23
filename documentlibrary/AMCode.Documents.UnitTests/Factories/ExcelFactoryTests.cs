using System;
using System.IO;
using NUnit.Framework;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Factories;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Documents.UnitTests.Factories
{
    /// <summary>
    /// Unit tests for Excel factory classes
    /// Tests both WorkbookFactory (OpenXml) and ExcelApplication (Syncfusion)
    /// </summary>
    [TestFixture]
    public class ExcelFactoryTests
    {
        private IWorkbookEngine _workbookEngine;
        private IWorkbookLogger _workbookLogger;
        private IWorkbookValidator _workbookValidator;
        private IWorkbookFactory _workbookFactory;

        [SetUp]
        public void SetUp()
        {
            _workbookEngine = new OpenXmlWorkbookEngine();
            _workbookLogger = new TestWorkbookLogger();
            _workbookValidator = new TestWorkbookValidator();
            _workbookFactory = new WorkbookFactory(_workbookEngine, _workbookLogger, _workbookValidator);
        }

        [TearDown]
        public void TearDown()
        {
            _workbookFactory = null;
            _workbookEngine = null;
            _workbookLogger = null;
            _workbookValidator = null;
        }

        #region WorkbookFactory Tests

        [Test]
        public void ShouldCreateApplication()
        {
            // Act
            using var application = new ExcelApplication();

            // Assert
            Assert.IsNotNull(application);
            Assert.IsNotNull(application.Workbooks);
        }

        [Test]
        public void ShouldCreateWorkbook()
        {
            // Act
            var result = _workbookFactory.CreateWorkbook();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void ShouldCreateWorkbookWithData()
        {
            // Arrange
            var title = "Test Workbook";
            var author = "Test Author";

            // Act
            var result = _workbookFactory.CreateWorkbook(title, author);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(title, result.Value.Metadata.Title);
            Assert.AreEqual(author, result.Value.Metadata.Author);
        }

        [Test]
        public void ShouldCreateFormattedWorkbook()
        {
            // Arrange
            var metadata = new WorkbookCreationMetadata
            {
                Title = "Formatted Workbook",
                Author = "Test Author",
                Subject = "Test Subject",
                Keywords = "test, workbook, formatted"
            };

            // Act
            var result = _workbookFactory.CreateWorkbook(metadata);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(metadata.Title, result.Value.Metadata.Title);
            Assert.AreEqual(metadata.Author, result.Value.Metadata.Author);
            Assert.AreEqual(metadata.Subject, result.Value.Metadata.Subject);
            Assert.AreEqual(metadata.Keywords, result.Value.Metadata.Keywords);
        }

        [Test]
        public void ShouldHandleDisposal()
        {
            // Arrange
            using var application = new ExcelApplication();
            var workbookResult = _workbookFactory.CreateWorkbook();

            // Act & Assert
            Assert.DoesNotThrow(() => application.Dispose());
            Assert.IsTrue(workbookResult.IsSuccess);
        }

        [Test]
        public void ShouldOpenWorkbookFromStream()
        {
            // Arrange
            var createResult = _workbookFactory.CreateWorkbook("Test Workbook");
            Assert.IsTrue(createResult.IsSuccess);

            using var stream = new MemoryStream();
            var saveResult = createResult.Value.SaveAs(stream);
            Assert.IsTrue(saveResult.IsSuccess);

            stream.Position = 0;

            // Act
            var openResult = _workbookFactory.OpenWorkbook(stream);

            // Assert
            Assert.IsTrue(openResult.IsSuccess);
            Assert.IsNotNull(openResult.Value);
        }

        [Test]
        public void ShouldHandleInvalidStream()
        {
            // Arrange
            using var invalidStream = new MemoryStream();
            invalidStream.Write(new byte[] { 0x00, 0x01, 0x02 }, 0, 3);
            invalidStream.Position = 0;

            // Act
            var result = _workbookFactory.OpenWorkbook(invalidStream);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);
        }

        [Test]
        public void ShouldHandleNullDependencies()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WorkbookFactory(null, _workbookLogger, _workbookValidator));
            Assert.Throws<ArgumentNullException>(() => new WorkbookFactory(_workbookEngine, null, _workbookValidator));
            Assert.Throws<ArgumentNullException>(() => new WorkbookFactory(_workbookEngine, _workbookLogger, null));
        }

        #endregion

        #region ExcelApplication Tests

        [Test]
        public void ShouldCreateWorkbooksCollection()
        {
            // Arrange
            using var application = new ExcelApplication();

            // Act
            var workbooks = application.Workbooks;

            // Assert
            Assert.IsNotNull(workbooks);
            Assert.AreEqual(0, workbooks.Count);
        }

        [Test]
        public void ShouldCreateNewWorkbook()
        {
            // Arrange
            using var application = new ExcelApplication();

            // Act
            var workbook = application.Workbooks.Create();

            // Assert
            Assert.IsNotNull(workbook);
            Assert.AreEqual(1, application.Workbooks.Count);
        }

        [Test]
        public void ShouldCreateWorkbookWithName()
        {
            // Arrange
            using var application = new ExcelApplication();
            var workbookName = "Test Workbook";

            // Act
            var workbook = application.Workbooks.Create(workbookName);

            // Assert
            Assert.IsNotNull(workbook);
            Assert.AreEqual(workbookName, workbook.Name);
        }

        [Test]
        public void ShouldOpenWorkbookFromStream()
        {
            // Arrange
            using var application = new ExcelApplication();
            var workbook = application.Workbooks.Create("Test Workbook");
            
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            // Act
            var openedWorkbook = application.Workbooks.Open(stream);

            // Assert
            Assert.IsNotNull(openedWorkbook);
            Assert.AreEqual(2, application.Workbooks.Count);
        }

        [Test]
        public void ShouldOpenWorkbookFromFile()
        {
            // Arrange
            using var application = new ExcelApplication();
            var workbook = application.Workbooks.Create("Test Workbook");
            
            var tempFile = Path.GetTempFileName();
            try
            {
                workbook.SaveAs(tempFile);

                // Act
                var openedWorkbook = application.Workbooks.Open(tempFile);

                // Assert
                Assert.IsNotNull(openedWorkbook);
                Assert.AreEqual(2, application.Workbooks.Count);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [Test]
        public void ShouldDisposeApplication()
        {
            // Arrange
            var application = new ExcelApplication();
            var workbook = application.Workbooks.Create();

            // Act
            application.Dispose();

            // Assert
            Assert.DoesNotThrow(() => application.Dispose()); // Multiple dispose should not throw
        }

        #endregion
    }

    #region Test Helpers

    /// <summary>
    /// Test implementation of IWorkbookLogger
    /// </summary>
    public class TestWorkbookLogger : IWorkbookLogger
    {
        public void LogInfo(string message) { }
        public void LogWarning(string message) { }
        public void LogError(string message) { }
        public void LogError(string message, Exception exception) { }
    }

    /// <summary>
    /// Test implementation of IWorkbookValidator
    /// </summary>
    public class TestWorkbookValidator : IWorkbookValidator
    {
        public Result ValidateWorkbook(IWorkbookDomain workbook)
        {
            return Result.Success();
        }

        public Result ValidateWorkbookCreation(WorkbookCreationMetadata metadata)
        {
            return Result.Success();
        }
    }

    #endregion
}
