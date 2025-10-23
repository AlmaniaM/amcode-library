using NUnit.Framework;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.UnitTests.Common
{
    [TestFixture]
    public class MarginsTests
    {
        [Test]
        public void ShouldCreateMarginsWithDefaultValues()
        {
            // Arrange & Act
            var margins = new Margins();

            // Assert
            Assert.That(margins.Top, Is.EqualTo(72));
            Assert.That(margins.Bottom, Is.EqualTo(72));
            Assert.That(margins.Left, Is.EqualTo(72));
            Assert.That(margins.Right, Is.EqualTo(72));
        }

        [Test]
        public void ShouldSetCustomMargins()
        {
            // Arrange
            var margins = new Margins();
            var top = 50.0;
            var bottom = 60.0;
            var left = 70.0;
            var right = 80.0;

            // Act
            margins.Top = top;
            margins.Bottom = bottom;
            margins.Left = left;
            margins.Right = right;

            // Assert
            Assert.That(margins.Top, Is.EqualTo(top));
            Assert.That(margins.Bottom, Is.EqualTo(bottom));
            Assert.That(margins.Left, Is.EqualTo(left));
            Assert.That(margins.Right, Is.EqualTo(right));
        }

        [Test]
        public void ShouldCreateDefaultMargins()
        {
            // Act
            var margins = Margins.Default;

            // Assert
            Assert.That(margins.Top, Is.EqualTo(72));
            Assert.That(margins.Bottom, Is.EqualTo(72));
            Assert.That(margins.Left, Is.EqualTo(72));
            Assert.That(margins.Right, Is.EqualTo(72));
        }

        [Test]
        public void ShouldCreateNarrowMargins()
        {
            // Act
            var margins = Margins.Narrow;

            // Assert
            Assert.That(margins.Top, Is.EqualTo(36));
            Assert.That(margins.Bottom, Is.EqualTo(36));
            Assert.That(margins.Left, Is.EqualTo(36));
            Assert.That(margins.Right, Is.EqualTo(36));
        }

        [Test]
        public void ShouldCreateWideMargins()
        {
            // Act
            var margins = Margins.Wide;

            // Assert
            Assert.That(margins.Top, Is.EqualTo(108));
            Assert.That(margins.Bottom, Is.EqualTo(108));
            Assert.That(margins.Left, Is.EqualTo(108));
            Assert.That(margins.Right, Is.EqualTo(108));
        }
    }
}
