using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Base document logger implementation with common functionality
    /// </summary>
    public abstract class BaseDocumentLogger : IDocumentLogger
    {
        protected readonly string _category;
        protected readonly IDocumentLoggerProvider _provider;
        protected readonly DocumentLoggingConfiguration _configuration;
        protected readonly IDictionary<string, object> _context;

        protected BaseDocumentLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration)
        {
            _category = category ?? throw new ArgumentNullException(nameof(category));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = new Dictionary<string, object>();
        }

        public virtual void LogDocumentOperation(string operation, DocumentType documentType, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"Document operation: {operation}", null, context);
            logEntry.DocumentType = documentType;
            logEntry.Operation = operation;
            WriteLog(logEntry);
        }

        public virtual void LogDocumentCreation(string documentType, TimeSpan duration, object properties = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"Document created: {documentType}", null, properties);
            logEntry.Duration = duration;
            logEntry.Operation = "DocumentCreation";
            WriteLog(logEntry);
        }

        public virtual void LogContentOperation(string operation, string contentType, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Content operation: {operation}", null, context);
            logEntry.Operation = operation;
            logEntry.Properties["ContentType"] = contentType;
            WriteLog(logEntry);
        }

        public virtual void LogFormattingOperation(string operation, object styleContext = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Formatting operation: {operation}", null, styleContext);
            logEntry.Operation = operation;
            WriteLog(logEntry);
        }

        public virtual void LogFileOperation(string operation, string filePath, long fileSizeBytes = 0)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"File operation: {operation}", null, null);
            logEntry.Operation = operation;
            logEntry.FilePath = filePath;
            logEntry.FileSizeBytes = fileSizeBytes;
            WriteLog(logEntry);
        }

        public virtual void LogProviderOperation(string providerName, string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"Provider operation: {operation}", null, context);
            logEntry.Operation = operation;
            logEntry.Provider = providerName;
            WriteLog(logEntry);
        }

        public virtual void LogDocumentPerformance(string operation, TimeSpan duration, object metrics = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"Performance: {operation} completed in {duration.TotalMilliseconds}ms", null, metrics);
            logEntry.Operation = operation;
            logEntry.Duration = duration;
            WriteLog(logEntry);
        }

        public virtual void LogDocumentError(string operation, Exception exception, DocumentType documentType, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Error)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Error, $"Error in {operation}: {exception?.Message}", exception, context);
            logEntry.Operation = operation;
            logEntry.DocumentType = documentType;
            WriteLog(logEntry);
        }

        public virtual IDocumentLogger WithDocumentContext(string documentId, DocumentType documentType)
        {
            var newContext = new Dictionary<string, object>(_context)
            {
                ["DocumentId"] = documentId,
                ["DocumentType"] = documentType.ToString()
            };
            return CreateScopedLogger(newContext);
        }

        public virtual IDocumentLogger WithOperationContext(string operationId, string operationName)
        {
            var newContext = new Dictionary<string, object>(_context)
            {
                ["OperationId"] = operationId,
                ["OperationName"] = operationName
            };
            return CreateScopedLogger(newContext);
        }

        protected virtual bool IsEnabled(DocumentLogLevel level)
        {
            return level >= _configuration.MinimumLevel;
        }

        protected virtual DocumentLogEntry CreateDocumentLogEntry(DocumentLogLevel level, string message, Exception exception, object context)
        {
            var logEntry = new DocumentLogEntry
            {
                Level = level,
                Message = message,
                Category = _category,
                Exception = exception,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                Application = "AMCode.Documents",
                Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString()
            };

            // Add context information
            foreach (var kvp in _context)
            {
                logEntry.Properties[kvp.Key] = kvp.Value;
            }

            // Add correlation ID if enabled
            if (_configuration.EnableCorrelationId)
            {
                logEntry.CorrelationId = GetCorrelationId();
            }

            return logEntry;
        }

        protected abstract void WriteLog(DocumentLogEntry logEntry);
        protected abstract IDocumentLogger CreateScopedLogger(IDictionary<string, object> context);

        private string GetCorrelationId()
        {
            return Activity.Current?.Id ?? Guid.NewGuid().ToString();
        }
    }
}
