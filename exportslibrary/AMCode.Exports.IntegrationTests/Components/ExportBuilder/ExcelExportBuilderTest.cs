using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.BookBuilder.Actions;
using AMCode.Exports.DataSource;
using AMCode.Exports.ExportBuilder;
using AMCode.Exports.Results;
using AMCode.Documents.Xlsx;
using AMCode.Documents.Xlsx.Drawing;
using NUnit.Framework;

namespace AMCode.Exports.IntegrationTests.ExportBuilder.ExcelExportBuilderTests
{
    [TestFixture]
    public class ExcelExportBuilderTest
    {
        private ExcelBookBuilderConfig builderConfig;
        private IEnumerable<IExcelDataColumn> columns;
        private readonly IList<IExcelBookStyleAction> stylers = new List<IExcelBookStyleAction>
            {
                new ApplyColumnStylesAction(),
                new ApplyColumnWidthAction(),
                new ApplyBoldHeadersAction(),
            };
        private ExcelExportBuilder exportBuilder;
        private ExportDataRangeFetch dataRangeFetch;
        private IExportResult exportResult;

        [SetUp]
        public void SetUp()
        {
            columns = Enumerable.Range(1, 3).Select(
                index => new ExcelDataColumn
                {
                    DataType = typeof(string),
                    WorksheetHeaderName = $"Column{index}"
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

            builderConfig = new ExcelBookBuilderConfig
            {
                FetchDataAsync = dataRangeFetch,
                MaxRowsPerDataFetch = 10
            };

            exportBuilder = new ExcelExportBuilder(builderConfig, stylers, 100);
        }

        [TearDown]
        public void TearDown() => exportResult?.Dispose();

        [Test]
        public async Task ShouldCreateSingleExcelBook()
        {
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 99, columns, new CancellationToken());

            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateZipOfExcelBooks()
        {
            var result = await exportBuilder.CreateExportAsync("TestBook", 999, columns, new CancellationToken());

            Assert.AreEqual(10, result.Count);
            Assert.AreEqual(FileType.Zip, result.FileType);

            var actualStream = await result.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", result.Name);
        }

        [Test]
        public async Task ShouldCreateExcelFileUsingStylers()
        {
            var columnWidth = 180;

            builderConfig.ColumnStyles = columns.Select(
                    column => new ColumnStyle
                    {
                        Name = column.WorksheetHeaderName,
                        Width = columnWidth,
                        Style = new ColumnStyleParam
                        {
                            Color = Color.Green
                        }
                    } as IColumnStyle).ToList();

            var result = await exportBuilder.CreateExportAsync("TestBook", 99, columns, new CancellationToken());

            using var excelApplication = new ExcelApplication();
            var workbook = excelApplication.Workbooks.Open(await result.GetDataAsync());
            var worksheet = workbook.Worksheets.GetWorksheet(0);

            var columnsRange = worksheet.GetRange("A1:C1");
            Assert.IsTrue(columnsRange.CellStyle.Font.Bold);

            Assert.AreEqual(columnWidth, worksheet.GetColumnWidthInPixels(1));
            Assert.AreEqual(columnWidth, worksheet.GetColumnWidthInPixels(2));
            Assert.AreEqual(columnWidth, worksheet.GetColumnWidthInPixels(3));

            Assert.IsTrue(Color.Green == worksheet.GetRange(1, 3, 11, 3).CellStyle.Color);
        }

        [Test]
        public async Task ShouldCancelExport()
        {
            var cancellationSource = new CancellationTokenSource();

            var task = Task.Run(() =>
            {
                Thread.Sleep(100);
                cancellationSource.Cancel();
            });

            Assert.ThrowsAsync<OperationCanceledException>(() => exportBuilder.CreateExportAsync("TestBook", 10000, columns, cancellationSource.Token));

            await task;
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookBase()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig);
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookIExportResultFactoryIExportStreamDataSourceFactory()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, new DataSourceExportResultFactory(), new MemoryStreamDataSourceFactory());
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookIExportResultFactoryIExportStreamDataSourceFactoryAsNulls()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, null, null);
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookBaseWithMaxRows()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, 10);
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookBaseWithMaxRowsAndIExportResultFactoryIExportStreamDataSourceFactory()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, 10, new DataSourceExportResultFactory(), new MemoryStreamDataSourceFactory());
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookWithIExportResultFactoryIExportStreamDataSourceFactoryAsNulls()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, 10, null, null);
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookLargeConstructorWithStylerList()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, new List<IExcelBookStyleAction>(), 10, new DataSourceExportResultFactory(), new MemoryStreamDataSourceFactory());
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookLargeConstructorWithStylerListAndNullStorage()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, new List<IExcelBookStyleAction>(), 10, null, null);
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookLargeConstructorWithIExcelBookStyler()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, new SimpleColumnBasedStyler(new List<IExcelBookStyleAction>()), 10, new DataSourceExportResultFactory(), new MemoryStreamDataSourceFactory());
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
            await assertResult(exportResult);
        }

        [Test]
        public async Task ShouldCreateSingleExcelBookLargeConstructorWithIExcelBookStylerAndNullStorage()
        {
            exportBuilder = new ExcelExportBuilder(builderConfig, new SimpleColumnBasedStyler(new List<IExcelBookStyleAction>()), 10, null, null);
            exportResult = await exportBuilder.CreateExportAsync("TestBook", 5, columns, new CancellationToken());
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
            Assert.AreEqual(FileType.Xlsx, exportResult.FileType);

            var actualStream = await exportResult.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", exportResult.Name);
        }
    }
}