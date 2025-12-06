using AMCode.Common.Models;
using AMCode.Storage.Components.Storage;
using AMCode.Storage.UnitTests.Logging;
using Azure.Storage.Blobs;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AMCode.Storage.UnitTests.Components.Storage
{
    [TestFixture]
    public class AzureBlobStorageTests
    {
        private MockStorageLogger _mockLogger;
        private AzureBlobStorage _storage;
        private const string ConnectionString = "UseDevelopmentStorage=true";
        private const string ContainerName = "test-container";

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new MockStorageLogger();
            _storage = new AzureBlobStorage(ConnectionString, ContainerName, _mockLogger);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up test container if it exists
            try
            {
                var containerClient = new BlobContainerClient(ConnectionString, ContainerName);
                if (containerClient.Exists())
                {
                    containerClient.Delete();
                }
            }
            catch
            {
                // Ignore cleanup errors
            }
        }

        [Test]
        public void Constructor_WithValidParameters_CreatesInstance()
        {
            // Act
            var storage = new AzureBlobStorage(ConnectionString, ContainerName, _mockLogger);

            // Assert
            Assert.IsNotNull(storage);
        }

        [Test]
        public void Constructor_WithNullConnectionString_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AzureBlobStorage(null, ContainerName, _mockLogger));
        }

        [Test]
        public void Constructor_WithNullContainerName_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AzureBlobStorage(ConnectionString, null, _mockLogger));
        }

        [Test]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new AzureBlobStorage(ConnectionString, ContainerName, null));
        }

        [Test]
        public void GenerateBlobUrl_WithValidPath_ReturnsCorrectUrl()
        {
            // Arrange
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=testaccount;AccountKey=testkey;EndpointSuffix=core.windows.net";
            var storage = new AzureBlobStorage(connectionString, ContainerName, _mockLogger);
            var filePath = "images/test.jpg";

            // Act
            var url = storage.GenerateBlobUrl(filePath);

            // Assert
            Assert.IsNotNull(url);
            Assert.IsTrue(url.Contains("testaccount"));
            Assert.IsTrue(url.Contains(ContainerName));
            Assert.IsTrue(url.Contains("images/test.jpg"));
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task StoreFileAsync_WithValidData_ReturnsSuccess()
        {
            // This test requires actual Azure Storage access
            // Arrange
            var fileName = "test-image.jpg";
            var content = Encoding.UTF8.GetBytes("test content");
            using var stream = new MemoryStream(content);

            // Act
            var result = await _storage.StoreFileAsync(stream, fileName);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task StoreFileAsync_WithFolderPath_StoresInCorrectPath()
        {
            // This test requires actual Azure Storage access
            // Arrange
            var fileName = "test-image.jpg";
            var folderPath = "images";
            var content = Encoding.UTF8.GetBytes("test content");
            using var stream = new MemoryStream(content);

            // Act
            var result = await _storage.StoreFileAsync(stream, fileName, folderPath);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Value.Contains("images"));
            Assert.IsTrue(result.Value.Contains(fileName));
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task GetFileAsync_WithExistingFile_ReturnsStream()
        {
            // This test requires actual Azure Storage access
            // Arrange
            var fileName = "test-image.jpg";
            var content = Encoding.UTF8.GetBytes("test content");
            using var uploadStream = new MemoryStream(content);
            await _storage.StoreFileAsync(uploadStream, fileName);

            // Act
            var result = await _storage.GetFileAsync(fileName);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            using var reader = new StreamReader(result.Value);
            var retrievedContent = await reader.ReadToEndAsync();
            Assert.AreEqual("test content", retrievedContent);
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task GetFileAsync_WithNonExistingFile_ReturnsFailure()
        {
            // This test requires actual Azure Storage access
            // Act
            var result = await _storage.GetFileAsync("non-existing-file.jpg");

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("File not found", result.Error);
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task DeleteFileAsync_WithExistingFile_ReturnsSuccess()
        {
            // This test requires actual Azure Storage access
            // Arrange
            var fileName = "test-image.jpg";
            var content = Encoding.UTF8.GetBytes("test content");
            using var stream = new MemoryStream(content);
            await _storage.StoreFileAsync(stream, fileName);

            // Act
            var result = await _storage.DeleteFileAsync(fileName);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task DeleteFileAsync_WithNonExistingFile_ReturnsFailure()
        {
            // This test requires actual Azure Storage access
            // Act
            var result = await _storage.DeleteFileAsync("non-existing-file.jpg");

            // Assert
            Assert.IsTrue(result.IsFailure);
            Assert.AreEqual("File not found", result.Error);
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task FileExistsAsync_WithExistingFile_ReturnsTrue()
        {
            // This test requires actual Azure Storage access
            // Arrange
            var fileName = "test-image.jpg";
            var content = Encoding.UTF8.GetBytes("test content");
            using var stream = new MemoryStream(content);
            await _storage.StoreFileAsync(stream, fileName);

            // Act
            var exists = await _storage.FileExistsAsync(fileName);

            // Assert
            Assert.IsTrue(exists);
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task FileExistsAsync_WithNonExistingFile_ReturnsFalse()
        {
            // This test requires actual Azure Storage access
            // Act
            var exists = await _storage.FileExistsAsync("non-existing-file.jpg");

            // Assert
            Assert.IsFalse(exists);
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task ListFilesAsync_WithDirectoryPath_ReturnsFiles()
        {
            // This test requires actual Azure Storage access
            // Arrange
            var directoryPath = "images";
            var fileName1 = "test1.jpg";
            var fileName2 = "test2.jpg";
            var content = Encoding.UTF8.GetBytes("test content");
            
            using var stream1 = new MemoryStream(content);
            using var stream2 = new MemoryStream(content);
            await _storage.StoreFileAsync(stream1, fileName1, directoryPath);
            await _storage.StoreFileAsync(stream2, fileName2, directoryPath);

            // Act
            var result = await _storage.ListFilesAsync(directoryPath);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.GreaterOrEqual(result.Value.Length, 2);
            Assert.IsTrue(result.Value.Any(f => f.Contains(fileName1)));
            Assert.IsTrue(result.Value.Any(f => f.Contains(fileName2)));
        }

        [Test]
        [Ignore("Requires actual Azure Storage access. Run as integration test.")]
        public async Task ListFilesAsync_WithNonExistingDirectory_ReturnsEmptyArray()
        {
            // This test requires actual Azure Storage access
            // Act
            var result = await _storage.ListFilesAsync("non-existing-directory");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(0, result.Value.Length);
        }
    }
}

