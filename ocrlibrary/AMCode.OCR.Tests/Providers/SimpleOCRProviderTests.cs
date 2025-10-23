using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using AMCode.OCR.Models;
using AMCode.OCR.Providers;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Moq;

namespace AMCode.OCR.Tests.Providers;

[TestClass]
public class SimpleOCRProviderTests
{
    private Mock<ILogger> _mockLogger = null!;
    private Mock<IHttpClientFactory> _mockHttpClientFactory = null!;
    private TestOCRProvider _provider = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockLogger = new Mock<ILogger>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _provider = new TestOCRProvider(_mockLogger.Object, _mockHttpClientFactory.Object);
    }

    [TestMethod]
    public void ProviderName_ShouldReturnCorrectName()
    {
        // Act
        var result = _provider.ProviderName;

        // Assert
        result.Should().Be("Test OCR Provider");
    }

    [TestMethod]
    public void RequiresInternet_ShouldReturnFalse()
    {
        // Act
        var result = _provider.RequiresInternet;

        // Assert
        result.Should().BeFalse();
    }

    [TestMethod]
    public void IsAvailable_ShouldReturnTrue()
    {
        // Act
        var result = _provider.IsAvailable;

        // Assert
        result.Should().BeTrue();
    }

    [TestMethod]
    public void Capabilities_ShouldReturnValidCapabilities()
    {
        // Act
        var capabilities = _provider.Capabilities;

        // Assert
        capabilities.Should().NotBeNull();
        capabilities.SupportsLanguageDetection.Should().BeFalse();
        capabilities.SupportsBoundingBoxes.Should().BeFalse();
        capabilities.SupportsConfidenceScores.Should().BeTrue();
    }

    [TestMethod]
    public async Task ProcessImageAsync_WithValidStream_ShouldReturnResult()
    {
        // Arrange
        var imageBytes = Encoding.UTF8.GetBytes("test image data");
        using var imageStream = new MemoryStream(imageBytes);

        // Act
        var result = await _provider.ProcessImageAsync(imageStream, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Text.Should().NotBeNullOrEmpty();
        result.Provider.Should().Be("Test OCR Provider");
    }

    [TestMethod]
    public async Task ProcessImageAsync_WithOptions_ShouldReturnResult()
    {
        // Arrange
        var imageBytes = Encoding.UTF8.GetBytes("test image data");
        using var imageStream = new MemoryStream(imageBytes);
        var options = new OCRRequest
        {
            ConfidenceThreshold = 0.8,
            RequiresLanguageDetection = true
        };

        // Act
        var result = await _provider.ProcessImageAsync(imageStream, options, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Text.Should().NotBeNullOrEmpty();
        result.Provider.Should().Be("Test OCR Provider");
    }

    [TestMethod]
    public async Task CheckHealthAsync_ShouldReturnHealthyStatus()
    {
        // Arrange
        // Use _provider field

        // Act
        var health = await _provider.CheckHealthAsync();

        // Assert
        health.Should().NotBeNull();
        health.IsHealthy.Should().BeTrue();
        health.IsAvailable.Should().BeTrue();
        health.Status.Should().Be("Healthy");
    }

    [TestMethod]
    public async Task GetCostEstimateAsync_ShouldReturnZero()
    {
        // Arrange
        // Use _provider field
        var options = new OCRRequest();

        // Act
        var cost = await _provider.GetCostEstimateAsync(1024, options);

        // Assert
        cost.Should().Be(0);
    }

    [TestMethod]
    public async Task ProcessBatchAsync_WithMultipleStreams_ShouldReturnResults()
    {
        // Arrange
        // Use _provider field
        var imageBytes = Encoding.UTF8.GetBytes("test image data");
        var streams = new[]
        {
            new MemoryStream(imageBytes),
            new MemoryStream(imageBytes)
        };

        // Act
        var results = await _provider.ProcessBatchAsync(streams, null, CancellationToken.None);

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(2);
        results.All(r => !string.IsNullOrEmpty(r.Text)).Should().BeTrue();
    }

    [TestMethod]
    public void CanProcess_WithValidOptions_ShouldReturnTrue()
    {
        // Arrange
        var options = new OCRRequest
        {
            ImagePath = "test.jpg",
            MaxImageSizeMB = 5
        };

        // Act
        var canProcess = _provider.CanProcess(options);

        // Assert
        canProcess.Should().BeTrue();
    }

    [TestMethod]
    public void GetEstimatedProcessingTime_ShouldReturnReasonableTime()
    {
        // Arrange
        // Use _provider field
        var options = new OCRRequest();

        // Act
        var time = _provider.GetEstimatedProcessingTime(1024, options);

        // Assert
        time.Should().BeGreaterThan(TimeSpan.Zero);
        time.Should().BeLessThan(TimeSpan.FromMinutes(1));
    }

    [TestMethod]
    public void GetReliabilityScore_ShouldReturnValidScore()
    {
        // Arrange
        // Use _provider field

        // Act
        var score = _provider.GetReliabilityScore();

        // Assert
        score.Should().BeGreaterOrEqualTo(0.0);
        score.Should().BeLessOrEqualTo(1.0);
    }

    [TestMethod]
    public void GetQualityScore_ShouldReturnValidScore()
    {
        // Arrange
        // Use _provider field

        // Act
        var score = _provider.GetQualityScore();

        // Assert
        score.Should().BeGreaterOrEqualTo(0.0);
        score.Should().BeLessOrEqualTo(1.0);
    }
}
