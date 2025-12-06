# AMCode.AI

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Multi-cloud AI service library with support for OpenAI, Anthropic, AWS Bedrock, Ollama, HuggingFace, Grok, and other providers for recipe parsing and general AI operations

---

## Overview

AMCode.AI is a comprehensive multi-provider AI/LLM library designed to provide a unified interface for interacting with various AI services. It supports 12+ AI providers including cloud services (OpenAI, Anthropic, AWS Bedrock, Azure OpenAI) and local/self-hosted solutions (Ollama, LM Studio, HuggingFace). The library features intelligent provider selection, cost analysis, health monitoring, and specialized recipe parsing capabilities.

## Architecture

AMCode.AI follows Clean Architecture principles with a multi-provider pattern. The library is organized into clear layers:

- **Provider Layer**: Provider implementations for each AI service (OpenAI, Anthropic, etc.)
- **Service Layer**: High-level services (RecipeParserService, SmartAIProviderSelector, CostAnalyzer)
- **Factory Layer**: Provider and selector factories for dependency injection
- **Configuration Layer**: Provider-specific configuration classes
- **Model Layer**: Request/response models and domain entities
- **Interface Layer**: Core interfaces (IAIProvider, IRecipeParserService)

### Key Components

- **Multi-Provider Support**: 12+ AI provider implementations with unified interface
- **Smart Provider Selection**: Intelligent provider selection based on cost, availability, and capabilities
- **Recipe Parsing Service**: Specialized service for parsing recipe text into structured data
- **Cost Analysis**: Track and estimate costs across different providers
- **Health Monitoring**: Provider health checks and availability monitoring
- **Streaming Support**: Real-time streaming responses for chat and completions
- **Vision Support**: Image analysis capabilities for supported providers

## Features

- **Multi-Provider AI Support**: 
  - OpenAI (GPT-3.5, GPT-4, GPT-4o, GPT-4o Mini)
  - Anthropic Claude (Claude 3 Opus, Sonnet, Haiku)
  - AWS Bedrock (Claude, Llama, Titan models)
  - Azure OpenAI (GPT models via Azure)
  - Azure Computer Vision
  - Ollama (local models)
  - LM Studio (local models)
  - HuggingFace (Inference API)
  - Grok (X.AI)
  - Perplexity
  - Generic provider for custom implementations

- **Recipe Parsing**: Specialized recipe text parsing into structured data (ingredients, directions, timing, etc.)
- **Smart Provider Selection**: Automatic provider selection based on:
  - Cost optimization
  - Availability and health
  - Capability matching
  - Performance metrics
  - Custom selection strategies

- **Streaming Support**: Real-time streaming for chat and completion operations
- **Structured JSON Responses**: Type-safe JSON response parsing
- **Embeddings**: Text embedding generation
- **Vision/Image Analysis**: Image analysis with prompts
- **Cost Tracking**: Cost estimation and tracking across providers
- **Health Monitoring**: Provider health checks and status monitoring
- **Fallback Mechanisms**: Automatic fallback to alternative providers
- **Configuration-Based Setup**: Easy configuration via appsettings.json

## Dependencies

### Internal Dependencies

- **AMCode.Common** - Common utilities and components

### External Dependencies

- **OpenAI** (2.1.0) - OpenAI API SDK
- **Azure.AI.OpenAI** (2.1.0) - Azure OpenAI SDK
- **Anthropic.SDK** (0.1.0) - Anthropic Claude API SDK
- **AWSSDK.BedrockRuntime** (3.7.400.1) - AWS Bedrock runtime SDK
- **Microsoft.Extensions.Logging.Abstractions** (8.0.0) - Logging abstractions
- **Microsoft.Extensions.Http** (8.0.0) - HTTP client factory
- **Microsoft.Extensions.Configuration.Abstractions** (8.0.0) - Configuration abstractions
- **Microsoft.Extensions.Options** (8.0.0) - Options pattern
- **Microsoft.Extensions.DependencyInjection.Abstractions** (8.0.0) - DI abstractions
- **System.Text.Json** (8.0.0) - JSON serialization

## Project Structure

```
AMCode.AI/
├── [Configurations/](Configurations/README.md)              # Provider configuration classes
│   ├── AIConfiguration.cs      # Base AI configuration
│   ├── AIProviderRegistry.cs  # Provider registry
│   ├── OpenAIConfiguration.cs  # OpenAI configuration
│   ├── AnthropicConfiguration.cs # Anthropic configuration
│   ├── AWSBedrockConfiguration.cs # AWS Bedrock configuration
│   ├── AzureOpenAIConfiguration.cs # Azure OpenAI configuration
│   ├── AzureComputerVisionConfiguration.cs # Azure Vision configuration
│   ├── OllamaConfiguration.cs # Ollama configuration
│   ├── LMStudioConfiguration.cs # LM Studio configuration
│   ├── HuggingFaceConfiguration.cs # HuggingFace configuration
│   ├── GrokConfiguration.cs    # Grok configuration
│   └── PerplexityConfiguration.cs # Perplexity configuration
├── [Providers/](Providers/README.md)                   # AI provider implementations
│   ├── OpenAIGPTProvider.cs    # OpenAI GPT provider
│   ├── AnthropicClaudeProvider.cs # Anthropic Claude provider
│   ├── AWSBedrockProvider.cs   # AWS Bedrock provider
│   ├── AzureOpenAIProvider.cs  # Azure OpenAI provider
│   ├── AzureOpenAISdkProvider.cs # Azure OpenAI SDK provider
│   ├── AzureComputerVisionProvider.cs # Azure Vision provider
│   ├── OllamaAIProvider.cs     # Ollama provider
│   ├── LMStudioAIProvider.cs  # LM Studio provider
│   ├── HuggingFaceAIProvider.cs # HuggingFace provider
│   ├── GrokProvider.cs         # Grok provider
│   ├── PerplexityProvider.cs  # Perplexity provider
│   └── GenericAIProvider.cs    # Generic provider base class
├── [Services/](Services/README.md)                    # High-level services
│   ├── SmartAIProviderSelector.cs # Intelligent provider selection
│   ├── ConfigurationAIProviderSelector.cs # Configuration-based selection
│   ├── EnhancedHybridAIService.cs # Hybrid AI service
│   ├── PromptBuilderService.cs # Prompt building service
│   ├── RecipeValidationService.cs # Recipe validation
│   ├── CostAnalyzer.cs         # Cost analysis service
│   ├── IRecipeValidationService.cs
│   └── ICostAnalyzer.cs
├── [Factories/](Factories/README.md)                   # Factory classes
│   ├── AIProviderFactory.cs    # Provider factory
│   ├── AIProviderSelectorFactory.cs # Selector factory
│   ├── IAIProviderFactory.cs
│   └── IAIProviderSelectorFactory.cs
├── [Models/](Models/README.md)                      # Request/response models
│   ├── AICompletionRequest.cs  # Completion request
│   ├── AICompletionResult.cs   # Completion result
│   ├── AIRequest.cs            # Base AI request
│   ├── ParsedRecipe.cs         # Parsed recipe model
│   ├── ParsedRecipeResult.cs   # Recipe parsing result
│   ├── RecipeParsingOptions.cs # Recipe parsing options
│   ├── RecipeValidationResult.cs # Validation result
│   ├── AIProviderCapabilities.cs # Provider capabilities
│   ├── AIProviderHealth.cs     # Provider health status
│   └── Result.cs               # Result pattern
├── [Enums/](Enums/README.md)                       # Enumerations
│   ├── AIProviderNames.cs      # Provider name constants
│   └── AIProviderSelectionStrategy.cs # Selection strategies
├── [Extensions/](Extensions/README.md)                  # Extension methods
│   └── AIServiceCollectionExtensions.cs # DI extensions
├── IAIProvider.cs              # Main provider interface
├── IRecipeParserService.cs     # Recipe parser interface
└── AMCode.AI.csproj           # Project file
```

## Key Interfaces

### IAIProvider

**Location:** `IAIProvider.cs`

**Purpose:** Unified interface for all AI providers, supporting text completion, structured JSON, chat, embeddings, vision, and recipe parsing.

**Key Methods:**

- `CompleteAsync(string prompt, CancellationToken)` - Text completion
- `CompleteAsync(AICompletionRequest, CancellationToken)` - Completion with options
- `CompleteStreamingAsync(AICompletionRequest, CancellationToken)` - Streaming completion
- `CompleteJsonAsync<T>(string prompt, CancellationToken)` - Structured JSON response
- `ChatAsync(string message, string? systemMessage, CancellationToken)` - Single message chat
- `ChatAsync(AIChatRequest, CancellationToken)` - Multi-turn conversation
- `ChatStreamingAsync(AIChatRequest, CancellationToken)` - Streaming chat
- `GetEmbeddingAsync(string text, CancellationToken)` - Generate embeddings
- `AnalyzeImageAsync(string prompt, string imageUrl, CancellationToken)` - Image analysis
- `ParseTextAsync(string text, CancellationToken)` - Parse recipe text
- `ParseTextAsync(string text, RecipeParsingOptions, CancellationToken)` - Parse with options
- `CheckHealthAsync()` - Check provider health
- `GetCostEstimateAsync(string text, RecipeParsingOptions)` - Cost estimation

**Key Properties:**

- `ProviderName` - Provider identifier
- `RequiresInternet` - Whether provider needs internet
- `IsAvailable` - Current availability status
- `Capabilities` - Provider capabilities and features

**See Also:** [Providers Documentation](Providers/README.md)

### IRecipeParserService

**Location:** `IRecipeParserService.cs`

**Purpose:** High-level service interface for recipe text parsing with automatic provider selection and fallback.

**Key Methods:**

- `ParseRecipeAsync(string text, CancellationToken)` - Parse recipe with default options
- `ParseRecipeAsync(string text, RecipeParsingOptions, CancellationToken)` - Parse with custom options
- `IsAvailableAsync()` - Check if any providers are available

**See Also:** [Services Documentation](Services/README.md)

## Key Classes

### SmartAIProviderSelector

**Location:** `Services/SmartAIProviderSelector.cs`

**Purpose:** Intelligent provider selection based on cost, availability, capabilities, and performance metrics.

**Key Responsibilities:**

- Select optimal provider for each request
- Consider cost, availability, and capabilities
- Support multiple selection strategies (CostOptimized, PerformanceOptimized, Balanced)
- Health monitoring and caching
- Cost estimation

**See Also:** [Services Documentation](Services/README.md)

### AIProviderFactory

**Location:** `Factories/AIProviderFactory.cs`

**Purpose:** Factory for creating AI provider instances dynamically based on configuration.

**Key Responsibilities:**

- Create provider instances from configuration
- Support multiple provider registrations
- Handle provider initialization and validation

**See Also:** [Factories Documentation](Factories/README.md)

### ParsedRecipe

**Location:** `Models/ParsedRecipe.cs`

**Purpose:** Structured recipe data model containing ingredients, directions, timing, and metadata.

**Key Properties:**

- `Title` - Recipe title
- `Description` - Recipe description
- `Ingredients` - List of ingredients with quantities
- `Directions` - List of cooking steps
- `PrepTimeMinutes`, `CookTimeMinutes`, `TotalTimeMinutes` - Timing information
- `Servings` - Number of servings
- `Category`, `Cuisine`, `Difficulty` - Classification
- `Nutrition` - Nutritional information
- `Tags` - Recipe tags

**See Also:** [Models Documentation](Models/README.md)

## Usage Examples

### Basic Usage - Recipe Parsing

```csharp
using AMCode.AI;
using Microsoft.Extensions.DependencyInjection;

// Register services
services.AddAIServices(configuration);

// Use recipe parser service
var recipeParser = serviceProvider.GetRequiredService<IRecipeParserService>();

var recipeText = @"
    Chocolate Chip Cookies
    Ingredients: 2 cups flour, 1 cup sugar, 1/2 cup butter
    Directions: Mix ingredients, bake at 350F for 12 minutes
";

var result = await recipeParser.ParseRecipeAsync(recipeText);

if (result.IsSuccess)
{
    var recipe = result.Value.ParsedRecipe;
    Console.WriteLine($"Title: {recipe.Title}");
    Console.WriteLine($"Ingredients: {recipe.Ingredients.Count}");
    Console.WriteLine($"Directions: {recipe.Directions.Count}");
}
```

### Advanced Usage - Direct Provider Access

```csharp
using AMCode.AI.Providers;
using AMCode.AI.Models;

// Get provider from factory
var providerFactory = serviceProvider.GetRequiredService<IAIProviderFactory>();
var provider = providerFactory.CreateProvider();

// Text completion
var completionResult = await provider.CompleteAsync(
    "Explain quantum computing in simple terms",
    cancellationToken
);
Console.WriteLine(completionResult.Text);

// Structured JSON response
var jsonResult = await provider.CompleteJsonAsync<RecipeData>(
    "Parse this recipe: [recipe text]",
    cancellationToken
);
var recipe = jsonResult.Data;

// Chat conversation
var chatResult = await provider.ChatAsync(
    "What's the best way to cook pasta?",
    systemMessage: "You are a helpful cooking assistant",
    cancellationToken
);
Console.WriteLine(chatResult.Message);
```

### Provider Selection Strategies

```csharp
using AMCode.AI.Services;
using AMCode.AI.Enums;

// Smart selector with cost optimization
var selector = new SmartAIProviderSelector(
    providers,
    logger,
    costAnalyzer,
    strategy: AIProviderSelectionStrategy.CostOptimized
);

var request = new AIRequest { Text = "Parse recipe..." };
var selectedProvider = await selector.SelectOCRProvider(request);

// Use selected provider
var result = await selectedProvider.ParseTextAsync(request.Text);
```

### Streaming Responses

```csharp
var request = new AICompletionRequest
{
    Prompt = "Write a recipe for chocolate cake",
    MaxTokens = 1000
};

await foreach (var chunk in provider.CompleteStreamingAsync(request, cancellationToken))
{
    Console.Write(chunk.Text);
    if (chunk.IsComplete)
        break;
}
```

### Cost Analysis

```csharp
using AMCode.AI.Services;

var costAnalyzer = serviceProvider.GetRequiredService<ICostAnalyzer>();

var options = new RecipeParsingOptions
{
    ProviderPreference = AIProviderNames.OpenAI,
    Model = "gpt-4o-mini"
};

var costEstimate = await costAnalyzer.EstimateCostAsync(
    recipeText,
    options
);

Console.WriteLine($"Estimated cost: ${costEstimate:F4}");
```

### Health Monitoring

```csharp
// Check provider health
var health = await provider.CheckHealthAsync();

if (health.IsHealthy)
{
    Console.WriteLine($"Provider {provider.ProviderName} is healthy");
    Console.WriteLine($"Response time: {health.AverageResponseTime}");
    Console.WriteLine($"Success rate: {health.SuccessRate:P}");
}
else
{
    Console.WriteLine($"Provider is unhealthy: {health.ErrorMessage}");
}

// Get all available providers
var selector = serviceProvider.GetRequiredService<IAIProviderSelector>();
var availableProviders = await selector.GetAvailableProvidersAsync();

foreach (var provider in availableProviders)
{
    Console.WriteLine($"{provider.ProviderName}: {(provider.IsAvailable ? "Available" : "Unavailable")}");
}
```

## Configuration

### appsettings.json Example

```json
{
  "AI": {
    "Provider": "OpenAI",
    "DefaultMaxTokens": 4096,
    "DefaultTemperature": 0.1,
    "EnableCostTracking": true,
    "EnableHealthMonitoring": true,
    "SelectionStrategy": "Balanced",
    
    "OpenAI": {
      "AIProvider": "OpenAI",
      "ApiKey": "sk-...",
      "Model": "gpt-4o-mini",
      "MaxTokens": 4096,
      "Temperature": 0.1,
      "CostPerInputToken": 0.00015,
      "CostPerOutputToken": 0.0006
    },
    
    "Anthropic": {
      "AIProvider": "Anthropic",
      "ApiKey": "sk-ant-...",
      "Model": "claude-3-haiku-20240307",
      "MaxTokens": 4096
    },
    
    "Ollama": {
      "AIProvider": "Ollama",
      "BaseUrl": "http://localhost:11434",
      "Model": "llama2"
    },
    
    "AWSBedrock": {
      "AIProvider": "AWSBedrock",
      "Region": "us-east-1",
      "ModelId": "anthropic.claude-3-haiku-20240307-v1:0"
    }
  }
}
```

### Dependency Injection Setup

```csharp
using AMCode.AI.Extensions;
using Microsoft.Extensions.DependencyInjection;

// Add AI services with configuration
services.AddAIServices(configuration);

// Or configure manually
services.AddScoped<IAIProviderFactory, AIProviderFactory>();
services.AddScoped<IAIProviderSelector, SmartAIProviderSelector>();
services.AddScoped<IRecipeParserService, EnhancedHybridAIService>();
services.AddScoped<ICostAnalyzer, CostAnalyzer>();

// Register specific providers
services.AddScoped<IAIProvider, OpenAIGPTProvider>();
services.AddScoped<IAIProvider, AnthropicClaudeProvider>();
services.AddScoped<IAIProvider, OllamaAIProvider>();
```

## Testing

### Test Projects

- **AMCode.AI.Tests**: Comprehensive unit and integration tests
  - Provider implementation tests
  - Recipe parsing tests
  - Provider selection tests
  - Cost analysis tests
  - Health monitoring tests
  - [Test Project README](../AMCode.AI.Tests/README.md)

### Running Tests

```bash
dotnet test ailibrary/AMCode.AI.Tests/AMCode.AI.Tests.csproj
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Configurations](Configurations/README.md) - Provider configuration classes
- [Providers](Providers/README.md) - AI provider implementations (12+ providers)
- [Services](Services/README.md) - High-level services (RecipeParserService, SmartSelector, CostAnalyzer)
- [Models](Models/README.md) - Request/response models and domain entities
- [Factories](Factories/README.md) - Provider and selector factories
- [Enums](Enums/README.md) - Provider names and selection strategies
- [Extensions](Extensions/README.md) - Dependency injection extension methods

## Related Libraries

- [AMCode.Common](../../commonlibrary/AMCode.Common/README.md) - Common utilities used by this library
- [AMCode.OCR](../../ocrlibrary/AMCode.OCR/README.md) - OCR services that may use AI for text processing

## Migration Notes

- **Version 1.0.0**: Initial release with multi-provider support
- All providers implement IAIProvider interface for unified access
- Configuration-based provider selection is the default
- Smart provider selection available for advanced scenarios

## Known Issues

None currently documented.

## Future Considerations

- Additional provider implementations
- Enhanced streaming support
- More sophisticated cost optimization
- Advanced health monitoring and auto-recovery
- Provider performance benchmarking
- Custom model fine-tuning support

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
