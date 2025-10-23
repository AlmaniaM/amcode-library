using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Pdf.Infrastructure.Performance;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AMCode.Documents.UnitTests.Pdf.Performance
{
    [TestFixture]
    public class PdfOptimizedPerformanceTests
    {
        private IPdfProvider _questPdfProvider;
        private IPdfProvider _iTextSharpProvider;
        private IPdfLogger _logger;
        private IPdfValidator _validator;
        private PdfMemoryEfficientFactory _memoryFactory;

        [SetUp]
        public void Setup()
        {
            _logger = new TestPdfLogger();
            _validator = new PdfValidator();
            _questPdfProvider = new QuestPdfProvider(_logger, _validator);
            _iTextSharpProvider = new iTextSharpProvider(_logger, _validator);
            _memoryFactory = new PdfMemoryEfficientFactory(_questPdfProvider, _logger, _validator);
        }

        [TearDown]
        public void TearDown()
        {
            _memoryFactory?.Dispose();
        }

        [Test]
        public void MemoryEfficientFactory_DocumentCreation_PerformanceTest()
        {
            // Arrange
            const int iterations = 100;
            var stopwatch = new Stopwatch();
            var documents = new IPdfDocument[iterations];

            // Act
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                var result = _memoryFactory.CreateDocument();
                Assert.IsTrue(result.IsSuccess, $"Failed to create document {i}: {result.Error}");
                documents[i] = result.Value;
            }
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Console.WriteLine($"Memory Efficient Factory: Average document creation time: {averageTime:F2}ms for {iterations} iterations");
            
            // Performance assertion: should be faster than regular factory
            Assert.Less(averageTime, 5.0, "Memory efficient factory should create documents faster than 5ms average");

            // Cleanup
            foreach (var doc in documents)
            {
                _memoryFactory.ReturnDocument(doc);
                doc?.Dispose();
            }
        }

        [Test]
        public void MemoryEfficientFactory_MemoryUsage_OptimizationTest()
        {
            // Arrange
            const int iterations = 100;
            var initialMemory = GC.GetTotalMemory(true);
            var documents = new IPdfDocument[iterations];

            // Act - Create documents
            for (int i = 0; i < iterations; i++)
            {
                var result = _memoryFactory.CreateDocument();
                Assert.IsTrue(result.IsSuccess);
                documents[i] = result.Value;
            }

            var memoryAfterCreation = GC.GetTotalMemory(false);
            var memoryIncrease = memoryAfterCreation - initialMemory;

            // Act - Return documents to pool
            foreach (var doc in documents)
            {
                _memoryFactory.ReturnDocument(doc);
            }

            var memoryAfterReturn = GC.GetTotalMemory(false);
            var memoryAfterReturnIncrease = memoryAfterReturn - initialMemory;

            // Assert
            Console.WriteLine($"Memory Efficient Factory - Memory increase after creation: {memoryIncrease / 1024.0:F2}KB");
            Console.WriteLine($"Memory Efficient Factory - Memory increase after return to pool: {memoryAfterReturnIncrease / 1024.0:F2}KB");
            
            // Memory should be significantly lower than regular factory
            Assert.Less(memoryAfterReturnIncrease, 50 * 1024, "Memory usage should be less than 50KB after optimization");

            // Cleanup
            foreach (var doc in documents)
            {
                doc?.Dispose();
            }
        }

        [Test]
        public void ObjectPooling_Reuse_EffectivenessTest()
        {
            // Arrange
            const int iterations = 50;
            var documents = new IPdfDocument[iterations];

            // Act - Create and return documents multiple times
            for (int round = 0; round < 3; round++)
            {
                for (int i = 0; i < iterations; i++)
                {
                    var result = _memoryFactory.CreateDocument();
                    Assert.IsTrue(result.IsSuccess);
                    documents[i] = result.Value;
                }

                // Return all documents to pool
                foreach (var doc in documents)
                {
                    _memoryFactory.ReturnDocument(doc);
                }
            }

            // Assert - Check pool statistics
            var stats = _memoryFactory.GetMemoryStats();
            Console.WriteLine($"Pool Statistics:");
            Console.WriteLine($"  Document Pool Size: {stats.DocumentPoolSize}");
            Console.WriteLine($"  Pages Pool Size: {stats.PagesPoolSize}");
            Console.WriteLine($"  Page Pool Size: {stats.PagePoolSize}");
            Console.WriteLine($"  Optimizer Cache Size: {stats.OptimizerStats.CacheSize}");

            // Should have objects in pool
            Assert.Greater(stats.DocumentPoolSize, 0, "Should have documents in pool");
            Assert.Greater(stats.PagesPoolSize, 0, "Should have pages in pool");

            // Cleanup
            foreach (var doc in documents)
            {
                doc?.Dispose();
            }
        }

        [Test]
        public void PerformanceOptimizer_Caching_EffectivenessTest()
        {
            // Arrange
            var optimizer = new PdfPerformanceOptimizer(_logger);
            const int iterations = 100;
            var stopwatch = new Stopwatch();

            // Act - Test caching
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                var properties = optimizer.GetProperties($"Document {i}", "Test Author");
                var metadata = optimizer.GetMetadataAdapter(properties);
                
                // Return to pool
                optimizer.ReturnProperties(properties);
                optimizer.ReturnMetadataAdapter(metadata);
            }
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Console.WriteLine($"Performance Optimizer: Average time per operation: {averageTime:F2}ms for {iterations} iterations");
            
            var stats = optimizer.GetStats();
            Console.WriteLine($"Optimizer Statistics:");
            Console.WriteLine($"  Properties Pool Size: {stats.PropertiesPoolSize}");
            Console.WriteLine($"  Metadata Pool Size: {stats.MetadataPoolSize}");
            Console.WriteLine($"  Cache Size: {stats.CacheSize}");

            // Should be fast and have pooled objects
            Assert.Less(averageTime, 1.0, "Optimized operations should be faster than 1ms average");
            Assert.Greater(stats.PropertiesPoolSize, 0, "Should have properties in pool");
            Assert.Greater(stats.MetadataPoolSize, 0, "Should have metadata in pool");

            optimizer.Dispose();
        }

        [Test]
        public void LargeDocument_Performance_OptimizationTest()
        {
            // Arrange
            const int pageCount = 50;
            const int textElementsPerPage = 20;
            var stopwatch = new Stopwatch();
            var documents = new IPdfDocument[10];

            // Act - Create multiple large documents
            stopwatch.Start();
            for (int docIndex = 0; docIndex < 10; docIndex++)
            {
                var result = _memoryFactory.CreateDocument();
                Assert.IsTrue(result.IsSuccess);
                var document = result.Value;
                documents[docIndex] = document;

                // Add many pages with content
                for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
                {
                    var page = document.Pages.Create();
                    
                    // Add multiple text elements
                    for (int textIndex = 0; textIndex < textElementsPerPage; textIndex++)
                    {
                        page.AddText($"Text {textIndex} on page {pageIndex}", 50 + textIndex * 10, 50 + textIndex * 15);
                    }
                }
            }
            stopwatch.Stop();

            // Assert
            var totalElements = 10 * pageCount * textElementsPerPage;
            var averageTimePerElement = stopwatch.ElapsedMilliseconds / (double)totalElements;
            Console.WriteLine($"Large Document Test:");
            Console.WriteLine($"  Total elements created: {totalElements}");
            Console.WriteLine($"  Total time: {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"  Average time per element: {averageTimePerElement:F4}ms");

            // Should handle large documents efficiently
            Assert.Less(averageTimePerElement, 0.1, "Should create elements efficiently even in large documents");

            // Cleanup
            foreach (var doc in documents)
            {
                _memoryFactory.ReturnDocument(doc);
                doc?.Dispose();
            }
        }

        [Test]
        public void ProviderComparison_Optimized_PerformanceTest()
        {
            // Arrange
            const int iterations = 50;
            var questPdfFactory = new PdfMemoryEfficientFactory(_questPdfProvider, _logger, _validator);
            var iTextSharpFactory = new PdfMemoryEfficientFactory(_iTextSharpProvider, _logger, _validator);
            var questPdfTimes = new long[iterations];
            var iTextSharpTimes = new long[iterations];

            try
            {
                // Act - Test QuestPDF
                for (int i = 0; i < iterations; i++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    var result = questPdfFactory.CreateDocument();
                    stopwatch.Stop();
                    
                    Assert.IsTrue(result.IsSuccess);
                    questPdfTimes[i] = stopwatch.ElapsedMilliseconds;
                    questPdfFactory.ReturnDocument(result.Value);
                    result.Value?.Dispose();
                }

                // Act - Test iTextSharp
                for (int i = 0; i < iterations; i++)
                {
                    var stopwatch = Stopwatch.StartNew();
                    var result = iTextSharpFactory.CreateDocument();
                    stopwatch.Stop();
                    
                    Assert.IsTrue(result.IsSuccess);
                    iTextSharpTimes[i] = stopwatch.ElapsedMilliseconds;
                    iTextSharpFactory.ReturnDocument(result.Value);
                    result.Value?.Dispose();
                }

                // Assert
                var questPdfAverage = questPdfTimes.Average();
                var iTextSharpAverage = iTextSharpTimes.Average();
                
                Console.WriteLine($"Optimized Provider Comparison:");
                Console.WriteLine($"  QuestPDF Average: {questPdfAverage:F2}ms");
                Console.WriteLine($"  iTextSharp Average: {iTextSharpAverage:F2}ms");
                Console.WriteLine($"  QuestPDF Pool Stats: {questPdfFactory.GetMemoryStats().DocumentPoolSize} documents");
                Console.WriteLine($"  iTextSharp Pool Stats: {iTextSharpFactory.GetMemoryStats().DocumentPoolSize} documents");

                // Both should be fast with optimization
                Assert.Less(questPdfAverage, 10.0, "QuestPDF should be fast with optimization");
                Assert.Less(iTextSharpAverage, 10.0, "iTextSharp should be fast with optimization");
            }
            finally
            {
                questPdfFactory.Dispose();
                iTextSharpFactory.Dispose();
            }
        }
    }
}
