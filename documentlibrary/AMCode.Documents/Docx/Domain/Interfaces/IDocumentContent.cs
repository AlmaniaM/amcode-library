using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;

namespace AMCode.Docx
{
    /// <summary>
    /// Domain interface for document content operations
    /// </summary>
    public interface IDocumentContent
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
        /// Document sections
        /// </summary>
        ISections Sections { get; }
        
        /// <summary>
        /// Save document to stream
        /// </summary>
        /// <param name="stream">The stream to save to</param>
        void SaveAs(Stream stream);
        
        /// <summary>
        /// Save document to file
        /// </summary>
        /// <param name="filePath">The file path to save to</param>
        void SaveAs(string filePath);
    }
}
