using System;
using AMCode.Documents.Common.Logging.Configuration;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Structured document logger provider
    /// </summary>
    public class StructuredDocumentLoggerProvider : IDocumentLoggerProvider
    {
        private readonly DocumentLoggingConfiguration _configuration;
        private readonly StructuredDocumentLoggerConfiguration _structuredConfig;
        private DocumentLogLevel _minimumLevel = DocumentLogLevel.Information;

        public StructuredDocumentLoggerProvider(DocumentLoggingConfiguration configuration, StructuredDocumentLoggerConfiguration structuredConfig)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _structuredConfig = structuredConfig ?? throw new ArgumentNullException(nameof(structuredConfig));
        }

        public IDocumentLogger CreateLogger(string category)
        {
            return new StructuredDocumentLogger(category, this, _configuration, _structuredConfig);
        }

        public void Dispose()
        {
            // Nothing to dispose for structured logger
        }

        public void SetMinimumLevel(DocumentLogLevel level)
        {
            _minimumLevel = level;
        }

        public DocumentLogLevel GetMinimumLevel()
        {
            return _minimumLevel;
        }
    }
}
