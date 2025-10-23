using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.DataSource;
using AMCode.Storage;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.CsvBookBuilderTests
{
    [TestFixture]
    public class CsvBookBuilderCustomStreamDataSourceTest
    {
        private CsvBookBuilder csvBookBuilder;
        private Mock<IExportStreamDataSourceFactory> dataSourceFactoryMoq;
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

            dataSourceFactoryMoq = new();
            dataSourceFactoryMoq.Setup(moq => moq.CreateAsync(It.IsAny<Stream>())).Returns(Task.FromResult(new Mock<IStreamDataSourceAsync>().Object));

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
            },
            dataSourceFactoryMoq.Object);
        }

        [Test]
        public async Task ShouldCallCustomDataSourceFactory()
        {
            var bookDataSource = await csvBookBuilder.BuildBookAsync(0, 5, columns);
            dataSourceFactoryMoq.Verify(moq => moq.CreateAsync(It.IsAny<Stream>()), Times.Once());
        }
    }
}