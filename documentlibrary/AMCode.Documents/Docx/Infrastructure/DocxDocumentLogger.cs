using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Logging;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Infrastructure;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Docx.Infrastructure
{
    /// <summary>
    /// DOCX-specific logger implementation
    /// </summary>
    public class DocxDocumentLogger : BaseDocumentLogger, AMCode.Docx.Infrastructure.Interfaces.IDocumentLogger
    {
        public DocxDocumentLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration)
            : base(category, provider, configuration)
        {
        }

        public void LogDocxOperation(string operation, object context = null)
        {
            LogDocumentOperation(operation, DocumentType.Docx, context);
        }

        public void LogParagraphOperation(string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Paragraph operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Docx;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Paragraph";
            WriteLog(logEntry);
        }

        public void LogTableOperation(string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Table operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Docx;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Table";
            WriteLog(logEntry);
        }

        public void LogSectionOperation(string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Section operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Docx;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Section";
            WriteLog(logEntry);
        }

        public void LogOpenXmlOperation(string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"OpenXML operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Docx;
            logEntry.Operation = operation;
            logEntry.Provider = "OpenXML";
            logEntry.Properties["OperationType"] = "OpenXML";
            WriteLog(logEntry);
        }

        // Legacy methods for backward compatibility
        public void LogDocumentOperation(string operation, object parameters = null)
        {
            LogDocumentOperation(operation, DocumentType.Docx, parameters);
        }

        public void LogError(string operation, Exception exception)
        {
            LogDocumentError(operation, exception, DocumentType.Docx);
        }

        public void LogPerformance(string operation, TimeSpan duration)
        {
            LogDocumentPerformance(operation, duration);
        }

        public void LogWarning(string operation, string message)
        {
            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Warning, message, null, null);
            logEntry.DocumentType = DocumentType.Docx;
            logEntry.Operation = operation;
            WriteLog(logEntry);
        }

        protected override void WriteLog(DocumentLogEntry logEntry)
        {
            // Implement DOCX-specific logging
            Console.WriteLine($"[DOCX] {logEntry.Level}: {logEntry.Message}");
            if (logEntry.Exception != null)
            {
                Console.WriteLine($"[DOCX] Exception: {logEntry.Exception}");
            }
        }

        protected override IDocumentLogger CreateScopedLogger(IDictionary<string, object> context)
        {
            var logger = new DocxDocumentLogger(_category, _provider, _configuration);
            // Copy context to the new logger
            foreach (var kvp in context)
            {
                logger._context[kvp.Key] = kvp.Value;
            }
            return logger;
        }
    }
}
