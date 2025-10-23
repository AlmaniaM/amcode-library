using NUnit.Framework;
using AMCode.SyncfusionIo.Xlsx;
using System.IO;

namespace AMCode.Documents.UnitTests.OpenXml
{
    [TestFixture]
    public class WorkbooksTests
    {
        private ExcelApplication _excelApplication;
        private IWorkbooks _workbooks;

        [SetUp]
        public void SetUp()
        {
            _excelApplication = new ExcelApplication();
            _workbooks = _excelApplication.Workbooks;
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
            Assert.That(_workbooks.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldCreateNewWorkbook()
        {
            // Act
            var workbook = _workbooks.Create();

            // Assert
            Assert.That(workbook, Is.Not.Null);
            Assert.That(_workbooks.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldCreateMultipleWorkbooks()
        {
            // Act
            var workbook1 = _workbooks.Create();
            var workbook2 = _workbooks.Create();

            // Assert
            Assert.That(workbook1, Is.Not.Null);
            Assert.That(workbook2, Is.Not.Null);
            Assert.That(workbook1, Is.Not.EqualTo(workbook2));
            Assert.That(_workbooks.Count, Is.EqualTo(2));
        }

        [Test]
        public void ShouldGetWorkbookByIndex()
        {
            // Arrange
            var createdWorkbook = _workbooks.Create();

            // Act
            var retrievedWorkbook = _workbooks[0];

            // Assert
            Assert.That(retrievedWorkbook, Is.Not.Null);
            Assert.That(retrievedWorkbook, Is.Not.EqualTo(createdWorkbook)); // Different instances
        }

        [Test]
        public void ShouldGetWorkbookByIndexMethod()
        {
            // Arrange
            var createdWorkbook = _workbooks.Create();

            // Act
            var retrievedWorkbook = _workbooks.GetWorkbook(0);

            // Assert
            Assert.That(retrievedWorkbook, Is.Not.Null);
            Assert.That(retrievedWorkbook, Is.Not.EqualTo(createdWorkbook)); // Different instances
        }

        [Test]
        public void ShouldReturnNullForInvalidIndex()
        {
            // Act
            var workbook = _workbooks.GetWorkbook(-1);

            // Assert
            Assert.That(workbook, Is.Null);
        }

        [Test]
        public void ShouldReturnNullForOutOfRangeIndex()
        {
            // Act
            var workbook = _workbooks.GetWorkbook(999);

            // Assert
            Assert.That(workbook, Is.Null);
        }

        [Test]
        public void ShouldHaveApplicationReference()
        {
            // Assert
            Assert.That(_workbooks.Application, Is.Not.Null);
            Assert.That(_workbooks.Application, Is.EqualTo(_excelApplication));
        }
    }
}
