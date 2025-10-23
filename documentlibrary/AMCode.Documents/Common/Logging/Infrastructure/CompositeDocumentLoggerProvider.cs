using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Logging.Configuration;

namespace AMCode.Documents.Common.Logging.Infrastructure
{
    /// <summary>
    /// Composite document logger provider that manages multiple logger providers
    /// </summary>
    public class CompositeDocumentLoggerProvider : IDocumentLoggerProvider
    {
        private readonly List<IDocumentLoggerProvider> _providers = new();
        private readonly DocumentLoggingConfiguration _configuration;
        private DocumentLogLevel _minimumLevel = DocumentLogLevel.Information;

        public CompositeDocumentLoggerProvider(DocumentLoggingConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Adds a logger provider to the composite
        /// </summary>
        public void AddProvider(IDocumentLoggerProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            _providers.Add(provider);
        }

        /// <summary>
        /// Removes a logger provider from the composite
        /// </summary>
        public void RemoveProvider(IDocumentLoggerProvider provider)
        {
            _providers.Remove(provider);
        }

        public IDocumentLogger CreateLogger(string category)
        {
            if (string.IsNullOrEmpty(category))
                throw new ArgumentException("Category cannot be null or empty", nameof(category));

            // For now, return a console logger as the default implementation
            // In a more sophisticated implementation, this would create a composite logger
            // that writes to all providers
            return new ConsoleDocumentLogger(category, this, _configuration, new ConsoleDocumentLoggerConfiguration());
        }

        public void Dispose()
        {
            foreach (var provider in _providers)
            {
                provider?.Dispose();
            }
            _providers.Clear();
        }

        public void SetMinimumLevel(DocumentLogLevel level)
        {
            _minimumLevel = level;
            foreach (var provider in _providers)
            {
                provider.SetMinimumLevel(level);
            }
        }

        public DocumentLogLevel GetMinimumLevel()
        {
            return _minimumLevel;
        }
    }
}
