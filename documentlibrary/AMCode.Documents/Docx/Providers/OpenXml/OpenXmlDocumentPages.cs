using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of IDocumentPages
    /// </summary>
    public class OpenXmlDocumentPages : IDocumentPages, IEnumerable<IDocumentPage>
    {
        private readonly OpenXmlDocument _document;

        public OpenXmlDocumentPages(OpenXmlDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public IDocumentPage this[int index] => throw new NotImplementedException("Page access by index not implemented in OpenXml");

        public int Count => 1; // OpenXml documents are typically single-page

        public IDocument Document => _document;

        public IDocumentPage Create()
        {
            throw new NotImplementedException("Page creation not implemented in OpenXml");
        }

        public IEnumerator<IDocumentPage> GetEnumerator()
        {
            // Return a single page for now
            yield return new OpenXmlDocumentPage(_document);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
