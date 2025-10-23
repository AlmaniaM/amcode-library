using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// File logger configuration
    /// </summary>
    public class FileDocumentLoggerConfiguration
    {
        public bool EnableRotation { get; set; } = true;
        public string DateFormat { get; set; } = "yyyy-MM-dd";
        public bool EnableCompression { get; set; } = true;
        public bool EnableAsyncWriting { get; set; } = true;
        public int BufferSizeKB { get; set; } = 64;
        public string LogDirectory { get; set; } = "logs";
        public string LogFileName { get; set; } = "documents.log";
    }

    /// <summary>
    /// File logger implementation for document operations
    /// </summary>
    public class FileDocumentLogger : BaseDocumentLogger
    {
        private readonly FileDocumentLoggerConfiguration _fileConfig;
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        public FileDocumentLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration, FileDocumentLoggerConfiguration fileConfig)
            : base(category, provider, configuration)
        {
            _fileConfig = fileConfig ?? throw new ArgumentNullException(nameof(fileConfig));
            _logFilePath = Path.Combine(_fileConfig.LogDirectory, _fileConfig.LogFileName);
            
            // Ensure log directory exists
            Directory.CreateDirectory(_fileConfig.LogDirectory);
        }

        protected override void WriteLog(DocumentLogEntry logEntry)
        {
            if (!_configuration.EnableFile) return;

            var logMessage = FormatLogMessage(logEntry);
            
            if (_fileConfig.EnableAsyncWriting)
            {
                Task.Run(() => WriteToFileAsync(logMessage));
            }
            else
            {
                WriteToFile(logMessage);
            }
        }

        protected override IDocumentLogger CreateScopedLogger(IDictionary<string, object> context)
        {
            var logger = new FileDocumentLogger(_category, _provider, _configuration, _fileConfig);
            // Copy context to the new logger
            foreach (var kvp in context)
            {
                logger._context[kvp.Key] = kvp.Value;
            }
            return logger;
        }

        private string FormatLogMessage(DocumentLogEntry logEntry)
        {
            if (_configuration.EnableStructuredLogging)
            {
                return logEntry.ToJson();
            }
            else
            {
                var timestamp = logEntry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var level = logEntry.Level.ToString().ToUpper().PadRight(5);
                var category = $"[{logEntry.Category}]";
                var documentType = logEntry.DocumentType != DocumentType.Unknown ? $" [{logEntry.DocumentType}]" : "";
                var correlationId = string.IsNullOrEmpty(logEntry.CorrelationId) ? "" : $" [{logEntry.CorrelationId}]";
                
                return $"{timestamp} {level} {category}{documentType}{correlationId}: {logEntry.Message}";
            }
        }

        private void WriteToFile(string message)
        {
            lock (_lockObject)
            {
                try
                {
                    // Check if file rotation is needed
                    if (_fileConfig.EnableRotation && ShouldRotateFile())
                    {
                        RotateFile();
                    }

                    File.AppendAllText(_logFilePath, message + Environment.NewLine, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    // Fallback to console if file writing fails
                    Console.WriteLine($"[ERROR] Failed to write to log file: {ex.Message}");
                    Console.WriteLine($"[FALLBACK] {message}");
                }
            }
        }

        private async Task WriteToFileAsync(string message)
        {
            try
            {
                // Check if file rotation is needed
                if (_fileConfig.EnableRotation && ShouldRotateFile())
                {
                    RotateFile();
                }

                await File.AppendAllTextAsync(_logFilePath, message + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // Fallback to console if file writing fails
                Console.WriteLine($"[ERROR] Failed to write to log file: {ex.Message}");
                Console.WriteLine($"[FALLBACK] {message}");
            }
        }

        private bool ShouldRotateFile()
        {
            if (!File.Exists(_logFilePath))
                return false;

            var fileInfo = new FileInfo(_logFilePath);
            return fileInfo.Length > (_configuration.MaxFileSizeMB * 1024 * 1024);
        }

        private void RotateFile()
        {
            if (!File.Exists(_logFilePath))
                return;

            var timestamp = DateTime.Now.ToString(_fileConfig.DateFormat);
            var rotatedFileName = $"{Path.GetFileNameWithoutExtension(_logFilePath)}_{timestamp}{Path.GetExtension(_logFilePath)}";
            var rotatedFilePath = Path.Combine(_fileConfig.LogDirectory, rotatedFileName);

            // Move current file to rotated name
            File.Move(_logFilePath, rotatedFilePath);

            // Compress if enabled
            if (_fileConfig.EnableCompression)
            {
                CompressFile(rotatedFilePath);
            }

            // Clean up old files
            CleanupOldFiles();
        }

        private void CompressFile(string filePath)
        {
            try
            {
                // Simple compression using GZip
                var compressedPath = filePath + ".gz";
                using (var sourceStream = File.OpenRead(filePath))
                using (var compressedStream = File.Create(compressedPath))
                using (var gzipStream = new System.IO.Compression.GZipStream(compressedStream, System.IO.Compression.CompressionMode.Compress))
                {
                    sourceStream.CopyTo(gzipStream);
                }
                
                // Remove original file
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Failed to compress log file {filePath}: {ex.Message}");
            }
        }

        private void CleanupOldFiles()
        {
            try
            {
                var logFiles = Directory.GetFiles(_fileConfig.LogDirectory, "*.log*")
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(_configuration.MaxFiles)
                    .ToList();

                foreach (var file in logFiles)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] Failed to cleanup old log files: {ex.Message}");
            }
        }
    }
}
