using System.IO;
using System.Threading.Tasks;
using AMCode.Exports;
using AMCode.Storage;
using Moq;
using NUnit.Framework;

namespace DlExportsLibrary.UnitTests.Components.Results.DataSourceExportResultTests
{
    [TestFixture]
    public class DataSourceExportResultTest
    {
        private DataSourceExportResult exportResult;
        private Mock<IStreamDataSourceAsync> dataSourceAsyncMoq;
        private MemoryStream memoryStream;

        [SetUp]
        public void SetUp()
        {
            memoryStream = new MemoryStream();

            dataSourceAsyncMoq = new();
            dataSourceAsyncMoq.Setup(moq => moq.GetStreamAsync()).Returns(() => Task.FromResult<Stream>(memoryStream));
            dataSourceAsyncMoq.Setup(moq => moq.SetStreamAsync(It.IsAny<Stream>())).Returns(() => Task.CompletedTask);

            exportResult = new DataSourceExportResult(dataSourceAsyncMoq.Object);
        }

        [TearDown]
        public void TearDown()
        {
            memoryStream?.Dispose();
            exportResult?.Dispose();
        }

        [Test]
        public async Task ShouldGetStream()
        {
            var stream = await exportResult.GetDataAsync();
            Assert.That(stream, Is.SameAs(memoryStream));
        }

        [Test]
        public async Task ShouldSetStream()
        {
            await exportResult.SetDataAsync(memoryStream);

            dataSourceAsyncMoq.Verify(moq => moq.SetStreamAsync(It.Is<Stream>(s => s == memoryStream)), Times.Once());
        }
    }
}