using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf
{
    /// <summary>
    /// Test class to verify PDF factory and builder functionality
    /// This is a temporary file for testing purposes
    /// </summary>
    public class PdfFactoryTest
    {
        /// <summary>
        /// Test basic PDF factory functionality
        /// </summary>
        public static void TestPdfFactory()
        {
            try
            {
                Console.WriteLine("Testing PDF Factory...");
                
                // Initialize default providers
                PdfFactory.InitializeDefaultProviders();
                Console.WriteLine("✓ Initialized default providers");
                
                // Test provider registration
                var availableProviders = PdfFactory.GetAvailableProviders();
                Console.WriteLine($"✓ Available providers: {string.Join(", ", availableProviders)}");
                
                // Test default provider
                var defaultProvider = PdfFactory.GetDefaultProvider();
                if (defaultProvider != null)
                {
                    Console.WriteLine($"✓ Default provider: {defaultProvider.Name} v{defaultProvider.Version}");
                }
                
                // Test document creation
                var documentResult = PdfFactory.CreateDocument();
                if (documentResult.IsSuccess)
                {
                    Console.WriteLine("✓ Created PDF document successfully");
                    Console.WriteLine($"  Document ID: {documentResult.Value.Id}");
                    Console.WriteLine($"  Created: {documentResult.Value.CreatedAt}");
                    Console.WriteLine($"  Pages: {documentResult.Value.Pages.Count}");
                }
                else
                {
                    Console.WriteLine($"✗ Failed to create PDF document: {documentResult.Error}");
                }
                
                // Test document creation with properties
                var properties = new PdfProperties("Test Document", "Test Author", "Test Subject", "test, pdf");
                var documentWithPropsResult = PdfFactory.CreateDocument(properties);
                if (documentWithPropsResult.IsSuccess)
                {
                    Console.WriteLine("✓ Created PDF document with properties successfully");
                    Console.WriteLine($"  Title: {documentWithPropsResult.Value.Properties.Title}");
                    Console.WriteLine($"  Author: {documentWithPropsResult.Value.Properties.Author}");
                }
                else
                {
                    Console.WriteLine($"✗ Failed to create PDF document with properties: {documentWithPropsResult.Error}");
                }
                
                Console.WriteLine("✓ PDF Factory test completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ PDF Factory test failed: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// Test PDF builder functionality
        /// </summary>
        public static void TestPdfBuilder()
        {
            try
            {
                Console.WriteLine("Testing PDF Builder...");
                
                // Initialize default providers
                PdfFactory.InitializeDefaultProviders();
                
                // Test fluent builder
                var builderResult = PdfFactory.CreateBuilder()
                    .WithTitle("Builder Test Document")
                    .WithAuthor("Test Author")
                    .WithSubject("Builder Testing")
                    .WithKeywords("test, builder, pdf")
                    .WithPage(page =>
                    {
                        page.AddText("Hello from Builder!", 100, 100);
                        page.AddRectangle(50, 50, 200, 100, Color.LightBlue);
                    })
                    .WithPage(PageSize.A4, page =>
                    {
                        page.AddText("Second Page", 100, 100);
                    })
                    .Build();
                
                if (builderResult.IsSuccess)
                {
                    Console.WriteLine("✓ PDF Builder created document successfully");
                    Console.WriteLine($"  Title: {builderResult.Value.Properties.Title}");
                    Console.WriteLine($"  Author: {builderResult.Value.Properties.Author}");
                    Console.WriteLine($"  Pages: {builderResult.Value.Pages.Count}");
                }
                else
                {
                    Console.WriteLine($"✗ PDF Builder failed: {builderResult.Error}");
                }
                
                Console.WriteLine("✓ PDF Builder test completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ PDF Builder test failed: {ex.Message}");
                throw;
            }
        }
    }
}
