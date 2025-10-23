using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using System;
using System.IO;

namespace AMCode.Documents.UnitTests.Pdf
{
    [TestFixture]
    public class SimplePdfTest
    {
        [Test]
        public void CreateDocument_ShouldReturnSuccess()
        {
            // Arrange
            var logger = new TestPdfLogger();
            var validator = new PdfValidator();
            var provider = new QuestPdfProvider(logger, validator);

            // Act
            var result = provider.CreateDocument();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IPdfDocument>(result.Value);
        }

        [Test]
        public void CreateDocument_WithProperties_ShouldReturnSuccess()
        {
            // Arrange
            var logger = new TestPdfLogger();
            var validator = new PdfValidator();
            var provider = new QuestPdfProvider(logger, validator);
            var properties = new PdfProperties
            {
                Title = "Test Document",
                Author = "Test Author",
                Subject = "Test Subject"
            };

            // Act
            var result = provider.CreateDocument(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsInstanceOf<IPdfDocument>(result.Value);
        }

        [Test]
        public void AddPage_ShouldIncreasePageCount()
        {
            // Arrange
            var logger = new TestPdfLogger();
            var validator = new PdfValidator();
            var provider = new QuestPdfProvider(logger, validator);
            var documentResult = provider.CreateDocument();
            Assert.IsTrue(documentResult.IsSuccess);
            var document = documentResult.Value;

            // Act
            var page = document.Pages.Create();
            page.AddText("Hello World", 100, 100);

            // Assert
            Assert.AreEqual(1, document.Pages.Count);
            Assert.IsNotNull(page);
        }

        [Test]
        public void Document_ShouldHaveUniqueId()
        {
            // Arrange
            var logger = new TestPdfLogger();
            var validator = new PdfValidator();
            var provider = new QuestPdfProvider(logger, validator);

            // Act
            var result1 = provider.CreateDocument();
            var result2 = provider.CreateDocument();

            // Assert
            Assert.IsTrue(result1.IsSuccess);
            Assert.IsTrue(result2.IsSuccess);
            Assert.AreNotEqual(result1.Value.Id, result2.Value.Id);
        }

        [Test]
        public void Document_ShouldTrackCreationTime()
        {
            // Arrange
            var logger = new TestPdfLogger();
            var validator = new PdfValidator();
            var provider = new QuestPdfProvider(logger, validator);
            var beforeCreation = DateTime.UtcNow;

            // Act
            var result = provider.CreateDocument();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            Assert.IsTrue(document.CreatedAt >= beforeCreation);
            Assert.IsTrue(document.CreatedAt <= DateTime.UtcNow);
        }
    }
}
