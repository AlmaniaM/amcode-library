# Services

**Location:** `AMCode.AI/Services/`  
**Last Updated:** 2025-01-27  
**Purpose:** High-level services for AI provider selection, recipe parsing, cost analysis, prompt building, and recipe validation

---

## Overview

The Services subfolder contains high-level services that orchestrate AI provider interactions, implement intelligent provider selection strategies, handle recipe parsing workflows, analyze costs, build prompts, and validate recipe data. These services provide the business logic layer above the provider implementations.

## Responsibilities

- Intelligent AI provider selection based on cost, performance, reliability, and capabilities
- Recipe parsing orchestration with fallback mechanisms
- Cost analysis and estimation across providers
- Prompt building and optimization
- Recipe validation and quality assurance
- Provider health monitoring and caching

## Class Catalog

### Interfaces

#### IAIProviderSelector

**File:** `SmartAIProviderSelector.cs` (interface defined in same file)

**Purpose:** Interface for AI provider selection strategies, allowing different selection algorithms to be implemented.

**Key Members:**
```csharp
public interface IAIProviderSelector
{
    Task<IAIProvider> SelectOCRProvider(AIRequest request);
    Task<IEnumerable<IAIProvider>> GetAvailableProvidersAsync();
    Task<AIProviderHealth> GetProviderHealthAsync(string providerName);
    Task<decimal> GetCostEstimateAsync(AIRequest request);
}
```

**Usage:**
```csharp
IAIProviderSelector selector = new SmartAIProviderSelector(providers, logger, costAnalyzer);
var provider = await selector.SelectOCRProvider(request);
```

**Implementations:**
- [SmartAIProviderSelector](#smartaiproviderselector) - Intelligent multi-strategy selection
- [ConfigurationAIProviderSelector](#configurationaiproviderselector) - Configuration-based selection

---

#### ICostAnalyzer

**File:** `ICostAnalyzer.cs`

**Purpose:** Interface for analyzing and estimating costs across different AI providers.

**Key Members:**
```csharp
public interface ICostAnalyzer
{
    Task<decimal> EstimateCostAsync(IAIProvider provider, AIRequest request);
    Task<Dictionary<string, decimal>> CompareCostsAsync(AIRequest request, IEnumerable<IAIProvider> providers);
    Task<decimal> GetHistoricalCostAsync(string providerName, TimeSpan period);
}
```

**Usage:**
```csharp
ICostAnalyzer costAnalyzer = new CostAnalyzer(logger);
var cost = await costAnalyzer.EstimateCostAsync(provider, request);
```

**Implementations:**
- [CostAnalyzer](#costanalyzer) - Cost analysis implementation

---

#### IRecipeValidationService

**File:** `IRecipeValidationService.cs`

**Purpose:** Interface for validating parsed recipe data for completeness and quality.

**Key Members:**
```csharp
public interface IRecipeValidationService
{
    Task<RecipeValidationResult> ValidateAsync(ParsedRecipe recipe);
    Task<RecipeValidationResult> ValidateAsync(ParsedRecipeResult result);
}
```

**Usage:**
```csharp
IRecipeValidationService validator = new RecipeValidationService(logger);
var validation = await validator.ValidateAsync(parsedRecipe);
```

**Implementations:**
- [RecipeValidationService](#recipevalidationservice) - Recipe validation implementation

---

### Classes

#### SmartAIProviderSelector

**File:** `SmartAIProviderSelector.cs`

**Purpose:** Intelligent AI provider selector with multiple selection strategies including cost optimization, performance optimization, reliability optimization, capability matching, quality optimization, and balanced selection.

**Key Responsibilities:**
- Select best provider based on configured strategy
- Consider cost, performance, reliability, capabilities, and quality
- Cache provider health information
- Provide cost estimates for requests
- Filter providers based on availability and capabilities

**Key Members:**
```csharp
public class SmartAIProviderSelector : IAIProviderSelector
{
    public SmartAIProviderSelector(
        IEnumerable<IAIProvider> providers,
        ILogger<SmartAIProviderSelector> logger,
        ICostAnalyzer costAnalyzer,
        AIProviderSelectionStrategy strategy = AIProviderSelectionStrategy.Balanced);
    
    public async Task<IAIProvider> SelectOCRProvider(AIRequest request);
    public async Task<IEnumerable<IAIProvider>> GetAvailableProvidersAsync();
    public async Task<AIProviderHealth> GetProviderHealthAsync(string providerName);
    public async Task<decimal> GetCostEstimateAsync(AIRequest request);
}
```

**Selection Strategies:**
- **CostOptimized**: Selects provider with lowest cost
- **PerformanceOptimized**: Selects provider with best performance metrics
- **ReliabilityOptimized**: Selects most reliable provider based on health
- **CapabilityOptimized**: Selects provider that best matches request requirements
- **QualityOptimized**: Selects provider with highest quality scores
- **Balanced**: Balances all factors for optimal selection

**Usage:**
```csharp
var selector = new SmartAIProviderSelector(
    providers,
    logger,
    costAnalyzer,
    AIProviderSelectionStrategy.CostOptimized
);

var provider = await selector.SelectOCRProvider(request);
```

**Dependencies:**
- `AMCode.AI.Enums` - Selection strategy enum
- `AMCode.AI.Models` - Request and health models
- `ICostAnalyzer` - Cost analysis service

**Related Components:**
- [ConfigurationAIProviderSelector](#configurationaiproviderselector) - Alternative selection strategy
- [CostAnalyzer](#costanalyzer) - Used for cost-based selection

---

#### ConfigurationAIProviderSelector

**File:** `ConfigurationAIProviderSelector.cs`

**Purpose:** Configuration-based AI provider selector that selects providers based on configuration settings rather than intelligent analysis.

**Key Responsibilities:**
- Select provider based on configuration
- Support fallback providers
- Handle provider unavailability
- Simple, predictable selection logic

**Key Members:**
```csharp
public class ConfigurationAIProviderSelector : IAIProviderSelector
{
    public ConfigurationAIProviderSelector(
        IEnumerable<IAIProvider> providers,
        ILogger<ConfigurationAIProviderSelector> logger,
        IOptions<AIConfiguration> config);
    
    public async Task<IAIProvider> SelectOCRProvider(AIRequest request);
    public async Task<IEnumerable<IAIProvider>> GetAvailableProvidersAsync();
    public async Task<AIProviderHealth> GetProviderHealthAsync(string providerName);
    public async Task<decimal> GetCostEstimateAsync(AIRequest request);
}
```

**Usage:**
```csharp
var selector = new ConfigurationAIProviderSelector(
    providers,
    logger,
    Options.Create(aiConfig)
);

var provider = await selector.SelectOCRProvider(request);
```

**Dependencies:**
- `AMCode.AI.Configurations` - Configuration classes
- `Microsoft.Extensions.Options` - Options pattern

**Related Components:**
- [SmartAIProviderSelector](#smartaiproviderselector) - Alternative intelligent selection
- [AIConfiguration](../Configurations/AIConfiguration.cs) - Configuration used for selection

---

#### EnhancedHybridAIService

**File:** `EnhancedHybridAIService.cs`

**Purpose:** Enhanced hybrid AI service that implements recipe parsing with intelligent provider selection, fallback mechanisms, and cost tracking.

**Key Responsibilities:**
- Parse recipe text using AI providers
- Implement fallback mechanisms for reliability
- Track costs across providers
- Handle errors gracefully with retries
- Support custom parsing options

**Key Members:**
```csharp
public class EnhancedHybridAIService : IRecipeParserService
{
    public EnhancedHybridAIService(
        IEnumerable<IAIProvider> providers,
        IAIProviderFactory providerFactory,
        ILogger<EnhancedHybridAIService> logger,
        ICostAnalyzer costAnalyzer,
        IOptions<AIConfiguration> config);
    
    public async Task<Result<ParsedRecipeResult>> ParseRecipeAsync(string text, CancellationToken cancellationToken = default);
    public async Task<Result<ParsedRecipeResult>> ParseRecipeAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default);
}
```

**Usage:**
```csharp
var service = new EnhancedHybridAIService(
    providers,
    providerFactory,
    logger,
    costAnalyzer,
    Options.Create(aiConfig)
);

var result = await service.ParseRecipeAsync(recipeText, options);
if (result.Success)
{
    var recipe = result.Value.Recipe;
}
```

**Dependencies:**
- `AMCode.AI.Factories` - Provider factory
- `AMCode.AI.Configurations` - Configuration
- `ICostAnalyzer` - Cost analysis

**Related Components:**
- [IRecipeParserService](../IRecipeParserService.cs) - Interface implemented
- [AIProviderFactory](../Factories/AIProviderFactory.cs) - Used for provider creation
- [CostAnalyzer](#costanalyzer) - Used for cost tracking

---

#### CostAnalyzer

**File:** `CostAnalyzer.cs`

**Purpose:** Service for analyzing and estimating costs across different AI providers.

**Key Responsibilities:**
- Estimate costs for AI requests
- Compare costs across multiple providers
- Track historical costs
- Calculate token-based costs
- Provide cost optimization recommendations

**Key Members:**
```csharp
public class CostAnalyzer : ICostAnalyzer
{
    public CostAnalyzer(ILogger<CostAnalyzer> logger);
    
    public async Task<decimal> EstimateCostAsync(IAIProvider provider, AIRequest request);
    public async Task<Dictionary<string, decimal>> CompareCostsAsync(AIRequest request, IEnumerable<IAIProvider> providers);
    public async Task<decimal> GetHistoricalCostAsync(string providerName, TimeSpan period);
}
```

**Usage:**
```csharp
var costAnalyzer = new CostAnalyzer(logger);

// Estimate cost for a request
var cost = await costAnalyzer.EstimateCostAsync(provider, request);

// Compare costs across providers
var costs = await costAnalyzer.CompareCostsAsync(request, providers);
var cheapest = costs.OrderBy(c => c.Value).First();
```

**Dependencies:**
- `AMCode.AI.Models` - Request models
- `AMCode.AI.Providers` - Provider interface

**Related Components:**
- [SmartAIProviderSelector](#smartaiproviderselector) - Uses cost analyzer for selection
- [EnhancedHybridAIService](#enhancedhybridaiservice) - Uses cost analyzer for tracking

---

#### PromptBuilderService

**File:** `PromptBuilderService.cs`

**Purpose:** Service for building and optimizing prompts for AI providers, especially for recipe parsing.

**Key Responsibilities:**
- Build structured prompts for recipe parsing
- Optimize prompts for different providers
- Handle prompt templates
- Support multi-step prompts
- Include context and instructions

**Key Members:**
```csharp
public class PromptBuilderService
{
    public PromptBuilderService(ILogger<PromptBuilderService> logger);
    
    public string BuildRecipeParsingPrompt(string text, RecipeParsingOptions options);
    public string BuildValidationPrompt(ParsedRecipe recipe);
    public string BuildEnhancementPrompt(ParsedRecipe recipe, string enhancementType);
}
```

**Usage:**
```csharp
var promptBuilder = new PromptBuilderService(logger);

var prompt = promptBuilder.BuildRecipeParsingPrompt(
    recipeText,
    new RecipeParsingOptions { IncludeNutrition = true }
);
```

**Dependencies:**
- `AMCode.AI.Models` - Recipe parsing options and models

**Related Components:**
- All providers use this service for prompt building
- [EnhancedHybridAIService](#enhancedhybridaiservice) - Uses prompt builder

---

#### RecipeValidationService

**File:** `RecipeValidationService.cs`

**Purpose:** Service for validating parsed recipe data to ensure completeness, correctness, and quality.

**Key Responsibilities:**
- Validate recipe structure and completeness
- Check ingredient format and validity
- Validate directions and timing
- Verify nutrition data if present
- Provide validation feedback and suggestions

**Key Members:**
```csharp
public class RecipeValidationService : IRecipeValidationService
{
    public RecipeValidationService(ILogger<RecipeValidationService> logger);
    
    public async Task<RecipeValidationResult> ValidateAsync(ParsedRecipe recipe);
    public async Task<RecipeValidationResult> ValidateAsync(ParsedRecipeResult result);
}
```

**Usage:**
```csharp
var validator = new RecipeValidationService(logger);

var validation = await validator.ValidateAsync(parsedRecipe);
if (!validation.IsValid)
{
    foreach (var error in validation.Errors)
    {
        Console.WriteLine($"Error: {error}");
    }
}
```

**Dependencies:**
- `AMCode.AI.Models` - Recipe and validation models

**Related Components:**
- [ParsedRecipe](../Models/ParsedRecipe.cs) - Validated model
- [RecipeValidationResult](../Models/RecipeValidationResult.cs) - Validation result model

---

## Architecture Patterns

### Strategy Pattern
Provider selection uses the Strategy pattern:
- `IAIProviderSelector` defines the selection strategy interface
- `SmartAIProviderSelector` and `ConfigurationAIProviderSelector` are different strategies
- Selection strategy can be changed at runtime

### Factory Pattern
Services use factories for provider creation:
- `EnhancedHybridAIService` uses `IAIProviderFactory` to create providers
- Allows dynamic provider instantiation
- Supports dependency injection

### Service Layer Pattern
Services provide business logic layer:
- Services orchestrate provider interactions
- Handle complex workflows (fallback, retry, validation)
- Abstract complexity from consumers

### Caching Pattern
Health information is cached:
- `SmartAIProviderSelector` caches provider health
- Reduces redundant health checks
- Improves selection performance

## Usage Patterns

### Pattern 1: Smart Provider Selection

```csharp
var selector = new SmartAIProviderSelector(
    providers,
    logger,
    costAnalyzer,
    AIProviderSelectionStrategy.CostOptimized
);

var request = new AIRequest
{
    Text = recipeText,
    Options = new RecipeParsingOptions(),
    EstimatedTokens = 1000
};

var provider = await selector.SelectOCRProvider(request);
var result = await provider.ParseTextAsync(recipeText);
```

### Pattern 2: Recipe Parsing with Fallback

```csharp
var service = new EnhancedHybridAIService(
    providers,
    providerFactory,
    logger,
    costAnalyzer,
    Options.Create(aiConfig)
);

var options = new RecipeParsingOptions
{
    IncludeNutrition = true,
    IncludeTiming = true
};

var result = await service.ParseRecipeAsync(recipeText, options);
if (result.Success)
{
    var recipe = result.Value.Recipe;
}
```

### Pattern 3: Cost Comparison

```csharp
var costAnalyzer = new CostAnalyzer(logger);

var request = new AIRequest { Text = recipeText, EstimatedTokens = 1000 };
var costs = await costAnalyzer.CompareCostsAsync(request, providers);

foreach (var cost in costs.OrderBy(c => c.Value))
{
    Console.WriteLine($"{cost.Key}: ${cost.Value:F4}");
}
```

### Pattern 4: Recipe Validation

```csharp
var validator = new RecipeValidationService(logger);

var validation = await validator.ValidateAsync(parsedRecipe);
if (validation.IsValid)
{
    Console.WriteLine("Recipe is valid!");
}
else
{
    Console.WriteLine($"Validation failed: {validation.ErrorMessage}");
    foreach (var error in validation.Errors)
    {
        Console.WriteLine($"  - {error}");
    }
}
```

## Dependencies

### Internal Dependencies

- `AMCode.AI.Providers` - Provider interfaces and implementations
- `AMCode.AI.Models` - Request/response models
- `AMCode.AI.Configurations` - Configuration classes
- `AMCode.AI.Factories` - Provider factories
- `AMCode.AI.Enums` - Selection strategies

### External Dependencies

- `Microsoft.Extensions.Logging` - Logging
- `Microsoft.Extensions.Options` - Options pattern
- `Microsoft.Extensions.DependencyInjection` - Dependency injection

## Related Components

### Within Same Library

- [Providers](../Providers/README.md) - Provider implementations used by services
- [Factories](../Factories/README.md) - Factories used for provider creation
- [Models](../Models/README.md) - Models used by services
- [Configurations](../Configurations/README.md) - Configuration classes
- [Enums](../Enums/README.md) - Selection strategy enums

### In Other Libraries

- None

## Testing

### Test Coverage

- Service unit tests in `AMCode.AI.Tests/Services/`
- Provider selection strategy tests
- Cost analysis tests
- Recipe validation tests
- Integration tests for service workflows

### Example Test

```csharp
[Test]
public async Task SmartAIProviderSelector_SelectOCRProvider_ReturnsProvider()
{
    // Arrange
    var providers = new[] { openAIProvider, anthropicProvider };
    var selector = new SmartAIProviderSelector(providers, logger, costAnalyzer);
    var request = new AIRequest { Text = "test", EstimatedTokens = 100 };
    
    // Act
    var provider = await selector.SelectOCRProvider(request);
    
    // Assert
    Assert.That(provider, Is.Not.Null);
    Assert.That(provider.IsAvailable, Is.True);
}
```

## Notes

- Services provide the business logic layer above providers
- Smart selection strategies can be configured via `AIConfiguration`
- Fallback mechanisms ensure reliability
- Cost tracking is optional but recommended for production
- Health caching improves selection performance
- All services support dependency injection
- Services are thread-safe and can be used concurrently

---

**See Also:**
- [Library README](../README.md) - Library overview
- [IRecipeParserService](../IRecipeParserService.cs) - Recipe parsing interface
- [Root README](../../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
