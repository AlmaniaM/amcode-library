using NUnit.Framework;
using AMCode.Documents.Pdf;
using System;
using System.IO;

namespace AMCode.Documents.UnitTests.Pdf
{
    [TestFixture]
    public class PdfFactoryTests
    {
        private IPdfLogger _logger;
        private IPdfValidator _validator;
        private IPdfProvider _questPdfProvider;
        private IPdfProvider _iTextSharpProvider;

        [SetUp]
        public void Setup()
        {
            _logger = new TestPdfLogger();
            _validator = new PdfValidator();
            _questPdfProvider = new QuestPdfProvider(_logger, _validator);
            _iTextSharpProvider = new iTextSharpProvider(_logger, _validator);
        }

        [TearDown]
        public void TearDown()
        {
            // Reset factory state
            PdfFactory.ClearAllProviders();
        }

        [Test]
        public void SetDefaultProvider_ShouldSetProvider()
        {
            // Act
            PdfFactory.SetDefaultProvider(_questPdfProvider);

            // Assert
            var result = PdfFactory.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void SetDefaultProvider_WithNull_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PdfFactory.SetDefaultProvider(null));
        }

        [Test]
        public void RegisterProvider_ShouldRegisterProvider()
        {
            // Act
            PdfFactory.RegisterProvider("QuestPDF", _questPdfProvider);

            // Assert
            var result = PdfFactory.CreateDocument("QuestPDF");
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void RegisterProvider_WithNullName_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => PdfFactory.RegisterProvider(null, _questPdfProvider));
        }

        [Test]
        public void RegisterProvider_WithEmptyName_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => PdfFactory.RegisterProvider("", _questPdfProvider));
        }

        [Test]
        public void RegisterProvider_WithNullProvider_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => PdfFactory.RegisterProvider("Test", null));
        }

        [Test]
        public void CreateDocument_WithoutDefaultProvider_ShouldReturnFailure()
        {
            // Act
            var result = PdfFactory.CreateDocument();

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No default PDF provider configured", result.Error);
        }

        [Test]
        public void CreateDocument_WithDefaultProvider_ShouldReturnSuccess()
        {
            // Arrange
            PdfFactory.SetDefaultProvider(_questPdfProvider);

            // Act
            var result = PdfFactory.CreateDocument();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void CreateDocument_WithProperties_ShouldReturnSuccess()
        {
            // Arrange
            PdfFactory.SetDefaultProvider(_questPdfProvider);
            var properties = new PdfProperties
            {
                Title = "Factory Test Document",
                Author = "Test Author"
            };

            // Act
            var result = PdfFactory.CreateDocument(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(properties.Title, result.Value.Properties.Title);
        }

        [Test]
        public void CreateDocument_WithSpecificProvider_ShouldReturnSuccess()
        {
            // Arrange
            PdfFactory.RegisterProvider("QuestPDF", _questPdfProvider);

            // Act
            var result = PdfFactory.CreateDocument("QuestPDF");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void CreateDocument_WithNonExistentProvider_ShouldReturnFailure()
        {
            // Act
            var result = PdfFactory.CreateDocument("NonExistentProvider");

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("PDF provider 'NonExistentProvider' not found", result.Error);
        }

        [Test]
        public void OpenDocument_WithoutDefaultProvider_ShouldReturnFailure()
        {
            // Arrange
            using var stream = new MemoryStream();

            // Act
            var result = PdfFactory.OpenDocument(stream);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No default PDF provider configured", result.Error);
        }

        [Test]
        public void OpenDocument_WithDefaultProvider_ShouldReturnSuccess()
        {
            // Arrange
            PdfFactory.SetDefaultProvider(_questPdfProvider);
            var document = _questPdfProvider.CreateDocument().Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);
            
            using var stream = new MemoryStream();
            document.SaveAs(stream);
            stream.Position = 0;

            // Act
            var result = PdfFactory.OpenDocument(stream);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void OpenDocument_FromFile_ShouldReturnSuccess()
        {
            // Arrange
            PdfFactory.SetDefaultProvider(_questPdfProvider);
            var document = _questPdfProvider.CreateDocument().Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);
            
            var filePath = Path.GetTempFileName();
            try
            {
                document.SaveAs(filePath);

                // Act
                var result = PdfFactory.OpenDocument(filePath);

                // Assert
                Assert.IsTrue(result.IsSuccess);
                Assert.IsNotNull(result.Value);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Test]
        public void CreateBuilder_WithoutDefaultProvider_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => PdfFactory.CreateBuilder());
        }

        [Test]
        public void CreateBuilder_WithDefaultProvider_ShouldReturnBuilder()
        {
            // Arrange
            PdfFactory.SetDefaultProvider(_questPdfProvider);

            // Act
            var builder = PdfFactory.CreateBuilder();

            // Assert
            Assert.IsNotNull(builder);
        }

        [Test]
        public void CreateBuilder_WithSpecificProvider_ShouldReturnBuilder()
        {
            // Arrange
            PdfFactory.RegisterProvider("QuestPDF", _questPdfProvider);

            // Act
            var builder = PdfFactory.CreateBuilder("QuestPDF");

            // Assert
            Assert.IsNotNull(builder);
        }

        [Test]
        public void CreateBuilder_WithNonExistentProvider_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => PdfFactory.CreateBuilder("NonExistentProvider"));
        }

        [Test]
        public void MultipleProviders_ShouldWorkIndependently()
        {
            // Arrange
            PdfFactory.RegisterProvider("QuestPDF", _questPdfProvider);
            PdfFactory.RegisterProvider("iTextSharp", _iTextSharpProvider);

            // Act
            var questResult = PdfFactory.CreateDocument("QuestPDF");
            var iTextResult = PdfFactory.CreateDocument("iTextSharp");

            // Assert
            Assert.IsTrue(questResult.IsSuccess);
            Assert.IsTrue(iTextResult.IsSuccess);
            Assert.IsNotNull(questResult.Value);
            Assert.IsNotNull(iTextResult.Value);
        }

        [Test]
        public void ProviderRegistration_ShouldOverwriteExisting()
        {
            // Arrange
            PdfFactory.RegisterProvider("Test", _questPdfProvider);
            PdfFactory.RegisterProvider("Test", _iTextSharpProvider);

            // Act
            var result = PdfFactory.CreateDocument("Test");

            // Assert
            Assert.IsTrue(result.IsSuccess);
            // The provider should be the last one registered (iTextSharp)
        }

    }
}
