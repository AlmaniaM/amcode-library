# Enums

**Location:** `AMCode.OCR/Enums/`  
**Last Updated:** 2025-01-27  
**Purpose:** Enumeration types for OCR provider selection strategies

---

## Overview

The Enums subfolder contains enumeration types used throughout the OCR library, primarily for defining provider selection strategies.

## Responsibilities

- Define provider selection strategies
- Provide type-safe enumeration values
- Support configuration-based selection

## Class Catalog

### Enums

#### OCRProviderSelectionStrategy

**File:** `OCRProviderSelectionStrategy.cs`

**Purpose:** Strategy for selecting OCR providers based on different optimization criteria.

**Values:**
- `CostOptimized` - Select provider with lowest cost
- `PerformanceOptimized` - Select provider with best performance (fastest response time)
- `ReliabilityOptimized` - Select provider with highest reliability
- `CapabilityOptimized` - Select provider with best capabilities for the request
- `QualityOptimized` - Select provider with best quality output
- `Balanced` - Select provider with best balance of all factors
- `LoadBalanced` - Select provider based on current load and availability
- `GeographicOptimized` - Select provider based on geographic proximity
- `Custom` - Select provider based on custom criteria
- `Configuration` - Select provider based on manual configuration from appsettings.json

**Usage:**
```csharp
// In configuration
var config = new OCRConfiguration
{
    ProviderSelectionStrategy = OCRProviderSelectionStrategy.CostOptimized
};

// In selector creation
var selector = selectorFactory.CreateSelector(OCRProviderSelectionStrategy.PerformanceOptimized);
```

**Selection Logic:**
- **CostOptimized**: Compares `CostPerRequest` across providers, selects cheapest
- **PerformanceOptimized**: Compares average response times, selects fastest
- **ReliabilityOptimized**: Compares success rates and health, selects most reliable
- **CapabilityOptimized**: Matches request requirements to provider capabilities
- **QualityOptimized**: Compares quality scores, selects highest quality
- **Balanced**: Weighted combination of cost, performance, reliability, quality
- **LoadBalanced**: Distributes requests across available providers
- **GeographicOptimized**: Selects provider closest to request origin
- **Custom**: Uses custom selection logic
- **Configuration**: Uses `OCR:Provider` from appsettings.json

---

## Architecture Patterns

- **Strategy Pattern**: Different selection strategies for different optimization goals
- **Enum Pattern**: Type-safe enumeration values

## Usage Patterns

### Pattern 1: Configuration-Based Strategy

```csharp
// appsettings.json
{
  "AMCode": {
    "OCR": {
      "ProviderSelectionStrategy": "CostOptimized"
    }
  }
}

// Code
var selector = selectorFactory.CreateSelector(); // Uses strategy from config
```

### Pattern 2: Programmatic Strategy Selection

```csharp
var selector = selectorFactory.CreateSelector(OCRProviderSelectionStrategy.PerformanceOptimized);
```

### Pattern 3: Dynamic Strategy Selection

```csharp
OCRProviderSelectionStrategy strategy = userPreference == "cost" 
    ? OCRProviderSelectionStrategy.CostOptimized 
    : OCRProviderSelectionStrategy.QualityOptimized;
var selector = selectorFactory.CreateSelector(strategy);
```

## Dependencies

### Internal Dependencies

None - Enums are standalone types.

### External Dependencies

None

## Related Components

### Within Same Library

- [Services](../Services/README.md) - Uses strategies for provider selection
- [Configurations](../Configurations/README.md) - Configuration includes strategy selection
- [Factories](../Factories/README.md) - Factories use strategies for selector creation

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.OCR.Tests/Enums/`

### Example Test

```csharp
[Test]
public void OCRProviderSelectionStrategy_Values_AreValid()
{
    Assert.IsTrue(Enum.IsDefined(typeof(OCRProviderSelectionStrategy), OCRProviderSelectionStrategy.CostOptimized));
    Assert.IsTrue(Enum.IsDefined(typeof(OCRProviderSelectionStrategy), OCRProviderSelectionStrategy.Configuration));
}
```

## Notes

- Default strategy is `Configuration` (uses appsettings.json)
- Strategies can be combined or customized for specific use cases
- Strategy selection affects provider selection performance and cost
- Some strategies require provider health monitoring to be effective

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

