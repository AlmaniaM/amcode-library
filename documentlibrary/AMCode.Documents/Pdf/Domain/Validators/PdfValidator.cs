using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF document validator
    /// </summary>
    public class PdfValidator : IPdfValidator
    {
        /// <summary>
        /// Validate a PDF document
        /// </summary>
        public Result ValidateDocument(IPdfDocument document)
        {
            if (document == null)
                return Result.Failure("Document cannot be null");

            if (document.Pages == null)
                return Result.Failure("Document must have a pages collection");

            if (document.Pages.Count == 0)
                return Result.Failure("Document must have at least one page");

            if (document.Properties == null)
                return Result.Failure("Document properties cannot be null");

            // Validate each page
            for (int i = 0; i < document.Pages.Count; i++)
            {
                var pageResult = ValidatePage(document.Pages[i]);
                if (!pageResult.IsSuccess)
                    return Result.Failure($"Page {i + 1} validation failed: {pageResult.Error}");
            }

            // Validate properties
            var propertiesResult = ValidateProperties(document.Properties);
            if (!propertiesResult.IsSuccess)
                return Result.Failure($"Properties validation failed: {propertiesResult.Error}");

            return Result.Success();
        }

        /// <summary>
        /// Validate a PDF page
        /// </summary>
        public Result ValidatePage(IPage page)
        {
            if (page == null)
                return Result.Failure("Page cannot be null");

            if (page.PageNumber < 1)
                return Result.Failure("Page number must be greater than 0");

            if (page.Size == null)
                return Result.Failure("Page size cannot be null");

            if (page.Margins == null)
                return Result.Failure("Page margins cannot be null");

            if (page.Document == null)
                return Result.Failure("Page must belong to a document");

            // Validate margins
            if (page.Margins.Top < 0 || page.Margins.Bottom < 0 || 
                page.Margins.Left < 0 || page.Margins.Right < 0)
                return Result.Failure("Page margins cannot be negative");

            return Result.Success();
        }

        /// <summary>
        /// Validate PDF properties
        /// </summary>
        public Result ValidateProperties(IPdfProperties properties)
        {
            if (properties == null)
                return Result.Failure("Properties cannot be null");

            if (string.IsNullOrWhiteSpace(properties.Title))
                return Result.Failure("Document title cannot be empty");

            if (string.IsNullOrWhiteSpace(properties.Author))
                return Result.Failure("Document author cannot be empty");

            if (properties.CreationDate.HasValue && properties.CreationDate.Value > DateTime.UtcNow)
                return Result.Failure("Creation date cannot be in the future");

            if (properties.ModificationDate.HasValue && properties.ModificationDate.Value > DateTime.UtcNow)
                return Result.Failure("Modification date cannot be in the future");

            if (properties.CreationDate.HasValue && properties.ModificationDate.HasValue && 
                properties.ModificationDate.Value < properties.CreationDate.Value)
                return Result.Failure("Modification date cannot be before creation date");

            return Result.Success();
        }
    }
}
