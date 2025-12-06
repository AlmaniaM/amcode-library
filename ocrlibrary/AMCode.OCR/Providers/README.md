# Providers

**Location:** `AMCode.OCR/Providers/`
**Last Updated:** 2025-01-27
**Purpose:** OCR provider implementations for various cloud services and local OCR solutions

---

## Overview

The Providers subfolder contains implementations of the `IOCRProvider` interface for different OCR services. Each provider encapsulates the logic for communicating with a specific OCR service (Azure, AWS, Google Cloud, PaddleOCR, etc.) and converting their responses to the standardized `OCRResult` format.

## Responsibilities

- Implement `IOCRProvider` interface for each OCR service
- Handle provider-specific API communication
- Convert provider responses to standardized `OCRResult` format
- Manage provider health and availability
- Provide cost estimates and processing time estimates
- Support batch processing where available

## Class Catalog

### Classes

#### AzureComputerVisionOCRService

**File:** `AzureComputerVisionOCRService.cs`

**Purpose:** Azure Computer Vision OCR provider implementation using Microsoft Azure Cognitive Services.

**Key Responsibilities:**

- Communicate with Azure Computer Vision API
- Convert Azure responses to `OCRResult` format
- Handle Azure-specific authentication and errors
- Provide health monitoring for Azure service

**Key Members:**

```csharp
public class AzureComputerVisionOCRService : IOCRProvider
{
    public string ProviderName => "Azure Computer Vision";
    public bool RequiresInternet => true;
    public bool IsAvailable { get; }
    public OCRProviderCapabilities Capabilities { get; }

    public Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default);
    public Task<OCRProviderHealth> CheckHealthAsync();
    public Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null);
}
```

**Dependencies:**

- `Microsoft.Azure.CognitiveServices.Vision.ComputerVision` - Azure Computer Vision SDK
- `Microsoft.Extensions.Logging` - Logging
- `Microsoft.Extensions.Options` - Configuration

**Usage:**

```csharp
var azureProvider = new AzureComputerVisionOCRService(client, logger, config);
var result = await azureProvider.ProcessImageAsync(imageStream);
```

---

#### AWSTextractOCRService

**File:** `AWSTextractOCRService.cs`

**Purpose:** AWS Textract OCR provider implementation using Amazon Textract service.

**Key Responsibilities:**

- Communicate with AWS Textract API
- Convert AWS Textract responses to `OCRResult` format
- Handle AWS-specific authentication (IAM credentials)
- Support AWS Textract features (tables, forms, handwriting)

**Key Members:**

```csharp
public class AWSTextractOCRService : IOCRProvider
{
    public string ProviderName => "AWS Textract";
    public bool RequiresInternet => true;
    public Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default);
}
```

**Dependencies:**

- `AWSSDK.Textract` - AWS Textract SDK

---

#### GoogleCloudVisionOCRService

**File:** `GoogleCloudVisionOCRService.cs`

**Purpose:** Google Cloud Vision OCR provider implementation using Google Cloud Vision API.

**Key Responsibilities:**

- Communicate with Google Cloud Vision API
- Convert Google Vision responses to `OCRResult` format
- Handle Google Cloud authentication (service account credentials)
- Support Google Vision features (language detection, handwriting)

**Key Members:**

```csharp
public class GoogleCloudVisionOCRService : IOCRProvider
{
    public string ProviderName => "Google Cloud Vision";
    public bool RequiresInternet => true;
    public Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default);
}
```

**Dependencies:**

- `Google.Cloud.Vision.V1` - Google Cloud Vision SDK

---

#### PaddleOCRProvider

**File:** `PaddleOCRProvider.cs`

**Purpose:** PaddleOCR provider implementation that calls a Python OCR service via HTTP.

**Key Responsibilities:**

- Communicate with PaddleOCR Python service via HTTP
- Convert PaddleOCR responses to `OCRResult` format
- Handle HTTP communication and error handling
- Support local/on-premise OCR processing

**Key Members:**

```csharp
public class PaddleOCRProvider : GenericOCRProvider
{
    public override string ProviderName => "PaddleOCR";
    public override bool RequiresInternet => true;
    public override Task<OCRResult> ProcessImageAsync(Stream imageStream, CancellationToken cancellationToken = default);
}
```

**Dependencies:**

- `Microsoft.Extensions.Http` - HTTP client factory
- `PaddleOCRConfiguration` - PaddleOCR service configuration

**Usage:**

```csharp
var paddleProvider = new PaddleOCRProvider(logger, httpClientFactory, config);
var result = await paddleProvider.ProcessImageAsync(imageStream);
```

**Note:** PaddleOCR requires a separate Python service running. See integration guide for setup.

---

#### AnthropicOCRService

**File:** `AnthropicOCRService.cs`

**Purpose:** Anthropic OCR provider implementation (if supported).

**Key Responsibilities:**

- Communicate with Anthropic OCR API (if available)
- Convert Anthropic responses to `OCRResult` format

---

#### GenericOCRProvider

**File:** `GenericOCRProvider.cs`

**Purpose:** Base class for HTTP-based OCR providers with common functionality.

**Key Responsibilities:**

- Provide common HTTP communication logic
- Handle standard HTTP error scenarios
- Implement base provider functionality

**Usage:**

```csharp
public class CustomOCRProvider : GenericOCRProvider
{
    // Inherit common HTTP functionality
}
```

---

## Architecture Patterns

- **Provider Pattern**: Each provider implements `IOCRProvider` interface
- **Factory Pattern**: Providers are created via `OCRProviderFactory`
- **Strategy Pattern**: Different providers for different OCR strategies
- **Adapter Pattern**: Convert provider-specific responses to `OCRResult`

## Usage Patterns

### Pattern 1: Direct Provider Usage

```csharp
var provider = new AzureComputerVisionOCRService(client, logger, config);
var result = await provider.ProcessImageAsync(imageStream);
```

### Pattern 2: Provider via Factory

```csharp
var factory = serviceProvider.GetRequiredService<IOCRProviderFactory>();
var provider = factory.CreateProvider();
var result = await provider.ProcessImageAsync(imageStream);
```

### Pattern 3: Provider Selection

```csharp
var selector = serviceProvider.GetRequiredService<IOCRProviderSelector>();
var provider = await selector.SelectOCRProvider(request);
var result = await provider.ProcessImageAsync(imageStream);
```

## Dependencies

### Internal Dependencies

- `AMCode.OCR.Models` - OCR models (OCRResult, OCRRequest, etc.)
- `AMCode.OCR.Configurations` - Provider configurations

### External Dependencies

- `Microsoft.Azure.CognitiveServices.Vision.ComputerVision` (7.0.0) - Azure OCR
- `AWSSDK.Textract` (3.7.400.0) - AWS OCR
- `Google.Cloud.Vision.V1` (3.7.0) - Google OCR
- `Microsoft.Extensions.Logging.Abstractions` (8.0.0) - Logging
- `Microsoft.Extensions.Http` (8.0.0) - HTTP client
- `Microsoft.Extensions.Options` (8.0.0) - Configuration

## Related Components

### Within Same Library

- [Services](../Services/README.md) - Uses providers for OCR operations
- [Configurations](../Configurations/README.md) - Provider configuration classes
- [Factories](../Factories/README.md) - Provider factory for creation

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.OCR.Tests/Providers/`
- Integration tests: `AMCode.OCR.Tests/Integration/`

### Example Test

```csharp
[Test]
public async Task AzureProvider_ProcessImage_ReturnsResult()
{
    var provider = new AzureComputerVisionOCRService(client, logger, config);
    var result = await provider.ProcessImageAsync(imageStream);

    Assert.IsNotNull(result);
    Assert.IsNotEmpty(result.Text);
}
```

## Notes

- Providers handle graceful degradation when credentials are missing
- All providers implement health checking for availability monitoring
- Providers support batch processing where the underlying service supports it
- PaddleOCR requires a separate Python service - see integration guide

---

**See Also:**

- [Library README](../README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27
**Maintained By:** Development Team
