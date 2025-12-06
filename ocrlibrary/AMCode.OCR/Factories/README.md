# Factories

**Location:** `AMCode.OCR/Factories/`  
**Last Updated:** 2025-01-27  
**Purpose:** Factory classes for creating OCR providers and selectors

---

## Overview

The Factories subfolder contains factory implementations for creating OCR providers and provider selectors dynamically based on configuration. These factories use dependency injection to resolve dependencies and create instances.

## Responsibilities

- Create OCR providers based on configuration
- Create provider selectors based on selection strategy
- Manage provider lifecycle and dependencies
- Support dynamic provider creation

## Class Catalog

### Interfaces

#### IOCRProviderFactory

**File:** `IOCRProviderFactory.cs`

**Purpose:** Interface for creating OCR providers.

**Key Members:**
```csharp
public interface IOCRProviderFactory
{
    IOCRProvider? CreateProvider();
    IOCRProvider? CreateProvider(string providerName);
    IEnumerable<IOCRProvider> CreateAllProviders();
}
```

**Usage:**
```csharp
IOCRProviderFactory factory = serviceProvider.GetRequiredService<IOCRProviderFactory>();
var provider = factory.CreateProvider();
```

---

#### IOCRProviderSelectorFactory

**File:** `IOCRProviderSelectorFactory.cs`

**Purpose:** Interface for creating OCR provider selectors.

**Key Members:**
```csharp
public interface IOCRProviderSelectorFactory
{
    IOCRProviderSelector CreateSelector(OCRProviderSelectionStrategy strategy);
    IOCRProviderSelector CreateSelector();
}
```

**Usage:**
```csharp
IOCRProviderSelectorFactory factory = serviceProvider.GetRequiredService<IOCRProviderSelectorFactory>();
var selector = factory.CreateSelector(OCRProviderSelectionStrategy.CostOptimized);
```

---

### Classes

#### OCRProviderFactory

**File:** `OCRProviderFactory.cs`

**Purpose:** Factory for creating OCR providers dynamically based on configuration.

**Key Responsibilities:**
- Create primary provider from `OCR:Provider` configuration
- Create fallback provider from `OCR:FallbackProvider` configuration
- Create all available providers
- Resolve provider dependencies via dependency injection

**Key Members:**
```csharp
public class OCRProviderFactory : IOCRProviderFactory
{
    public IOCRProvider? CreateProvider();
    public IOCRProvider? CreateProvider(string providerName);
    public IEnumerable<IOCRProvider> CreateAllProviders();
}
```

**Usage:**
```csharp
var factory = new OCRProviderFactory(serviceProvider, options, logger);
var provider = factory.CreateProvider(); // Creates provider from OCR:Provider config
var azureProvider = factory.CreateProvider("Azure"); // Creates specific provider
var allProviders = factory.CreateAllProviders(); // Creates all configured providers
```

**Dependencies:**
- `IServiceProvider` - Dependency injection container
- `IOptions<OCRConfiguration>` - OCR configuration
- `ILogger<OCRProviderFactory>` - Logging

---

#### OCRProviderSelectorFactory

**File:** `OCRProviderSelectorFactory.cs`

**Purpose:** Factory for creating OCR provider selectors based on selection strategy.

**Key Responsibilities:**
- Create selector based on selection strategy
- Create default selector from configuration
- Resolve selector dependencies

**Key Members:**
```csharp
public class OCRProviderSelectorFactory : IOCRProviderSelectorFactory
{
    public IOCRProviderSelector CreateSelector(OCRProviderSelectionStrategy strategy);
    public IOCRProviderSelector CreateSelector();
}
```

**Usage:**
```csharp
var factory = new OCRProviderSelectorFactory(providers, logger);
var selector = factory.CreateSelector(OCRProviderSelectionStrategy.CostOptimized);
var defaultSelector = factory.CreateSelector(); // Uses strategy from config
```

**Dependencies:**
- `IEnumerable<IOCRProvider>` - Available providers
- `ILogger<OCRProviderSelectorFactory>` - Logging
- `IOptions<OCRConfiguration>` - OCR configuration

---

## Architecture Patterns

- **Factory Pattern**: Centralized creation of providers and selectors
- **Dependency Injection**: Uses IServiceProvider for dependency resolution
- **Strategy Pattern**: Different selectors for different selection strategies

## Usage Patterns

### Pattern 1: Create Provider from Configuration

```csharp
var factory = serviceProvider.GetRequiredService<IOCRProviderFactory>();
var provider = factory.CreateProvider(); // Uses OCR:Provider from config
```

### Pattern 2: Create Specific Provider

```csharp
var factory = serviceProvider.GetRequiredService<IOCRProviderFactory>();
var azureProvider = factory.CreateProvider("Azure");
```

### Pattern 3: Create All Providers

```csharp
var factory = serviceProvider.GetRequiredService<IOCRProviderFactory>();
var allProviders = factory.CreateAllProviders();
```

### Pattern 4: Create Selector with Strategy

```csharp
var selectorFactory = serviceProvider.GetRequiredService<IOCRProviderSelectorFactory>();
var selector = selectorFactory.CreateSelector(OCRProviderSelectionStrategy.CostOptimized);
```

## Dependencies

### Internal Dependencies

- `AMCode.OCR.Providers` - Provider implementations
- `AMCode.OCR.Services` - Selector implementations
- `AMCode.OCR.Configurations` - Configuration classes
- `AMCode.OCR.Enums` - Selection strategies

### External Dependencies

- `Microsoft.Extensions.DependencyInjection` - Dependency injection
- `Microsoft.Extensions.Options` (8.0.0) - Options pattern
- `Microsoft.Extensions.Logging.Abstractions` (8.0.0) - Logging

## Related Components

### Within Same Library

- [Providers](../Providers/README.md) - Creates provider instances
- [Services](../Services/README.md) - Creates selector instances
- [Configurations](../Configurations/README.md) - Uses configurations for creation

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.OCR.Tests/Factories/`

### Example Test

```csharp
[Test]
public void OCRProviderFactory_CreateProvider_ReturnsProvider()
{
    var factory = new OCRProviderFactory(serviceProvider, options, logger);
    var provider = factory.CreateProvider("Azure");
    
    Assert.IsNotNull(provider);
    Assert.AreEqual("Azure Computer Vision", provider.ProviderName);
}
```

## Notes

- Factories use dependency injection to resolve provider dependencies
- Providers are created lazily - only when needed
- Factory methods return null if provider is not configured or unavailable
- All providers are registered in dependency injection container

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

