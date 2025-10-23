using System;
using System.IO;
using AMCode.Pdf;
using AMCode.Documents.Common.Models;

namespace PdfTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting PDF Console Test...");

            try
            {
                // Configure the PDF factory with a QuestPDF provider
                var logger = new TestPdfLogger();
                var validator = new PdfValidator();
                var questPdfProvider = new QuestPdfProvider(logger, validator);
                PdfFactory.SetDefaultProvider(questPdfProvider);

                // Create a new PDF document
                var createResult = PdfFactory.CreateDocument();

                if (createResult.IsSuccess)
                {
                    var document = createResult.Value;
                    Console.WriteLine($"PDF Document created successfully with ID: {document.Id}");

                    // Add a page and some content
                    var page = document.Pages.Create();
                    page.AddText("Hello from TestPdfConsole!", 50, 50);
                    Console.WriteLine("Added text to the page.");

                    // Save the document
                    string filePath = "TestDocument.pdf";
                    document.SaveAs(filePath);
                    Console.WriteLine($"Document saved to {filePath}");

                    // Close and dispose
                    document.Close();
                    Console.WriteLine("Document closed.");
                }
                else
                {
                    Console.WriteLine($"Failed to create PDF document: {createResult.Error}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("PDF Console Test Finished.");
        }
    }

    // Simple test logger implementation
    public class TestPdfLogger : IPdfLogger
    {
        public void LogDocumentOperation(string operation, object context = null)
        {
            Console.WriteLine($"DOCUMENT_OP: {operation}");
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