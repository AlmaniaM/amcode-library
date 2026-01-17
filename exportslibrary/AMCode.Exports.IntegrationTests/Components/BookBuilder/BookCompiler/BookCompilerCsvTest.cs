using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Columns.Core;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.IntegrationTests.BookBuilder.BookCompilerTests
{
    [TestFixture]
    public class BookCompilerCsvTest
    {
        private Mock<IBookBuilderFactory> bookFactoryMoq;
        private IEnumerable<ICsvDataColumn> columns;
        private IExportResult exportResult;

        [SetUp]
        public void SetUp()
        {
            var csvBookBuilder = new CsvBookBuilder(new CsvBookFactory(), new BookBuilderConfig
            {
                FetchDataAsync = (start, count, _) => Task.Run<IList<ExpandoObject>>(() =>
                {
                    var data = Enumerable.Range(start, count)
                    .Select(index =>
                    {
                        var row = new ExpandoObject()
                        .AddOrUpdatePropertyWithValue("Column1", $"Value 1{index}")
                        .AddOrUpdatePropertyWithValue("Column2", $"Value 2{index}")
                        .AddOrUpdatePropertyWithValue("Column3", $"Value 2{index}");

                        return row;
                    }).ToList();

                    return data;
                }),
                MaxRowsPerDataFetch = 10
            });

            var valueFormatterMoq = new Mock<IColumnValueFormatter<object, string>>();
            valueFormatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns((object obj) => obj.ToString());

            bookFactoryMoq = new Mock<IBookBuilderFactory>();
            bookFactoryMoq.Setup(moq => moq.CreateBuilder(It.IsAny<FileType>())).Returns(() => csvBookBuilder);

            columns = Enumerable.Range(1, 3).Select(
                index => new CsvDataColumn
                {
                    Formatter = valueFormatterMoq.Object,
                    DataFieldName = $"Column{index}",
                    WorksheetHeaderName = $"Column {index}"
                });
        }

        [TearDown]
        public void TearDown() => exportResult?.Dispose();

        [Test]
        public async Task ShouldCreateSingleCsvBook()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
            exportResult = await bookCompiler.CompileCsvAsync("TestBook", 10, columns, new CancellationToken());

            Assert.AreEqual(1, exportResult.Count);
            Assert.AreEqual(FileType.Csv, exportResult.FileType);

            var actualStream = await exportResult.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", exportResult.Name);
        }

        [Test]
        public async Task ShouldCreateZipOfCsvBooks()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
            exportResult = await bookCompiler.CompileCsvAsync("TestBook", 100, columns, new CancellationToken());

            Assert.AreEqual(10, exportResult.Count);
            Assert.AreEqual(FileType.Zip, exportResult.FileType);

            var actualStream = await exportResult.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", exportResult.Name);
        }
    }
}