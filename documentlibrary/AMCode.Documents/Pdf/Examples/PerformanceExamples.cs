using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using AMCode.Documents.Pdf;
using AMCode.Documents.Pdf.Infrastructure.Performance;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.Pdf.Examples
{
    /// <summary>
    /// Performance optimization examples for the AMCode PDF library
    /// </summary>
    public static class PerformanceExamples
    {
        /// <summary>
        /// Example: High-volume document generation with memory efficiency
        /// </summary>
        public static void HighVolumeDocumentGeneration()
        {
            Console.WriteLine("=== High-Volume Document Generation ===");

            const int documentCount = 100;
            var stopwatch = Stopwatch.StartNew();

            // Initialize memory-efficient factory
            var logger = new PdfLogger();
            var validator = new PdfValidator();
            var questPdfProvider = new QuestPdfProvider(logger, validator);
            var memoryFactory = new PdfMemoryEfficientFactory(questPdfProvider, logger, validator);

            try
            {
                for (int i = 0; i < documentCount; i++)
                {
                    var documentResult = memoryFactory.CreateDocument();
                    if (documentResult.IsSuccess)
                    {
                        var document = documentResult.Value;
                        var page = document.Pages.Create();
                        page.AddText($"Document #{i + 1}", 100, 100);
                        page.AddText($"Generated at: {DateTime.Now:HH:mm:ss}", 100, 120);
                        
                        // Return to pool for reuse
                        memoryFactory.ReturnDocument(document);
                    }
                }

                stopwatch.Stop();
                var stats = memoryFactory.GetMemoryStats();

                Console.WriteLine($"Generated {documentCount} documents in {stopwatch.ElapsedMilliseconds}ms");
                Console.WriteLine($"Average time per document: {stopwatch.ElapsedMilliseconds / (double)documentCount:F2}ms");
                Console.WriteLine($"Document pool size: {stats.DocumentPoolSize}");
                Console.WriteLine($"Pages pool size: {stats.PagesPoolSize}");
                Console.WriteLine($"Page pool size: {stats.PagePoolSize}");
            }
            finally
            {
                memoryFactory.Dispose();
            }
        }

        /// <summary>
        /// Example: Batch document processing with performance optimization
        /// </summary>
        public static void BatchDocumentProcessing()
        {
            Console.WriteLine("=== Batch Document Processing ===");

            const int batchSize = 50;
            var documents = new List<IPdfDocument>();

            // Initialize performance optimizer
            var logger = new PdfLogger();
            var optimizer = new PdfPerformanceOptimizer(logger);
            var validator = new PdfValidator();
            var questPdfProvider = new QuestPdfProvider(logger, validator);

            PdfFactory.RegisterProvider("QuestPDF", questPdfProvider);
            PdfFactory.SetDefaultProvider(questPdfProvider);

            try
            {
                var stopwatch = Stopwatch.StartNew();

                // Create batch of documents
                for (int i = 0; i < batchSize; i++)
                {
                    var result = PdfFactory.CreateDocument("QuestPDF");
                    if (result.IsSuccess)
                    {
                        var document = result.Value;
                        var page = document.Pages.Create();
                        
                        page.AddText($"Batch Document #{i + 1}", 100, 100);
                        page.AddText($"Processing Time: {DateTime.Now:HH:mm:ss.fff}", 100, 120);
                        
                        // Add some content to make it realistic
                        for (int j = 0; j < 10; j++)
                        {
                            page.AddText($"Line {j + 1}: Sample content for document {i + 1}", 100, 140 + (j * 15));
                        }

                        documents.Add(document);
                    }
                }

                stopwatch.Stop();
                Console.WriteLine($"Created {documents.Count} documents in {stopwatch.ElapsedMilliseconds}ms");

                // Process documents (simulate some processing)
                stopwatch.Restart();
                foreach (var document in documents)
                {
                    // Simulate processing by accessing properties
                    var _ = document.Properties.Title;
                    var __ = document.Pages.Count;
                }
                stopwatch.Stop();
                Console.WriteLine($"Processed {documents.Count} documents in {stopwatch.ElapsedMilliseconds}ms");

                // Save documents
                stopwatch.Restart();
                for (int i = 0; i < documents.Count; i++)
                {
                    documents[i].SaveAs($"BatchDocument_{i + 1}.pdf");
                }
                stopwatch.Stop();
                Console.WriteLine($"Saved {documents.Count} documents in {stopwatch.ElapsedMilliseconds}ms");

                // Clean up
                foreach (var document in documents)
                {
                    document.Dispose();
                }

                Console.WriteLine("Batch processing completed successfully");
            }
            finally
            {
                optimizer.Dispose();
            }
        }

        /// <summary>
        /// Example: Memory usage monitoring during document creation
        /// </summary>
        public static void MemoryUsageMonitoring()
        {
            Console.WriteLine("=== Memory Usage Monitoring ===");

            var initialMemory = GC.GetTotalMemory(true);
            Console.WriteLine($"Initial memory usage: {initialMemory / 1024.0:F2} KB");

            const int documentCount = 25;
            var documents = new List<IPdfDocument>();

            try
            {
                for (int i = 0; i < documentCount; i++)
                {
                    var result = PdfFactory.CreateDocument();
                    if (result.IsSuccess)
                    {
                        var document = result.Value;
                        var page = document.Pages.Create();
                        
                        // Add substantial content
                        page.AddText($"Memory Test Document #{i + 1}", 100, 100);
                        
                        // Add multiple elements to increase memory usage
                        for (int j = 0; j < 20; j++)
                        {
                            page.AddText($"Content line {j + 1}", 100, 120 + (j * 15));
                            page.AddRectangle(50 + j * 5, 200 + j * 3, 10, 10, Color.LightBlue);
                        }

                        documents.Add(document);

                        if (i % 5 == 0)
                        {
                            var currentMemory = GC.GetTotalMemory(false);
                            var memoryIncrease = (currentMemory - initialMemory) / 1024.0;
                            Console.WriteLine($"After {i + 1} documents: {memoryIncrease:F2} KB increase");
                        }
                    }
                }

                var peakMemory = GC.GetTotalMemory(false);
                var peakIncrease = (peakMemory - initialMemory) / 1024.0;
                Console.WriteLine($"Peak memory increase: {peakIncrease:F2} KB");

                // Dispose documents and measure memory cleanup
                foreach (var document in documents)
                {
                    document.Dispose();
                }
                documents.Clear();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var finalMemory = GC.GetTotalMemory(false);
                var finalIncrease = (finalMemory - initialMemory) / 1024.0;
                Console.WriteLine($"Final memory increase after cleanup: {finalIncrease:F2} KB");
                Console.WriteLine($"Memory cleanup efficiency: {((peakIncrease - finalIncrease) / peakIncrease * 100):F1}%");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during memory monitoring: {ex.Message}");
            }
        }

        /// <summary>
        /// Example: Provider performance comparison
        /// </summary>
        public static void ProviderPerformanceComparison()
        {
            Console.WriteLine("=== Provider Performance Comparison ===");

            const int testCount = 20;
            var logger = new PdfLogger();
            var validator = new PdfValidator();

            // Test QuestPDF
            var questPdfProvider = new QuestPdfProvider(logger, validator);
            var questStopwatch = Stopwatch.StartNew();
            var questDocuments = new List<IPdfDocument>();

            try
            {
                for (int i = 0; i < testCount; i++)
                {
                    var result = PdfFactory.CreateDocument();
                    if (result.IsSuccess)
                    {
                        var document = result.Value;
                        var page = document.Pages.Create();
                        page.AddText($"QuestPDF Document #{i + 1}", 100, 100);
                        questDocuments.Add(document);
                    }
                }
                questStopwatch.Stop();

                Console.WriteLine($"QuestPDF: Created {questDocuments.Count} documents in {questStopwatch.ElapsedMilliseconds}ms");
                Console.WriteLine($"QuestPDF: Average time per document: {questStopwatch.ElapsedMilliseconds / (double)questDocuments.Count:F2}ms");

                // Clean up QuestPDF documents
                foreach (var document in questDocuments)
                {
                    document.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"QuestPDF test failed: {ex.Message}");
            }

            // Test iTextSharp
            var iTextSharpProvider = new iTextSharpProvider(logger, validator);
            PdfFactory.RegisterProvider("iTextSharp", iTextSharpProvider);
            PdfFactory.SetDefaultProvider(iTextSharpProvider);

            var iTextStopwatch = Stopwatch.StartNew();
            var iTextDocuments = new List<IPdfDocument>();

            try
            {
                for (int i = 0; i < testCount; i++)
                {
                    var result = PdfFactory.CreateDocument("iTextSharp");
                    if (result.IsSuccess)
                    {
                        var document = result.Value;
                        var page = document.Pages.Create();
                        page.AddText($"iTextSharp Document #{i + 1}", 100, 100);
                        iTextDocuments.Add(document);
                    }
                }
                iTextStopwatch.Stop();

                Console.WriteLine($"iTextSharp: Created {iTextDocuments.Count} documents in {iTextStopwatch.ElapsedMilliseconds}ms");
                Console.WriteLine($"iTextSharp: Average time per document: {iTextStopwatch.ElapsedMilliseconds / (double)iTextDocuments.Count:F2}ms");

                // Clean up iTextSharp documents
                foreach (var document in iTextDocuments)
                {
                    document.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"iTextSharp test failed: {ex.Message}");
            }

            // Performance comparison
            if (questStopwatch.ElapsedMilliseconds > 0 && iTextStopwatch.ElapsedMilliseconds > 0)
            {
                var questAvg = questStopwatch.ElapsedMilliseconds / (double)testCount;
                var iTextAvg = iTextStopwatch.ElapsedMilliseconds / (double)testCount;
                var performanceRatio = questAvg / iTextAvg;

                Console.WriteLine($"Performance Comparison:");
                Console.WriteLine($"QuestPDF is {performanceRatio:F2}x {(performanceRatio < 1 ? "faster" : "slower")} than iTextSharp");
            }
        }

        /// <summary>
        /// Example: Concurrent document processing
        /// </summary>
        public static void ConcurrentDocumentProcessing()
        {
            Console.WriteLine("=== Concurrent Document Processing ===");

            const int threadCount = 4;
            const int documentsPerThread = 10;
            var tasks = new List<System.Threading.Tasks.Task>();
            var stopwatch = Stopwatch.StartNew();

            for (int threadId = 0; threadId < threadCount; threadId++)
            {
                int currentThreadId = threadId; // Capture for closure
                var task = System.Threading.Tasks.Task.Run(() =>
                {
                    var threadDocuments = new List<IPdfDocument>();
                    
                    try
                    {
                        for (int i = 0; i < documentsPerThread; i++)
                        {
                            var result = PdfFactory.CreateDocument();
                            if (result.IsSuccess)
                            {
                                var document = result.Value;
                                var page = document.Pages.Create();
                                page.AddText($"Thread {currentThreadId} - Document {i + 1}", 100, 100);
                                page.AddText($"Created at: {DateTime.Now:HH:mm:ss.fff}", 100, 120);
                                threadDocuments.Add(document);
                            }
                        }

                        Console.WriteLine($"Thread {currentThreadId}: Created {threadDocuments.Count} documents");

                        // Clean up
                        foreach (var document in threadDocuments)
                        {
                            document.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Thread {currentThreadId} failed: {ex.Message}");
                    }
                });

                tasks.Add(task);
            }

            // Wait for all threads to complete
            System.Threading.Tasks.Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();

            var totalDocuments = threadCount * documentsPerThread;
            Console.WriteLine($"Concurrent processing completed:");
            Console.WriteLine($"Total documents: {totalDocuments}");
            Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Average time per document: {stopwatch.ElapsedMilliseconds / (double)totalDocuments:F2}ms");
            Console.WriteLine($"Documents per second: {totalDocuments / (stopwatch.ElapsedMilliseconds / 1000.0):F2}");
        }

        /// <summary>
        /// Example: Document caching and reuse
        /// </summary>
        public static void DocumentCachingExample()
        {
            Console.WriteLine("=== Document Caching and Reuse ===");

            var cache = new Dictionary<string, IPdfDocument>();
            const int cacheSize = 5;
            const int requests = 20;

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < requests; i++)
            {
                var documentKey = $"template_{i % cacheSize}"; // Simulate template reuse

                if (!cache.ContainsKey(documentKey))
                {
                    // Create new document template
                    var result = PdfFactory.CreateDocument();
                    if (result.IsSuccess)
                    {
                        var document = result.Value;
                        var page = document.Pages.Create();
                        page.AddText($"Template {documentKey}", 100, 100);
                        page.AddText($"Created at: {DateTime.Now:HH:mm:ss.fff}", 100, 120);
                        
                        cache[documentKey] = document;
                        Console.WriteLine($"Created new template: {documentKey}");
                    }
                }
                else
                {
                    Console.WriteLine($"Reused cached template: {documentKey}");
                }
            }

            stopwatch.Stop();

            Console.WriteLine($"Caching test completed:");
            Console.WriteLine($"Total requests: {requests}");
            Console.WriteLine($"Cache size: {cache.Count}");
            Console.WriteLine($"Cache hit ratio: {(requests - cache.Count) / (double)requests * 100:F1}%");
            Console.WriteLine($"Total time: {stopwatch.ElapsedMilliseconds}ms");

            // Clean up cache
            foreach (var document in cache.Values)
            {
                document.Dispose();
            }
        }

        /// <summary>
        /// Run all performance examples
        /// </summary>
        public static void RunAllPerformanceExamples()
        {
            Console.WriteLine("Running AMCode PDF Library Performance Examples...\n");

            try
            {
                HighVolumeDocumentGeneration();
                Console.WriteLine();

                BatchDocumentProcessing();
                Console.WriteLine();

                MemoryUsageMonitoring();
                Console.WriteLine();

                ProviderPerformanceComparison();
                Console.WriteLine();

                ConcurrentDocumentProcessing();
                Console.WriteLine();

                DocumentCachingExample();
                Console.WriteLine();

                Console.WriteLine("All performance examples completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running performance examples: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
