# Models

**Location:** `AMCode.AI/Models/`  
**Last Updated:** 2025-01-27  
**Purpose:** Request/response models, domain entities, and data structures for AI operations and recipe parsing

---

## Overview

The Models subfolder contains all data models used throughout the AMCode.AI library, including request models for AI operations, response models for AI results, domain entities for recipes, and utility models for health monitoring and capabilities. These models provide type-safe data structures for all library operations.

## Responsibilities

- Define request structures for AI operations (completion, chat, embeddings, vision)
- Define response structures for AI results
- Represent domain entities (recipes, ingredients, nutrition)
- Provide health and capability models for providers
- Support result patterns for error handling
- Define parsing options and validation results

## Class Catalog

### Request Models

#### AIRequest

**File:** `AIRequest.cs`

**Purpose:** Base request model containing common properties for all AI requests.

**Key Properties:**
```csharp
public class AIRequest
{
    public string Text { get; set; } = string.Empty;
    public RecipeParsingOptions? Options { get; set; }
    public int EstimatedTokens { get; set; }
    public bool RequiresFunctionCalling { get; set; }
    public bool RequiresVision { get; set; }
    public int MaxRetries { get; set; } = 3;
    public double ConfidenceThreshold { get; set; } = 0.7;
}
```

**Usage:**
```csharp
var request = new AIRequest
{
    Text = recipeText,
    Options = new RecipeParsingOptions(),
    EstimatedTokens = 1000,
    RequiresFunctionCalling = true
};
```

**Related Components:**
- [RecipeParsingOptions](#recipeparsingoptions) - Parsing options
- Used by provider selectors and services

---

#### AICompletionRequest

**File:** `AICompletionRequest.cs`

**Purpose:** Request model for text completion operations.

**Key Properties:**
```csharp
public class AICompletionRequest
{
    public string Prompt { get; set; } = string.Empty;
    public string? SystemMessage { get; set; }
    public int? MaxTokens { get; set; }
    public float? Temperature { get; set; }
    public float? TopP { get; set; }
    public string[]? StopSequences { get; set; }
    public TimeSpan? Timeout { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}
```

**Usage:**
```csharp
var request = new AICompletionRequest
{
    Prompt = "Write a recipe for chocolate cake",
    MaxTokens = 1000,
    Temperature = 0.7f
};

var result = await provider.CompleteAsync(request);
```

**Related Components:**
- [AICompletionResult](#aicompletionresult) - Response model
- Used by `IAIProvider.CompleteAsync()`

---

### Response Models

#### AICompletionResult

**File:** `AICompletionResult.cs`

**Purpose:** Response model for text completion operations.

**Key Properties:**
```csharp
public class AICompletionResult
{
    public string Content { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string? FinishReason { get; set; }
    public ProviderUsage? Usage { get; set; }
    public TimeSpan? ResponseTime { get; set; }
    public decimal? Cost { get; set; }
}
```

**Usage:**
```csharp
var result = await provider.CompleteAsync("Hello, world!");
if (result.Success)
{
    Console.WriteLine(result.Content);
    Console.WriteLine($"Tokens used: {result.Usage?.TotalTokens}");
    Console.WriteLine($"Cost: ${result.Cost:F4}");
}
```

**Related Components:**
- [AICompletionRequest](#aicompletionrequest) - Request model
- Returned by `IAIProvider.CompleteAsync()`

---

#### ParsedRecipeResult

**File:** `ParsedRecipeResult.cs`

**Purpose:** Result model for recipe parsing operations, containing the parsed recipe and metadata.

**Key Properties:**
```csharp
public class ParsedRecipeResult
{
    public ParsedRecipe? Recipe { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public string Provider { get; set; } = string.Empty;
    public ProviderUsage? Usage { get; set; }
    public TimeSpan? ResponseTime { get; set; }
    public decimal? Cost { get; set; }
    public double? Confidence { get; set; }
}
```

**Usage:**
```csharp
var result = await provider.ParseTextAsync(recipeText);
if (result.Success && result.Recipe != null)
{
    Console.WriteLine($"Title: {result.Recipe.Title}");
    Console.WriteLine($"Ingredients: {result.Recipe.Ingredients.Count}");
    Console.WriteLine($"Confidence: {result.Confidence:P}");
}
```

**Related Components:**
- [ParsedRecipe](#parsedrecipe) - Parsed recipe entity
- Returned by `IAIProvider.ParseTextAsync()`

---

### Domain Entities

#### ParsedRecipe

**File:** `ParsedRecipe.cs`

**Purpose:** Domain entity representing a structured recipe parsed from text.

**Key Properties:**
```csharp
public class ParsedRecipe
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RecipeIngredient> Ingredients { get; set; } = new();
    public List<string> Directions { get; set; } = new();
    public int? PrepTimeMinutes { get; set; }
    public int? CookTimeMinutes { get; set; }
    public int? TotalTimeMinutes { get; set; }
    public int? Servings { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int? DifficultyLevel { get; set; }
    public RecipeNutrition? Nutrition { get; set; }
    public string? Cuisine { get; set; }
    public string? Source { get; set; }
    public string? ImageUrl { get; set; }
}
```

**Usage:**
```csharp
var recipe = new ParsedRecipe
{
    Title = "Chocolate Cake",
    Ingredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { Name = "Flour", Amount = "2 cups" }
    },
    Directions = new List<string> { "Mix ingredients", "Bake at 350°F" }
};
```

**Related Components:**
- [RecipeIngredient](#recipeingredient) - Ingredient model (nested class)
- [RecipeNutrition](#recipenutrition) - Nutrition model (nested class)
- Used in [ParsedRecipeResult](#parsedreciperesult)

---

#### RecipeIngredient

**File:** `ParsedRecipe.cs` (nested class)

**Purpose:** Represents a single ingredient in a recipe.

**Key Properties:**
```csharp
public class RecipeIngredient
{
    public string Name { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Preparation { get; set; } = string.Empty;
    public string Directions { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
```

**Usage:**
```csharp
var ingredient = new RecipeIngredient
{
    Name = "Flour",
    Amount = "2",
    Unit = "cups"
};
```

**JSON Deserialization:**
The `RecipeIngredient` class uses a custom JSON converter (`RecipeIngredientJsonConverter`) that handles both string and structured formats for `amount` and `unit` fields:

- **String format**: `{ "amount": "1/2", "unit": "cup" }`
- **Structured format**: `{ "amount": { "numeric": 0.5, "text": "1/2" }, "unit": { "unit": "cup", "text": "cup" } }`

The converter automatically extracts the `text` field from structured objects and handles null values gracefully.

**Related Components:**
- [RecipeIngredientJsonConverter](#recipeingredientjsonconverter) - Custom JSON converter for flexible deserialization

---

#### RecipeNutrition

**File:** `ParsedRecipe.cs` (nested class)

**Purpose:** Represents nutritional information for a recipe.

**Key Properties:**
```csharp
public class RecipeNutrition
{
    public int? Calories { get; set; }
    public decimal? Protein { get; set; }
    public decimal? Carbohydrates { get; set; }
    public decimal? Fat { get; set; }
    public decimal? Fiber { get; set; }
    public decimal? Sugar { get; set; }
    public decimal? Sodium { get; set; }
}
```

---

### Configuration Models

#### RecipeParsingOptions

**File:** `RecipeParsingOptions.cs`

**Purpose:** Options and preferences for recipe parsing operations.

**Key Properties:**
```csharp
public class RecipeParsingOptions
{
    public bool IncludeNutrition { get; set; } = false;
    public bool IncludeTiming { get; set; } = true;
    public bool IncludeTags { get; set; } = true;
    public bool IncludeDifficulty { get; set; } = false;
    public string Language { get; set; } = "en";
    public bool RequiresFunctionCalling { get; set; } = false;
    public bool RequiresVision { get; set; } = false;
    public Dictionary<string, object>? Metadata { get; set; }
}
```

**Usage:**
```csharp
var options = new RecipeParsingOptions
{
    IncludeNutrition = true,
    IncludeTiming = true,
    Language = "en"
};

var result = await provider.ParseTextAsync(recipeText, options);
```

**Related Components:**
- Used in [AIRequest](#airequest) and parsing methods

---

### Provider Models

#### AIProviderCapabilities

**File:** `AIProviderCapabilities.cs`

**Purpose:** Model representing the capabilities and features of an AI provider.

**Key Properties:**
```csharp
public class AIProviderCapabilities
{
    public bool SupportsStreaming { get; set; }
    public bool SupportsFunctionCalling { get; set; }
    public bool SupportsVision { get; set; }
    public bool SupportsLongContext { get; set; }
    public int MaxTokens { get; set; }
    public int MaxContextLength { get; set; }
    public string[] SupportedLanguages { get; set; } = Array.Empty<string>();
    public decimal CostPerToken { get; set; }
    public decimal CostPerRequest { get; set; }
    public TimeSpan AverageResponseTime { get; set; }
    public bool SupportsCustomModels { get; set; }
    public bool SupportsFineTuning { get; set; }
    public bool SupportsEmbeddings { get; set; }
    public bool SupportsModeration { get; set; }
    public int MaxRequestsPerMinute { get; set; }
    public int MaxRequestsPerDay { get; set; }
}
```

**Usage:**
```csharp
var capabilities = provider.Capabilities;
if (capabilities.SupportsStreaming)
{
    await foreach (var chunk in provider.ChatStreamingAsync(request))
    {
        // Process streaming chunks
    }
}
```

**Related Components:**
- Returned by `IAIProvider.Capabilities` property

---

#### AIProviderHealth

**File:** `AIProviderHealth.cs`

**Purpose:** Model representing the health status of an AI provider.

**Key Properties:**
```csharp
public class AIProviderHealth
{
    public bool IsHealthy { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan? AverageResponseTime { get; set; }
    public DateTime? LastChecked { get; set; }
    public int? SuccessRate { get; set; }
    public int? ErrorCount { get; set; }
}
```

**Usage:**
```csharp
var health = await provider.CheckHealthAsync();
if (health.IsHealthy)
{
    Console.WriteLine($"Provider is healthy. Avg response: {health.AverageResponseTime}");
}
```

**Related Components:**
- Returned by `IAIProvider.CheckHealthAsync()`

---

### Utility Models

#### Result<T>

**File:** `Result.cs`

**Purpose:** Generic result pattern for operations that can succeed or fail, providing type-safe error handling.

**Key Properties:**
```csharp
public class Result<T>
{
    public T? Value { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    
    public static Result<T> Ok(T value);
    public static Result<T> Fail(string errorMessage);
}
```

**Usage:**
```csharp
var result = await service.ParseRecipeAsync(text);
if (result.Success)
{
    var recipe = result.Value; // Type-safe access
}
else
{
    Console.WriteLine($"Error: {result.ErrorMessage}");
}
```

**Related Components:**
- Used by services for type-safe error handling

---

#### RecipeValidationResult

**File:** `RecipeValidationResult.cs`

**Purpose:** Result model for recipe validation operations.

**Key Properties:**
```csharp
public class RecipeValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public List<string> Errors { get; set; } = new();
    public List<string> Warnings { get; set; } = new();
    public Dictionary<string, object>? Metadata { get; set; }
}
```

**Usage:**
```csharp
var validation = await validator.ValidateAsync(recipe);
if (!validation.IsValid)
{
    foreach (var error in validation.Errors)
    {
        Console.WriteLine($"Error: {error}");
    }
}
```

**Related Components:**
- Returned by `IRecipeValidationService.ValidateAsync()`

---

## Architecture Patterns

### Result Pattern
The `Result<T>` class implements the Result pattern:
- Type-safe error handling
- No exceptions for expected failures
- Clear success/failure states
- Used throughout service layer

### Value Objects
Domain entities use value objects:
- `RecipeIngredient` and `RecipeNutrition` are value objects
- Immutable where appropriate
- Rich domain models

### DTO Pattern
Request/response models follow DTO pattern:
- Simple data transfer objects
- No business logic
- Serialization-friendly

## Usage Patterns

### Pattern 1: Recipe Parsing

```csharp
var options = new RecipeParsingOptions
{
    IncludeNutrition = true,
    IncludeTiming = true,
    Language = "en"
};

var result = await provider.ParseTextAsync(recipeText, options);
if (result.Success && result.Recipe != null)
{
    var recipe = result.Recipe;
    Console.WriteLine($"Title: {result.Recipe.Title}");
    Console.WriteLine($"Ingredients: {result.Recipe.Ingredients.Count}");
}
```

### Pattern 2: Result Pattern

```csharp
var result = await service.ParseRecipeAsync(text);
if (result.Success)
{
    var parsedRecipe = result.Value; // Type-safe
    // Use parsedRecipe
}
else
{
    // Handle error
    Console.WriteLine(result.ErrorMessage);
}
```

### Pattern 3: Capability Checking

```csharp
var capabilities = provider.Capabilities;
if (capabilities.SupportsStreaming)
{
    await foreach (var chunk in provider.ChatStreamingAsync(request))
    {
        // Process streaming
    }
}
else
{
    // Fallback to non-streaming
    var result = await provider.ChatAsync(request);
}
```

## Dependencies

### Internal Dependencies

- None (models are leaf nodes in dependency graph)

### External Dependencies

- `System.Text.Json` - JSON serialization attributes (if used)

## Related Components

### Within Same Library

- [Providers](../Providers/README.md) - Use these models for requests/responses
- [Services](../Services/README.md) - Use these models for business logic
- [Configurations](../Configurations/README.md) - Configuration models

### In Other Libraries

- None

## Testing

### Test Coverage

- Model serialization tests
- Validation tests
- Result pattern tests
- Domain entity tests

### Example Test

```csharp
[Test]
public void ParsedRecipe_WithValidData_IsValid()
{
    var recipe = new ParsedRecipe
    {
        Title = "Test Recipe",
        Ingredients = new List<RecipeIngredient>
    {
        new RecipeIngredient { Name = "Flour", Amount = "2 cups" }
    },
        Directions = new List<string> { "Mix", "Bake" }
    };
    
    Assert.That(recipe.Title, Is.EqualTo("Test Recipe"));
    Assert.That(recipe.Ingredients, Has.Count.EqualTo(1));
}
```

### JSON Converters

#### RecipeIngredientJsonConverter

**File:** `RecipeIngredientJsonConverter.cs`

**Purpose:** Custom JSON converter for `RecipeIngredient` that handles flexible JSON formats from AI providers.

**Key Features:**
- Handles both string and structured formats for `amount` and `unit` fields
- Extracts `text` field from structured objects (preferred)
- Falls back to `unit` field for unit extraction
- Gracefully handles null values without deserialization errors
- Skips `numeric` field without attempting to deserialize (prevents null decimal errors)

**Supported Formats:**

1. **String Format** (simple):
```json
{
  "name": "flour",
  "amount": "2",
  "unit": "cups"
}
```

2. **Structured Format** (with nested objects):
```json
{
  "name": "flour",
  "amount": {
    "numeric": 2,
    "text": "2"
  },
  "unit": {
    "unit": "cup",
    "text": "cup"
  }
}
```

3. **Null Handling** (gracefully handled):
```json
{
  "name": "honey",
  "amount": {
    "numeric": null,
    "text": null
  },
  "unit": {
    "unit": null,
    "text": null
  }
}
```

**Registration:**
The converter is automatically registered in `GenericAIProvider._jsonOptions.Converters` and is used for all `RecipeIngredient` deserialization operations.

**Usage:**
The converter is transparent - no code changes needed. It automatically handles both formats when deserializing JSON responses from AI providers.

---

## Notes

- All models are serialization-friendly
- Models use nullable reference types where appropriate
- Result pattern provides type-safe error handling
- Domain entities are rich models with nested value objects
- Request/response models follow consistent patterns
- Provider models support capability and health monitoring
- Custom JSON converters handle flexible AI response formats

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Providers](../Providers/README.md) - Use these models
- [Services](../Services/README.md) - Use these models
- [Root README](../../../README.md) - Project overview

**Last Updated:** 2025-01-28  
**Maintained By:** Development Team
