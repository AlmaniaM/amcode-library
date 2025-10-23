using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;

namespace AMCode.Docx
{
    /// <summary>
    /// Domain interface for document metadata operations
    /// </summary>
    public interface IDocumentMetadata
    {
        /// <summary>
        /// Document properties
        /// </summary>
        IDocumentProperties Properties { get; }
        
        /// <summary>
        /// Close the document
        /// </summary>
        void Close();
    }
}
