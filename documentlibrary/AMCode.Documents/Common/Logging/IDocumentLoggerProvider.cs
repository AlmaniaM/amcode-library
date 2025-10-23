using System;

namespace AMCode.Documents.Common.Logging
{
    /// <summary>
    /// Provider interface for document logging infrastructure
    /// </summary>
    public interface IDocumentLoggerProvider
    {
        /// <summary>
        /// Creates a logger for the specified category
        /// </summary>
        IDocumentLogger CreateLogger(string category);
        
        /// <summary>
        /// Disposes the provider and releases resources
        /// </summary>
        void Dispose();
        
        /// <summary>
        /// Sets the minimum log level for the provider
        /// </summary>
        void SetMinimumLevel(DocumentLogLevel level);
        
        /// <summary>
        /// Gets the current minimum log level
        /// </summary>
        DocumentLogLevel GetMinimumLevel();
    }
}
