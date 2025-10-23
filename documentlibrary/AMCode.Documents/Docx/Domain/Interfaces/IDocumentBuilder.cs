using AMCode.Documents.Docx.Interfaces;
using AMCode.Documents.Docx.Interfaces;
using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx
{
    /// <summary>
    /// Builder interface for creating documents with fluent API
    /// </summary>
    public interface IDocumentBuilder
    {
        /// <summary>
        /// Set the document title
        /// </summary>
        /// <param name="title">The document title</param>
        /// <returns>The builder instance for chaining</returns>
        IDocumentBuilder WithTitle(string title);

        /// <summary>
        /// Set the document author
        /// </summary>
        /// <param name="author">The document author</param>
        /// <returns>The builder instance for chaining</returns>
        IDocumentBuilder WithAuthor(string author);

        /// <summary>
        /// Set the document subject
        /// </summary>
        /// <param name="subject">The document subject</param>
        /// <returns>The builder instance for chaining</returns>
        IDocumentBuilder WithSubject(string subject);

        /// <summary>
        /// Set the document keywords
        /// </summary>
        /// <param name="keywords">The document keywords</param>
        /// <returns>The builder instance for chaining</returns>
        IDocumentBuilder WithKeywords(string keywords);

        /// <summary>
        /// Set the document comments
        /// </summary>
        /// <param name="comments">The document comments</param>
        /// <returns>The builder instance for chaining</returns>
        IDocumentBuilder WithComments(string comments);

        /// <summary>
        /// Add a paragraph to the document
        /// </summary>
        /// <param name="configureParagraph">Action to configure the paragraph</param>
        /// <returns>The builder instance for chaining</returns>
        IDocumentBuilder WithParagraph(Action<IParagraphBuilder> configureParagraph);

        /// <summary>
        /// Add a table to the document
        /// </summary>
        /// <param name="rowCount">Number of rows in the table</param>
        /// <param name="columnCount">Number of columns in the table</param>
        /// <param name="configureTable">Optional action to configure the table</param>
        /// <returns>The builder instance for chaining</returns>
        IDocumentBuilder WithTable(int rowCount, int columnCount, Action<ITable> configureTable = null);

        /// <summary>
        /// Build the document
        /// </summary>
        /// <returns>Result containing the created document or error</returns>
        Result<IDocumentDomain> Build();
    }
}
