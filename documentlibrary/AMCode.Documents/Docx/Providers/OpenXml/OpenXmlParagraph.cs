using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;
using AMCode.Documents.Common.Enums;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of IParagraph
    /// </summary>
    public class OpenXmlParagraph : IParagraph
    {
        private readonly OpenXmlParagraphs _paragraphs;
        private readonly DocumentFormat.OpenXml.Wordprocessing.Paragraph _openXmlParagraph;

        public IRuns Runs { get; }
        public IParagraphs Paragraphs => _paragraphs;

        public string Text
        {
            get => _openXmlParagraph.InnerText;
            set
            {
                _openXmlParagraph.RemoveAllChildren<Run>();
                if (!string.IsNullOrEmpty(value))
                {
                    var run = new Run(new Text(value));
                    _openXmlParagraph.AppendChild(run);
                }
            }
        }

        public HorizontalAlignment Alignment
        {
            get
            {
                var justification = _openXmlParagraph.ParagraphProperties?.Justification?.Val?.Value;
                if (justification == null) return HorizontalAlignment.Left;
                
                if (justification == JustificationValues.Left) return HorizontalAlignment.Left;
                if (justification == JustificationValues.Center) return HorizontalAlignment.Center;
                if (justification == JustificationValues.Right) return HorizontalAlignment.Right;
                if (justification == JustificationValues.Both) return HorizontalAlignment.Justify;
                
                return HorizontalAlignment.Left;
            }
            set
            {
                var paragraphProperties = _openXmlParagraph.ParagraphProperties ?? new ParagraphProperties();
                paragraphProperties.Justification = new Justification()
                {
                    Val = new EnumValue<JustificationValues>(value switch
                    {
                        HorizontalAlignment.Left => JustificationValues.Left,
                        HorizontalAlignment.Center => JustificationValues.Center,
                        HorizontalAlignment.Right => JustificationValues.Right,
                        HorizontalAlignment.Justify => JustificationValues.Both,
                        _ => JustificationValues.Left
                    })
                };
                _openXmlParagraph.ParagraphProperties = paragraphProperties;
            }
        }

        public double SpacingBefore
        {
            get 
            {
                var value = _openXmlParagraph.ParagraphProperties?.SpacingBetweenLines?.Before?.Value;
                return value != null ? double.Parse(value) : 0;
            }
            set
            {
                var paragraphProperties = _openXmlParagraph.ParagraphProperties ?? new ParagraphProperties();
                paragraphProperties.SpacingBetweenLines = new SpacingBetweenLines()
                {
                    Before = value.ToString()
                };
                _openXmlParagraph.ParagraphProperties = paragraphProperties;
            }
        }

        public double SpacingAfter
        {
            get 
            {
                var value = _openXmlParagraph.ParagraphProperties?.SpacingBetweenLines?.After?.Value;
                return value != null ? double.Parse(value) : 0;
            }
            set
            {
                var paragraphProperties = _openXmlParagraph.ParagraphProperties ?? new ParagraphProperties();
                paragraphProperties.SpacingBetweenLines = new SpacingBetweenLines()
                {
                    After = value.ToString()
                };
                _openXmlParagraph.ParagraphProperties = paragraphProperties;
            }
        }

        public double LineSpacing
        {
            get 
            {
                var value = _openXmlParagraph.ParagraphProperties?.SpacingBetweenLines?.Line?.Value;
                return value != null ? double.Parse(value) : 0;
            }
            set
            {
                var paragraphProperties = _openXmlParagraph.ParagraphProperties ?? new ParagraphProperties();
                paragraphProperties.SpacingBetweenLines = new SpacingBetweenLines()
                {
                    Line = value.ToString()
                };
                _openXmlParagraph.ParagraphProperties = paragraphProperties;
            }
        }

        internal DocumentFormat.OpenXml.Wordprocessing.Paragraph OpenXmlElement => _openXmlParagraph;

        public OpenXmlParagraph(OpenXmlParagraphs paragraphs, DocumentFormat.OpenXml.Wordprocessing.Paragraph openXmlParagraph)
        {
            _paragraphs = paragraphs ?? throw new ArgumentNullException(nameof(paragraphs));
            _openXmlParagraph = openXmlParagraph ?? throw new ArgumentNullException(nameof(openXmlParagraph));
            Runs = new OpenXmlRuns(this);
        }

        public IRun AddText(string text)
        {
            var run = new Run(new Text(text));
            _openXmlParagraph.AppendChild(run);
            return new OpenXmlRun(this, run);
        }

        public IRun AddText(string text, FontStyle fontStyle)
        {
            var run = new Run();
            
            // Apply font formatting
            var runProperties = new RunProperties();
            
            if (fontStyle.Bold)
                runProperties.AppendChild(new Bold());
            
            if (fontStyle.Italic)
                runProperties.AppendChild(new Italic());
            
            if (fontStyle.Underline)
                runProperties.AppendChild(new Underline());
            
            if (!string.IsNullOrEmpty(fontStyle.FontName))
                runProperties.AppendChild(new RunFonts() { Ascii = fontStyle.FontName });
            
            if (fontStyle.FontSize > 0)
                runProperties.AppendChild(new FontSize() { Val = (fontStyle.FontSize * 2).ToString() }); // FontSize is in half-points
            
            if (fontStyle.Color != null)
                runProperties.AppendChild(new Color() { Val = fontStyle.Color.ToArgb().ToString("X8")[2..] });
            
            run.AppendChild(runProperties);
            run.AppendChild(new Text(text));
            
            _openXmlParagraph.AppendChild(run);
            return new OpenXmlRun(this, run);
        }

        public void AddLineBreak()
        {
            _openXmlParagraph.AppendChild(new Break());
        }

        public void Remove()
        {
            _openXmlParagraph.Remove();
        }
    }
}
