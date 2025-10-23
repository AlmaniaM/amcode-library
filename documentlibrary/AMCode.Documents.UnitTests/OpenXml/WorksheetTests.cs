using NUnit.Framework;
using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Documents.UnitTests.OpenXml
{
    [TestFixture]
    public class WorksheetTests
    {
        private ExcelApplication _excelApplication;
        private IWorkbook _workbook;
        private IWorksheet _worksheet;

        [SetUp]
        public void SetUp()
        {
            _excelApplication = new ExcelApplication();
            _workbook = _excelApplication.Workbooks.Create();
            _worksheet = _workbook.Worksheets.Create("TestSheet");
        }

        [TearDown]
        public void TearDown()
        {
            _excelApplication?.Dispose();
        }

        [Test]
        public void ShouldHaveCorrectName()
        {
            // Assert
            Assert.That(_worksheet.Name, Is.EqualTo("TestSheet"));
        }

        [Test]
        public void ShouldSetName()
        {
            // Act
            _worksheet.Name = "NewName";

            // Assert
            Assert.That(_worksheet.Name, Is.EqualTo("NewName"));
        }

        [Test]
        public void ShouldHaveRangeProperty()
        {
            // Assert
            Assert.That(_worksheet.Range, Is.Not.Null);
        }

        [Test]
        public void ShouldHaveCellsProperty()
        {
            // Assert
            Assert.That(_worksheet.Cells, Is.Not.Null);
        }

        [Test]
        public void ShouldGetRangeByAddress()
        {
            // Act
            var range = _worksheet["A1"];

            // Assert
            Assert.That(range, Is.Not.Null);
        }

        [Test]
        public void ShouldGetRangeByRowColumn()
        {
            // Act
            var range = _worksheet[1, 1];

            // Assert
            Assert.That(range, Is.Not.Null);
        }

        [Test]
        public void ShouldGetRangeByRowColumnRange()
        {
            // Act
            var range = _worksheet[1, 1, 5, 5];

            // Assert
            Assert.That(range, Is.Not.Null);
        }

        [Test]
        public void ShouldHandleMultipleRangeOperations()
        {
            // Act
            var range1 = _worksheet["A1"];
            var range2 = _worksheet[1, 1];
            var range3 = _worksheet[1, 1, 3, 3];

            // Assert
            Assert.That(range1, Is.Not.Null);
            Assert.That(range2, Is.Not.Null);
            Assert.That(range3, Is.Not.Null);
        }

        [Test]
        public void ShouldHandleNameChanges()
        {
            // Act
            _worksheet.Name = "FirstChange";
            var name1 = _worksheet.Name;
            _worksheet.Name = "SecondChange";
            var name2 = _worksheet.Name;

            // Assert
            Assert.That(name1, Is.EqualTo("FirstChange"));
            Assert.That(name2, Is.EqualTo("SecondChange"));
        }

        [Test]
        public void ShouldMaintainConsistencyAfterOperations()
        {
            // Act
            var range = _worksheet["A1:B2"];
            _worksheet.Name = "UpdatedName";

            // Assert
            Assert.That(_worksheet.Name, Is.EqualTo("UpdatedName"));
            Assert.That(range, Is.Not.Null);
            Assert.That(_worksheet.Range, Is.Not.Null);
        }
    }
}
