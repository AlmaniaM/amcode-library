using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Documents.Common.Logging;

namespace AMCode.Docx.Infrastructure.Interfaces
{
    /// <summary>
    /// Infrastructure interface for document logging that extends the unified document logger
    /// </summary>
    public interface IDocumentLogger : AMCode.Documents.Common.Logging.IDocumentLogger
    {
        /// <summary>
        /// Log a document operation (legacy method for backward compatibility)
        /// </summary>
        /// <param name="operation">The operation being performed</param>
        /// <param name="parameters">Optional parameters for the operation</param>
        void LogDocumentOperation(string operation, object parameters = null);

        /// <summary>
        /// Log an error (legacy method for backward compatibility)
        /// </summary>
        /// <param name="operation">The operation that failed</param>
        /// <param name="exception">The exception that occurred</param>
        void LogError(string operation, Exception exception);

        /// <summary>
        /// Log performance information (legacy method for backward compatibility)
        /// </summary>
        /// <param name="operation">The operation being measured</param>
        /// <param name="duration">The duration of the operation</param>
        void LogPerformance(string operation, TimeSpan duration);

        /// <summary>
        /// Log a warning (legacy method for backward compatibility)
        /// </summary>
        /// <param name="operation">The operation that generated the warning</param>
        /// <param name="message">The warning message</param>
        void LogWarning(string operation, string message);
        
        /// <summary>
        /// Log DOCX-specific operations
        /// </summary>
        void LogDocxOperation(string operation, object context = null);
        
        /// <summary>
        /// Log paragraph operations
        /// </summary>
        void LogParagraphOperation(string operation, object context = null);
        
        /// <summary>
        /// Log table operations
        /// </summary>
        void LogTableOperation(string operation, object context = null);
        
        /// <summary>
        /// Log section operations
        /// </summary>
        void LogSectionOperation(string operation, object context = null);
        
        /// <summary>
        /// Log OpenXML-specific operations
        /// </summary>
        void LogOpenXmlOperation(string operation, object context = null);
    }
}
