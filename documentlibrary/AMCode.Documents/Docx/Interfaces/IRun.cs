using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a run (text segment) in a paragraph
    /// </summary>
    public interface IRun
    {
        /// <summary>
        /// Text content of the run
        /// </summary>
        string Text { get; set; }
        
        /// <summary>
        /// Font style for the run
        /// </summary>
        FontStyle Font { get; set; }
        
        /// <summary>
        /// Parent runs collection
        /// </summary>
        IRuns Runs { get; }
        
        /// <summary>
        /// Apply font style to the run
        /// </summary>
        void ApplyFont(FontStyle fontStyle);
        
        /// <summary>
        /// Set bold formatting
        /// </summary>
        void SetBold(bool bold);
        
        /// <summary>
        /// Set italic formatting
        /// </summary>
        void SetItalic(bool italic);
        
        /// <summary>
        /// Set underline formatting
        /// </summary>
        void SetUnderline(bool underline);
        
        /// <summary>
        /// Set font size
        /// </summary>
        void SetFontSize(double size);
        
        /// <summary>
        /// Set font name
        /// </summary>
        void SetFontName(string fontName);
        
        /// <summary>
        /// Set font color
        /// </summary>
        void SetFontColor(AMCode.Documents.Common.Drawing.Color color);
    }
}
