using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Manages log file operations
    /// </summary>
    public class DocumentLogFileManager
    {
        private readonly string _logDirectory;
        private readonly object _lockObject = new object();

        public DocumentLogFileManager(string logDirectory = "logs")
        {
            _logDirectory = logDirectory;
            Directory.CreateDirectory(_logDirectory);
        }

        /// <summary>
        /// Writes a log entry to file
        /// </summary>
        public async Task WriteLogEntryAsync(DocumentLogEntry logEntry, string fileName = "documents.log")
        {
            var logPath = Path.Combine(_logDirectory, fileName);
            var logMessage = FormatLogEntry(logEntry);
            
            await File.AppendAllTextAsync(logPath, logMessage + Environment.NewLine, Encoding.UTF8);
        }

        /// <summary>
        /// Writes a log entry to file synchronously
        /// </summary>
        public void WriteLogEntry(DocumentLogEntry logEntry, string fileName = "documents.log")
        {
            var logPath = Path.Combine(_logDirectory, fileName);
            var logMessage = FormatLogEntry(logEntry);
            
            lock (_lockObject)
            {
                File.AppendAllText(logPath, logMessage + Environment.NewLine, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Rotates log files if needed
        /// </summary>
        public void RotateLogFileIfNeeded(string fileName = "documents.log", long maxSizeBytes = 100 * 1024 * 1024)
        {
            var logPath = Path.Combine(_logDirectory, fileName);
            
            if (!File.Exists(logPath))
                return;

            var fileInfo = new FileInfo(logPath);
            if (fileInfo.Length <= maxSizeBytes)
                return;

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var rotatedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{Path.GetExtension(fileName)}";
            var rotatedPath = Path.Combine(_logDirectory, rotatedFileName);

            lock (_lockObject)
            {
                File.Move(logPath, rotatedPath);
            }
        }

        /// <summary>
        /// Cleans up old log files
        /// </summary>
        public void CleanupOldLogFiles(int maxFiles = 10)
        {
            try
            {
                var logFiles = Directory.GetFiles(_logDirectory, "*.log*")
                    .Select(f => new FileInfo(f))
                    .OrderByDescending(f => f.CreationTime)
                    .Skip(maxFiles)
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

        private string FormatLogEntry(DocumentLogEntry logEntry)
        {
            var timestamp = logEntry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var level = logEntry.Level.ToString().ToUpper().PadRight(5);
            var category = $"[{logEntry.Category}]";
            var documentType = logEntry.DocumentType != DocumentType.Unknown ? $" [{logEntry.DocumentType}]" : "";
            var correlationId = string.IsNullOrEmpty(logEntry.CorrelationId) ? "" : $" [{logEntry.CorrelationId}]";
            
            return $"{timestamp} {level} {category}{documentType}{correlationId}: {logEntry.Message}";
        }
    }
}
