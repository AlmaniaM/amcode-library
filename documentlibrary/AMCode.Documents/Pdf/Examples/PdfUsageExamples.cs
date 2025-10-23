using System;
using System.IO;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf.Examples
{
    /// <summary>
    /// Comprehensive usage examples for the AMCode PDF library
    /// </summary>
    public static class PdfUsageExamples
    {
        /// <summary>
        /// Example 1: Basic PDF document creation
        /// </summary>
        public static void BasicDocumentCreation()
        {
            Console.WriteLine("=== Basic PDF Document Creation ===");

            // Initialize the PDF factory with default providers
            PdfFactory.InitializeDefaultProviders();

            // Create a simple PDF document
            var result = PdfFactory.CreateDocument();
            if (result.IsSuccess)
            {
                var document = result.Value;
                Console.WriteLine($"Created PDF document with ID: {document.Id}");

                // Add a page with content
                var page = document.Pages.Create();
                page.AddText("Hello, World!", 100, 100);

                // Save the document
                document.SaveAs("BasicDocument.pdf");
                Console.WriteLine("Document saved as BasicDocument.pdf");

                // Clean up
                document.Dispose();
            }
            else
            {
                Console.WriteLine($"Failed to create document: {result.Error}");
            }
        }

        /// <summary>
        /// Example 2: Document with custom properties
        /// </summary>
        public static void DocumentWithProperties()
        {
            Console.WriteLine("=== Document with Custom Properties ===");

            // Create document properties
            var properties = new PdfProperties
            {
                Title = "My Custom Document",
                Author = "John Doe",
                Subject = "PDF Library Example",
                Keywords = "pdf, example, amcode",
                Creator = "AMCode PDF Library",
                Producer = "AMCode PDF Library v1.0"
            };

            // Create document with properties
            var result = PdfFactory.CreateDocument(properties);
            if (result.IsSuccess)
            {
                var document = result.Value;
                Console.WriteLine($"Created document: {document.Properties.Title}");

                // Add content
                var page = document.Pages.Create();
                page.AddText($"Title: {document.Properties.Title}", 50, 50);
                page.AddText($"Author: {document.Properties.Author}", 50, 70);
                page.AddText($"Subject: {document.Properties.Subject}", 50, 90);

                document.SaveAs("DocumentWithProperties.pdf");
                Console.WriteLine("Document saved as DocumentWithProperties.pdf");

                document.Dispose();
            }
        }

        /// <summary>
        /// Example 3: Multi-page document with various content
        /// </summary>
        public static void MultiPageDocument()
        {
            Console.WriteLine("=== Multi-Page Document ===");

            var result = PdfFactory.CreateDocument();
            if (result.IsSuccess)
            {
                var document = result.Value;

                // Page 1: Title page
                var titlePage = document.Pages.Create();
                titlePage.AddText("AMCode PDF Library", 200, 300, new FontStyle { FontSize = 24, Bold = true });
                titlePage.AddText("Usage Examples", 250, 350, new FontStyle { FontSize = 18 });
                titlePage.AddRectangle(50, 50, 500, 700, Color.LightGray, Color.Black);

                // Page 2: Content with shapes
                var contentPage = document.Pages.Create();
                contentPage.AddText("Shapes and Graphics", 50, 50, new FontStyle { FontSize = 16, Bold = true });
                
                // Add rectangles
                contentPage.AddRectangle(50, 100, 100, 50, Color.Red, Color.Black);
                contentPage.AddRectangle(200, 100, 100, 50, Color.Green, Color.Black);
                contentPage.AddRectangle(350, 100, 100, 50, Color.Blue, Color.Black);

                // Add lines
                contentPage.AddLine(50, 200, 450, 200, Color.Black, 2.0);
                contentPage.AddLine(50, 250, 450, 250, Color.Red, 1.0);
                contentPage.AddLine(50, 300, 450, 300, Color.Blue, 3.0);

                // Page 3: Table
                var tablePage = document.Pages.Create();
                tablePage.AddText("Data Table", 50, 50, new FontStyle { FontSize = 16, Bold = true });
                
                var table = tablePage.AddTable(50, 100, 4, 3);
                table.SetCellValue(0, 0, "Name");
                table.SetCellValue(0, 1, "Age");
                table.SetCellValue(0, 2, "City");
                table.SetCellValue(1, 0, "John Doe");
                table.SetCellValue(1, 1, "30");
                table.SetCellValue(1, 2, "New York");
                table.SetCellValue(2, 0, "Jane Smith");
                table.SetCellValue(2, 1, "25");
                table.SetCellValue(2, 2, "Los Angeles");
                table.SetCellValue(3, 0, "Bob Johnson");
                table.SetCellValue(3, 1, "35");
                table.SetCellValue(3, 2, "Chicago");

                document.SaveAs("MultiPageDocument.pdf");
                Console.WriteLine("Multi-page document saved as MultiPageDocument.pdf");

                document.Dispose();
            }
        }

        /// <summary>
        /// Example 4: Using the fluent builder API
        /// </summary>
        public static void FluentBuilderAPI()
        {
            Console.WriteLine("=== Fluent Builder API ===");

            var result = PdfFactory.CreateBuilder()
                .WithTitle("Fluent API Example")
                .WithAuthor("AMCode Library")
                .WithSubject("Builder Pattern Example")
                .WithKeywords("fluent, builder, pdf")
                .WithPage(page =>
                {
                    page.AddText("Fluent API Example", 200, 100, new FontStyle { FontSize = 20, Bold = true });
                    page.AddText("This document was created using the fluent builder API", 150, 150);
                    page.AddRectangle(100, 200, 400, 200, Color.LightBlue, Color.DarkGray);
                    page.AddText("Builder Pattern", 250, 280, new FontStyle { FontSize = 16, Bold = true });
                })
                .WithPage(page =>
                {
                    page.AddText("Second Page", 200, 100, new FontStyle { FontSize = 18, Bold = true });
                    page.AddText("This is the second page created with the fluent API", 150, 150);
                })
                .Build();

            if (result.IsSuccess)
            {
                result.Value.SaveAs("FluentBuilderExample.pdf");
                Console.WriteLine("Fluent builder document saved as FluentBuilderExample.pdf");
                result.Value.Dispose();
            }
        }

        /// <summary>
        /// Example 5: Using different providers
        /// </summary>
        public static void DifferentProviders()
        {
            Console.WriteLine("=== Different Providers ===");

            // Register providers
            var logger = new PdfLogger();
            var validator = new PdfValidator();
            var questPdfProvider = new QuestPdfProvider(logger, validator);
            var iTextSharpProvider = new iTextSharpProvider(logger, validator);

            PdfFactory.RegisterProvider("QuestPDF", questPdfProvider);
            PdfFactory.RegisterProvider("iTextSharp", iTextSharpProvider);

            // Create document with QuestPDF
            var questResult = PdfFactory.CreateDocument("QuestPDF");
            if (questResult.IsSuccess)
            {
                var page = questResult.Value.Pages.Create();
                page.AddText("Created with QuestPDF", 100, 100);
                questResult.Value.SaveAs("QuestPDFDocument.pdf");
                Console.WriteLine("QuestPDF document saved as QuestPDFDocument.pdf");
                questResult.Value.Dispose();
            }

            // Create document with iTextSharp
            var iTextResult = PdfFactory.CreateDocument("iTextSharp");
            if (iTextResult.IsSuccess)
            {
                var page = iTextResult.Value.Pages.Create();
                page.AddText("Created with iTextSharp", 100, 100);
                iTextResult.Value.SaveAs("iTextSharpDocument.pdf");
                Console.WriteLine("iTextSharp document saved as iTextSharpDocument.pdf");
                iTextResult.Value.Dispose();
            }
        }

        /// <summary>
        /// Example 6: Document validation
        /// </summary>
        public static void DocumentValidation()
        {
            Console.WriteLine("=== Document Validation ===");

            var result = PdfFactory.CreateDocument();
            if (result.IsSuccess)
            {
                var document = result.Value;
                var page = document.Pages.Create();
                page.AddText("Validation Example", 100, 100);

                // Validate the document
                var validator = new PdfValidator();
                var validationResult = validator.ValidateDocument(document);

                if (validationResult.IsSuccess)
                {
                    Console.WriteLine("Document validation passed");
                }
                else
                {
                    Console.WriteLine($"Document validation failed: {validationResult.Error}");
                }

                // Validate individual page
                var pageValidationResult = validator.ValidatePage(page);
                if (pageValidationResult.IsSuccess)
                {
                    Console.WriteLine("Page validation passed");
                }
                else
                {
                    Console.WriteLine($"Page validation failed: {pageValidationResult.Error}");
                }

                document.SaveAs("ValidationExample.pdf");
                Console.WriteLine("Validation example saved as ValidationExample.pdf");

                document.Dispose();
            }
        }

        /// <summary>
        /// Example 7: Error handling
        /// </summary>
        public static void ErrorHandling()
        {
            Console.WriteLine("=== Error Handling ===");

            try
            {
                // Try to create document without provider
                PdfFactory.SetDefaultProvider(null);
                var result = PdfFactory.CreateDocument();

                if (!result.IsSuccess)
                {
                    Console.WriteLine($"Expected error: {result.Error}");
                }

                // Reset to valid provider
                PdfFactory.InitializeDefaultProviders();
                var validResult = PdfFactory.CreateDocument();

                if (validResult.IsSuccess)
                {
                    Console.WriteLine("Successfully created document after error handling");
                    validResult.Value.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }
        }

        /// <summary>
        /// Example 8: Performance testing
        /// </summary>
        public static void PerformanceExample()
        {
            Console.WriteLine("=== Performance Example ===");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            const int documentCount = 10;

            for (int i = 0; i < documentCount; i++)
            {
                var result = PdfFactory.CreateDocument();
                if (result.IsSuccess)
                {
                    var page = result.Value.Pages.Create();
                    page.AddText($"Document {i + 1}", 100, 100);
                    page.AddRectangle(50, 50, 200, 100, Color.LightBlue);
                    result.Value.Dispose();
                }
            }

            stopwatch.Stop();
            var averageTime = stopwatch.ElapsedMilliseconds / (double)documentCount;

            Console.WriteLine($"Created {documentCount} documents in {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Average time per document: {averageTime:F2}ms");
        }

        /// <summary>
        /// Run all examples
        /// </summary>
        public static void RunAllExamples()
        {
            Console.WriteLine("Running AMCode PDF Library Examples...\n");

            try
            {
                BasicDocumentCreation();
                Console.WriteLine();

                DocumentWithProperties();
                Console.WriteLine();

                MultiPageDocument();
                Console.WriteLine();

                FluentBuilderAPI();
                Console.WriteLine();

                DifferentProviders();
                Console.WriteLine();

                DocumentValidation();
                Console.WriteLine();

                ErrorHandling();
                Console.WriteLine();

                PerformanceExample();
                Console.WriteLine();

                Console.WriteLine("All examples completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running examples: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
