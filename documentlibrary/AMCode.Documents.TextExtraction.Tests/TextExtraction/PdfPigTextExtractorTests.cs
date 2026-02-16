using System.IO;
using AMCode.Documents.TextExtraction.Domain.Models;
using AMCode.Documents.TextExtraction.Providers.PdfPig;
using NUnit.Framework;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Writer;

namespace AMCode.Documents.TextExtraction.Tests
{
    [TestFixture]
    public class PdfPigTextExtractorTests
    {
        private PdfPigTextExtractor _extractor;

        [SetUp]
        public void SetUp()
        {
            _extractor = new PdfPigTextExtractor();
        }

        [Test]
        public void ExtractText_WithValidPdf_ReturnsSuccess()
        {
            // Arrange
            using var stream = CreatePdfWithText("Hello World");

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("Hello World"));
            Assert.That(result.Value.Format, Is.EqualTo("pdf"));
            Assert.That(result.Value.PageCount, Is.EqualTo(1));
            Assert.That(result.Value.CharacterCount, Is.GreaterThan(0));
        }

        [Test]
        public void ExtractText_WithMultiPagePdf_ExtractsAllPages()
        {
            // Arrange
            using var stream = CreateMultiPagePdf("Page One Content", "Page Two Content");

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("Page One Content"));
            Assert.That(result.Value.Text, Does.Contain("Page Two Content"));
            Assert.That(result.Value.PageCount, Is.EqualTo(2));
        }

        [Test]
        public void ExtractText_WithMaxPagesOption_LimitsExtraction()
        {
            // Arrange
            using var stream = CreateMultiPagePdf("Page One", "Page Two", "Page Three");
            var options = new TextExtractionOptions { MaxPages = 1 };

            // Act
            var result = _extractor.ExtractText(stream, options);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("Page One"));
            Assert.That(result.Value.Text, Does.Not.Contain("Page Two"));
            Assert.That(result.Value.PageCount, Is.EqualTo(3)); // Total pages in doc
            Assert.That(result.Value.Warnings.Count, Is.GreaterThan(0));
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
            Assert.That(result.Error, Does.Contain("Failed to extract text from PDF"));
        }

        [Test]
        public void ExtractText_WithCustomPageSeparator_UsesIt()
        {
            // Arrange
            using var stream = CreateMultiPagePdf("PageA", "PageB");
            var options = new TextExtractionOptions { PageSeparator = "---" };

            // Act
            var result = _extractor.ExtractText(stream, options);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("---"));
        }

        [Test]
        public void ExtractText_WithDefaultOptions_TrimsWhitespace()
        {
            // Arrange
            using var stream = CreatePdfWithText("  Hello  ");

            // Act
            var result = _extractor.ExtractText(stream);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            // PdfPig may not preserve leading/trailing spaces exactly,
            // but with TrimWhitespace=true (default), any extra spaces get trimmed
            Assert.That(result.Value.Text, Does.Not.StartWith(" "));
        }

        [Test]
        public void ExtractText_WithNullOptions_UsesDefaults()
        {
            // Arrange
            using var stream = CreatePdfWithText("Test content");

            // Act
            var result = _extractor.ExtractText(stream, null);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("Test content"));
        }

        #region Helpers

        private static MemoryStream CreatePdfWithText(string text)
        {
            var builder = new PdfDocumentBuilder();
            var page = builder.AddPage(UglyToad.PdfPig.Content.PageSize.A4);
            var font = builder.AddStandard14Font(UglyToad.PdfPig.Fonts.Standard14Fonts.Standard14Font.Helvetica);
            page.AddText(text, 12, new UglyToad.PdfPig.Core.PdfPoint(50, 700), font);
            var bytes = builder.Build();

            var stream = new MemoryStream(bytes);
            stream.Position = 0;
            return stream;
        }

        private static MemoryStream CreateMultiPagePdf(params string[] pageTexts)
        {
            var builder = new PdfDocumentBuilder();
            var font = builder.AddStandard14Font(UglyToad.PdfPig.Fonts.Standard14Fonts.Standard14Font.Helvetica);

            foreach (var text in pageTexts)
            {
                var page = builder.AddPage(UglyToad.PdfPig.Content.PageSize.A4);
                page.AddText(text, 12, new UglyToad.PdfPig.Core.PdfPoint(50, 700), font);
            }

            var bytes = builder.Build();
            var stream = new MemoryStream(bytes);
            stream.Position = 0;
            return stream;
        }

        #endregion
    }
}
