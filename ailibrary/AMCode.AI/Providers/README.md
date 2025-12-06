# Providers

**Location:** `AMCode.AI/Providers/`  
**Last Updated:** 2025-01-27  
**Purpose:** AI provider implementations for various AI services, including cloud providers (OpenAI, Anthropic, AWS Bedrock, Azure) and local/self-hosted solutions (Ollama, LM Studio, HuggingFace)

---

## Overview

The Providers subfolder contains all AI provider implementations in the AMCode.AI library. These providers implement the `IAIProvider` interface and inherit from `GenericAIProvider` base class, providing a unified interface for interacting with different AI services. The library supports 12+ providers including major cloud services and local solutions.

## Responsibilities

- Implement AI provider interfaces for various AI services
- Provide unified API for text completion, chat, embeddings, vision, and recipe parsing
- Handle provider-specific API communication and error handling
- Support streaming responses for real-time interactions
- Implement cost calculation and health monitoring
- Provide fallback mechanisms for reliability

## Class Catalog

### Base Classes

#### GenericAIProvider

**File:** `GenericAIProvider.cs`

**Purpose:** Abstract base class that implements common functionality for all AI providers, reducing code duplication and ensuring consistent behavior across providers.

**Key Responsibilities:**
- Common HTTP request/response handling
- JSON serialization/deserialization
- Error handling and retry logic
- Logging and diagnostics
- Default implementations for common operations

**Key Members:**
```csharp
public abstract class GenericAIProvider : IAIProvider
{
    public abstract string ProviderName { get; }
    public abstract bool RequiresInternet { get; }
    public abstract AIProviderCapabilities Capabilities { get; }
    public virtual bool IsAvailable => CheckAvailability();
    
    // Abstract methods (must be implemented by derived classes)
    public abstract Task<ParsedRecipeResult> ParseTextAsync(string text, CancellationToken cancellationToken = default);
    public abstract Task<ParsedRecipeResult> ParseTextAsync(string text, RecipeParsingOptions options, CancellationToken cancellationToken = default);
    public abstract Task<AIProviderHealth> CheckHealthAsync();
    public abstract Task<decimal> GetCostEstimateAsync(string text, RecipeParsingOptions options);
    
    // Virtual methods (can be overridden)
    public virtual async Task<AICompletionResult> CompleteAsync(string prompt, CancellationToken cancellationToken = default);
    public virtual async Task<AICompletionResult> CompleteAsync(AICompletionRequest request, CancellationToken cancellationToken = default);
    public virtual async Task<AIChatResult> ChatAsync(string message, string? systemMessage = null, CancellationToken cancellationToken = default);
    public virtual async Task<AIChatResult> ChatAsync(AIChatRequest request, CancellationToken cancellationToken = default);
    public virtual IAsyncEnumerable<AIStreamChunk> ChatStreamingAsync(AIChatRequest request, CancellationToken cancellationToken = default);
    public virtual async Task<AIJsonResult<T>> CompleteJsonAsync<T>(string prompt, CancellationToken cancellationToken = default) where T : class;
    public virtual async Task<AIEmbeddingResult> GetEmbeddingAsync(string text, CancellationToken cancellationToken = default);
    public virtual async Task<AIVisionResult> AnalyzeImageAsync(string prompt, string imageUrl, CancellationToken cancellationToken = default);
}
```

**Usage:**
```csharp
// All providers inherit from GenericAIProvider
public class OpenAIGPTProvider : GenericAIProvider
{
    // Implement abstract methods
    // Override virtual methods as needed
}
```

**Dependencies:**
- `AMCode.AI.Models` - Request/response models
- `Microsoft.Extensions.Logging` - Logging
- `Microsoft.Extensions.Http` - HTTP client factory

**Related Components:**
- [IAIProvider](../IAIProvider.cs) - Interface that all providers implement
- All provider implementations inherit from this class

---

### Cloud Provider Implementations

#### OpenAIGPTProvider

**File:** `OpenAIGPTProvider.cs`

**Purpose:** OpenAI GPT provider implementation supporting GPT-3.5, GPT-4, GPT-4o, and GPT-4o Mini models.

**Key Features:**
- Supports streaming, function calling, vision, and embeddings
- Long context support (up to 128K tokens)
- Cost tracking per token
- Multiple model selection

**Provider Name:** `"OpenAI GPT"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅
- Vision: ✅
- Long Context: ✅ (128K tokens)
- Embeddings: ✅
- Custom Models: ✅

**Configuration:** Uses `OpenAIConfiguration` from [Configurations](../Configurations/README.md)

**Usage:**
```csharp
var provider = new OpenAIGPTProvider(
    logger,
    httpClientFactory,
    Options.Create(openAIConfig),
    promptBuilder
);

var result = await provider.CompleteAsync("Hello, world!");
```

**Related Components:**
- [OpenAIConfiguration](../Configurations/OpenAIConfiguration.cs) - Configuration class

---

#### AnthropicClaudeProvider

**File:** `AnthropicClaudeProvider.cs`

**Purpose:** Anthropic Claude provider implementation supporting Claude 3 Opus, Sonnet, Haiku, and Claude 3.5 models.

**Key Features:**
- Supports streaming, function calling, and vision
- Very long context support (up to 200K tokens)
- High-quality responses
- No custom model support (pre-built models only)

**Provider Name:** `"Anthropic Claude"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅
- Vision: ✅
- Long Context: ✅ (200K tokens)
- Embeddings: ❌
- Custom Models: ❌ (pre-built models only)

**Configuration:** Uses `AnthropicConfiguration` from [Configurations](../Configurations/README.md)

**Usage:**
```csharp
var provider = new AnthropicClaudeProvider(
    logger,
    httpClientFactory,
    Options.Create(anthropicConfig),
    promptBuilder
);

var result = await provider.ChatAsync("Hello, Claude!");
```

**Related Components:**
- [AnthropicConfiguration](../Configurations/AnthropicConfiguration.cs) - Configuration class

---

#### AWSBedrockProvider

**File:** `AWSBedrockProvider.cs`

**Purpose:** AWS Bedrock provider implementation supporting multiple models including Claude, Llama, and Titan models.

**Key Features:**
- Supports multiple model families via AWS Bedrock
- Region-based configuration
- AWS IAM authentication
- Cost-effective for high-volume usage

**Provider Name:** `"AWS Bedrock"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅ (model-dependent)
- Vision: ✅ (model-dependent)
- Long Context: ✅ (model-dependent)
- Embeddings: ✅ (model-dependent)
- Custom Models: ✅ (via AWS Bedrock custom models)

**Configuration:** Uses `AWSBedrockConfiguration` from [Configurations](../Configurations/README.md)

**Usage:**
```csharp
var provider = new AWSBedrockProvider(
    logger,
    httpClientFactory,
    Options.Create(bedrockConfig),
    promptBuilder
);

var result = await provider.CompleteAsync("Hello from Bedrock!");
```

**Related Components:**
- [AWSBedrockConfiguration](../Configurations/AWSBedrockConfiguration.cs) - Configuration class

---

#### AzureOpenAIProvider

**File:** `AzureOpenAIProvider.cs`

**Purpose:** Azure OpenAI provider implementation using REST API for GPT models hosted on Azure.

**Key Features:**
- Azure-hosted GPT models
- Enterprise-grade security and compliance
- Regional deployment options
- Integration with Azure services

**Provider Name:** `"Azure OpenAI"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅
- Vision: ✅
- Long Context: ✅
- Embeddings: ✅
- Custom Models: ✅ (via Azure OpenAI fine-tuning)

**Configuration:** Uses `AzureOpenAIConfiguration` from [Configurations](../Configurations/README.md)

**Related Components:**
- [AzureOpenAIConfiguration](../Configurations/AzureOpenAIConfiguration.cs) - Configuration class

---

#### AzureOpenAISdkProvider

**File:** `AzureOpenAISdkProvider.cs`

**Purpose:** Azure OpenAI provider implementation using the official Azure.AI.OpenAI SDK.

**Key Features:**
- Uses official Azure SDK
- Better integration with Azure services
- Enhanced error handling
- SDK-specific features

**Provider Name:** `"Azure OpenAI SDK"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅
- Vision: ✅
- Long Context: ✅
- Embeddings: ✅
- Custom Models: ✅

**Configuration:** Uses `AzureOpenAIConfiguration` from [Configurations](../Configurations/README.md)

**Related Components:**
- [AzureOpenAIConfiguration](../Configurations/AzureOpenAIConfiguration.cs) - Configuration class

---

#### AzureComputerVisionProvider

**File:** `AzureComputerVisionProvider.cs`

**Purpose:** Azure Computer Vision provider for image analysis and OCR capabilities.

**Key Features:**
- Specialized for image analysis
- OCR capabilities
- Image description and analysis
- Azure Cognitive Services integration

**Provider Name:** `"Azure Computer Vision"`

**Capabilities:**
- Streaming: ❌
- Function Calling: ❌
- Vision: ✅ (primary purpose)
- Long Context: ❌
- Embeddings: ❌
- Custom Models: ❌

**Configuration:** Uses `AzureComputerVisionConfiguration` from [Configurations](../Configurations/README.md)

**Related Components:**
- [AzureComputerVisionConfiguration](../Configurations/AzureComputerVisionConfiguration.cs) - Configuration class

---

#### GrokProvider

**File:** `GrokProvider.cs`

**Purpose:** Grok (X.AI) provider implementation for X.AI's Grok models.

**Key Features:**
- X.AI Grok models
- Real-time information access
- X (Twitter) integration capabilities

**Provider Name:** `"Grok (X.AI)"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅
- Vision: ✅
- Long Context: ✅
- Embeddings: ❌
- Custom Models: ❌

**Configuration:** Uses `GrokConfiguration` from [Configurations](../Configurations/README.md)

**Related Components:**
- [GrokConfiguration](../Configurations/GrokConfiguration.cs) - Configuration class

---

#### PerplexityProvider

**File:** `PerplexityProvider.cs`

**Purpose:** Perplexity provider implementation for Perplexity AI's search-enhanced models.

**Key Features:**
- Search-enhanced responses
- Real-time information access
- Online model support

**Provider Name:** `"Perplexity"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅
- Vision: ❌
- Long Context: ✅
- Embeddings: ❌
- Custom Models: ❌

**Configuration:** Uses `PerplexityConfiguration` from [Configurations](../Configurations/README.md)

**Related Components:**
- [PerplexityConfiguration](../Configurations/PerplexityConfiguration.cs) - Configuration class

---

### Local/Self-Hosted Provider Implementations

#### OllamaAIProvider

**File:** `OllamaAIProvider.cs`

**Purpose:** Ollama provider for running local LLM models via Ollama service.

**Key Features:**
- Local model execution
- No internet required (after model download)
- Privacy-focused
- Supports various open-source models (Llama, Mistral, etc.)

**Provider Name:** `"Ollama Local"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅ (model-dependent)
- Vision: ✅ (model-dependent)
- Long Context: ✅ (model-dependent)
- Embeddings: ✅ (model-dependent)
- Custom Models: ✅ (via Ollama model import)

**Configuration:** Uses `OllamaConfiguration` from [Configurations](../Configurations/README.md)

**Usage:**
```csharp
var provider = new OllamaAIProvider(
    logger,
    httpClientFactory,
    Options.Create(ollamaConfig),
    promptBuilder
);

var result = await provider.CompleteAsync("Hello from local model!");
```

**Related Components:**
- [OllamaConfiguration](../Configurations/OllamaConfiguration.cs) - Configuration class

---

#### LMStudioAIProvider

**File:** `LMStudioAIProvider.cs`

**Purpose:** LM Studio provider for running local LLM models via LM Studio service.

**Key Features:**
- Local model execution
- LM Studio compatibility
- No internet required
- GUI-based model management

**Provider Name:** `"LM Studio Local"`

**Capabilities:**
- Streaming: ✅
- Function Calling: ✅ (model-dependent)
- Vision: ❌
- Long Context: ✅ (model-dependent)
- Embeddings: ❌
- Custom Models: ✅ (via LM Studio model import)

**Configuration:** Uses `LMStudioConfiguration` from [Configurations](../Configurations/README.md)

**Related Components:**
- [LMStudioConfiguration](../Configurations/LMStudioConfiguration.cs) - Configuration class

---

#### HuggingFaceAIProvider

**File:** `HuggingFaceAIProvider.cs`

**Purpose:** HuggingFace provider for accessing models via HuggingFace Inference API.

**Key Features:**
- Access to HuggingFace model hub
- Wide variety of models
- Inference API integration
- Both cloud and local options

**Provider Name:** `"Hugging Face"`

**Capabilities:**
- Streaming: ✅ (model-dependent)
- Function Calling: ❌
- Vision: ✅ (model-dependent)
- Long Context: ✅ (model-dependent)
- Embeddings: ✅ (model-dependent)
- Custom Models: ✅ (via HuggingFace model hosting)

**Configuration:** Uses `HuggingFaceConfiguration` from [Configurations](../Configurations/README.md)

**Related Components:**
- [HuggingFaceConfiguration](../Configurations/HuggingFaceConfiguration.cs) - Configuration class

---

## Architecture Patterns

### Template Method Pattern
The `GenericAIProvider` base class implements the Template Method pattern:
- Defines the skeleton of algorithms in base class
- Derived classes implement specific steps (CreateRequestAsync, ProcessResponseAsync)
- Common functionality shared across all providers

### Strategy Pattern
Each provider is a strategy implementation:
- All providers implement `IAIProvider` interface
- Provider selection can be changed at runtime
- Different providers can be used for different use cases

### Factory Pattern
Providers are created via factories:
- [AIProviderFactory](../Factories/AIProviderFactory.cs) creates provider instances
- Uses configuration to determine which provider to instantiate
- Supports dependency injection

### Adapter Pattern
Providers adapt different AI service APIs to unified interface:
- Each provider adapts its specific API to `IAIProvider` interface
- Hides provider-specific implementation details
- Provides consistent API across all providers

## Usage Patterns

### Pattern 1: Direct Provider Usage

```csharp
// Create provider instance
var provider = new OpenAIGPTProvider(
    logger,
    httpClientFactory,
    Options.Create(openAIConfig),
    promptBuilder
);

// Use provider
var result = await provider.CompleteAsync("Hello, world!");
if (result.Success)
{
    Console.WriteLine(result.Content);
}
```

### Pattern 2: Provider Selection via Factory

```csharp
// Use factory to create provider
var factory = serviceProvider.GetRequiredService<IAIProviderFactory>();
var provider = await factory.CreateProviderAsync("OpenAI");

// Use provider
var result = await provider.ChatAsync("Hello!");
```

### Pattern 3: Streaming Responses

```csharp
var provider = new AnthropicClaudeProvider(...);

await foreach (var chunk in provider.ChatStreamingAsync(chatRequest))
{
    if (chunk.Type == AIStreamChunkType.Content)
    {
        Console.Write(chunk.Content);
    }
}
```

### Pattern 4: Recipe Parsing

```csharp
var provider = new OpenAIGPTProvider(...);

var options = new RecipeParsingOptions
{
    IncludeNutrition = true,
    IncludeTiming = true,
    Language = "en"
};

var result = await provider.ParseTextAsync(recipeText, options);
if (result.Success && result.Recipe != null)
{
    Console.WriteLine($"Title: {result.Recipe.Title}");
    Console.WriteLine($"Ingredients: {result.Recipe.Ingredients.Count}");
}
```

### Pattern 5: Health Checking

```csharp
var provider = new OpenAIGPTProvider(...);

var health = await provider.CheckHealthAsync();
if (health.IsHealthy)
{
    Console.WriteLine($"Provider is healthy");
    Console.WriteLine($"Average response time: {health.AverageResponseTime}");
}
else
{
    Console.WriteLine($"Provider is unhealthy: {health.ErrorMessage}");
}
```

### Pattern 6: Cost Estimation

```csharp
var provider = new OpenAIGPTProvider(...);

var options = new RecipeParsingOptions { IncludeNutrition = true };
var estimatedCost = await provider.GetCostEstimateAsync(recipeText, options);

Console.WriteLine($"Estimated cost: ${estimatedCost:F4}");
```

## Dependencies

### Internal Dependencies

- `AMCode.AI.Configurations` - Provider configuration classes
- `AMCode.AI.Models` - Request/response models
- `AMCode.AI.Services` - Services like PromptBuilderService
- `AMCode.Common` - Common utilities

### External Dependencies

- `OpenAI` (2.1.0) - OpenAI API SDK (for OpenAIGPTProvider)
- `Azure.AI.OpenAI` (2.1.0) - Azure OpenAI SDK (for AzureOpenAISdkProvider)
- `Anthropic.SDK` (0.1.0) - Anthropic Claude API SDK (for AnthropicClaudeProvider)
- `AWSSDK.BedrockRuntime` (3.7.400.1) - AWS Bedrock SDK (for AWSBedrockProvider)
- `Microsoft.Extensions.Logging.Abstractions` (8.0.0) - Logging
- `Microsoft.Extensions.Http` (8.0.0) - HTTP client factory
- `Microsoft.Extensions.Options` (8.0.0) - Options pattern
- `System.Text.Json` (8.0.0) - JSON serialization

## Related Components

### Within Same Library

- [IAIProvider](../IAIProvider.cs) - Interface that all providers implement
- [Configurations](../Configurations/README.md) - Provider configuration classes
- [Factories](../Factories/README.md) - Provider factory for instantiation
- [Services](../Services/README.md) - Services that use providers
- [Models](../Models/README.md) - Request/response models used by providers

### In Other Libraries

- None

## Testing

### Test Coverage

- Provider implementation tests in `AMCode.AI.Tests/Providers/`
- Integration tests for each provider
- Health check tests
- Cost calculation tests
- Streaming tests
- Error handling tests

### Example Test

```csharp
[Test]
public async Task OpenAIGPTProvider_CompleteAsync_ReturnsSuccess()
{
    // Arrange
    var provider = new OpenAIGPTProvider(...);
    
    // Act
    var result = await provider.CompleteAsync("Hello, world!");
    
    // Assert
    Assert.That(result.Success, Is.True);
    Assert.That(result.Content, Is.Not.Empty);
    Assert.That(result.Provider, Is.EqualTo("OpenAI GPT"));
}
```

## Notes

- All providers implement the same `IAIProvider` interface for unified access
- Providers inherit from `GenericAIProvider` to reduce code duplication
- Each provider has specific capabilities that may differ from others
- Cost calculation is provider-specific and based on token usage
- Health monitoring is available for all providers
- Streaming support varies by provider and model
- Local providers (Ollama, LM Studio) don't require internet after model download
- Cloud providers require API keys and internet connectivity
- Provider selection can be done via configuration or smart selection service

---

**See Also:**
- [Library README](../README.md) - Library overview
- [IAIProvider Interface](../IAIProvider.cs) - Provider interface documentation
- [Root README](../../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
