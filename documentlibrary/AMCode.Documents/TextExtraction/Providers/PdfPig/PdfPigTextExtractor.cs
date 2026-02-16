using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMCode.Documents.Common.Models;
using AMCode.Documents.TextExtraction.Domain.Interfaces;
using AMCode.Documents.TextExtraction.Domain.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace AMCode.Documents.TextExtraction.Providers.PdfPig
{
    /// <summary>
    /// PDF text extractor using the PdfPig library (MIT license).
    /// Extracts text content from PDF documents with page-aware processing.
    /// </summary>
    public class PdfPigTextExtractor : IPdfTextExtractor
    {
        /// <inheritdoc />
        public Result<TextExtractionResult> ExtractText(Stream stream, TextExtractionOptions options = null)
        {
            options ??= new TextExtractionOptions();
            var warnings = new List<string>();

            try
            {
                if (stream == null || stream.Length == 0)
                {
                    return Result<TextExtractionResult>.Failure("PDF stream is empty");
                }

                stream.Position = 0;

                using var document = PdfDocument.Open(stream);
                var totalPages = document.NumberOfPages;
                var maxPages = options.MaxPages ?? totalPages;
                var pagesToProcess = Math.Min(totalPages, maxPages);
                var pageTexts = new List<string>();

                for (var i = 1; i <= pagesToProcess; i++)
                {
                    var page = document.GetPage(i);
                    var pageText = ContentOrderTextExtractor.GetText(page);

                    if (string.IsNullOrWhiteSpace(pageText))
                    {
                        warnings.Add($"Page {i} had no extractable text (may be scanned/image-based)");
                        continue;
                    }

                    pageTexts.Add(options.TrimWhitespace ? pageText.Trim() : pageText);
                }

                if (maxPages < totalPages)
                {
                    warnings.Add($"Only extracted {maxPages} of {totalPages} pages (MaxPages limit)");
                }

                var text = string.Join(options.PageSeparator, pageTexts);

                if (string.IsNullOrWhiteSpace(text))
                {
                    return Result<TextExtractionResult>.Failure(
                        "No text could be extracted from PDF. The file may contain only images.");
                }

                return Result<TextExtractionResult>.Success(
                    new TextExtractionResult(text, totalPages, "pdf", warnings));
            }
            catch (Exception ex)
            {
                return Result<TextExtractionResult>.Failure(
                    $"Failed to extract text from PDF: {ex.Message}", ex);
            }
        }
    }
}
