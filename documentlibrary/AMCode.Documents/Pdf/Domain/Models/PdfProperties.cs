using System;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF document properties model
    /// </summary>
    public class PdfProperties : IPdfProperties
    {
        /// <summary>
        /// Document title
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Document author
        /// </summary>
        public string Author { get; set; } = string.Empty;
        
        /// <summary>
        /// Document subject
        /// </summary>
        public string Subject { get; set; } = string.Empty;
        
        /// <summary>
        /// Document keywords
        /// </summary>
        public string Keywords { get; set; } = string.Empty;
        
        /// <summary>
        /// Document creator
        /// </summary>
        public string Creator { get; set; } = "AMCode.Pdf";
        
        /// <summary>
        /// Document producer
        /// </summary>
        public string Producer { get; set; } = "AMCode.Pdf";
        
        /// <summary>
        /// Document creation date
        /// </summary>
        public DateTime? CreationDate { get; set; }
        
        /// <summary>
        /// Document modification date
        /// </summary>
        public DateTime? ModificationDate { get; set; }
        
        /// <summary>
        /// Create PDF properties with default values
        /// </summary>
        public PdfProperties()
        {
            CreationDate = DateTime.UtcNow;
            ModificationDate = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Create PDF properties with specific values
        /// </summary>
        public PdfProperties(string title, string author, string subject = null, string keywords = null)
        {
            Title = title ?? string.Empty;
            Author = author ?? string.Empty;
            Subject = subject ?? string.Empty;
            Keywords = keywords ?? string.Empty;
            CreationDate = DateTime.UtcNow;
            ModificationDate = DateTime.UtcNow;
        }
    }
}
