using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of IParagraphs
    /// </summary>
    public class OpenXmlParagraphs : IParagraphs
    {
        private readonly OpenXmlDocument _document;
        private readonly List<IParagraph> _paragraphs;

        public int Count => _paragraphs.Count;
        public IDocument Document => _document;

        public IParagraph this[int index] => _paragraphs[index];

        public OpenXmlParagraphs(OpenXmlDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            _paragraphs = new List<IParagraph>();
            
            // Initialize with existing paragraphs
            InitializeParagraphs();
        }

        private void InitializeParagraphs()
        {
            var openXmlParagraphs = _document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            foreach (var openXmlParagraph in openXmlParagraphs)
            {
                _paragraphs.Add(new OpenXmlParagraph(this, openXmlParagraph));
            }
        }

        public IParagraph Create()
        {
            var openXmlParagraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();
            _document.Body.AppendChild(openXmlParagraph);
            
            var paragraph = new OpenXmlParagraph(this, openXmlParagraph);
            _paragraphs.Add(paragraph);
            return paragraph;
        }

        public IParagraph Create(string text)
        {
            var paragraph = Create();
            paragraph.Text = text;
            return paragraph;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _paragraphs.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var paragraph = _paragraphs[index];
            Remove(paragraph);
        }

        public void Remove(IParagraph paragraph)
        {
            if (paragraph is OpenXmlParagraph openXmlParagraph)
            {
                openXmlParagraph.OpenXmlElement.Remove();
                _paragraphs.Remove(paragraph);
            }
        }

        public IEnumerator<IParagraph> GetEnumerator()
        {
            return _paragraphs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
