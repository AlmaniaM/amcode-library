using System;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// PDF document-specific validator
    /// </summary>
    public class PdfDocumentValidator
    {
        private readonly IPdfValidator _baseValidator;

        /// <summary>
        /// Create PDF document validator
        /// </summary>
        public PdfDocumentValidator(IPdfValidator baseValidator = null)
        {
            _baseValidator = baseValidator ?? new PdfValidator();
        }

        /// <summary>
        /// Validate document structure
        /// </summary>
        public Result ValidateStructure(IPdfDocument document)
        {
            if (document == null)
                return Result.Failure("Document cannot be null");

            // Validate basic structure
            var basicResult = _baseValidator.ValidateDocument(document);
            if (!basicResult.IsSuccess)
                return basicResult;

            // Validate document ID
            if (document.Id == Guid.Empty)
                return Result.Failure("Document must have a valid ID");

            // Validate timestamps
            if (document.CreatedAt == DateTime.MinValue)
                return Result.Failure("Document must have a valid creation timestamp");

            if (document.LastModified < document.CreatedAt)
                return Result.Failure("Last modified date cannot be before creation date");

            return Result.Success();
        }

        /// <summary>
        /// Validate document content
        /// </summary>
        public Result ValidateContent(IPdfDocument document)
        {
            if (document == null)
                return Result.Failure("Document cannot be null");

            // Check if document has content
            if (document.Pages.Count == 0)
                return Result.Failure("Document must have at least one page");

            // Validate each page has content
            for (int i = 0; i < document.Pages.Count; i++)
            {
                var page = document.Pages[i];
                if (page == null)
                    return Result.Failure($"Page {i + 1} cannot be null");
            }

            return Result.Success();
        }

        /// <summary>
        /// Validate document for saving
        /// </summary>
        public Result ValidateForSaving(IPdfDocument document)
        {
            if (document == null)
                return Result.Failure("Document cannot be null");

            // Validate structure
            var structureResult = ValidateStructure(document);
            if (!structureResult.IsSuccess)
                return structureResult;

            // Validate content
            var contentResult = ValidateContent(document);
            if (!contentResult.IsSuccess)
                return contentResult;

            // Validate properties are complete
            if (string.IsNullOrWhiteSpace(document.Properties.Title))
                return Result.Failure("Document title is required for saving");

            if (string.IsNullOrWhiteSpace(document.Properties.Author))
                return Result.Failure("Document author is required for saving");

            return Result.Success();
        }
    }
}
