using AMCode.Storage.Infrastructure.Logging;
using AMCode.Storage.UnitTests.Logging;
using NUnit.Framework;

namespace AMCode.Storage.UnitTests.Logging
{
    /// <summary>
    /// Unit tests for the StorageMetricsLogger class.
    /// </summary>
    [TestFixture]
    public class StorageMetricsLoggerTests : LoggingTestBase
    {
        private StorageMetricsLogger _metricsLogger = null!;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _metricsLogger = new StorageMetricsLogger(MockLogger);
        }

        [Test]
        public void ShouldRecordOperation()
        {
            // Arrange
            var duration = TimeSpan.FromMilliseconds(200);

            // Act
            _metricsLogger.RecordOperation("CreateFile", "Local", duration, 1024);

            // Assert
            AssertOperationMetricsCount(1);
            AssertOperationMetricExists("CreateFile", "Local");
            
            var metric = MockMetricsLogger.OperationMetrics[0];
            Assert.AreEqual("CreateFile", metric.Operation);
            Assert.AreEqual("Local", metric.StorageType);
            Assert.AreEqual(duration, metric.Duration);
            Assert.AreEqual(1024, metric.FileSizeBytes);
        }

        [Test]
        public void ShouldRecordOperationWithoutFileSize()
        {
            // Arrange
            var duration = TimeSpan.FromMilliseconds(100);

            // Act
            _metricsLogger.RecordOperation("DeleteFile", "AzureBlob", duration);

            // Assert
            AssertOperationMetricsCount(1);
            
            var metric = MockMetricsLogger.OperationMetrics[0];
            Assert.AreEqual("DeleteFile", metric.Operation);
            Assert.AreEqual("AzureBlob", metric.StorageType);
            Assert.AreEqual(duration, metric.Duration);
            Assert.IsNull(metric.FileSizeBytes);
        }

        [Test]
        public void ShouldRecordError()
        {
            // Act
            _metricsLogger.RecordError("CreateFile", "Local", "IOException");

            // Assert
            Assert.AreEqual(1, MockMetricsLogger.ErrorMetrics.Count);
            
            var metric = MockMetricsLogger.ErrorMetrics[0];
            Assert.AreEqual("CreateFile", metric.Operation);
            Assert.AreEqual("Local", metric.StorageType);
            Assert.AreEqual("IOException", metric.ErrorType);
        }

        [Test]
        public void ShouldRecordStorageUsage()
        {
            // Act
            _metricsLogger.RecordStorageUsage("Local", 2048);

            // Assert
            Assert.AreEqual(1, MockMetricsLogger.StorageUsageMetrics.Count);
            
            var metric = MockMetricsLogger.StorageUsageMetrics[0];
            Assert.AreEqual("Local", metric.StorageType);
            Assert.AreEqual(2048, metric.BytesUsed);
        }

        [Test]
        public void ShouldAccumulateStorageUsage()
        {
            // Act
            _metricsLogger.RecordStorageUsage("Local", 1024);
            _metricsLogger.RecordStorageUsage("Local", 2048);
            _metricsLogger.RecordStorageUsage("AzureBlob", 512);

            // Assert
            Assert.AreEqual(3, MockMetricsLogger.StorageUsageMetrics.Count);
            Assert.AreEqual(3072, MockMetricsLogger.TotalBytesUsed);
            
            var localMetrics = MockMetricsLogger.StorageUsageMetrics.Where(m => m.StorageType == "Local").ToList();
            Assert.AreEqual(2, localMetrics.Count);
            Assert.AreEqual(1024, localMetrics[0].BytesUsed);
            Assert.AreEqual(2048, localMetrics[1].BytesUsed);
        }

        [Test]
        public void ShouldAccumulateOperationDuration()
        {
            // Arrange
            var duration1 = TimeSpan.FromMilliseconds(100);
            var duration2 = TimeSpan.FromMilliseconds(200);

            // Act
            _metricsLogger.RecordOperation("CreateFile", "Local", duration1);
            _metricsLogger.RecordOperation("DeleteFile", "Local", duration2);

            // Assert
            Assert.AreEqual(2, MockMetricsLogger.OperationMetrics.Count);
            Assert.AreEqual(TimeSpan.FromMilliseconds(300), MockMetricsLogger.TotalDuration);
        }

        [Test]
        public void ShouldGetOperationCounts()
        {
            // Act
            _metricsLogger.RecordOperation("CreateFile", "Local", TimeSpan.FromMilliseconds(100));
            _metricsLogger.RecordOperation("CreateFile", "Local", TimeSpan.FromMilliseconds(150));
            _metricsLogger.RecordOperation("DeleteFile", "AzureBlob", TimeSpan.FromMilliseconds(200));

            // Assert
            var operationCounts = MockMetricsLogger.GetOperationCounts();
            Assert.AreEqual(3, operationCounts.Count);
            Assert.IsTrue(operationCounts.ContainsKey("CreateFile_Local"));
            Assert.IsTrue(operationCounts.ContainsKey("DeleteFile_AzureBlob"));
            Assert.AreEqual(2, operationCounts["CreateFile_Local"]);
            Assert.AreEqual(1, operationCounts["DeleteFile_AzureBlob"]);
        }

        [Test]
        public void ShouldGetErrorCounts()
        {
            // Act
            _metricsLogger.RecordError("CreateFile", "Local", "IOException");
            _metricsLogger.RecordError("CreateFile", "Local", "IOException");
            _metricsLogger.RecordError("DeleteFile", "AzureBlob", "NetworkException");

            // Assert
            var errorCounts = MockMetricsLogger.GetErrorCounts();
            Assert.AreEqual(3, errorCounts.Count);
            Assert.IsTrue(errorCounts.ContainsKey("CreateFile_Local_IOException"));
            Assert.IsTrue(errorCounts.ContainsKey("DeleteFile_AzureBlob_NetworkException"));
            Assert.AreEqual(2, errorCounts["CreateFile_Local_IOException"]);
            Assert.AreEqual(1, errorCounts["DeleteFile_AzureBlob_NetworkException"]);
        }

        [Test]
        public void ShouldGetStorageUsage()
        {
            // Act
            _metricsLogger.RecordStorageUsage("Local", 1024);
            _metricsLogger.RecordStorageUsage("AzureBlob", 2048);

            // Assert
            var storageUsage = MockMetricsLogger.GetStorageUsage();
            Assert.AreEqual(2, storageUsage.Count);
            Assert.IsTrue(storageUsage.ContainsKey("Local"));
            Assert.IsTrue(storageUsage.ContainsKey("AzureBlob"));
            Assert.AreEqual(1024, storageUsage["Local"]);
            Assert.AreEqual(2048, storageUsage["AzureBlob"]);
        }

        [Test]
        public void ShouldHandleNullParameters()
        {
            // Act & Assert - Should not throw
            Assert.DoesNotThrow(() => _metricsLogger.RecordOperation("Test", null, TimeSpan.Zero));
            Assert.DoesNotThrow(() => _metricsLogger.RecordError("Test", null, null));
            Assert.DoesNotThrow(() => _metricsLogger.RecordStorageUsage(null, 0));
        }
    }
}
