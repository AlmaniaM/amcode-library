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
    /// OpenXml implementation of IRuns
    /// </summary>
    public class OpenXmlRuns : IRuns
    {
        private readonly OpenXmlParagraph _paragraph;
        private readonly List<IRun> _runs;

        public int Count => _runs.Count;
        public IParagraph Paragraph => _paragraph;

        public IRun this[int index] => _runs[index];

        public OpenXmlRuns(OpenXmlParagraph paragraph)
        {
            _paragraph = paragraph ?? throw new ArgumentNullException(nameof(paragraph));
            _runs = new List<IRun>();
            
            // Initialize with existing runs
            InitializeRuns();
        }

        private void InitializeRuns()
        {
            var openXmlRuns = _paragraph.OpenXmlElement.Elements<Run>();
            foreach (var openXmlRun in openXmlRuns)
            {
                _runs.Add(new OpenXmlRun(_paragraph, openXmlRun));
            }
        }

        public IRun Create(string text)
        {
            var openXmlRun = new Run(new Text(text));
            _paragraph.OpenXmlElement.AppendChild(openXmlRun);
            
            var run = new OpenXmlRun(_paragraph, openXmlRun);
            _runs.Add(run);
            return run;
        }

        public IRun Create(string text, FontStyle fontStyle)
        {
            var openXmlRun = new Run();
            
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
                runProperties.AppendChild(new FontSize() { Val = (fontStyle.FontSize * 2).ToString() });
            
            if (fontStyle.Color != null)
                runProperties.AppendChild(new Color() { Val = fontStyle.Color.ToArgb().ToString("X8")[2..] });
            
            openXmlRun.AppendChild(runProperties);
            openXmlRun.AppendChild(new Text(text));
            
            _paragraph.OpenXmlElement.AppendChild(openXmlRun);
            
            var run = new OpenXmlRun(_paragraph, openXmlRun);
            _runs.Add(run);
            return run;
        }

        public void Remove(int index)
        {
            if (index < 0 || index >= _runs.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var run = _runs[index];
            Remove(run);
        }

        public void Remove(IRun run)
        {
            if (run is OpenXmlRun openXmlRun)
            {
                openXmlRun.OpenXmlElement.Remove();
                _runs.Remove(run);
            }
        }

        public IEnumerator<IRun> GetEnumerator()
        {
            return _runs.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
