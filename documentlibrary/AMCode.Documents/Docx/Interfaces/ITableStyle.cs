using System;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Table style definition
    /// </summary>
    public interface ITableStyle
    {
        /// <summary>
        /// Style name
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// Header row background color
        /// </summary>
        Color HeaderBackgroundColor { get; set; }
        
        /// <summary>
        /// Header row text color
        /// </summary>
        Color HeaderTextColor { get; set; }
        
        /// <summary>
        /// Alternating row background color
        /// </summary>
        Color AlternatingRowColor { get; set; }
        
        /// <summary>
        /// Border color
        /// </summary>
        Color BorderColor { get; set; }
        
        /// <summary>
        /// Border line style
        /// </summary>
        BorderLineStyle BorderLineStyle { get; set; }
        
        /// <summary>
        /// Font name
        /// </summary>
        string FontName { get; set; }
        
        /// <summary>
        /// Font size
        /// </summary>
        double FontSize { get; set; }
        
        /// <summary>
        /// Header row bold
        /// </summary>
        bool HeaderBold { get; set; }
        
        /// <summary>
        /// Apply alternating row colors
        /// </summary>
        bool UseAlternatingRows { get; set; }
    }
}
