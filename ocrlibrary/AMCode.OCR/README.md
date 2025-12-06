# AMCode.OCR

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Multi-cloud OCR service library with smart provider selection, fallback mechanisms, and cost optimization

---

## Overview

AMCode.OCR is a comprehensive optical character recognition (OCR) library that provides a unified interface for multiple cloud OCR providers. It supports Google Cloud Vision, Azure Computer Vision, AWS Textract, PaddleOCR, and other providers with intelligent provider selection, automatic fallback, cost optimization, and health monitoring.

The library abstracts away provider-specific implementations, allowing applications to switch between providers seamlessly or use multiple providers with automatic failover. It includes smart selection strategies based on cost, performance, reliability, quality, or a balanced combination of factors.

## Architecture

The library follows Clean Architecture principles with a multi-provider pattern:

- **Provider Abstraction**: `IOCRProvider` interface defines the contract for all OCR providers
- **Service Layer**: `IOCRService` provides high-level OCR operations with automatic provider selection
- **Smart Selection**: Multiple selection strategies (cost, performance, reliability, quality, balanced)
- **Fallback Mechanism**: Automatic fallback to alternative providers on failure
- **Health Monitoring**: Provider health checking and availability tracking
- **Cost Analysis**: Cost estimation and optimization across providers

### Key Components

- **Providers**: Cloud OCR provider implementations (Google, Azure, AWS, PaddleOCR, Anthropic, AWS Bedrock)
- **Services**: OCR service implementations with smart provider selection
- **Models**: OCR request/response models and data structures
- **Configurations**: Provider configuration classes
- **Factories**: Provider and selector factory implementations
- **Enums**: Selection strategies and provider types

## Features

- **Multi-Provider Support**
  - Google Cloud Vision API
  - Azure Computer Vision
  - AWS Textract
  - PaddleOCR (Python service integration)
  - Anthropic OCR (via Claude)
  - AWS Bedrock OCR

- **Smart Provider Selection**
  - Cost-optimized selection
  - Performance-optimized selection
  - Reliability-optimized selection
  - Quality-optimized selection
  - Balanced selection (multiple factors)
  - Load-balanced selection
  - Geographic-optimized selection
  - Configuration-based selection

- **Advanced Features**
  - Automatic provider fallback on failure
  - Health monitoring and availability tracking
  - Cost estimation and optimization
  - Batch processing support
  - Language detection
  - Handwriting recognition
  - Table and form detection
  - Confidence scoring
  - Text block extraction with bounding boxes

- **Developer Experience**
  - Simple, unified API across all providers
  - Dependency injection support
  - Comprehensive logging
  - Result wrapper with error handling
  - Configuration via appsettings.json

## Dependencies

### Internal Dependencies

- None (standalone library)

### External Dependencies

- **Microsoft.Azure.CognitiveServices.Vision.ComputerVision** (7.0.0) - Azure Computer Vision OCR
- **AWSSDK.Textract** (3.7.400.0) - AWS Textract OCR
- **Google.Cloud.Vision.V1** (3.7.0) - Google Cloud Vision OCR
- **Microsoft.Extensions.Logging.Abstractions** (8.0.0) - Logging infrastructure
- **Microsoft.Extensions.Http** (8.0.0) - HTTP client factory
- **Microsoft.Extensions.Configuration.Abstractions** (8.0.0) - Configuration support
- **Microsoft.Extensions.Options** (8.0.0) - Options pattern
- **System.Text.Json** (8.0.5) - JSON serialization

## Project Structure

```
AMCode.OCR/
├── Configurations/              # Provider configuration classes
│   ├── AzureOCRConfiguration.cs
│   ├── AWSTextractConfiguration.cs
│   ├── GoogleVisionConfiguration.cs
│   ├── PaddleOCRConfiguration.cs
│   ├── OCRConfiguration.cs
│   └── OCRProviderRegistry.cs
├── Providers/                    # OCR provider implementations
│   ├── AzureComputerVisionOCRService.cs
│   ├── AWSTextractOCRService.cs
│   ├── GoogleCloudVisionOCRService.cs
│   ├── PaddleOCRProvider.cs
│   ├── AnthropicOCRService.cs
│   ├── AWSBedrockOCRService.cs
│   └── GenericOCRProvider.cs    # Base class for providers
├── Services/                     # OCR service implementations
│   ├── EnhancedHybridOCRService.cs
│   ├── SmartOCRProviderSelector.cs
│   ├── ConfigurationOCRProviderSelector.cs
│   └── CostAnalyzer.cs
├── Models/                       # OCR models and data structures
│   ├── OCRResult.cs
│   ├── OCRRequest.cs
│   ├── TextBlock.cs
│   ├── BoundingBox.cs
│   ├── OCRProviderCapabilities.cs
│   └── OCRProviderHealth.cs
├── Factories/                    # Factory implementations
│   ├── OCRProviderFactory.cs
│   ├── IOCRProviderFactory.cs
│   ├── OCRProviderSelectorFactory.cs
│   └── IOCRProviderSelectorFactory.cs
├── Enums/                        # Enumerations
│   └── OCRProviderSelectionStrategy.cs
├── Extensions/                   # Extension methods
│   └── OCRServiceCollectionExtensions.cs
├── Examples/                     # Usage examples
│   ├── OCRExample.cs
│   └── appsettings.example.json
├── IOCRProvider.cs              # Provider interface
├── IOCRService.cs                # Service interface
├── INTEGRATION_GUIDE.md         # Integration documentation
└── AMCode.OCR.csproj
```

## Key Interfaces

### IOCRProvider

**Location:** `IOCRProvider.cs`

**Purpose:** Interface defining the contract for all OCR provider implementations.

**Key Methods:**

- `ProcessImageAsync(Stream, CancellationToken)` - Process image and extract text
- `ProcessImageAsync(Stream, OCRRequest, CancellationToken)` - Process with custom options
- `CheckHealthAsync()` - Check provider health status
- `GetCostEstimateAsync(long, OCRRequest?)` - Get cost estimate
- `ProcessBatchAsync(IEnumerable<Stream>, OCRRequest?, CancellationToken)` - Batch processing
- `CanProcess(OCRRequest)` - Validate if provider can handle request
- `GetEstimatedProcessingTime(long, OCRRequest?)` - Get processing time estimate
- `GetReliabilityScore()` - Get reliability score (0.0-1.0)
- `GetQualityScore()` - Get quality score (0.0-1.0)

**Key Properties:**

- `ProviderName` - Name of the provider
- `RequiresInternet` - Whether provider needs internet connection
- `IsAvailable` - Current availability status
- `Capabilities` - Provider capabilities and features

**See Also:** [Providers README](Providers/README.md)

### IOCRService

**Location:** `IOCRService.cs`

**Purpose:** High-level interface for OCR operations with automatic provider selection.

**Key Methods:**

- `ExtractTextAsync(Stream, CancellationToken)` - Extract text from image stream
- `ExtractTextAsync(string, CancellationToken)` - Extract text from image file
- `ExtractTextAsync(Stream, OCRRequest, CancellationToken)` - Extract with custom options
- `ExtractTextAsync(string, OCRRequest, CancellationToken)` - Extract from file with options
- `IsAvailableAsync()` - Check service availability
- `GetHealthAsync()` - Get health status
- `GetCapabilities()` - Get service capabilities
- `GetCostEstimateAsync(long, OCRRequest?)` - Get cost estimate
- `ProcessBatchAsync(IEnumerable<Stream>, OCRRequest?, CancellationToken)` - Batch processing
- `ProcessBatchAsync(IEnumerable<string>, OCRRequest?, CancellationToken)` - Batch from files

**See Also:** [Services README](Services/README.md)

## Key Classes

### EnhancedHybridOCRService

**Location:** `Services/EnhancedHybridOCRService.cs`

**Purpose:** Main OCR service implementation with smart provider selection and automatic fallback.

**Key Responsibilities:**

- Automatic provider selection based on strategy
- Fallback to alternative providers on failure
- Health monitoring and provider availability tracking
- Cost optimization across providers
- Batch processing with error handling

**Usage Example:**

```csharp
using AMCode.OCR;
using AMCode.OCR.Models;

var ocrService = serviceProvider.GetRequiredService<IOCRService>();

// Extract text from image file
var result = await ocrService.ExtractTextAsync("image.jpg");
if (result.IsSuccess)
{
    Console.WriteLine($"Extracted text: {result.Value.Text}");
    Console.WriteLine($"Confidence: {result.Value.Confidence}");
    Console.WriteLine($"Provider: {result.Value.Provider}");
}
```

### SmartOCRProviderSelector

**Location:** `Services/SmartOCRProviderSelector.cs`

**Purpose:** Intelligent provider selection based on multiple strategies.

**Key Responsibilities:**

- Select best provider based on strategy (cost, performance, reliability, quality, balanced)
- Health checking and availability filtering
- Cost estimation across providers
- Provider capability matching

**Selection Strategies:**

- `CostOptimized` - Lowest cost provider
- `PerformanceOptimized` - Fastest provider
- `ReliabilityOptimized` - Most reliable provider
- `QualityOptimized` - Highest quality provider
- `Balanced` - Best balance of all factors
- `LoadBalanced` - Distribute load across providers
- `GeographicOptimized` - Geographic proximity
- `Configuration` - Manual configuration

### GenericOCRProvider

**Location:** `Providers/GenericOCRProvider.cs`

**Purpose:** Base abstract class for OCR provider implementations.

**Key Responsibilities:**

- Common provider functionality
- HTTP client management
- JSON serialization/deserialization
- Language detection
- Confidence calculation
- Batch processing support

**See Also:** [Providers README](Providers/README.md)

### OCRResult

**Location:** `Models/OCRResult.cs`

**Purpose:** Result model containing extracted text and metadata.

**Key Properties:**

- `Text` - Extracted text content
- `TextBlocks` - Individual text blocks with positions
- `Confidence` - Overall confidence score (0.0-1.0)
- `Language` - Detected language code
- `Provider` - Provider that processed the image
- `ProcessingTime` - Time taken to process
- `Cost` - Cost of the operation
- `ContainsHandwriting` - Whether handwriting was detected
- `ContainsPrintedText` - Whether printed text was detected

### OCRRequest

**Location:** `Models/OCRRequest.cs`

**Purpose:** Request model for OCR processing options.

**Key Properties:**

- `ImageStream` - Image stream to process
- `ImagePath` - Path to image file
- `RequiresLanguageDetection` - Enable language detection
- `RequiresHandwritingSupport` - Enable handwriting recognition
- `RequiresTableDetection` - Enable table detection
- `RequiresFormDetection` - Enable form detection
- `ConfidenceThreshold` - Minimum confidence threshold
- `MaxRetries` - Maximum retry attempts
- `Timeout` - Request timeout
- `MaxImageSizeMB` - Maximum image size

## Usage Examples

### Basic Usage

```csharp
using AMCode.OCR;
using AMCode.OCR.Models;

// Register services
services.AddAMCodeOCR(configuration);

// Use service
var ocrService = serviceProvider.GetRequiredService<IOCRService>();

// Extract text from image file
var result = await ocrService.ExtractTextAsync("recipe-image.jpg");
if (result.IsSuccess)
{
    var ocrResult = result.Value;
    Console.WriteLine($"Text: {ocrResult.Text}");
    Console.WriteLine($"Confidence: {ocrResult.Confidence:P}");
    Console.WriteLine($"Provider: {ocrResult.Provider}");
}
```

### Advanced Usage with Options

```csharp
using AMCode.OCR;
using AMCode.OCR.Models;

var request = new OCRRequest
{
    ImagePath = "recipe-image.jpg",
    RequiresLanguageDetection = true,
    RequiresHandwritingSupport = true,
    ConfidenceThreshold = 0.8,
    MaxRetries = 3,
    ReturnDetailedTextBlocks = true,
    ReturnBoundingBoxes = true
};

var result = await ocrService.ExtractTextAsync(request);
if (result.IsSuccess)
{
    var ocrResult = result.Value;
    
    // Access text blocks with positions
    foreach (var block in ocrResult.TextBlocks)
    {
        Console.WriteLine($"Text: {block.Text}");
        Console.WriteLine($"Position: {block.BoundingBox}");
        Console.WriteLine($"Confidence: {block.Confidence}");
    }
}
```

### Batch Processing

```csharp
var imagePaths = new[] { "image1.jpg", "image2.jpg", "image3.jpg" };
var result = await ocrService.ProcessBatchAsync(imagePaths);

if (result.IsSuccess)
{
    foreach (var ocrResult in result.Value)
    {
        Console.WriteLine($"Provider: {ocrResult.Provider}");
        Console.WriteLine($"Text: {ocrResult.Text}");
        Console.WriteLine($"Cost: ${ocrResult.Cost}");
    }
}
```

### Direct Provider Usage

```csharp
using AMCode.OCR.Providers;

var provider = serviceProvider.GetRequiredService<IOCRProvider>();
var result = await provider.ProcessImageAsync(imageStream);

Console.WriteLine($"Text: {result.Text}");
Console.WriteLine($"Provider: {result.Provider}");
```

## Configuration

### appsettings.json Example

```json
{
  "OCR": {
    "DefaultProvider": "PaddleOCR",
    "SelectionStrategy": "Balanced",
    "DefaultConfidenceThreshold": 0.7,
    "MaxRetries": 3,
    "Providers": {
      "Azure": {
        "Endpoint": "https://your-resource.cognitiveservices.azure.com/",
        "ApiKey": "your-api-key"
      },
      "Google": {
        "ProjectId": "your-project-id",
        "CredentialsPath": "path/to/credentials.json"
      },
      "AWS": {
        "Region": "us-east-1",
        "AccessKeyId": "your-access-key",
        "SecretAccessKey": "your-secret-key"
      },
      "PaddleOCR": {
        "ServiceUrl": "http://localhost:8000",
        "Timeout": "00:05:00"
      }
    }
  }
}
```

### Dependency Injection Setup

```csharp
using AMCode.OCR;
using Microsoft.Extensions.DependencyInjection;

public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Add AMCode.OCR services
    services.AddAMCodeOCR(configuration);
    
    // Or configure manually
    services.AddAMCodeOCR(options =>
    {
        options.DefaultProvider = "PaddleOCR";
        options.SelectionStrategy = OCRProviderSelectionStrategy.Balanced;
        options.DefaultConfidenceThreshold = 0.7;
    });
}
```

## Testing

### Test Projects

- **AMCode.OCR.Tests**: Unit and integration tests for OCR library
  - Provider tests
  - Service tests
  - Selection strategy tests
  - [Test Project README](../AMCode.OCR.Tests/README.md)

### Running Tests

```bash
dotnet test ocrlibrary/AMCode.OCR.Tests
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Providers](Providers/README.md) - OCR provider implementations
- [Services](Services/README.md) - OCR service implementations and selectors
- [Models](Models/README.md) - OCR models and data structures
- [Configurations](Configurations/README.md) - Provider configuration classes
- [Factories](Factories/README.md) - Factory implementations
- [Enums](Enums/README.md) - Enumerations and selection strategies

## Related Libraries

- None (standalone library)

## Migration Notes

- **.NET 8.0**: This library targets .NET 8.0. Ensure compatibility with your application framework.
- **Provider Configuration**: All providers require proper configuration in appsettings.json or via dependency injection.
- **PaddleOCR**: Requires separate Python service to be running (see INTEGRATION_GUIDE.md).
- **Result Pattern**: All service methods return `Result<T>` for error handling.

## Known Issues

- PaddleOCR requires external Python service
- Some providers may have rate limits
- Cost estimation is approximate and may vary
- Health checks may impact performance if called frequently

## Future Considerations

- Additional OCR provider support
- Enhanced image preprocessing
- OCR result caching
- Real-time OCR streaming
- Custom provider plugin system

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Integration Guide](INTEGRATION_GUIDE.md) - Detailed integration instructions
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
