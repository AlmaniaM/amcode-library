using NUnit.Framework;
using AMCode.Documents.Pdf;
using System;
using System.IO;

namespace AMCode.Documents.UnitTests.Pdf
{
    [TestFixture]
    public class PdfProviderTests
    {
        private IPdfLogger _logger;
        private IPdfValidator _validator;

        [SetUp]
        public void Setup()
        {
            _logger = new TestPdfLogger();
            _validator = new PdfValidator();
        }

        [Test]
        public void QuestPdfProvider_ShouldHaveCorrectName()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);

            // Assert
            Assert.AreEqual("QuestPDF", provider.Name);
        }

        [Test]
        public void QuestPdfProvider_ShouldHaveCorrectVersion()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);

            // Assert
            Assert.AreEqual("2024.12.4", provider.Version);
        }

        [Test]
        public void iTextSharpProvider_ShouldHaveCorrectName()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);

            // Assert
            Assert.AreEqual("iTextSharp", provider.Name);
        }

        [Test]
        public void iTextSharpProvider_ShouldHaveCorrectVersion()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);

            // Assert
            Assert.AreEqual("5.5.13.3", provider.Version);
        }

        [Test]
        public void QuestPdfProvider_CreateDocument_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);

            // Act
            var result = provider.CreateDocument();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void iTextSharpProvider_CreateDocument_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);

            // Act
            var result = provider.CreateDocument();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void QuestPdfProvider_CreateDocumentWithProperties_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);
            var properties = new PdfProperties
            {
                Title = "Test Document",
                Author = "Test Author"
            };

            // Act
            var result = provider.CreateDocument(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(properties.Title, result.Value.Properties.Title);
        }

        [Test]
        public void iTextSharpProvider_CreateDocumentWithProperties_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);
            var properties = new PdfProperties
            {
                Title = "Test Document",
                Author = "Test Author"
            };

            // Act
            var result = provider.CreateDocument(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(properties.Title, result.Value.Properties.Title);
        }

        [Test]
        public void QuestPdfProvider_OpenDocumentFromStream_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);
            var document = provider.CreateDocument().Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);
            
            using var stream = new MemoryStream();
            document.SaveAs(stream);
            stream.Position = 0;

            // Act
            var result = provider.OpenDocument(stream);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void iTextSharpProvider_OpenDocumentFromStream_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);
            var document = provider.CreateDocument().Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);
            
            using var stream = new MemoryStream();
            document.SaveAs(stream);
            stream.Position = 0;

            // Act
            var result = provider.OpenDocument(stream);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public void QuestPdfProvider_OpenDocumentFromFile_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);
            var document = provider.CreateDocument().Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);
            
            var filePath = Path.GetTempFileName();
            try
            {
                document.SaveAs(filePath);

                // Act
                var result = provider.OpenDocument(filePath);

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
        public void iTextSharpProvider_OpenDocumentFromFile_ShouldReturnSuccess()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);
            var document = provider.CreateDocument().Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);
            
            var filePath = Path.GetTempFileName();
            try
            {
                document.SaveAs(filePath);

                // Act
                var result = provider.OpenDocument(filePath);

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
        public void QuestPdfProvider_OpenNonExistentFile_ShouldReturnFailure()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);
            var nonExistentFile = "non-existent-file.pdf";

            // Act
            var result = provider.OpenDocument(nonExistentFile);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);
        }

        [Test]
        public void iTextSharpProvider_OpenNonExistentFile_ShouldReturnFailure()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);
            var nonExistentFile = "non-existent-file.pdf";

            // Act
            var result = provider.OpenDocument(nonExistentFile);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);
        }

        [Test]
        public void QuestPdfProvider_OpenInvalidStream_ShouldReturnFailure()
        {
            // Arrange
            var provider = new QuestPdfProvider(_logger, _validator);
            using var invalidStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02 });

            // Act
            var result = provider.OpenDocument(invalidStream);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);
        }

        [Test]
        public void iTextSharpProvider_OpenInvalidStream_ShouldReturnFailure()
        {
            // Arrange
            var provider = new iTextSharpProvider(_logger, _validator);
            using var invalidStream = new MemoryStream(new byte[] { 0x00, 0x01, 0x02 });

            // Act
            var result = provider.OpenDocument(invalidStream);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);
        }
    }
}
