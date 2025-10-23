using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;
using AMCode.Docx;

namespace AMCode.Docx.Infrastructure.Adapters
{
    /// <summary>
    /// Infrastructure adapter for document content operations
    /// Bridges domain interface with OpenXml implementation
    /// </summary>
    public class DocumentContentAdapter : IDocumentContent
    {
        private readonly IDocument _document;

        public IParagraphs Paragraphs => _document.Paragraphs;
        public ITables Tables => _document.Tables;
        public ISections Sections => _document.Sections;

        public DocumentContentAdapter(IDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public void SaveAs(Stream stream)
        {
            _document.SaveAs(stream);
        }

        public void SaveAs(string filePath)
        {
            _document.SaveAs(filePath);
        }
    }
}
