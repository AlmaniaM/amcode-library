using AMCode.OCR.Models;
using AMCode.OCR.Providers;
using AMCode.OCR.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Moq;
using FluentAssertions;
using System.Text;
using NUnit.Framework;

namespace AMCode.OCR.Tests.Providers;

[TestFixture]
public class SimpleAzureOCRServiceTests
{
    private Mock<ComputerVisionClient> _mockClient = null!;
    private Mock<ILogger<AzureComputerVisionOCRService>> _mockLogger = null!;
    private Mock<IOptions<AzureOCRConfiguration>> _mockOptions = null!;
    private AzureComputerVisionOCRService _service = null!;
    private AzureOCRConfiguration _config = null!;

    [SetUp]
    public void Setup()
    {
        _mockClient = new Mock<ComputerVisionClient>();
        _mockLogger = new Mock<ILogger<AzureComputerVisionOCRService>>();
        _mockOptions = new Mock<IOptions<AzureOCRConfiguration>>();
        
        _config = new AzureOCRConfiguration
        {
            SubscriptionKey = "test-key",
            Endpoint = "https://test.cognitiveservices.azure.com/",
            Region = "eastus"
        };
        
        _mockOptions.Setup(x => x.Value).Returns(_config);
        _service = new AzureComputerVisionOCRService(_mockClient.Object, _mockLogger.Object, _mockOptions.Object);
    }

    [Test]
    public void ProviderName_ShouldReturnCorrectName()
    {
        // Act
        var result = _service.ProviderName;

        // Assert
        result.Should().Be("Azure Computer Vision");
    }

    [Test]
    public void Capabilities_ShouldReturnCorrectCapabilities()
    {
        // Act
        var capabilities = _service.Capabilities;

        // Assert
        capabilities.Should().NotBeNull();
        capabilities.SupportsHandwriting.Should().BeTrue();
        capabilities.SupportsPrintedText.Should().BeTrue();
        capabilities.SupportsLanguageDetection.Should().BeTrue();
        capabilities.MaxImageSizeMB.Should().Be(50);
        capabilities.SupportedLanguages.Should().Contain("en");
        capabilities.SupportedLanguages.Should().Contain("es");
        capabilities.SupportedLanguages.Should().Contain("fr");
        capabilities.SupportedLanguages.Should().Contain("de");
    }

    [Test]
    public void RequiresInternet_ShouldReturnTrue()
    {
        // Act
        var result = _service.RequiresInternet;

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsAvailable_ShouldReturnTrue()
    {
        // Act
        var result = _service.IsAvailable;

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task CheckHealthAsync_WhenHealthy_ShouldReturnHealthy()
    {
        // Arrange
        _mockClient.Setup(x => x.GetReadResultAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReadOperationResult { Status = OperationStatusCodes.Succeeded });

        // Act
        var result = await _service.CheckHealthAsync();

        // Assert
        result.Should().NotBeNull();
        result.IsHealthy.Should().BeTrue();
        result.ResponseTime.Should().BeGreaterThan(TimeSpan.Zero);
        result.LastChecked.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task CheckHealthAsync_WhenUnhealthy_ShouldReturnUnhealthy()
    {
        // Arrange
        _mockClient.Setup(x => x.GetReadResultAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service unavailable"));

        // Act
        var result = await _service.CheckHealthAsync();

        // Assert
        result.Should().NotBeNull();
        result.IsHealthy.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Service unavailable");
        result.LastChecked.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task ProcessImageAsync_WithValidImage_ShouldReturnOCRResult()
    {
        // Arrange
        var imageStream = new MemoryStream(Encoding.UTF8.GetBytes("test image"));
        var operationId = "test-operation-id";
        
        var readResult = new ReadOperationResult
        {
            Status = OperationStatusCodes.Succeeded,
            AnalyzeResult = new AnalyzeResults
            {
                ReadResults = new List<ReadResult>
                {
                    new ReadResult
                    {
                        Page = 1,
                        Lines = new List<Line>
                        {
                            new Line
                            {
                                Text = "Sample text",
                                BoundingBox = new List<double?> { 10, 20, 100, 20, 100, 40, 10, 40 }
                            }
                        }
                    }
                }
            }
        };

        _mockClient.Setup(x => x.ReadInStreamAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReadInStreamHeaders { OperationLocation = $"https://test.com/operations/{operationId}" });
        
        _mockClient.Setup(x => x.GetReadResultAsync(Guid.Parse(operationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(readResult);

        // Act
        var result = await _service.ProcessImageAsync(imageStream, new OCRRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Text.Should().Contain("Sample text");
        result.Confidence.Should().BeGreaterThan(0);
        result.TextBlocks.Should().HaveCount(1);
        result.TextBlocks[0].Text.Should().Be("Sample text");
        result.TextBlocks[0].Confidence.Should().Be(0.9);
        result.TextBlocks[0].BoundingBox.Should().NotBeNull();
        result.TextBlocks[0].BoundingBox.X.Should().Be(10);
        result.TextBlocks[0].BoundingBox.Y.Should().Be(20);
        result.TextBlocks[0].BoundingBox.Width.Should().Be(90);
        result.TextBlocks[0].BoundingBox.Height.Should().Be(20);
    }

    [Test]
    public async Task ProcessImageAsync_WithEmptyImage_ShouldReturnEmptyResult()
    {
        // Arrange
        var imageStream = new MemoryStream();
        var operationId = "test-operation-id";
        
        var readResult = new ReadOperationResult
        {
            Status = OperationStatusCodes.Succeeded,
            AnalyzeResult = new AnalyzeResults
            {
                ReadResults = new List<ReadResult>()
            }
        };

        _mockClient.Setup(x => x.ReadInStreamAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReadInStreamHeaders { OperationLocation = $"https://test.com/operations/{operationId}" });
        
        _mockClient.Setup(x => x.GetReadResultAsync(Guid.Parse(operationId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(readResult);

        // Act
        var result = await _service.ProcessImageAsync(imageStream, new OCRRequest(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Text.Should().BeEmpty();
        result.TextBlocks.Should().BeEmpty();
    }

    [Test]
    public async Task ProcessImageAsync_WhenServiceFails_ShouldThrowException()
    {
        // Arrange
        var imageStream = new MemoryStream(Encoding.UTF8.GetBytes("test image"));
        
        _mockClient.Setup(x => x.ReadInStreamAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _service.ProcessImageAsync(imageStream));
    }
}
