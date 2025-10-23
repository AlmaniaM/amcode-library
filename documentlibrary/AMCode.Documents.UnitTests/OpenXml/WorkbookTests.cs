using NUnit.Framework;
using AMCode.SyncfusionIo.Xlsx;
using System.IO;

namespace AMCode.Documents.UnitTests.OpenXml
{
    [TestFixture]
    public class WorkbookTests
    {
        private ExcelApplication _excelApplication;
        private IWorkbook _workbook;

        [SetUp]
        public void SetUp()
        {
            _excelApplication = new ExcelApplication();
            _workbook = _excelApplication.Workbooks.Create();
        }

        [TearDown]
        public void TearDown()
        {
            _excelApplication?.Dispose();
        }

        [Test]
        public void ShouldHaveWorkbooksReference()
        {
            // Assert
            Assert.That(_workbook.Workbooks, Is.Not.Null);
        }

        [Test]
        public void ShouldHaveWorksheetsCollection()
        {
            // Assert
            Assert.That(_workbook.Worksheets, Is.Not.Null);
        }

        [Test]
        public void ShouldHaveStylesCollection()
        {
            // Assert
            Assert.That(_workbook.Styles, Is.Not.Null);
        }

        [Test]
        public void ShouldCreateNewWorksheet()
        {
            // Arrange
            var initialCount = _workbook.Worksheets.Count;

            // Act
            var worksheet = _workbook.Worksheets.Create();

            // Assert
            Assert.That(worksheet, Is.Not.Null);
            Assert.That(_workbook.Worksheets.Count, Is.EqualTo(initialCount + 1));
        }

        [Test]
        public void ShouldCreateNamedWorksheet()
        {
            // Act
            var worksheet = _workbook.Worksheets.Create("TestSheet");

            // Assert
            Assert.That(worksheet, Is.Not.Null);
            Assert.That(worksheet.Name, Is.EqualTo("TestSheet"));
        }

        [Test]
        public void ShouldSaveToStream()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            _workbook.SaveAs(stream);

            // Assert
            Assert.That(stream.Length, Is.GreaterThan(0));
        }

        [Test]
        public void ShouldCloseWorkbook()
        {
            // Act & Assert - Should not throw exception
            Assert.DoesNotThrow(() => _workbook.Close());
        }

        [Test]
        public void ShouldHandleMultipleOperations()
        {
            // Act
            var worksheet1 = _workbook.Worksheets.Create("Sheet1");
            var worksheet2 = _workbook.Worksheets.Create("Sheet2");
            using var stream = new MemoryStream();
            _workbook.SaveAs(stream);

            // Assert
            Assert.That(worksheet1, Is.Not.Null);
            Assert.That(worksheet2, Is.Not.Null);
            Assert.That(worksheet1.Name, Is.EqualTo("Sheet1"));
            Assert.That(worksheet2.Name, Is.EqualTo("Sheet2"));
            Assert.That(stream.Length, Is.GreaterThan(0));
        }
    }
}
