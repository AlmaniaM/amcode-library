using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO.CSV;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.IntegrationTests.BookBuilder.CsvBookBuilderTests
{
    [TestFixture]
    public class CsvBookBuilderDataTest
    {
        private CsvBookBuilder csvBookBuilder;
        private Mock<IColumnValueFormatter<object, string>> formatterMoq;
        private IList<ICsvDataColumn> columns;
        private readonly int columnCount = 10;

        [SetUp]
        public void SetUp()
        {
            formatterMoq = new();
            formatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns<string>(value => value.ToString());

            columns = Enumerable.Range(1, columnCount).Select(index => new CsvDataColumn
            {
                Formatter = formatterMoq.Object,
                DataFieldName = $"Column{index}",
                WorksheetHeaderName = $"Column {index}"
            }).ToList<ICsvDataColumn>();

            var csvBookFactoryMoq = new Mock<ICsvBookFactory>();
            csvBookFactoryMoq.Setup(moq => moq.CreateBook()).Returns(() => new CsvBook());

            csvBookBuilder = new CsvBookBuilder(csvBookFactoryMoq.Object, new BookBuilderConfig
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
        public async Task ShouldCrateLargeCsvBook()
        {
            var rowCount = 500000;
            using var bookResult = await csvBookBuilder.BuildBookAsync(0, rowCount, columns);
            var book = await bookResult.GetStreamAsync();

            var csvData = new CSVReader().GetExpandoList(book);

            Assert.AreEqual("Value 10", csvData[0].GetValue<string>("Column 1"));
            Assert.AreEqual($"Value {columnCount}{rowCount - 1}", csvData[rowCount - 1].GetValue<string>($"Column {columnCount}"));
        }
    }
}