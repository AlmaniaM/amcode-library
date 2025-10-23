using System;
using AMCode.Documents.Pdf;

namespace AMCode.Documents.Pdf.Examples
{
    /// <summary>
    /// Main example runner for the AMCode PDF Library
    /// </summary>
    public class PdfExampleRunner
    {
        /// <summary>
        /// Main entry point for running PDF examples
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            Console.WriteLine("==========================================");
            Console.WriteLine("    AMCode PDF Library Examples");
            Console.WriteLine("==========================================");
            Console.WriteLine();

            // Initialize the PDF library
            try
            {
                PdfFactory.InitializeDefaultProviders();
                Console.WriteLine("✓ PDF Library initialized successfully");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Failed to initialize PDF library: {ex.Message}");
                return;
            }

            // Parse command line arguments
            var runBasic = args.Length == 0 || Array.Exists(args, arg => arg.ToLower() == "basic");
            var runAdvanced = args.Length == 0 || Array.Exists(args, arg => arg.ToLower() == "advanced");
            var runPerformance = args.Length == 0 || Array.Exists(args, arg => arg.ToLower() == "performance");
            var runAll = args.Length == 0 || Array.Exists(args, arg => arg.ToLower() == "all");

            if (runAll || runBasic)
            {
                Console.WriteLine("Running Basic Examples...");
                Console.WriteLine("==========================");
                PdfUsageExamples.RunAllExamples();
                Console.WriteLine();
            }

            if (runAll || runAdvanced)
            {
                Console.WriteLine("Running Advanced Examples...");
                Console.WriteLine("=============================");
                AdvancedPdfExamples.RunAllAdvancedExamples();
                Console.WriteLine();
            }

            if (runAll || runPerformance)
            {
                Console.WriteLine("Running Performance Examples...");
                Console.WriteLine("===============================");
                PerformanceExamples.RunAllPerformanceExamples();
                Console.WriteLine();
            }

            Console.WriteLine("==========================================");
            Console.WriteLine("    All Examples Completed Successfully!");
            Console.WriteLine("==========================================");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  dotnet run                    - Run all examples");
            Console.WriteLine("  dotnet run basic              - Run basic examples only");
            Console.WriteLine("  dotnet run advanced           - Run advanced examples only");
            Console.WriteLine("  dotnet run performance        - Run performance examples only");
            Console.WriteLine("  dotnet run all                - Run all examples");
        }

        /// <summary>
        /// Display help information
        /// </summary>
        public static void ShowHelp()
        {
            Console.WriteLine("AMCode PDF Library Examples");
            Console.WriteLine("============================");
            Console.WriteLine();
            Console.WriteLine("This example runner demonstrates various features of the AMCode PDF Library:");
            Console.WriteLine();
            Console.WriteLine("Basic Examples:");
            Console.WriteLine("  - Simple document creation");
            Console.WriteLine("  - Document with custom properties");
            Console.WriteLine("  - Multi-page documents");
            Console.WriteLine("  - Fluent builder API");
            Console.WriteLine("  - Different providers");
            Console.WriteLine("  - Document validation");
            Console.WriteLine("  - Error handling");
            Console.WriteLine("  - Performance testing");
            Console.WriteLine();
            Console.WriteLine("Advanced Examples:");
            Console.WriteLine("  - Professional invoice creation");
            Console.WriteLine("  - Technical reports with charts");
            Console.WriteLine("  - Form documents");
            Console.WriteLine("  - Multi-language documents");
            Console.WriteLine("  - Branded documents");
            Console.WriteLine();
            Console.WriteLine("Performance Examples:");
            Console.WriteLine("  - High-volume document generation");
            Console.WriteLine("  - Batch document processing");
            Console.WriteLine("  - Memory usage monitoring");
            Console.WriteLine("  - Provider performance comparison");
            Console.WriteLine("  - Concurrent document processing");
            Console.WriteLine("  - Document caching and reuse");
            Console.WriteLine();
            Console.WriteLine("Command Line Options:");
            Console.WriteLine("  basic       - Run basic examples only");
            Console.WriteLine("  advanced    - Run advanced examples only");
            Console.WriteLine("  performance - Run performance examples only");
            Console.WriteLine("  all         - Run all examples (default)");
            Console.WriteLine("  help        - Show this help message");
        }
    }
}
