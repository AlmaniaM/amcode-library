using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF provider factory
    /// </summary>
    public class PdfProviderFactory
    {
        private readonly Dictionary<string, Func<IPdfLogger, IPdfValidator, IPdfProvider>> _providerFactories;
        private readonly IPdfLogger _logger;

        /// <summary>
        /// Create PDF provider factory
        /// </summary>
        public PdfProviderFactory(IPdfLogger logger = null)
        {
            _logger = logger ?? new PdfLogger();
            _providerFactories = new Dictionary<string, Func<IPdfLogger, IPdfValidator, IPdfProvider>>();
            
            // Register default providers
            RegisterProvider("QuestPDF", (logger, validator) => new QuestPdfProvider(logger, validator));
            RegisterProvider("iTextSharp", (logger, validator) => new iTextSharpProvider(logger, validator));
        }

        /// <summary>
        /// Register a PDF provider
        /// </summary>
        public void RegisterProvider(string name, Func<IPdfLogger, IPdfValidator, IPdfProvider> factory)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Provider name cannot be null or empty", nameof(name));
            
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _providerFactories[name] = factory;
            _logger.LogInformation($"Registered PDF provider: {name}");
        }

        /// <summary>
        /// Create a PDF provider
        /// </summary>
        public Result<IPdfProvider> CreateProvider(string name, IPdfValidator validator = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<IPdfProvider>.Failure("Provider name cannot be null or empty");

            if (!_providerFactories.TryGetValue(name, out var factory))
                return Result<IPdfProvider>.Failure($"PDF provider '{name}' not found");

            try
            {
                var provider = factory(_logger, validator ?? new PdfValidator());
                _logger.LogInformation($"Created PDF provider: {name}");
                return Result<IPdfProvider>.Success(provider);
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateProvider({name})", ex);
                return Result<IPdfProvider>.Failure(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get available provider names
        /// </summary>
        public IEnumerable<string> GetAvailableProviders()
        {
            return _providerFactories.Keys;
        }

        /// <summary>
        /// Check if provider is registered
        /// </summary>
        public bool IsProviderRegistered(string name)
        {
            return _providerFactories.ContainsKey(name);
        }
    }
}
