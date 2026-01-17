using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Exports;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.BookBuilder.Actions;
using AMCode.Exports.Extensions;
using AMCode.Exports.Results;
using AMCode.Storage;
using AMCode.Exports.SharedTestLibrary.Global;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.IntegrationTests.BookBuilder.BookCompilerTests
{
    [TestFixture]
    public class BookCompilerExcelFileStorageTest
    {
        private ExcelBookBuilder bookBuilder;
        private ExcelBookBuilderConfig bookConfig;
        private Mock<IBookBuilderFactory> bookFactoryMoq;
        private Mock<IExportResultFactory> exportResultFactoryMoq;
        private IExportResult exportResult;
        private IEnumerable<IExcelDataColumn> columns;
        private IExcelBookFactory excelBookFactory;
        private readonly SimpleColumnBasedStyler styler = new(new List<IExcelBookStyleAction>
            {
                new ApplyColumnStylesAction(),
                new ApplyColumnWidthAction(),
                new ApplyBoldHeadersAction(),
            });
        private TestHelper testHelper;

        [SetUp]
        public void SetUp()
        {
            testHelper = new();

            columns = Enumerable.Range(1, 1).Select(
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
                        .AddOrUpdatePropertyWithValue("Column1", $"Value 1{index}");

                        return row;
                    }).ToList();

                    return data;
                }),
                MaxRowsPerDataFetch = 10
            };

            exportResultFactoryMoq = new();
            exportResultFactoryMoq.Setup(moq => moq.CreateAsync(It.IsAny<IStreamDataSourceAsync>(), It.IsAny<FileType>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns<IStreamDataSourceAsync, FileType, string, int>(async (data, fileType, name, count) =>
                {
                    var result = new ExportBookResultFileStorage(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext))
                    {
                        Count = count,
                        FileType = fileType,
                        Name = name,
                    };

                    await result.SetDataAsync(await data.GetStreamAsync());

                    return result;
                });
            exportResultFactoryMoq.Setup(moq => moq.CreateAsync(It.IsAny<Stream>(), It.IsAny<FileType>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns<Stream, FileType, string, int>(async (data, fileType, name, count) =>
                {
                    var result = new ExportBookResultFileStorage(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext))
                    {
                        Count = count,
                        FileType = fileType,
                        Name = name,
                    };

                    await result.SetDataAsync(data);

                    return result;
                });

            excelBookFactory = new ExcelBookFactory(10);

            bookBuilder = new ExcelBookBuilder(excelBookFactory, bookConfig);

            ExcelBookBuilder getBuilder() => bookBuilder;

            bookFactoryMoq = new Mock<IBookBuilderFactory>();
            bookFactoryMoq.Setup(moq => moq.CreateBuilder(It.IsAny<FileType>())).Returns(() => getBuilder());
        }

        [TearDown]
        public void TearDown()
        {
            getFiles("xlsx").ForEach(file => File.Delete(file));
            getFiles("zip").ForEach(file => File.Delete(file));

            exportResult?.Dispose();
        }

        [Test]
        public async Task ShouldCreateSingleExcelFile()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10, exportResultFactoryMoq.Object);
            exportResult = await bookCompiler.CompileExcelAsync("TestBook", 100, columns, new CancellationToken());

            var filePath = Path.Combine(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), exportResult.CreateFileName());

            Assert.That(File.Exists(filePath), Is.True);
        }

        [Test]
        public async Task ShouldCreateZipOfExcelBooksAsAFile()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10, exportResultFactoryMoq.Object);
            exportResult = await bookCompiler.CompileExcelAsync("TestBook", 100, columns, new CancellationToken());

            var files = getFiles("xlsx");
            Assert.That(files.Count, Is.EqualTo(0));

            var zipFiles = getFiles("zip");
            Assert.That(zipFiles.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ShouldCreateZipOfExcelBooksAsAFileAndFileExtensionsShouldBeXlsx()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10, exportResultFactoryMoq.Object);
            exportResult = await bookCompiler.CompileExcelAsync("TestBook", 100, columns, new CancellationToken());

            var zipFiles = getFiles("zip");
            Assert.That(zipFiles.Count, Is.EqualTo(1));

            using var fileStream = new FileStream(zipFiles[0], FileMode.Open, FileAccess.Read);
            using var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read, false);

            zipArchive.Entries.ForEach(entry => Assert.That(Path.GetExtension(entry.Name).ToLower(), Is.EqualTo(".xlsx")));
        }

        /// <summary>
        /// Get a list of files from the test work directory with the provided file extension name.
        /// </summary>
        /// <param name="extension">The file extension name without the period.</param>
        /// <returns>A collection of file paths.</returns>
        private List<string> getFiles(string extension) => Directory.GetFiles(testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext), $"*.{extension}").ToList();
    }
}