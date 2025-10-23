using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;
using AMCode.Docx;

namespace AMCode.Docx.Domain.Models
{
    /// <summary>
    /// Domain model for document operations
    /// Combines content and metadata adapters
    /// </summary>
    public class DocumentDomain : IDocumentDomain
    {
        private readonly IDocumentContent _content;
        private readonly IDocumentMetadata _metadata;

        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public DateTime LastModified { get; set; }

        public DocumentDomain(IDocumentContent content, IDocumentMetadata metadata)
        {
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
        }

        // Delegate content operations to content adapter
        public IParagraphs Paragraphs => _content.Paragraphs;
        public ITables Tables => _content.Tables;
        public ISections Sections => _content.Sections;

        // Delegate metadata operations to metadata adapter
        public IDocumentProperties Properties => _metadata.Properties;

        // Delegate content operations
        public void SaveAs(Stream stream) => _content.SaveAs(stream);
        public void SaveAs(string filePath) => _content.SaveAs(filePath);

        // Delegate metadata operations
        public void Close() => _metadata.Close();

        public void Dispose()
        {
            // Cleanup if needed
        }
    }
}