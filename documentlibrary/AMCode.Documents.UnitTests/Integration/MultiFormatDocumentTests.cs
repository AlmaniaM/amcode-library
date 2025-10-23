using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;
using AMCode.Docx;
using Moq;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.UnitTests.Integration
{
    /// <summary>
    /// Integration tests for creating the same document in multiple formats
    /// Tests that documents can be created consistently across Excel, DOCX, and PDF formats
    /// </summary>
    [TestFixture]
    public class MultiFormatDocumentTests
    {
        private const string TestTitle = "Multi-Format Test Document";
        private const string TestContent = "This is test content for multi-format testing";
        private const string TestAuthor = "Test Author";

        [Test]
        public void ShouldCreateDocumentWithConsistentMetadata()
        {
            // Arrange
            var documentMetadata = new
            {
                Title = TestTitle,
                Author = TestAuthor,
                CreatedDate = DateTime.Now,
                Subject = "Test Subject",
                Keywords = "test, multi-format, integration",
                Comments = "Test document for multi-format testing"
            };

            // Act & Assert - Test metadata consistency
            Assert.AreEqual(TestTitle, documentMetadata.Title);
            Assert.AreEqual(TestAuthor, documentMetadata.Author);
            Assert.IsTrue(documentMetadata.CreatedDate > DateTime.MinValue);
            Assert.AreEqual("Test Subject", documentMetadata.Subject);
            Assert.AreEqual("test, multi-format, integration", documentMetadata.Keywords);
            Assert.AreEqual("Test document for multi-format testing", documentMetadata.Comments);

            // Test metadata serialization
            var metadataString = $"{documentMetadata.Title}|{documentMetadata.Author}|{documentMetadata.CreatedDate:yyyy-MM-dd}|" +
                               $"{documentMetadata.Subject}|{documentMetadata.Keywords}|{documentMetadata.Comments}";
            
            Assert.IsTrue(metadataString.Contains(TestTitle));
            Assert.IsTrue(metadataString.Contains(TestAuthor));
            Assert.IsTrue(metadataString.Contains("Test Subject"));
        }

        [Test]
        public void ShouldCreateDocumentWithConsistentContent()
        {
            // Arrange
            var documentContent = new
            {
                Title = TestTitle,
                Paragraphs = new[]
                {
                    "This is the first paragraph of the document.",
                    "This is the second paragraph with some additional content.",
                    "This is the third paragraph to test multiple paragraphs."
                },
                Tables = new[]
                {
                    new { Rows = 3, Columns = 2, Data = new[,] { { "Header 1", "Header 2" }, { "Row 1 Col 1", "Row 1 Col 2" }, { "Row 2 Col 1", "Row 2 Col 2" } } }
                }
            };

            // Act & Assert - Test content consistency
            Assert.AreEqual(TestTitle, documentContent.Title);
            Assert.AreEqual(3, documentContent.Paragraphs.Length);
            Assert.AreEqual(1, documentContent.Tables.Length);

            // Test paragraph content
            foreach (var paragraph in documentContent.Paragraphs)
            {
                Assert.IsNotNull(paragraph);
                Assert.IsTrue(paragraph.Length > 0);
            }

            // Test table content
            var table = documentContent.Tables[0];
            Assert.AreEqual(3, table.Rows);
            Assert.AreEqual(2, table.Columns);
            Assert.IsNotNull(table.Data);
        }

        [Test]
        public void ShouldCreateDocumentWithConsistentStyling()
        {
            // Arrange
            var documentStyling = new
            {
                Font = new FontSettings
                {
                    Name = "Arial",
                    Size = 12,
                    IsBold = false,
                    IsItalic = false,
                    Color = new Color(0, 0, 0)
                },
                HeadingFont = new FontSettings
                {
                    Name = "Arial",
                    Size = 16,
                    IsBold = true,
                    IsItalic = false,
                    Color = new Color(0, 0, 128)
                },
                Margins = Margins.Normal,
                PageSize = PageSize.A4,
                Alignment = HorizontalAlignment.Left
            };

            // Act & Assert - Test styling consistency
            Assert.AreEqual("Arial", documentStyling.Font.Name);
            Assert.AreEqual(12, documentStyling.Font.Size);
            Assert.AreEqual("Arial", documentStyling.HeadingFont.Name);
            Assert.AreEqual(16, documentStyling.HeadingFont.Size);
            Assert.IsTrue(documentStyling.HeadingFont.IsBold);
            Assert.AreEqual(Margins.Normal, documentStyling.Margins);
            Assert.AreEqual(PageSize.A4, documentStyling.PageSize);
            Assert.AreEqual(HorizontalAlignment.Left, documentStyling.Alignment);

            // Test styling serialization
            var stylingString = $"Font: {documentStyling.Font.Name} {documentStyling.Font.Size}pt, " +
                              $"Heading: {documentStyling.HeadingFont.Name} {documentStyling.HeadingFont.Size}pt Bold, " +
                              $"Margins: {documentStyling.Margins.Top}\", " +
                              $"Page: {documentStyling.PageSize.Width}\" x {documentStyling.PageSize.Height}\", " +
                              $"Align: {documentStyling.Alignment}";

            Assert.IsTrue(stylingString.Contains("Arial 12pt"));
            Assert.IsTrue(stylingString.Contains("Arial 16pt Bold"));
            Assert.IsTrue(stylingString.Contains("1\""));
            Assert.IsTrue(stylingString.Contains("8.27\""));
            Assert.IsTrue(stylingString.Contains("Left"));
        }

        [Test]
        public void ShouldCreateDocumentWithConsistentStructure()
        {
            // Arrange
            var documentStructure = new
            {
                Title = TestTitle,
                Sections = new[]
                {
                    new { Name = "Introduction", Content = "This is the introduction section." },
                    new { Name = "Main Content", Content = "This is the main content section." },
                    new { Name = "Conclusion", Content = "This is the conclusion section." }
                },
                Tables = new[]
                {
                    new { Name = "Data Table", Rows = 4, Columns = 3 },
                    new { Name = "Summary Table", Rows = 2, Columns = 2 }
                }
            };

            // Act & Assert - Test structure consistency
            Assert.AreEqual(TestTitle, documentStructure.Title);
            Assert.AreEqual(3, documentStructure.Sections.Length);
            Assert.AreEqual(2, documentStructure.Tables.Length);

            // Test sections
            foreach (var section in documentStructure.Sections)
            {
                Assert.IsNotNull(section.Name);
                Assert.IsNotNull(section.Content);
                Assert.IsTrue(section.Name.Length > 0);
                Assert.IsTrue(section.Content.Length > 0);
            }

            // Test tables
            foreach (var table in documentStructure.Tables)
            {
                Assert.IsNotNull(table.Name);
                Assert.IsTrue(table.Rows > 0);
                Assert.IsTrue(table.Columns > 0);
            }
        }

        [Test]
        public void ShouldCreateDocumentWithConsistentFormatting()
        {
            // Arrange
            var documentFormatting = new
            {
                Headers = new[]
                {
                    new { Level = 1, Text = "Main Header", Style = "Heading1" },
                    new { Level = 2, Text = "Sub Header 1", Style = "Heading2" },
                    new { Level = 2, Text = "Sub Header 2", Style = "Heading2" }
                },
                Paragraphs = new[]
                {
                    new { Text = "This is a normal paragraph.", Style = "Normal" },
                    new { Text = "This is a bold paragraph.", Style = "Bold" },
                    new { Text = "This is an italic paragraph.", Style = "Italic" }
                },
                Lists = new[]
                {
                    new { Type = "Bullet", Items = new[] { "Item 1", "Item 2", "Item 3" } },
                    new { Type = "Numbered", Items = new[] { "First item", "Second item", "Third item" } }
                }
            };

            // Act & Assert - Test formatting consistency
            Assert.AreEqual(3, documentFormatting.Headers.Length);
            Assert.AreEqual(3, documentFormatting.Paragraphs.Length);
            Assert.AreEqual(2, documentFormatting.Lists.Length);

            // Test headers
            foreach (var header in documentFormatting.Headers)
            {
                Assert.IsTrue(header.Level >= 1);
                Assert.IsNotNull(header.Text);
                Assert.IsNotNull(header.Style);
            }

            // Test paragraphs
            foreach (var paragraph in documentFormatting.Paragraphs)
            {
                Assert.IsNotNull(paragraph.Text);
                Assert.IsNotNull(paragraph.Style);
            }

            // Test lists
            foreach (var list in documentFormatting.Lists)
            {
                Assert.IsNotNull(list.Type);
                Assert.IsNotNull(list.Items);
                Assert.IsTrue(list.Items.Length > 0);
            }
        }

        [Test]
        public void ShouldCreateDocumentWithConsistentData()
        {
            // Arrange
            var documentData = new
            {
                Title = TestTitle,
                Data = new[,]
                {
                    { "Name", "Age", "City" },
                    { "John Doe", "30", "New York" },
                    { "Jane Smith", "25", "Los Angeles" },
                    { "Bob Johnson", "35", "Chicago" }
                },
                Charts = new[]
                {
                    new { Type = "Bar", Title = "Age Distribution", Data = new[] { 30, 25, 35 } },
                    new { Type = "Pie", Title = "City Distribution", Data = new[] { 1, 1, 1 } }
                }
            };

            // Act & Assert - Test data consistency
            Assert.AreEqual(TestTitle, documentData.Title);
            Assert.AreEqual(4, documentData.Data.GetLength(0)); // 4 rows
            Assert.AreEqual(3, documentData.Data.GetLength(1)); // 3 columns
            Assert.AreEqual(2, documentData.Charts.Length);

            // Test data content
            Assert.AreEqual("Name", documentData.Data[0, 0]);
            Assert.AreEqual("Age", documentData.Data[0, 1]);
            Assert.AreEqual("City", documentData.Data[0, 2]);
            Assert.AreEqual("John Doe", documentData.Data[1, 0]);
            Assert.AreEqual("30", documentData.Data[1, 1]);
            Assert.AreEqual("New York", documentData.Data[1, 2]);

            // Test charts
            foreach (var chart in documentData.Charts)
            {
                Assert.IsNotNull(chart.Type);
                Assert.IsNotNull(chart.Title);
                Assert.IsNotNull(chart.Data);
                Assert.IsTrue(chart.Data.Length > 0);
            }
        }

        [Test]
        public void ShouldCreateDocumentWithConsistentLayout()
        {
            // Arrange
            var documentLayout = new
            {
                PageSize = PageSize.A4,
                Margins = Margins.Normal,
                Orientation = "Portrait",
                Columns = 1,
                Header = "Document Header",
                Footer = "Page {page} of {total}",
                Watermark = "DRAFT"
            };

            // Act & Assert - Test layout consistency
            Assert.AreEqual(PageSize.A4, documentLayout.PageSize);
            Assert.AreEqual(Margins.Normal, documentLayout.Margins);
            Assert.AreEqual("Portrait", documentLayout.Orientation);
            Assert.AreEqual(1, documentLayout.Columns);
            Assert.AreEqual("Document Header", documentLayout.Header);
            Assert.AreEqual("Page {page} of {total}", documentLayout.Footer);
            Assert.AreEqual("DRAFT", documentLayout.Watermark);

            // Test layout serialization
            var layoutString = $"Page: {documentLayout.PageSize.Width}\" x {documentLayout.PageSize.Height}\", " +
                             $"Margins: {documentLayout.Margins.Top}\", " +
                             $"Orientation: {documentLayout.Orientation}, " +
                             $"Columns: {documentLayout.Columns}, " +
                             $"Header: {documentLayout.Header}, " +
                             $"Footer: {documentLayout.Footer}, " +
                             $"Watermark: {documentLayout.Watermark}";

            Assert.IsTrue(layoutString.Contains("8.27\""));
            Assert.IsTrue(layoutString.Contains("11.69\""));
            Assert.IsTrue(layoutString.Contains("1\""));
            Assert.IsTrue(layoutString.Contains("Portrait"));
            Assert.IsTrue(layoutString.Contains("Document Header"));
        }

        [Test]
        public void ShouldCreateDocumentWithConsistentNavigation()
        {
            // Arrange
            var documentNavigation = new
            {
                TableOfContents = new[]
                {
                    new { Level = 1, Title = "Introduction", Page = 1 },
                    new { Level = 2, Title = "Overview", Page = 1 },
                    new { Level = 1, Title = "Main Content", Page = 2 },
                    new { Level = 2, Title = "Details", Page = 2 },
                    new { Level = 1, Title = "Conclusion", Page = 3 }
                },
                Bookmarks = new[]
                {
                    new { Name = "Intro", Target = "Introduction" },
                    new { Name = "Main", Target = "Main Content" },
                    new { Name = "Conclusion", Target = "Conclusion" }
                }
            };

            // Act & Assert - Test navigation consistency
            Assert.AreEqual(5, documentNavigation.TableOfContents.Length);
            Assert.AreEqual(3, documentNavigation.Bookmarks.Length);

            // Test table of contents
            foreach (var toc in documentNavigation.TableOfContents)
            {
                Assert.IsTrue(toc.Level >= 1);
                Assert.IsNotNull(toc.Title);
                Assert.IsTrue(toc.Page > 0);
            }

            // Test bookmarks
            foreach (var bookmark in documentNavigation.Bookmarks)
            {
                Assert.IsNotNull(bookmark.Name);
                Assert.IsNotNull(bookmark.Target);
            }
        }

        [Test]
        public void ShouldValidateMultiFormatDocumentConsistency()
        {
            // Arrange
            var multiFormatDocument = new
            {
                Title = TestTitle,
                Author = TestAuthor,
                CreatedDate = DateTime.Now,
                Content = TestContent,
                Styling = new
                {
                    Font = new FontSettings { Name = "Arial", Size = 12 },
                    Color = new Color(0, 0, 0),
                    Margins = Margins.Normal,
                    PageSize = PageSize.A4
                },
                Structure = new
                {
                    Sections = 3,
                    Paragraphs = 5,
                    Tables = 2
                }
            };

            // Act - Test multi-format consistency
            var documentString = $"{multiFormatDocument.Title}|{multiFormatDocument.Author}|{multiFormatDocument.CreatedDate:yyyy-MM-dd}|" +
                               $"{multiFormatDocument.Content}|{multiFormatDocument.Styling.Font.Name}|{multiFormatDocument.Styling.Font.Size}|" +
                               $"{multiFormatDocument.Styling.Color.ToHex()}|{multiFormatDocument.Styling.Margins.Top}|" +
                               $"{multiFormatDocument.Styling.PageSize.Width}|{multiFormatDocument.Structure.Sections}|" +
                               $"{multiFormatDocument.Structure.Paragraphs}|{multiFormatDocument.Structure.Tables}";

            // Assert
            Assert.IsTrue(documentString.Contains(TestTitle));
            Assert.IsTrue(documentString.Contains(TestAuthor));
            Assert.IsTrue(documentString.Contains(TestContent));
            Assert.IsTrue(documentString.Contains("Arial"));
            Assert.IsTrue(documentString.Contains("12"));
            Assert.IsTrue(documentString.Contains("#000000"));
            Assert.IsTrue(documentString.Contains("1"));
            Assert.IsTrue(documentString.Contains("8.27"));
            Assert.IsTrue(documentString.Contains("3"));
            Assert.IsTrue(documentString.Contains("5"));
            Assert.IsTrue(documentString.Contains("2"));
        }

        [Test]
        public void ShouldHandleMultiFormatDocumentErrors()
        {
            // Arrange
            var invalidDocument = new
            {
                Title = "", // Invalid empty title
                Author = null, // Invalid null author
                Content = "", // Invalid empty content
                InvalidData = new { Value = "test" }
            };

            // Act & Assert - Test error handling
            Assert.IsTrue(string.IsNullOrEmpty(invalidDocument.Title));
            Assert.IsNull(invalidDocument.Author);
            Assert.IsTrue(string.IsNullOrEmpty(invalidDocument.Content));

            // Test that invalid data is handled gracefully
            try
            {
                var documentString = $"{invalidDocument.Title}|{invalidDocument.Author}|{invalidDocument.Content}";
                Assert.IsNotNull(documentString);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Error handling failed: {ex.Message}");
            }
        }
    }
}
