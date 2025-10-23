using System;
using AMCode.Documents.Docx;

namespace TestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AMCode.Documents DOCX Library Integration Test Runner");
            Console.WriteLine("=====================================================");
            
            try
            {
                // Run basic integration test
                IntegrationTest.RunBasicDocumentTest();
                
                // Run advanced integration test
                IntegrationTest.RunAdvancedDocumentTest();
                
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Test runner failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
