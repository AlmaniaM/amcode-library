# AMCode.OCR - Multi-Cloud OCR Service Library

## 🎯 Overview

AMCode.OCR is a comprehensive, multi-cloud OCR (Optical Character Recognition) service library that provides intelligent text extraction from images using multiple cloud providers. The library features smart provider selection, automatic fallback mechanisms, and comprehensive error handling.

## ✨ Features

- **Multi-Cloud Support**: Azure Computer Vision, AWS Textract, and Google Cloud Vision
- **Smart Provider Selection**: Automatic selection of the best provider based on image characteristics
- **Fallback Mechanisms**: Automatic failover when primary provider fails
- **Confidence Scoring**: OCR result quality assessment
- **Cost Optimization**: Smart cost analysis and provider selection
- **Batch Processing**: Process multiple images efficiently
- **Comprehensive Error Handling**: Robust error handling and recovery
- **High Performance**: Optimized for speed and reliability
- **Easy Integration**: Simple dependency injection setup

## 🚀 Quick Start

### Installation

```bash
dotnet add package AMCode.OCR --version 1.0.0
```

### Basic Usage

```csharp
using AMCode.OCR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Register services
var services = new ServiceCollection();
services.AddAMCodeOCR(configuration);

var serviceProvider = services.BuildServiceProvider();
var ocrService = serviceProvider.GetRequiredService<IOCRService>();

// Extract text from image
using var imageStream = File.OpenRead("image.jpg");
var result = await ocrService.ExtractTextAsync(imageStream);

if (result.IsSuccess)
{
    Console.WriteLine($"Extracted text: {result.Value.Text}");
    Console.WriteLine($"Confidence: {result.Value.Confidence}");
}
```

## 🔧 Configuration

### appsettings.json

```json
{
  "AMCode": {
    "OCR": {
      "DefaultProvider": "Azure",
      "FallbackProviders": ["AWS", "Google"],
      "ConfidenceThreshold": 0.7,
      "MaxRetries": 3,
      "EnableFallbackProviders": true,
      "MaxFallbackProviders": 2,
      "EnableBatchProcessing": true,
      "MaxBatchSize": 10,
      "Azure": {
        "SubscriptionKey": "your-azure-key",
        "Endpoint": "https://your-endpoint.cognitiveservices.azure.com/",
        "Region": "eastus",
        "CostPerRequest": 0.001
      },
      "AWS": {
        "Region": "us-east-1",
        "AccessKey": "your-access-key",
        "SecretKey": "your-secret-key",
        "CostPerRequest": 0.0015
      },
      "Google": {
        "ProjectId": "your-project-id",
        "CredentialsPath": "path/to/credentials.json",
        "CostPerRequest": 0.0008
      }
    }
  }
}
```

### Programmatic Configuration

```csharp
services.Configure<OCRConfiguration>(options =>
{
    options.DefaultProvider = "Azure";
    options.FallbackProviders = new[] { "AWS", "Google" };
    options.ConfidenceThreshold = 0.8;
    options.MaxRetries = 3;
    options.EnableFallbackProviders = true;
});
```

## 📚 API Reference

### IOCRService

The main interface for OCR operations.

#### Methods

- `ExtractTextAsync(Stream imageStream, CancellationToken cancellationToken = default)`
- `ExtractTextAsync(string imagePath, CancellationToken cancellationToken = default)`
- `ExtractTextAsync(Stream imageStream, OCRRequest options, CancellationToken cancellationToken = default)`
- `ExtractTextAsync(string imagePath, OCRRequest options, CancellationToken cancellationToken = default)`
- `ProcessBatchAsync(IEnumerable<Stream> imageStreams, OCRRequest? options = null, CancellationToken cancellationToken = default)`
- `ProcessBatchAsync(IEnumerable<string> imagePaths, OCRRequest? options = null, CancellationToken cancellationToken = default)`
- `IsAvailableAsync()`
- `GetHealthAsync()`
- `GetCapabilities()`
- `GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null)`

### OCRRequest

Configuration options for OCR processing.

```csharp
public class OCRRequest
{
    public string? ImagePath { get; set; }
    public Stream? ImageStream { get; set; }
    public double ConfidenceThreshold { get; set; } = 0.7;
    public int MaxRetries { get; set; } = 3;
    public bool RequiresLanguageDetection { get; set; } = false;
    public bool RequiresBoundingBoxes { get; set; } = false;
    public string[]? PreferredLanguages { get; set; }
    public long MaxImageSizeMB { get; set; } = 10;
    public bool EnablePreprocessing { get; set; } = true;
}
```

### OCRResult

The result of OCR processing.

```csharp
public class OCRResult
{
    public string Text { get; set; } = string.Empty;
    public double Confidence { get; set; }
    public string Language { get; set; } = "en";
    public string Provider { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public TimeSpan ProcessingTime { get; set; }
    public List<TextBlock> TextBlocks { get; set; } = new();
    public Dictionary<string, object> Metadata { get; set; } = new();
}
```

## 🔄 Provider Selection

The library automatically selects the best OCR provider based on:

1. **Image Characteristics**: Size, format, complexity
2. **Provider Availability**: Health status and response times
3. **Cost Optimization**: Cost per request and processing time
4. **Capabilities**: Language support, feature requirements

### Manual Provider Selection

```csharp
// Get available providers
var availableProviders = await providerSelector.GetAvailableProvidersAsync();

// Select specific provider
var azureProvider = availableProviders.First(p => p.ProviderName == "Azure Computer Vision");
var result = await azureProvider.ProcessImageAsync(imageStream);
```

## 🧪 Testing

### Unit Tests

The library includes comprehensive unit tests with 90%+ coverage:

```bash
dotnet test AMCode.OCR.Tests
```

### Integration Tests

Test with actual cloud providers:

```csharp
[TestMethod]
public async Task IntegrationTest_WithAzureProvider()
{
    // Configure with real Azure credentials
    var config = new OCRConfiguration
    {
        Azure = new AzureOCRConfiguration
        {
            SubscriptionKey = "your-key",
            Endpoint = "https://your-endpoint.cognitiveservices.azure.com/"
        }
    };
    
    var service = new EnhancedHybridOCRService(providers, selector, logger, Options.Create(config));
    var result = await service.ExtractTextAsync(imageStream);
    
    Assert.IsTrue(result.IsSuccess);
}
```

## 📊 Performance

### Benchmarks

- **Azure Computer Vision**: ~2-3 seconds per image
- **AWS Textract**: ~3-4 seconds per image
- **Google Cloud Vision**: ~2-3 seconds per image
- **Batch Processing**: Up to 10 images per batch
- **Memory Usage**: Optimized for minimal memory footprint

### Optimization Tips

1. **Image Preprocessing**: Enable automatic image optimization
2. **Batch Processing**: Use batch methods for multiple images
3. **Provider Selection**: Let the library choose the optimal provider
4. **Caching**: Cache results for repeated images

## 🔒 Security

### API Key Management

- Store API keys in secure configuration
- Use environment variables for production
- Rotate keys regularly
- Monitor usage and costs

### Data Privacy

- Images are processed in memory only
- No persistent storage of image data
- Secure transmission to cloud providers
- Compliance with data protection regulations

## 🚨 Error Handling

### Common Errors

- **Provider Unavailable**: Automatic fallback to alternative providers
- **Low Confidence**: Retry with different provider or preprocessing
- **Rate Limiting**: Automatic retry with exponential backoff
- **Invalid Image**: Clear error messages and validation

### Error Recovery

```csharp
try
{
    var result = await ocrService.ExtractTextAsync(imageStream);
    if (!result.IsSuccess)
    {
        // Handle error
        Console.WriteLine($"OCR failed: {result.Error}");
    }
}
catch (OCRException ex)
{
    // Handle OCR-specific exceptions
    Console.WriteLine($"OCR error: {ex.Message}");
}
```

## 📈 Monitoring

### Health Checks

```csharp
var health = await ocrService.GetHealthAsync();
Console.WriteLine($"Service Status: {health.Status}");
Console.WriteLine($"Success Rate: {health.SuccessRate}%");
```

### Metrics

- Processing time per image
- Success rate by provider
- Cost per request
- Error rates and types

## 🔧 Advanced Usage

### Custom Provider

```csharp
public class CustomOCRProvider : IOCRProvider
{
    public string ProviderName => "Custom Provider";
    public bool RequiresInternet => true;
    public bool IsAvailable => true;
    public OCRProviderCapabilities Capabilities => new OCRProviderCapabilities { /* ... */ };

    public async Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default)
    {
        // Custom OCR implementation
    }
}
```

### Custom Configuration

```csharp
services.Configure<OCRConfiguration>(config =>
{
    config.EnableCustomProviders = true;
    config.CustomProviders = new[] { typeof(CustomOCRProvider) };
});
```

## 📝 Examples

### Recipe OCR Application

```csharp
public class RecipeOCRService
{
    private readonly IOCRService _ocrService;

    public RecipeOCRService(IOCRService ocrService)
    {
        _ocrService = ocrService;
    }

    public async Task<Recipe> ExtractRecipeFromImageAsync(string imagePath)
    {
        var result = await _ocrService.ExtractTextAsync(imagePath);
        
        if (!result.IsSuccess)
            throw new OCRException($"Failed to extract text: {result.Error}");

        // Parse recipe from extracted text
        return ParseRecipe(result.Value.Text);
    }
}
```

### Batch Processing

```csharp
public async Task<List<Document>> ProcessDocumentBatchAsync(List<string> imagePaths)
{
    var results = await _ocrService.ProcessBatchAsync(imagePaths);
    
    if (!results.IsSuccess)
        throw new OCRException($"Batch processing failed: {results.Error}");

    return results.Value.Select(r => new Document
    {
        Text = r.Text,
        Confidence = r.Confidence,
        ProcessingTime = r.ProcessingTime
    }).ToList();
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:

- Create an issue on GitHub
- Check the documentation
- Review the examples
- Contact the development team

## 🔄 Version History

### Version 1.0.0
- Initial release
- Multi-cloud OCR support
- Smart provider selection
- Comprehensive error handling
- Batch processing
- High test coverage

---

**AMCode.OCR** - Making OCR simple, reliable, and cost-effective across multiple cloud providers.
