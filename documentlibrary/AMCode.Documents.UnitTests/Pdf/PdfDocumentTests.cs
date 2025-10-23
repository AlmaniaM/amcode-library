using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using System;
using System.IO;
using System.Threading;

namespace AMCode.Documents.UnitTests.Pdf
{
    [TestFixture]
    public class PdfDocumentTests
    {
        private IPdfProvider _provider;
        private IPdfLogger _logger;
        private IPdfValidator _validator;

        [SetUp]
        public void Setup()
        {
            _logger = new TestPdfLogger();
            _validator = new PdfValidator();
            _provider = new QuestPdfProvider(_logger, _validator);
        }

        [Test]
        public void CreateDocument_ShouldReturnSuccess()
        {
            // Act
            var result = _provider.CreateDocument();

            // Debug: Print error message if failed
            if (!result.IsSuccess)
            {
                Console.WriteLine($"CreateDocument failed: {result.Error}");
            }

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.IsNotNull(result.Value.Id);
            Assert.IsTrue(result.Value.CreatedAt > DateTime.MinValue);
        }

        [Test]
        public void CreateDocument_WithProperties_ShouldReturnSuccess()
        {
            // Arrange
            var properties = new PdfProperties
            {
                Title = "Test Document",
                Author = "Test Author",
                Subject = "Test Subject"
            };

            // Act
            var result = _provider.CreateDocument(properties);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(properties.Title, result.Value.Properties.Title);
            Assert.AreEqual(properties.Author, result.Value.Properties.Author);
        }

        [Test]
        public void AddPage_ShouldIncreasePageCount()
        {
            // Arrange
            var documentResult = _provider.CreateDocument();
            var document = documentResult.Value;

            // Act
            var page = document.Pages.Create();
            page.AddText("Hello World", 100, 100);

            // Assert
            Assert.AreEqual(1, document.Pages.Count);
        }

        [Test]
        public void SaveAs_Stream_ShouldNotThrow()
        {
            // Arrange
            var documentResult = _provider.CreateDocument();
            var document = documentResult.Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);

            using var stream = new MemoryStream();

            // Act & Assert
            Assert.DoesNotThrow(() => document.SaveAs(stream));
            Assert.IsTrue(stream.Length > 0);
        }

        [Test]
        public void SaveAs_File_ShouldNotThrow()
        {
            // Arrange
            var documentResult = _provider.CreateDocument();
            var document = documentResult.Value;
            var page = document.Pages.Create();
            page.AddText("Test Content", 100, 100);

            var filePath = Path.GetTempFileName();

            try
            {
                // Act & Assert
                Assert.DoesNotThrow(() => document.SaveAs(filePath));
                Assert.IsTrue(File.Exists(filePath));
                Assert.IsTrue(new FileInfo(filePath).Length > 0);
            }
            finally
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        [Test]
        public void Document_ShouldHaveUniqueId()
        {
            // Arrange & Act
            var result1 = _provider.CreateDocument();
            var result2 = _provider.CreateDocument();

            // Assert
            Assert.IsTrue(result1.IsSuccess);
            Assert.IsTrue(result2.IsSuccess);
            Assert.AreNotEqual(result1.Value.Id, result2.Value.Id);
        }

        [Test]
        public void Document_ShouldTrackCreationTime()
        {
            // Arrange
            var beforeCreation = DateTime.UtcNow;

            // Act
            var result = _provider.CreateDocument();
            var afterCreation = DateTime.UtcNow;

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Value.CreatedAt >= beforeCreation);
            Assert.IsTrue(result.Value.CreatedAt <= afterCreation);
        }

        [Test]
        public void Document_ShouldUpdateLastModified()
        {
            // Arrange
            var result = _provider.CreateDocument();
            var document = result.Value;
            var originalModified = document.LastModified;

            // Act
            Thread.Sleep(10); // Ensure time difference
            document.LastModified = DateTime.UtcNow;

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(document.LastModified > originalModified);
        }

        [Test]
        public void Document_ShouldDisposeProperly()
        {
            // Arrange
            var result = _provider.CreateDocument();
            var document = result.Value;

            // Act & Assert
            Assert.DoesNotThrow(() => document.Dispose());
        }

        [Test]
        public void Document_ShouldCloseProperly()
        {
            // Arrange
            var result = _provider.CreateDocument();
            var document = result.Value;

            // Act & Assert
            Assert.DoesNotThrow(() => document.Close());
        }
    }
}
