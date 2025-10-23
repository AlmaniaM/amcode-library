using AMCode.Documents.Docx.Interfaces;
using System;
using System.Linq;
using AMCode.Docx;
using AMCode.Documents.Common.Models;

namespace AMCode.Docx.Domain.Validators
{
    /// <summary>
    /// Domain validator for document operations
    /// </summary>
    public class DocumentValidator : IDocumentValidator
    {
        public Result ValidateDocument(IDocumentDomain document)
        {
            if (document == null)
                return Result.Failure("Document cannot be null");

            if (document.Id == Guid.Empty)
                return Result.Failure("Document must have a valid ID");

            if (document.CreatedAt == default)
                return Result.Failure("Document must have a valid creation date");

            if (document.Paragraphs == null)
                return Result.Failure("Document must have a paragraphs collection");

            if (document.Tables == null)
                return Result.Failure("Document must have a tables collection");

            if (document.Sections == null)
                return Result.Failure("Document must have a sections collection");

            if (document.Properties == null)
                return Result.Failure("Document must have properties");

            return Result.Success();
        }

        public Result ValidateParagraph(IParagraph paragraph)
        {
            if (paragraph == null)
                return Result.Failure("Paragraph cannot be null");

            if (paragraph.Runs == null)
                return Result.Failure("Paragraph must have a runs collection");

            if (paragraph.SpacingBefore < 0)
                return Result.Failure("Paragraph spacing before cannot be negative");

            if (paragraph.SpacingAfter < 0)
                return Result.Failure("Paragraph spacing after cannot be negative");

            if (paragraph.LineSpacing < 0)
                return Result.Failure("Paragraph line spacing cannot be negative");

            return Result.Success();
        }

        public Result ValidateTable(ITable table)
        {
            if (table == null)
                return Result.Failure("Table cannot be null");

            if (table.Rows == null)
                return Result.Failure("Table must have a rows collection");

            if (table.Columns == null)
                return Result.Failure("Table must have a columns collection");

            if (table.RowCount < 0)
                return Result.Failure("Table row count cannot be negative");

            if (table.ColumnCount < 0)
                return Result.Failure("Table column count cannot be negative");

            return Result.Success();
        }

        public Result ValidateDocumentProperties(IDocumentProperties properties)
        {
            if (properties == null)
                return Result.Failure("Document properties cannot be null");

            if (properties.Created > DateTime.UtcNow)
                return Result.Failure("Document creation date cannot be in the future");

            if (properties.Modified > DateTime.UtcNow)
                return Result.Failure("Document modification date cannot be in the future");

            if (properties.PageCount < 0)
                return Result.Failure("Document page count cannot be negative");

            if (properties.Revision < 0)
                return Result.Failure("Document revision cannot be negative");

            return Result.Success();
        }
    }
}
