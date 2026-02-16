namespace AMCode.Documents.TextExtraction.Domain.Models
{
    /// <summary>
    /// Options for configuring document text extraction behavior
    /// </summary>
    public class TextExtractionOptions
    {
        /// <summary>
        /// Separator between pages (default: double newline)
        /// </summary>
        public string PageSeparator { get; set; } = "\n\n";

        /// <summary>
        /// Whether to preserve original line breaks within pages
        /// </summary>
        public bool PreserveLineBreaks { get; set; } = true;

        /// <summary>
        /// Maximum number of pages to extract (null = all pages)
        /// </summary>
        public int? MaxPages { get; set; }

        /// <summary>
        /// Whether to trim whitespace from extracted text
        /// </summary>
        public bool TrimWhitespace { get; set; } = true;
    }
}
