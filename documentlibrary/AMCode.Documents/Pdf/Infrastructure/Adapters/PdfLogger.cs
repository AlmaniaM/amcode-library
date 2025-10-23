using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Logging;
using AMCode.Documents.Common.Logging.Configuration;
using AMCode.Documents.Common.Logging.Infrastructure;
using AMCode.Documents.Common.Logging.Models;
using AMCode.Documents.Pdf.Infrastructure;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF logger implementation using the new unified logging infrastructure
    /// </summary>
    public class PdfLogger : IPdfLogger
    {
        private readonly PdfDocumentLogger _documentLogger;

        /// <summary>
        /// Create PDF logger with default configuration
        /// </summary>
        public PdfLogger(string category = "AMCode.Pdf")
        {
            var configuration = new DocumentLoggingConfiguration();
            var provider = new CompositeDocumentLoggerProvider(configuration);
            var consoleConfig = new ConsoleDocumentLoggerConfiguration();
            
            // Add console provider
            provider.AddProvider(new ConsoleDocumentLoggerProvider(configuration, consoleConfig));
            
            _documentLogger = new PdfDocumentLogger(category, provider, configuration);
        }

        /// <summary>
        /// Create PDF logger with custom configuration
        /// </summary>
        public PdfLogger(string category, IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration)
        {
            _documentLogger = new PdfDocumentLogger(category, provider, configuration);
        }

        // Delegate all methods to the document logger
        public void LogDocumentOperation(string operation, DocumentType documentType, object context = null)
        {
            _documentLogger.LogDocumentOperation(operation, documentType, context);
        }

        public void LogDocumentCreation(string documentType, TimeSpan duration, object properties = null)
        {
            _documentLogger.LogDocumentCreation(documentType, duration, properties);
        }

        public void LogContentOperation(string operation, string contentType, object context = null)
        {
            _documentLogger.LogContentOperation(operation, contentType, context);
        }

        public void LogFormattingOperation(string operation, object styleContext = null)
        {
            _documentLogger.LogFormattingOperation(operation, styleContext);
        }

        public void LogFileOperation(string operation, string filePath, long fileSizeBytes = 0)
        {
            _documentLogger.LogFileOperation(operation, filePath, fileSizeBytes);
        }

        public void LogProviderOperation(string providerName, string operation, object context = null)
        {
            _documentLogger.LogProviderOperation(providerName, operation, context);
        }

        public void LogDocumentPerformance(string operation, TimeSpan duration, object metrics = null)
        {
            _documentLogger.LogDocumentPerformance(operation, duration, metrics);
        }

        public void LogDocumentError(string operation, Exception exception, DocumentType documentType, object context = null)
        {
            _documentLogger.LogDocumentError(operation, exception, documentType, context);
        }

        public IDocumentLogger WithDocumentContext(string documentId, DocumentType documentType)
        {
            return _documentLogger.WithDocumentContext(documentId, documentType);
        }

        public IDocumentLogger WithOperationContext(string operationId, string operationName)
        {
            return _documentLogger.WithOperationContext(operationId, operationName);
        }

        // Legacy methods for backward compatibility
        public void LogDocumentOperation(string operation, object context = null)
        {
            _documentLogger.LogDocumentOperation(operation, context);
        }

        public void LogError(string operation, Exception exception)
        {
            _documentLogger.LogError(operation, exception);
        }

        public void LogWarning(string message, object context = null)
        {
            _documentLogger.LogWarning(message, context);
        }

        public void LogInformation(string message, object context = null)
        {
            _documentLogger.LogInformation(message, context);
        }

        public void LogDebug(string message, object context = null)
        {
            _documentLogger.LogDebug(message, context);
        }

        public void LogPdfOperation(string operation, object context = null)
        {
            _documentLogger.LogPdfOperation(operation, context);
        }

        public void LogPageOperation(string operation, int pageNumber, object context = null)
        {
            _documentLogger.LogPageOperation(operation, pageNumber, context);
        }

    }
}
