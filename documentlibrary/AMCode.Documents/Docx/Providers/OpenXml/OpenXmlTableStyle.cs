using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Docx;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;

namespace AMCode.Docx.Providers.OpenXml
{
    /// <summary>
    /// OpenXml implementation of ITableStyle
    /// </summary>
    public class OpenXmlTableStyle : ITableStyle
    {
        public string Name { get; set; } = "TableGrid";
        public Color HeaderBackgroundColor { get; set; } = Color.LightGray;
        public Color HeaderTextColor { get; set; } = Color.Black;
        public Color AlternatingRowColor { get; set; } = Color.LightBlue;
        public Color BorderColor { get; set; } = Color.Black;
        public BorderLineStyle BorderLineStyle { get; set; } = BorderLineStyle.Thin;
        public string FontName { get; set; } = "Calibri";
        public double FontSize { get; set; } = 11;
        public bool HeaderBold { get; set; } = true;
        public bool UseAlternatingRows { get; set; } = false;

        public OpenXmlTableStyle()
        {
        }

        public OpenXmlTableStyle(string name)
        {
            Name = name;
        }
    }
}
