using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Docx;

namespace AMCode.Docx.Infrastructure.Adapters
{
    /// <summary>
    /// Infrastructure adapter for document metadata operations
    /// Bridges domain interface with OpenXml implementation
    /// </summary>
    public class DocumentMetadataAdapter : IDocumentMetadata
    {
        private readonly IDocument _document;

        public IDocumentProperties Properties => _document.Properties;

        public DocumentMetadataAdapter(IDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public void Close()
        {
            _document.Close();
        }
    }
}
