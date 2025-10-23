using System;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Document properties and metadata
    /// </summary>
    public interface IDocumentProperties
    {
        /// <summary>
        /// Document title
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// Document subject
        /// </summary>
        string Subject { get; set; }
        
        /// <summary>
        /// Document author
        /// </summary>
        string Author { get; set; }
        
        /// <summary>
        /// Document keywords
        /// </summary>
        string Keywords { get; set; }
        
        /// <summary>
        /// Document comments
        /// </summary>
        string Comments { get; set; }
        
        /// <summary>
        /// Document category
        /// </summary>
        string Category { get; set; }
        
        /// <summary>
        /// Document company
        /// </summary>
        string Company { get; set; }
        
        /// <summary>
        /// Document manager
        /// </summary>
        string Manager { get; set; }
        
        /// <summary>
        /// Document description
        /// </summary>
        string Description { get; set; }
        
        /// <summary>
        /// Document version
        /// </summary>
        string Version { get; set; }
        
        /// <summary>
        /// Document creation date
        /// </summary>
        DateTime Created { get; set; }
        
        /// <summary>
        /// Document last modified date
        /// </summary>
        DateTime Modified { get; set; }
        
        /// <summary>
        /// Document last printed date
        /// </summary>
        DateTime LastPrinted { get; set; }
        
        /// <summary>
        /// Document revision number
        /// </summary>
        int Revision { get; set; }
        
        /// <summary>
        /// Document page count
        /// </summary>
        int PageCount { get; }
        
        /// <summary>
        /// Document word count
        /// </summary>
        int WordCount { get; }
        
        /// <summary>
        /// Document character count
        /// </summary>
        int CharacterCount { get; }
    }
}
