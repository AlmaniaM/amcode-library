# Extensions

**Location:** `AMCode.AI/Extensions/`  
**Last Updated:** 2025-01-27  
**Purpose:** Extension methods for registering AI services with dependency injection

---

## Overview

The Extensions subfolder contains extension methods that simplify registering AMCode.AI services with .NET's dependency injection container. These extensions provide a fluent API for configuring AI providers, services, and related components.

## Responsibilities

- Provide extension methods for service registration
- Simplify dependency injection setup
- Enable automatic provider discovery
- Support configuration-based registration
- Provide fluent configuration API

## Class Catalog

### Extension Classes

#### AIServiceCollectionExtensions

**File:** `AIServiceCollectionExtensions.cs`

**Purpose:** Extension methods for `IServiceCollection` to register all AMCode.AI services, providers, and configurations with dependency injection.

**Key Methods:**

##### AddMultiCloudAI

**Purpose:** Main extension method that registers all AI services, providers, configurations, and HTTP clients.

**Signature:**
```csharp
public static IServiceCollection AddMultiCloudAI(
    this IServiceCollection services, 
    IConfiguration configuration)
```

**What It Registers:**
- All provider configurations (OpenAI, Anthropic, AWS Bedrock, etc.)
- HTTP clients for all providers
- All provider instances as singletons
- Core services (PromptBuilderService, CostAnalyzer, RecipeParserService, etc.)
- Provider factory
- Dynamic provider discovery from configuration

**Usage:**
```csharp
// In Startup.cs or Program.cs
services.AddMultiCloudAI(configuration);
```

**Configuration Requirements:**
```json
{
  "AI": {
    "Provider": "OpenAI",
    "OpenAI": {
      "ApiKey": "sk-...",
      "Model": "gpt-4o"
    },
    "Anthropic": {
      "ApiKey": "sk-ant-...",
      "Model": "claude-3-opus-20240229"
    }
    // ... other providers
  }
}
```

---

##### AddCustomAIProvider

**Purpose:** Register a custom AI provider implementation.

**Signature:**
```csharp
public static IServiceCollection AddCustomAIProvider<T>(
    this IServiceCollection services, 
    string name, 
    IConfiguration configuration) 
    where T : class, IAIProvider
```

**Usage:**
```csharp
services.AddCustomAIProvider<CustomProvider>("Custom", configuration);
```

---

##### AddAIProvider

**Purpose:** Register an AI provider with a custom factory function.

**Signature:**
```csharp
public static IServiceCollection AddAIProvider(
    this IServiceCollection services, 
    Func<IServiceProvider, IAIProvider> factory)
```

**Usage:**
```csharp
services.AddAIProvider(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<CustomProvider>>();
    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    return new CustomProvider(logger, httpClientFactory);
});
```

---

##### ConfigureAIProviderSelection

**Purpose:** Configure the provider selection strategy.

**Signature:**
```csharp
public static IServiceCollection ConfigureAIProviderSelection(
    this IServiceCollection services, 
    AIProviderSelectionStrategy strategy)
```

**Usage:**
```csharp
services.ConfigureAIProviderSelection(AIProviderSelectionStrategy.CostOptimized);
```

---

##### ConfigureAICostTracking

**Purpose:** Configure cost tracking for AI operations.

**Signature:**
```csharp
public static IServiceCollection ConfigureAICostTracking(
    this IServiceCollection services, 
    bool enableTracking = true)
```

**Usage:**
```csharp
services.ConfigureAICostTracking(enableTracking: true);
```

---

##### ConfigureAIHealthMonitoring

**Purpose:** Configure health monitoring for AI providers.

**Signature:**
```csharp
public static IServiceCollection ConfigureAIHealthMonitoring(
    this IServiceCollection services, 
    bool enableMonitoring = true, 
    TimeSpan? checkInterval = null)
```

**Usage:**
```csharp
services.ConfigureAIHealthMonitoring(
    enableMonitoring: true, 
    checkInterval: TimeSpan.FromMinutes(5));
```

---

##### ConfigureAIFallbackProviders

**Purpose:** Configure fallback provider behavior.

**Signature:**
```csharp
public static IServiceCollection ConfigureAIFallbackProviders(
    this IServiceCollection services, 
    bool enableFallback = true, 
    int maxAttempts = 2)
```

**Usage:**
```csharp
services.ConfigureAIFallbackProviders(
    enableFallback: true, 
    maxAttempts: 3);
```

---

## Architecture Patterns

### Extension Method Pattern
Extension methods provide a fluent API for service registration without modifying the base `IServiceCollection` interface.

### Builder Pattern
The fluent API allows chaining configuration methods for a builder-like experience.

### Convention over Configuration
`AddMultiCloudAI` uses conventions to automatically discover and register providers from configuration.

## Usage Patterns

### Pattern 1: Basic Registration

```csharp
// In Program.cs or Startup.cs
var builder = WebApplication.CreateBuilder(args);

// Register all AI services
builder.Services.AddMultiCloudAI(builder.Configuration);

var app = builder.Build();
```

### Pattern 2: Custom Configuration

```csharp
builder.Services.AddMultiCloudAI(builder.Configuration)
    .ConfigureAIProviderSelection(AIProviderSelectionStrategy.Balanced)
    .ConfigureAICostTracking(enableTracking: true)
    .ConfigureAIHealthMonitoring(enableMonitoring: true, 
        checkInterval: TimeSpan.FromMinutes(5))
    .ConfigureAIFallbackProviders(enableFallback: true, maxAttempts: 3);
```

### Pattern 3: Custom Provider Registration

```csharp
builder.Services.AddMultiCloudAI(builder.Configuration)
    .AddCustomAIProvider<CustomProvider>("Custom", builder.Configuration);
```

### Pattern 4: Minimal Registration

```csharp
// Register only specific providers
builder.Services.AddHttpClient<OpenAIGPTProvider>();
builder.Services.Configure<OpenAIConfiguration>(
    builder.Configuration.GetSection("AI:OpenAI"));
builder.Services.AddSingleton<IAIProvider, OpenAIGPTProvider>();
builder.Services.AddSingleton<IRecipeParserService, EnhancedHybridAIService>();
```

## Dependencies

### Internal Dependencies

- `AMCode.AI.Providers` - Provider implementations
- `AMCode.AI.Services` - Service implementations
- `AMCode.AI.Configurations` - Configuration classes
- `AMCode.AI.Factories` - Factory implementations
- `AMCode.AI.Enums` - Enum types

### External Dependencies

- `Microsoft.Extensions.DependencyInjection` - Dependency injection
- `Microsoft.Extensions.Configuration` - Configuration system
- `Microsoft.Extensions.Http` - HTTP client factory
- `Microsoft.Extensions.Options` - Options pattern
- `System.Reflection` - Reflection for dynamic provider discovery

## Related Components

### Within Same Library

- [Providers](../Providers/README.md) - Provider implementations registered by extensions
- [Services](../Services/README.md) - Services registered by extensions
- [Configurations](../Configurations/README.md) - Configuration classes registered by extensions
- [Factories](../Factories/README.md) - Factory registered by extensions

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.AI.Tests/Extensions/`

### Example Test

```csharp
[Test]
public void AddMultiCloudAI_RegistersAllServices()
{
    // Arrange
    var services = new ServiceCollection();
    var configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string>
        {
            { "AI:Provider", "OpenAI" },
            { "AI:OpenAI:ApiKey", "test-key" }
        })
        .Build();
    
    // Act
    services.AddMultiCloudAI(configuration);
    var serviceProvider = services.BuildServiceProvider();
    
    // Assert
    Assert.IsNotNull(serviceProvider.GetService<IRecipeParserService>());
    Assert.IsNotNull(serviceProvider.GetService<IAIProviderFactory>());
}
```

## Notes

- **Automatic Discovery**: `AddMultiCloudAI` automatically discovers providers from configuration sections
- **Named Options**: Supports multiple instances of the same provider type using named options
- **Error Handling**: Provider registration errors are logged but don't fail startup
- **Performance**: All providers are registered as singletons for optimal performance
- **Extensibility**: Easy to add custom providers using `AddCustomAIProvider` or `AddAIProvider`
- **Configuration**: Requires proper configuration structure in appsettings.json
- **Reflection**: Uses reflection for dynamic provider discovery; consider performance implications for large configurations

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Providers](../Providers/README.md) - Provider implementations
- [Services](../Services/README.md) - Service implementations
- [Configurations](../Configurations/README.md) - Configuration classes
- [Root README](../../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
