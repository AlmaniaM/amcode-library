using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMCode.Documents.Common.Models;
using AMCode.Documents.TextExtraction.Domain.Interfaces;
using AMCode.Documents.TextExtraction.Domain.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AMCode.Documents.TextExtraction.Providers.OpenXml
{
    /// <summary>
    /// DOCX text extractor using DocumentFormat.OpenXml.
    /// Extracts text from paragraphs and tables in Word documents.
    /// </summary>
    public class OpenXmlDocxTextExtractor : IDocxTextExtractor
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
                    return Result<TextExtractionResult>.Failure("DOCX stream is empty");
                }

                stream.Position = 0;

                using var document = WordprocessingDocument.Open(stream, false);
                var body = document.MainDocumentPart?.Document?.Body;

                if (body == null)
                {
                    return Result<TextExtractionResult>.Failure("DOCX file has no document body");
                }

                var textParts = new List<string>();

                // Iterate top-level child elements to avoid double-counting
                // paragraphs that are inside tables
                foreach (var element in body.ChildElements)
                {
                    if (element is Paragraph paragraph)
                    {
                        var paragraphText = paragraph.InnerText;
                        if (!string.IsNullOrWhiteSpace(paragraphText))
                        {
                            textParts.Add(options.TrimWhitespace ? paragraphText.Trim() : paragraphText);
                        }
                    }
                    else if (element is Table table)
                    {
                        ExtractTableText(table, textParts, options);
                    }
                }

                var separator = options.PreserveLineBreaks ? "\n" : " ";
                var text = string.Join(separator, textParts);

                if (string.IsNullOrWhiteSpace(text))
                {
                    return Result<TextExtractionResult>.Failure(
                        "No text could be extracted from DOCX file");
                }

                return Result<TextExtractionResult>.Success(
                    new TextExtractionResult(text, 1, "docx", warnings));
            }
            catch (Exception ex)
            {
                return Result<TextExtractionResult>.Failure(
                    $"Failed to extract text from DOCX: {ex.Message}", ex);
            }
        }

        private static void ExtractTableText(Table table, List<string> textParts, TextExtractionOptions options)
        {
            foreach (var row in table.Elements<TableRow>())
            {
                var cellTexts = row
                    .Elements<TableCell>()
                    .Select(cell => cell.InnerText?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t));

                var rowText = string.Join(" | ", cellTexts);
                if (!string.IsNullOrWhiteSpace(rowText))
                {
                    textParts.Add(options.TrimWhitespace ? rowText.Trim() : rowText);
                }
            }
        }
    }
}
