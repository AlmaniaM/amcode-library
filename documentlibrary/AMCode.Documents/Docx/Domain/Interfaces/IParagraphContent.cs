using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx
{
    /// <summary>
    /// Domain interface for paragraph content operations
    /// </summary>
    public interface IParagraphContent
    {
        /// <summary>
        /// Paragraph text content
        /// </summary>
        string Text { get; set; }
        
        /// <summary>
        /// Collection of runs (text segments)
        /// </summary>
        IRuns Runs { get; }
        
        /// <summary>
        /// Add text to the paragraph
        /// </summary>
        /// <param name="text">The text to add</param>
        /// <returns>The created run</returns>
        IRun AddText(string text);
        
        /// <summary>
        /// Add text with specific formatting
        /// </summary>
        /// <param name="text">The text to add</param>
        /// <param name="fontStyle">The font styling to apply</param>
        /// <returns>The created run</returns>
        IRun AddText(string text, FontStyle fontStyle);
        
        /// <summary>
        /// Add a line break
        /// </summary>
        void AddLineBreak();
    }
}
