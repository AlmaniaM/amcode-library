# Enums

**Location:** `AMCode.AI/Enums/`  
**Last Updated:** 2025-01-27  
**Purpose:** Enumerations and constants for AI provider names and selection strategies

---

## Overview

The Enums subfolder contains enumeration types and constant classes used throughout the AMCode.AI library for provider identification and selection strategy configuration. These enums provide type-safe constants and prevent string-based errors.

## Responsibilities

- Define provider name constants for configuration
- Define selection strategy enumeration
- Provide centralized constants to prevent mismatches
- Support configuration binding and validation

## Class Catalog

### Constants Classes

#### AIProviderNames

**File:** `AIProviderNames.cs`

**Purpose:** Static class providing centralized access to AI provider names as defined in appsettings.json. This class prevents mismatches from using static strings throughout the application.

**Key Members:**
```csharp
public static class AIProviderNames
{
    public static string OpenAI => "OpenAI";
    public static string OCRTextParserAI => "OCRTextParserAI";
    public static string Anthropic => "Anthropic";
    public static string AzureOpenAIGPT5Nano => "AzureOpenAIGPT5Nano";
    public static string AWSBedrock => "AWSBedrock";
    public static string Ollama => "Ollama";
    public static string LMStudio => "LMStudio";
    public static string Perplexity => "Perplexity";
    public static string Grok => "Grok";
    public static string HuggingFace => "HuggingFace";
}
```

**Usage:**
```csharp
// Use constants instead of strings
var providerName = AIProviderNames.OpenAI;

// In configuration
configuration["AI:Provider"] = AIProviderNames.Anthropic;

// Compare provider names
if (provider.ProviderName == AIProviderNames.OpenAI)
{
    // Handle OpenAI provider
}
```

**Benefits:**
- Type-safe provider name references
- Prevents typos in provider names
- Centralized location for provider name constants
- Easy refactoring if names change

**Related Components:**
- [AIProviderRegistry](../Configurations/AIProviderRegistry.cs) - Uses these names
- [AIProviderFactory](../Factories/AIProviderFactory.cs) - Uses these names for provider creation
- All provider implementations - Use these names

---

### Enumerations

#### AIProviderSelectionStrategy

**File:** `AIProviderSelectionStrategy.cs`

**Purpose:** Enumeration defining the strategies available for intelligent AI provider selection.

**Values:**
```csharp
public enum AIProviderSelectionStrategy
{
    /// <summary>
    /// Select provider with lowest cost
    /// </summary>
    CostOptimized,
    
    /// <summary>
    /// Select provider with best performance metrics
    /// </summary>
    PerformanceOptimized,
    
    /// <summary>
    /// Select most reliable provider based on health
    /// </summary>
    ReliabilityOptimized,
    
    /// <summary>
    /// Select provider that best matches request requirements
    /// </summary>
    CapabilityOptimized,
    
    /// <summary>
    /// Select provider with highest quality scores
    /// </summary>
    QualityOptimized,
    
    /// <summary>
    /// Balance all factors for optimal selection
    /// </summary>
    Balanced
}
```

**Usage:**
```csharp
// In configuration
{
  "AI": {
    "SmartSelectionStrategy": "CostOptimized"
  }
}

// In code
var selector = new SmartAIProviderSelector(
    providers,
    logger,
    costAnalyzer,
    AIProviderSelectionStrategy.CostOptimized
);

// Switch on strategy
switch (strategy)
{
    case AIProviderSelectionStrategy.CostOptimized:
        // Select cheapest provider
        break;
    case AIProviderSelectionStrategy.PerformanceOptimized:
        // Select fastest provider
        break;
    // ... other cases
}
```

**Configuration Binding:**
The enum can be bound from configuration strings:
- `"CostOptimized"` → `AIProviderSelectionStrategy.CostOptimized`
- `"PerformanceOptimized"` → `AIProviderSelectionStrategy.PerformanceOptimized`
- `"Balanced"` → `AIProviderSelectionStrategy.Balanced`
- etc.

**Related Components:**
- [SmartAIProviderSelector](../Services/SmartAIProviderSelector.cs) - Uses this enum for selection
- [AIConfiguration](../Configurations/AIConfiguration.cs) - Configuration property uses this enum
- [AIProviderSelectorFactory](../Factories/AIProviderSelectorFactory.cs) - Creates selectors with this strategy

---

## Architecture Patterns

### Constants Pattern
`AIProviderNames` uses the Constants pattern:
- Centralized string constants
- Prevents magic strings
- Type-safe references
- Easy maintenance

### Enumeration Pattern
`AIProviderSelectionStrategy` uses enumeration:
- Type-safe strategy selection
- Configuration binding support
- Clear strategy definitions
- Extensible for new strategies

## Usage Patterns

### Pattern 1: Using Provider Name Constants

```csharp
// ✅ CORRECT: Use constants
var provider = factory.CreateProvider(AIProviderNames.OpenAI);

// ❌ INCORRECT: Don't use magic strings
var provider = factory.CreateProvider("OpenAI"); // Typo risk
```

### Pattern 2: Configuration with Constants

```csharp
// In appsettings.json
{
  "AI": {
    "Provider": "OpenAI"  // Matches AIProviderNames.OpenAI
  }
}

// In code - validate against constants
var configuredProvider = configuration["AI:Provider"];
if (configuredProvider == AIProviderNames.OpenAI)
{
    // Handle OpenAI
}
```

### Pattern 3: Selection Strategy Configuration

```csharp
// In appsettings.json
{
  "AI": {
    "SmartSelectionStrategy": "CostOptimized"
  }
}

// In code
var strategy = Enum.Parse<AIProviderSelectionStrategy>(
    configuration["AI:SmartSelectionStrategy"]
);

var selector = new SmartAIProviderSelector(
    providers,
    logger,
    costAnalyzer,
    strategy
);
```

### Pattern 4: Strategy Switching

```csharp
public IAIProvider SelectProvider(AIProviderSelectionStrategy strategy, AIRequest request)
{
    var selector = strategy switch
    {
        AIProviderSelectionStrategy.CostOptimized => 
            new SmartAIProviderSelector(providers, logger, costAnalyzer, strategy),
        AIProviderSelectionStrategy.PerformanceOptimized => 
            new SmartAIProviderSelector(providers, logger, costAnalyzer, strategy),
        _ => new ConfigurationAIProviderSelector(providers, logger, Options.Create(config))
    };
    
    return await selector.SelectOCRProvider(request);
}
```

## Dependencies

### Internal Dependencies

- None (enums are leaf nodes)

### External Dependencies

- None

## Related Components

### Within Same Library

- [Configurations](../Configurations/README.md) - Uses provider names and strategies
- [Services](../Services/README.md) - Uses selection strategy enum
- [Factories](../Factories/README.md) - Uses provider names and strategies
- [Providers](../Providers/README.md) - Provider names match constants

### In Other Libraries

- None

## Testing

### Test Coverage

- Enum value tests
- Constants validation tests
- Configuration binding tests

### Example Test

```csharp
[Test]
public void AIProviderNames_Constants_AreValid()
{
    Assert.That(AIProviderNames.OpenAI, Is.EqualTo("OpenAI"));
    Assert.That(AIProviderNames.Anthropic, Is.EqualTo("Anthropic"));
}

[Test]
public void AIProviderSelectionStrategy_CanParseFromString()
{
    var strategy = Enum.Parse<AIProviderSelectionStrategy>("CostOptimized");
    Assert.That(strategy, Is.EqualTo(AIProviderSelectionStrategy.CostOptimized));
}
```

## Notes

- Provider name constants match configuration keys in appsettings.json
- Selection strategy enum supports string binding from configuration
- Using constants prevents typos and makes refactoring easier
- Enum values are documented with XML comments
- Constants are static properties for easy access
- Enums support extension with new values

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Configurations](../Configurations/README.md) - Uses these enums
- [Services](../Services/README.md) - Uses selection strategy
- [Root README](../../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
