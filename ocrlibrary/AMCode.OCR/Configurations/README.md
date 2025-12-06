# Configurations

**Location:** `AMCode.OCR/Configurations/`  
**Last Updated:** 2025-01-27  
**Purpose:** Configuration classes for OCR service and provider settings

---

## Overview

The Configurations subfolder contains all configuration classes used to configure the OCR service and individual providers. These configurations are typically loaded from `appsettings.json` and bound using the Options pattern.

## Responsibilities

- Define main OCR service configuration
- Define provider-specific configurations (Azure, AWS, Google, PaddleOCR, etc.)
- Provide configuration validation
- Support configuration binding from appsettings.json
- Manage provider registry

## Class Catalog

### Classes

#### OCRConfiguration

**File:** `OCRConfiguration.cs`

**Purpose:** Main configuration class for OCR service settings.

**Key Properties:**
- `Provider` - Default provider name (e.g., "PaddleOCR", "Azure", "AWS", "Google")
- `FallbackProvider` - Fallback provider name if primary is unavailable
- `ProviderSelectionStrategy` - Selection strategy (CostOptimized, PerformanceOptimized, etc.)
- `DefaultConfidenceThreshold` - Default confidence threshold (default: 0.5)
- `MaxRetries` - Maximum retry attempts (default: 3)
- `DefaultTimeout` - Default request timeout (default: 5 minutes)
- `EnableFallbackProviders` - Whether to enable automatic fallback (default: true)
- `MaxFallbackProviders` - Maximum number of fallback providers to try
- `Azure` - Azure Computer Vision configuration
- `AWS` - AWS Textract configuration
- `Google` - Google Cloud Vision configuration
- `PaddleOCR` - PaddleOCR service configuration
- `Anthropic` - Anthropic OCR configuration (if supported)
- `AWSBedrock` - AWS Bedrock OCR configuration
- `GCPDocumentAI` - GCP Document AI configuration

**Usage:**
```csharp
services.Configure<OCRConfiguration>(options =>
{
    options.Provider = "PaddleOCR";
    options.FallbackProvider = "Azure";
    options.ProviderSelectionStrategy = OCRProviderSelectionStrategy.CostOptimized;
    options.DefaultConfidenceThreshold = 0.7;
});
```

---

#### AzureOCRConfiguration

**File:** `AzureOCRConfiguration.cs`

**Purpose:** Configuration for Azure Computer Vision OCR provider.

**Key Properties:**
- `SubscriptionKey` - Azure subscription key
- `Endpoint` - Azure endpoint URL
- `Region` - Azure region
- `CostPerRequest` - Cost per request in USD

**Usage:**
```csharp
services.Configure<AzureOCRConfiguration>(options =>
{
    options.SubscriptionKey = "your-key";
    options.Endpoint = "https://your-endpoint.cognitiveservices.azure.com/";
    options.Region = "eastus";
});
```

---

#### AWSTextractConfiguration

**File:** `AWSTextractConfiguration.cs`

**Purpose:** Configuration for AWS Textract OCR provider.

**Key Properties:**
- `Region` - AWS region
- `AccessKey` - AWS access key
- `SecretKey` - AWS secret key
- `CostPerRequest` - Cost per request in USD

**Usage:**
```csharp
services.Configure<AWSTextractConfiguration>(options =>
{
    options.Region = "us-east-1";
    options.AccessKey = "your-access-key";
    options.SecretKey = "your-secret-key";
});
```

---

#### GoogleVisionConfiguration

**File:** `GoogleVisionConfiguration.cs`

**Purpose:** Configuration for Google Cloud Vision OCR provider.

**Key Properties:**
- `ProjectId` - Google Cloud project ID
- `CredentialsPath` - Path to service account credentials JSON file
- `CostPerRequest` - Cost per request in USD

**Usage:**
```csharp
services.Configure<GoogleVisionConfiguration>(options =>
{
    options.ProjectId = "your-project-id";
    options.CredentialsPath = "path/to/credentials.json";
});
```

---

#### PaddleOCRConfiguration

**File:** `PaddleOCRConfiguration.cs`

**Purpose:** Configuration for PaddleOCR Python service provider.

**Key Properties:**
- `ServiceUrl` - PaddleOCR service URL (e.g., "http://localhost:8000")
- `Timeout` - Request timeout
- `CostPerRequest` - Cost per request in USD (typically 0 for local service)

**Usage:**
```csharp
services.Configure<PaddleOCRConfiguration>(options =>
{
    options.ServiceUrl = "http://localhost:8000";
    options.Timeout = TimeSpan.FromMinutes(2);
});
```

---

#### OCRProviderRegistry

**File:** `OCRProviderRegistry.cs`

**Purpose:** Registry for managing available OCR providers and their configurations.

**Key Responsibilities:**
- Register provider configurations
- Retrieve provider configurations by name
- Validate provider configurations
- Manage provider availability

**Usage:**
```csharp
var registry = new OCRProviderRegistry();
registry.RegisterProvider("Azure", azureConfig);
var config = registry.GetProviderConfiguration("Azure");
```

---

## Architecture Patterns

- **Options Pattern**: All configurations use Microsoft.Extensions.Options
- **Configuration Pattern**: Load from appsettings.json or programmatic configuration
- **Registry Pattern**: `OCRProviderRegistry` manages provider configurations

## Usage Patterns

### Pattern 1: appsettings.json Configuration

```json
{
  "AMCode": {
    "OCR": {
      "Provider": "PaddleOCR",
      "FallbackProvider": "Azure",
      "ProviderSelectionStrategy": "CostOptimized",
      "PaddleOCR": {
        "ServiceUrl": "http://localhost:8000"
      },
      "Azure": {
        "SubscriptionKey": "your-key",
        "Endpoint": "https://your-endpoint.cognitiveservices.azure.com/"
      }
    }
  }
}
```

### Pattern 2: Programmatic Configuration

```csharp
services.Configure<OCRConfiguration>(config =>
{
    config.Provider = "PaddleOCR";
    config.FallbackProvider = "Azure";
    config.PaddleOCR = new PaddleOCRConfiguration
    {
        ServiceUrl = "http://localhost:8000"
    };
});
```

### Pattern 3: Environment Variables

```csharp
services.Configure<AzureOCRConfiguration>(options =>
{
    options.SubscriptionKey = Environment.GetEnvironmentVariable("AZURE_OCR_KEY");
    options.Endpoint = Environment.GetEnvironmentVariable("AZURE_OCR_ENDPOINT");
});
```

## Dependencies

### Internal Dependencies

- `AMCode.OCR.Enums` - Selection strategy enums

### External Dependencies

- `Microsoft.Extensions.Options` (8.0.0) - Options pattern
- `System.Text.Json.Serialization` - JSON serialization

## Related Components

### Within Same Library

- [Services](../Services/README.md) - Uses configurations for service setup
- [Providers](../Providers/README.md) - Uses provider-specific configurations
- [Enums](../Enums/README.md) - Selection strategy enums

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.OCR.Tests/Configurations/`

### Example Test

```csharp
[Test]
public void OCRConfiguration_WithProvider_IsValid()
{
    var config = new OCRConfiguration
    {
        Provider = "PaddleOCR",
        FallbackProvider = "Azure",
        ProviderSelectionStrategy = OCRProviderSelectionStrategy.CostOptimized
    };
    
    Assert.AreEqual("PaddleOCR", config.Provider);
    Assert.AreEqual("Azure", config.FallbackProvider);
}
```

## Notes

- All configurations support binding from appsettings.json
- Provider configurations are optional - providers are unavailable if not configured
- Cost per request is used for cost optimization in provider selection
- Configuration validation should be performed during service registration

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

