using NUnit.Framework;
using AMCode.SyncfusionIo.Xlsx;

namespace AMCode.Documents.UnitTests.OpenXml
{
    [TestFixture]
    public class RangeTests
    {
        private ExcelApplication _excelApplication;
        private IWorkbook _workbook;
        private IWorksheet _worksheet;
        private IRange _range;

        [SetUp]
        public void SetUp()
        {
            _excelApplication = new ExcelApplication();
            _workbook = _excelApplication.Workbooks.Create();
            _worksheet = _workbook.Worksheets.Create("TestSheet");
            _range = _worksheet["A1"];
        }

        [TearDown]
        public void TearDown()
        {
            _excelApplication?.Dispose();
        }

        [Test]
        public void ShouldCreateRangeFromAddress()
        {
            // Act
            var range = _worksheet["A1"];

            // Assert
            Assert.That(range, Is.Not.Null);
        }

        [Test]
        public void ShouldCreateRangeFromRowColumn()
        {
            // Act
            var range = _worksheet[1, 1];

            // Assert
            Assert.That(range, Is.Not.Null);
        }

        [Test]
        public void ShouldCreateRangeFromRowColumnRange()
        {
            // Act
            var range = _worksheet[1, 1, 5, 5];

            // Assert
            Assert.That(range, Is.Not.Null);
        }

        [Test]
        public void ShouldSetValue()
        {
            // Act
            _range.Value = "Test Value";

            // Assert
            Assert.That(_range.Value, Is.EqualTo("Test Value"));
        }

        [Test]
        public void ShouldSetNumericValue()
        {
            // Act
            _range.Number = 42;

            // Assert
            Assert.That(_range.Number, Is.EqualTo(42));
        }

        [Test]
        public void ShouldSetText()
        {
            // Act
            _range.Text = "Test Text";

            // Assert
            Assert.That(_range.Text, Is.EqualTo("Test Text"));
        }

        [Test]
        public void ShouldHaveCellStyleProperty()
        {
            // Assert
            Assert.That(_range.CellStyle, Is.Not.Null);
        }

        [Test]
        public void ShouldHandleDifferentRangeTypes()
        {
            // Act
            var singleCell = _worksheet["A1"];
            var multiCell = _worksheet["A1:B2"];
            var rowRange = _worksheet[1, 1, 1, 5];

            // Assert
            Assert.That(singleCell, Is.Not.Null);
            Assert.That(multiCell, Is.Not.Null);
            Assert.That(rowRange, Is.Not.Null);
        }

        [Test]
        public void ShouldMaintainValueAfterOperations()
        {
            // Arrange
            _range.Value = "Initial Value";

            // Act
            var retrievedValue = _range.Value;
            _range.Text = "Updated Text";

            // Assert
            Assert.That(retrievedValue, Is.EqualTo("Initial Value"));
            Assert.That(_range.Text, Is.EqualTo("Updated Text"));
        }

        [Test]
        public void ShouldHandleComplexOperations()
        {
            // Act
            _range.Value = "Test";
            _range.Text = "Test Text";
            var cellStyle = _range.CellStyle;

            // Assert
            Assert.That(_range.Text, Is.EqualTo("Test Text"));
            Assert.That(cellStyle, Is.Not.Null);
        }
    }
}
