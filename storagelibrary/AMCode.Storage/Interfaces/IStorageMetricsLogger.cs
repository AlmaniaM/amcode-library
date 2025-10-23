using System;

namespace AMCode.Storage.Interfaces
{
    /// <summary>
    /// Storage metrics logging interface for performance and usage tracking.
    /// Provides methods for recording operation metrics, errors, and storage usage statistics.
    /// </summary>
    public interface IStorageMetricsLogger
    {
        /// <summary>
        /// Records metrics for a storage operation.
        /// </summary>
        /// <param name="operation">The operation that was performed.</param>
        /// <param name="storageType">The type of storage used (e.g., "AzureBlob", "Local", "InMemory").</param>
        /// <param name="duration">The duration of the operation.</param>
        /// <param name="fileSizeBytes">Optional file size in bytes for throughput calculations.</param>
        void RecordOperation(string operation, string storageType, TimeSpan duration, long? fileSizeBytes = null);

        /// <summary>
        /// Records an error occurrence for metrics tracking.
        /// </summary>
        /// <param name="operation">The operation that failed.</param>
        /// <param name="storageType">The type of storage where the error occurred.</param>
        /// <param name="errorType">The type of error that occurred.</param>
        void RecordError(string operation, string storageType, string errorType);

        /// <summary>
        /// Records storage usage statistics.
        /// </summary>
        /// <param name="storageType">The type of storage.</param>
        /// <param name="bytesUsed">The number of bytes used.</param>
        void RecordStorageUsage(string storageType, long bytesUsed);
    }
}
