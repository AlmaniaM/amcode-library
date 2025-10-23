using System;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF metadata implementation
    /// </summary>
    public class PdfMetadata : IPdfMetadata
    {
        private readonly IPdfProperties _properties;

        /// <summary>
        /// Document properties
        /// </summary>
        public IPdfProperties Properties => _properties;

        /// <summary>
        /// Create PDF metadata
        /// </summary>
        public PdfMetadata(IPdfProperties properties = null)
        {
            _properties = properties ?? new PdfProperties();
        }

        /// <summary>
        /// Close metadata and cleanup resources
        /// </summary>
        public void Close()
        {
            // Cleanup any resources if needed
            // For now, there's nothing to clean up
        }
    }
}
