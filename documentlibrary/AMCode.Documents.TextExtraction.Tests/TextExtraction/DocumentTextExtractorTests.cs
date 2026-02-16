using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.TextExtraction;
using AMCode.Documents.TextExtraction.Domain.Interfaces;
using AMCode.Documents.TextExtraction.Domain.Models;
using Moq;
using NUnit.Framework;

namespace AMCode.Documents.TextExtraction.Tests
{
    [TestFixture]
    public class DocumentTextExtractorTests
    {
        private Mock<IPdfTextExtractor> _mockPdfExtractor;
        private Mock<IDocxTextExtractor> _mockDocxExtractor;
        private DocumentTextExtractor _extractor;

        [SetUp]
        public void SetUp()
        {
            _mockPdfExtractor = new Mock<IPdfTextExtractor>();
            _mockDocxExtractor = new Mock<IDocxTextExtractor>();
            _extractor = new DocumentTextExtractor(_mockPdfExtractor.Object, _mockDocxExtractor.Object);
        }

        [Test]
        public void ExtractText_WithPdfFile_RoutesToPdfExtractor()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            var expected = new TextExtractionResult("PDF text", 1, "pdf");
            _mockPdfExtractor
                .Setup(x => x.ExtractText(stream, null))
                .Returns(Result<TextExtractionResult>.Success(expected));

            // Act
            var result = _extractor.ExtractText(stream, "document.pdf");

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Is.EqualTo("PDF text"));
            _mockPdfExtractor.Verify(x => x.ExtractText(stream, null), Times.Once);
            _mockDocxExtractor.Verify(x => x.ExtractText(It.IsAny<Stream>(), It.IsAny<TextExtractionOptions>()), Times.Never);
        }

        [Test]
        public void ExtractText_WithDocxFile_RoutesToDocxExtractor()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            var expected = new TextExtractionResult("DOCX text", 1, "docx");
            _mockDocxExtractor
                .Setup(x => x.ExtractText(stream, null))
                .Returns(Result<TextExtractionResult>.Success(expected));

            // Act
            var result = _extractor.ExtractText(stream, "document.docx");

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Text, Is.EqualTo("DOCX text"));
            _mockDocxExtractor.Verify(x => x.ExtractText(stream, null), Times.Once);
            _mockPdfExtractor.Verify(x => x.ExtractText(It.IsAny<Stream>(), It.IsAny<TextExtractionOptions>()), Times.Never);
        }

        [Test]
        public void ExtractText_WithUpperCaseExtension_RoutesCaseInsensitive()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            var expected = new TextExtractionResult("PDF text", 1, "pdf");
            _mockPdfExtractor
                .Setup(x => x.ExtractText(stream, null))
                .Returns(Result<TextExtractionResult>.Success(expected));

            // Act
            var result = _extractor.ExtractText(stream, "DOCUMENT.PDF");

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _mockPdfExtractor.Verify(x => x.ExtractText(stream, null), Times.Once);
        }

        [Test]
        public void ExtractText_WithUnsupportedFormat_ReturnsFailure()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = _extractor.ExtractText(stream, "file.xlsx");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("Unsupported file format"));
            Assert.That(result.Error, Does.Contain(".xlsx"));
        }

        [Test]
        public void ExtractText_WithNullStream_ReturnsFailure()
        {
            // Act
            var result = _extractor.ExtractText(null, "document.pdf");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("Stream cannot be null"));
        }

        [Test]
        public void ExtractText_WithNullFileName_ReturnsFailure()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = _extractor.ExtractText(stream, null);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("File name cannot be null or empty"));
        }

        [Test]
        public void ExtractText_WithEmptyFileName_ReturnsFailure()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });

            // Act
            var result = _extractor.ExtractText(stream, "  ");

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Does.Contain("File name cannot be null or empty"));
        }

        [Test]
        public void ExtractText_PassesOptionsToProvider()
        {
            // Arrange
            using var stream = new MemoryStream(new byte[] { 1 });
            var options = new TextExtractionOptions { MaxPages = 5, TrimWhitespace = false };
            var expected = new TextExtractionResult("text", 1, "pdf");
            _mockPdfExtractor
                .Setup(x => x.ExtractText(stream, options))
                .Returns(Result<TextExtractionResult>.Success(expected));

            // Act
            var result = _extractor.ExtractText(stream, "doc.pdf", options);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _mockPdfExtractor.Verify(x => x.ExtractText(stream, options), Times.Once);
        }

        [Test]
        public void IsFormatSupported_WithPdf_ReturnsTrue()
        {
            // Act & Assert
            Assert.That(_extractor.IsFormatSupported("file.pdf"), Is.True);
            Assert.That(_extractor.IsFormatSupported("file.PDF"), Is.True);
            Assert.That(_extractor.IsFormatSupported("path/to/file.pdf"), Is.True);
        }

        [Test]
        public void IsFormatSupported_WithDocx_ReturnsTrue()
        {
            // Act & Assert
            Assert.That(_extractor.IsFormatSupported("file.docx"), Is.True);
            Assert.That(_extractor.IsFormatSupported("file.DOCX"), Is.True);
        }

        [Test]
        public void IsFormatSupported_WithUnsupported_ReturnsFalse()
        {
            // Act & Assert
            Assert.That(_extractor.IsFormatSupported("file.txt"), Is.False);
            Assert.That(_extractor.IsFormatSupported("file.xlsx"), Is.False);
            Assert.That(_extractor.IsFormatSupported("file"), Is.False);
            Assert.That(_extractor.IsFormatSupported(""), Is.False);
        }

        [Test]
        public void SupportedExtensions_ContainsPdfAndDocx()
        {
            // Act
            var extensions = _extractor.SupportedExtensions;

            // Assert
            Assert.That(extensions, Does.Contain(".pdf"));
            Assert.That(extensions, Does.Contain(".docx"));
            Assert.That(extensions.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_WithNullPdfExtractor_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() =>
                new DocumentTextExtractor(null, _mockDocxExtractor.Object));
        }

        [Test]
        public void Constructor_WithNullDocxExtractor_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<System.ArgumentNullException>(() =>
                new DocumentTextExtractor(_mockPdfExtractor.Object, null));
        }
    }
}
