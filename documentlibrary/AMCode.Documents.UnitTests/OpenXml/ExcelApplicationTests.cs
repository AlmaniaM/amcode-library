using NUnit.Framework;
using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Documents.UnitTests.OpenXml
{
    [TestFixture]
    public class ExcelApplicationTests
    {
        private ExcelApplication _excelApplication;

        [SetUp]
        public void SetUp()
        {
            _excelApplication = new ExcelApplication();
        }

        [TearDown]
        public void TearDown()
        {
            _excelApplication?.Dispose();
        }

        [Test]
        public void ShouldCreateExcelApplication()
        {
            // Arrange & Act
            var app = new ExcelApplication();

            // Assert
            Assert.That(app, Is.Not.Null);
            Assert.That(app.Workbooks, Is.Not.Null);
            
            // Cleanup
            app.Dispose();
        }

        [Test]
        public void ShouldHaveWorkbooksCollection()
        {
            // Assert
            Assert.That(_excelApplication.Workbooks, Is.Not.Null);
        }

        [Test]
        public void ShouldDisposeProperly()
        {
            // Arrange
            var app = new ExcelApplication();
            var workbooks = app.Workbooks;

            // Act
            app.Dispose();

            // Assert - Should not throw exception
            Assert.DoesNotThrow(() => app.Dispose());
        }

        [Test]
        public void ShouldAllowMultipleDisposeCalls()
        {
            // Arrange
            var app = new ExcelApplication();

            // Act & Assert - Should not throw exception
            Assert.DoesNotThrow(() => app.Dispose());
            Assert.DoesNotThrow(() => app.Dispose());
        }

        [Test]
        public void ShouldCreateNewWorkbook()
        {
            // Act
            var workbook = _excelApplication.Workbooks.Create();

            // Assert
            Assert.That(workbook, Is.Not.Null);
        }

        [Test]
        public void ShouldHaveCorrectWorkbookCount()
        {
            // Arrange
            var initialCount = _excelApplication.Workbooks.Count;

            // Act
            _excelApplication.Workbooks.Create();
            var newCount = _excelApplication.Workbooks.Count;

            // Assert
            Assert.That(initialCount, Is.EqualTo(0));
            Assert.That(newCount, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCreateMultipleWorkbooks()
        {
            // Act
            var workbook1 = _excelApplication.Workbooks.Create();
            var workbook2 = _excelApplication.Workbooks.Create();

            // Assert
            Assert.That(workbook1, Is.Not.Null);
            Assert.That(workbook2, Is.Not.Null);
            Assert.That(workbook1, Is.Not.EqualTo(workbook2));
            Assert.That(_excelApplication.Workbooks.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShouldHandleWorkbookOperations()
        {
            // Act
            var workbook = _excelApplication.Workbooks.Create();
            var worksheet = workbook.Worksheets.Create("TestSheet");

            // Assert
            Assert.That(workbook, Is.Not.Null);
            Assert.That(worksheet, Is.Not.Null);
            Assert.That(worksheet.Name, Is.EqualTo("TestSheet"));
        }
    }
}
