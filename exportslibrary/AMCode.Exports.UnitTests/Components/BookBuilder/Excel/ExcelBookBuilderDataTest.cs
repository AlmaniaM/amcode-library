using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Documents.Xlsx;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.ExcelBookBuilderTests
{
    [TestFixture]
    public class ExcelBookBuilderDataTest
    {
        private ExcelBookBuilder excelBookBuilder;
        private IList<IExcelDataColumn> columns;
        private readonly int columnCount = 2;

        [SetUp]
        public void SetUp()
        {
            columns = Enumerable.Range(1, columnCount).Select(index => new ExcelDataColumn
            {
                DataFieldName = $"Column{index}",
                WorksheetHeaderName = $"Column {index}",
                DataType = typeof(string)
            }).ToList<IExcelDataColumn>();

            var excelBookFactoryMoq = new Mock<IExcelBookFactory>();
            excelBookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => new ExcelBook(new ExcelApplication()));

            excelBookBuilder = new ExcelBookBuilder(excelBookFactoryMoq.Object, new ExcelBookBuilderConfig
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
                MaxRowsPerDataFetch = 10,
            });
        }

        [TestCase(0, 10)]
        [TestCase(10, 20)]
        [TestCase(100, 101)]
        [TestCase(100, 1)]
        public async Task ShouldCreateExcelBook(int start, int count)
        {
            var bookDataSource = await excelBookBuilder.BuildBookAsync(start, count, columns);
            var bookStream = await bookDataSource.GetStreamAsync();

            bookStream.Seek(0, SeekOrigin.Begin);
            using var excelApplication = new ExcelApplication();
            var workbook = excelApplication.Workbooks.Open(bookStream);
            var worksheet = workbook.Worksheets.GetWorksheet(0);

            Assert.AreEqual((columnCount * count) + columnCount, worksheet.Cells.Length);
            Assert.AreEqual($"Value 1{start}", worksheet.GetRange(2, 1).ObjectValue);

            Assert.AreEqual($"Value 2{start + (count - 1)}", worksheet.GetRange(count + 1, 2).ObjectValue);
        }
    }
}