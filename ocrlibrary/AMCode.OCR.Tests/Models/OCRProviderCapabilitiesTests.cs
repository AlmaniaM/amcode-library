using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using AMCode.OCR.Models;

namespace AMCode.OCR.Tests.Models;

[TestClass]
public class OCRProviderCapabilitiesTests
{
    [TestMethod]
    public void OCRProviderCapabilities_ShouldInitializeWithDefaultValues()
    {
        // Act
        var capabilities = new OCRProviderCapabilities();
        
        // Assert
        capabilities.SupportsLanguageDetection.Should().BeFalse();
        capabilities.SupportsBoundingBoxes.Should().BeFalse();
        capabilities.SupportsConfidenceScores.Should().BeFalse();
        capabilities.SupportsHandwriting.Should().BeFalse();
        capabilities.SupportsPrintedText.Should().BeFalse();
        capabilities.MaxImageSizeMB.Should().Be(0);
        capabilities.SupportedLanguages.Should().NotBeNull();
        capabilities.SupportedLanguages.Should().BeEmpty();
        capabilities.CostPerRequest.Should().Be(0);
        capabilities.AverageResponseTime.Should().Be(TimeSpan.Zero);
    }
    
    [TestMethod]
    public void OCRProviderCapabilities_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var supportedLanguages = new[] { "en", "es", "fr" };
        var averageResponseTime = TimeSpan.FromSeconds(2.5);
        
        // Act
        var capabilities = new OCRProviderCapabilities
        {
            SupportsLanguageDetection = true,
            SupportsBoundingBoxes = true,
            SupportsConfidenceScores = true,
            SupportsHandwriting = true,
            SupportsPrintedText = true,
            MaxImageSizeMB = 50,
            SupportedLanguages = supportedLanguages,
            CostPerRequest = 0.001m,
            AverageResponseTime = averageResponseTime
        };
        
        // Assert
        capabilities.SupportsLanguageDetection.Should().BeTrue();
        capabilities.SupportsBoundingBoxes.Should().BeTrue();
        capabilities.SupportsConfidenceScores.Should().BeTrue();
        capabilities.SupportsHandwriting.Should().BeTrue();
        capabilities.SupportsPrintedText.Should().BeTrue();
        capabilities.MaxImageSizeMB.Should().Be(50);
        capabilities.SupportedLanguages.Should().BeEquivalentTo(supportedLanguages);
        capabilities.CostPerRequest.Should().Be(0.001m);
        capabilities.AverageResponseTime.Should().Be(averageResponseTime);
    }
}
