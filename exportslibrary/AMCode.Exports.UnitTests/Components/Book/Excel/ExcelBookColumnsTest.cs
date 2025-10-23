using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common.Exceptions;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Xlsx;
using AMCode.Xlsx.Drawing;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.Book.ExcelBookTests
{
    [TestFixture]
    public class ExcelBookColumnsTest
    {
        private IEnumerable<ExcelDataColumn> columns;
        private IList<ExpandoObject> data;
        private IExcelApplication excelApplication;
        private ExcelBook excelBook;
        private List<IColumnStyleParam> styles;

        [SetUp]
        public void SetUp()
        {
            excelApplication = new ExcelApplication();
            excelBook = new ExcelBook(excelApplication);

            data = Enumerable.Range(0, 4).Select(index => new ExpandoObject()
                    .AddOrUpdatePropertyWithValue("Column1", $"Value 1{index}")
                    .AddOrUpdatePropertyWithValue("Column2", $"Value 2{index}")
                    .AddOrUpdatePropertyWithValue("Column3", $"Value 3{index}")
                    .AddOrUpdatePropertyWithValue("Column4", $"Value 4{index}")
                    .AddOrUpdatePropertyWithValue("Column5", $"Value 5{index}")
                    ).ToList();

            columns = Enumerable.Range(1, 5).Select(index => new ExcelDataColumn
            {
                DataType = typeof(string),
                WorksheetHeaderName = $"Column{index}",
            });

            styles = new List<IColumnStyleParam>
            {
                new ColumnStyleParam
                {
                    ColumnIndex = 2,
                    ColumnName = "Column2",
                    Bold = true
                },
                new ColumnStyleParam
                {
                    ColumnIndex = 5,
                    ColumnName = "Column5",
                    Color = Color.Green
                }
            };
        }

        [TearDown]
        public void TearDown() => excelApplication.Dispose();

        [Test]
        public void ShouldSetColumns()
        {
            excelBook.SetColumns(new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" });
            var range = excelApplication.Workbooks[0].Worksheets.GetWorksheet("Sheet 1").GetRange("A1:E1");
            Assert.AreEqual("Column1", range["A1"].Text);
            Assert.AreEqual("Column2", range["B1"].Text);
            Assert.AreEqual("Column3", range["C1"].Text);
            Assert.AreEqual("Column4", range["D1"].Text);
            Assert.AreEqual("Column5", range["E1"].Text);
        }

        [Test]
        public void ShouldSetColumnsAllSheets()
        {
            excelBook.AddSheet();

            excelBook.SetColumnsAllSheets(new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" });

            var worksheets = excelApplication.Workbooks[0].Worksheets;
            Assert.AreEqual(2, worksheets.Count);
            foreach (var worksheet in worksheets)
            {
                var range = worksheet.GetRange("A1:E1");
                Assert.AreEqual("Column1", range["A1"].Text);
                Assert.AreEqual("Column2", range["B1"].Text);
                Assert.AreEqual("Column3", range["C1"].Text);
                Assert.AreEqual("Column4", range["D1"].Text);
                Assert.AreEqual("Column5", range["E1"].Text);
            }
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenIsNullForSetColumns()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumns(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(excelBook.SetColumns), "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenCollectionIsEmptyForSetColumns()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.SetColumns(new List<string>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(excelBook.SetColumns), "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowMaxColumnCountExceededExceptionWhenCollectionIsEmptyForSetColumns()
        {
            Assert.Throws<MaxColumnCountExceededException>(
                () => excelBook.SetColumns(Enumerable.Range(0, ExcelLimitValues.MaxColumnCount + 1).Select(i => $"{i}").ToList()),
                ExportsExceptionUtil.CreateMaxColumnCountExceededExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(excelBook.SetColumns), ExcelLimitValues.MaxColumnCount
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenIsNullForSetColumnsAllSheets()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumnsAllSheets(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(excelBook.SetColumnsAllSheets), "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenCollectionIsEmptyForSetColumnsAllSheets()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.SetColumnsAllSheets(new List<string>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(excelBook.SetColumnsAllSheets), "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowMaxColumnCountExceededExceptionWhenCollectionIsEmptyForSetColumnsAllSheets()
        {
            Assert.Throws<MaxColumnCountExceededException>(
                () => excelBook.SetColumnsAllSheets(Enumerable.Range(0, ExcelLimitValues.MaxColumnCount + 1).Select(i => $"{i}").ToList()),
                ExportsExceptionUtil.CreateMaxColumnCountExceededExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<string>>(excelBook.SetColumnsAllSheets), ExcelLimitValues.MaxColumnCount
                )
            );
        }

        [Test]
        public void ShouldSetStylesByColumnIndex()
        {
            excelBook.SetColumns(new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" });
            styles.ForEach(style => style.ColumnName = null);

            excelBook.SetColumnStylesByColumnIndex(styles);

            var range = excelApplication.Workbooks[0].Worksheets.GetWorksheet("Sheet 1").GetRange("A1:E1");

            Assert.IsTrue(range.GetRange("B1").CellStyle.Font.Bold);
            Assert.AreEqual(Color.Green, range.GetRange("E1").CellStyle.Color);
        }

        [Test]
        public void ShouldSetStylesByColumnIndexAllSheets()
        {
            excelBook.AddSheet();
            excelBook.SetColumns(new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" });
            styles.ForEach(style => style.ColumnName = null);

            excelBook.SetColumnStylesByColumnIndexAllSheets(styles);

            Assert.AreEqual(2, excelApplication.Workbooks[0].Worksheets.Count);
            foreach (var worksheet in excelApplication.Workbooks[0].Worksheets)
            {
                var range = worksheet.GetRange("A1:E1");
                Assert.IsTrue(range.GetRange("B1").CellStyle.Font.Bold);
                Assert.AreEqual(Color.Green, range.GetRange("E1").CellStyle.Color);
            }
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStylesIsNullForSetStylesByColumnIndex()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumnStylesByColumnIndex(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnIndex), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenStylesCollectionIsEmptyForSetStylesByColumnIndex()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.SetColumnStylesByColumnIndex(new List<IColumnStyleParam>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnIndex), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStylesIsNullForSetStylesByColumnIndexAllSheets()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumnStylesByColumnIndexAllSheets(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnIndexAllSheets), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenStylesCollectionIsEmptyForSetStylesByColumnIndexAllSheets()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.SetColumnStylesByColumnIndexAllSheets(new List<IColumnStyleParam>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnIndexAllSheets), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldSetStylesByColumnName()
        {
            excelBook.SetColumns(new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" });
            styles.ForEach(style => style.ColumnIndex = null);

            excelBook.SetColumnStylesByColumnName(styles);

            var range = excelApplication.Workbooks[0].Worksheets.GetWorksheet("Sheet 1").GetRange("A1:E1");

            Assert.IsTrue(range.GetRange("B1").CellStyle.Font.Bold);
            Assert.AreEqual(Color.Green, range.GetRange("E1").CellStyle.Color);
        }

        [Test]
        public void ShouldSetStylesByColumnNameAllSheets()
        {
            excelBook.AddSheet();
            excelBook.SetColumnsAllSheets(new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" });
            styles.ForEach(style => style.ColumnIndex = null);

            excelBook.SetColumnStylesByColumnNameAllSheets(styles);

            Assert.AreEqual(2, excelApplication.Workbooks[0].Worksheets.Count);
            foreach (var worksheet in excelApplication.Workbooks[0].Worksheets)
            {
                var range = worksheet.GetRange("A1:E1");
                Assert.IsTrue(range.GetRange("B1").CellStyle.Font.Bold);
                Assert.AreEqual(Color.Green, range.GetRange("E1").CellStyle.Color);
            }
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStylesIsNullForSetStylesByColumnName()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumnStylesByColumnName(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnName), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenStylesCollectionIsEmptyForSetStylesByColumnName()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.SetColumnStylesByColumnName(new List<IColumnStyleParam>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnName), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenStylesIsNullForSetStylesByColumnNameAllSheets()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumnStylesByColumnNameAllSheets(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnNameAllSheets), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenStylesCollectionIsEmptyForSetStylesByColumnNameAllSheets()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.SetColumnStylesByColumnNameAllSheets(new List<IColumnStyleParam>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<IColumnStyleParam>>(excelBook.SetColumnStylesByColumnNameAllSheets), "styleParams"
                )
            );
        }

        [Test]
        public void ShouldSetColumnWidths()
        {
            excelBook.SetColumns(new List<string> { "Column1" });

            excelBook.SetColumnWidthInPixels("Column1", 20);

            var worksheet = excelApplication.Workbooks[0].Worksheets.GetWorksheet("Sheet 1");

            Assert.AreEqual(20, worksheet.GetColumnWidthInPixels("Column1"));
        }

        [Test]
        public void ShouldSetColumnWidthsAllSheets()
        {
            excelBook.AddSheet();
            excelBook.SetColumnsAllSheets(new List<string> { "Column1" });

            excelBook.SetColumnWidthInPixelsAllSheets("Column1", 20);

            Assert.AreEqual(2, excelApplication.Workbooks[0].Worksheets.Count);
            foreach (var worksheet in excelApplication.Workbooks[0].Worksheets)
            {
                Assert.AreEqual(20, worksheet.GetColumnWidthInPixels("Column1"));
            }
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenColumnNameIsNullForSetColumnWidth()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumnWidthInPixels(null, 1),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<string, double>(excelBook.SetColumnWidthInPixels), "name"
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenColumnNameIsNullForSetColumnWidthAllSheets()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetColumnWidthInPixelsAllSheets(null, 1),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<string, double>(excelBook.SetColumnWidthInPixelsAllSheets), "name"
                )
            );
        }
    }
}