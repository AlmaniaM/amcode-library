using System;

namespace AMCode.Documents.Common.Models
{
    /// <summary>
    /// Represents font styling information
    /// </summary>
    public class FontStyle
    {
        private string _fontName = "Calibri";
        private AMCode.Documents.Common.Drawing.Color _color = AMCode.Documents.Common.Drawing.Color.Black;

        /// <summary>
        /// Font name
        /// </summary>
        public string FontName 
        { 
            get => _fontName;
            set => _fontName = string.IsNullOrEmpty(value) ? "Calibri" : value;
        }

        /// <summary>
        /// Font size in points
        /// </summary>
        public double FontSize { get; set; } = 11;

        /// <summary>
        /// Bold formatting
        /// </summary>
        public bool Bold { get; set; }

        /// <summary>
        /// Italic formatting
        /// </summary>
        public bool Italic { get; set; }

        /// <summary>
        /// Underline formatting
        /// </summary>
        public bool Underline { get; set; }

        /// <summary>
        /// Strikethrough formatting
        /// </summary>
        public bool Strikethrough { get; set; }

        /// <summary>
        /// Font color
        /// </summary>
        public AMCode.Documents.Common.Drawing.Color Color 
        { 
            get => _color;
            set => _color = value ?? AMCode.Documents.Common.Drawing.Color.Black;
        }
    }
}
