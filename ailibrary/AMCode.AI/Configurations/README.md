# Configurations

**Location:** `AMCode.AI/Configurations/`  
**Last Updated:** 2025-01-27  
**Purpose:** Configuration classes for AI providers and services, mapping to appsettings.json structure

---

## Overview

The Configurations subfolder contains all configuration classes used to configure AI providers and services. These classes map to configuration sections in `appsettings.json` and are used with .NET's Options pattern for dependency injection.

## Responsibilities

- Define configuration structure for each AI provider
- Map configuration sections to strongly-typed classes
- Provide default values for configuration properties
- Support named options for multiple provider instances
- Enable configuration validation

## Class Catalog

### Base Configuration

#### AIConfiguration

**File:** `AIConfiguration.cs`

**Purpose:** Base configuration class for AI services containing common settings like retries, timeouts, and feature flags.

**Properties:**
- `MaxRetries` (int) - Maximum retry attempts (default: 3)
- `ConfidenceThreshold` (double) - Confidence threshold (default: 0.7)
- `DefaultTimeout` (TimeSpan) - Default request timeout (default: 5 minutes)
- `DefaultMaxTokens` (int) - Default maximum tokens (default: 4096)
- `DefaultTemperature` (float) - Default temperature (default: 0.1)
- `EnableCostTracking` (bool) - Enable cost tracking (default: true)
- `EnableHealthMonitoring` (bool) - Enable health monitoring
- `HealthCheckInterval` (TimeSpan) - Health check interval
- `ProviderSelectionStrategy` (string) - Provider selection strategy
- `EnableFallbackProviders` (bool) - Enable fallback providers
- `MaxFallbackAttempts` (int) - Maximum fallback attempts

**Usage:**
```csharp
services.Configure<AIConfiguration>(configuration.GetSection("AI"));
```

---

#### AIProviderRegistry

**File:** `AIProviderRegistry.cs`

**Purpose:** Registry mapping provider names to provider types for dynamic provider discovery and registration.

**Properties:**
- `ProviderTypeMap` (Dictionary<string, Type>) - Map of provider names to types

**Usage:**
```csharp
// Automatically used by factory for provider discovery
var providerType = AIProviderRegistry.ProviderTypeMap["OpenAI"];
```

---

### Provider Configurations

#### OpenAIConfiguration

**File:** `OpenAIConfiguration.cs`

**Purpose:** Configuration for OpenAI provider including API key, model, and request parameters.

**Properties:**
- `ApiKey` (string) - OpenAI API key
- `OrganizationId` (string) - OpenAI organization ID
- `Model` (string) - Model to use (default: "gpt-4o")
- `BaseUrl` (string) - Base API URL (default: "https://api.openai.com/v1")
- `MaxTokens` (int) - Maximum tokens (default: 4096)
- `Temperature` (float) - Temperature (default: 0.1)
- `Timeout` (TimeSpan) - Request timeout
- `CostPerInputToken` (decimal) - Cost per input token
- `CostPerOutputToken` (decimal) - Cost per output token

**Configuration Example:**
```json
{
  "AI": {
    "OpenAI": {
      "ApiKey": "sk-...",
      "Model": "gpt-4o",
      "MaxTokens": 4096
    }
  }
}
```

---

#### AnthropicConfiguration

**File:** `AnthropicConfiguration.cs`

**Purpose:** Configuration for Anthropic Claude provider.

**Properties:**
- `ApiKey` (string) - Anthropic API key
- `Model` (string) - Claude model to use
- `BaseUrl` (string) - Base API URL
- `MaxTokens` (int) - Maximum tokens
- `Temperature` (float) - Temperature
- Additional Claude-specific settings

---

#### AWSBedrockConfiguration

**File:** `AWSBedrockConfiguration.cs`

**Purpose:** Configuration for AWS Bedrock provider.

**Properties:**
- `Region` (string) - AWS region
- `ModelId` (string) - Bedrock model ID
- `AccessKeyId` (string) - AWS access key
- `SecretAccessKey` (string) - AWS secret key
- Additional AWS-specific settings

---

#### AzureOpenAIConfiguration

**File:** `AzureOpenAIConfiguration.cs`

**Purpose:** Configuration for Azure OpenAI provider.

**Properties:**
- `ApiKey` (string) - Azure OpenAI API key
- `Endpoint` (string) - Azure endpoint URL
- `DeploymentName` (string) - Deployment name
- `ApiVersion` (string) - API version
- Additional Azure-specific settings

---

#### AzureComputerVisionConfiguration

**File:** `AzureComputerVisionConfiguration.cs`

**Purpose:** Configuration for Azure Computer Vision provider.

**Properties:**
- `ApiKey` (string) - Computer Vision API key
- `Endpoint` (string) - Endpoint URL
- Additional vision-specific settings

---

#### OllamaConfiguration

**File:** `OllamaConfiguration.cs`

**Purpose:** Configuration for Ollama local provider.

**Properties:**
- `BaseUrl` (string) - Ollama server URL (default: "http://localhost:11434")
- `Model` (string) - Model to use
- `Timeout` (TimeSpan) - Request timeout

---

#### LMStudioConfiguration

**File:** `LMStudioConfiguration.cs`

**Purpose:** Configuration for LM Studio local provider.

**Properties:**
- `BaseUrl` (string) - LM Studio server URL
- `Model` (string) - Model to use
- Additional LM Studio-specific settings

---

#### HuggingFaceConfiguration

**File:** `HuggingFaceConfiguration.cs`

**Purpose:** Configuration for HuggingFace provider.

**Properties:**
- `ApiKey` (string) - HuggingFace API key
- `Model` (string) - Model to use
- `BaseUrl` (string) - Inference API URL
- Additional HuggingFace-specific settings

---

#### GrokConfiguration

**File:** `GrokConfiguration.cs`

**Purpose:** Configuration for Grok (X.AI) provider.

**Properties:**
- `ApiKey` (string) - Grok API key
- `Model` (string) - Grok model
- `BaseUrl` (string) - API base URL
- Additional Grok-specific settings

---

#### PerplexityConfiguration

**File:** `PerplexityConfiguration.cs`

**Purpose:** Configuration for Perplexity provider.

**Properties:**
- `ApiKey` (string) - Perplexity API key
- `Model` (string) - Perplexity model
- `BaseUrl` (string) - API base URL
- Additional Perplexity-specific settings

---

## Architecture Patterns

### Options Pattern
All configuration classes follow .NET's Options pattern, enabling strongly-typed configuration with dependency injection.

### Named Options
Some providers support named options (e.g., multiple OpenAI instances) using `IOptionsMonitor<T>`.

### Configuration Validation
Configuration classes can include validation attributes or implement `IValidateOptions<T>`.

## Usage Patterns

### Pattern 1: Basic Configuration

```csharp
// In appsettings.json
{
  "AI": {
    "OpenAI": {
      "ApiKey": "sk-...",
      "Model": "gpt-4o"
    }
  }
}

// In Startup.cs
services.Configure<OpenAIConfiguration>(configuration.GetSection("AI:OpenAI"));
```

### Pattern 2: Named Options

```csharp
// Multiple OpenAI instances
services.Configure<OpenAIConfiguration>("OCRTextParserAI", 
    configuration.GetSection("AI:OCRTextParserAI"));

// Access named options
var config = optionsMonitor.Get("OCRTextParserAI");
```

### Pattern 3: Configuration Validation

```csharp
services.AddOptions<OpenAIConfiguration>()
    .Bind(configuration.GetSection("AI:OpenAI"))
    .ValidateDataAnnotations();
```

## Dependencies

### Internal Dependencies

- None (configurations are self-contained)

### External Dependencies

- `Microsoft.Extensions.Options` - Options pattern
- `Microsoft.Extensions.Configuration` - Configuration system

## Related Components

### Within Same Library

- [Providers](../Providers/README.md) - Use configurations for provider setup
- [Factories](../Factories/README.md) - Use configurations for provider creation
- [Services](../Services/README.md) - Use configurations for service setup

### In Other Libraries

- None

## Testing

### Test Coverage

- Unit tests: `AMCode.AI.Tests/Configurations/`

### Example Test

```csharp
[Test]
public void OpenAIConfiguration_DefaultValues_AreCorrect()
{
    var config = new OpenAIConfiguration();
    Assert.AreEqual("gpt-4o", config.Model);
    Assert.AreEqual(4096, config.MaxTokens);
}
```

## Notes

- **Security**: Never commit API keys to source control; use environment variables or secure configuration stores
- **Validation**: Consider adding validation attributes to required properties
- **Defaults**: All configuration classes provide sensible defaults
- **Named Options**: Use named options for multiple instances of the same provider type
- **Environment-Specific**: Use different configuration files for different environments (appsettings.Development.json, etc.)

---

**See Also:**
- [Library README](../README.md) - Library overview
- [Providers](../Providers/README.md) - Provider implementations using configurations
- [Factories](../Factories/README.md) - Factory using configurations
- [Root README](../../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
