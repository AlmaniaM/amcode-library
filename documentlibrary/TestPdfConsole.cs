using System;
using AMCode.Pdf;
using AMCode.Documents.Common.Models;

namespace TestPdfConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing PDF Library...");
            
            try
            {
                // Test 1: Create a simple document
                Console.WriteLine("Test 1: Creating a simple document...");
                var logger = new TestPdfLogger();
                var validator = new PdfValidator();
                var provider = new QuestPdfProvider(logger, validator);
                
                var result = provider.CreateDocument();
                if (result.IsSuccess)
                {
                    Console.WriteLine("✅ Document created successfully!");
                    var document = result.Value;
                    Console.WriteLine($"   Document ID: {document.Id}");
                    Console.WriteLine($"   Created at: {document.CreatedAt}");
                    Console.WriteLine($"   Pages count: {document.Pages.Count}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create document: {result.ErrorMessage}");
                    return;
                }

                // Test 2: Add a page with content
                Console.WriteLine("\nTest 2: Adding a page with content...");
                var page = document.Pages.Create();
                page.AddText("Hello from PDF Library!", 100, 100);
                page.AddRectangle(50, 50, 200, 100, Color.LightBlue);
                
                Console.WriteLine($"✅ Page added successfully! Total pages: {document.Pages.Count}");

                // Test 3: Test document properties
                Console.WriteLine("\nTest 3: Testing document properties...");
                Console.WriteLine($"   Document ID: {document.Id}");
                Console.WriteLine($"   Created at: {document.CreatedAt}");
                Console.WriteLine($"   Last modified: {document.LastModified}");
                Console.WriteLine($"   Pages count: {document.Pages.Count}");
                Console.WriteLine($"   Properties title: {document.Properties.Title}");

                // Test 4: Test with custom properties
                Console.WriteLine("\nTest 4: Creating document with custom properties...");
                var properties = new PdfProperties
                {
                    Title = "Test PDF Document",
                    Author = "PDF Library Test",
                    Subject = "Testing PDF functionality",
                    Keywords = "test, pdf, library"
                };
                
                var result2 = provider.CreateDocument(properties);
                if (result2.IsSuccess)
                {
                    Console.WriteLine("✅ Document with custom properties created successfully!");
                    var document2 = result2.Value;
                    Console.WriteLine($"   Title: {document2.Properties.Title}");
                    Console.WriteLine($"   Author: {document2.Properties.Author}");
                    Console.WriteLine($"   Subject: {document2.Properties.Subject}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create document with properties: {result2.ErrorMessage}");
                }

                Console.WriteLine("\n🎉 All tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error during testing: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }

    // Simple test logger for console testing
    public class TestPdfLogger : IPdfLogger
    {
        public void LogDocumentOperation(string operation, object context = null)
        {
            Console.WriteLine($"LOG: Document operation: {operation}");
        }

        public void LogError(string operation, Exception exception)
        {
            Console.WriteLine($"ERROR: {operation} - {exception.Message}");
        }

        public void LogWarning(string message, object context = null)
        {
            Console.WriteLine($"WARNING: {message}");
        }

        public void LogInformation(string message, object context = null)
        {
            Console.WriteLine($"INFO: {message}");
        }

        public void LogDebug(string message, object context = null)
        {
            Console.WriteLine($"DEBUG: {message}");
        }
    }
}
