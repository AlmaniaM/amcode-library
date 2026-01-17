using System;
using AMCode.Common.Util;
using AMCode.Common.Xlsx;
using AMCode.Exports.Book;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Documents.Xlsx;
using AMCode.Documents.Xlsx.Drawing;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.Book.ExcelBookTests
{
    [TestFixture]
    public class ExcelBookRangeTest
    {
        private IExcelApplication excelApplication;
        private ExcelBook excelBook;

        [SetUp]
        public void SetUp()
        {
            excelApplication = new ExcelApplication();
            excelBook = new ExcelBook(excelApplication);
        }

        [TearDown]
        public void TearDown() => excelApplication.Dispose();

        [Test]
        public void ShouldSetStyleOnRange()
        {
            excelBook.SetRangeStyle(1, 1, 5, 5, new StyleParam
            {
                Color = Color.Blue,
                FillPattern = ExcelPattern.Solid,
                HorizontalAlignment = ExcelHAlign.HAlignRight
            });

            var worksheets = excelApplication.Workbooks[0].Worksheets;
            var worksheet = worksheets[0];
            var range = worksheet.GetRange("A1:E5");

            Assert.AreEqual(Color.Blue, range.CellStyle.Color);
            Assert.AreEqual(ExcelPattern.Solid, range.CellStyle.FillPattern);
            Assert.AreEqual(ExcelHAlign.HAlignRight, range.CellStyle.HorizontalAlignment);
        }

        [Test]
        public void ShouldSetStyleOnRangeAllSheets()
        {
            excelBook.AddSheet();

            excelBook.SetRangeStyleAllSheets(1, 1, 5, 5, new StyleParam
            {
                Color = Color.Blue,
                FillPattern = ExcelPattern.Solid,
                HorizontalAlignment = ExcelHAlign.HAlignRight
            });

            var worksheets = excelApplication.Workbooks[0].Worksheets;
            Assert.AreEqual(2, worksheets.Count);
            foreach (var worksheet in worksheets)
            {
                var range = worksheet.GetRange("A1:E5");

                Assert.AreEqual(Color.Blue, range.CellStyle.Color);
                Assert.AreEqual(ExcelPattern.Solid, range.CellStyle.FillPattern);
                Assert.AreEqual(ExcelHAlign.HAlignRight, range.CellStyle.HorizontalAlignment);
            }
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenNullStyleParam()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetRangeStyle(1, 1, 1, 1, null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<int, int, int, int, IStyleParam>(excelBook.SetRangeStyle), "styleParam"
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenNullStyleParamAllSheets()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetRangeStyleAllSheets(1, 1, 1, 1, null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<int, int, int, int, IStyleParam>(excelBook.SetRangeStyleAllSheets), "styleParam"
                )
            );
        }

        [Test]
        public void ShouldThrowIndexOutOfBoundsExceptionWhenAnyIndexIsLessThanOrEqualToZero()
        {
            var styleParam = new StyleParam();

            string createExceptionMessage(string incorrectParameters)
            {
                return ExportsExceptionUtil.CreateIndexOutOfRangeException(
                    ExceptionUtil.CreateExceptionHeader<int, int, int, int, IStyleParam>(excelBook.SetRangeStyle),
                    incorrectParameters
                );
            }

            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyle(0, 1, 1, 1, styleParam),
                createExceptionMessage("row")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyle(1, 0, 1, 1, styleParam),
                createExceptionMessage("column")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyle(1, 1, 0, 1, styleParam),
                createExceptionMessage("lastRow")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyle(1, 1, 1, 0, styleParam),
                createExceptionMessage("lastColumn")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyle(0, 0, 0, 0, styleParam),
                createExceptionMessage("row, column, lastRow, lastColumn")
            );
        }

        [Test]
        public void ShouldThrowIndexOutOfBoundsExceptionWhenAnyIndexIsLessThanOrEqualToZeroAllSheets()
        {
            var styleParam = new StyleParam();

            excelBook.AddSheet();

            string createExceptionMessage(string incorrectParameters)
            {
                return ExportsExceptionUtil.CreateIndexOutOfRangeException(
                    ExceptionUtil.CreateExceptionHeader<int, int, int, int, IStyleParam>(excelBook.SetRangeStyleAllSheets),
                    incorrectParameters
                );
            }

            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyleAllSheets(0, 1, 1, 1, styleParam),
                createExceptionMessage("row")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyleAllSheets(1, 0, 1, 1, styleParam),
                createExceptionMessage("column")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyleAllSheets(1, 1, 0, 1, styleParam),
                createExceptionMessage("lastRow")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyleAllSheets(1, 1, 1, 0, styleParam),
                createExceptionMessage("lastColumn")
            );
            Assert.Throws<IndexOutOfRangeException>(
                () => excelBook.SetRangeStyleAllSheets(0, 0, 0, 0, styleParam),
                createExceptionMessage("row, column, lastRow, lastColumn")
            );
        }
    }
}