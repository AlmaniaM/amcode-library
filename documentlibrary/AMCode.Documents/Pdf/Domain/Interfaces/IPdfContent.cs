using System;
using System.IO;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Content management interface for PDF documents
    /// </summary>
    public interface IPdfContent
    {
        /// <summary>
        /// Collection of pages
        /// </summary>
        IPages Pages { get; }
        
        /// <summary>
        /// Save content to stream
        /// </summary>
        void SaveAs(Stream stream);
        
        /// <summary>
        /// Save content to file
        /// </summary>
        void SaveAs(string filePath);
    }
}
