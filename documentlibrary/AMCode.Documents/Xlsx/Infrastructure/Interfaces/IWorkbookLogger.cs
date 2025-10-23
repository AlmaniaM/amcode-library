using System;
using AMCode.Documents.Common.Logging;

namespace AMCode.Documents.Xlsx.Infrastructure.Interfaces
{
    /// <summary>
    /// Infrastructure interface for workbook logging operations
    /// Provides logging capabilities for workbook operations
    /// </summary>
    public interface IWorkbookLogger : IDocumentLogger
    {
        /// <summary>
        /// Logs an information message (legacy method for backward compatibility)
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="workbookId">The workbook ID</param>
        void LogInformation(string message, Guid workbookId);

        /// <summary>
        /// Logs a warning message (legacy method for backward compatibility)
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="workbookId">The workbook ID</param>
        void LogWarning(string message, Guid workbookId);

        /// <summary>
        /// Logs an error message (legacy method for backward compatibility)
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception that occurred</param>
        /// <param name="workbookId">The workbook ID</param>
        void LogError(string message, Exception exception, Guid workbookId);

        /// <summary>
        /// Logs a workbook operation (legacy method for backward compatibility)
        /// </summary>
        /// <param name="operation">The operation description</param>
        /// <param name="workbookId">The workbook ID</param>
        void LogWorkbookOperation(string operation, Guid workbookId);
        
        /// <summary>
        /// Logs worksheet operations
        /// </summary>
        void LogWorksheetOperation(string operation, string worksheetName, Guid workbookId, object context = null);
        
        /// <summary>
        /// Logs cell operations
        /// </summary>
        void LogCellOperation(string operation, string cellAddress, Guid workbookId, object context = null);
        
        /// <summary>
        /// Logs range operations
        /// </summary>
        void LogRangeOperation(string operation, string rangeAddress, Guid workbookId, object context = null);
        
        /// <summary>
        /// Logs styling operations
        /// </summary>
        void LogStylingOperation(string operation, Guid workbookId, object context = null);
    }
}