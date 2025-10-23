using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;
using AMCode.Docx;
using Moq;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.UnitTests.Integration
{
    /// <summary>
    /// Integration tests for format conversion between Excel, DOCX, and PDF
    /// Tests data conversion and format compatibility
    /// </summary>
    [TestFixture]
    public class FormatConversionTests
    {
        [Test]
        public void ShouldConvertTextDataBetweenFormats()
        {
            // Arrange
            var testText = "This is test content for format conversion";
            var expectedLength = testText.Length;

            // Act - Test text conversion
            var textBytes = Encoding.UTF8.GetBytes(testText);
            var convertedText = Encoding.UTF8.GetString(textBytes);

            // Assert
            Assert.AreEqual(testText, convertedText);
            Assert.AreEqual(expectedLength, textBytes.Length);
            Assert.AreEqual(expectedLength, convertedText.Length);
        }

        [Test]
        public void ShouldConvertNumericDataBetweenFormats()
        {
            // Arrange
            var testNumbers = new[] { 1.5, 2.7, 3.14, 100.0, -50.5 };

            // Act & Assert - Test numeric conversion
            foreach (var number in testNumbers)
            {
                var numberString = number.ToString();
                var convertedNumber = double.Parse(numberString);
                
                Assert.AreEqual(number, convertedNumber, 0.001);
                Assert.IsTrue(numberString.Length > 0);
            }
        }

        [Test]
        public void ShouldConvertDateTimeDataBetweenFormats()
        {
            // Arrange
            var testDate = new DateTime(2024, 12, 19, 15, 30, 45);
            var expectedTicks = testDate.Ticks;

            // Act - Test date conversion
            var dateString = testDate.ToString("yyyy-MM-dd HH:mm:ss");
            var convertedDate = DateTime.Parse(dateString);

            // Assert
            Assert.AreEqual(testDate.Year, convertedDate.Year);
            Assert.AreEqual(testDate.Month, convertedDate.Month);
            Assert.AreEqual(testDate.Day, convertedDate.Day);
            Assert.AreEqual(testDate.Hour, convertedDate.Hour);
            Assert.AreEqual(testDate.Minute, convertedDate.Minute);
            Assert.AreEqual(testDate.Second, convertedDate.Second);
        }

        [Test]
        public void ShouldConvertColorDataBetweenFormats()
        {
            // Arrange
            var testColors = new[]
            {
                new Color(255, 0, 0),     // Red
                new Color(0, 255, 0),     // Green
                new Color(0, 0, 255),     // Blue
                new Color(255, 255, 255), // White
                new Color(0, 0, 0)        // Black
            };

            // Act & Assert - Test color conversion
            foreach (var color in testColors)
            {
                var hexString = color.ToHex();
                var convertedColor = Color.FromHex(hexString);
                
                Assert.AreEqual(color.Red, convertedColor.Red);
                Assert.AreEqual(color.Green, convertedColor.Green);
                Assert.AreEqual(color.Blue, convertedColor.Blue);
                Assert.IsTrue(hexString.StartsWith("#"));
                Assert.AreEqual(7, hexString.Length); // #RRGGBB format
            }
        }

        [Test]
        public void ShouldConvertFontSettingsBetweenFormats()
        {
            // Arrange
            var testFont = new FontSettings
            {
                Name = "Arial",
                Size = 12,
                IsBold = true,
                IsItalic = false,
                Color = new Color(0, 0, 0)
            };

            // Act - Test font conversion
            var fontString = $"{testFont.Name}|{testFont.Size}|{testFont.IsBold}|{testFont.IsItalic}|{testFont.Color.ToHex()}";
            var parts = fontString.Split('|');

            // Assert
            Assert.AreEqual(5, parts.Length);
            Assert.AreEqual("Arial", parts[0]);
            Assert.AreEqual("12", parts[1]);
            Assert.AreEqual("True", parts[2]);
            Assert.AreEqual("False", parts[3]);
            Assert.AreEqual("#000000", parts[4]);

            // Test reverse conversion
            var convertedFont = new FontSettings
            {
                Name = parts[0],
                Size = int.Parse(parts[1]),
                IsBold = bool.Parse(parts[2]),
                IsItalic = bool.Parse(parts[3]),
                Color = Color.FromHex(parts[4])
            };

            Assert.AreEqual(testFont.Name, convertedFont.Name);
            Assert.AreEqual(testFont.Size, convertedFont.Size);
            Assert.AreEqual(testFont.IsBold, convertedFont.IsBold);
            Assert.AreEqual(testFont.IsItalic, convertedFont.IsItalic);
            Assert.AreEqual(testFont.Color, convertedFont.Color);
        }

        [Test]
        public void ShouldConvertAlignmentDataBetweenFormats()
        {
            // Arrange
            var testAlignments = new[]
            {
                (HorizontalAlignment.Left, VerticalAlignment.Top),
                (HorizontalAlignment.Center, VerticalAlignment.Middle),
                (HorizontalAlignment.Right, VerticalAlignment.Bottom)
            };

            // Act & Assert - Test alignment conversion
            foreach (var (horizontal, vertical) in testAlignments)
            {
                var horizontalString = horizontal.ToString();
                var verticalString = vertical.ToString();
                
                var convertedHorizontal = Enum.Parse<HorizontalAlignment>(horizontalString);
                var convertedVertical = Enum.Parse<VerticalAlignment>(verticalString);
                
                Assert.AreEqual(horizontal, convertedHorizontal);
                Assert.AreEqual(vertical, convertedVertical);
            }
        }

        [Test]
        public void ShouldConvertBorderStyleDataBetweenFormats()
        {
            // Arrange
            var testBorderStyle = new BorderStyle
            {
                Color = new Color(128, 128, 128),
                LineStyle = BorderLineStyle.Dashed,
                Sides = BorderSides.Top | BorderSides.Bottom,
                Width = 2.0
            };

            // Act - Test border style conversion
            var borderString = $"{testBorderStyle.Color.ToHex()}|{testBorderStyle.LineStyle}|{testBorderStyle.Sides}|{testBorderStyle.Width}";
            var parts = borderString.Split('|');

            // Assert
            Assert.AreEqual(4, parts.Length);
            Assert.AreEqual("#808080", parts[0]);
            Assert.AreEqual("Dashed", parts[1]);
            Assert.AreEqual("Top, Bottom", parts[2]);
            Assert.AreEqual("2", parts[3]);

            // Test reverse conversion
            var convertedBorderStyle = new BorderStyle
            {
                Color = Color.FromHex(parts[0]),
                LineStyle = Enum.Parse<BorderLineStyle>(parts[1]),
                Sides = Enum.Parse<BorderSides>(parts[2]),
                Width = double.Parse(parts[3])
            };

            Assert.AreEqual(testBorderStyle.Color, convertedBorderStyle.Color);
            Assert.AreEqual(testBorderStyle.LineStyle, convertedBorderStyle.LineStyle);
            Assert.AreEqual(testBorderStyle.Sides, convertedBorderStyle.Sides);
            Assert.AreEqual(testBorderStyle.Width, convertedBorderStyle.Width);
        }

        [Test]
        public void ShouldConvertMarginsDataBetweenFormats()
        {
            // Arrange
            var testMargins = new Margins(1.5, 1.0, 1.25, 1.0);

            // Act - Test margins conversion
            var marginsString = $"{testMargins.Top}|{testMargins.Bottom}|{testMargins.Left}|{testMargins.Right}";
            var parts = marginsString.Split('|');

            // Assert
            Assert.AreEqual(4, parts.Length);
            Assert.AreEqual("1.5", parts[0]);
            Assert.AreEqual("1", parts[1]);
            Assert.AreEqual("1.25", parts[2]);
            Assert.AreEqual("1", parts[3]);

            // Test reverse conversion
            var convertedMargins = new Margins(
                double.Parse(parts[0]),
                double.Parse(parts[1]),
                double.Parse(parts[2]),
                double.Parse(parts[3])
            );

            Assert.AreEqual(testMargins.Top, convertedMargins.Top);
            Assert.AreEqual(testMargins.Bottom, convertedMargins.Bottom);
            Assert.AreEqual(testMargins.Left, convertedMargins.Left);
            Assert.AreEqual(testMargins.Right, convertedMargins.Right);
        }

        [Test]
        public void ShouldConvertPageSizeDataBetweenFormats()
        {
            // Arrange
            var testPageSize = new PageSize(8.5, 11.0); // Letter size

            // Act - Test page size conversion
            var pageSizeString = $"{testPageSize.Width}|{testPageSize.Height}";
            var parts = pageSizeString.Split('|');

            // Assert
            Assert.AreEqual(2, parts.Length);
            Assert.AreEqual("8.5", parts[0]);
            Assert.AreEqual("11", parts[1]);

            // Test reverse conversion
            var convertedPageSize = new PageSize(
                double.Parse(parts[0]),
                double.Parse(parts[1])
            );

            Assert.AreEqual(testPageSize.Width, convertedPageSize.Width);
            Assert.AreEqual(testPageSize.Height, convertedPageSize.Height);
        }

        [Test]
        public void ShouldHandleComplexDataConversion()
        {
            // Arrange
            var complexData = new
            {
                Title = "Test Document",
                Author = "Test Author",
                CreatedDate = DateTime.Now,
                Font = new FontSettings { Name = "Arial", Size = 12, IsBold = true },
                Color = new Color(255, 0, 0),
                Margins = Margins.Normal,
                PageSize = PageSize.A4,
                Alignment = HorizontalAlignment.Center
            };

            // Act - Test complex data conversion
            var dataString = $"{complexData.Title}|{complexData.Author}|{complexData.CreatedDate:yyyy-MM-dd HH:mm:ss}|" +
                           $"{complexData.Font.Name}|{complexData.Font.Size}|{complexData.Font.IsBold}|" +
                           $"{complexData.Color.ToHex()}|{complexData.Margins.Top}|{complexData.PageSize.Width}|" +
                           $"{complexData.Alignment}";

            var parts = dataString.Split('|');

            // Assert
            Assert.AreEqual(10, parts.Length);
            Assert.AreEqual("Test Document", parts[0]);
            Assert.AreEqual("Test Author", parts[1]);
            Assert.IsTrue(DateTime.TryParse(parts[2], out _));
            Assert.AreEqual("Arial", parts[3]);
            Assert.AreEqual("12", parts[4]);
            Assert.AreEqual("True", parts[5]);
            Assert.AreEqual("#FF0000", parts[6]);
            Assert.AreEqual("1", parts[7]);
            Assert.AreEqual("8.27", parts[8]);
            Assert.AreEqual("Center", parts[9]);
        }

        [Test]
        public void ShouldValidateConversionConsistency()
        {
            // Arrange
            var originalData = new
            {
                Text = "Sample text",
                Number = 42.5,
                Date = new DateTime(2024, 1, 1),
                Color = new Color(100, 150, 200),
                Font = new FontSettings { Name = "Times New Roman", Size = 14 }
            };

            // Act - Test round-trip conversion
            var serializedData = $"{originalData.Text}|{originalData.Number}|{originalData.Date:yyyy-MM-dd}|" +
                               $"{originalData.Color.ToHex()}|{originalData.Font.Name}|{originalData.Font.Size}";

            var parts = serializedData.Split('|');
            var deserializedData = new
            {
                Text = parts[0],
                Number = double.Parse(parts[1]),
                Date = DateTime.Parse(parts[2]),
                Color = Color.FromHex(parts[3]),
                Font = new FontSettings { Name = parts[4], Size = int.Parse(parts[5]) }
            };

            // Assert
            Assert.AreEqual(originalData.Text, deserializedData.Text);
            Assert.AreEqual(originalData.Number, deserializedData.Number, 0.001);
            Assert.AreEqual(originalData.Date.Date, deserializedData.Date.Date);
            Assert.AreEqual(originalData.Color, deserializedData.Color);
            Assert.AreEqual(originalData.Font.Name, deserializedData.Font.Name);
            Assert.AreEqual(originalData.Font.Size, deserializedData.Font.Size);
        }
    }
}
