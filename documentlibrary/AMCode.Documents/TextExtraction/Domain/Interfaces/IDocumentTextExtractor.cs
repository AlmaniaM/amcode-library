using System.Collections.Generic;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.TextExtraction.Domain.Models;

namespace AMCode.Documents.TextExtraction.Domain.Interfaces
{
    /// <summary>
    /// Unified interface for extracting text from any supported document format.
    /// Routes to the appropriate format-specific extractor based on file extension.
    /// </summary>
    public interface IDocumentTextExtractor
    {
        /// <summary>
        /// Extract text from a document stream, auto-detecting format from filename
        /// </summary>
        /// <param name="stream">The document file stream</param>
        /// <param name="fileName">Filename used to detect format (e.g., "recipe.pdf")</param>
        /// <param name="options">Extraction options (null for defaults)</param>
        /// <returns>Result containing extracted text with metadata, or failure</returns>
        Result<TextExtractionResult> ExtractText(Stream stream, string fileName, TextExtractionOptions options = null);

        /// <summary>
        /// Check if a file format is supported for text extraction
        /// </summary>
        /// <param name="fileName">Filename to check (uses extension)</param>
        /// <returns>True if the format is supported</returns>
        bool IsFormatSupported(string fileName);

        /// <summary>
        /// Get list of supported file extensions
        /// </summary>
        IReadOnlyList<string> SupportedExtensions { get; }
    }
}
