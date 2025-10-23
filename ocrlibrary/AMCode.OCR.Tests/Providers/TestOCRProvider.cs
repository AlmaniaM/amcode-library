using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using AMCode.OCR.Models;
using AMCode.OCR.Providers;

namespace AMCode.OCR.Tests.Providers;

/// <summary>
/// Test implementation of OCR provider for unit testing
/// </summary>
public class TestOCRProvider : GenericOCRProvider
{
    public TestOCRProvider(ILogger logger, IHttpClientFactory httpClientFactory) 
        : base(logger, httpClientFactory)
    {
    }

    public override string ProviderName => "Test OCR Provider";

    public override bool RequiresInternet => false;

    public override OCRProviderCapabilities Capabilities => new OCRProviderCapabilities
    {
        SupportsLanguageDetection = false,
        SupportsBoundingBoxes = false,
        SupportsConfidenceScores = true,
        SupportsHandwriting = false,
        SupportsPrintedText = true,
        SupportsTableDetection = false,
        SupportsFormDetection = false,
        MaxImageSizeMB = 10,
        SupportedLanguages = new[] { "en" },
        CostPerRequest = 0,
        AverageResponseTime = TimeSpan.FromMilliseconds(100),
        SupportsCustomModels = false,
        MaxPagesPerRequest = 1,
        SupportsBatchProcessing = true,
        SupportsRealTimeProcessing = true,
        SupportsImagePreprocessing = false,
        SupportsRotationDetection = false,
        SupportsSkewDetection = false
    };

    public override Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default)
    {
        return ProcessImageAsync(imageStream, new OCRRequest(), cancellationToken);
    }

    public override async Task<OCRResult> ProcessImageAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        
        // Simulate processing delay
        await Task.Delay(50, cancellationToken);
        
        var processingTime = DateTime.UtcNow - startTime;
        
        // Create a simple test result
        var textBlocks = new List<TextBlock>
        {
            new TextBlock
            {
                Text = "Test extracted text",
                Confidence = 0.9,
                Language = "en",
                IsHandwritten = false,
                IsPrinted = true
            }
        };
        
        return CreateOCRResult("Test extracted text", processingTime, textBlocks);
    }

    public override async Task<OCRProviderHealth> CheckHealthAsync()
    {
        await Task.Delay(10); // Simulate health check delay
        
        return new OCRProviderHealth
        {
            IsHealthy = true,
            IsAvailable = true,
            Status = "Healthy",
            LastChecked = DateTime.UtcNow,
            SuccessRate = 100.0,
            AverageProcessingTime = TimeSpan.FromMilliseconds(100),
            Metrics = new Dictionary<string, object>
            {
                ["Provider"] = ProviderName,
                ["TestMode"] = true
            }
        };
    }

    public override Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null)
    {
        return Task.FromResult(0m);
    }

    protected override bool CheckAvailability()
    {
        return true;
    }
}
