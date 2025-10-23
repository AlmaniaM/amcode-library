using System;
using AMCode.Documents.Common.Logging.Configuration;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Console document logger provider
    /// </summary>
    public class ConsoleDocumentLoggerProvider : IDocumentLoggerProvider
    {
        private readonly DocumentLoggingConfiguration _configuration;
        private readonly ConsoleDocumentLoggerConfiguration _consoleConfig;
        private DocumentLogLevel _minimumLevel = DocumentLogLevel.Information;

        public ConsoleDocumentLoggerProvider(DocumentLoggingConfiguration configuration, ConsoleDocumentLoggerConfiguration consoleConfig)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _consoleConfig = consoleConfig ?? throw new ArgumentNullException(nameof(consoleConfig));
        }

        public IDocumentLogger CreateLogger(string category)
        {
            return new ConsoleDocumentLogger(category, this, _configuration, _consoleConfig);
        }

        public void Dispose()
        {
            // Nothing to dispose for console logger
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
