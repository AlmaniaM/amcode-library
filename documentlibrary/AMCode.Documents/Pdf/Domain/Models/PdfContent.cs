using System;
using System.IO;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF content implementation
    /// </summary>
    public class PdfContent : IPdfContent
    {
        private readonly IPages _pages;
        private readonly IPdfDocument _document;

        /// <summary>
        /// Collection of pages
        /// </summary>
        public IPages Pages => _pages;

        /// <summary>
        /// Parent document
        /// </summary>
        public IPdfDocument Document => _document;

        /// <summary>
        /// Create PDF content
        /// </summary>
        public PdfContent(IPdfDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            _pages = new PdfPages();
        }

        /// <summary>
        /// Save content to stream
        /// </summary>
        public void SaveAs(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // This would be implemented by the specific provider
            // For now, we'll throw a not implemented exception
            throw new NotImplementedException("SaveAs will be implemented by the specific PDF provider");
        }

        /// <summary>
        /// Save content to file
        /// </summary>
        public void SaveAs(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

            using var stream = File.Create(filePath);
            SaveAs(stream);
        }
    }
}
