using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Logging;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Infrastructure;
using AMCode.Documents.Common.Logging.Models;

namespace AMCode.Documents.Excel.Infrastructure
{
    /// <summary>
    /// Excel-specific logger implementation
    /// </summary>
    public class ExcelDocumentLogger : BaseDocumentLogger, IExcelLogger
    {
        public ExcelDocumentLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration)
            : base(category, provider, configuration)
        {
        }

        public void LogWorkbookOperation(string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"Workbook operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Excel;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Workbook";
            WriteLog(logEntry);
        }

        public void LogWorksheetOperation(string operation, string worksheetName, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"Worksheet operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Excel;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Worksheet";
            logEntry.Properties["WorksheetName"] = worksheetName;
            WriteLog(logEntry);
        }

        public void LogCellOperation(string operation, string cellAddress, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Cell operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Excel;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Cell";
            logEntry.Properties["CellAddress"] = cellAddress;
            WriteLog(logEntry);
        }

        public void LogRangeOperation(string operation, string rangeAddress, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Range operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Excel;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Range";
            logEntry.Properties["RangeAddress"] = rangeAddress;
            WriteLog(logEntry);
        }

        public void LogStylingOperation(string operation, object styleContext = null)
        {
            if (!IsEnabled(DocumentLogLevel.Debug)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Debug, $"Styling operation: {operation}", null, styleContext);
            logEntry.DocumentType = DocumentType.Excel;
            logEntry.Operation = operation;
            logEntry.Properties["OperationType"] = "Styling";
            WriteLog(logEntry);
        }

        public void LogSyncfusionOperation(string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"Syncfusion operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Excel;
            logEntry.Operation = operation;
            logEntry.Provider = "Syncfusion";
            logEntry.Properties["OperationType"] = "Syncfusion";
            WriteLog(logEntry);
        }

        public void LogOpenXmlOperation(string operation, object context = null)
        {
            if (!IsEnabled(DocumentLogLevel.Information)) return;

            var logEntry = CreateDocumentLogEntry(DocumentLogLevel.Information, $"OpenXML operation: {operation}", null, context);
            logEntry.DocumentType = DocumentType.Excel;
            logEntry.Operation = operation;
            logEntry.Provider = "OpenXML";
            logEntry.Properties["OperationType"] = "OpenXML";
            WriteLog(logEntry);
        }

        protected override void WriteLog(DocumentLogEntry logEntry)
        {
            // Implement Excel-specific logging
            Console.WriteLine($"[EXCEL] {logEntry.Level}: {logEntry.Message}");
            if (logEntry.Exception != null)
            {
                Console.WriteLine($"[EXCEL] Exception: {logEntry.Exception}");
            }
        }

        protected override IDocumentLogger CreateScopedLogger(IDictionary<string, object> context)
        {
            var logger = new ExcelDocumentLogger(_category, _provider, _configuration);
            // Copy context to the new logger
            foreach (var kvp in context)
            {
                logger._context[kvp.Key] = kvp.Value;
            }
            return logger;
        }
    }
}
