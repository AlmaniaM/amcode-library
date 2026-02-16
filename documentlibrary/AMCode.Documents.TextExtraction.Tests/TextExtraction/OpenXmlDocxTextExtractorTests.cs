using System.IO;
using AMCode.Documents.TextExtraction.Domain.Models;
using AMCode.Documents.TextExtraction.Providers.OpenXml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using NUnit.Framework;

namespace AMCode.Documents.TextExtraction.Tests
{
    [TestFixture]
    public class OpenXmlDocxTextExtractorTests
    {
        private OpenXmlDocxTextExtractor _extractor;

        [SetUp]
        public void SetUp()
        {
            _extractor = new OpenXmlDocxTextExtractor();
        }

        [Test]
        public void ExtractText_WithValidDocx_ReturnsSuccess()
        {
            // Arrange
            using var stream = CreateDocxWithParagraphs("Hello World");

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Is.EqualTo("Hello World"));
            Assert.That(result.Value.Format, Is.EqualTo("docx"));
            Assert.That(result.Value.PageCount, Is.EqualTo(1));
            Assert.That(result.Value.CharacterCount, Is.EqualTo(11));
        }

        [Test]
        public void ExtractText_WithMultipleParagraphs_JoinsWithNewlines()
        {
            // Arrange
            using var stream = CreateDocxWithParagraphs("First paragraph", "Second paragraph", "Third paragraph");

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Is.EqualTo("First paragraph\nSecond paragraph\nThird paragraph"));
        }

        [Test]
        public void ExtractText_WithPreserveLineBreaksFalse_JoinsWithSpaces()
        {
            // Arrange
            using var stream = CreateDocxWithParagraphs("First", "Second");
            var options = new TextExtractionOptions { PreserveLineBreaks = false };

            // Act
            var result = _extractor.ExtractText(stream, options);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Is.EqualTo("First Second"));
        }

        [Test]
        public void ExtractText_WithTable_ExtractsTableText()
        {
            // Arrange
            using var stream = CreateDocxWithTable(
                new[] { "Name", "Age" },
                new[] { "Alice", "30" },
                new[] { "Bob", "25" });

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("Name | Age"));
            Assert.That(result.Value.Text, Does.Contain("Alice | 30"));
            Assert.That(result.Value.Text, Does.Contain("Bob | 25"));
        }

        [Test]
        public void ExtractText_WithMixedContent_ExtractsParagraphsAndTables()
        {
            // Arrange
            using var stream = CreateDocxWithMixedContent();

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("Intro paragraph"));
            Assert.That(result.Value.Text, Does.Contain("Cell1 | Cell2"));
            Assert.That(result.Value.Text, Does.Contain("Closing paragraph"));
        }

        [Test]
        public void ExtractText_WithNullStream_ReturnsFailure()
        {
            // Act
            var result = _extractor.ExtractText(null);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("empty"));
        }

        [Test]
        public void ExtractText_WithEmptyStream_ReturnsFailure()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("empty"));
        }

        [Test]
        public void ExtractText_WithCorruptedStream_ReturnsFailure()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02, 0x03 });

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("Failed to extract text from DOCX"));
        }

        [Test]
        public void ExtractText_WithEmptyDocx_ReturnsFailure()
        {
            // Arrange
            using var stream = CreateDocxWithParagraphs(); // No paragraphs

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("No text could be extracted"));
        }

        [Test]
        public void ExtractText_WithNullOptions_UsesDefaults()
        {
            // Arrange
            using var stream = CreateDocxWithParagraphs("Test content");

            // Act
            var result = _extractor.ExtractText(stream, null);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Is.EqualTo("Test content"));
        }

        [Test]
        public void ExtractText_SkipsWhitespaceOnlyParagraphs()
        {
            // Arrange
            using var stream = CreateDocxWithParagraphs("Real content", "   ", "More content");

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Is.EqualTo("Real content\nMore content"));
        }

        #region Helpers

        private static MemoryStream CreateDocxWithParagraphs(params string[] paragraphTexts)
        {
            var stream = new MemoryStream();
            using (var doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                var body = new Body();

                foreach (var text in paragraphTexts)
                {
                    var paragraph = new Paragraph(
                        new Run(
                            new Text(text) { Space = SpaceProcessingModeValues.Preserve }));
                    body.Append(paragraph);
                }

                mainPart.Document = new Document(body);
                mainPart.Document.Save();
            }

            stream.Position = 0;
            return stream;
        }

        private static MemoryStream CreateDocxWithTable(params string[][] rows)
        {
            var stream = new MemoryStream();
            using (var doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                var body = new Body();
                var table = new Table();

                foreach (var row in rows)
                {
                    var tableRow = new TableRow();
                    foreach (var cellText in row)
                    {
                        var cell = new TableCell(
                            new Paragraph(
                                new Run(
                                    new Text(cellText))));
                        tableRow.Append(cell);
                    }
                    table.Append(tableRow);
                }

                body.Append(table);
                mainPart.Document = new Document(body);
                mainPart.Document.Save();
            }

            stream.Position = 0;
            return stream;
        }

        private static MemoryStream CreateDocxWithMixedContent()
        {
            var stream = new MemoryStream();
            using (var doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                var body = new Body();

                // Paragraph before table
                body.Append(new Paragraph(new Run(new Text("Intro paragraph"))));

                // Table
                var table = new Table();
                var row = new TableRow();
                row.Append(new TableCell(new Paragraph(new Run(new Text("Cell1")))));
                row.Append(new TableCell(new Paragraph(new Run(new Text("Cell2")))));
                table.Append(row);
                body.Append(table);

                // Paragraph after table
                body.Append(new Paragraph(new Run(new Text("Closing paragraph"))));

                mainPart.Document = new Document(body);
                mainPart.Document.Save();
            }

            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}
