using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Documents.Common.Enums;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx
{
    /// <summary>
    /// Enhanced paragraph interface with proper error handling
    /// </summary>
    public interface IParagraphEnhanced : IParagraphContent, IParagraphFormatting
    {
        /// <summary>
        /// Parent paragraphs collection
        /// </summary>
        IParagraphs Paragraphs { get; }
        
        /// <summary>
        /// Remove the paragraph with validation
        /// </summary>
        /// <returns>Result indicating success or failure</returns>
        Result Remove();
        
        /// <summary>
        /// Validate the paragraph
        /// </summary>
        /// <returns>Validation result</returns>
        Result Validate();
        
        /// <summary>
        /// Set alignment with validation
        /// </summary>
        /// <param name="alignment">The alignment to set</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetAlignment(HorizontalAlignment alignment);
        
        /// <summary>
        /// Set spacing with validation
        /// </summary>
        /// <param name="before">Spacing before in points</param>
        /// <param name="after">Spacing after in points</param>
        /// <param name="line">Line spacing</param>
        /// <returns>Result indicating success or failure</returns>
        Result SetSpacing(double before, double after, double line);
    }
}
