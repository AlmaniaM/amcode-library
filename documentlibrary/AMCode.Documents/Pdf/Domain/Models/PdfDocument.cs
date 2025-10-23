using System;
using System.IO;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF document domain model
    /// </summary>
    public class PdfDocument : IPdfDocument
    {
        private readonly IPdfContent _content;
        private readonly IPdfMetadata _metadata;
        private readonly IPdfProvider _provider;

        /// <summary>
        /// Document identifier
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Document creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Document last modified timestamp
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Collection of pages
        /// </summary>
        public IPages Pages => _content.Pages;

        /// <summary>
        /// Document properties
        /// </summary>
        public IPdfProperties Properties => _metadata.Properties;

        /// <summary>
        /// Create PDF document
        /// </summary>
        public PdfDocument(IPdfContent content, IPdfMetadata metadata, IPdfProvider provider)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            LastModified = CreatedAt;
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Save document to stream
        /// </summary>
        public void SaveAs(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            
            _content.SaveAs(stream);
            LastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Save document to file
        /// </summary>
        public void SaveAs(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
            
            _content.SaveAs(filePath);
            LastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Close the document
        /// </summary>
        public void Close()
        {
            _metadata.Close();
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
