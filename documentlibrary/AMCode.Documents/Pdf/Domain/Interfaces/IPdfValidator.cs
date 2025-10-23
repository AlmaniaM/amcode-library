using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF document validation interface
    /// </summary>
    public interface IPdfValidator
    {
        /// <summary>
        /// Validate a PDF document
        /// </summary>
        Result ValidateDocument(IPdfDocument document);
        
        /// <summary>
        /// Validate a PDF page
        /// </summary>
        Result ValidatePage(IPage page);
        
        /// <summary>
        /// Validate PDF properties
        /// </summary>
        Result ValidateProperties(IPdfProperties properties);
    }
}
