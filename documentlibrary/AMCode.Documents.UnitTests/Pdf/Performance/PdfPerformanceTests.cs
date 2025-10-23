using NUnit.Framework;
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AMCode.Documents.UnitTests.Pdf.Performance
{
    [TestFixture]
    public class PdfPerformanceTests
    {
        private IPdfProvider _questPdfProvider;
        private IPdfProvider _iTextSharpProvider;
        private IPdfLogger _logger;
        private IPdfValidator _validator;

        [SetUp]
        public void Setup()
        {
            _logger = new TestPdfLogger();
            _validator = new PdfValidator();
            _questPdfProvider = new QuestPdfProvider(_logger, _validator);
            _iTextSharpProvider = new iTextSharpProvider(_logger, _validator);
        }

        [Test]
        public void QuestPdfProvider_CreateDocument_PerformanceTest()
        {
            // Arrange
            const int iterations = 100;
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                var result = _questPdfProvider.CreateDocument();
                Assert.IsTrue(result.IsSuccess);
                result.Value?.Dispose();
            }
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Console.WriteLine($"QuestPDF: Average document creation time: {averageTime:F2}ms for {iterations} iterations");
            
            // Performance assertion: should create documents in reasonable time
            Assert.Less(averageTime, 100, "Document creation should be fast");
        }

        [Test]
        public void iTextSharpProvider_CreateDocument_PerformanceTest()
        {
            // Arrange
            const int iterations = 100;
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                var result = _iTextSharpProvider.CreateDocument();
                Assert.IsTrue(result.IsSuccess);
                result.Value?.Dispose();
            }
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Console.WriteLine($"iTextSharp: Average document creation time: {averageTime:F2}ms for {iterations} iterations");
            
            // Performance assertion: should create documents in reasonable time
            Assert.Less(averageTime, 100, "Document creation should be fast");
        }

        [Test]
        public void QuestPdfProvider_AddMultiplePages_PerformanceTest()
        {
            // Arrange
            const int pageCount = 50;
            var stopwatch = new Stopwatch();

            // Act
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;

            stopwatch.Start();
            for (int i = 0; i < pageCount; i++)
            {
                var page = document.Pages.Create();
                page.AddText($"Page {i + 1} content", 50, 50);
                page.AddRectangle(10, 10, 100, 50, Color.LightBlue);
            }
            stopwatch.Stop();

            // Assert
            Console.WriteLine($"QuestPDF: Added {pageCount} pages in {stopwatch.ElapsedMilliseconds}ms");
            Assert.AreEqual(pageCount, document.Pages.Count);
            
            // Performance assertion: should add pages efficiently
            var averageTimePerPage = stopwatch.ElapsedMilliseconds / (double)pageCount;
            Assert.Less(averageTimePerPage, 10, "Page addition should be efficient");
        }

        [Test]
        public void iTextSharpProvider_AddMultiplePages_PerformanceTest()
        {
            // Arrange
            const int pageCount = 50;
            var stopwatch = new Stopwatch();

            // Act
            var result = _iTextSharpProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;

            stopwatch.Start();
            for (int i = 0; i < pageCount; i++)
            {
                var page = document.Pages.Create();
                page.AddText($"Page {i + 1} content", 50, 50);
                page.AddRectangle(10, 10, 100, 50, Color.LightBlue);
            }
            stopwatch.Stop();

            // Assert
            Console.WriteLine($"iTextSharp: Added {pageCount} pages in {stopwatch.ElapsedMilliseconds}ms");
            Assert.AreEqual(pageCount, document.Pages.Count);
            
            // Performance assertion: should add pages efficiently
            var averageTimePerPage = stopwatch.ElapsedMilliseconds / (double)pageCount;
            Assert.Less(averageTimePerPage, 10, "Page addition should be efficient");
        }

        [Test]
        public void QuestPdfProvider_AddLargeTable_PerformanceTest()
        {
            // Arrange
            const int rows = 100;
            const int columns = 10;
            var stopwatch = new Stopwatch();

            // Act
            var result = _questPdfProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create();

            stopwatch.Start();
            var table = page.AddTable(50, 50, rows, columns);
            
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    table.SetCellValue(row, col, $"Cell {row},{col}");
                }
            }
            stopwatch.Stop();

            // Assert
            Console.WriteLine($"QuestPDF: Created {rows}x{columns} table in {stopwatch.ElapsedMilliseconds}ms");
            
            // Performance assertion: should create large tables efficiently
            Assert.Less(stopwatch.ElapsedMilliseconds, 1000, "Large table creation should be efficient");
        }

        [Test]
        public void iTextSharpProvider_AddLargeTable_PerformanceTest()
        {
            // Arrange
            const int rows = 100;
            const int columns = 10;
            var stopwatch = new Stopwatch();

            // Act
            var result = _iTextSharpProvider.CreateDocument();
            Assert.IsTrue(result.IsSuccess);
            var document = result.Value;
            var page = document.Pages.Create();

            stopwatch.Start();
            var table = page.AddTable(50, 50, rows, columns);
            
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    table.SetCellValue(row, col, $"Cell {row},{col}");
                }
            }
            stopwatch.Stop();

            // Assert
            Console.WriteLine($"iTextSharp: Created {rows}x{columns} table in {stopwatch.ElapsedMilliseconds}ms");
            
            // Performance assertion: should create large tables efficiently
            Assert.Less(stopwatch.ElapsedMilliseconds, 1000, "Large table creation should be efficient");
        }

        [Test]
        public void QuestPdfProvider_MemoryUsage_PerformanceTest()
        {
            // Arrange
            const int iterations = 100;
            var initialMemory = GC.GetTotalMemory(true);

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var result = _questPdfProvider.CreateDocument();
                Assert.IsTrue(result.IsSuccess);
                
                var page = result.Value.Pages.Create();
                page.AddText($"Document {i} content", 50, 50);
                page.AddRectangle(10, 10, 100, 50, Color.LightBlue);
                
                result.Value.Dispose();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var finalMemory = GC.GetTotalMemory(false);
            var memoryIncrease = finalMemory - initialMemory;

            // Assert
            Console.WriteLine($"QuestPDF: Memory increase after {iterations} documents: {memoryIncrease / 1024.0:F2}KB");
            
            // Performance assertion: should not leak significant memory
            Assert.Less(memoryIncrease, 10 * 1024 * 1024, "Should not leak more than 10MB of memory");
        }

        [Test]
        public void iTextSharpProvider_MemoryUsage_PerformanceTest()
        {
            // Arrange
            const int iterations = 100;
            var initialMemory = GC.GetTotalMemory(true);

            // Act
            for (int i = 0; i < iterations; i++)
            {
                var result = _iTextSharpProvider.CreateDocument();
                Assert.IsTrue(result.IsSuccess);
                
                var page = result.Value.Pages.Create();
                page.AddText($"Document {i} content", 50, 50);
                page.AddRectangle(10, 10, 100, 50, Color.LightBlue);
                
                result.Value.Dispose();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var finalMemory = GC.GetTotalMemory(false);
            var memoryIncrease = finalMemory - initialMemory;

            // Assert
            Console.WriteLine($"iTextSharp: Memory increase after {iterations} documents: {memoryIncrease / 1024.0:F2}KB");
            
            // Performance assertion: should not leak significant memory
            Assert.Less(memoryIncrease, 10 * 1024 * 1024, "Should not leak more than 10MB of memory");
        }

        [Test]
        public void PdfFactory_ProviderSwitching_PerformanceTest()
        {
            // Arrange
            PdfFactory.RegisterProvider("QuestPDF", _questPdfProvider);
            PdfFactory.RegisterProvider("iTextSharp", _iTextSharpProvider);
            const int iterations = 50;
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                var questResult = PdfFactory.CreateDocument("QuestPDF");
                Assert.IsTrue(questResult.IsSuccess);
                questResult.Value?.Dispose();

                var iTextResult = PdfFactory.CreateDocument("iTextSharp");
                Assert.IsTrue(iTextResult.IsSuccess);
                iTextResult.Value?.Dispose();
            }
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)(iterations * 2);
            Console.WriteLine($"Provider switching: Average time per document: {averageTime:F2}ms");
            
            // Performance assertion: provider switching should be efficient
            Assert.Less(averageTime, 50, "Provider switching should be efficient");
        }

        [Test]
        public void PdfBuilder_FluentAPI_PerformanceTest()
        {
            // Arrange
            PdfFactory.SetDefaultProvider(_questPdfProvider);
            const int iterations = 100;
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                var builder = PdfFactory.CreateBuilder();
                var result = builder
                    .WithTitle($"Document {i}")
                    .WithAuthor("Performance Test")
                    .WithPage(page =>
                    {
                        page.AddText($"Content {i}", 100, 100);
                        page.AddRectangle(50, 50, 200, 100, Color.LightBlue);
                    })
                    .Build();
                
                Assert.IsTrue(result.IsSuccess);
                result.Value?.Dispose();
            }
            stopwatch.Stop();

            // Assert
            var averageTime = stopwatch.ElapsedMilliseconds / (double)iterations;
            Console.WriteLine($"Fluent API: Average time per document: {averageTime:F2}ms");
            
            // Performance assertion: fluent API should be efficient
            Assert.Less(averageTime, 50, "Fluent API should be efficient");
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up any resources if needed
        }
    }
}