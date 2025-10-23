using AMCode.Documents.Docx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ISections
    /// </summary>
    public class OpenXmlSections : ISections
    {
        private readonly OpenXmlDocument _document;
        private readonly List<ISection> _sections;

        public int Count => _sections.Count;
        public IDocument Document => _document;

        public ISection this[int index] => _sections[index];

        public OpenXmlSections(OpenXmlDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
            _sections = new List<ISection>();
            
            // Initialize with existing sections
            InitializeSections();
        }

        private void InitializeSections()
        {
            // In OpenXml, sections are defined by section properties
            // For simplicity, we'll create one default section
            var section = new OpenXmlSection(this, 0);
            _sections.Add(section);
        }

        public ISection Create()
        {
            var section = new OpenXmlSection(this, _sections.Count);
            _sections.Add(section);
            return section;
        }

        public ISection Create(AMCode.Documents.Common.Models.PageSize pageSize)
        {
            var section = Create();
            section.SetPageSize(pageSize);
            return section;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _sections.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var section = _sections[index];
            Remove(section);
        }

        public void Remove(ISection section)
        {
            if (section is OpenXmlSection openXmlSection)
            {
                // In OpenXml, removing sections is complex
                // For simplicity, we'll just remove from our collection
                _sections.Remove(section);
            }
        }

        public IEnumerator<ISection> GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
