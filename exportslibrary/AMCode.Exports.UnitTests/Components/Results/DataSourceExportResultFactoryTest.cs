using System.IO;
using System.Text;
using System.Threading.Tasks;
using AMCode.Exports;
using AMCode.Exports.DataSource;
using AMCode.Exports.Results;
using AMCode.Storage;
using AMCode.Exports.SharedTestLibrary.Extensions;
using Moq;
using NUnit.Framework;

namespace AMCode.Exports.UnitTests.Results.DataSourceExportResultFactoryTests
{
    [TestFixture]
    public class DataSourceExportResultFactoryTest
    {
        private Mock<IExportStreamDataSourceFactory> dataSourceFactoryMoq;
        private DataSourceExportResultFactory exportResultFactory;
        private Mock<IStreamDataSourceAsync> dataSourceAsyncMoq;
        private Stream stream;

        [SetUp]
        public void SetUp()
        {
            stream = new MemoryStream();
            stream.Write(Encoding.UTF8.GetBytes("Test value"));
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            dataSourceAsyncMoq = new();
            dataSourceAsyncMoq.Setup(moq => moq.GetStreamAsync()).Returns(() => Task.FromResult(stream));

            dataSourceFactoryMoq = new();
            dataSourceFactoryMoq.Setup(moq => moq.CreateAsync(It.IsAny<Stream>())).Returns(() => Task.FromResult(dataSourceAsyncMoq.Object));

            exportResultFactory = new DataSourceExportResultFactory();
        }

        [TearDown]
        public void TearDown() => stream.Dispose();

        [Test]
        public async Task ShouldGetTypeOfDataSourceExportResult()
        {
            var exportResult = await exportResultFactory.CreateAsync(dataSourceAsyncMoq.Object, FileType.Csv, "test-file", 1);
            Assert.That(exportResult, Is.TypeOf<DataSourceExportResult>());
        }

        [Test]
        public async Task ShouldSetProvidedDataUsingDataSourceCreateFunction()
        {
            var exportResult = await exportResultFactory.CreateAsync(dataSourceAsyncMoq.Object, FileType.Csv, "test-file", 1);

            Assert.That(exportResult.FileType, Is.EqualTo(FileType.Csv));
            Assert.That(exportResult.Name, Is.EqualTo("test-file"));
            Assert.That(exportResult.Count, Is.EqualTo(1));
            Assert.That((await exportResult.GetDataAsync()).GetString(), Is.EqualTo("Test value"));
        }

        [Test]
        public async Task ShouldSetProvidedDataUsingStreamCreateFunction()
        {
            var exportResult = await exportResultFactory.CreateAsync(stream, FileType.Xlsx, "test-file2", 2);

            Assert.That(exportResult.FileType, Is.EqualTo(FileType.Xlsx));
            Assert.That(exportResult.Name, Is.EqualTo("test-file2"));
            Assert.That(exportResult.Count, Is.EqualTo(2));
            Assert.That((await exportResult.GetDataAsync()).GetString(), Is.EqualTo("Test value"));
        }

        [Test]
        public async Task ShouldCallCustomIExportStreamDataSourceFactoryCreateFunction()
        {
            exportResultFactory = new(dataSourceFactoryMoq.Object);

            var exportResult = await exportResultFactory.CreateAsync(stream, FileType.Csv, "test-file2", 2);

            dataSourceFactoryMoq.Verify(moq => moq.CreateAsync(It.IsAny<Stream>()), Times.Once());
        }
    }
}