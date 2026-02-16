using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.TextExtraction.Domain.Models;

namespace AMCode.Documents.TextExtraction.Domain.Interfaces
{
    /// <summary>
    /// Interface for extracting text content from DOCX documents
    /// </summary>
    public interface IDocxTextExtractor
    {
        /// <summary>
        /// Extract text from a DOCX stream
        /// </summary>
        /// <param name="stream">The DOCX file stream</param>
        /// <param name="options">Extraction options (null for defaults)</param>
        /// <returns>Result containing extracted text with metadata, or failure</returns>
        Result<TextExtractionResult> ExtractText(Stream stream, TextExtractionOptions options = null);
    }
}
