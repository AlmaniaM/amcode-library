using System;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF document properties interface
    /// </summary>
    public interface IPdfProperties
    {
        /// <summary>
        /// Document title
        /// </summary>
        string Title { get; set; }
        
        /// <summary>
        /// Document author
        /// </summary>
        string Author { get; set; }
        
        /// <summary>
        /// Document subject
        /// </summary>
        string Subject { get; set; }
        
        /// <summary>
        /// Document keywords
        /// </summary>
        string Keywords { get; set; }
        
        /// <summary>
        /// Document creator
        /// </summary>
        string Creator { get; set; }
        
        /// <summary>
        /// Document producer
        /// </summary>
        string Producer { get; set; }
        
        /// <summary>
        /// Document creation date
        /// </summary>
        DateTime? CreationDate { get; set; }
        
        /// <summary>
        /// Document modification date
        /// </summary>
        DateTime? ModificationDate { get; set; }
    }
}
