using System;
using System.IO;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Represents a PDF document
    /// </summary>
    public interface IPdfDocument : IDisposable
    {
        /// <summary>
        /// Document identifier
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// Document creation timestamp
        /// </summary>
        DateTime CreatedAt { get; }
        
        /// <summary>
        /// Document last modified timestamp
        /// </summary>
        DateTime LastModified { get; set; }
        
        /// <summary>
        /// Collection of pages
        /// </summary>
        IPages Pages { get; }
        
        /// <summary>
        /// Document properties
        /// </summary>
        IPdfProperties Properties { get; }
        
        /// <summary>
        /// Save document to stream
        /// </summary>
        void SaveAs(Stream stream);
        
        /// <summary>
        /// Save document to file
        /// </summary>
        void SaveAs(string filePath);
        
        /// <summary>
        /// Close the document
        /// </summary>
        void Close();
    }
}
