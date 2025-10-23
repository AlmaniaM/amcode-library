using NUnit.Framework;
using AMCode.Documents.Common.Models;

namespace AMCode.Documents.UnitTests.Common
{
    [TestFixture]
    public class PageSizeTests
    {
        [Test]
        public void ShouldCreatePageSizeWithDefaultValues()
        {
            // Arrange & Act
            var pageSize = new PageSize();

            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(595.3));
            Assert.That(pageSize.Height, Is.EqualTo(841.9));
            Assert.That(pageSize.WidthInPoints, Is.EqualTo(595.3));
            Assert.That(pageSize.HeightInPoints, Is.EqualTo(841.9));
            Assert.That(pageSize.Name, Is.EqualTo("A4"));
        }

        [Test]
        public void ShouldSetCustomDimensions()
        {
            // Arrange
            var pageSize = new PageSize();
            var width = 800.0;
            var height = 600.0;

            // Act
            pageSize.Width = width;
            pageSize.Height = height;

            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(width));
            Assert.That(pageSize.Height, Is.EqualTo(height));
            Assert.That(pageSize.WidthInPoints, Is.EqualTo(width));
            Assert.That(pageSize.HeightInPoints, Is.EqualTo(height));
        }

        [Test]
        public void ShouldSetPageName()
        {
            // Arrange
            var pageSize = new PageSize();
            var name = "Custom";

            // Act
            pageSize.Name = name;

            // Assert
            Assert.That(pageSize.Name, Is.EqualTo(name));
        }

        [Test]
        public void ShouldCreateA4PageSize()
        {
            // Act
            var pageSize = PageSize.A4;

            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(595.3));
            Assert.That(pageSize.Height, Is.EqualTo(841.9));
            Assert.That(pageSize.Name, Is.EqualTo("A4"));
        }

        [Test]
        public void ShouldCreateLetterPageSize()
        {
            // Act
            var pageSize = PageSize.Letter;

            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(612));
            Assert.That(pageSize.Height, Is.EqualTo(792));
            Assert.That(pageSize.Name, Is.EqualTo("Letter"));
        }

        [Test]
        public void ShouldCreateLegalPageSize()
        {
            // Act
            var pageSize = PageSize.Legal;

            // Assert
            Assert.That(pageSize.Width, Is.EqualTo(612));
            Assert.That(pageSize.Height, Is.EqualTo(1008));
            Assert.That(pageSize.Name, Is.EqualTo("Legal"));
        }
    }
}
