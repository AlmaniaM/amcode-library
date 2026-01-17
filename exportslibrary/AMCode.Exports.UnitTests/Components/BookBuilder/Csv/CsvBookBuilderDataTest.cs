using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO.CSV;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.CsvBookBuilderTests
{
    [TestFixture]
    public class CsvBookBuilderDataTest
    {
        private CsvBookBuilder csvBookBuilder;
        private Mock<IColumnValueFormatter<object, string>> formatterMoq;
        private IList<ICsvDataColumn> columns;

        [SetUp]
        public void SetUp()
        {
            formatterMoq = new();
            formatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns<string>(value => value.ToString());

            columns = new List<ICsvDataColumn>
            {
                new CsvDataColumn
                {
                    DataFieldName = "Column1",
                    WorksheetHeaderName = "Column 1",
                    Formatter = formatterMoq.Object,
                },
                new CsvDataColumn
                {
                    DataFieldName = "Column2",
                    WorksheetHeaderName = "Column 2",
                    Formatter = formatterMoq.Object,
                }
            };

            var csvBookFactoryMoq = new Mock<ICsvBookFactory>();
            csvBookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => new CsvBook());

            csvBookBuilder = new CsvBookBuilder(csvBookFactoryMoq.Object, new BookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.Run<IList<ExpandoObject>>(() =>
                {
                    var data = Enumerable.Range(start, count)
                    .Select(index =>
                    {
                        var row = new ExpandoObject()
                        .AddOrUpdatePropertyWithValue("Column1", $"Value 1{index}")
                        .AddOrUpdatePropertyWithValue("Column2", $"Value 2{index}");

                        return row;
                    }).ToList();

                    return data;
                }),
                MaxRowsPerDataFetch = 10
            });
        }

        [TestCase(0, 10)]
        [TestCase(10, 20)]
        [TestCase(100, 101)]
        [TestCase(100, 1)]
        public async Task ShouldCreateCsvBook(int start, int count)
        {
            using var bookResult = await csvBookBuilder.BuildBookAsync(start, count, columns);
            var book = await bookResult.GetStreamAsync();

            var csvData = new CSVReader().GetExpandoList(book);

            Assert.AreEqual(count, csvData.Count);
            Assert.AreEqual($"Value 1{start}", csvData[0].GetValue<string>("Column 1"));
            Assert.AreEqual($"Value 2{start + (count - 1)}", csvData[count - 1].GetValue<string>("Column 2"));
        }
    }
}