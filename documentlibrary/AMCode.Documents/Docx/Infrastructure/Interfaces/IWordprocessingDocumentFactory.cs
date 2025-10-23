using AMCode.Documents.Docx.Interfaces;
using System;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx.Infrastructure.Interfaces
{
    /// <summary>
    /// Infrastructure interface for WordprocessingDocument creation
    /// </summary>
    public interface IWordprocessingDocumentFactory
    {
        /// <summary>
        /// Create a new WordprocessingDocument
        /// </summary>
        /// <returns>Result containing the created document or error</returns>
        Result<WordprocessingDocument> Create();

        /// <summary>
        /// Open a WordprocessingDocument from stream
        /// </summary>
        /// <param name="stream">The stream containing the document</param>
        /// <returns>Result containing the opened document or error</returns>
        Result<WordprocessingDocument> Open(Stream stream);

        /// <summary>
        /// Open a WordprocessingDocument from file path
        /// </summary>
        /// <param name="filePath">The file path to the document</param>
        /// <returns>Result containing the opened document or error</returns>
        Result<WordprocessingDocument> Open(string filePath);
    }
}
