using NUnit.Framework;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.UnitTests.Common
{
    [TestFixture]
    public class FontStyleTests
    {
        [Test]
        public void ShouldCreateFontStyleWithDefaultValues()
        {
            // Arrange & Act
            var fontStyle = new FontStyle();

            // Assert
            Assert.That(fontStyle.FontName, Is.EqualTo("Calibri"));
            Assert.That(fontStyle.FontSize, Is.EqualTo(11));
            Assert.That(fontStyle.Bold, Is.False);
            Assert.That(fontStyle.Italic, Is.False);
            Assert.That(fontStyle.Underline, Is.False);
            Assert.That(fontStyle.Strikethrough, Is.False);
            Assert.That(fontStyle.Color.A, Is.EqualTo(Color.Black.A));
            Assert.That(fontStyle.Color.R, Is.EqualTo(Color.Black.R));
            Assert.That(fontStyle.Color.G, Is.EqualTo(Color.Black.G));
            Assert.That(fontStyle.Color.B, Is.EqualTo(Color.Black.B));
        }

        [Test]
        public void ShouldSetFontName()
        {
            // Arrange
            var fontStyle = new FontStyle();
            var fontName = "Arial";

            // Act
            fontStyle.FontName = fontName;

            // Assert
            Assert.That(fontStyle.FontName, Is.EqualTo(fontName));
        }

        [Test]
        public void ShouldSetFontSize()
        {
            // Arrange
            var fontStyle = new FontStyle();
            var fontSize = 14.5;

            // Act
            fontStyle.FontSize = fontSize;

            // Assert
            Assert.That(fontStyle.FontSize, Is.EqualTo(fontSize));
        }

        [Test]
        public void ShouldSetFormattingProperties()
        {
            // Arrange
            var fontStyle = new FontStyle();

            // Act
            fontStyle.Bold = true;
            fontStyle.Italic = true;
            fontStyle.Underline = true;
            fontStyle.Strikethrough = true;

            // Assert
            Assert.That(fontStyle.Bold, Is.True);
            Assert.That(fontStyle.Italic, Is.True);
            Assert.That(fontStyle.Underline, Is.True);
            Assert.That(fontStyle.Strikethrough, Is.True);
        }

        [Test]
        public void ShouldSetFontColor()
        {
            // Arrange
            var fontStyle = new FontStyle();
            var color = Color.FromArgb(255, 0, 0); // Red

            // Act
            fontStyle.Color = color;

            // Assert
            Assert.That(fontStyle.Color, Is.EqualTo(color));
        }

        [Test]
        public void ShouldCreateFontStyleWithCustomValues()
        {
            // Arrange & Act
            var fontStyle = new FontStyle
            {
                FontName = "Times New Roman",
                FontSize = 16,
                Bold = true,
                Italic = true,
                Underline = false,
                Strikethrough = false,
                Color = Color.Blue
            };

            // Assert
            Assert.That(fontStyle.FontName, Is.EqualTo("Times New Roman"));
            Assert.That(fontStyle.FontSize, Is.EqualTo(16));
            Assert.That(fontStyle.Bold, Is.True);
            Assert.That(fontStyle.Italic, Is.True);
            Assert.That(fontStyle.Underline, Is.False);
            Assert.That(fontStyle.Strikethrough, Is.False);
            Assert.That(fontStyle.Color.A, Is.EqualTo(Color.Blue.A));
            Assert.That(fontStyle.Color.R, Is.EqualTo(Color.Blue.R));
            Assert.That(fontStyle.Color.G, Is.EqualTo(Color.Blue.G));
            Assert.That(fontStyle.Color.B, Is.EqualTo(Color.Blue.B));
        }
    }
}
