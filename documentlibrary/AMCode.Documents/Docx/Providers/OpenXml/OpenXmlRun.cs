using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;
using AMCode.Docx;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Models;

// Type aliases to disambiguate between Common and OpenXml types
using CommonColor = AMCode.Documents.Common.Drawing.Color;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of IRun
    /// </summary>
    public class OpenXmlRun : IRun
    {
        private readonly OpenXmlParagraph _paragraph;
        private readonly Run _openXmlRun;

        public string Text
        {
            get => _openXmlRun.InnerText;
            set
            {
                // Remove existing text elements
                var textElements = _openXmlRun.Elements<Text>().ToList();
                foreach (var textElement in textElements)
                {
                    textElement.Remove();
                }
                
                if (!string.IsNullOrEmpty(value))
                {
                    _openXmlRun.AppendChild(new Text(value));
                }
            }
        }

        public FontStyle Font
        {
            get
            {
                var runProperties = _openXmlRun.RunProperties;
                if (runProperties == null)
                    return new FontStyle();

                return new FontStyle
                {
                    Bold = runProperties.Bold != null,
                    Italic = runProperties.Italic != null,
                    Underline = runProperties.Underline != null,
                    FontName = runProperties.RunFonts?.Ascii ?? "Calibri",
                    FontSize = runProperties.FontSize?.Val != null ? double.Parse(runProperties.FontSize.Val) / 2 : 11,
                    Color = runProperties.Color?.Val != null ? CommonColor.FromArgb(Convert.ToInt32(runProperties.Color.Val, 16)) : CommonColor.Black
                };
            }
            set
            {
                ApplyFont(value);
            }
        }

        public IRuns Runs => _paragraph.Runs;

        internal Run OpenXmlElement => _openXmlRun;

        public OpenXmlRun(OpenXmlParagraph paragraph, Run openXmlRun)
        {
            _paragraph = paragraph ?? throw new ArgumentNullException(nameof(paragraph));
            _openXmlRun = openXmlRun ?? throw new ArgumentNullException(nameof(openXmlRun));
        }

        public void ApplyFont(FontStyle fontStyle)
        {
            var runProperties = _openXmlRun.RunProperties ?? new RunProperties();
            
            // Clear existing formatting
            runProperties.RemoveAllChildren<Bold>();
            runProperties.RemoveAllChildren<Italic>();
            runProperties.RemoveAllChildren<Underline>();
            runProperties.RemoveAllChildren<RunFonts>();
            runProperties.RemoveAllChildren<FontSize>();
            runProperties.RemoveAllChildren<DocumentFormat.OpenXml.Wordprocessing.Color>();
            
            // Apply new formatting
            if (fontStyle.Bold)
                runProperties.AppendChild(new Bold());
            
            if (fontStyle.Italic)
                runProperties.AppendChild(new Italic());
            
            if (fontStyle.Underline)
                runProperties.AppendChild(new Underline());
            
            if (!string.IsNullOrEmpty(fontStyle.FontName))
                runProperties.AppendChild(new RunFonts() { Ascii = fontStyle.FontName });
            
            if (fontStyle.FontSize > 0)
                runProperties.AppendChild(new FontSize() { Val = (fontStyle.FontSize * 2).ToString() });
            
            if (fontStyle.Color != null)
                runProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = fontStyle.Color.ToArgb().ToString("X8")[2..] });
            
            _openXmlRun.RunProperties = runProperties;
        }

        public void SetBold(bool bold)
        {
            var runProperties = _openXmlRun.RunProperties ?? new RunProperties();
            
            if (bold)
            {
                if (runProperties.Bold == null)
                    runProperties.AppendChild(new Bold());
            }
            else
            {
                runProperties.Bold?.Remove();
            }
            
            _openXmlRun.RunProperties = runProperties;
        }

        public void SetItalic(bool italic)
        {
            var runProperties = _openXmlRun.RunProperties ?? new RunProperties();
            
            if (italic)
            {
                if (runProperties.Italic == null)
                    runProperties.AppendChild(new Italic());
            }
            else
            {
                runProperties.Italic?.Remove();
            }
            
            _openXmlRun.RunProperties = runProperties;
        }

        public void SetUnderline(bool underline)
        {
            var runProperties = _openXmlRun.RunProperties ?? new RunProperties();
            
            if (underline)
            {
                if (runProperties.Underline == null)
                    runProperties.AppendChild(new Underline());
            }
            else
            {
                runProperties.Underline?.Remove();
            }
            
            _openXmlRun.RunProperties = runProperties;
        }

        public void SetFontSize(double size)
        {
            var runProperties = _openXmlRun.RunProperties ?? new RunProperties();
            
            if (size > 0)
            {
                runProperties.FontSize = new FontSize() { Val = (size * 2).ToString() };
            }
            else
            {
                runProperties.FontSize?.Remove();
            }
            
            _openXmlRun.RunProperties = runProperties;
        }

        public void SetFontName(string fontName)
        {
            var runProperties = _openXmlRun.RunProperties ?? new RunProperties();
            
            if (!string.IsNullOrEmpty(fontName))
            {
                runProperties.RunFonts = new RunFonts() { Ascii = fontName };
            }
            else
            {
                runProperties.RunFonts?.Remove();
            }
            
            _openXmlRun.RunProperties = runProperties;
        }

        public void SetFontColor(AMCode.Documents.Common.Drawing.Color color)
        {
            var runProperties = _openXmlRun.RunProperties ?? new RunProperties();
            
            if (color != null)
            {
                runProperties.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() 
                { 
                    Val = color.ToArgb().ToString("X8")[2..] 
                };
            }
            else
            {
                runProperties.Color?.Remove();
            }
            
            _openXmlRun.RunProperties = runProperties;
        }
    }
}
