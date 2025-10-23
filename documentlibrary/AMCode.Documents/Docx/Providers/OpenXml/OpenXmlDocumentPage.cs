using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of IDocumentPage
    /// </summary>
    public class OpenXmlDocumentPage : IDocumentPage
    {
        private readonly OpenXmlDocument _document;

        public OpenXmlDocumentPage(OpenXmlDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public IDocument Document => _document;

        public int PageNumber => 1;

        public void AddContent(string content)
        {
            // Add content to the document's paragraphs
            var paragraph = _document.Paragraphs.Create();
            paragraph.AddText(content);
        }

        public void AddParagraph(IParagraph paragraph)
        {
            // This would need to be implemented based on the specific requirements
            throw new NotImplementedException("AddParagraph not implemented in OpenXml");
        }

        public void AddText(string text, string fontFamily = "Arial", int fontSize = 12, string fontColor = "Black", int x = 0, int y = 0)
        {
            // Add text to the document's paragraphs
            var paragraph = _document.Paragraphs.Create();
            paragraph.AddText(text);
        }

        public async Task<Stream> SaveAsStreamAsync()
        {
            // Delegate to the document's SaveAsStreamAsync method
            return await _document.SaveAsStreamAsync();
        }
    }
}
