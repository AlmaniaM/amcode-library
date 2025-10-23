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
using AMCode.Xlsx;
using AMCode.Xlsx.Drawing;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.IntegrationTests.BookBuilder.BookCompilerTests
{
    [TestFixture]
    public class BookCompilerExcelTest
    {
        private ExcelBookBuilder bookBuilder;
        private ExcelBookBuilderConfig bookConfig;
        private Mock<IBookBuilderFactory> bookFactoryMoq;
        private IEnumerable<IExcelDataColumn> columns;
        private IExcelBookFactory excelBookFactory;
        private readonly SimpleColumnBasedStyler styler = new(new List<IExcelBookStyleAction>
            {
                new ApplyColumnStylesAction(),
                new ApplyColumnWidthAction(),
                new ApplyBoldHeadersAction(),
            });

        [SetUp]
        public void SetUp()
        {
            columns = Enumerable.Range(1, 3).Select(
                index => new ExcelDataColumn
                {
                    DataFieldName = $"Column{index}",
                    DataType = typeof(string),
                    WorksheetHeaderName = $"Column {index}"
                });

            bookConfig = new()
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
            };

            excelBookFactory = new ExcelBookFactory(10);

            bookBuilder = new ExcelBookBuilder(excelBookFactory, bookConfig);

            ExcelBookBuilder getBuilder() => bookBuilder;

            bookFactoryMoq = new Mock<IBookBuilderFactory>();
            bookFactoryMoq.Setup(moq => moq.CreateBuilder(It.IsAny<FileType>())).Returns(() => getBuilder());
        }

        [Test]
        public async Task ShouldCreateSingleExcelBook()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
            var result = await bookCompiler.CompileExcelAsync("TestBook", 10, columns, new CancellationToken());

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(FileType.Xlsx, result.FileType);

            var actualStream = await result.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", result.Name);
        }

        [Test]
        public async Task ShouldCreateZipOfExcelBooks()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
            var result = await bookCompiler.CompileExcelAsync("TestBook", 100, columns, new CancellationToken());

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

            bookConfig.ColumnStyles = columns.Select(
                    column => new ColumnStyle
                    {
                        Name = column.WorksheetHeaderName,
                        Width = columnWidth,
                        Style = new ColumnStyleParam
                        {
                            Color = Color.Green,
                            BorderStyles = new()
                            {
                                [ExcelBordersIndex.EdgeLeft] = new() { Color = Color.Black, LineStyle = ExcelLineStyle.Thick },
                                [ExcelBordersIndex.EdgeBottom] = new() { Color = Color.Red, LineStyle = ExcelLineStyle.Thick }
                            }
                        }
                    } as IColumnStyle).ToList();

            bookBuilder = new ExcelBookBuilder(excelBookFactory, bookConfig, styler);
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
            var result = await bookCompiler.CompileExcelAsync("TestBook", 9, columns);

            using var excelApplication = new ExcelApplication();
            var workbook = excelApplication.Workbooks.Open(await result.GetDataAsync());
            var worksheet = workbook.Worksheets.GetWorksheet(0);

            var columnsRange = worksheet.GetRange("A1:C1");
            Assert.IsTrue(columnsRange.CellStyle.Font.Bold);

            var dataRange = worksheet.GetRange("A2:C2");
            Assert.That(dataRange.CellStyle.Borders[ExcelBordersIndex.EdgeLeft].Color, Is.EqualTo(Color.Black));
            Assert.That(dataRange.CellStyle.Borders[ExcelBordersIndex.EdgeLeft].LineStyle, Is.EqualTo(ExcelLineStyle.Thick));
            Assert.That(dataRange.CellStyle.Borders[ExcelBordersIndex.EdgeBottom].Color, Is.EqualTo(Color.Red));
            Assert.That(dataRange.CellStyle.Borders[ExcelBordersIndex.EdgeBottom].LineStyle, Is.EqualTo(ExcelLineStyle.Thick));

            Assert.AreEqual(columnWidth, worksheet.GetColumnWidthInPixels(1));
            Assert.AreEqual(columnWidth, worksheet.GetColumnWidthInPixels(2));
            Assert.AreEqual(columnWidth, worksheet.GetColumnWidthInPixels(3));

            Assert.IsTrue(Color.Green == worksheet.GetRange(1, 3, 11, 3).CellStyle.Color);
        }
    }
}