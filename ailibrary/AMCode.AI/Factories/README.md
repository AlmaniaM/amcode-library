# Factories

**Location:** `AMCode.AI/Factories/`  
**Last Updated:** 2025-01-27  
**Purpose:** Factory classes for creating AI providers and provider selectors dynamically

---

## Overview

The Factories subfolder contains factory classes that create AI provider instances and provider selector instances dynamically based on configuration. These factories implement the Factory pattern to abstract provider instantiation and support dependency injection.

## Responsibilities

- Create AI provider instances based on configuration
- Map configuration keys to provider types
- Support dependency injection for providers
- Create provider selector instances
- Handle provider registration and discovery

## Class Catalog

### Interfaces

#### IAIProviderFactory

**File:** `IAIProviderFactory.cs`

**Purpose:** Interface for factory that creates AI provider instances.

**Key Members:**
```csharp
public interface IAIProviderFactory
{
    IAIProvider CreateProvider();
    IAIProvider CreateProvider(string providerName);
    Task<IAIProvider> CreateProviderAsync(string providerName);
    void RegisterProvider(string name, Func<IServiceProvider, IAIProvider> factory);
}
```

**Usage:**
```csharp
IAIProviderFactory factory = serviceProvider.GetRequiredService<IAIProviderFactory>();
var provider = factory.CreateProvider(); // Uses default from configuration
var specificProvider = factory.CreateProvider("OpenAI");
```

**Implementations:**
- [AIProviderFactory](#aiproviderfactory) - Factory implementation

---

#### IAIProviderSelectorFactory

**File:** `IAIProviderSelectorFactory.cs`

**Purpose:** Interface for factory that creates AI provider selector instances.

**Key Members:**
```csharp
public interface IAIProviderSelectorFactory
{
    IAIProviderSelector CreateSelector();
    IAIProviderSelector CreateSelector(AIProviderSelectionStrategy strategy);
}
```

**Usage:**
```csharp
IAIProviderSelectorFactory factory = serviceProvider.GetRequiredService<IAIProviderSelectorFactory>();
var selector = factory.CreateSelector(); // Uses default strategy
var smartSelector = factory.CreateSelector(AIProviderSelectionStrategy.CostOptimized);
```

**Implementations:**
- [AIProviderSelectorFactory](#aiproviderselectorfactory) - Factory implementation

---

### Classes

#### AIProviderFactory

**File:** `AIProviderFactory.cs`

**Purpose:** Factory implementation that creates AI provider instances dynamically based on configuration and registered providers.

**Key Responsibilities:**
- Read configuration to determine which provider to create
- Map configuration keys to provider types using `AIProviderRegistry`
- Support provider registration for custom providers
- Handle dependency injection for provider instantiation
- Support both synchronous and asynchronous creation

**Key Members:**
```csharp
public class AIProviderFactory : IAIProviderFactory
{
    public AIProviderFactory(IServiceProvider serviceProvider, ILogger<AIProviderFactory> logger);
    
    public IAIProvider CreateProvider();
    public IAIProvider CreateProvider(string providerName);
    public Task<IAIProvider> CreateProviderAsync(string providerName);
    public void RegisterProvider(string name, Func<IServiceProvider, IAIProvider> factory);
}
```

**Usage:**
```csharp
// Create factory
var factory = new AIProviderFactory(serviceProvider, logger);

// Create default provider (from configuration)
var provider = factory.CreateProvider();

// Create specific provider
var openAIProvider = factory.CreateProvider("OpenAI");

// Register custom provider
factory.RegisterProvider("CustomProvider", sp => new CustomProvider(...));
```

**Configuration Mapping:**
The factory uses `AIProviderRegistry.ProviderTypeMap` to map configuration keys to provider types:
- `"openai"` → `OpenAIGPTProvider`
- `"anthropic"` → `AnthropicClaudeProvider`
- `"aws-bedrock"` → `AWSBedrockProvider`
- etc.

**Dependencies:**
- `AMCode.AI.Configurations` - Configuration classes and registry
- `AMCode.AI.Providers` - Provider implementations
- `Microsoft.Extensions.Configuration` - Configuration access
- `Microsoft.Extensions.DependencyInjection` - Service provider

**Related Components:**
- [AIProviderRegistry](../Configurations/AIProviderRegistry.cs) - Provider type mapping
- All provider implementations - Created by this factory

---

#### AIProviderSelectorFactory

**File:** `AIProviderSelectorFactory.cs`

**Purpose:** Factory implementation that creates AI provider selector instances based on configuration and strategy.

**Key Responsibilities:**
- Create provider selectors with appropriate strategies
- Support configuration-based selection
- Support smart selection with different strategies
- Handle dependency injection for selector instantiation

**Key Members:**
```csharp
public class AIProviderSelectorFactory : IAIProviderSelectorFactory
{
    public AIProviderSelectorFactory(
        IServiceProvider serviceProvider,
        ILogger<AIProviderSelectorFactory> logger);
    
    public IAIProviderSelector CreateSelector();
    public IAIProviderSelector CreateSelector(AIProviderSelectionStrategy strategy);
}
```

**Usage:**
```csharp
// Create factory
var factory = new AIProviderSelectorFactory(serviceProvider, logger);

// Create default selector (from configuration)
var selector = factory.CreateSelector();

// Create selector with specific strategy
var costOptimizedSelector = factory.CreateSelector(AIProviderSelectionStrategy.CostOptimized);
```

**Selector Types:**
- **Configuration-based**: Uses `ConfigurationAIProviderSelector` when strategy is not "smart"
- **Smart selection**: Uses `SmartAIProviderSelector` with specified strategy

**Dependencies:**
- `AMCode.AI.Configurations` - Configuration classes
- `AMCode.AI.Services` - Selector implementations
- `AMCode.AI.Enums` - Selection strategy enum
- `Microsoft.Extensions.DependencyInjection` - Service provider

**Related Components:**
- [SmartAIProviderSelector](../Services/SmartAIProviderSelector.cs) - Smart selector implementation
- [ConfigurationAIProviderSelector](../Services/ConfigurationAIProviderSelector.cs) - Configuration selector
- [AIProviderSelectionStrategy](../Enums/AIProviderSelectionStrategy.cs) - Selection strategies

---

## Architecture Patterns

### Factory Pattern
Both factories implement the Factory pattern:
- Abstract object creation
- Centralize provider/selector instantiation
- Support configuration-based creation
- Enable dependency injection

### Registry Pattern
`AIProviderFactory` uses registry pattern:
- `AIProviderRegistry` maps configuration keys to types
- Supports dynamic provider discovery
- Enables extensibility through registration

### Dependency Injection
Factories support dependency injection:
- Use `IServiceProvider` for resolving dependencies
- Providers and selectors are created with proper DI
- Supports scoped, transient, and singleton lifetimes

## Usage Patterns

### Pattern 1: Default Provider Creation

```csharp
// In Startup.cs or Program.cs
services.AddScoped<IAIProviderFactory, AIProviderFactory>();

// In service
public class MyService
{
    private readonly IAIProviderFactory _factory;
    
    public MyService(IAIProviderFactory factory)
    {
        _factory = factory;
    }
    
    public async Task ProcessAsync()
    {
        var provider = _factory.CreateProvider(); // Uses AI:Provider from config
        var result = await provider.CompleteAsync("Hello");
    }
}
```

### Pattern 2: Specific Provider Creation

```csharp
var factory = serviceProvider.GetRequiredService<IAIProviderFactory>();

// Create specific provider
var openAIProvider = factory.CreateProvider("OpenAI");
var anthropicProvider = factory.CreateProvider("Anthropic");
```

### Pattern 3: Custom Provider Registration

```csharp
var factory = serviceProvider.GetRequiredService<IAIProviderFactory>();

// Register custom provider
factory.RegisterProvider("CustomProvider", sp =>
{
    var logger = sp.GetRequiredService<ILogger<CustomProvider>>();
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new CustomProvider(logger, httpClientFactory);
});

// Use custom provider
var customProvider = factory.CreateProvider("CustomProvider");
```

### Pattern 4: Selector Creation

```csharp
var selectorFactory = serviceProvider.GetRequiredService<IAIProviderSelectorFactory>();

// Create default selector
var selector = selectorFactory.CreateSelector();

// Create selector with specific strategy
var costOptimizedSelector = selectorFactory.CreateSelector(
    AIProviderSelectionStrategy.CostOptimized
);

var provider = await costOptimizedSelector.SelectOCRProvider(request);
```

## Dependencies

### Internal Dependencies

- `AMCode.AI.Configurations` - Configuration classes and registry
- `AMCode.AI.Providers` - Provider implementations
- `AMCode.AI.Services` - Selector implementations
- `AMCode.AI.Enums` - Selection strategy enum

### External Dependencies

- `Microsoft.Extensions.Configuration` - Configuration access
- `Microsoft.Extensions.DependencyInjection` - Service provider and DI
- `Microsoft.Extensions.Logging` - Logging

## Related Components

### Within Same Library

- [Configurations](../Configurations/README.md) - Configuration classes and registry used by factories
- [Providers](../Providers/README.md) - Provider implementations created by factory
- [Services](../Services/README.md) - Selector implementations created by factory
- [Enums](../Enums/README.md) - Selection strategy enum

### In Other Libraries

- None

## Testing

### Test Coverage

- Factory creation tests
- Provider registration tests
- Configuration mapping tests
- Dependency injection tests

### Example Test

```csharp
[Test]
public void AIProviderFactory_CreateProvider_ReturnsProvider()
{
    // Arrange
    var serviceProvider = CreateServiceProvider();
    var factory = new AIProviderFactory(serviceProvider, logger);
    
    // Act
    var provider = factory.CreateProvider("OpenAI");
    
    // Assert
    Assert.That(provider, Is.Not.Null);
    Assert.That(provider.ProviderName, Is.EqualTo("OpenAI GPT"));
}
```

## Notes

- Factories support both configuration-based and programmatic provider creation
- Provider registration allows adding custom providers at runtime
- Factories use dependency injection for all provider dependencies
- Configuration keys are case-insensitive
- Factories cache provider type mappings for performance
- Selector factory supports multiple selection strategies
- All factories are thread-safe and can be used concurrently

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Configurations](../Configurations/README.md) - Configuration classes used by factories
- [Providers](../Providers/README.md) - Providers created by factory
- [Root README](../../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
