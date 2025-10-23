using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ExcelBookTotalsTest
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
        public void ShouldSetTotalsRow()
        {
            var columns = new List<string> { "Column1", "Column2", "Column3", "Column4", "Column5" };
            excelBook.SetColumns(columns);

            var totals = columns
                .Select(column => new CellValue
                {
                    Value = int.Parse(column.Split("Column").ElementAt(1)),
                    ValueType = typeof(int)
                })
                .Cast<ICellValue>()
                .ToList();

            excelBook.SetTotals(totals);

            var range = excelApplication.Workbooks[0].Worksheets[0].GetRange("A2:E2");

            Assert.AreEqual(1, range["A2"].Number);
            Assert.AreEqual(2, range["B2"].Number);
            Assert.AreEqual(3, range["C2"].Number);
            Assert.AreEqual(4, range["D2"].Number);
            Assert.AreEqual(5, range["E2"].Number);
        }

        [Test]
        public void ShouldThrowNullReferenceExceptionWhenTotalsIsNull()
        {
            Assert.Throws<NullReferenceException>(
                () => excelBook.SetTotals(null),
                ExportsExceptionUtil.CreateNullReferenceExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ICellValue>>(excelBook.SetTotals), "totals"
                )
            );
        }

        [Test]
        public void ShouldThrowEmptyCollectionExceptionWhenTotalsCollectionIsEmpty()
        {
            Assert.Throws<EmptyCollectionException>(
                () => excelBook.SetTotals(new List<ICellValue>()),
                ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(
                    ExceptionUtil.CreateExceptionHeader<IList<ICellValue>>(excelBook.SetTotals), "totals"
                )
            );
        }
    }
}