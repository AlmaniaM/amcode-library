using System;

namespace AMCode.Documents.Common.Logging
{
    /// <summary>
    /// Unified logging interface for all document operations
    /// </summary>
    public interface IDocumentLogger
    {
        /// <summary>
        /// Logs a document operation with context
        /// </summary>
        void LogDocumentOperation(string operation, DocumentType documentType, object context = null);
        
        /// <summary>
        /// Logs document creation with performance metrics
        /// </summary>
        void LogDocumentCreation(string documentType, TimeSpan duration, object properties = null);
        
        /// <summary>
        /// Logs document content operations
        /// </summary>
        void LogContentOperation(string operation, string contentType, object context = null);
        
        /// <summary>
        /// Logs document formatting operations
        /// </summary>
        void LogFormattingOperation(string operation, object styleContext = null);
        
        /// <summary>
        /// Logs file operations (save, load, export)
        /// </summary>
        void LogFileOperation(string operation, string filePath, long fileSizeBytes = 0);
        
        /// <summary>
        /// Logs provider-specific operations
        /// </summary>
        void LogProviderOperation(string providerName, string operation, object context = null);
        
        /// <summary>
        /// Logs document performance metrics
        /// </summary>
        void LogDocumentPerformance(string operation, TimeSpan duration, object metrics = null);
        
        /// <summary>
        /// Logs document errors with context
        /// </summary>
        void LogDocumentError(string operation, Exception exception, DocumentType documentType, object context = null);
        
        /// <summary>
        /// Creates a scoped logger with document context
        /// </summary>
        IDocumentLogger WithDocumentContext(string documentId, DocumentType documentType);
        
        /// <summary>
        /// Creates a scoped logger with operation context
        /// </summary>
        IDocumentLogger WithOperationContext(string operationId, string operationName);
    }
}
