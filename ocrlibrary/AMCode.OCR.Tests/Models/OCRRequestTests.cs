using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using AMCode.OCR.Models;

namespace AMCode.OCR.Tests.Models;

[TestClass]
public class OCRRequestTests
{
    [TestMethod]
    public void OCRRequest_ShouldInitializeWithDefaultValues()
    {
        // Act
        var request = new OCRRequest();
        
        // Assert
        request.ImageStream.Should().BeNull();
        request.RequiresLanguageDetection.Should().BeFalse();
        request.RequiresHandwritingSupport.Should().BeFalse();
        request.MaxRetries.Should().Be(3);
        request.ConfidenceThreshold.Should().Be(0.5);
        request.PreferredLanguage.Should().BeNull();
        request.MaxCostPerRequest.Should().Be(1.0m);
    }
    
    [TestMethod]
    public void OCRRequest_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var imageStream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
        var requiresLanguageDetection = false;
        var requiresHandwritingSupport = true;
        var maxRetries = 5;
        var confidenceThreshold = 0.8;
        var preferredLanguage = "es";
        var maxCostPerRequest = 0.01m;
        
        // Act
        var request = new OCRRequest
        {
            ImageStream = imageStream,
            RequiresLanguageDetection = requiresLanguageDetection,
            RequiresHandwritingSupport = requiresHandwritingSupport,
            MaxRetries = maxRetries,
            ConfidenceThreshold = confidenceThreshold,
            PreferredLanguage = preferredLanguage,
            MaxCostPerRequest = maxCostPerRequest
        };
        
        // Assert
        request.ImageStream.Should().BeSameAs(imageStream);
        request.RequiresLanguageDetection.Should().Be(requiresLanguageDetection);
        request.RequiresHandwritingSupport.Should().Be(requiresHandwritingSupport);
        request.MaxRetries.Should().Be(maxRetries);
        request.ConfidenceThreshold.Should().Be(confidenceThreshold);
        request.PreferredLanguage.Should().Be(preferredLanguage);
        request.MaxCostPerRequest.Should().Be(maxCostPerRequest);
    }
}
