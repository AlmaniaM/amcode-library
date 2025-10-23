using System;
using AMCode.Documents.Common.Logging;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF logging interface that extends the unified document logger
    /// </summary>
    public interface IPdfLogger : IDocumentLogger
    {
        /// <summary>
        /// Log document operation (legacy method for backward compatibility)
        /// </summary>
        void LogDocumentOperation(string operation, object context = null);
        
        /// <summary>
        /// Log error (legacy method for backward compatibility)
        /// </summary>
        void LogError(string operation, Exception exception);
        
        /// <summary>
        /// Log warning (legacy method for backward compatibility)
        /// </summary>
        void LogWarning(string message, object context = null);
        
        /// <summary>
        /// Log information (legacy method for backward compatibility)
        /// </summary>
        void LogInformation(string message, object context = null);
        
        /// <summary>
        /// Log debug information (legacy method for backward compatibility)
        /// </summary>
        void LogDebug(string message, object context = null);
        
        /// <summary>
        /// Log PDF-specific operations
        /// </summary>
        void LogPdfOperation(string operation, object context = null);
        
        /// <summary>
        /// Log page operations
        /// </summary>
        void LogPageOperation(string operation, int pageNumber, object context = null);
        
        /// <summary>
        /// Log content operations
        /// </summary>
        void LogContentOperation(string operation, string contentType, object context = null);
        
        /// <summary>
        /// Log provider operations (QuestPDF, iTextSharp)
        /// </summary>
        void LogProviderOperation(string providerName, string operation, object context = null);
    }
}
