using System;

namespace AMCode.Storage.Interfaces
{
    /// <summary>
    /// Storage-specific operation logging interface.
    /// Provides specialized logging methods for file operations, storage providers, performance metrics, and errors.
    /// </summary>
    public interface IStorageOperationLogger
    {
        /// <summary>
        /// Logs a file operation with optional file size and storage type information.
        /// </summary>
        /// <param name="operation">The operation being performed (e.g., "CreateFile", "DownloadFile").</param>
        /// <param name="fileName">The name of the file being operated on.</param>
        /// <param name="fileSizeBytes">Optional file size in bytes.</param>
        /// <param name="storageType">Optional storage type (e.g., "AzureBlob", "Local", "InMemory").</param>
        void LogFileOperation(string operation, string fileName, long? fileSizeBytes = null, string? storageType = null);

        /// <summary>
        /// Logs storage provider information for an operation.
        /// </summary>
        /// <param name="providerName">The name of the storage provider.</param>
        /// <param name="operation">The operation being performed.</param>
        /// <param name="fileName">The name of the file being operated on.</param>
        void LogStorageProvider(string providerName, string operation, string fileName);

        /// <summary>
        /// Logs performance metrics for a storage operation.
        /// </summary>
        /// <param name="operation">The operation that was performed.</param>
        /// <param name="duration">The duration of the operation.</param>
        /// <param name="fileSizeBytes">Optional file size in bytes for throughput calculations.</param>
        void LogPerformance(string operation, TimeSpan duration, long? fileSizeBytes = null);

        /// <summary>
        /// Logs a storage error with context information.
        /// </summary>
        /// <param name="operation">The operation that failed.</param>
        /// <param name="fileName">The name of the file involved in the operation.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="storageType">Optional storage type where the error occurred.</param>
        void LogStorageError(string operation, string fileName, Exception exception, string? storageType = null);
    }
}
