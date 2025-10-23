using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Logging.Configuration;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Factory implementation for creating document loggers
    /// </summary>
    public class DocumentLoggerFactory : IDocumentLoggerFactory
    {
        private readonly IDocumentLoggerProvider _provider;
        private readonly DocumentLoggingConfiguration _configuration;
        private readonly Dictionary<string, IDocumentLogger> _loggers = new();

        public DocumentLoggerFactory(IDocumentLoggerProvider provider, DocumentLoggingConfiguration configuration)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IDocumentLogger CreateLogger(string category)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentException("Category cannot be null or empty", nameof(category));

            if (_loggers.TryGetValue(category, out var existingLogger))
                return existingLogger;

            var logger = _provider.CreateLogger(category);
            _loggers[category] = logger;
            return logger;
        }

        public IDocumentLogger CreateLogger<T>()
        {
            return CreateLogger(typeof(T).Name);
        }

        public IDocumentLogger CreateLogger(string category, string documentId, DocumentType documentType)
        {
            var logger = CreateLogger(category);
            return logger.WithDocumentContext(documentId, documentType);
        }

        /// <summary>
        /// Disposes all created loggers
        /// </summary>
        public void Dispose()
        {
            foreach (var logger in _loggers.Values)
            {
                if (logger is IDisposable disposableLogger)
                {
                    disposableLogger.Dispose();
                }
            }
            _loggers.Clear();
        }
    }
}
