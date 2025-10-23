using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using AMCode.Common.Extensions.Dictionary;
using AMCode.Storage.AzureBlob;
using AMCode.Storage.UnitTests.Globals;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.AzureBlob.AzureBlobStreamDataSourceTests
{
    [TestFixture]
    public class AzureBlobStreamDataSourceTest
    {
        private BlobContainerClient containerClient;
        private readonly string containerName = "test-data-source-container";
        private AzureBlobStreamDataSource dataSource;
        private readonly string fileName = "test-blob-download.json";
        private readonly LocalTestHelper testHelper = new();

        [SetUp]
        public void SetUp()
        {
            containerClient = new(TestResources.StorageConnectionString, containerName);
            dataSource = new(TestResources.StorageConnectionString, containerName);
        }

        [TearDown]
        public void TearDown() => dataSource.Dispose();

        [Test]
        public async Task ShouldSetStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            await dataSource.SetStreamAsync(stream);

            var blobClient = containerClient.GetBlobClient(dataSource.FileName);

            var fileExistsResponse = await blobClient.ExistsAsync();

            Assert.That(fileExistsResponse.Value, Is.True);
        }

        [Test]
        public async Task ShouldGetStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            var blobClient = containerClient.GetBlobClient(dataSource.FileName);
            await blobClient.UploadAsync(stream);

            var storedStream = await dataSource.GetStreamAsync();

            var streamReader = new StreamReader(storedStream);

            var blobContents = streamReader.ReadToEnd();

            var jsonFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(blobContents);

            Assert.AreEqual("download", jsonFile.GetValue("key"));
        }

        [Test]
        public async Task ShouldDisposeOfStream()
        {
            var filePath = testHelper.GetMockFilePath(TestContext.CurrentContext, fileName);
            using var stream = new FileStream(filePath, FileMode.Open);

            await dataSource.SetStreamAsync(stream);

            var blobClient = containerClient.GetBlobClient(dataSource.FileName);

            var fileExistsResponse = await blobClient.ExistsAsync();

            Assert.That(fileExistsResponse.Value, Is.True);

            dataSource.Dispose();

            blobClient = containerClient.GetBlobClient(dataSource.FileName);

            fileExistsResponse = await blobClient.ExistsAsync();

            Assert.That(fileExistsResponse.Value, Is.False);
        }
    }
}