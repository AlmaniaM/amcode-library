using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Fluent builder interface for PDF documents
    /// </summary>
    public interface IPdfBuilder
    {
        /// <summary>
        /// Set document title
        /// </summary>
        IPdfBuilder WithTitle(string title);
        
        /// <summary>
        /// Set document author
        /// </summary>
        IPdfBuilder WithAuthor(string author);
        
        /// <summary>
        /// Set document subject
        /// </summary>
        IPdfBuilder WithSubject(string subject);
        
        /// <summary>
        /// Set document keywords
        /// </summary>
        IPdfBuilder WithKeywords(string keywords);
        
        /// <summary>
        /// Add a page with default configuration
        /// </summary>
        IPdfBuilder WithPage(Action<IPage> configurePage);
        
        /// <summary>
        /// Add a page with specific size
        /// </summary>
        IPdfBuilder WithPage(PageSize pageSize, Action<IPage> configurePage);
        
        /// <summary>
        /// Build the PDF document
        /// </summary>
        Result<IPdfDocument> Build();
    }
}
