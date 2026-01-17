using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Strings;
using AMCode.Common.IO.Zip;
using AMCode.Common.IO.Zip.Models;
using AMCode.Exports;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.Results;
using AMCode.Exports.Zip;
using AMCode.Storage;
using AMCode.Exports.UnitTests.BookBuilder.Mocks;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.BookBuilder.BookCompilerTests
{
    [TestFixture]
    public class BookCompilerGenericTypeTest
    {
        private Mock<IBookBuilder> bookBuilderMoq;
        private Mock<IBookBuilderFactory> bookFactoryMoq;
        private IEnumerable<TestDataColumn> columns;
        private IExportResult exportResult;
        private Mock<IExportResultFactory> exportResultFactoryMoq;
        private Mock<IZipArchive> zipArchiveMoq;
        private Mock<IZipArchiveFactory> zipArchiveFactoryMoq;

        [SetUp]
        public void SetUp()
        {
            var streamDataSourceMoq = new Mock<IStreamDataSourceAsync>();
            streamDataSourceMoq.Setup(moq => moq.GetStreamAsync()).Returns(() => Task.FromResult("Test Value".ToStream()));

            bookBuilderMoq = new Mock<IBookBuilder>();
            bookBuilderMoq.Setup(
                moq => moq.BuildBookAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<IBookDataColumn>>(), It.IsAny<CancellationToken>())
            ).Returns(() => Task.FromResult(streamDataSourceMoq.Object));

            bookFactoryMoq = new Mock<IBookBuilderFactory>();
            bookFactoryMoq.Setup(moq => moq.CreateBuilder(It.IsAny<FileType>())).Returns(() => bookBuilderMoq.Object);

            columns = Enumerable.Range(1, 1).Select(index => new TestDataColumn { WorksheetHeaderName = $"Column{index}" });

            zipArchiveMoq = new();
            zipArchiveMoq.SetupGet(moq => moq.ZipEntries).Returns(() => new List<IZipEntry> { null, null });
            zipArchiveMoq.Setup(moq => moq.CreateZipAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(
                (string name, CancellationToken cancellationToken) => Task.FromResult<IZipArchiveResult>(new ZipArchiveResult
                {
                    Data = "Zip File".ToStream(),
                    Name = name
                })
            );

            zipArchiveFactoryMoq = new();
            zipArchiveFactoryMoq.Setup(moq => moq.Create(It.IsAny<IEnumerable<IZipEntry>>())).Returns(() => zipArchiveMoq.Object);

            exportResultFactoryMoq = new();
            exportResultFactoryMoq.Setup(moq => moq.CreateAsync(It.IsAny<IStreamDataSourceAsync>(), It.IsAny<FileType>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() => Task.FromResult<IExportResult>(new InMemoryExportResult()));
            exportResultFactoryMoq.Setup(moq => moq.CreateAsync(It.IsAny<Stream>(), It.IsAny<FileType>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(() => Task.FromResult<IExportResult>(new InMemoryExportResult()));
        }

        [TearDown]
        public void Teardown() => exportResult?.Dispose();

        [Test]
        public async Task ShouldCreateSingleExcelBook()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
            exportResult = await bookCompiler.CompileBookAsync("TestBook", 10, columns, FileType.Csv, new CancellationToken());

            Assert.AreEqual(FileType.Csv, exportResult.FileType);

            var actualStream = await exportResult.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", exportResult.Name);
        }

        [Test]
        public async Task ShouldCreateZipOfExcelBooks()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10, zipArchiveFactoryMoq.Object);
            exportResult = await bookCompiler.CompileBookAsync("TestBook", 100, columns, FileType.Xlsx, new CancellationToken());

            Assert.AreEqual(2, exportResult.Count);
            Assert.AreEqual(FileType.Zip, exportResult.FileType);

            var actualStream = await exportResult.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", exportResult.Name);

            zipArchiveMoq.Verify(moq => moq.CreateZipAsync(It.Is<string>(fileName => fileName.Equals("TestBook")), It.IsAny<CancellationToken>()), Times.Once());
        }

        [TestCase(91, 10, 10)]
        [TestCase(90, 10, 9)]
        [TestCase(9, 10, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 1000000, 1)]
        [TestCase(1000000, 1000000, 1)]
        [TestCase(1000000, 1000001, 1)]
        public void ShouldCalculateNumberOfBooks(int numberOfRows, int maxRowsPerBook, int expectedNumberOfBooks)
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, maxRowsPerBook, zipArchiveFactoryMoq.Object);
            Assert.That(bookCompiler.CalculateNumberOfBooks(numberOfRows), Is.EqualTo(expectedNumberOfBooks));
        }

        [Test]
        public async Task ShouldCallCustomExportResultFactoryCreateAsyncForStreamFunction()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10, zipArchiveFactoryMoq.Object, exportResultFactoryMoq.Object);
            exportResult = await bookCompiler.CompileBookAsync("TestBook", 100, columns, FileType.Xlsx, CancellationToken.None);

            exportResultFactoryMoq.Verify(moq => moq.CreateAsync(It.IsAny<Stream>(), It.IsAny<FileType>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestCase(FileType.Xlsx)]
        [TestCase(FileType.Csv)]
        public async Task ShouldCallCustomExportResultFactoryCreateAsyncForIStreamDataSourceAsyncFunction(FileType fileType)
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 100, zipArchiveFactoryMoq.Object, exportResultFactoryMoq.Object);
            exportResult = await bookCompiler.CompileBookAsync("TestBook", 10, columns, fileType, CancellationToken.None);

            exportResultFactoryMoq.Verify(moq => moq.CreateAsync(It.IsAny<IStreamDataSourceAsync>(), It.IsAny<FileType>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }
    }
}