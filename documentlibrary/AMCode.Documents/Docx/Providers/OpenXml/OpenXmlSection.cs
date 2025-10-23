using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;
using AMCode.Documents.Common.Models;

// Type aliases to disambiguate between Common and OpenXml types
using CommonPageSize = AMCode.Documents.Common.Models.PageSize;
using CommonMargins = AMCode.Documents.Common.Models.Margins;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ISection
    /// </summary>
    public class OpenXmlSection : ISection
    {
        private readonly OpenXmlSections _sections;
        private readonly int _index;

        public CommonPageSize PageSize { get; set; } = CommonPageSize.A4;
        public CommonMargins Margins { get; set; } = CommonMargins.Normal;
        public PageOrientation Orientation { get; set; } = PageOrientation.Portrait;
        public int Index => _index;
        public ISections Sections => _sections;

        public OpenXmlSection(OpenXmlSections sections, int index)
        {
            _sections = sections ?? throw new ArgumentNullException(nameof(sections));
            _index = index;
        }

        public void SetPageSize(CommonPageSize pageSize)
        {
            PageSize = pageSize ?? throw new ArgumentNullException(nameof(pageSize));
            
            // Apply page size to document
            var sectionProperties = GetOrCreateSectionProperties();
            if (sectionProperties != null)
            {
                var openXmlPageSize = new DocumentFormat.OpenXml.Wordprocessing.PageSize()
                {
                    Width = (uint)(pageSize.WidthInPoints * 20), // Convert to DXA
                    Height = (uint)(pageSize.HeightInPoints * 20)
                };
                sectionProperties.AppendChild(openXmlPageSize);
            }
        }

        public void SetMargins(CommonMargins margins)
        {
            Margins = margins ?? throw new ArgumentNullException(nameof(margins));
            
            // Apply margins to document
            var sectionProperties = GetOrCreateSectionProperties();
            if (sectionProperties != null)
            {
                var pageMargins = new PageMargin()
                {
                    Top = (int)(margins.Top * 20), // Convert to DXA
                    Right = (uint)(margins.Right * 20),
                    Bottom = (int)(margins.Bottom * 20),
                    Left = (uint)(margins.Left * 20)
                };
                sectionProperties.AppendChild(pageMargins);
            }
        }

        public void SetOrientation(PageOrientation orientation)
        {
            Orientation = orientation;
            
            // Apply orientation to document
            var sectionProperties = GetOrCreateSectionProperties();
            if (sectionProperties != null)
            {
                var openXmlPageSize = sectionProperties.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.PageSize>() 
                    ?? new DocumentFormat.OpenXml.Wordprocessing.PageSize();
                
                if (orientation == PageOrientation.Landscape)
                {
                    // Swap width and height for landscape
                    var temp = openXmlPageSize.Width;
                    openXmlPageSize.Width = openXmlPageSize.Height;
                    openXmlPageSize.Height = temp;
                }
                
                if (sectionProperties.GetFirstChild<DocumentFormat.OpenXml.Wordprocessing.PageSize>() == null)
                {
                    sectionProperties.AppendChild(openXmlPageSize);
                }
            }
        }

        private SectionProperties GetOrCreateSectionProperties()
        {
            var document = _sections.Document as OpenXmlDocument;
            if (document?.Body == null) return null;

            // Get or create section properties
            var sectionProperties = document.Body.Elements<SectionProperties>().LastOrDefault();
            if (sectionProperties == null)
            {
                sectionProperties = new SectionProperties();
                document.Body.AppendChild(sectionProperties);
            }

            return sectionProperties;
        }
    }
}
