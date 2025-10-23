using System;
using AMCode.Documents.Common.Models;
using System.IO;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Provider abstraction for PDF generation
    /// </summary>
    public interface IPdfProvider
    {
        /// <summary>
        /// Provider name
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Provider version
        /// </summary>
        string Version { get; }
        
        /// <summary>
        /// Create a new PDF document
        /// </summary>
        Result<IPdfDocument> CreateDocument();
        
        /// <summary>
        /// Create a new PDF document with properties
        /// </summary>
        Result<IPdfDocument> CreateDocument(IPdfProperties properties);
        
        /// <summary>
        /// Open existing PDF document from stream
        /// </summary>
        Result<IPdfDocument> OpenDocument(Stream stream);
        
        /// <summary>
        /// Open existing PDF document from file
        /// </summary>
        Result<IPdfDocument> OpenDocument(string filePath);
    }
}
