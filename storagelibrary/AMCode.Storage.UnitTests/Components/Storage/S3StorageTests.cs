using AMCode.Storage.Components.Storage;
using AMCode.Storage.UnitTests.Logging;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AMCode.Storage.UnitTests.Components.Storage
{
    [TestFixture]
    public class S3StorageTests
    {
        private MockStorageLogger _mockLogger;
        private S3Storage _storage;
        private const string AccessKey = "test-access-key";
        private const string SecretKey = "test-secret-key";
        private const string Region = "us-east-1";
        private const string BucketName = "test-bucket";

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new MockStorageLogger();
            _storage = new S3Storage(AccessKey, SecretKey, Region, BucketName, _mockLogger);
        }

        [TearDown]
        public void TearDown()
        {
            _storage?.Dispose();
        }

        [Test]
        public void Constructor_WithValidParameters_CreatesInstance()
        {
            // Act
            var storage = new S3Storage(AccessKey, SecretKey, Region, BucketName, _mockLogger);

            // Assert
            Assert.IsNotNull(storage);
            storage.Dispose();
        }

        [Test]
        public void Constructor_WithNullAccessKey_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new S3Storage(null, SecretKey, Region, BucketName, _mockLogger));
        }

        [Test]
        public void Constructor_WithNullSecretKey_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new S3Storage(AccessKey, null, Region, BucketName, _mockLogger));
        }

        [Test]
        public void Constructor_WithNullRegion_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new S3Storage(AccessKey, SecretKey, null, BucketName, _mockLogger));
        }

        [Test]
        public void Constructor_WithNullBucketName_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new S3Storage(AccessKey, SecretKey, Region, null, _mockLogger));
        }

        [Test]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new S3Storage(AccessKey, SecretKey, Region, BucketName, null));
        }

        [Test]
        public void GenerateS3Url_WithValidPath_ReturnsCorrectUrl()
        {
            // Arrange
            var filePath = "images/test.jpg";

            // Act
            var url = _storage.GenerateS3Url(filePath);

            // Assert
            Assert.IsNotNull(url);
            Assert.AreEqual($"https://{BucketName}.s3.{Region}.amazonaws.com/{filePath}", url);
        }

        [Test]
        public void GenerateS3Url_WithPathWithBackslashes_NormalizesPath()
        {
            // Arrange
            var filePath = "images\\test.jpg";

            // Act
            var url = _storage.GenerateS3Url(filePath);

            // Assert
            Assert.IsNotNull(url);
            Assert.IsTrue(url.Contains("images/test.jpg"));
        }

        [Test]
        public void GenerateS3Url_WithPathWithLeadingSlash_RemovesLeadingSlash()
        {
            // Arrange
            var filePath = "/images/test.jpg";

            // Act
            var url = _storage.GenerateS3Url(filePath);

            // Assert
            Assert.IsNotNull(url);
            Assert.IsTrue(url.EndsWith("images/test.jpg"));
        }

        // Note: The following tests would require actual S3 access or more sophisticated mocking
        // For unit tests with actual S3, you would need:
        // 1. Test S3 bucket configured
        // 2. AWS credentials for testing
        // 3. Cleanup after tests
        // 
        // For now, these are integration test scenarios that should be run separately
        // with actual S3 infrastructure or LocalStack

        [Test]
        [Ignore("Requires actual S3 access or LocalStack. Run as integration test.")]
        public async Task StoreFileAsync_WithValidData_ReturnsSuccess()
        {
            // This test requires actual S3 access
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
        [Ignore("Requires actual S3 access or LocalStack. Run as integration test.")]
        public async Task GetFileAsync_WithExistingFile_ReturnsStream()
        {
            // This test requires actual S3 access
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
        [Ignore("Requires actual S3 access or LocalStack. Run as integration test.")]
        public async Task DeleteFileAsync_WithExistingFile_ReturnsSuccess()
        {
            // This test requires actual S3 access
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
        [Ignore("Requires actual S3 access or LocalStack. Run as integration test.")]
        public async Task FileExistsAsync_WithExistingFile_ReturnsTrue()
        {
            // This test requires actual S3 access
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
        [Ignore("Requires actual S3 access or LocalStack. Run as integration test.")]
        public async Task FileExistsAsync_WithNonExistingFile_ReturnsFalse()
        {
            // This test requires actual S3 access
            // Act
            var exists = await _storage.FileExistsAsync("non-existing-file.jpg");

            // Assert
            Assert.IsFalse(exists);
        }

        [Test]
        [Ignore("Requires actual S3 access or LocalStack. Run as integration test.")]
        public async Task ListFilesAsync_WithDirectoryPath_ReturnsFiles()
        {
            // This test requires actual S3 access
            // Arrange
            var directoryPath = "images";
            var fileName1 = "images/test1.jpg";
            var fileName2 = "images/test2.jpg";
            var content = Encoding.UTF8.GetBytes("test content");
            
            using var stream1 = new MemoryStream(content);
            using var stream2 = new MemoryStream(content);
            await _storage.StoreFileAsync(stream1, "test1.jpg", "images");
            await _storage.StoreFileAsync(stream2, "test2.jpg", "images");

            // Act
            var result = await _storage.ListFilesAsync(directoryPath);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.GreaterOrEqual(result.Value.Length, 2);
            Assert.IsTrue(result.Value.Contains(fileName1));
            Assert.IsTrue(result.Value.Contains(fileName2));
        }
    }
}

