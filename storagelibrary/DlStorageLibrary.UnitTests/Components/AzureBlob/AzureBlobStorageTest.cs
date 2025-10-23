using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using DemandLink.Common.Extensions.Dictionary;
using DemandLink.Storage;
using DemandLink.Storage.AzureBlob;
using DlStorageLibrary.UnitTests.Globals;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DlStorageLibrary.UnitTests.AzureBlob.AzureBlobStorageTests
{
    [TestFixture]
    public class SimpleLocalStorageTest
    {
        private readonly string containerName = "test-container";
        private readonly string fileName = "test-blob-download.json";
        private BlobContainerClient containerClient;
        private SimpleAzureBlobStorage storage;

        [SetUp]
        public void SetUp()
        {
            containerClient = new BlobContainerClient(TestResources.StorageConnectionString, containerName);
            storage = new SimpleAzureBlobStorage(TestResources.StorageConnectionString, containerName);
        }

        [Test]
        public void ShouldThrowCannotAccessStorageException()
        {
            var storage = new SimpleAzureBlobStorage("key=value", "test-container");
            Assert.ThrowsAsync<CannotAccessStorageException>(
                () => storage.CreateFileAsync("file-name", null),
                $"[{nameof(SimpleAzureBlobStorage)}][createBlobContainerClient]() Error: Cannot access the storage location. Possibly incorrect connection string"
            );
        }

        [Test]
        public async Task ShouldSaveBlobFile()
        {
            var filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Components", "AzureBlob", "Mocks", fileName);
            var stream = new FileStream(filePath, FileMode.Open);

            // Save the file
            var updatedFileName = await storage.CreateFileAsync(fileName, stream);

            stream.Dispose();

            var containerClient = new BlobContainerClient(TestResources.StorageConnectionString, containerName);
            var blobClient = containerClient.GetBlobClient(updatedFileName);

            Assert.IsTrue(blobClient.Exists().Value);

            blobClient.Delete();
        }

        [Test]
        public async Task ShouldDeleteBlobFile()
        {
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(fileName);

            var filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Components", "AzureBlob", "Mocks", fileName);
            var stream = new FileStream(filePath, FileMode.Open);

            blobClient.Upload(stream);

            stream.Dispose();

            Assert.IsTrue(blobClient.Exists());

            await storage.DeleteFileAsync(fileName);

            Assert.IsFalse(blobClient.Exists());
        }

        [Test]
        public async Task ShouldDownloadBlobFile()
        {
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(fileName);

            var filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Components", "AzureBlob", "Mocks", fileName);
            var stream = new FileStream(filePath, FileMode.Open);

            if (blobClient.Exists())
            {
                blobClient.Delete();
            }

            blobClient.Upload(stream);

            stream.Dispose();

            Assert.IsTrue(blobClient.Exists());

            var responseStream = await storage.DownloadFileAsync(fileName);

            var streamReader = new StreamReader(responseStream);

            var blobContents = streamReader.ReadToEnd();

            var jsonFile = JsonConvert.DeserializeObject<Dictionary<string, string>>(blobContents);

            Assert.AreEqual("download", jsonFile.GetValue("key"));

            blobClient.Delete();

            streamReader.Dispose();
            responseStream.Dispose();
        }

        [Test]
        public void ShouldThrowExceptionForDownloadBlobFileWhenNoFileExists()
        {
            Assert.ThrowsAsync<FileDoesNotExistException>(
                () => storage.DownloadFileAsync(fileName),
                $"[{nameof(SimpleAzureBlobStorage)}][{nameof(SimpleAzureBlobStorage.DownloadFileAsync)}](fileName, cancellationToken) Error: The specified file '{fileName}' does not exist."
            );
        }

        [Test]
        public async Task ShouldReturnTrueIfBlobExistsAndFalseAfterBlobIsDeleted()
        {
            containerClient.CreateIfNotExists();
            var blobClient = containerClient.GetBlobClient(fileName);

            var filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Components", "AzureBlob", "Mocks", fileName);
            var stream = new FileStream(filePath, FileMode.Open);

            if (blobClient.Exists())
            {
                blobClient.Delete();
            }

            blobClient.Upload(stream);

            stream.Dispose();

            Assert.IsTrue(await storage.ExistsAsync(fileName));

            await storage.DeleteFileAsync(fileName);

            Assert.IsFalse(await storage.ExistsAsync(fileName));
        }

        [Test]
        public async Task ShouldReturnFalseIfBlobContainerDoesntExist()
        {
            var nonExistingcontainerName = "test-non-existing-container";

            var storage = new SimpleAzureBlobStorage(TestResources.StorageConnectionString, nonExistingcontainerName);

            Assert.IsFalse(await storage.ExistsAsync(fileName));
        }
    }
}