using AMCode.Storage.Infrastructure.Logging;
using AMCode.Storage.UnitTests.Logging;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.Logging
{
    /// <summary>
    /// Unit tests for the StorageOperationLogger class.
    /// </summary>
    [TestFixture]
    public class StorageOperationLoggerTests : LoggingTestBase
    {
        private StorageOperationLogger _operationLogger = null!;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _operationLogger = new StorageOperationLogger(MockLogger);
        }

        [Test]
        public void ShouldLogFileOperation()
        {
            // Act
            _operationLogger.LogFileOperation("CreateFile", "test.txt", 1024, "Local");

            // Assert
            AssertFileOperationLogCount(1);
            AssertFileOperationLogExists("CreateFile", "test.txt");
            
            var log = MockOperationLogger.FileOperationLogs[0];
            Assert.AreEqual("CreateFile", log.Operation);
            Assert.AreEqual("test.txt", log.FileName);
            Assert.AreEqual(1024, log.FileSizeBytes);
            Assert.AreEqual("Local", log.StorageType);
        }

        [Test]
        public void ShouldLogFileOperationWithoutOptionalParameters()
        {
            // Act
            _operationLogger.LogFileOperation("DeleteFile", "test.txt");

            // Assert
            AssertFileOperationLogCount(1);
            
            var log = MockOperationLogger.FileOperationLogs[0];
            Assert.AreEqual("DeleteFile", log.Operation);
            Assert.AreEqual("test.txt", log.FileName);
            Assert.IsNull(log.FileSizeBytes);
            Assert.IsNull(log.StorageType);
        }

        [Test]
        public void ShouldLogStorageProvider()
        {
            // Act
            _operationLogger.LogStorageProvider("AzureBlob", "CreateFile", "test.txt");

            // Assert
            Assert.AreEqual(1, MockOperationLogger.StorageProviderLogs.Count);
            
            var log = MockOperationLogger.StorageProviderLogs[0];
            Assert.AreEqual("AzureBlob", log.ProviderName);
            Assert.AreEqual("CreateFile", log.Operation);
            Assert.AreEqual("test.txt", log.FileName);
        }

        [Test]
        public void ShouldLogPerformance()
        {
            // Arrange
            var duration = TimeSpan.FromMilliseconds(150);

            // Act
            _operationLogger.LogPerformance("CreateFile", duration, 1024);

            // Assert
            AssertPerformanceLogCount(1);
            AssertPerformanceLogExists("CreateFile");
            
            var log = MockOperationLogger.PerformanceLogs[0];
            Assert.AreEqual("CreateFile", log.Operation);
            Assert.AreEqual(duration, log.Duration);
            Assert.AreEqual(1024, log.FileSizeBytes);
        }

        [Test]
        public void ShouldLogPerformanceWithoutFileSize()
        {
            // Arrange
            var duration = TimeSpan.FromMilliseconds(75);

            // Act
            _operationLogger.LogPerformance("DeleteFile", duration);

            // Assert
            AssertPerformanceLogCount(1);
            
            var log = MockOperationLogger.PerformanceLogs[0];
            Assert.AreEqual("DeleteFile", log.Operation);
            Assert.AreEqual(duration, log.Duration);
            Assert.IsNull(log.FileSizeBytes);
        }

        [Test]
        public void ShouldLogStorageError()
        {
            // Arrange
            var exception = new Exception("Test error");

            // Act
            _operationLogger.LogStorageError("CreateFile", "test.txt", exception, "Local");

            // Assert
            Assert.AreEqual(1, MockOperationLogger.StorageErrorLogs.Count);
            
            var log = MockOperationLogger.StorageErrorLogs[0];
            Assert.AreEqual("CreateFile", log.Operation);
            Assert.AreEqual("test.txt", log.FileName);
            Assert.AreEqual(exception, log.Exception);
            Assert.AreEqual("Local", log.StorageType);
        }

        [Test]
        public void ShouldLogStorageErrorWithoutStorageType()
        {
            // Arrange
            var exception = new Exception("Test error");

            // Act
            _operationLogger.LogStorageError("CreateFile", "test.txt", exception);

            // Assert
            Assert.AreEqual(1, MockOperationLogger.StorageErrorLogs.Count);
            
            var log = MockOperationLogger.StorageErrorLogs[0];
            Assert.AreEqual("CreateFile", log.Operation);
            Assert.AreEqual("test.txt", log.FileName);
            Assert.AreEqual(exception, log.Exception);
            Assert.IsNull(log.StorageType);
        }

        [Test]
        public void ShouldHandleNullParameters()
        {
            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => _operationLogger.LogFileOperation("Test", null));
            Assert.DoesNotThrow(() => _operationLogger.LogStorageProvider("Test", "Test", null));
            Assert.DoesNotThrow(() => _operationLogger.LogStorageError("Test", null, new Exception()));
        }
    }
}
