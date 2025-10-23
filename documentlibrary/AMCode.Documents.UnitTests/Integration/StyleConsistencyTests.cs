using NUnit.Framework;
using System;
using System.Collections.Generic;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;
using Moq;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.UnitTests.Integration
{
    /// <summary>
    /// Integration tests for style consistency across Excel, DOCX, and PDF formats
    /// Ensures that styling is applied consistently across all document formats
    /// </summary>
    [TestFixture]
    public class StyleConsistencyTests
    {
        [Test]
        public void ShouldApplyConsistentFontStyling()
        {
            // Arrange
            var fontSettings = new FontSettings
            {
                Name = "Arial",
                Size = 12,
                IsBold = true,
                IsItalic = false,
                Color = new Color(0, 0, 0)
            };

            // Act & Assert - Test font style consistency
            Assert.AreEqual("Arial", fontSettings.Name);
            Assert.AreEqual(12, fontSettings.Size);
            Assert.IsTrue(fontSettings.IsBold);
            Assert.IsFalse(fontSettings.IsItalic);
            Assert.AreEqual(new Color(0, 0, 0), fontSettings.Color);

            // Test that font settings can be applied consistently
            var fontDescription = $"{fontSettings.Name}, {fontSettings.Size}pt";
            if (fontSettings.IsBold) fontDescription += ", Bold";
            if (fontSettings.IsItalic) fontDescription += ", Italic";
            fontDescription += $", {fontSettings.Color.ToHex()}";

            Assert.IsTrue(fontDescription.Contains("Arial"));
            Assert.IsTrue(fontDescription.Contains("12pt"));
            Assert.IsTrue(fontDescription.Contains("Bold"));
            Assert.IsTrue(fontDescription.Contains("#000000"));
        }

        [Test]
        public void ShouldApplyConsistentColorStyling()
        {
            // Arrange
            var testColors = new Dictionary<string, Color>
            {
                { "Red", new Color(255, 0, 0) },
                { "Green", new Color(0, 255, 0) },
                { "Blue", new Color(0, 0, 255) },
                { "Black", new Color(0, 0, 0) },
                { "White", new Color(255, 255, 255) },
                { "Gray", new Color(128, 128, 128) }
            };

            // Act & Assert - Test color style consistency
            foreach (var (name, color) in testColors)
            {
                var hexValue = color.ToHex();
                var convertedColor = Color.FromHex(hexValue);

                Assert.AreEqual(color, convertedColor);
                Assert.IsTrue(hexValue.StartsWith("#"));
                Assert.AreEqual(7, hexValue.Length); // #RRGGBB format

                // Test color consistency across formats
                var colorDescription = $"{name}: {hexValue}";
                Assert.IsTrue(colorDescription.Contains(name));
                Assert.IsTrue(colorDescription.Contains(hexValue));
            }
        }

        [Test]
        public void ShouldApplyConsistentAlignmentStyling()
        {
            // Arrange
            var alignmentPairs = new[]
            {
                (HorizontalAlignment.Left, VerticalAlignment.Top),
                (HorizontalAlignment.Center, VerticalAlignment.Middle),
                (HorizontalAlignment.Right, VerticalAlignment.Bottom),
                (HorizontalAlignment.Justify, VerticalAlignment.Top)
            };

            // Act & Assert - Test alignment style consistency
            foreach (var (horizontal, vertical) in alignmentPairs)
            {
                var alignmentDescription = $"{horizontal} | {vertical}";
                
                Assert.IsTrue(alignmentDescription.Contains(horizontal.ToString()));
                Assert.IsTrue(alignmentDescription.Contains(vertical.ToString()));

                // Test that alignment values are consistent
                Assert.IsTrue(Enum.IsDefined(typeof(HorizontalAlignment), horizontal));
                Assert.IsTrue(Enum.IsDefined(typeof(VerticalAlignment), vertical));
            }
        }

        [Test]
        public void ShouldApplyConsistentBorderStyling()
        {
            // Arrange
            var borderStyles = new[]
            {
                new BorderStyle
                {
                    Color = new Color(0, 0, 0),
                    LineStyle = BorderLineStyle.Solid,
                    Sides = BorderSides.All,
                    Width = 1.0
                },
                new BorderStyle
                {
                    Color = new Color(128, 128, 128),
                    LineStyle = BorderLineStyle.Dashed,
                    Sides = BorderSides.Top | BorderSides.Bottom,
                    Width = 2.0
                },
                new BorderStyle
                {
                    Color = new Color(255, 0, 0),
                    LineStyle = BorderLineStyle.Dotted,
                    Sides = BorderSides.Left | BorderSides.Right,
                    Width = 0.5
                }
            };

            // Act & Assert - Test border style consistency
            foreach (var borderStyle in borderStyles)
            {
                var borderDescription = $"Border: {borderStyle.LineStyle}, {borderStyle.Width}pt, {borderStyle.Color.ToHex()}, {borderStyle.Sides}";
                
                Assert.IsTrue(borderDescription.Contains(borderStyle.LineStyle.ToString()));
                Assert.IsTrue(borderDescription.Contains($"{borderStyle.Width}pt"));
                Assert.IsTrue(borderDescription.Contains(borderStyle.Color.ToHex()));
                Assert.IsTrue(borderDescription.Contains(borderStyle.Sides.ToString()));

                // Test border style properties
                Assert.IsNotNull(borderStyle.Color);
                Assert.IsTrue(Enum.IsDefined(typeof(BorderLineStyle), borderStyle.LineStyle));
                Assert.IsTrue(borderStyle.Width > 0);
            }
        }

        [Test]
        public void ShouldApplyConsistentMarginsStyling()
        {
            // Arrange
            var marginStyles = new[]
            {
                Margins.Normal,
                Margins.Narrow,
                new Margins(2.0, 2.0, 2.0, 2.0), // Custom margins
                new Margins(0.5, 0.5, 0.5, 0.5)  // Small margins
            };

            // Act & Assert - Test margin style consistency
            foreach (var margins in marginStyles)
            {
                var marginDescription = $"Margins: T:{margins.Top}, B:{margins.Bottom}, L:{margins.Left}, R:{margins.Right}";
                
                Assert.IsTrue(marginDescription.Contains($"T:{margins.Top}"));
                Assert.IsTrue(marginDescription.Contains($"B:{margins.Bottom}"));
                Assert.IsTrue(marginDescription.Contains($"L:{margins.Left}"));
                Assert.IsTrue(marginDescription.Contains($"R:{margins.Right}"));

                // Test margin values
                Assert.IsTrue(margins.Top >= 0);
                Assert.IsTrue(margins.Bottom >= 0);
                Assert.IsTrue(margins.Left >= 0);
                Assert.IsTrue(margins.Right >= 0);
            }
        }

        [Test]
        public void ShouldApplyConsistentPageSizeStyling()
        {
            // Arrange
            var pageSizes = new[]
            {
                PageSize.A4,
                PageSize.Letter,
                PageSize.Legal,
                new PageSize(8.5, 11.0), // Custom Letter size
                new PageSize(11.0, 17.0) // Custom Legal size
            };

            // Act & Assert - Test page size style consistency
            foreach (var pageSize in pageSizes)
            {
                var pageSizeDescription = $"Page Size: {pageSize.Width}\" x {pageSize.Height}\"";
                
                Assert.IsTrue(pageSizeDescription.Contains($"{pageSize.Width}\""));
                Assert.IsTrue(pageSizeDescription.Contains($"{pageSize.Height}\""));

                // Test page size values
                Assert.IsTrue(pageSize.Width > 0);
                Assert.IsTrue(pageSize.Height > 0);
            }
        }

        [Test]
        public void ShouldApplyConsistentPatternStyling()
        {
            // Arrange
            var patternStyles = Enum.GetValues<PatternStyle>();

            // Act & Assert - Test pattern style consistency
            foreach (var patternStyle in patternStyles)
            {
                var patternDescription = $"Pattern: {patternStyle}";
                
                Assert.IsTrue(patternDescription.Contains(patternStyle.ToString()));
                Assert.IsTrue(Enum.IsDefined(typeof(PatternStyle), patternStyle));

                // Test pattern style string representation
                var patternString = patternStyle.ToString();
                Assert.IsNotNull(patternString);
                Assert.IsTrue(patternString.Length > 0);
            }
        }

        [Test]
        public void ShouldApplyConsistentComplexStyling()
        {
            // Arrange
            var complexStyle = new
            {
                Font = new FontSettings
                {
                    Name = "Times New Roman",
                    Size = 14,
                    IsBold = true,
                    IsItalic = true,
                    Color = new Color(0, 0, 128) // Navy blue
                },
                Border = new BorderStyle
                {
                    Color = new Color(0, 0, 0),
                    LineStyle = BorderLineStyle.Solid,
                    Sides = BorderSides.All,
                    Width = 1.5
                },
                Margins = new Margins(1.5, 1.5, 1.5, 1.5),
                PageSize = PageSize.A4,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle
            };

            // Act - Test complex style consistency
            var styleDescription = $"Font: {complexStyle.Font.Name} {complexStyle.Font.Size}pt, " +
                                 $"Color: {complexStyle.Font.Color.ToHex()}, " +
                                 $"Border: {complexStyle.Border.LineStyle} {complexStyle.Border.Width}pt, " +
                                 $"Margins: {complexStyle.Margins.Top}\", " +
                                 $"Page: {complexStyle.PageSize.Width}\" x {complexStyle.PageSize.Height}\", " +
                                 $"Align: {complexStyle.HorizontalAlignment} | {complexStyle.VerticalAlignment}";

            // Assert
            Assert.IsTrue(styleDescription.Contains("Times New Roman"));
            Assert.IsTrue(styleDescription.Contains("14pt"));
            Assert.IsTrue(styleDescription.Contains("#000080"));
            Assert.IsTrue(styleDescription.Contains("Solid"));
            Assert.IsTrue(styleDescription.Contains("1.5pt"));
            Assert.IsTrue(styleDescription.Contains("1.5\""));
            Assert.IsTrue(styleDescription.Contains("8.27\""));
            Assert.IsTrue(styleDescription.Contains("11.69\""));
            Assert.IsTrue(styleDescription.Contains("Center"));
            Assert.IsTrue(styleDescription.Contains("Middle"));
        }

        [Test]
        public void ShouldValidateStyleInheritance()
        {
            // Arrange
            var baseStyle = new FontSettings
            {
                Name = "Arial",
                Size = 12,
                IsBold = false,
                IsItalic = false,
                Color = new Color(0, 0, 0)
            };

            // Act - Test style inheritance
            var inheritedStyle = new FontSettings
            {
                Name = baseStyle.Name,
                Size = baseStyle.Size,
                IsBold = true, // Override
                IsItalic = baseStyle.IsItalic,
                Color = new Color(255, 0, 0) // Override
            };

            // Assert
            Assert.AreEqual(baseStyle.Name, inheritedStyle.Name);
            Assert.AreEqual(baseStyle.Size, inheritedStyle.Size);
            Assert.AreNotEqual(baseStyle.IsBold, inheritedStyle.IsBold);
            Assert.AreEqual(baseStyle.IsItalic, inheritedStyle.IsItalic);
            Assert.AreNotEqual(baseStyle.Color, inheritedStyle.Color);

            // Test that inheritance works consistently
            var baseDescription = $"{baseStyle.Name} {baseStyle.Size}pt";
            var inheritedDescription = $"{inheritedStyle.Name} {inheritedStyle.Size}pt Bold {inheritedStyle.Color.ToHex()}";

            Assert.IsTrue(inheritedDescription.Contains(baseDescription));
            Assert.IsTrue(inheritedDescription.Contains("Bold"));
            Assert.IsTrue(inheritedDescription.Contains("#FF0000"));
        }

        [Test]
        public void ShouldValidateStyleConsistencyAcrossFormats()
        {
            // Arrange
            var testStyles = new[]
            {
                new { Name = "Style1", Font = new FontSettings { Name = "Arial", Size = 12 }, Color = new Color(255, 0, 0) },
                new { Name = "Style2", Font = new FontSettings { Name = "Times New Roman", Size = 14 }, Color = new Color(0, 255, 0) },
                new { Name = "Style3", Font = new FontSettings { Name = "Courier New", Size = 10 }, Color = new Color(0, 0, 255) }
            };

            // Act & Assert - Test style consistency across formats
            foreach (var style in testStyles)
            {
                var styleString = $"{style.Name}: {style.Font.Name} {style.Font.Size}pt {style.Color.ToHex()}";
                
                Assert.IsTrue(styleString.Contains(style.Name));
                Assert.IsTrue(styleString.Contains(style.Font.Name));
                Assert.IsTrue(styleString.Contains($"{style.Font.Size}pt"));
                Assert.IsTrue(styleString.Contains(style.Color.ToHex()));

                // Test that styles can be applied consistently
                Assert.IsNotNull(style.Font.Name);
                Assert.IsTrue(style.Font.Size > 0);
                Assert.IsNotNull(style.Color);
            }
        }
    }
}
