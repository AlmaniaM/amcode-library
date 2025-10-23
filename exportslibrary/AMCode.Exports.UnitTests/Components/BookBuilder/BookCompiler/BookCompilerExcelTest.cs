using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Strings;
using AMCode.Common.IO.Zip;
using AMCode.Common.IO.Zip.Models;
using AMCode.Exports;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.Zip;
using AMCode.Storage;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.BookBuilder.BookCompilerTests
{
    [TestFixture]
    public class BookCompilerExcelTest
    {
        private Mock<IBookBuilder> bookBuilderMoq;
        private Mock<IBookBuilderFactory> bookFactoryMoq;
        private IEnumerable<IExcelDataColumn> columns;
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

            columns = Enumerable.Range(1, 1).Select(index => new ExcelDataColumn { WorksheetHeaderName = $"Column{index}" });

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
        }

        [Test]
        public async Task ShouldCreateSingleExcelBook()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10);
            var result = await bookCompiler.CompileExcelAsync("TestBook", 10, columns, new CancellationToken());

            Assert.AreEqual(FileType.Xlsx, result.FileType);

            var actualStream = await result.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", result.Name);
        }

        [Test]
        public async Task ShouldCreateZipOfExcelBooks()
        {
            var bookCompiler = new BookCompiler(bookFactoryMoq.Object, 10, zipArchiveFactoryMoq.Object);
            var result = await bookCompiler.CompileExcelAsync("TestBook", 100, columns, new CancellationToken());

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(FileType.Zip, result.FileType);

            var actualStream = await result.GetDataAsync();
            Assert.That(actualStream.Length, Is.GreaterThan(0));
            Assert.AreEqual("TestBook", result.Name);

            zipArchiveMoq.Verify(moq => moq.CreateZipAsync(It.Is<string>(fileName => fileName.Equals("TestBook")), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}