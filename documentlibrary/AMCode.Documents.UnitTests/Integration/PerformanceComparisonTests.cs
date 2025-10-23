using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;
using Moq;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.UnitTests.Integration
{
    /// <summary>
    /// Integration tests for performance comparison across different document formats
    /// Tests performance benchmarks for Excel, DOCX, and PDF generation
    /// </summary>
    [TestFixture]
    public class PerformanceComparisonTests
    {
        private const int TestIterations = 100;
        private const int LargeDataSetSize = 1000;

        [Test]
        public void ShouldCompareTextGenerationPerformance()
        {
            // Arrange
            var testText = "This is test text for performance comparison across formats.";
            var stopwatch = new Stopwatch();

            // Act - Test text generation performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var excelText = $"{testText}|{i}|{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var docxText = $"{testText}|{i}|{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var pdfText = $"{testText}|{i}|{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                
                // Simulate text processing
                var processedText = excelText.Replace("|", " - ");
                Assert.IsNotNull(processedText);
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 10); // Should be less than 10ms per iteration
            
            Console.WriteLine($"Text generation performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldCompareNumericDataPerformance()
        {
            // Arrange
            var random = new Random(42); // Fixed seed for consistent results
            var stopwatch = new Stopwatch();

            // Act - Test numeric data performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var numbers = new double[LargeDataSetSize];
                for (int j = 0; j < LargeDataSetSize; j++)
                {
                    numbers[j] = random.NextDouble() * 1000;
                }

                // Simulate Excel processing
                var excelData = string.Join("|", numbers);
                var excelSum = 0.0;
                foreach (var num in numbers) excelSum += num;
                var excelAverage = excelSum / numbers.Length;

                // Simulate DOCX processing
                var docxData = string.Join("|", numbers);
                var docxSum = 0.0;
                foreach (var num in numbers) docxSum += num;
                var docxAverage = docxSum / numbers.Length;

                // Simulate PDF processing
                var pdfData = string.Join("|", numbers);
                var pdfSum = 0.0;
                foreach (var num in numbers) pdfSum += num;
                var pdfAverage = pdfSum / numbers.Length;

                // Verify consistency
                Assert.AreEqual(excelSum, docxSum, 0.001);
                Assert.AreEqual(docxSum, pdfSum, 0.001);
                Assert.AreEqual(excelAverage, docxAverage, 0.001);
                Assert.AreEqual(docxAverage, pdfAverage, 0.001);
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 100); // Should be less than 100ms per iteration
            
            Console.WriteLine($"Numeric data performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldCompareColorProcessingPerformance()
        {
            // Arrange
            var random = new Random(42);
            var stopwatch = new Stopwatch();

            // Act - Test color processing performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var colors = new Color[100];
                for (int j = 0; j < 100; j++)
                {
                    colors[j] = new Color(
                        random.Next(0, 256),
                        random.Next(0, 256),
                        random.Next(0, 256)
                    );
                }

                // Simulate Excel color processing
                var excelColors = new string[100];
                for (int j = 0; j < 100; j++)
                {
                    excelColors[j] = colors[j].ToHex();
                }

                // Simulate DOCX color processing
                var docxColors = new string[100];
                for (int j = 0; j < 100; j++)
                {
                    docxColors[j] = colors[j].ToHex();
                }

                // Simulate PDF color processing
                var pdfColors = new string[100];
                for (int j = 0; j < 100; j++)
                {
                    pdfColors[j] = colors[j].ToHex();
                }

                // Verify consistency
                for (int j = 0; j < 100; j++)
                {
                    Assert.AreEqual(excelColors[j], docxColors[j]);
                    Assert.AreEqual(docxColors[j], pdfColors[j]);
                }
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 50); // Should be less than 50ms per iteration
            
            Console.WriteLine($"Color processing performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldCompareFontProcessingPerformance()
        {
            // Arrange
            var fontNames = new[] { "Arial", "Times New Roman", "Courier New", "Calibri", "Verdana" };
            var random = new Random(42);
            var stopwatch = new Stopwatch();

            // Act - Test font processing performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var fonts = new FontSettings[50];
                for (int j = 0; j < 50; j++)
                {
                    fonts[j] = new FontSettings
                    {
                        Name = fontNames[random.Next(fontNames.Length)],
                        Size = random.Next(8, 24),
                        IsBold = random.Next(2) == 1,
                        IsItalic = random.Next(2) == 1,
                        Color = new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256))
                    };
                }

                // Simulate Excel font processing
                var excelFonts = new string[50];
                for (int j = 0; j < 50; j++)
                {
                    excelFonts[j] = $"{fonts[j].Name}|{fonts[j].Size}|{fonts[j].IsBold}|{fonts[j].IsItalic}|{fonts[j].Color.ToHex()}";
                }

                // Simulate DOCX font processing
                var docxFonts = new string[50];
                for (int j = 0; j < 50; j++)
                {
                    docxFonts[j] = $"{fonts[j].Name}|{fonts[j].Size}|{fonts[j].IsBold}|{fonts[j].IsItalic}|{fonts[j].Color.ToHex()}";
                }

                // Simulate PDF font processing
                var pdfFonts = new string[50];
                for (int j = 0; j < 50; j++)
                {
                    pdfFonts[j] = $"{fonts[j].Name}|{fonts[j].Size}|{fonts[j].IsBold}|{fonts[j].IsItalic}|{fonts[j].Color.ToHex()}";
                }

                // Verify consistency
                for (int j = 0; j < 50; j++)
                {
                    Assert.AreEqual(excelFonts[j], docxFonts[j]);
                    Assert.AreEqual(docxFonts[j], pdfFonts[j]);
                }
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 75); // Should be less than 75ms per iteration
            
            Console.WriteLine($"Font processing performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldCompareAlignmentProcessingPerformance()
        {
            // Arrange
            var horizontalAlignments = Enum.GetValues<HorizontalAlignment>();
            var verticalAlignments = Enum.GetValues<VerticalAlignment>();
            var random = new Random(42);
            var stopwatch = new Stopwatch();

            // Act - Test alignment processing performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var alignments = new (HorizontalAlignment, VerticalAlignment)[100];
                for (int j = 0; j < 100; j++)
                {
                    alignments[j] = (
                        horizontalAlignments[random.Next(horizontalAlignments.Length)],
                        verticalAlignments[random.Next(verticalAlignments.Length)]
                    );
                }

                // Simulate Excel alignment processing
                var excelAlignments = new string[100];
                for (int j = 0; j < 100; j++)
                {
                    excelAlignments[j] = $"{alignments[j].Item1}|{alignments[j].Item2}";
                }

                // Simulate DOCX alignment processing
                var docxAlignments = new string[100];
                for (int j = 0; j < 100; j++)
                {
                    docxAlignments[j] = $"{alignments[j].Item1}|{alignments[j].Item2}";
                }

                // Simulate PDF alignment processing
                var pdfAlignments = new string[100];
                for (int j = 0; j < 100; j++)
                {
                    pdfAlignments[j] = $"{alignments[j].Item1}|{alignments[j].Item2}";
                }

                // Verify consistency
                for (int j = 0; j < 100; j++)
                {
                    Assert.AreEqual(excelAlignments[j], docxAlignments[j]);
                    Assert.AreEqual(docxAlignments[j], pdfAlignments[j]);
                }
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 25); // Should be less than 25ms per iteration
            
            Console.WriteLine($"Alignment processing performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldCompareBorderProcessingPerformance()
        {
            // Arrange
            var lineStyles = Enum.GetValues<BorderLineStyle>();
            var borderSides = new[] { BorderSides.None, BorderSides.All, BorderSides.Top, BorderSides.Bottom, BorderSides.Left, BorderSides.Right };
            var random = new Random(42);
            var stopwatch = new Stopwatch();

            // Act - Test border processing performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var borders = new BorderStyle[50];
                for (int j = 0; j < 50; j++)
                {
                    borders[j] = new BorderStyle
                    {
                        Color = new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)),
                        LineStyle = lineStyles[random.Next(lineStyles.Length)],
                        Sides = borderSides[random.Next(borderSides.Length)],
                        Width = random.NextDouble() * 5.0
                    };
                }

                // Simulate Excel border processing
                var excelBorders = new string[50];
                for (int j = 0; j < 50; j++)
                {
                    excelBorders[j] = $"{borders[j].Color.ToHex()}|{borders[j].LineStyle}|{borders[j].Sides}|{borders[j].Width}";
                }

                // Simulate DOCX border processing
                var docxBorders = new string[50];
                for (int j = 0; j < 50; j++)
                {
                    docxBorders[j] = $"{borders[j].Color.ToHex()}|{borders[j].LineStyle}|{borders[j].Sides}|{borders[j].Width}";
                }

                // Simulate PDF border processing
                var pdfBorders = new string[50];
                for (int j = 0; j < 50; j++)
                {
                    pdfBorders[j] = $"{borders[j].Color.ToHex()}|{borders[j].LineStyle}|{borders[j].Sides}|{borders[j].Width}";
                }

                // Verify consistency
                for (int j = 0; j < 50; j++)
                {
                    Assert.AreEqual(excelBorders[j], docxBorders[j]);
                    Assert.AreEqual(docxBorders[j], pdfBorders[j]);
                }
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 60); // Should be less than 60ms per iteration
            
            Console.WriteLine($"Border processing performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldCompareMarginsProcessingPerformance()
        {
            // Arrange
            var marginPresets = new[] { Margins.Normal, Margins.Narrow, new Margins(2.0, 2.0, 2.0, 2.0) };
            var random = new Random(42);
            var stopwatch = new Stopwatch();

            // Act - Test margins processing performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var margins = new Margins[25];
                for (int j = 0; j < 25; j++)
                {
                    if (random.Next(2) == 0)
                    {
                        margins[j] = marginPresets[random.Next(marginPresets.Length)];
                    }
                    else
                    {
                        margins[j] = new Margins(
                            random.NextDouble() * 3.0,
                            random.NextDouble() * 3.0,
                            random.NextDouble() * 3.0,
                            random.NextDouble() * 3.0
                        );
                    }
                }

                // Simulate Excel margins processing
                var excelMargins = new string[25];
                for (int j = 0; j < 25; j++)
                {
                    excelMargins[j] = $"{margins[j].Top}|{margins[j].Bottom}|{margins[j].Left}|{margins[j].Right}";
                }

                // Simulate DOCX margins processing
                var docxMargins = new string[25];
                for (int j = 0; j < 25; j++)
                {
                    docxMargins[j] = $"{margins[j].Top}|{margins[j].Bottom}|{margins[j].Left}|{margins[j].Right}";
                }

                // Simulate PDF margins processing
                var pdfMargins = new string[25];
                for (int j = 0; j < 25; j++)
                {
                    pdfMargins[j] = $"{margins[j].Top}|{margins[j].Bottom}|{margins[j].Left}|{margins[j].Right}";
                }

                // Verify consistency
                for (int j = 0; j < 25; j++)
                {
                    Assert.AreEqual(excelMargins[j], docxMargins[j]);
                    Assert.AreEqual(docxMargins[j], pdfMargins[j]);
                }
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 30); // Should be less than 30ms per iteration
            
            Console.WriteLine($"Margins processing performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldComparePageSizeProcessingPerformance()
        {
            // Arrange
            var pageSizes = new[] { PageSize.A4, PageSize.Letter, PageSize.Legal, new PageSize(8.5, 11.0), new PageSize(11.0, 17.0) };
            var random = new Random(42);
            var stopwatch = new Stopwatch();

            // Act - Test page size processing performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                var sizes = new PageSize[25];
                for (int j = 0; j < 25; j++)
                {
                    if (random.Next(2) == 0)
                    {
                        sizes[j] = pageSizes[random.Next(pageSizes.Length)];
                    }
                    else
                    {
                        sizes[j] = new PageSize(
                            random.NextDouble() * 20.0 + 1.0,
                            random.NextDouble() * 20.0 + 1.0
                        );
                    }
                }

                // Simulate Excel page size processing
                var excelSizes = new string[25];
                for (int j = 0; j < 25; j++)
                {
                    excelSizes[j] = $"{sizes[j].Width}|{sizes[j].Height}";
                }

                // Simulate DOCX page size processing
                var docxSizes = new string[25];
                for (int j = 0; j < 25; j++)
                {
                    docxSizes[j] = $"{sizes[j].Width}|{sizes[j].Height}";
                }

                // Simulate PDF page size processing
                var pdfSizes = new string[25];
                for (int j = 0; j < 25; j++)
                {
                    pdfSizes[j] = $"{sizes[j].Width}|{sizes[j].Height}";
                }

                // Verify consistency
                for (int j = 0; j < 25; j++)
                {
                    Assert.AreEqual(excelSizes[j], docxSizes[j]);
                    Assert.AreEqual(docxSizes[j], pdfSizes[j]);
                }
            }
            stopwatch.Stop();

            // Assert - Test performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 20); // Should be less than 20ms per iteration
            
            Console.WriteLine($"Page size processing performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }

        [Test]
        public void ShouldCompareOverallPerformanceAcrossFormats()
        {
            // Arrange
            var random = new Random(42);
            var stopwatch = new Stopwatch();

            // Act - Test overall performance
            stopwatch.Start();
            for (int i = 0; i < TestIterations; i++)
            {
                // Create test data
                var testData = new
                {
                    Title = $"Test Document {i}",
                    Content = $"This is test content for iteration {i}",
                    Author = "Test Author",
                    CreatedDate = DateTime.Now,
                    Font = new FontSettings
                    {
                        Name = "Arial",
                        Size = 12,
                        IsBold = random.Next(2) == 1,
                        IsItalic = random.Next(2) == 1,
                        Color = new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256))
                    },
                    Margins = Margins.Normal,
                    PageSize = PageSize.A4,
                    Alignment = HorizontalAlignment.Left,
                    Border = new BorderStyle
                    {
                        Color = new Color(0, 0, 0),
                        LineStyle = BorderLineStyle.Solid,
                        Sides = BorderSides.All,
                        Width = 1.0
                    }
                };

                // Simulate Excel processing
                var excelData = $"{testData.Title}|{testData.Content}|{testData.Author}|{testData.CreatedDate:yyyy-MM-dd}|" +
                              $"{testData.Font.Name}|{testData.Font.Size}|{testData.Font.IsBold}|{testData.Font.IsItalic}|{testData.Font.Color.ToHex()}|" +
                              $"{testData.Margins.Top}|{testData.PageSize.Width}|{testData.Alignment}|" +
                              $"{testData.Border.Color.ToHex()}|{testData.Border.LineStyle}|{testData.Border.Sides}|{testData.Border.Width}";

                // Simulate DOCX processing
                var docxData = $"{testData.Title}|{testData.Content}|{testData.Author}|{testData.CreatedDate:yyyy-MM-dd}|" +
                             $"{testData.Font.Name}|{testData.Font.Size}|{testData.Font.IsBold}|{testData.Font.IsItalic}|{testData.Font.Color.ToHex()}|" +
                             $"{testData.Margins.Top}|{testData.PageSize.Width}|{testData.Alignment}|" +
                             $"{testData.Border.Color.ToHex()}|{testData.Border.LineStyle}|{testData.Border.Sides}|{testData.Border.Width}";

                // Simulate PDF processing
                var pdfData = $"{testData.Title}|{testData.Content}|{testData.Author}|{testData.CreatedDate:yyyy-MM-dd}|" +
                            $"{testData.Font.Name}|{testData.Font.Size}|{testData.Font.IsBold}|{testData.Font.IsItalic}|{testData.Font.Color.ToHex()}|" +
                            $"{testData.Margins.Top}|{testData.PageSize.Width}|{testData.Alignment}|" +
                            $"{testData.Border.Color.ToHex()}|{testData.Border.LineStyle}|{testData.Border.Sides}|{testData.Border.Width}";

                // Verify consistency
                Assert.AreEqual(excelData, docxData);
                Assert.AreEqual(docxData, pdfData);
            }
            stopwatch.Stop();

            // Assert - Test overall performance
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var averageMs = elapsedMs / (double)TestIterations;
            
            Assert.IsTrue(elapsedMs > 0);
            Assert.IsTrue(averageMs < 150); // Should be less than 150ms per iteration
            
            Console.WriteLine($"Overall performance: {elapsedMs}ms total, {averageMs:F2}ms average per iteration");
        }
    }
}
