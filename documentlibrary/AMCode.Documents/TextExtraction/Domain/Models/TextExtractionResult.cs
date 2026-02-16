using System;
using System.Collections.Generic;

namespace AMCode.Documents.TextExtraction.Domain.Models
{
    /// <summary>
    /// Result of a document text extraction operation, including metadata
    /// </summary>
    public class TextExtractionResult
    {
        /// <summary>
        /// The extracted text content
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Number of pages in the document (1 for single-page formats like DOCX)
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        /// Total character count of extracted text
        /// </summary>
        public int CharacterCount { get; }

        /// <summary>
        /// File format that was processed (e.g., "pdf", "docx")
        /// </summary>
        public string Format { get; }

        /// <summary>
        /// Any warnings generated during extraction
        /// (e.g., "Page 3 had no extractable text")
        /// </summary>
        public IReadOnlyList<string> Warnings { get; }

        public TextExtractionResult(
            string text,
            int pageCount,
            string format,
            IReadOnlyList<string> warnings = null)
        {
            Text = text ?? string.Empty;
            PageCount = pageCount;
            CharacterCount = Text.Length;
            Format = format;
            Warnings = warnings ?? Array.Empty<string>();
        }
    }
}
