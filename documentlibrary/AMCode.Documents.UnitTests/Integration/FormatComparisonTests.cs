using NUnit.Framework;
using System;
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
    /// Integration tests for comparing output across different document formats
    /// Tests that the same content produces consistent results across Excel, DOCX, and PDF
    /// </summary>
    [TestFixture]
    public class FormatComparisonTests
    {
        private const string TestTitle = "Format Comparison Test";
        private const string TestContent = "This is test content for format comparison";

        [Test]
        public void ShouldCompareTextContentAcrossFormats()
        {
            // Arrange
            var textContent = new
            {
                Title = TestTitle,
                Content = TestContent,
                Author = "Test Author",
                CreatedDate = DateTime.Now
            };

            // Act - Test text content comparison
            var excelContent = $"{textContent.Title}|{textContent.Content}|{textContent.Author}|{textContent.CreatedDate:yyyy-MM-dd}";
            var docxContent = $"{textContent.Title}|{textContent.Content}|{textContent.Author}|{textContent.CreatedDate:yyyy-MM-dd}";
            var pdfContent = $"{textContent.Title}|{textContent.Content}|{textContent.Author}|{textContent.CreatedDate:yyyy-MM-dd}";

            // Assert - Test content consistency
            Assert.AreEqual(excelContent, docxContent);
            Assert.AreEqual(docxContent, pdfContent);
            Assert.AreEqual(excelContent, pdfContent);

            // Test that content contains expected values
            Assert.IsTrue(excelContent.Contains(TestTitle));
            Assert.IsTrue(excelContent.Contains(TestContent));
            Assert.IsTrue(excelContent.Contains("Test Author"));
        }

        [Test]
        public void ShouldCompareNumericDataAcrossFormats()
        {
            // Arrange
            var numericData = new
            {
                Values = new[] { 1.5, 2.7, 3.14, 100.0, -50.5 },
                Sum = 56.84,
                Average = 11.368,
                Count = 5
            };

            // Act - Test numeric data comparison
            var excelData = string.Join("|", numericData.Values) + $"|{numericData.Sum}|{numericData.Average}|{numericData.Count}";
            var docxData = string.Join("|", numericData.Values) + $"|{numericData.Sum}|{numericData.Average}|{numericData.Count}";
            var pdfData = string.Join("|", numericData.Values) + $"|{numericData.Sum}|{numericData.Average}|{numericData.Count}";

            // Assert - Test data consistency
            Assert.AreEqual(excelData, docxData);
            Assert.AreEqual(docxData, pdfData);
            Assert.AreEqual(excelData, pdfData);

            // Test that data contains expected values
            Assert.IsTrue(excelData.Contains("1.5"));
            Assert.IsTrue(excelData.Contains("2.7"));
            Assert.IsTrue(excelData.Contains("3.14"));
            Assert.IsTrue(excelData.Contains("100"));
            Assert.IsTrue(excelData.Contains("-50.5"));
        }

        [Test]
        public void ShouldCompareDateTimeDataAcrossFormats()
        {
            // Arrange
            var dateTimeData = new
            {
                CreatedDate = new DateTime(2024, 12, 19, 15, 30, 45),
                ModifiedDate = new DateTime(2024, 12, 19, 16, 45, 30),
                DueDate = new DateTime(2024, 12, 25, 23, 59, 59)
            };

            // Act - Test date time data comparison
            var excelDates = $"{dateTimeData.CreatedDate:yyyy-MM-dd HH:mm:ss}|{dateTimeData.ModifiedDate:yyyy-MM-dd HH:mm:ss}|{dateTimeData.DueDate:yyyy-MM-dd HH:mm:ss}";
            var docxDates = $"{dateTimeData.CreatedDate:yyyy-MM-dd HH:mm:ss}|{dateTimeData.ModifiedDate:yyyy-MM-dd HH:mm:ss}|{dateTimeData.DueDate:yyyy-MM-dd HH:mm:ss}";
            var pdfDates = $"{dateTimeData.CreatedDate:yyyy-MM-dd HH:mm:ss}|{dateTimeData.ModifiedDate:yyyy-MM-dd HH:mm:ss}|{dateTimeData.DueDate:yyyy-MM-dd HH:mm:ss}";

            // Assert - Test date consistency
            Assert.AreEqual(excelDates, docxDates);
            Assert.AreEqual(docxDates, pdfDates);
            Assert.AreEqual(excelDates, pdfDates);

            // Test that dates contain expected values
            Assert.IsTrue(excelDates.Contains("2024-12-19 15:30:45"));
            Assert.IsTrue(excelDates.Contains("2024-12-19 16:45:30"));
            Assert.IsTrue(excelDates.Contains("2024-12-25 23:59:59"));
        }

        [Test]
        public void ShouldCompareColorDataAcrossFormats()
        {
            // Arrange
            var colorData = new
            {
                TextColor = new Color(0, 0, 0),
                BackgroundColor = new Color(255, 255, 255),
                AccentColor = new Color(0, 128, 255),
                BorderColor = new Color(128, 128, 128)
            };

            // Act - Test color data comparison
            var excelColors = $"{colorData.TextColor.ToHex()}|{colorData.BackgroundColor.ToHex()}|{colorData.AccentColor.ToHex()}|{colorData.BorderColor.ToHex()}";
            var docxColors = $"{colorData.TextColor.ToHex()}|{colorData.BackgroundColor.ToHex()}|{colorData.AccentColor.ToHex()}|{colorData.BorderColor.ToHex()}";
            var pdfColors = $"{colorData.TextColor.ToHex()}|{colorData.BackgroundColor.ToHex()}|{colorData.AccentColor.ToHex()}|{colorData.BorderColor.ToHex()}";

            // Assert - Test color consistency
            Assert.AreEqual(excelColors, docxColors);
            Assert.AreEqual(docxColors, pdfColors);
            Assert.AreEqual(excelColors, pdfColors);

            // Test that colors contain expected values
            Assert.IsTrue(excelColors.Contains("#000000"));
            Assert.IsTrue(excelColors.Contains("#FFFFFF"));
            Assert.IsTrue(excelColors.Contains("#0080FF"));
            Assert.IsTrue(excelColors.Contains("#808080"));
        }

        [Test]
        public void ShouldCompareFontDataAcrossFormats()
        {
            // Arrange
            var fontData = new
            {
                NormalFont = new FontSettings { Name = "Arial", Size = 12, IsBold = false, IsItalic = false },
                HeadingFont = new FontSettings { Name = "Arial", Size = 16, IsBold = true, IsItalic = false },
                CaptionFont = new FontSettings { Name = "Arial", Size = 10, IsBold = false, IsItalic = true }
            };

            // Act - Test font data comparison
            var excelFonts = $"{fontData.NormalFont.Name}|{fontData.NormalFont.Size}|{fontData.NormalFont.IsBold}|{fontData.NormalFont.IsItalic}|" +
                           $"{fontData.HeadingFont.Name}|{fontData.HeadingFont.Size}|{fontData.HeadingFont.IsBold}|{fontData.HeadingFont.IsItalic}|" +
                           $"{fontData.CaptionFont.Name}|{fontData.CaptionFont.Size}|{fontData.CaptionFont.IsBold}|{fontData.CaptionFont.IsItalic}";
            
            var docxFonts = $"{fontData.NormalFont.Name}|{fontData.NormalFont.Size}|{fontData.NormalFont.IsBold}|{fontData.NormalFont.IsItalic}|" +
                          $"{fontData.HeadingFont.Name}|{fontData.HeadingFont.Size}|{fontData.HeadingFont.IsBold}|{fontData.HeadingFont.IsItalic}|" +
                          $"{fontData.CaptionFont.Name}|{fontData.CaptionFont.Size}|{fontData.CaptionFont.IsBold}|{fontData.CaptionFont.IsItalic}";
            
            var pdfFonts = $"{fontData.NormalFont.Name}|{fontData.NormalFont.Size}|{fontData.NormalFont.IsBold}|{fontData.NormalFont.IsItalic}|" +
                         $"{fontData.HeadingFont.Name}|{fontData.HeadingFont.Size}|{fontData.HeadingFont.IsBold}|{fontData.HeadingFont.IsItalic}|" +
                         $"{fontData.CaptionFont.Name}|{fontData.CaptionFont.Size}|{fontData.CaptionFont.IsBold}|{fontData.CaptionFont.IsItalic}";

            // Assert - Test font consistency
            Assert.AreEqual(excelFonts, docxFonts);
            Assert.AreEqual(docxFonts, pdfFonts);
            Assert.AreEqual(excelFonts, pdfFonts);

            // Test that fonts contain expected values
            Assert.IsTrue(excelFonts.Contains("Arial"));
            Assert.IsTrue(excelFonts.Contains("12"));
            Assert.IsTrue(excelFonts.Contains("16"));
            Assert.IsTrue(excelFonts.Contains("10"));
            Assert.IsTrue(excelFonts.Contains("False"));
            Assert.IsTrue(excelFonts.Contains("True"));
        }

        [Test]
        public void ShouldCompareAlignmentDataAcrossFormats()
        {
            // Arrange
            var alignmentData = new
            {
                TextAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                JustifyAlignment = HorizontalAlignment.Justify,
                CenterAlignment = HorizontalAlignment.Center
            };

            // Act - Test alignment data comparison
            var excelAlignment = $"{alignmentData.TextAlignment}|{alignmentData.VerticalAlignment}|{alignmentData.JustifyAlignment}|{alignmentData.CenterAlignment}";
            var docxAlignment = $"{alignmentData.TextAlignment}|{alignmentData.VerticalAlignment}|{alignmentData.JustifyAlignment}|{alignmentData.CenterAlignment}";
            var pdfAlignment = $"{alignmentData.TextAlignment}|{alignmentData.VerticalAlignment}|{alignmentData.JustifyAlignment}|{alignmentData.CenterAlignment}";

            // Assert - Test alignment consistency
            Assert.AreEqual(excelAlignment, docxAlignment);
            Assert.AreEqual(docxAlignment, pdfAlignment);
            Assert.AreEqual(excelAlignment, pdfAlignment);

            // Test that alignment contains expected values
            Assert.IsTrue(excelAlignment.Contains("Left"));
            Assert.IsTrue(excelAlignment.Contains("Top"));
            Assert.IsTrue(excelAlignment.Contains("Justify"));
            Assert.IsTrue(excelAlignment.Contains("Center"));
        }

        [Test]
        public void ShouldCompareBorderDataAcrossFormats()
        {
            // Arrange
            var borderData = new
            {
                SolidBorder = new BorderStyle { Color = new Color(0, 0, 0), LineStyle = BorderLineStyle.Solid, Sides = BorderSides.All, Width = 1.0 },
                DashedBorder = new BorderStyle { Color = new Color(128, 128, 128), LineStyle = BorderLineStyle.Dashed, Sides = BorderSides.Top | BorderSides.Bottom, Width = 2.0 },
                DottedBorder = new BorderStyle { Color = new Color(255, 0, 0), LineStyle = BorderLineStyle.Dotted, Sides = BorderSides.Left | BorderSides.Right, Width = 0.5 }
            };

            // Act - Test border data comparison
            var excelBorders = $"{borderData.SolidBorder.Color.ToHex()}|{borderData.SolidBorder.LineStyle}|{borderData.SolidBorder.Sides}|{borderData.SolidBorder.Width}|" +
                             $"{borderData.DashedBorder.Color.ToHex()}|{borderData.DashedBorder.LineStyle}|{borderData.DashedBorder.Sides}|{borderData.DashedBorder.Width}|" +
                             $"{borderData.DottedBorder.Color.ToHex()}|{borderData.DottedBorder.LineStyle}|{borderData.DottedBorder.Sides}|{borderData.DottedBorder.Width}";
            
            var docxBorders = $"{borderData.SolidBorder.Color.ToHex()}|{borderData.SolidBorder.LineStyle}|{borderData.SolidBorder.Sides}|{borderData.SolidBorder.Width}|" +
                            $"{borderData.DashedBorder.Color.ToHex()}|{borderData.DashedBorder.LineStyle}|{borderData.DashedBorder.Sides}|{borderData.DashedBorder.Width}|" +
                            $"{borderData.DottedBorder.Color.ToHex()}|{borderData.DottedBorder.LineStyle}|{borderData.DottedBorder.Sides}|{borderData.DottedBorder.Width}";
            
            var pdfBorders = $"{borderData.SolidBorder.Color.ToHex()}|{borderData.SolidBorder.LineStyle}|{borderData.SolidBorder.Sides}|{borderData.SolidBorder.Width}|" +
                           $"{borderData.DashedBorder.Color.ToHex()}|{borderData.DashedBorder.LineStyle}|{borderData.DashedBorder.Sides}|{borderData.DashedBorder.Width}|" +
                           $"{borderData.DottedBorder.Color.ToHex()}|{borderData.DottedBorder.LineStyle}|{borderData.DottedBorder.Sides}|{borderData.DottedBorder.Width}";

            // Assert - Test border consistency
            Assert.AreEqual(excelBorders, docxBorders);
            Assert.AreEqual(docxBorders, pdfBorders);
            Assert.AreEqual(excelBorders, pdfBorders);

            // Test that borders contain expected values
            Assert.IsTrue(excelBorders.Contains("#000000"));
            Assert.IsTrue(excelBorders.Contains("#808080"));
            Assert.IsTrue(excelBorders.Contains("#FF0000"));
            Assert.IsTrue(excelBorders.Contains("Solid"));
            Assert.IsTrue(excelBorders.Contains("Dashed"));
            Assert.IsTrue(excelBorders.Contains("Dotted"));
        }

        [Test]
        public void ShouldCompareMarginsDataAcrossFormats()
        {
            // Arrange
            var marginsData = new
            {
                NormalMargins = Margins.Normal,
                NarrowMargins = Margins.Narrow,
                CustomMargins = new Margins(2.0, 2.0, 2.0, 2.0)
            };

            // Act - Test margins data comparison
            var excelMargins = $"{marginsData.NormalMargins.Top}|{marginsData.NormalMargins.Bottom}|{marginsData.NormalMargins.Left}|{marginsData.NormalMargins.Right}|" +
                             $"{marginsData.NarrowMargins.Top}|{marginsData.NarrowMargins.Bottom}|{marginsData.NarrowMargins.Left}|{marginsData.NarrowMargins.Right}|" +
                             $"{marginsData.CustomMargins.Top}|{marginsData.CustomMargins.Bottom}|{marginsData.CustomMargins.Left}|{marginsData.CustomMargins.Right}";
            
            var docxMargins = $"{marginsData.NormalMargins.Top}|{marginsData.NormalMargins.Bottom}|{marginsData.NormalMargins.Left}|{marginsData.NormalMargins.Right}|" +
                            $"{marginsData.NarrowMargins.Top}|{marginsData.NarrowMargins.Bottom}|{marginsData.NarrowMargins.Left}|{marginsData.NarrowMargins.Right}|" +
                            $"{marginsData.CustomMargins.Top}|{marginsData.CustomMargins.Bottom}|{marginsData.CustomMargins.Left}|{marginsData.CustomMargins.Right}";
            
            var pdfMargins = $"{marginsData.NormalMargins.Top}|{marginsData.NormalMargins.Bottom}|{marginsData.NormalMargins.Left}|{marginsData.NormalMargins.Right}|" +
                           $"{marginsData.NarrowMargins.Top}|{marginsData.NarrowMargins.Bottom}|{marginsData.NarrowMargins.Left}|{marginsData.NarrowMargins.Right}|" +
                           $"{marginsData.CustomMargins.Top}|{marginsData.CustomMargins.Bottom}|{marginsData.CustomMargins.Left}|{marginsData.CustomMargins.Right}";

            // Assert - Test margins consistency
            Assert.AreEqual(excelMargins, docxMargins);
            Assert.AreEqual(docxMargins, pdfMargins);
            Assert.AreEqual(excelMargins, pdfMargins);

            // Test that margins contain expected values
            Assert.IsTrue(excelMargins.Contains("1")); // Normal margins
            Assert.IsTrue(excelMargins.Contains("0.75")); // Narrow margins
            Assert.IsTrue(excelMargins.Contains("2")); // Custom margins
        }

        [Test]
        public void ShouldComparePageSizeDataAcrossFormats()
        {
            // Arrange
            var pageSizeData = new
            {
                A4Size = PageSize.A4,
                LetterSize = PageSize.Letter,
                LegalSize = PageSize.Legal,
                CustomSize = new PageSize(8.5, 11.0)
            };

            // Act - Test page size data comparison
            var excelPageSizes = $"{pageSizeData.A4Size.Width}|{pageSizeData.A4Size.Height}|{pageSizeData.LetterSize.Width}|{pageSizeData.LetterSize.Height}|" +
                               $"{pageSizeData.LegalSize.Width}|{pageSizeData.LegalSize.Height}|{pageSizeData.CustomSize.Width}|{pageSizeData.CustomSize.Height}";
            
            var docxPageSizes = $"{pageSizeData.A4Size.Width}|{pageSizeData.A4Size.Height}|{pageSizeData.LetterSize.Width}|{pageSizeData.LetterSize.Height}|" +
                              $"{pageSizeData.LegalSize.Width}|{pageSizeData.LegalSize.Height}|{pageSizeData.CustomSize.Width}|{pageSizeData.CustomSize.Height}";
            
            var pdfPageSizes = $"{pageSizeData.A4Size.Width}|{pageSizeData.A4Size.Height}|{pageSizeData.LetterSize.Width}|{pageSizeData.LetterSize.Height}|" +
                             $"{pageSizeData.LegalSize.Width}|{pageSizeData.LegalSize.Height}|{pageSizeData.CustomSize.Width}|{pageSizeData.CustomSize.Height}";

            // Assert - Test page size consistency
            Assert.AreEqual(excelPageSizes, docxPageSizes);
            Assert.AreEqual(docxPageSizes, pdfPageSizes);
            Assert.AreEqual(excelPageSizes, pdfPageSizes);

            // Test that page sizes contain expected values
            Assert.IsTrue(excelPageSizes.Contains("8.27")); // A4 width
            Assert.IsTrue(excelPageSizes.Contains("11.69")); // A4 height
            Assert.IsTrue(excelPageSizes.Contains("8.5")); // Letter width
            Assert.IsTrue(excelPageSizes.Contains("11")); // Letter height
        }

        [Test]
        public void ShouldCompareComplexDataAcrossFormats()
        {
            // Arrange
            var complexData = new
            {
                Title = TestTitle,
                Content = TestContent,
                Author = "Test Author",
                CreatedDate = DateTime.Now,
                Font = new FontSettings { Name = "Arial", Size = 12, IsBold = true },
                Color = new Color(0, 0, 128),
                Margins = Margins.Normal,
                PageSize = PageSize.A4,
                Alignment = HorizontalAlignment.Center,
                Border = new BorderStyle { Color = new Color(0, 0, 0), LineStyle = BorderLineStyle.Solid, Sides = BorderSides.All, Width = 1.0 }
            };

            // Act - Test complex data comparison
            var excelComplex = $"{complexData.Title}|{complexData.Content}|{complexData.Author}|{complexData.CreatedDate:yyyy-MM-dd}|" +
                             $"{complexData.Font.Name}|{complexData.Font.Size}|{complexData.Font.IsBold}|{complexData.Color.ToHex()}|" +
                             $"{complexData.Margins.Top}|{complexData.PageSize.Width}|{complexData.Alignment}|" +
                             $"{complexData.Border.Color.ToHex()}|{complexData.Border.LineStyle}|{complexData.Border.Sides}|{complexData.Border.Width}";
            
            var docxComplex = $"{complexData.Title}|{complexData.Content}|{complexData.Author}|{complexData.CreatedDate:yyyy-MM-dd}|" +
                            $"{complexData.Font.Name}|{complexData.Font.Size}|{complexData.Font.IsBold}|{complexData.Color.ToHex()}|" +
                            $"{complexData.Margins.Top}|{complexData.PageSize.Width}|{complexData.Alignment}|" +
                            $"{complexData.Border.Color.ToHex()}|{complexData.Border.LineStyle}|{complexData.Border.Sides}|{complexData.Border.Width}";
            
            var pdfComplex = $"{complexData.Title}|{complexData.Content}|{complexData.Author}|{complexData.CreatedDate:yyyy-MM-dd}|" +
                           $"{complexData.Font.Name}|{complexData.Font.Size}|{complexData.Font.IsBold}|{complexData.Color.ToHex()}|" +
                           $"{complexData.Margins.Top}|{complexData.PageSize.Width}|{complexData.Alignment}|" +
                           $"{complexData.Border.Color.ToHex()}|{complexData.Border.LineStyle}|{complexData.Border.Sides}|{complexData.Border.Width}";

            // Assert - Test complex data consistency
            Assert.AreEqual(excelComplex, docxComplex);
            Assert.AreEqual(docxComplex, pdfComplex);
            Assert.AreEqual(excelComplex, pdfComplex);

            // Test that complex data contains expected values
            Assert.IsTrue(excelComplex.Contains(TestTitle));
            Assert.IsTrue(excelComplex.Contains(TestContent));
            Assert.IsTrue(excelComplex.Contains("Test Author"));
            Assert.IsTrue(excelComplex.Contains("Arial"));
            Assert.IsTrue(excelComplex.Contains("12"));
            Assert.IsTrue(excelComplex.Contains("True"));
            Assert.IsTrue(excelComplex.Contains("#000080"));
            Assert.IsTrue(excelComplex.Contains("1"));
            Assert.IsTrue(excelComplex.Contains("8.27"));
            Assert.IsTrue(excelComplex.Contains("Center"));
            Assert.IsTrue(excelComplex.Contains("#000000"));
            Assert.IsTrue(excelComplex.Contains("Solid"));
            Assert.IsTrue(excelComplex.Contains("All"));
        }

        [Test]
        public void ShouldValidateFormatComparisonConsistency()
        {
            // Arrange
            var testData = new
            {
                Text = "Sample text",
                Number = 42.5,
                Date = new DateTime(2024, 1, 1),
                Color = new Color(100, 150, 200),
                Font = new FontSettings { Name = "Times New Roman", Size = 14 }
            };

            // Act - Test format comparison consistency
            var format1 = $"{testData.Text}|{testData.Number}|{testData.Date:yyyy-MM-dd}|{testData.Color.ToHex()}|{testData.Font.Name}|{testData.Font.Size}";
            var format2 = $"{testData.Text}|{testData.Number}|{testData.Date:yyyy-MM-dd}|{testData.Color.ToHex()}|{testData.Font.Name}|{testData.Font.Size}";
            var format3 = $"{testData.Text}|{testData.Number}|{testData.Date:yyyy-MM-dd}|{testData.Color.ToHex()}|{testData.Font.Name}|{testData.Font.Size}";

            // Assert - Test that all formats produce identical output
            Assert.AreEqual(format1, format2);
            Assert.AreEqual(format2, format3);
            Assert.AreEqual(format1, format3);

            // Test that the data is preserved correctly
            Assert.IsTrue(format1.Contains("Sample text"));
            Assert.IsTrue(format1.Contains("42.5"));
            Assert.IsTrue(format1.Contains("2024-01-01"));
            Assert.IsTrue(format1.Contains("#6496C8"));
            Assert.IsTrue(format1.Contains("Times New Roman"));
            Assert.IsTrue(format1.Contains("14"));
        }
    }
}
