using NUnit.Framework;
using AMCode.Documents.Common.Models;
using AMCode.Documents.Common.Drawing;
using AMCode.Documents.Common.Enums;

namespace AMCode.Documents.UnitTests.Common
{
    [TestFixture]
    public class BorderStyleTests
    {
        [Test]
        public void ShouldCreateBorderStyleWithDefaultValues()
        {
            // Arrange & Act
            var borderStyle = new BorderStyle();

            // Assert
            Assert.That(borderStyle.LineStyle, Is.EqualTo(BorderLineStyle.Single));
            Assert.That(borderStyle.Color.A, Is.EqualTo(Color.Black.A));
            Assert.That(borderStyle.Color.R, Is.EqualTo(Color.Black.R));
            Assert.That(borderStyle.Color.G, Is.EqualTo(Color.Black.G));
            Assert.That(borderStyle.Color.B, Is.EqualTo(Color.Black.B));
            Assert.That(borderStyle.Sides, Is.EqualTo(BorderSides.All));
            Assert.That(borderStyle.Width, Is.EqualTo(0.5));
        }

        [Test]
        public void ShouldSetLineStyle()
        {
            // Arrange
            var borderStyle = new BorderStyle();
            var lineStyle = BorderLineStyle.Double;

            // Act
            borderStyle.LineStyle = lineStyle;

            // Assert
            Assert.That(borderStyle.LineStyle, Is.EqualTo(lineStyle));
        }

        [Test]
        public void ShouldSetBorderColor()
        {
            // Arrange
            var borderStyle = new BorderStyle();
            var color = Color.FromArgb(255, 0, 0); // Red

            // Act
            borderStyle.Color = color;

            // Assert
            Assert.That(borderStyle.Color.A, Is.EqualTo(color.A));
            Assert.That(borderStyle.Color.R, Is.EqualTo(color.R));
            Assert.That(borderStyle.Color.G, Is.EqualTo(color.G));
            Assert.That(borderStyle.Color.B, Is.EqualTo(color.B));
        }

        [Test]
        public void ShouldSetBorderSides()
        {
            // Arrange
            var borderStyle = new BorderStyle();
            var sides = BorderSides.Top | BorderSides.Bottom;

            // Act
            borderStyle.Sides = sides;

            // Assert
            Assert.That(borderStyle.Sides, Is.EqualTo(sides));
        }

        [Test]
        public void ShouldSetBorderWidth()
        {
            // Arrange
            var borderStyle = new BorderStyle();
            var width = 2.0;

            // Act
            borderStyle.Width = width;

            // Assert
            Assert.That(borderStyle.Width, Is.EqualTo(width));
        }

        [Test]
        public void ShouldCreateBorderStyleWithCustomValues()
        {
            // Arrange & Act
            var borderStyle = new BorderStyle
            {
                LineStyle = BorderLineStyle.Dashed,
                Color = Color.Blue,
                Sides = BorderSides.Left | BorderSides.Right,
                Width = 1.5
            };

            // Assert
            Assert.That(borderStyle.LineStyle, Is.EqualTo(BorderLineStyle.Dashed));
            Assert.That(borderStyle.Color.A, Is.EqualTo(Color.Blue.A));
            Assert.That(borderStyle.Color.R, Is.EqualTo(Color.Blue.R));
            Assert.That(borderStyle.Color.G, Is.EqualTo(Color.Blue.G));
            Assert.That(borderStyle.Color.B, Is.EqualTo(Color.Blue.B));
            Assert.That(borderStyle.Sides, Is.EqualTo(BorderSides.Left | BorderSides.Right));
            Assert.That(borderStyle.Width, Is.EqualTo(1.5));
        }
    }
}
