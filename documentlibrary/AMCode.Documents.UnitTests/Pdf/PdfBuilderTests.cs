using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using System;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.UnitTests.Pdf
{
    [TestFixture]
    public class PdfBuilderTests
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
        public void CreateBuilder_ShouldReturnValidBuilder()
        {
            // Act
            var builder = new PdfBuilder(_provider);

            // Assert
            Assert.IsNotNull(builder);
        }

        [Test]
        public void WithTitle_ShouldSetTitle()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);
            var title = "Test Document Title";

            // Act
            var result = builder.WithTitle(title).Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(title, result.Value.Properties.Title);
        }

        [Test]
        public void WithAuthor_ShouldSetAuthor()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);
            var author = "Test Author";

            // Act
            var result = builder.WithAuthor(author).Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(author, result.Value.Properties.Author);
        }

        [Test]
        public void WithSubject_ShouldSetSubject()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);
            var subject = "Test Subject";

            // Act
            var result = builder.WithSubject(subject).Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(subject, result.Value.Properties.Subject);
        }

        [Test]
        public void WithKeywords_ShouldSetKeywords()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);
            var keywords = "test, pdf, builder";

            // Act
            var result = builder.WithKeywords(keywords).Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(keywords, result.Value.Properties.Keywords);
        }

        [Test]
        public void WithPage_ShouldAddPage()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);

            // Act
            var result = builder
                .WithPage(page => page.AddText("Test Content", 100, 100))
                .Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Value.Pages.Count);
        }

        [Test]
        public void WithPage_WithPageSize_ShouldAddPageWithCorrectSize()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);
            var pageSize = AMCode.Documents.Pdf.PageSize.A4;

            // Act
            var result = builder
                .WithPage(pageSize, page => page.AddText("Test Content", 100, 100))
                .Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Value.Pages.Count);
            Assert.AreEqual(pageSize, result.Value.Pages[0].Size);
        }

        [Test]
        public void MultipleWithPage_ShouldAddMultiplePages()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);

            // Act
            var result = builder
                .WithPage(page => page.AddText("Page 1", 100, 100))
                .WithPage(page => page.AddText("Page 2", 100, 100))
                .WithPage(page => page.AddText("Page 3", 100, 100))
                .Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(3, result.Value.Pages.Count);
        }

        [Test]
        public void FluentChaining_ShouldWork()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);

            // Act
            var result = builder
                .WithTitle("Fluent Test Document")
                .WithAuthor("Test Author")
                .WithSubject("Fluent API Testing")
                .WithKeywords("fluent, test, pdf")
                .WithPage(page => page.AddText("Fluent Content", 100, 100))
                .Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Fluent Test Document", result.Value.Properties.Title);
            Assert.AreEqual("Test Author", result.Value.Properties.Author);
            Assert.AreEqual("Fluent API Testing", result.Value.Properties.Subject);
            Assert.AreEqual("fluent, test, pdf", result.Value.Properties.Keywords);
            Assert.AreEqual(1, result.Value.Pages.Count);
        }

        [Test]
        public void Build_WithoutPages_ShouldReturnSuccess()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);

            // Act
            var result = builder.Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(0, result.Value.Pages.Count);
        }

        [Test]
        public void Build_WithComplexPage_ShouldReturnSuccess()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);

            // Act
            var result = builder
                .WithTitle("Complex Document")
                .WithPage(page =>
                {
                    page.AddText("Title", 100, 100, new AMCode.Documents.Pdf.FontStyle { FontSize = 24, Bold = true });
                    page.AddText("Subtitle", 100, 130, new AMCode.Documents.Pdf.FontStyle { FontSize = 16 });
                    page.AddRectangle(50, 50, 200, 100, Color.LightGray, Color.Black);
                    page.AddLine(50, 150, 250, 150, Color.Blue, 2.0);
                    var table = page.AddTable(100, 200, 2, 3);
                    table.SetCellValue(0, 0, "Header 1");
                    table.SetCellValue(0, 1, "Header 2");
                    table.SetCellValue(1, 0, "Data 1");
                    table.SetCellValue(1, 1, "Data 2");
                })
                .Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Value.Pages.Count);
        }

        [Test]
        public void Build_WithMultipleComplexPages_ShouldReturnSuccess()
        {
            // Arrange
            var builder = new PdfBuilder(_provider);

            // Act
            var result = builder
                .WithTitle("Multi-Page Document")
                .WithPage(AMCode.Documents.Pdf.PageSize.A4, page =>
                {
                    page.AddText("Page 1 - A4", 100, 100);
                    page.AddRectangle(50, 50, 200, 100);
                })
                .WithPage(AMCode.Documents.Pdf.PageSize.Letter, page =>
                {
                    page.AddText("Page 2 - Letter", 100, 100);
                    page.AddLine(0, 0, 100, 100, Color.Red);
                })
                .WithPage(AMCode.Documents.Pdf.PageSize.Legal, page =>
                {
                    page.AddText("Page 3 - Legal", 100, 100);
                    var table = page.AddTable(100, 200, 2, 2);
                    table.SetCellValue(0, 0, "A");
                    table.SetCellValue(0, 1, "B");
                    table.SetCellValue(1, 0, "C");
                    table.SetCellValue(1, 1, "D");
                })
                .Build();

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(3, result.Value.Pages.Count);
            Assert.AreEqual(AMCode.Documents.Pdf.PageSize.A4, result.Value.Pages[0].Size);
            Assert.AreEqual(AMCode.Documents.Pdf.PageSize.Letter, result.Value.Pages[1].Size);
            Assert.AreEqual(AMCode.Documents.Pdf.PageSize.Legal, result.Value.Pages[2].Size);
        }

        [Test]
        public void Build_WithNullProvider_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new PdfBuilder(null));
        }
    }
}
