using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMCode.Documents.Common.Models;
using AMCode.Documents.TextExtraction.Domain.Interfaces;
using AMCode.Documents.TextExtraction.Domain.Models;
using AMCode.Documents.TextExtraction.Providers.OpenXml;
using AMCode.Documents.TextExtraction.Providers.PdfPig;

namespace AMCode.Documents.TextExtraction
{
    /// <summary>
    /// Static factory for document text extraction.
    /// Manages format-specific extractors and routes extraction requests.
    /// </summary>
    public static class DocumentTextExtractorFactory
    {
        private static IPdfTextExtractor _pdfExtractor;
        private static IDocxTextExtractor _docxExtractor;

        private static readonly HashSet<string> _supportedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".docx" };

        /// <summary>
        /// Set the PDF text extractor implementation
        /// </summary>
        public static void SetPdfExtractor(IPdfTextExtractor extractor)
        {
            _pdfExtractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
        }

        /// <summary>
        /// Set the DOCX text extractor implementation
        /// </summary>
        public static void SetDocxExtractor(IDocxTextExtractor extractor)
        {
            _docxExtractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
        }

        /// <summary>
        /// Initialize default providers (PdfPig for PDF, OpenXml for DOCX)
        /// </summary>
        public static void InitializeDefaultProviders()
        {
            _pdfExtractor = new PdfPigTextExtractor();
            _docxExtractor = new OpenXmlDocxTextExtractor();
        }

        /// <summary>
        /// Clear all registered providers
        /// </summary>
        public static void ClearProviders()
        {
            _pdfExtractor = null;
            _docxExtractor = null;
        }

        /// <summary>
        /// Extract text from a document, routing to the appropriate extractor by file extension
        /// </summary>
        public static Result<TextExtractionResult> ExtractText(
            Stream stream, string fileName, TextExtractionOptions options = null)
        {
            if (stream == null)
                return Result<TextExtractionResult>.Failure("Stream cannot be null");

            if (string.IsNullOrWhiteSpace(fileName))
                return Result<TextExtractionResult>.Failure("File name cannot be null or empty");

            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            return extension switch
            {
                ".pdf" => _pdfExtractor != null
                    ? _pdfExtractor.ExtractText(stream, options)
                    : Result<TextExtractionResult>.Failure("No PDF text extractor configured. Call InitializeDefaultProviders() first."),
                ".docx" => _docxExtractor != null
                    ? _docxExtractor.ExtractText(stream, options)
                    : Result<TextExtractionResult>.Failure("No DOCX text extractor configured. Call InitializeDefaultProviders() first."),
                _ => Result<TextExtractionResult>.Failure($"Unsupported file format: {extension}. Supported: .pdf, .docx")
            };
        }

        /// <summary>
        /// Check if a file format is supported for text extraction
        /// </summary>
        public static bool IsFormatSupported(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return !string.IsNullOrEmpty(extension) && _supportedExtensions.Contains(extension);
        }

        /// <summary>
        /// Get list of supported file extensions
        /// </summary>
        public static IReadOnlyList<string> SupportedExtensions =>
            _supportedExtensions.ToList().AsReadOnly();
    }
}
