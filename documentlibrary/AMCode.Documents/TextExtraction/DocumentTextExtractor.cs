using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMCode.Documents.Common.Models;
using AMCode.Documents.TextExtraction.Domain.Interfaces;
using AMCode.Documents.TextExtraction.Domain.Models;

namespace AMCode.Documents.TextExtraction
{
    /// <summary>
    /// DI-friendly unified document text extractor.
    /// Routes extraction requests to format-specific extractors based on file extension.
    /// </summary>
    public class DocumentTextExtractor : IDocumentTextExtractor
    {
        private readonly IPdfTextExtractor _pdfExtractor;
        private readonly IDocxTextExtractor _docxExtractor;

        private static readonly HashSet<string> _supportedExtensions =
            new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".docx" };

        public DocumentTextExtractor(
            IPdfTextExtractor pdfExtractor,
            IDocxTextExtractor docxExtractor)
        {
            _pdfExtractor = pdfExtractor ?? throw new ArgumentNullException(nameof(pdfExtractor));
            _docxExtractor = docxExtractor ?? throw new ArgumentNullException(nameof(docxExtractor));
        }

        /// <inheritdoc />
        public Result<TextExtractionResult> ExtractText(
            Stream stream, string fileName, TextExtractionOptions options = null)
        {
            if (stream == null)
                return Result<TextExtractionResult>.Failure("Stream cannot be null");

            if (string.IsNullOrWhiteSpace(fileName))
                return Result<TextExtractionResult>.Failure("File name cannot be null or empty");

            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            return extension switch
            {
                ".pdf" => _pdfExtractor.ExtractText(stream, options),
                ".docx" => _docxExtractor.ExtractText(stream, options),
                _ => Result<TextExtractionResult>.Failure(
                    $"Unsupported file format: {extension}. Supported: .pdf, .docx")
            };
        }

        /// <inheritdoc />
        public bool IsFormatSupported(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return !string.IsNullOrEmpty(extension) && _supportedExtensions.Contains(extension);
        }

        /// <inheritdoc />
        public IReadOnlyList<string> SupportedExtensions =>
            _supportedExtensions.ToList().AsReadOnly();
    }
}
