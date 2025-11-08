using NUnit.Framework;
using FluentAssertions;
using AMCode.OCR.Models;

namespace AMCode.OCR.Tests.Models;

[TestFixture]
public class BoundingBoxTests
{
    [Test]
    public void BoundingBox_ShouldInitializeWithDefaultValues()
    {
        // Act
        var boundingBox = new BoundingBox();

        // Assert
        boundingBox.X.Should().Be(0);
        boundingBox.Y.Should().Be(0);
        boundingBox.Width.Should().Be(0);
        boundingBox.Height.Should().Be(0);
    }

    [Test]
    public void BoundingBox_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var x = 10;
        var y = 20;
        var width = 100;
        var height = 50;

        // Act
        var boundingBox = new BoundingBox
        {
            X = x,
            Y = y,
            Width = width,
            Height = height
        };

        // Assert
        boundingBox.X.Should().Be(x);
        boundingBox.Y.Should().Be(y);
        boundingBox.Width.Should().Be(width);
        boundingBox.Height.Should().Be(height);
    }

    [Test]
    public void BoundingBox_WithNegativeValues_ShouldAcceptThem()
    {
        // Arrange
        var boundingBox = new BoundingBox
        {
            X = -10,
            Y = -20,
            Width = 100,
            Height = 50
        };

        // Assert
        boundingBox.X.Should().Be(-10);
        boundingBox.Y.Should().Be(-20);
        boundingBox.Width.Should().Be(100);
        boundingBox.Height.Should().Be(50);
    }

    [Test]
    public void BoundingBox_WithZeroDimensions_ShouldBeValid()
    {
        // Arrange
        var boundingBox = new BoundingBox
        {
            X = 0,
            Y = 0,
            Width = 0,
            Height = 0
        };

        // Assert
        boundingBox.X.Should().Be(0);
        boundingBox.Y.Should().Be(0);
        boundingBox.Width.Should().Be(0);
        boundingBox.Height.Should().Be(0);
    }

    [Test]
    public void BoundingBox_WithLargeValues_ShouldAcceptThem()
    {
        // Arrange
        var boundingBox = new BoundingBox
        {
            X = int.MaxValue,
            Y = int.MaxValue,
            Width = int.MaxValue,
            Height = int.MaxValue
        };

        // Assert
        boundingBox.X.Should().Be(int.MaxValue);
        boundingBox.Y.Should().Be(int.MaxValue);
        boundingBox.Width.Should().Be(int.MaxValue);
        boundingBox.Height.Should().Be(int.MaxValue);
    }

    [Test]
    public void BoundingBox_Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var boundingBox1 = new BoundingBox { X = 10, Y = 20, Width = 100, Height = 50 };
        var boundingBox2 = new BoundingBox { X = 10, Y = 20, Width = 100, Height = 50 };
        var boundingBox3 = new BoundingBox { X = 11, Y = 20, Width = 100, Height = 50 };

        // Assert
        boundingBox1.Should().BeEquivalentTo(boundingBox2);
        boundingBox1.Should().NotBeEquivalentTo(boundingBox3);
    }
}