using System;
using System.IO;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF generation engine interface
    /// </summary>
    public interface IPdfEngine
    {
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
        
        /// <summary>
        /// Save document to stream
        /// </summary>
        Result SaveAs(IPdfDocument document, Stream stream);
        
        /// <summary>
        /// Save document to file
        /// </summary>
        Result SaveAs(IPdfDocument document, string filePath);
    }
}
