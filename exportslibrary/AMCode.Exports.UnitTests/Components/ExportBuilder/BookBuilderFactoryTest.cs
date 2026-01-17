using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.DataSource;
using AMCode.Exports.ExportBuilder;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.Components.ExportBuilder.BookBuilderFactoryTests
{
    [TestFixture]
    public class BookBuilderFactoryTest
    {
        private BookBuilderFactory bookBuilderFactory;
        private readonly BookBuilderConfig bookBuilderConfig = new()
        {
            FetchDataAsync = (int start, int count, CancellationToken cancellationToken) => Task.FromResult<IList<ExpandoObject>>(new List<ExpandoObject>()),
            MaxRowsPerDataFetch = 100,
        };
        private readonly ExcelBookBuilderConfig excelBookBuilderConfig = new()
        {
            ColumnStyles = new List<IColumnStyle>(),
            FetchDataAsync = (int start, int count, CancellationToken cancellationToken) => Task.FromResult<IList<ExpandoObject>>(new List<ExpandoObject>()),
            MaxRowsPerDataFetch = 100,
        };
        private Mock<IExportStreamDataSourceFactory> streamDataSourceFactoryMoq;

        [SetUp]
        public void SetUp() => streamDataSourceFactoryMoq = new();

        [Test]
        public void ShouldCsvCreateBookBuilder()
        {
            bookBuilderFactory = new BookBuilderFactory(bookBuilderConfig, 100);
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Csv);

            Assert.That(bookBuilder, Is.TypeOf<CsvBookBuilder>());
        }

        [Test]
        public void ShouldXlsxCreateBookBuilder()
        {
            bookBuilderFactory = new BookBuilderFactory(bookBuilderConfig, 100);
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Xlsx);

            Assert.That(bookBuilder, Is.TypeOf<ExcelBookBuilder>());
        }

        [Test]
        public void ShouldCsvCreateBookBuilderWithIExportStreamDataSourceFactory()
        {
            bookBuilderFactory = new BookBuilderFactory(bookBuilderConfig, 100, streamDataSourceFactoryMoq.Object);
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Csv);

            Assert.That(bookBuilder, Is.TypeOf<CsvBookBuilder>());
        }

        [Test]
        public void ShouldXlsxCreateBookBuilderWithIExportStreamDataSourceFactory()
        {
            bookBuilderFactory = new BookBuilderFactory(bookBuilderConfig, 100, streamDataSourceFactoryMoq.Object);
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Xlsx);

            Assert.That(bookBuilder, Is.TypeOf<ExcelBookBuilder>());
        }

        [Test]
        public void ShouldCsvCreateBookBuilderWithIExcelBookStyler()
        {
            bookBuilderFactory = new BookBuilderFactory(excelBookBuilderConfig, 100, new SimpleColumnBasedStyler(new List<IExcelBookStyleAction>()));
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Csv);

            Assert.That(bookBuilder, Is.TypeOf<CsvBookBuilder>());
        }

        [Test]
        public void ShouldXlsxCreateBookBuilderWithIExcelBookStyler()
        {
            bookBuilderFactory = new BookBuilderFactory(excelBookBuilderConfig, 100, new SimpleColumnBasedStyler(new List<IExcelBookStyleAction>()));
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Xlsx);

            Assert.That(bookBuilder, Is.TypeOf<ExcelBookBuilder>());
        }

        [Test]
        public void ShouldCsvCreateBookBuilderWithIExcelBookStylerAndIExportStreamDataSourceFactory()
        {
            bookBuilderFactory = new BookBuilderFactory(excelBookBuilderConfig, 100, new SimpleColumnBasedStyler(new List<IExcelBookStyleAction>()), streamDataSourceFactoryMoq.Object);
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Csv);

            Assert.That(bookBuilder, Is.TypeOf<CsvBookBuilder>());
        }

        [Test]
        public void ShouldXlsxCreateBookBuilderWithIExcelBookStylerAndIExportStreamDataSourceFactory()
        {
            bookBuilderFactory = new BookBuilderFactory(excelBookBuilderConfig, 100, new SimpleColumnBasedStyler(new List<IExcelBookStyleAction>()), streamDataSourceFactoryMoq.Object);
            var bookBuilder = bookBuilderFactory.CreateBuilder(FileType.Xlsx);

            Assert.That(bookBuilder, Is.TypeOf<ExcelBookBuilder>());
        }
    }
}