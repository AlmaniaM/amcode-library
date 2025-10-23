using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx
{
    /// <summary>
    /// Domain interface for document validation
    /// </summary>
    public interface IDocumentValidator
    {
        /// <summary>
        /// Validate a document
        /// </summary>
        /// <param name="document">The document to validate</param>
        /// <returns>Validation result</returns>
        Result ValidateDocument(IDocumentDomain document);

        /// <summary>
        /// Validate a paragraph
        /// </summary>
        /// <param name="paragraph">The paragraph to validate</param>
        /// <returns>Validation result</returns>
        Result ValidateParagraph(IParagraph paragraph);

        /// <summary>
        /// Validate a table
        /// </summary>
        /// <param name="table">The table to validate</param>
        /// <returns>Validation result</returns>
        Result ValidateTable(ITable table);

        /// <summary>
        /// Validate document properties
        /// </summary>
        /// <param name="properties">The properties to validate</param>
        /// <returns>Validation result</returns>
        Result ValidateDocumentProperties(IDocumentProperties properties);
    }
}
