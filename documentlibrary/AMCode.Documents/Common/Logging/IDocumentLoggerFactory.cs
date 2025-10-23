using System;

namespace AMCode.Documents.Common.Logging
{
    /// <summary>
    /// Factory interface for creating document loggers
    /// </summary>
    public interface IDocumentLoggerFactory
    {
        /// <summary>
        /// Creates a logger for the specified category
        /// </summary>
        IDocumentLogger CreateLogger(string category);
        
        /// <summary>
        /// Creates a logger for the specified type
        /// </summary>
        IDocumentLogger CreateLogger<T>();
        
        /// <summary>
        /// Creates a logger with document context
        /// </summary>
        IDocumentLogger CreateLogger(string category, string documentId, DocumentType documentType);
    }
}
