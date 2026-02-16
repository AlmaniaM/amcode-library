using System.IO;
using AMCode.Documents.TextExtraction;
using AMCode.Documents.TextExtraction.Domain.Models;
using NUnit.Framework;
using UglyToad.PdfPig.Writer;

namespace AMCode.Documents.TextExtraction.Tests
{
    [TestFixture]
    public class DocumentTextExtractorFactoryTests
    {
        [SetUp]
        public void SetUp()
        {
            // Start each test with clean state
            DocumentTextExtractorFactory.ClearProviders();
        }

        [TearDown]
        public void TearDown()
        {
            DocumentTextExtractorFactory.ClearProviders();
        }

        [Test]
        public void InitializeDefaultProviders_ThenExtractPdf_ReturnsSuccess()
        {
            // Arrange
            DocumentTextExtractorFactory.InitializeDefaultProviders();
            using var stream = CreatePdfWithText("Factory test");

            // Act
            var result = DocumentTextExtractorFactory.ExtractText(stream, "test.pdf");

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Does.Contain("Factory test"));
        }

        [Test]
        public void ExtractText_WithoutInitialization_ReturnsFailure()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = DocumentTextExtractorFactory.ExtractText(stream, "test.pdf");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("No PDF text extractor configured"));
        }

        [Test]
        public void ExtractText_DocxWithoutInitialization_ReturnsFailure()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = DocumentTextExtractorFactory.ExtractText(stream, "test.docx");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("No DOCX text extractor configured"));
        }

        [Test]
        public void ExtractText_WithNullStream_ReturnsFailure()
        {
            // Arrange
            DocumentTextExtractorFactory.InitializeDefaultProviders();

            // Act
            var result = DocumentTextExtractorFactory.ExtractText(null, "test.pdf");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("Stream cannot be null"));
        }

        [Test]
        public void ExtractText_WithNullFileName_ReturnsFailure()
        {
            // Arrange
            DocumentTextExtractorFactory.InitializeDefaultProviders();
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = DocumentTextExtractorFactory.ExtractText(stream, null);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("File name cannot be null or empty"));
        }

        [Test]
        public void ExtractText_WithUnsupportedFormat_ReturnsFailure()
        {
            // Arrange
            DocumentTextExtractorFactory.InitializeDefaultProviders();
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = DocumentTextExtractorFactory.ExtractText(stream, "file.txt");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("Unsupported file format"));
        }

        [Test]
        public void ClearProviders_ResetsState()
        {
            // Arrange
            DocumentTextExtractorFactory.InitializeDefaultProviders();
            DocumentTextExtractorFactory.ClearProviders();
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = DocumentTextExtractorFactory.ExtractText(stream, "test.pdf");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("No PDF text extractor configured"));
        }

        [Test]
        public void IsFormatSupported_WithPdf_ReturnsTrue()
        {
            // Act & Assert
            Assert.That(DocumentTextExtractorFactory.IsFormatSupported("file.pdf"), Is.True);
            Assert.That(DocumentTextExtractorFactory.IsFormatSupported("file.PDF"), Is.True);
        }

        [Test]
        public void IsFormatSupported_WithDocx_ReturnsTrue()
        {
            // Act & Assert
            Assert.That(DocumentTextExtractorFactory.IsFormatSupported("file.docx"), Is.True);
            Assert.That(DocumentTextExtractorFactory.IsFormatSupported("file.DOCX"), Is.True);
        }

        [Test]
        public void IsFormatSupported_WithUnsupported_ReturnsFalse()
        {
            // Act & Assert
            Assert.That(DocumentTextExtractorFactory.IsFormatSupported("file.txt"), Is.False);
            Assert.That(DocumentTextExtractorFactory.IsFormatSupported("file.xlsx"), Is.False);
            Assert.That(DocumentTextExtractorFactory.IsFormatSupported(""), Is.False);
        }

        [Test]
        public void SupportedExtensions_ContainsPdfAndDocx()
        {
            // Act
            var extensions = DocumentTextExtractorFactory.SupportedExtensions;

            // Assert
            Assert.That(extensions, Does.Contain(".pdf"));
            Assert.That(extensions, Does.Contain(".docx"));
            Assert.That(extensions.Count, Is.EqualTo(2));
        }

        [Test]
        public void SetPdfExtractor_WithNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() =>
                DocumentTextExtractorFactory.SetPdfExtractor(null));
        }

        [Test]
        public void SetDocxExtractor_WithNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() =>
                DocumentTextExtractorFactory.SetDocxExtractor(null));
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

        #endregion
    }
}
