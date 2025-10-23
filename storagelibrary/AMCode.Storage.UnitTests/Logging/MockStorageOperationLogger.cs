using AMCode.Storage.Interfaces;
using System.Collections.Generic;

namespace AMCode.Storage.UnitTests.Logging
{
    /// <summary>
    /// Mock implementation of IStorageOperationLogger for unit testing.
    /// Captures all operation log calls for verification in tests.
    /// </summary>
    public class MockStorageOperationLogger : IStorageOperationLogger
    {
        /// <summary>
        /// Gets the list of file operation log calls.
        /// </summary>
        public List<FileOperationLog> FileOperationLogs { get; } = new List<FileOperationLog>();

        /// <summary>
        /// Gets the list of storage provider log calls.
        /// </summary>
        public List<StorageProviderLog> StorageProviderLogs { get; } = new List<StorageProviderLog>();

        /// <summary>
        /// Gets the list of performance log calls.
        /// </summary>
        public List<PerformanceLog> PerformanceLogs { get; } = new List<PerformanceLog>();

        /// <summary>
        /// Gets the list of storage error log calls.
        /// </summary>
        public List<StorageErrorLog> StorageErrorLogs { get; } = new List<StorageErrorLog>();

        /// <inheritdoc />
        public void LogFileOperation(string operation, string fileName, long? fileSizeBytes = null, string? storageType = null)
        {
            FileOperationLogs.Add(new FileOperationLog
            {
                Operation = operation,
                FileName = fileName,
                FileSizeBytes = fileSizeBytes,
                StorageType = storageType
            });
        }

        /// <inheritdoc />
        public void LogStorageProvider(string providerName, string operation, string fileName)
        {
            StorageProviderLogs.Add(new StorageProviderLog
            {
                ProviderName = providerName,
                Operation = operation,
                FileName = fileName
            });
        }

        /// <inheritdoc />
        public void LogPerformance(string operation, TimeSpan duration, long? fileSizeBytes = null)
        {
            PerformanceLogs.Add(new PerformanceLog
            {
                Operation = operation,
                Duration = duration,
                FileSizeBytes = fileSizeBytes
            });
        }

        /// <inheritdoc />
        public void LogStorageError(string operation, string fileName, Exception exception, string? storageType = null)
        {
            StorageErrorLogs.Add(new StorageErrorLog
            {
                Operation = operation,
                FileName = fileName,
                Exception = exception,
                StorageType = storageType
            });
        }

        /// <summary>
        /// Clears all captured log calls.
        /// </summary>
        public void Clear()
        {
            FileOperationLogs.Clear();
            StorageProviderLogs.Clear();
            PerformanceLogs.Clear();
            StorageErrorLogs.Clear();
        }

        /// <summary>
        /// Gets the total count of all operation log calls.
        /// </summary>
        public int TotalLogCount => FileOperationLogs.Count + StorageProviderLogs.Count + PerformanceLogs.Count + StorageErrorLogs.Count;
    }

    /// <summary>
    /// Represents a file operation log entry.
    /// </summary>
    public class FileOperationLog
    {
        public string Operation { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long? FileSizeBytes { get; set; }
        public string? StorageType { get; set; }
    }

    /// <summary>
    /// Represents a storage provider log entry.
    /// </summary>
    public class StorageProviderLog
    {
        public string ProviderName { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a performance log entry.
    /// </summary>
    public class PerformanceLog
    {
        public string Operation { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public long? FileSizeBytes { get; set; }
    }

    /// <summary>
    /// Represents a storage error log entry.
    /// </summary>
    public class StorageErrorLog
    {
        public string Operation { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public Exception Exception { get; set; } = new Exception();
        public string? StorageType { get; set; }
    }
}
