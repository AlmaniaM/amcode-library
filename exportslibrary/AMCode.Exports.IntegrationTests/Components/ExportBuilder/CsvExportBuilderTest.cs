using System;
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
using AMCode.Exports.DataSource;
using AMCode.Exports.ExportBuilder;
using AMCode.Exports.Results;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.IntegrationTests.ExportBuilder.CsvExportBuilderTests
{
    [TestFixture]
    public class CsvExportBuilderTest
    {
        private IEnumerable<ICsvDataColumn> columns;
        private CsvExportBuilder exportBuilder;
        private ExportDataRangeFetch dataRangeFetch;
        private IExportResult exportResult;

        [SetUp]
        public void SetUp()
        {
            var valueFormatterMoq = new Mock<IColumnValueFormatter<object, string>>();
            valueFormatterMoq.Setup(moq => moq.FormatToObject(It.IsAny<object>())).Returns((object obj) => obj.ToString());

            columns = Enumerable.Range(1, 3).Select(
                index => new CsvDataColumn
                {
                    Formatter = valueFormatterMoq.Object,
                    DataFieldName = $"Column{index}",
                    WorksheetHeaderName = $"Column {index}"
                });

            dataRangeFetch = (start, count, _) => Task.Run<IList<ExpandoObject>>(() =>
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
            });

            exportBuilder = new CsvExportBuilder(new BookBuilderConfig
            {
                FetchDataAsync = dataRangeFetch,
                MaxRowsPerDataFetch = 10
            });
        }

        [TearDown]
        public void TearDown() => exportResult?.Dispose();

        [Test]
        public async Task ShouldCreateSingleCsvBook()
        {
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 10, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCancelExport()
        {
            var cancellationSource = new CancellationTokenSource();

            var task = Task.Run(async () =>
            {
                await Task.Run(() => Thread.Sleep(10));
                cancellationSource.Cancel();
            });

            Assert.ThrowsAsync<OperationCanceledException>(() => exportBuilder.CreateExportAsync("TestBook", 10000, columns, cancellationSource.Token));

            await task;
        }

        [Test]
        public async Task ShouldCreateSingleCsvBookWithIExportStreamDataSourceFactoryAndIExportResultFactory()
        {
            exportBuilder = new CsvExportBuilder(new BookBuilderConfig
            {
                FetchDataAsync = dataRangeFetch,
                MaxRowsPerDataFetch = 100
            }, new DataSourceExportResultFactory(), new MemoryStreamDataSourceFactory());

            exportResult = await exportBuilder.CreateExportAsync("TestBook", 10, columns, new CancellationToken());

            await assertResult(exportResult);
        }

        /// <summary>
        /// Assert that the generated <see cref="IExportResult"/> is correct.
        /// </summary>
        /// <param name="exportResult">The <see cref="IExportResult"/> to validate.</param>
        /// <returns>A void <see cref="Task"/>.</returns>
        private async Task assertResult(IExportResult exportResult)
        {
            Assert.AreEqual(1, exportResult.Count);
            Assert.AreEqual(FileType.Csv, exportResult.FileType);

            var actualStream = await exportResult.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", exportResult.Name);
        }
    }
}