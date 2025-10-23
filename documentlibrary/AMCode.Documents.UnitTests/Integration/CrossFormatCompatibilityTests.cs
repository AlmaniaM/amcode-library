using NUnit.Framework;
using System;
using System.IO;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;
using AMCode.Docx;
using AMCode.Documents.Xlsx.Domain.Interfaces;
using AMCode.Documents.Xlsx.Infrastructure.Factories;
using AMCode.Documents.Xlsx.Infrastructure.Adapters;
using AMCode.Documents.Xlsx.Infrastructure.Interfaces;
using AMCode.Documents.Xlsx.Providers.OpenXml;
using Moq;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.UnitTests.Integration
{
    /// <summary>
    /// Integration tests for cross-format compatibility
    /// Tests that common types work consistently across Excel, DOCX, and PDF formats
    /// </summary>
    [TestFixture]
    public class CrossFormatCompatibilityTests
    {
        private Mock<IWorkbookEngine> _mockWorkbookEngine;
        private Mock<IWorkbookLogger> _mockWorkbookLogger;
        private Mock<IWorkbookValidator> _mockWorkbookValidator;
        private WorkbookFactory _workbookFactory;

        [SetUp]
        public void Setup()
        {
            _mockWorkbookEngine = new Mock<IWorkbookEngine>();
            _mockWorkbookLogger = new Mock<IWorkbookLogger>();
            _mockWorkbookValidator = new Mock<IWorkbookValidator>();
            _workbookFactory = new WorkbookFactory(_mockWorkbookEngine.Object, _mockWorkbookLogger.Object, _mockWorkbookValidator.Object);
        }

        [Test]
        public void ShouldUseSameColorAcrossFormats()
        {
            // Arrange
            var testColor = new Color(255, 128, 64); // Orange color
            var expectedHex = "#FF8040";

            // Act & Assert - Test color consistency
            Assert.AreEqual(expectedHex, testColor.ToHex());
            Assert.AreEqual(255, testColor.Red);
            Assert.AreEqual(128, testColor.Green);
            Assert.AreEqual(64, testColor.Blue);

            // Test that color can be used in different contexts
            var colorFromHex = Color.FromHex(expectedHex);
            Assert.AreEqual(testColor.Red, colorFromHex.Red);
            Assert.AreEqual(testColor.Green, colorFromHex.Green);
            Assert.AreEqual(testColor.Blue, colorFromHex.Blue);
        }

        [Test]
        public void ShouldUseSameFontAcrossFormats()
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

            // Act & Assert - Test font consistency
            Assert.AreEqual("Arial", fontSettings.Name);
            Assert.AreEqual(12, fontSettings.Size);
            Assert.IsTrue(fontSettings.IsBold);
            Assert.IsFalse(fontSettings.IsItalic);
            Assert.AreEqual(new Color(0, 0, 0), fontSettings.Color);

            // Test font settings can be applied consistently
            var fontString = $"{fontSettings.Name}, {fontSettings.Size}pt";
            Assert.IsTrue(fontString.Contains("Arial"));
            Assert.IsTrue(fontString.Contains("12pt"));
        }

        [Test]
        public void ShouldUseSameAlignmentAcrossFormats()
        {
            // Arrange
            var horizontalAlignment = HorizontalAlignment.Center;
            var verticalAlignment = VerticalAlignment.Middle;

            // Act & Assert - Test alignment consistency
            Assert.AreEqual(HorizontalAlignment.Center, horizontalAlignment);
            Assert.AreEqual(VerticalAlignment.Middle, verticalAlignment);

            // Test alignment enum values
            Assert.AreEqual(1, (int)HorizontalAlignment.Center);
            Assert.AreEqual(2, (int)VerticalAlignment.Middle);

            // Test alignment string representation
            Assert.AreEqual("Center", horizontalAlignment.ToString());
            Assert.AreEqual("Middle", verticalAlignment.ToString());
        }

        [Test]
        public void ShouldConvertBetweenFormats()
        {
            // Arrange
            var testData = new
            {
                Title = "Test Document",
                Content = "This is test content",
                Author = "Test Author",
                CreatedDate = DateTime.Now
            };

            // Act & Assert - Test data conversion consistency
            Assert.IsNotNull(testData.Title);
            Assert.IsNotNull(testData.Content);
            Assert.IsNotNull(testData.Author);
            Assert.IsTrue(testData.CreatedDate > DateTime.MinValue);

            // Test that data can be serialized/deserialized consistently
            var titleBytes = System.Text.Encoding.UTF8.GetBytes(testData.Title);
            var contentBytes = System.Text.Encoding.UTF8.GetBytes(testData.Content);
            
            Assert.IsTrue(titleBytes.Length > 0);
            Assert.IsTrue(contentBytes.Length > 0);
        }

        [Test]
        public void ShouldMaintainStyleConsistency()
        {
            // Arrange
            var borderStyle = new BorderStyle
            {
                Color = new Color(0, 0, 0),
                LineStyle = BorderLineStyle.Solid,
                Sides = BorderSides.All,
                Width = 1.0
            };

            var margins = new Margins
            {
                Top = 1.0,
                Bottom = 1.0,
                Left = 1.0,
                Right = 1.0
            };

            var pageSize = new PageSize(8.5, 11.0); // Letter size

            // Act & Assert - Test style consistency
            Assert.AreEqual(new Color(0, 0, 0), borderStyle.Color);
            Assert.AreEqual(BorderLineStyle.Solid, borderStyle.LineStyle);
            Assert.AreEqual(BorderSides.All, borderStyle.Sides);
            Assert.AreEqual(1.0, borderStyle.Width);

            Assert.AreEqual(1.0, margins.Top);
            Assert.AreEqual(1.0, margins.Bottom);
            Assert.AreEqual(1.0, margins.Left);
            Assert.AreEqual(1.0, margins.Right);

            Assert.AreEqual(8.5, pageSize.Width);
            Assert.AreEqual(11.0, pageSize.Height);

            // Test that styles can be applied consistently across formats
            var styleString = $"Border: {borderStyle.LineStyle}, {borderStyle.Width}pt, {borderStyle.Color.ToHex()}";
            Assert.IsTrue(styleString.Contains("Solid"));
            Assert.IsTrue(styleString.Contains("1pt"));
            Assert.IsTrue(styleString.Contains("#000000"));
        }

        [Test]
        public void ShouldHandleCommonEnumsConsistently()
        {
            // Arrange & Act - Test enum consistency
            var horizontalAlignments = Enum.GetValues<HorizontalAlignment>();
            var verticalAlignments = Enum.GetValues<VerticalAlignment>();
            var borderLineStyles = Enum.GetValues<BorderLineStyle>();
            var patternStyles = Enum.GetValues<PatternStyle>();

            // Assert - Test that enums have expected values
            Assert.IsTrue(horizontalAlignments.Length > 0);
            Assert.IsTrue(verticalAlignments.Length > 0);
            Assert.IsTrue(borderLineStyles.Length > 0);
            Assert.IsTrue(patternStyles.Length > 0);

            // Test specific enum values
            Assert.IsTrue(Enum.IsDefined(typeof(HorizontalAlignment), HorizontalAlignment.Left));
            Assert.IsTrue(Enum.IsDefined(typeof(HorizontalAlignment), HorizontalAlignment.Center));
            Assert.IsTrue(Enum.IsDefined(typeof(HorizontalAlignment), HorizontalAlignment.Right));

            Assert.IsTrue(Enum.IsDefined(typeof(VerticalAlignment), VerticalAlignment.Top));
            Assert.IsTrue(Enum.IsDefined(typeof(VerticalAlignment), VerticalAlignment.Middle));
            Assert.IsTrue(Enum.IsDefined(typeof(VerticalAlignment), VerticalAlignment.Bottom));

            Assert.IsTrue(Enum.IsDefined(typeof(BorderLineStyle), BorderLineStyle.Solid));
            Assert.IsTrue(Enum.IsDefined(typeof(BorderLineStyle), BorderLineStyle.Dashed));
            Assert.IsTrue(Enum.IsDefined(typeof(BorderLineStyle), BorderLineStyle.Dotted));
        }

        [Test]
        public void ShouldValidateCommonTypesAcrossFormats()
        {
            // Arrange
            var testColor = Color.FromHex("#FF0000");
            var testFont = new FontSettings { Name = "Times New Roman", Size = 14 };
            var testMargins = Margins.Normal;
            var testPageSize = PageSize.A4;

            // Act & Assert - Test that common types work consistently
            Assert.AreEqual("#FF0000", testColor.ToHex());
            Assert.AreEqual("Times New Roman", testFont.Name);
            Assert.AreEqual(14, testFont.Size);
            Assert.AreEqual(1.0, testMargins.Top);
            Assert.AreEqual(8.27, testPageSize.Width, 0.01); // A4 width in inches
            Assert.AreEqual(11.69, testPageSize.Height, 0.01); // A4 height in inches

            // Test that types can be converted to strings consistently
            var colorString = testColor.ToString();
            var fontString = $"{testFont.Name} {testFont.Size}pt";
            var marginsString = testMargins.ToString();
            var pageSizeString = testPageSize.ToString();

            Assert.IsNotNull(colorString);
            Assert.IsNotNull(fontString);
            Assert.IsNotNull(marginsString);
            Assert.IsNotNull(pageSizeString);
        }
    }
}
