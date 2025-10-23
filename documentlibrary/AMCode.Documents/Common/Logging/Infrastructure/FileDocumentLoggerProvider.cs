using System;
using AMCode.Documents.Common.Logging.Configuration;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// File document logger provider
    /// </summary>
    public class FileDocumentLoggerProvider : IDocumentLoggerProvider
    {
        private readonly DocumentLoggingConfiguration _configuration;
        private readonly FileDocumentLoggerConfiguration _fileConfig;
        private DocumentLogLevel _minimumLevel = DocumentLogLevel.Information;

        public FileDocumentLoggerProvider(DocumentLoggingConfiguration configuration, FileDocumentLoggerConfiguration fileConfig)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _fileConfig = fileConfig ?? throw new ArgumentNullException(nameof(fileConfig));
        }

        public IDocumentLogger CreateLogger(string category)
        {
            return new FileDocumentLogger(category, this, _configuration, _fileConfig);
        }

        public void Dispose()
        {
            // Nothing to dispose for file logger
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
