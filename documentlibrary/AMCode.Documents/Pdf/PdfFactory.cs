using System;
using System.Collections.Generic;
using System.IO;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF document factory for easy creation
    /// </summary>
    public static class PdfFactory
    {
        private static IPdfProvider _defaultProvider;
        private static readonly Dictionary<string, IPdfProvider> _providers = new();
        private static readonly IPdfLogger _defaultLogger = new PdfLogger();
        private static readonly IPdfValidator _defaultValidator = new PdfValidator();

        /// <summary>
        /// Clear the default PDF provider
        /// </summary>
        public static void ClearDefaultProvider()
        {
            _defaultProvider = null;
        }

        /// <summary>
        /// Clear all registered providers
        /// </summary>
        public static void ClearAllProviders()
        {
            _defaultProvider = null;
            _providers.Clear();
        }

        /// <summary>
        /// Set the default PDF provider
        /// </summary>
        public static void SetDefaultProvider(IPdfProvider provider)
        {
            _defaultProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Register a PDF provider
        /// </summary>
        public static void RegisterProvider(string name, IPdfProvider provider)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Provider name cannot be null or empty", nameof(name));
            
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            _providers[name] = provider;
        }

        /// <summary>
        /// Create a new PDF document using the default provider with custom logger
        /// </summary>
        public static Result<IPdfDocument> CreateDocument(IPdfLogger logger)
        {
            if (_defaultProvider == null)
                return Result<IPdfDocument>.Failure("No default PDF provider configured");

            logger?.LogPdfOperation("CreateDocument", new { Provider = _defaultProvider.GetType().Name });
            var result = _defaultProvider.CreateDocument();
            
            if (result.IsSuccess)
            {
                logger?.LogPdfOperation("CreateDocumentSuccess", new { DocumentId = result.Value?.GetHashCode() });
            }
            else
            {
                logger?.LogError("CreateDocument", new Exception(result.Error));
            }
            
            return result;
        }

        /// <summary>
        /// Create a new PDF document with properties using the default provider with custom logger
        /// </summary>
        public static Result<IPdfDocument> CreateDocument(IPdfProperties properties, IPdfLogger logger)
        {
            if (_defaultProvider == null)
                return Result<IPdfDocument>.Failure("No default PDF provider configured");

            logger?.LogPdfOperation("CreateDocumentWithProperties", new { Provider = _defaultProvider.GetType().Name, Properties = properties });
            var result = _defaultProvider.CreateDocument(properties);
            
            if (result.IsSuccess)
            {
                logger?.LogPdfOperation("CreateDocumentWithPropertiesSuccess", new { DocumentId = result.Value?.GetHashCode() });
            }
            else
            {
                logger?.LogError("CreateDocumentWithProperties", new Exception(result.Error));
            }
            
            return result;
        }

        /// <summary>
        /// Create a new PDF document using a specific provider with custom logger
        /// </summary>
        public static Result<IPdfDocument> CreateDocument(string providerName, IPdfLogger logger)
        {
            if (!_providers.TryGetValue(providerName, out var provider))
                return Result<IPdfDocument>.Failure($"PDF provider '{providerName}' not found");

            logger?.LogProviderOperation(providerName, "CreateDocument");
            var result = provider.CreateDocument();
            
            if (result.IsSuccess)
            {
                logger?.LogProviderOperation(providerName, "CreateDocumentSuccess", new { DocumentId = result.Value?.GetHashCode() });
            }
            else
            {
                logger?.LogError($"CreateDocument-{providerName}", new Exception(result.Error));
            }
            
            return result;
        }

        /// <summary>
        /// Create a new PDF document with properties using a specific provider with custom logger
        /// </summary>
        public static Result<IPdfDocument> CreateDocument(string providerName, IPdfProperties properties, IPdfLogger logger)
        {
            if (!_providers.TryGetValue(providerName, out var provider))
                return Result<IPdfDocument>.Failure($"PDF provider '{providerName}' not found");

            logger?.LogProviderOperation(providerName, "CreateDocumentWithProperties", new { Properties = properties });
            var result = provider.CreateDocument(properties);
            
            if (result.IsSuccess)
            {
                logger?.LogProviderOperation(providerName, "CreateDocumentWithPropertiesSuccess", new { DocumentId = result.Value?.GetHashCode() });
            }
            else
            {
                logger?.LogError($"CreateDocumentWithProperties-{providerName}", new Exception(result.Error));
            }
            
            return result;
        }

        /// <summary>
        /// Create a new PDF document using the default provider (legacy method for backward compatibility)
        /// </summary>
        public static Result<IPdfDocument> CreateDocument()
        {
            return CreateDocument(_defaultLogger);
        }

        /// <summary>
        /// Create a new PDF document with properties using the default provider (legacy method for backward compatibility)
        /// </summary>
        public static Result<IPdfDocument> CreateDocument(IPdfProperties properties)
        {
            return CreateDocument(properties, _defaultLogger);
        }

        /// <summary>
        /// Create a new PDF document using a specific provider (legacy method for backward compatibility)
        /// </summary>
        public static Result<IPdfDocument> CreateDocument(string providerName)
        {
            return CreateDocument(providerName, _defaultLogger);
        }

        /// <summary>
        /// Create a new PDF document with properties using a specific provider (legacy method for backward compatibility)
        /// </summary>
        public static Result<IPdfDocument> CreateDocument(string providerName, IPdfProperties properties)
        {
            return CreateDocument(providerName, properties, _defaultLogger);
        }

        /// <summary>
        /// Open an existing PDF document using the default provider
        /// </summary>
        public static Result<IPdfDocument> OpenDocument(Stream stream)
        {
            if (_defaultProvider == null)
                return Result<IPdfDocument>.Failure("No default PDF provider configured");

            if (stream == null)
                return Result<IPdfDocument>.Failure("Stream cannot be null");

            return _defaultProvider.OpenDocument(stream);
        }

        /// <summary>
        /// Open an existing PDF document using the default provider
        /// </summary>
        public static Result<IPdfDocument> OpenDocument(string filePath)
        {
            if (_defaultProvider == null)
                return Result<IPdfDocument>.Failure("No default PDF provider configured");

            if (filePath == null)
                return Result<IPdfDocument>.Failure("File path cannot be null");

            if (string.IsNullOrWhiteSpace(filePath))
                return Result<IPdfDocument>.Failure("File path cannot be null or empty");

            return _defaultProvider.OpenDocument(filePath);
        }

        /// <summary>
        /// Get a PDF builder for fluent document creation
        /// </summary>
        public static IPdfBuilder CreateBuilder()
        {
            if (_defaultProvider == null)
                throw new InvalidOperationException("No default PDF provider configured");

            return new PdfBuilder(_defaultProvider);
        }

        /// <summary>
        /// Get a PDF builder for fluent document creation using a specific provider
        /// </summary>
        public static IPdfBuilder CreateBuilder(string providerName)
        {
            if (!_providers.TryGetValue(providerName, out var provider))
                throw new InvalidOperationException($"PDF provider '{providerName}' not found");

            return new PdfBuilder(provider);
        }

        /// <summary>
        /// Initialize default providers
        /// </summary>
        public static void InitializeDefaultProviders()
        {
            try
            {
                // Register QuestPDF provider
                var questPdfProvider = new QuestPdfProvider(_defaultLogger, _defaultValidator);
                RegisterProvider("QuestPDF", questPdfProvider);
                SetDefaultProvider(questPdfProvider);

                // Register iTextSharp provider
                var iTextSharpProvider = new iTextSharpProvider(_defaultLogger, _defaultValidator);
                RegisterProvider("iTextSharp", iTextSharpProvider);

                _defaultLogger.LogInformation("Initialized default PDF providers: QuestPDF (default), iTextSharp");
            }
            catch (Exception ex)
            {
                _defaultLogger.LogError("InitializeDefaultProviders", ex);
                throw;
            }
        }

        /// <summary>
        /// Get available provider names
        /// </summary>
        public static IEnumerable<string> GetAvailableProviders()
        {
            return _providers.Keys;
        }

        /// <summary>
        /// Check if provider is registered
        /// </summary>
        public static bool IsProviderRegistered(string name)
        {
            return _providers.ContainsKey(name);
        }

        /// <summary>
        /// Get the default provider
        /// </summary>
        public static IPdfProvider GetDefaultProvider()
        {
            return _defaultProvider;
        }
    }
}
