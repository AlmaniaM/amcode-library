using AMCode.Storage.Interfaces;
using System.Collections.Generic;

namespace AMCode.Storage.UnitTests.Logging
{
    /// <summary>
    /// Mock implementation of IStorageMetricsLogger for unit testing.
    /// Captures all metrics log calls for verification in tests.
    /// </summary>
    public class MockStorageMetricsLogger : IStorageMetricsLogger
    {
        /// <summary>
        /// Gets the list of operation metrics recorded.
        /// </summary>
        public List<OperationMetric> OperationMetrics { get; } = new List<OperationMetric>();

        /// <summary>
        /// Gets the list of error metrics recorded.
        /// </summary>
        public List<ErrorMetric> ErrorMetrics { get; } = new List<ErrorMetric>();

        /// <summary>
        /// Gets the list of storage usage metrics recorded.
        /// </summary>
        public List<StorageUsageMetric> StorageUsageMetrics { get; } = new List<StorageUsageMetric>();

        /// <inheritdoc />
        public void RecordOperation(string operation, string storageType, TimeSpan duration, long? fileSizeBytes = null)
        {
            OperationMetrics.Add(new OperationMetric
            {
                Operation = operation,
                StorageType = storageType,
                Duration = duration,
                FileSizeBytes = fileSizeBytes
            });
        }

        /// <inheritdoc />
        public void RecordError(string operation, string storageType, string errorType)
        {
            ErrorMetrics.Add(new ErrorMetric
            {
                Operation = operation,
                StorageType = storageType,
                ErrorType = errorType
            });
        }

        /// <inheritdoc />
        public void RecordStorageUsage(string storageType, long bytesUsed)
        {
            StorageUsageMetrics.Add(new StorageUsageMetric
            {
                StorageType = storageType,
                BytesUsed = bytesUsed
            });
        }

        /// <summary>
        /// Clears all captured metrics.
        /// </summary>
        public void Clear()
        {
            OperationMetrics.Clear();
            ErrorMetrics.Clear();
            StorageUsageMetrics.Clear();
        }

        /// <summary>
        /// Gets the total count of all metrics recorded.
        /// </summary>
        public int TotalMetricsCount => OperationMetrics.Count + ErrorMetrics.Count + StorageUsageMetrics.Count;

        /// <summary>
        /// Gets the total bytes used across all storage types.
        /// </summary>
        public long TotalBytesUsed => StorageUsageMetrics.Sum(m => m.BytesUsed);

        /// <summary>
        /// Gets the total duration across all operations.
        /// </summary>
        public TimeSpan TotalDuration => TimeSpan.FromMilliseconds(OperationMetrics.Sum(m => m.Duration.TotalMilliseconds));
    }

    /// <summary>
    /// Represents an operation metric entry.
    /// </summary>
    public class OperationMetric
    {
        public string Operation { get; set; } = string.Empty;
        public string StorageType { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public long? FileSizeBytes { get; set; }
    }

    /// <summary>
    /// Represents an error metric entry.
    /// </summary>
    public class ErrorMetric
    {
        public string Operation { get; set; } = string.Empty;
        public string StorageType { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a storage usage metric entry.
    /// </summary>
    public class StorageUsageMetric
    {
        public string StorageType { get; set; } = string.Empty;
        public long BytesUsed { get; set; }
    }
}
