using System;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Font style for text rendering
    /// </summary>
    public class FontStyle
    {
        /// <summary>
        /// Font family name
        /// </summary>
        public string FontFamily { get; set; } = "Arial";
        
        /// <summary>
        /// Font size in points
        /// </summary>
        public double FontSize { get; set; } = 12;
        
        /// <summary>
        /// Font color
        /// </summary>
        public Color Color { get; set; } = Color.Black;
        
        /// <summary>
        /// Bold text
        /// </summary>
        public bool Bold { get; set; }
        
        /// <summary>
        /// Italic text
        /// </summary>
        public bool Italic { get; set; }
        
        /// <summary>
        /// Underlined text
        /// </summary>
        public bool Underline { get; set; }
        
        /// <summary>
        /// Strikethrough text
        /// </summary>
        public bool Strikethrough { get; set; }
        
        /// <summary>
        /// Text alignment
        /// </summary>
        public TextAlignment Alignment { get; set; } = TextAlignment.Left;
        
        /// <summary>
        /// Line spacing multiplier
        /// </summary>
        public double LineSpacing { get; set; } = 1.0;
        
        /// <summary>
        /// Character spacing in points
        /// </summary>
        public double CharacterSpacing { get; set; } = 0;
        
        /// <summary>
        /// Word spacing in points
        /// </summary>
        public double WordSpacing { get; set; } = 0;
    }
    
    /// <summary>
    /// Text alignment enumeration
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// Left alignment
        /// </summary>
        Left,
        
        /// <summary>
        /// Center alignment
        /// </summary>
        Center,
        
        /// <summary>
        /// Right alignment
        /// </summary>
        Right,
        
        /// <summary>
        /// Justified alignment
        /// </summary>
        Justified
    }
}
