# AMCode.AI Library — Agent Guide

## What This Is

Multi-cloud AI service abstraction library for .NET 8. Provides a unified interface (`IAIProvider`) across 12+ AI providers, a pipeline pattern (`IAIPipeline<TInput, TOutput>`) for composable AI tasks, tool/function calling, and RAG building blocks.

## When to Use This Module

- **Any AI integration**: Text completion, structured JSON, chat, embeddings, vision, tool calling
- **Adding new AI providers**: Extend `GenericAIProvider` and register in `AIProviderRegistry`
- **Creating new AI-powered features**: Create a pipeline class extending `AIPipelineBase<TInput, TOutput>`
- **Config-driven provider selection**: Use `PipelineConfiguration` to select provider/model per task in `appsettings.json`
- **RAG / semantic search**: Use `IEmbeddingService`, `IVectorStore`, `IRAGService`

## When NOT to Use This Module

- **Domain-specific business logic**: Recipe parsing prompts, meal plan generation logic → belongs in the app layer (e.g., `RecipeTextExtractionPipeline` in the backend)
- **Direct provider API calls**: NEVER call OpenAI/Anthropic/Groq APIs directly — always go through `IAIProvider`
- **OCR / text extraction from images**: Use `AMCode.OCR` instead
- **Cloud storage**: Use `AMCode.Storage` instead

## Key Interfaces & Entry Points

| Interface | Purpose | Location |
|-----------|---------|----------|
| `IAIProvider` | Core provider interface (complete, chat, JSON, vision, embeddings) | `IAIProvider.cs` |
| `IAIPipeline<TInput, TOutput>` | Pipeline interface for composable AI tasks | `Pipelines/IAIPipeline.cs` |
| `AIPipelineBase<TInput, TOutput>` | Base class with provider resolution, retry, fallback | `Pipelines/AIPipelineBase.cs` |
| `IAIProviderFactory` | Creates providers by name or config | `Factories/IAIProviderFactory.cs` |
| `IEmbeddingService` | Generate text embeddings | `RAG/IEmbeddingService.cs` |
| `IVectorStore` | Vector storage and similarity search | `RAG/IVectorStore.cs` |
| `IRAGService` | Retrieval-augmented generation | `RAG/IRAGService.cs` |

## Creating a New Pipeline

```csharp
// 1. Create pipeline class in your app layer
public class MyFeaturePipeline : AIPipelineBase<MyInput, MyOutput>
{
    public override string PipelineName => "MyFeature";

    public MyFeaturePipeline(IAIProviderFactory providerFactory,
        PipelineConfiguration config, ILogger<MyFeaturePipeline> logger)
        : base(providerFactory, config, logger) { }

    protected override async Task<Result<MyOutput>> ExecuteWithProviderAsync(
        IAIProvider provider, MyInput input, CancellationToken ct)
    {
        var request = new AIChatRequest { /* ... */ };
        var result = await provider.ChatAsync(request, ct);
        // Parse and return
    }
}

// 2. Register in DI
services.AddAIPipeline<MyFeaturePipeline, MyInput, MyOutput>(configuration, "MyFeature");

// 3. Configure in appsettings.json
// "AI": { "Pipelines": { "MyFeature": { "Provider": "OpenAI", "Model": "gpt-4o-mini" } } }
```

## Provider Registry

Providers are registered in `Configurations/AIProviderRegistry.cs`. Available providers:

| Config Key | Provider Class | Capabilities |
|-----------|----------------|-------------|
| `openai` | OpenAIGPTProvider | Chat, JSON, Vision, Embeddings, Tools |
| `anthropic` | AnthropicClaudeProvider | Chat, JSON, Vision, Tools |
| `groqcloud` | GroqCloudProvider | Chat, JSON, Vision (Llama 4 Scout) |
| `azure-openai` | AzureOpenAIProvider | Chat, JSON, Vision, Embeddings |
| `aws-bedrock` | AWSBedrockProvider | Chat, JSON, Vision |
| `ollama` | OllamaAIProvider | Chat, JSON (local) |
| `lmstudio` | LMStudioAIProvider | Chat, JSON (local) |
| `grok` | GrokProvider | Chat, JSON |
| `perplexity` | PerplexityProvider | Chat, JSON |
| `huggingface` | HuggingFaceAIProvider | Chat, Embeddings |

## Tool/Function Calling

```csharp
var request = new AIChatRequest
{
    Messages = new() { AIChatMessage.User("What's the weather?") },
    Tools = new()
    {
        new AIToolDefinition
        {
            Name = "get_weather",
            Description = "Get weather for a location",
            ParametersJsonSchema = """{"type":"object","properties":{"location":{"type":"string"}}}"""
        }
    },
    ToolChoice = "auto"
};

var result = await provider.ChatAsync(request);
if (result.HasToolCalls)
{
    foreach (var tc in result.ToolCalls)
    {
        // Execute tool, then send result back
        var toolResult = AIChatMessage.ToolResult(tc.Id, "72°F, sunny");
    }
}
```

Supported by: OpenAI, Anthropic. Groq does not support tools yet.

## Configuration

```json
{
  "AI": {
    "Provider": "OpenAI",
    "FallbackProvider": "Anthropic",
    "Pipelines": {
      "RecipeExtraction": {
        "Provider": "GroqCloud",
        "Model": "llama-3.3-70b-versatile",
        "FallbackProvider": "OpenAI",
        "Temperature": 0.1,
        "MaxTokens": 4096,
        "MaxRetries": 3
      }
    },
    "OpenAI": { "ApiKey": "", "Model": "gpt-4o-mini" },
    "Anthropic": { "ApiKey": "", "Model": "claude-sonnet-4-5" },
    "GroqCloud": { "ApiKey": "", "Model": "llama-3.3-70b-versatile" }
  }
}
```

## Deprecated APIs (Do Not Use)

- `IAIProvider.ParseTextAsync()` → Use `IAIPipeline<string, ParsedRecipeResult>` pipeline
- `IRecipeParserService` → Use pipelines
- `IRecipeValidationService` → Use pipeline-level validation
- `GenericAIProvider.BuildPrompt()` / `ParseJsonResponse()` → Move prompts to your pipeline

## Architecture Rules

1. **This library is domain-agnostic** — no recipe/cooking/food concepts. Those belong in the app layer.
2. **All providers extend `GenericAIProvider`** — override `ChatAsync`, `AnalyzeImageAsync`, etc.
3. **Provider resolution**: Pipeline config `Provider` → Global `AI:Provider` → First available
4. **Retry**: Exponential backoff with configurable `MaxRetries` per pipeline
5. **Fallback**: Automatic fallback to `FallbackProvider` on primary failure

## Verification

```bash
cd repos/amcode-library
dotnet build ailibrary/AMCode.AI/AMCode.AI.csproj
dotnet test ailibrary/AMCode.AI.Tests/AMCode.AI.Tests.csproj  # if tests exist
```
