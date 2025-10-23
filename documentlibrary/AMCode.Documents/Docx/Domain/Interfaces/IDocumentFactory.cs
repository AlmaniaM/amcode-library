using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx
{
    /// <summary>
    /// Domain interface for document factory operations
    /// </summary>
    public interface IDocumentFactory
    {
        /// <summary>
        /// Create a new document
        /// </summary>
        /// <returns>Result containing the created document or error</returns>
        Result<IDocumentDomain> CreateDocument();

        /// <summary>
        /// Create a new document with specified content
        /// </summary>
        /// <param name="title">Document title</param>
        /// <param name="content">Document content</param>
        /// <returns>Result containing the created document or error</returns>
        Result<IDocumentDomain> CreateDocument(string title, string content);

        /// <summary>
        /// Open an existing document from stream
        /// </summary>
        /// <param name="stream">The stream containing the document</param>
        /// <returns>Result containing the opened document or error</returns>
        Result<IDocumentDomain> OpenDocument(Stream stream);

        /// <summary>
        /// Open an existing document from file path
        /// </summary>
        /// <param name="filePath">The file path to the document</param>
        /// <returns>Result containing the opened document or error</returns>
        Result<IDocumentDomain> OpenDocument(string filePath);
    }
}
