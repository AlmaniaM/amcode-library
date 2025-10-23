using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.DataSource;
using AMCode.Storage;
using AMCode.Xlsx;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.ExcelBookBuilderCustomDataSourceFactoryTests
{
    [TestFixture]
    public class ExcelBookBuilderCustomDataSourceFactoryTest
    {
        private ExcelBookBuilder excelBookBuilder;
        private Mock<IExportStreamDataSourceFactory> dataSourceFactoryMoq;
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

            dataSourceFactoryMoq = new();
            dataSourceFactoryMoq.Setup(moq => moq.CreateAsync(It.IsAny<Stream>())).Returns(Task.FromResult(new Mock<IStreamDataSourceAsync>().Object));

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
            },
            dataSourceFactoryMoq.Object);
        }

        [Test]
        public async Task ShouldCallCustomDataSourceFactory()
        {
            var bookDataSource = await excelBookBuilder.BuildBookAsync(0, 5, columns);
            dataSourceFactoryMoq.Verify(moq => moq.CreateAsync(It.IsAny<Stream>()), Times.Once());
        }
    }
}