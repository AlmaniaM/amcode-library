# Services

**Location:** `AMCode.OCR/Services/`
**Last Updated:** 2025-01-27
**Purpose:** OCR services, provider selectors, and cost analysis components

---

## Overview

The Services subfolder contains the main OCR service implementation (`EnhancedHybridOCRService`), intelligent provider selection logic (`SmartOCRProviderSelector`), configuration-based selection (`ConfigurationOCRProviderSelector`), and cost analysis utilities (`CostAnalyzer`).

## Responsibilities

- Implement `IOCRService` interface for OCR operations
- Provide intelligent provider selection based on multiple factors
- Handle automatic fallback to alternative providers
- Analyze costs and optimize provider selection
- Manage batch processing
- Monitor provider health and availability

## Class Catalog

### Interfaces

#### IOCRProviderSelector

**File:** `SmartOCRProviderSelector.cs` (interface definition)

**Purpose:** Interface for OCR provider selection logic.

**Key Members:**

```csharp
public interface IOCRProviderSelector
{
    Task<IOCRProvider> SelectOCRProvider(OCRRequest request);
    Task<IEnumerable<IOCRProvider>> GetAvailableProvidersAsync();
    Task<OCRProviderHealth> GetProviderHealthAsync(string providerName);
    Task<decimal> GetCostEstimateAsync(OCRRequest request);
}
```

**Usage:**

```csharp
IOCRProviderSelector selector = new SmartOCRProviderSelector(providers, logger, strategy);
var provider = await selector.SelectOCRProvider(request);
```

---

### Classes

#### EnhancedHybridOCRService

**File:** `EnhancedHybridOCRService.cs`

**Purpose:** Main OCR service implementation with intelligent provider selection and automatic fallback.

**Key Responsibilities:**

- Coordinate OCR operations across multiple providers
- Select optimal provider based on request characteristics
- Handle automatic fallback to alternative providers on errors
- Support batch processing
- Monitor service health and capabilities

**Key Members:**

```csharp
public class EnhancedHybridOCRService : IOCRService
{
    public Task<Result<OCRResult>> ExtractTextAsync(Stream imageStream, CancellationToken cancellationToken = default);
    public Task<Result<OCRResult>> ExtractTextAsync(string imagePath, CancellationToken cancellationToken = default);
    public Task<Result<IEnumerable<OCRResult>>> ProcessBatchAsync(IEnumerable<Stream> imageStreams, OCRRequest? options = null, CancellationToken cancellationToken = default);
    public Task<bool> IsAvailableAsync();
    public Task<OCRProviderHealth> GetHealthAsync();
    public OCRProviderCapabilities GetCapabilities();
    public Task<decimal> GetCostEstimateAsync(long imageSizeBytes, OCRRequest? options = null);
}
```

**Usage:**

```csharp
var service = new EnhancedHybridOCRService(providers, factory, logger, config);
var result = await service.ExtractTextAsync(imageStream);
```

**Dependencies:**

- `IEnumerable<IOCRProvider>` - Available OCR providers
- `IOCRProviderFactory` - Provider factory
- `IOCRProviderSelector` - Provider selector (via factory)
- `OCRConfiguration` - Service configuration

---

#### SmartOCRProviderSelector

**File:** `SmartOCRProviderSelector.cs`

**Purpose:** Intelligent provider selection based on cost, performance, reliability, capabilities, and quality.

**Key Responsibilities:**

- Select best provider based on selection strategy
- Evaluate providers using multiple factors
- Cache provider health information
- Optimize selection for cost, performance, or quality

**Key Members:**

```csharp
public class SmartOCRProviderSelector : IOCRProviderSelector
{
    public Task<IOCRProvider> SelectOCRProvider(OCRRequest request);
    public Task<IEnumerable<IOCRProvider>> GetAvailableProvidersAsync();
    public Task<OCRProviderHealth> GetProviderHealthAsync(string providerName);
    public Task<decimal> GetCostEstimateAsync(OCRRequest request);
}
```

**Selection Strategies:**

- `CostOptimized` - Select provider with lowest cost
- `PerformanceOptimized` - Select fastest provider
- `ReliabilityOptimized` - Select most reliable provider
- `CapabilityOptimized` - Select provider with best capabilities
- `QualityOptimized` - Select provider with best quality
- `Balanced` - Balance all factors
- `LoadBalanced` - Distribute load across providers

**Usage:**

```csharp
var selector = new SmartOCRProviderSelector(providers, logger, OCRProviderSelectionStrategy.CostOptimized);
var provider = await selector.SelectOCRProvider(request);
```

---

#### ConfigurationOCRProviderSelector

**File:** `ConfigurationOCRProviderSelector.cs`

**Purpose:** Provider selection based on configuration settings (appsettings.json).

**Key Responsibilities:**

- Select provider based on `OCR:Provider` configuration
- Use fallback provider if primary is unavailable
- Simple, configuration-driven selection

**Usage:**

```csharp
var selector = new ConfigurationOCRProviderSelector(providers, config, logger);
var provider = await selector.SelectOCRProvider(request);
```

---

#### CostAnalyzer

**File:** `CostAnalyzer.cs`

**Purpose:** Analyze and compare costs across different OCR providers.

**Key Responsibilities:**

- Calculate cost estimates for each provider
- Compare costs across providers
- Optimize provider selection for cost
- Track cost metrics

**Key Members:**

```csharp
public class CostAnalyzer
{
    public Task<decimal> CalculateCostAsync(IOCRProvider provider, long imageSizeBytes, OCRRequest? options = null);
    public Task<Dictionary<string, decimal>> CompareCostsAsync(IEnumerable<IOCRProvider> providers, long imageSizeBytes, OCRRequest? options = null);
}
```

**Usage:**

```csharp
var analyzer = new CostAnalyzer();
var costs = await analyzer.CompareCostsAsync(providers, imageSizeBytes);
var cheapestProvider = costs.OrderBy(c => c.Value).First().Key;
```

---

## Architecture Patterns

- **Service Pattern**: `EnhancedHybridOCRService` implements `IOCRService`
- **Strategy Pattern**: Multiple selection strategies (cost, performance, quality)
- **Factory Pattern**: Uses `IOCRProviderFactory` for provider creation
- **Adapter Pattern**: Adapts multiple providers to unified interface

## Usage Patterns

### Pattern 1: Basic OCR Service Usage

```csharp
var service = serviceProvider.GetRequiredService<IOCRService>();
var result = await service.ExtractTextAsync(imageStream);
```

### Pattern 2: Provider Selection

```csharp
var selector = serviceProvider.GetRequiredService<IOCRProviderSelector>();
var provider = await selector.SelectOCRProvider(request);
var result = await provider.ProcessImageAsync(imageStream);
```

### Pattern 3: Cost-Optimized Selection

```csharp
var selector = new SmartOCRProviderSelector(providers, logger, OCRProviderSelectionStrategy.CostOptimized);
var provider = await selector.SelectOCRProvider(request);
```

### Pattern 4: Batch Processing

```csharp
var service = serviceProvider.GetRequiredService<IOCRService>();
var results = await service.ProcessBatchAsync(imageStreams);
```

## Dependencies

### Internal Dependencies

- `AMCode.OCR.Providers` - OCR provider implementations
- `AMCode.OCR.Models` - OCR models (OCRResult, OCRRequest, etc.)
- `AMCode.OCR.Configurations` - Service configurations
- `AMCode.OCR.Enums` - Selection strategies
- `AMCode.OCR.Factories` - Provider factories

### External Dependencies

- `Microsoft.Extensions.Logging.Abstractions` (8.0.0) - Logging
- `Microsoft.Extensions.Options` (8.0.0) - Configuration

## Related Components

### Within Same Library

- [Providers](../Providers/README.md) - OCR provider implementations
- [Configurations](../Configurations/README.md) - Service configurations
- [Models](../Models/README.md) - OCR models
- [Factories](../Factories/README.md) - Provider factories
- [Enums](../Enums/README.md) - Selection strategies

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.OCR.Tests/Services/`
- Integration tests: `AMCode.OCR.Tests/Integration/`

### Example Test

```csharp
[Test]
public async Task EnhancedHybridOCRService_ExtractText_ReturnsResult()
{
    var service = new EnhancedHybridOCRService(providers, factory, logger, config);
    var result = await service.ExtractTextAsync(imageStream);

    Assert.IsTrue(result.IsSuccess);
    Assert.IsNotEmpty(result.Value.Text);
}
```

## Notes

- `EnhancedHybridOCRService` automatically falls back to alternative providers on errors
- Provider selection can be optimized for cost, performance, quality, or balanced approach
- Health caching improves selection performance
- Batch processing is supported for multiple images

---

**See Also:**

- [Library README](../README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27
**Maintained By:** Development Team
