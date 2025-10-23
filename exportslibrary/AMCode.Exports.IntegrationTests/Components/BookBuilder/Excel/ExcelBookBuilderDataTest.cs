using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Xlsx;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.IntegrationTests.BookBuilder.ExcelBookBuilderTests
{
    [TestFixture]
    public class ExcelBookBuilderDataTest
    {
        private ExcelBookBuilder excelBookBuilder;
        private Mock<IColumnValueFormatter<object, string>> formatterMoq;
        private IList<IExcelDataColumn> columns;
        private readonly int columnCount = 10;

        [SetUp]
        public void SetUp()
        {
            formatterMoq = new();
            formatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns<string>(value => value.ToString());

            columns = Enumerable.Range(1, columnCount).Select(index => new ExcelDataColumn
            {
                DataFieldName = $"Column{index}",
                WorksheetHeaderName = $"Column {index}",
                DataType = typeof(string)
            }).ToList<IExcelDataColumn>();

            var excelBookFactoryMoq = new Mock<IExcelBookFactory>();
            excelBookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => new ExcelBook(new ExcelApplication()));

            excelBookBuilder = new(excelBookFactoryMoq.Object, new ExcelBookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.Run<IList<ExpandoObject>>(() =>
                {
                    var data = Enumerable.Range(start, count)
                    .Select(index =>
                    {
                        var row = new ExpandoObject();

                        columns.ForEach((column, i) => row.AddOrUpdatePropertyWithValue(column.DataFieldName, $"Value {i + 1}{index}"));

                        return row;
                    }).ToList();

                    return data;
                }),
                MaxRowsPerDataFetch = 10
            });
        }

        [Test]
        public async Task ShouldCreateExcelBook()
        {
            var rowCount = 500000;
            using var bookResult = await excelBookBuilder.BuildBookAsync(0, rowCount, columns);
            var bookStream = await bookResult.GetStreamAsync();

            bookStream.Seek(0, SeekOrigin.Begin);
            using var excelApplication = new ExcelApplication();
            var workbook = excelApplication.Workbooks.Open(bookStream);
            var worksheet = workbook.Worksheets.GetWorksheet(0);

            Assert.AreEqual((columnCount * rowCount) + columnCount, worksheet.Cells.Length);
            Assert.AreEqual($"Value 10", worksheet.GetRange(2, 1).ObjectValue);

            Assert.AreEqual($"Value {columnCount}{rowCount - 1}", worksheet.GetRange(rowCount + 1, columnCount).ObjectValue);
        }
    }
}