using NUnit.Framework;
using AMCode.SyncfusionIo.Xlsx;
using AMCode.Common.Xlsx;
using AMCode.SyncfusionIo.Xlsx.Common;
using AMCode.SyncfusionIo.Xlsx.Drawing;

namespace AMCode.Documents.UnitTests.OpenXml
{
    [TestFixture]
    public class StyleTests
    {
        private ExcelApplication _excelApplication;
        private IWorkbook _workbook;
        private IWorksheet _worksheet;
        private IRange _range;
        private IStyle _style;

        [SetUp]
        public void SetUp()
        {
            _excelApplication = new ExcelApplication();
            _workbook = _excelApplication.Workbooks.Create();
            _worksheet = _workbook.Worksheets.Create("TestSheet");
            _range = _worksheet["A1"];
            _style = _range.CellStyle;
        }

        [TearDown]
        public void TearDown()
        {
            _excelApplication?.Dispose();
        }

        [Test]
        public void ShouldCreateStyle()
        {
            // Assert
            Assert.That(_style, Is.Not.Null);
        }

        [Test]
        public void ShouldHaveFontProperty()
        {
            // Assert
            Assert.That(_style.Font, Is.Not.Null);
        }

        [Test]
        public void ShouldHaveBordersProperty()
        {
            // Assert
            Assert.That(_style.Borders, Is.Not.Null);
        }

        [Test]
        public void ShouldSetNumberFormat()
        {
            // Act
            _style.NumberFormat = "0.00";

            // Assert
            Assert.That(_style.NumberFormat, Is.EqualTo("0.00"));
        }

        [Test]
        public void ShouldSetHorizontalAlignment()
        {
            // Act
            _style.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            // Assert
            Assert.That(_style.HorizontalAlignment, Is.EqualTo(ExcelHAlign.HAlignCenter));
        }

        [Test]
        public void ShouldSetColor()
        {
            // Act
            _style.Color = Color.Red;

            // Assert
            Assert.That(_style.Color, Is.Not.Null);
        }

        [Test]
        public void ShouldSetFillPattern()
        {
            // Act
            _style.FillPattern = ExcelPattern.Solid;

            // Assert
            Assert.That(_style.FillPattern, Is.EqualTo(ExcelPattern.Solid));
        }

        [Test]
        public void ShouldHandleMultipleStyleProperties()
        {
            // Act
            _style.NumberFormat = "0.00%";
            _style.HorizontalAlignment = ExcelHAlign.HAlignRight;
            _style.Color = Color.Blue;
            _style.FillPattern = ExcelPattern.Solid;

            // Assert
            Assert.That(_style.NumberFormat, Is.EqualTo("0.00%"));
            Assert.That(_style.HorizontalAlignment, Is.EqualTo(ExcelHAlign.HAlignRight));
            Assert.That(_style.Color, Is.Not.Null);
            Assert.That(_style.FillPattern, Is.EqualTo(ExcelPattern.Solid));
        }
    }
}
