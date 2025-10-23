using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using AMCode.OCR.Models;

namespace AMCode.OCR.Tests.Models;

[TestClass]
public class TextBlockTests
{
    [TestMethod]
    public void TextBlock_ShouldInitializeWithDefaultValues()
    {
        // Act
        var textBlock = new TextBlock();

        // Assert
        textBlock.Text.Should().BeEmpty();
        textBlock.Confidence.Should().Be(0.0);
        textBlock.BoundingBox.Should().NotBeNull();
        textBlock.Language.Should().BeEmpty();
        textBlock.IsHandwritten.Should().BeFalse();
        textBlock.IsPrinted.Should().BeFalse();
    }

    [TestMethod]
    public void TextBlock_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var boundingBox = new BoundingBox { X = 10, Y = 20, Width = 100, Height = 30 };
        var text = "Sample text";
        var confidence = 0.95;
        var language = "en";
        var isHandwritten = true;
        var isPrinted = false;

        // Act
        var textBlock = new TextBlock
        {
            Text = text,
            Confidence = confidence,
            BoundingBox = boundingBox,
            Language = language,
            IsHandwritten = isHandwritten,
            IsPrinted = isPrinted
        };

        // Assert
        textBlock.Text.Should().Be(text);
        textBlock.Confidence.Should().Be(confidence);
        textBlock.BoundingBox.Should().Be(boundingBox);
        textBlock.Language.Should().Be(language);
        textBlock.IsHandwritten.Should().Be(isHandwritten);
        textBlock.IsPrinted.Should().Be(isPrinted);
    }

    [TestMethod]
    public void TextBlock_WithBoundingBox_ShouldHaveCorrectProperties()
    {
        // Arrange
        var boundingBox = new BoundingBox { X = 5, Y = 10, Width = 50, Height = 20 };
        var textBlock = new TextBlock
        {
            Text = "Test",
            BoundingBox = boundingBox
        };

        // Assert
        textBlock.BoundingBox.Should().NotBeNull();
        textBlock.BoundingBox.X.Should().Be(5);
        textBlock.BoundingBox.Y.Should().Be(10);
        textBlock.BoundingBox.Width.Should().Be(50);
        textBlock.BoundingBox.Height.Should().Be(20);
    }

    [TestMethod]
    public void TextBlock_WithHandwriting_ShouldSetCorrectFlags()
    {
        // Arrange
        var textBlock = new TextBlock
        {
            Text = "Handwritten text",
            IsHandwritten = true,
            IsPrinted = false
        };

        // Assert
        textBlock.IsHandwritten.Should().BeTrue();
        textBlock.IsPrinted.Should().BeFalse();
    }

    [TestMethod]
    public void TextBlock_WithPrintedText_ShouldSetCorrectFlags()
    {
        // Arrange
        var textBlock = new TextBlock
        {
            Text = "Printed text",
            IsHandwritten = false,
            IsPrinted = true
        };

        // Assert
        textBlock.IsHandwritten.Should().BeFalse();
        textBlock.IsPrinted.Should().BeTrue();
    }
}