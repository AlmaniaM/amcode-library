using NUnit.Framework;
using FluentAssertions;
using AMCode.OCR.Models;

namespace AMCode.OCR.Tests.Models;

[TestFixture]
public class OCRResultTests
{
    [Test]
    public void OCRResult_ShouldInitializeWithDefaultValues()
    {
        // Act
        var result = new OCRResult();

        // Assert
        result.Text.Should().BeEmpty();
        result.TextBlocks.Should().NotBeNull();
        result.TextBlocks.Should().BeEmpty();
        result.Confidence.Should().Be(0.0);
        result.Language.Should().BeEmpty();
        result.Provider.Should().BeEmpty();
        result.ProcessingTime.Should().Be(TimeSpan.Zero);
        result.Cost.Should().Be(0);
        result.ProcessedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        result.Metadata.Should().NotBeNull();
        result.Metadata.Should().BeEmpty();
        result.ContainsHandwriting.Should().BeFalse();
        result.ContainsPrintedText.Should().BeFalse();
        result.Error.Should().BeEmpty();
    }

    [Test]
    public void OCRResult_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var text = "Sample extracted text";
        var confidence = 0.95;
        var language = "en";
        var provider = "Test Provider";
        var processingTime = TimeSpan.FromMilliseconds(500);
        var cost = 0.01m;
        var processedAt = DateTime.UtcNow;
        var metadata = new Dictionary<string, object> { ["key"] = "value" };
        var textBlocks = new List<TextBlock>
        {
            new TextBlock { Text = "Sample", Confidence = 0.9 },
            new TextBlock { Text = "extracted", Confidence = 0.8 },
            new TextBlock { Text = "text", Confidence = 0.95 }
        };

        // Act
        var result = new OCRResult
        {
            Text = text,
            Confidence = confidence,
            Language = language,
            Provider = provider,
            ProcessingTime = processingTime,
            Cost = cost,
            ProcessedAt = processedAt,
            Metadata = metadata,
            TextBlocks = textBlocks,
            ContainsHandwriting = true,
            ContainsPrintedText = false,
            Error = ""
        };

        // Assert
        result.Text.Should().Be(text);
        result.Confidence.Should().Be(confidence);
        result.Language.Should().Be(language);
        result.Provider.Should().Be(provider);
        result.ProcessingTime.Should().Be(processingTime);
        result.Cost.Should().Be(cost);
        result.ProcessedAt.Should().Be(processedAt);
        result.Metadata.Should().BeEquivalentTo(metadata);
        result.TextBlocks.Should().BeEquivalentTo(textBlocks);
        result.ContainsHandwriting.Should().BeTrue();
        result.ContainsPrintedText.Should().BeFalse();
        result.Error.Should().BeEmpty();
    }

    [Test]
    public void WordCount_WithText_ShouldReturnCorrectCount()
    {
        // Arrange
        var result = new OCRResult
        {
            Text = "This is a sample text with multiple words"
        };

        // Act
        var wordCount = result.WordCount;

        // Assert
        wordCount.Should().Be(8);
    }

    [Test]
    public void WordCount_WithEmptyText_ShouldReturnZero()
    {
        // Arrange
        var result = new OCRResult
        {
            Text = ""
        };

        // Act
        var wordCount = result.WordCount;

        // Assert
        wordCount.Should().Be(0);
    }

    [Test]
    public void WordCount_WithWhitespaceOnly_ShouldReturnZero()
    {
        // Arrange
        var result = new OCRResult
        {
            Text = "   \t\n  "
        };

        // Act
        var wordCount = result.WordCount;

        // Assert
        wordCount.Should().Be(0);
    }

    [Test]
    public void LineCount_WithText_ShouldReturnCorrectCount()
    {
        // Arrange
        var result = new OCRResult
        {
            Text = "Line 1\nLine 2\nLine 3"
        };

        // Act
        var lineCount = result.LineCount;

        // Assert
        lineCount.Should().Be(3);
    }

    [Test]
    public void LineCount_WithEmptyText_ShouldReturnZero()
    {
        // Arrange
        var result = new OCRResult
        {
            Text = ""
        };

        // Act
        var lineCount = result.LineCount;

        // Assert
        lineCount.Should().Be(0);
    }

    [Test]
    public void LineCount_WithSingleLine_ShouldReturnOne()
    {
        // Arrange
        var result = new OCRResult
        {
            Text = "Single line text"
        };

        // Act
        var lineCount = result.LineCount;

        // Assert
        lineCount.Should().Be(1);
    }

    [Test]
    public void OCRResult_WithTextBlocks_ShouldCalculatePropertiesCorrectly()
    {
        // Arrange
        var textBlocks = new List<TextBlock>
        {
            new TextBlock { Text = "Hello", Confidence = 0.9, IsHandwritten = true },
            new TextBlock { Text = "World", Confidence = 0.8, IsPrinted = true }
        };

        // Act
        var result = new OCRResult
        {
            Text = "Hello World",
            TextBlocks = textBlocks,
            ContainsHandwriting = true,
            ContainsPrintedText = true
        };

        // Assert
        result.TextBlocks.Should().HaveCount(2);
        result.ContainsHandwriting.Should().BeTrue();
        result.ContainsPrintedText.Should().BeTrue();
    }

    [Test]
    public void OCRResult_WithError_ShouldSetErrorCorrectly()
    {
        // Arrange
        var errorMessage = "OCR processing failed";

        // Act
        var result = new OCRResult
        {
            Error = errorMessage
        };

        // Assert
        result.Error.Should().Be(errorMessage);
    }
}