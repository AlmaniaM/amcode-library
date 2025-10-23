using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using System;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.UnitTests.Pdf
{
    [TestFixture]
    public class PdfPageTests
    {
        private IPdfProvider _provider;
        private IPdfLogger _logger;
        private IPdfValidator _validator;
        private IPdfDocument _document;

        [SetUp]
        public void Setup()
        {
            _logger = new TestPdfLogger();
            _validator = new PdfValidator();
            _provider = new QuestPdfProvider(_logger, _validator);
            
            var result = _provider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            _document = result.Value;
        }

        [Test]
        public void CreatePage_ShouldReturnValidPage()
        {
            // Act
            var page = _document.Pages.Create();

            // Assert
            Assert.IsNotNull(page);
            Assert.AreEqual(1, page.PageNumber);
            Assert.IsNotNull(page.Document);
            Assert.AreEqual(_document, page.Document);
        }

        [Test]
        public void Page_ShouldHaveCorrectPageNumber()
        {
            // Arrange & Act
            var page1 = _document.Pages.Create();
            var page2 = _document.Pages.Create();
            var page3 = _document.Pages.Create();

            // Assert
            Assert.AreEqual(1, page1.PageNumber);
            Assert.AreEqual(2, page2.PageNumber);
            Assert.AreEqual(3, page3.PageNumber);
        }

        [Test]
        public void AddText_ShouldNotThrow()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Act & Assert
            Assert.DoesNotThrow(() => page.AddText("Test Text", 100, 100));
        }

        [Test]
        public void AddText_WithFontStyle_ShouldNotThrow()
        {
            // Arrange
            var page = _document.Pages.Create();
            var fontStyle = new AMCode.Documents.Pdf.FontStyle
            {
                FontSize = 16,
                Bold = true,
                Color = AMCode.Documents.Common.Drawing.Color.Blue
            };

            // Act & Assert
            Assert.DoesNotThrow(() => page.AddText("Styled Text", 100, 100, fontStyle));
        }

        [Test]
        public void AddRectangle_ShouldNotThrow()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Act & Assert
            Assert.DoesNotThrow(() => page.AddRectangle(50, 50, 200, 100));
        }

        [Test]
        public void AddRectangle_WithColors_ShouldNotThrow()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Act & Assert
            Assert.DoesNotThrow(() => page.AddRectangle(50, 50, 200, 100, Color.LightBlue, Color.Black));
        }

        [Test]
        public void AddLine_ShouldNotThrow()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Act & Assert
            Assert.DoesNotThrow(() => page.AddLine(0, 0, 100, 100, Color.Black, 2.0));
        }

        [Test]
        public void AddImage_ShouldNotThrow()
        {
            // Arrange
            var page = _document.Pages.Create();
            var imageData = GenerateTestImageData();

            // Act & Assert
            Assert.DoesNotThrow(() => page.AddImage(imageData, 100, 100, 200, 150));
        }

        [Test]
        public void AddTable_ShouldReturnValidTable()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Act
            var table = page.AddTable(100, 100, 3, 4);

            // Assert
            Assert.IsNotNull(table);
            Assert.AreEqual(3, table.Rows);
            Assert.AreEqual(4, table.Columns);
        }

        [Test]
        public void Page_ShouldHaveDefaultSize()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Assert
            Assert.IsNotNull(page.Size);
        }

        [Test]
        public void Page_ShouldAllowSizeChange()
        {
            // Arrange
            var page = _document.Pages.Create();
            var newSize = AMCode.Documents.Pdf.PageSize.A4;

            // Act
            page.Size = newSize;

            // Assert
            Assert.AreEqual(newSize, page.Size);
        }

        [Test]
        public void Page_ShouldHaveDefaultMargins()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Assert
            Assert.IsNotNull(page.Margins);
        }

        [Test]
        public void Page_ShouldAllowMarginChange()
        {
            // Arrange
            var page = _document.Pages.Create();
            var newMargins = new AMCode.Documents.Pdf.Margins(20, 20, 20, 20);

            // Act
            page.Margins = newMargins;

            // Assert
            Assert.AreEqual(newMargins, page.Margins);
        }

        [Test]
        public void Page_ShouldHaveDefaultOrientation()
        {
            // Arrange
            var page = _document.Pages.Create();

            // Assert
            Assert.IsNotNull(page.Orientation);
        }

        [Test]
        public void Page_ShouldAllowOrientationChange()
        {
            // Arrange
            var page = _document.Pages.Create();
            var newOrientation = PageOrientation.Landscape;

            // Act
            page.Orientation = newOrientation;

            // Assert
            Assert.AreEqual(newOrientation, page.Orientation);
        }

        private byte[] GenerateTestImageData()
        {
            // Generate a simple 1x1 pixel PNG for testing
            return Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==");
        }
    }
}
