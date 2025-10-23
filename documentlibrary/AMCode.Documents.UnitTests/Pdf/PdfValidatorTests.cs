using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using System;
using System.IO;

namespace AMCode.Documents.UnitTests.Pdf
{
    [TestFixture]
    public class PdfValidatorTests
    {
        private IPdfValidator _validator;
        private IPdfProvider _provider;
        private IPdfLogger _logger;

        [SetUp]
        public void Setup()
        {
            _validator = new PdfValidator();
            _logger = new TestPdfLogger();
            _provider = new QuestPdfProvider(_logger, _validator);
        }

        [Test]
        public void ValidateDocument_WithValidDocument_ShouldReturnSuccess()
        {
            // Arrange
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create(document);
            page.AddText("Test Content", 100, 100);

            // Act
            var validationResult = _validator.ValidateDocument(document);

            // Assert
            Assert.IsTrue(validationResult.IsSuccess);
        }

        [Test]
        public void ValidateDocument_WithNullDocument_ShouldReturnFailure()
        {
            // Act
            var result = _validator.ValidateDocument(null);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Document cannot be null", result.Error);
        }

        [Test]
        public void ValidateDocument_WithNoPages_ShouldReturnFailure()
        {
            // Arrange
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;

            // Act
            var validationResult = _validator.ValidateDocument(document);

            // Assert
            Assert.IsFalse(validationResult.IsSuccess);
            Assert.AreEqual("Document must have at least one page", validationResult.Error);
        }

        [Test]
        public void ValidateDocument_WithNullPages_ShouldReturnFailure()
        {
            // Arrange
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            
            // Simulate null pages by creating a mock document
            var mockDocument = new MockPdfDocument { Pages = null };

            // Act
            var validationResult = _validator.ValidateDocument(mockDocument);

            // Assert
            Assert.IsFalse(validationResult.IsSuccess);
            Assert.AreEqual("Document must have a pages collection", validationResult.Error);
        }

        [Test]
        public void ValidateDocument_WithNullProperties_ShouldReturnFailure()
        {
            // Arrange
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);
            
            // Simulate null properties by creating a mock document
            var mockDocument = new MockPdfDocument 
            { 
                Pages = document.Pages, 
                Properties = null 
            };

            // Act
            var validationResult = _validator.ValidateDocument(mockDocument);

            // Assert
            Assert.IsFalse(validationResult.IsSuccess);
            Assert.AreEqual("Document properties cannot be null", validationResult.Error);
        }

        [Test]
        public void ValidatePage_WithValidPage_ShouldReturnSuccess()
        {
            // Arrange
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create();

            // Act
            var validationResult = _validator.ValidatePage(page);

            // Assert
            Assert.IsTrue(validationResult.IsSuccess);
        }

        [Test]
        public void ValidatePage_WithNullPage_ShouldReturnFailure()
        {
            // Act
            var result = _validator.ValidatePage(null);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Page cannot be null", result.Error);
        }

        [Test]
        public void ValidatePage_WithInvalidPageNumber_ShouldReturnFailure()
        {
            // Arrange
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create();
            
            // Simulate invalid page number
            var mockPage = new MockPdfPage { PageNumber = 0 };

            // Act
            var validationResult = _validator.ValidatePage(mockPage);

            // Assert
            Assert.IsFalse(validationResult.IsSuccess);
            Assert.AreEqual("Page number must be greater than 0", validationResult.Error);
        }

        [Test]
        public void ValidatePage_WithNullSize_ShouldReturnFailure()
        {
            // Arrange
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create();
            
            // Simulate null margins (which comes after size check)
            var mockPage = new MockPdfPage { PageNumber = 1, Size = AMCode.Documents.Pdf.PageSize.Custom, Margins = null };

            // Act
            var validationResult = _validator.ValidatePage(mockPage);

            // Assert
            Assert.IsFalse(validationResult.IsSuccess);
            Assert.AreEqual("Page margins cannot be null", validationResult.Error);
        }

        [Test]
        public void ValidateProperties_WithValidProperties_ShouldReturnSuccess()
        {
            // Arrange
            var properties = new PdfProperties
            {
                Title = "Test Document",
                Author = "Test Author",
                Subject = "Test Subject"
            };

            // Act
            var result = _validator.ValidateProperties(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void ValidateProperties_WithNullProperties_ShouldReturnFailure()
        {
            // Act
            var result = _validator.ValidateProperties(null);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Properties cannot be null", result.Error);
        }

        [Test]
        public void ValidateProperties_WithEmptyTitle_ShouldReturnFailure()
        {
            // Arrange
            var properties = new PdfProperties
            {
                Title = "",
                Author = "Test Author"
            };

            // Act
            var result = _validator.ValidateProperties(properties);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Document title cannot be empty", result.Error);
        }

        [Test]
        public void ValidateProperties_WithWhitespaceTitle_ShouldReturnFailure()
        {
            // Arrange
            var properties = new PdfProperties
            {
                Title = "   ",
                Author = "Test Author"
            };

            // Act
            var result = _validator.ValidateProperties(properties);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Document title cannot be empty", result.Error);
        }

        [Test]
        public void ValidateProperties_WithNullTitle_ShouldReturnFailure()
        {
            // Arrange
            var properties = new PdfProperties
            {
                Title = null,
                Author = "Test Author"
            };

            // Act
            var result = _validator.ValidateProperties(properties);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Document title cannot be empty", result.Error);
        }

        // Mock classes for testing
        private class MockPdfDocument : IPdfDocument
        {
            public Guid Id => Guid.NewGuid();
            public DateTime CreatedAt => DateTime.UtcNow;
            public DateTime LastModified { get; set; } = DateTime.UtcNow;
            public IPages Pages { get; set; }
            public IPdfProperties Properties { get; set; }

            public void SaveAs(Stream stream) { }
            public void SaveAs(string filePath) { }
            public void Close() { }
            public void Dispose() { }
        }

        private class MockPdfPage : IPage
        {
            public int PageNumber { get; set; }
            public AMCode.Documents.Pdf.PageSize Size { get; set; }
            public AMCode.Documents.Pdf.Margins Margins { get; set; }
            public PageOrientation Orientation { get; set; }
            public IPdfDocument Document => null;

            public void AddText(string text, double x, double y, AMCode.Documents.Pdf.FontStyle fontStyle = null) { }
            public void AddImage(byte[] imageData, double x, double y, double width, double height) { }
            public void AddRectangle(double x, double y, double width, double height, Color? fillColor = null, Color? strokeColor = null) { }
            public void AddLine(double x1, double y1, double x2, double y2, Color color, double thickness = 1.0) { }
            public ITable AddTable(double x, double y, int rows, int columns) => null;
        }
    }
}
