using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Logging;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Infrastructure;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Pdf.Infrastructure
{
    /// <summary>
    /// PDF-specific logger implementation
    /// </summary>
    public class PdfDocumentLogger : BaseDocumentLogger, IPdfLogger
    {
        public PdfDocumentLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration)
            : base(category, provider, configuration)
        {
        }

        // Implement IPdfLogger methods using base functionality
        public void LogDocumentOperation(string operation, object context = null)
        {
            LogDocumentOperation(operation, DocumentType.Pdf, context);
        }

        public void LogError(string operation, Exception exception)
        {
            LogDocumentError(operation, exception, DocumentType.Pdf);
        }

        public void LogWarning(string message, object context = null)
        {
            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Warning, message, null, context);
            logEntry.DocumentType = DocumentType.Pdf;
            WriteLog(logEntry);
        }

        public void LogInformation(string message, object context = null)
        {
            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, message, null, context);
            logEntry.DocumentType = DocumentType.Pdf;
            WriteLog(logEntry);
        }

        public void LogDebug(string message, object context = null)
        {
            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, message, null, context);
            logEntry.DocumentType = DocumentType.Pdf;
            WriteLog(logEntry);
        }

        public void LogPdfOperation(string operation, object context = null)
        {
            LogDocumentOperation(operation, DocumentType.Pdf, context);
        }

        public void LogPageOperation(string operation, int pageNumber, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Page operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Pdf;
            logEntry.Operation = operation;
            logEntry.Properties["PageNumber"] = pageNumber;
            WriteLog(logEntry);
        }

        public void LogContentOperation(string operation, string contentType, object context = null)
        {
            LogContentOperation(operation, contentType, context);
        }

        public void LogProviderOperation(string providerName, string operation, object context = null)
        {
            LogProviderOperation(providerName, operation, context);
        }

        protected override void WriteLog(DocumentLogEntry logEntry)
        {
            // Implement PDF-specific logging
            Console.WriteLine($"[PDF] {logEntry.Level}: {logEntry.Message}");
            if (logEntry.Exception != null)
            {
                Console.WriteLine($"[PDF] Exception: {logEntry.Exception}");
            }
        }

        protected override IDocumentLogger CreateScopedLogger(IDictionary<string, object> context)
        {
            var logger = new PdfDocumentLogger(_category, _provider, _configuration);
            // Copy context to the new logger
            foreach (var kvp in context)
            {
                logger._context[kvp.Key] = kvp.Value;
            }
            return logger;
        }
    }
}
