using System;
using AMCode.Documents.Common.Enums;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a paragraph in a Word document
    /// </summary>
    public interface IParagraph
    {
        /// <summary>
        /// Collection of runs (text segments)
        /// </summary>
        IRuns Runs { get; }
        
        /// <summary>
        /// Paragraph text content
        /// </summary>
        string Text { get; set; }
        
        /// <summary>
        /// Paragraph alignment
        /// </summary>
        HorizontalAlignment Alignment { get; set; }
        
        /// <summary>
        /// Paragraph spacing before (in points)
        /// </summary>
        double SpacingBefore { get; set; }
        
        /// <summary>
        /// Paragraph spacing after (in points)
        /// </summary>
        double SpacingAfter { get; set; }
        
        /// <summary>
        /// Line spacing
        /// </summary>
        double LineSpacing { get; set; }
        
        /// <summary>
        /// Parent paragraphs collection
        /// </summary>
        IParagraphs Paragraphs { get; }
        
        /// <summary>
        /// Add text to the paragraph
        /// </summary>
        IRun AddText(string text);
        
        /// <summary>
        /// Add text with specific formatting
        /// </summary>
        IRun AddText(string text, FontStyle fontStyle);
        
        /// <summary>
        /// Add a line break
        /// </summary>
        void AddLineBreak();
        
        /// <summary>
        /// Remove the paragraph
        /// </summary>
        void Remove();
    }
}
