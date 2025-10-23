using System;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a table cell
    /// </summary>
    public interface ITableCell
    {
        /// <summary>
        /// Cell content as text
        /// </summary>
        string Text { get; set; }
        
        /// <summary>
        /// Collection of paragraphs in this cell
        /// </summary>
        IParagraphs Paragraphs { get; }
        
        /// <summary>
        /// Row index (0-based)
        /// </summary>
        int RowIndex { get; }
        
        /// <summary>
        /// Column index (0-based)
        /// </summary>
        int ColumnIndex { get; }
        
        /// <summary>
        /// Parent row
        /// </summary>
        ITableRow Row { get; }
        
        /// <summary>
        /// Cell background color
        /// </summary>
        AMCode.Documents.Common.Drawing.Color BackgroundColor { get; set; }
        
        /// <summary>
        /// Cell border style
        /// </summary>
        BorderStyle BorderStyle { get; set; }
        
        /// <summary>
        /// Add text to the cell
        /// </summary>
        void AddText(string text);
        
        /// <summary>
        /// Add text with formatting
        /// </summary>
        void AddText(string text, FontStyle fontStyle);
        
        /// <summary>
        /// Add a paragraph to the cell
        /// </summary>
        IParagraph AddParagraph();
        
        /// <summary>
        /// Add a paragraph with text
        /// </summary>
        IParagraph AddParagraph(string text);
        
        /// <summary>
        /// Set cell background color
        /// </summary>
        void SetBackgroundColor(AMCode.Documents.Common.Drawing.Color color);
        
        /// <summary>
        /// Set cell border
        /// </summary>
        void SetBorder(BorderStyle borderStyle);
    }
}
