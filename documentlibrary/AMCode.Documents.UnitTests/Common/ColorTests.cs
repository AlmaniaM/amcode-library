using NUnit.Framework;
using AMCode.Documents.Common.Drawing;

namespace AMCode.Documents.UnitTests.Common
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void ShouldCreateColorFromRgb()
        {
            // Arrange
            byte r = 255, g = 128, b = 64;

            // Act
            var color = Color.FromArgb(r, g, b);

            // Assert
            Assert.That(color.R, Is.EqualTo(r));
            Assert.That(color.G, Is.EqualTo(g));
            Assert.That(color.B, Is.EqualTo(b));
            Assert.That(color.A, Is.EqualTo(255)); // Default alpha
        }

        [Test]
        public void ShouldCreateColorFromArgb()
        {
            // Arrange
            byte a = 200, r = 255, g = 128, b = 64;

            // Act
            var color = Color.FromArgb(a, r, g, b);

            // Assert
            Assert.That(color.A, Is.EqualTo(a));
            Assert.That(color.R, Is.EqualTo(r));
            Assert.That(color.G, Is.EqualTo(g));
            Assert.That(color.B, Is.EqualTo(b));
        }

        [Test]
        public void ShouldConvertToArgb()
        {
            // Arrange
            var color = Color.FromArgb(200, 255, 128, 64);
            int expectedArgb = (200 << 24) | (255 << 16) | (128 << 8) | 64;

            // Act
            var result = color.ToArgb();

            // Assert
            Assert.That(result, Is.EqualTo(expectedArgb));
        }

        [Test]
        public void ShouldCompareColors()
        {
            // Arrange
            var color1 = Color.FromArgb(255, 128, 64);
            var color2 = Color.FromArgb(255, 128, 64);
            var color3 = Color.FromArgb(255, 128, 65);

            // Act & Assert - Since Color doesn't override Equals, we compare properties
            Assert.That(color1.A, Is.EqualTo(color2.A));
            Assert.That(color1.R, Is.EqualTo(color2.R));
            Assert.That(color1.G, Is.EqualTo(color2.G));
            Assert.That(color1.B, Is.EqualTo(color2.B));
            
            Assert.That(color1.A, Is.EqualTo(color3.A)); // Both have default alpha of 255
            Assert.That(color1.R, Is.EqualTo(color3.R)); // Both have R=255
            Assert.That(color1.G, Is.EqualTo(color3.G)); // Both have G=128
            Assert.That(color1.B, Is.Not.EqualTo(color3.B)); // Only B is different (64 vs 65)
        }

        [Test]
        public void ShouldHandleEmptyColor()
        {
            // Arrange & Act
            var color = new Color();

            // Assert
            Assert.That(color.A, Is.EqualTo(255)); // Default alpha
            Assert.That(color.R, Is.EqualTo(0));
            Assert.That(color.G, Is.EqualTo(0));
            Assert.That(color.B, Is.EqualTo(0));
        }

        [Test]
        public void ShouldValidatePredefinedColors()
        {
            // Act & Assert
            Assert.That(Color.Black.R, Is.EqualTo(0));
            Assert.That(Color.Black.G, Is.EqualTo(0));
            Assert.That(Color.Black.B, Is.EqualTo(0));

            Assert.That(Color.White.R, Is.EqualTo(255));
            Assert.That(Color.White.G, Is.EqualTo(255));
            Assert.That(Color.White.B, Is.EqualTo(255));

            Assert.That(Color.Red.R, Is.EqualTo(255));
            Assert.That(Color.Red.G, Is.EqualTo(0));
            Assert.That(Color.Red.B, Is.EqualTo(0));

            Assert.That(Color.Green.R, Is.EqualTo(0));
            Assert.That(Color.Green.G, Is.EqualTo(255));
            Assert.That(Color.Green.B, Is.EqualTo(0));

            Assert.That(Color.Blue.R, Is.EqualTo(0));
            Assert.That(Color.Blue.G, Is.EqualTo(0));
            Assert.That(Color.Blue.B, Is.EqualTo(255));

            Assert.That(Color.Transparent.A, Is.EqualTo(0));
            Assert.That(Color.Transparent.R, Is.EqualTo(0));
            Assert.That(Color.Transparent.G, Is.EqualTo(0));
            Assert.That(Color.Transparent.B, Is.EqualTo(0));
        }

        [Test]
        public void ShouldHandleColorEquality()
        {
            // Arrange
            var color1 = Color.FromArgb(255, 128, 64);
            var color2 = Color.FromArgb(255, 128, 64);
            var color3 = Color.FromArgb(255, 128, 65);

            // Act & Assert - Color implements value equality
            Assert.That(color1.Equals(color2), Is.True); // Same values
            Assert.That(color1.Equals(color1), Is.True); // Same instance
            Assert.That(color1.Equals(color3), Is.False); // Different values
            Assert.That(color1.Equals(null), Is.False);
            Assert.That(color1.Equals("string"), Is.False);
        }

        [Test]
        public void ShouldHandleColorHashCode()
        {
            // Arrange
            var color1 = Color.FromArgb(255, 128, 64);
            var color2 = Color.FromArgb(255, 128, 64);
            var color3 = Color.FromArgb(255, 128, 65);

            // Act
            var hash1 = color1.GetHashCode();
            var hash2 = color2.GetHashCode();
            var hash3 = color3.GetHashCode();

            // Assert - Color implements value-based hash code
            Assert.That(hash1, Is.EqualTo(hash2)); // Same values should have same hash
            Assert.That(hash1, Is.Not.EqualTo(hash3)); // Different values should have different hash
            Assert.That(hash1, Is.EqualTo(color1.GetHashCode())); // Same instance
        }

        [Test]
        public void ShouldCreateColorFromArgbInt()
        {
            // Arrange
            int argb = (200 << 24) | (255 << 16) | (128 << 8) | 64;

            // Act
            var color = Color.FromArgb(argb);

            // Assert
            Assert.That(color.A, Is.EqualTo(200));
            Assert.That(color.R, Is.EqualTo(255));
            Assert.That(color.G, Is.EqualTo(128));
            Assert.That(color.B, Is.EqualTo(64));
        }

        [Test]
        public void ShouldValidateAdditionalPredefinedColors()
        {
            // Act & Assert
            Assert.That(Color.LightGray.R, Is.EqualTo(211));
            Assert.That(Color.LightGray.G, Is.EqualTo(211));
            Assert.That(Color.LightGray.B, Is.EqualTo(211));

            Assert.That(Color.LightBlue.R, Is.EqualTo(173));
            Assert.That(Color.LightBlue.G, Is.EqualTo(216));
            Assert.That(Color.LightBlue.B, Is.EqualTo(230));

            Assert.That(Color.Gray.R, Is.EqualTo(128));
            Assert.That(Color.Gray.G, Is.EqualTo(128));
            Assert.That(Color.Gray.B, Is.EqualTo(128));

            Assert.That(Color.DarkGray.R, Is.EqualTo(64));
            Assert.That(Color.DarkGray.G, Is.EqualTo(64));
            Assert.That(Color.DarkGray.B, Is.EqualTo(64));
        }
    }
}
