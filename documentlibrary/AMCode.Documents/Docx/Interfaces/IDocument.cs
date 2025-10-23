using System;
using System.IO;
using System.Threading.Tasks;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Docx.Interfaces
{
    /// <summary>
    /// Represents a Word document
    /// </summary>
    public interface IDocument : IDisposable
    {
        /// <summary>
        /// Collection of paragraphs
        /// </summary>
        IParagraphs Paragraphs { get; }
        
        /// <summary>
        /// Collection of tables
        /// </summary>
        ITables Tables { get; }
        
        /// <summary>
        /// Document properties
        /// </summary>
        IDocumentProperties Properties { get; }
        
        /// <summary>
        /// Document sections
        /// </summary>
        ISections Sections { get; }
        
        /// <summary>
        /// Collection of pages
        /// </summary>
        IDocumentPages Pages { get; }
        
        /// <summary>
        /// Save document to stream
        /// </summary>
        void SaveAs(Stream stream);
        
        /// <summary>
        /// Save document to file
        /// </summary>
        void SaveAs(string filePath);
        
        /// <summary>
        /// Save document as stream async
        /// </summary>
        Task<Stream> SaveAsStreamAsync();
        
        /// <summary>
        /// Close the document
        /// </summary>
        void Close();
    }
}
