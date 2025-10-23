using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common.Exceptions;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Xlsx;
using AMCode.Xlsx.Common;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.Book.ExcelBookTests
{
    [TestFixture]
    public class ExcelBookAddDataTest
    {
        private List<IExcelDataColumn> defaultColumns;
        private List<ExpandoObject> defaultData;
        private IExcelApplication excelApplication;
        private ExcelBook excelBook;

        [SetUp]
        public void SetUp()
        {
            excelApplication = new ExcelApplication();
            excelBook = new ExcelBook(excelApplication);

            defaultData = Enumerable.Range(0, 10).Select(index =>
                new ExpandoObject()
                    .AddOrUpdatePropertyWithValue("Column1", "1")
                    .AddOrUpdatePropertyWithValue("Column2", 2)
                    .AddOrUpdatePropertyWithValue("Column3", 3.3)
                    .AddOrUpdatePropertyWithValue("Column4", true)
            ).ToList();

            defaultColumns = new List<IExcelDataColumn>
            {
                new ExcelDataColumn { DataType = typeof(string), DataFieldName = "Column1", WorksheetHeaderName = "Column 1" },
                new ExcelDataColumn { DataType = typeof(int), DataFieldName = "Column2", WorksheetHeaderName = "Column 2" },
                new ExcelDataColumn { DataType = typeof(double), DataFieldName = "Column3", WorksheetHeaderName = "Column 3" },
                new ExcelDataColumn { DataType = typeof(bool), DataFieldName = "Column4", WorksheetHeaderName = "Column 4" }
            };
        }

        [TearDown]
        public void TearDown() => excelApplication.Dispose();

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldSetDataWithoutColumns(bool withColumns)
        {
            if (withColumns)
            {
                excelBook.SetColumns(defaultColumns.Select(column => column.WorksheetHeaderName).ToList());
            }

            excelBook.AddData(defaultData, defaultColumns, new CancellationToken());

            var worksheet = excelApplication.Workbooks[0].Worksheets[0];

            var startRow = withColumns ? 2 : 1;
            var endRow = withColumns ? 11 : 10;

            Assert.AreEqual("1", worksheet[startRow, 1].Text);
            Assert.AreEqual("1", worksheet[endRow, 1].Text);
            Assert.AreEqual(2, worksheet[startRow, 2].Number);
            Assert.AreEqual(2, worksheet[endRow, 2].Number);
            Assert.AreEqual(3.3, worksheet[startRow, 3].Number);
            Assert.AreEqual(3.3, worksheet[endRow, 3].Number);
            Assert.AreEqual(true, worksheet[startRow, 4].Boolean);
            Assert.AreEqual(true, worksheet[endRow, 4].Boolean);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ShouldIncrementWorksheetWhenMaxRowsHasBeenReached(bool withColumns)
        {
            var data = Enumerable.Range(0, 30).Select(index => new ExpandoObject().AddOrUpdatePropertyWithValue("Column1", "1")).ToList();

            var columns = new List<IExcelDataColumn>
            {
                new ExcelDataColumn { DataType = typeof(string), WorksheetHeaderName = "Column1" }
            };

            // Limit to 10 rows per sheet
            excelBook.MaxRowsPerSheet = 10;

            if (withColumns)
            {
                excelBook.SetColumns(columns.Select(column => column.WorksheetHeaderName).ToList());
            }

            excelBook.AddData(data, columns, new CancellationToken());

            Assert.AreEqual(withColumns ? 4 : 3, excelApplication.Workbooks[0].Worksheets.Count);
        }

        [Test]
        public void ShouldSetTotalsRowAndInsertDataAfter()
        {
            excelBook.SetColumns(defaultColumns.Select(column => column.WorksheetHeaderName).ToList());

            var totals = defaultColumns
                .Select(column => new CellValue
                {
                    Value = int.Parse(column.WorksheetHeaderName.Split("Column").ElementAt(1)),
                    ValueType = typeof(int)
                })
                .Cast<ICellValue>()
                .ToList();

            excelBook.SetTotals(totals);
            excelBook.AddData(defaultData, defaultColumns, new CancellationToken());

            var worksheet = excelApplication.Workbooks[0].Worksheets.GetWorksheet(0);

            // Verify totals
            Assert.AreEqual(1, worksheet["A2"].Number);
            Assert.AreEqual(2, worksheet["B2"].Number);
            Assert.AreEqual(3, worksheet["C2"].Number);
            Assert.AreEqual(4, worksheet["D2"].Number);

            // Verify data
            var startRow = 3;
            var endRow = 12;
            Assert.AreEqual("1", worksheet[startRow, 1].Text);
            Assert.AreEqual("1", worksheet[endRow, 1].Text);
            Assert.AreEqual(2, worksheet[startRow, 2].Number);
            Assert.AreEqual(2, worksheet[endRow, 2].Number);
            Assert.AreEqual(3.3, worksheet[startRow, 3].Number);
            Assert.AreEqual(3.3, worksheet[endRow, 3].Number);
            Assert.AreEqual(true, worksheet[startRow, 4].Boolean);
            Assert.AreEqual(true, worksheet[endRow, 4].Boolean);
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenDataIsNull()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.AddData(null, new List<IExcelDataColumn>(), new CancellationToken()),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<IExcelDataColumn>, CancellationToken>(excelBook.AddData), "dataList"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenDataIsEmpty()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.AddData(new List<ExpandoObject>(), new List<IExcelDataColumn>(), new CancellationToken()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<IExcelDataColumn>, CancellationToken>(excelBook.AddData), "dataList"
                )
            );
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenColumnsIsNull()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.AddData(new List<ExpandoObject> { new ExpandoObject() }, null, new CancellationToken()),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<IExcelDataColumn>, CancellationToken>(excelBook.AddData), "columns"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenColumnsIsEmpty()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.AddData(new List<ExpandoObject> { new ExpandoObject() }, new List<IExcelDataColumn>(), new CancellationToken()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<IExcelDataColumn>, CancellationToken>(excelBook.AddData), "columns"
                )
            );
        }

        [Test]
        public async Task ShouldCancelAddDataOperation()
        {
            var cancellationSource = new CancellationTokenSource();

            var data = Enumerable.Range(0, 20000).Select(index => new ExpandoObject().AddOrUpdatePropertyWithValue("Column1", "1")).ToList();

            var columns = new List<IExcelDataColumn>
            {
                new ExcelDataColumn { DataType = typeof(string), DataFieldName = "Column1", WorksheetHeaderName = "Column 1" }
            };

            var task = Task.Run(() =>
            {
                Thread.Sleep(5);
                cancellationSource.Cancel();
            });

            Assert.Throws<OperationCanceledException>(() => excelBook.AddData(data, columns, cancellationSource.Token));

            await task;
        }
    }
}