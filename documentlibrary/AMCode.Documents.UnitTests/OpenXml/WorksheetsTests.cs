using NUnit.Framework;
using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Documents.UnitTests.OpenXml
{
    [TestFixture]
    public class WorksheetsTests
    {
        private ExcelApplication _excelApplication;
        private IWorkbook _workbook;
        private IWorksheets _worksheets;

        [SetUp]
        public void SetUp()
        {
            _excelApplication = new ExcelApplication();
            _workbook = _excelApplication.Workbooks.Create();
            _worksheets = _workbook.Worksheets;
        }

        [TearDown]
        public void TearDown()
        {
            _excelApplication?.Dispose();
        }

        [Test]
        public void ShouldHaveCorrectInitialCount()
        {
            // Assert
            Assert.That(_worksheets.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldCreateNewWorksheet()
        {
            // Act
            var worksheet = _worksheets.Create();

            // Assert
            Assert.That(worksheet, Is.Not.Null);
            Assert.That(_worksheets.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCreateNamedWorksheet()
        {
            // Act
            var worksheet = _worksheets.Create("TestSheet");

            // Assert
            Assert.That(worksheet, Is.Not.Null);
            Assert.That(worksheet.Name, Is.EqualTo("TestSheet"));
            Assert.That(_worksheets.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCreateMultipleWorksheets()
        {
            // Act
            var worksheet1 = _worksheets.Create("Sheet1");
            var worksheet2 = _worksheets.Create("Sheet2");

            // Assert
            Assert.That(worksheet1, Is.Not.Null);
            Assert.That(worksheet2, Is.Not.Null);
            Assert.That(worksheet1.Name, Is.EqualTo("Sheet1"));
            Assert.That(worksheet2.Name, Is.EqualTo("Sheet2"));
            Assert.That(_worksheets.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShouldGetWorksheetByIndex()
        {
            // Arrange
            var createdWorksheet = _worksheets.Create("TestSheet");

            // Act
            var retrievedWorksheet = _worksheets[0];

            // Assert
            Assert.That(retrievedWorksheet, Is.Not.Null);
            Assert.That(retrievedWorksheet.Name, Is.EqualTo("TestSheet"));
        }

        [Test]
        public void ShouldGetWorksheetByName()
        {
            // Arrange
            var createdWorksheet = _worksheets.Create("TestSheet");

            // Act
            var retrievedWorksheet = _worksheets["TestSheet"];

            // Assert
            Assert.That(retrievedWorksheet, Is.Not.Null);
            Assert.That(retrievedWorksheet.Name, Is.EqualTo("TestSheet"));
        }

        [Test]
        public void ShouldGetWorksheetByIndexMethod()
        {
            // Arrange
            var createdWorksheet = _worksheets.Create("TestSheet");

            // Act
            var retrievedWorksheet = _worksheets.GetWorksheet(0);

            // Assert
            Assert.That(retrievedWorksheet, Is.Not.Null);
            Assert.That(retrievedWorksheet.Name, Is.EqualTo("TestSheet"));
        }

        [Test]
        public void ShouldGetWorksheetByNameMethod()
        {
            // Arrange
            var createdWorksheet = _worksheets.Create("TestSheet");

            // Act
            var retrievedWorksheet = _worksheets.GetWorksheet("TestSheet");

            // Assert
            Assert.That(retrievedWorksheet, Is.Not.Null);
            Assert.That(retrievedWorksheet.Name, Is.EqualTo("TestSheet"));
        }
    }
}
