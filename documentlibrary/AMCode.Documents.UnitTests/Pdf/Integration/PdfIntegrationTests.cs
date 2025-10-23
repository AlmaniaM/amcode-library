using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using System;
using System.IO;
using System.Linq;

namespace AMCode.Documents.UnitTests.Pdf.Integration
{
    [TestFixture]
    public class PdfIntegrationTests
    {
        private IPdfProvider _questPdfProvider;
        private IPdfProvider _iTextSharpProvider;
        private IPdfLogger _logger;
        private IPdfValidator _validator;

        [SetUp]
        public void Setup()
        {
            _logger = new TestPdfLogger();
            _validator = new PdfValidator();
            _questPdfProvider = new QuestPdfProvider(_logger, _validator);
            _iTextSharpProvider = new iTextSharpProvider(_logger, _validator);
        }

        [Test]
        public void QuestPdfProvider_CreateDocument_ShouldReturnSuccess()
        {
            // Act
            var result = _questPdfProvider.CreateDocument();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IPdfDocument>(result.Value);
            Assert.IsNotNull(result.Value.Id);
            Assert.IsTrue(result.Value.CreatedAt > DateTime.MinValue);
        }

        [Test]
        public void iTextSharpProvider_CreateDocument_ShouldReturnSuccess()
        {
            // Act
            var result = _iTextSharpProvider.CreateDocument();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IPdfDocument>(result.Value);
            Assert.IsNotNull(result.Value.Id);
            Assert.IsTrue(result.Value.CreatedAt > DateTime.MinValue);
        }

        [Test]
        public void QuestPdfProvider_CreateDocumentWithProperties_ShouldReturnSuccess()
        {
            // Arrange
            var properties = new PdfProperties
            {
                Title = "Test Document",
                Author = "Test Author",
                Subject = "Test Subject",
                Keywords = "test, pdf, integration"
            };

            // Act
            var result = _questPdfProvider.CreateDocument(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(properties.Title, result.Value.Properties.Title);
            Assert.AreEqual(properties.Author, result.Value.Properties.Author);
            Assert.AreEqual(properties.Subject, result.Value.Properties.Subject);
            Assert.AreEqual(properties.Keywords, result.Value.Properties.Keywords);
        }

        [Test]
        public void iTextSharpProvider_CreateDocumentWithProperties_ShouldReturnSuccess()
        {
            // Arrange
            var properties = new PdfProperties
            {
                Title = "Test Document",
                Author = "Test Author",
                Subject = "Test Subject",
                Keywords = "test, pdf, integration"
            };

            // Act
            var result = _iTextSharpProvider.CreateDocument(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(properties.Title, result.Value.Properties.Title);
            Assert.AreEqual(properties.Author, result.Value.Properties.Author);
            Assert.AreEqual(properties.Subject, result.Value.Properties.Subject);
            Assert.AreEqual(properties.Keywords, result.Value.Properties.Keywords);
        }

        [Test]
        public void PdfDocument_AddMultiplePages_ShouldWorkCorrectly()
        {
            // Arrange
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;

            // Act
            var page1 = document.Pages.Create();
            page1.AddText("Page 1 Content", 50, 50);

            var page2 = document.Pages.Create();
            page2.AddText("Page 2 Content", 50, 50);

            var page3 = document.Pages.Create();
            page3.AddText("Page 3 Content", 50, 50);

            // Assert
            Assert.AreEqual(3, document.Pages.Count);
            Assert.AreEqual(1, page1.PageNumber);
            Assert.AreEqual(2, page2.PageNumber);
            Assert.AreEqual(3, page3.PageNumber);
        }

        [Test]
        public void PdfDocument_AddContentToPages_ShouldWorkCorrectly()
        {
            // Arrange
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create();

            // Act
            page.AddText("Hello World", 100, 100);
            page.AddRectangle(50, 50, 200, 100, Color.LightBlue, Color.Black);
            page.AddLine(50, 50, 250, 150, Color.Red, 2.0);

            var table = page.AddTable(300, 200, 3, 2);
            table.SetCellValue(0, 0, "Header 1");
            table.SetCellValue(0, 1, "Header 2");
            table.SetCellValue(1, 0, "Data 1");
            table.SetCellValue(1, 1, "Data 2");

            // Assert
            Assert.IsNotNull(table);
            Assert.IsInstanceOf<ITable>(table);
        }

        [Test]
        public void PdfFactory_RegisterAndUseProviders_ShouldWorkCorrectly()
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
        public void PdfFactory_SetDefaultProvider_ShouldWorkCorrectly()
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
        public void PdfBuilder_FluentAPI_ShouldWorkCorrectly()
        {
            // Arrange
            PdfFactory.SetDefaultProvider(_questPdfProvider);

            // Act
            var builder = PdfFactory.CreateBuilder();
            var result = builder
                .WithTitle("Test Document")
                .WithAuthor("Test Author")
                .WithPage(page =>
                {
                    page.AddText("Hello from Builder!", 100, 100);
                    page.AddRectangle(50, 50, 200, 100, Color.LightBlue);
                })
                .Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("Test Document", result.Value.Properties.Title);
            Assert.AreEqual("Test Author", result.Value.Properties.Author);
            Assert.AreEqual(1, result.Value.Pages.Count);
        }

        [Test]
        public void PdfDocument_Dispose_ShouldCleanUpResources()
        {
            // Arrange
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;

            // Act
            document.Dispose();

            // Assert
            // Document should be disposed without throwing exceptions
            Assert.DoesNotThrow(() => document.Dispose());
        }

        [Test]
        public void PdfDocument_Close_ShouldWorkCorrectly()
        {
            // Arrange
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;

            // Act
            document.Close();

            // Assert
            // Document should be closed without throwing exceptions
            Assert.DoesNotThrow(() => document.Close());
        }

        [Test]
        public void PdfValidator_ValidateDocument_ShouldWorkCorrectly()
        {
            // Arrange
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create(document);
            page.AddText("Test content", 50, 50);

            // Act
            var validationResult = _validator.ValidateDocument(document);

            // Assert
            Assert.IsTrue(validationResult.IsSuccess);
        }

        [Test]
        public void PdfValidator_ValidatePage_ShouldWorkCorrectly()
        {
            // Arrange
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create(document);
            page.AddText("Test content", 50, 50);

            // Act
            var validationResult = _validator.ValidatePage(page);

            // Assert
            Assert.IsTrue(validationResult.IsSuccess);
        }

        [Test]
        public void PdfLogger_LogOperations_ShouldWorkCorrectly()
        {
            // Arrange
            var testLogger = new TestPdfLogger();

            // Act
            testLogger.LogDocumentOperation("Test Operation");
            testLogger.LogInformation("Test Information");
            testLogger.LogWarning("Test Warning");
            testLogger.LogDebug("Test Debug");
            testLogger.LogError("Test Error", new Exception("Test Exception"));

            // Assert
            Assert.IsTrue(testLogger.LogMessages.Count > 0);
            Assert.IsTrue(testLogger.Exceptions.Count > 0);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up any resources if needed
        }
    }
}