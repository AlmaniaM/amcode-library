# AMCode.AI.Tests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive unit and integration tests for AMCode.AI library

---

## Overview

AMCode.AI.Tests is the test project for the AMCode.AI library. It provides comprehensive test coverage for all components in the AMCode.AI library, including AI provider implementations (OpenAI, Anthropic, AWS Bedrock, Ollama, HuggingFace, etc.), recipe parsing services, provider selection logic, cost analysis, prompt building, and recipe validation.

## Test Structure

The test project mirrors the structure of the AMCode.AI library, with test classes organized by component:

```
AMCode.AI.Tests/
├── Factories/                  # Factory tests
│   └── AIProviderFactoryTests.cs
├── Integration/                # Integration tests
│   └── AIIntegrationTests.cs
├── Models/                     # Model tests
│   ├── AIProviderCapabilitiesTests.cs
│   ├── AIRequestTests.cs
│   ├── ParsedRecipeResultTests.cs
│   ├── ParsedRecipeTests.cs
│   └── RecipeParsingOptionsTests.cs
├── Providers/                  # Provider tests
│   └── LMStudioAIProviderTests.cs
├── Services/                   # Service tests
│   ├── CostAnalyzerTests.cs
│   ├── PromptBuilderServiceTests.cs
│   ├── RecipeValidationServiceTests.cs
│   └── SmartAIProviderSelectorTests.cs
└── AMCode.AI.Tests.csproj
```

## Test Categories

### Factory Tests

**Location:** `Factories/`

Tests for AI provider factory functionality:

- **AIProviderFactoryTests**: Tests for AI provider factory implementation
  - Provider registration
  - Provider creation from configuration
  - Provider retrieval by name
  - Factory error handling

**Test Files:**
- `AIProviderFactoryTests.cs` - Main factory tests

### Integration Tests

**Location:** `Integration/`

End-to-end integration tests for AI services:

- **AIIntegrationTests**: Integration tests for complete AI service workflows
  - Service registration and dependency injection
  - End-to-end recipe parsing workflows
  - Provider selection and fallback mechanisms
  - Configuration binding and validation

**Test Files:**
- `AIIntegrationTests.cs` - Integration test suite

### Model Tests

**Location:** `Models/`

Tests for AI models and data structures:

- **AIProviderCapabilitiesTests**: Tests for provider capabilities model
  - Capability properties validation
  - Cost and token limit validation
  - Feature support flags

- **AIRequestTests**: Tests for AI request model
  - Request initialization with default values
  - Property setting and validation
  - Options and metadata handling

- **ParsedRecipeResultTests**: Tests for parsed recipe result model
  - Result structure validation
  - Success and error result handling
  - Confidence scoring

- **ParsedRecipeTests**: Tests for parsed recipe model
  - Recipe initialization
  - Property validation
  - Ingredient and instruction handling
  - Time and serving calculations

- **RecipeParsingOptionsTests**: Tests for recipe parsing options
  - Options initialization
  - Token limits and temperature settings
  - Language and extraction options

**Test Files:**
- `AIProviderCapabilitiesTests.cs` - Provider capabilities tests
- `AIRequestTests.cs` - AI request model tests
- `ParsedRecipeResultTests.cs` - Parsed recipe result tests
- `ParsedRecipeTests.cs` - Parsed recipe model tests
- `RecipeParsingOptionsTests.cs` - Recipe parsing options tests

### Provider Tests

**Location:** `Providers/`

Tests for AI provider implementations:

- **LMStudioAIProviderTests**: Tests for LM Studio AI provider
  - Provider initialization
  - Recipe parsing functionality
  - HTTP communication
  - Error handling and retries
  - Configuration validation

**Test Files:**
- `LMStudioAIProviderTests.cs` - LM Studio provider tests

**Note:** Additional provider tests (OpenAI, Anthropic, AWS Bedrock, etc.) may be in the main library or separate test projects.

### Service Tests

**Location:** `Services/`

Tests for AI services and utilities:

- **CostAnalyzerTests**: Tests for cost analysis service
  - Cost calculation for different providers
  - Cost recording and tracking
  - Cost comparison across providers
  - Cost optimization logic

- **PromptBuilderServiceTests**: Tests for prompt building service
  - Recipe parsing prompt generation
  - Custom options integration
  - Prompt formatting and structure
  - Language and instruction handling

- **RecipeValidationServiceTests**: Tests for recipe validation service
  - Recipe validation logic
  - Validation scoring
  - Issue detection and reporting
  - Recipe completeness checks

- **SmartAIProviderSelectorTests**: Tests for intelligent provider selection
  - Cost-optimized selection
  - Performance-optimized selection
  - Reliability-optimized selection
  - Balanced selection strategy
  - Fallback provider selection

**Test Files:**
- `CostAnalyzerTests.cs` - Cost analyzer service tests
- `PromptBuilderServiceTests.cs` - Prompt builder service tests
- `RecipeValidationServiceTests.cs` - Recipe validation service tests
- `SmartAIProviderSelectorTests.cs` - Provider selector tests

## Testing Framework

### Framework and Tools

- **xUnit** (2.6.1) - Primary testing framework
- **xunit.runner.visualstudio** (2.5.3) - Visual Studio test runner
- **Moq** (4.20.69) - Mocking framework for dependencies
- **FluentAssertions** (6.12.0) - Fluent assertion library
- **coverlet.collector** (6.0.0) - Code coverage collection
- **Microsoft.NET.Test.Sdk** (17.8.0) - Test SDK

### Test Patterns

- **Arrange-Act-Assert (AAA)**: All tests follow AAA pattern
- **Mocking**: Extensive use of Moq for dependencies
- **Fluent Assertions**: Readable assertion syntax
- **Integration Tests**: End-to-end workflow testing
- **Unit Tests**: Isolated component testing

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test AMCode.AI.Tests

# Run with coverage
dotnet test AMCode.AI.Tests --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test AMCode.AI.Tests --filter "FullyQualifiedName~AIProviderFactoryTests"

# Run tests in specific category
dotnet test AMCode.AI.Tests --filter "Category=Integration"
```

### Visual Studio

1. Open Test Explorer (Test → Test Explorer)
2. Build solution
3. Run all tests or select specific tests
4. View test results and coverage

### Test Execution

```bash
# From solution root
dotnet test AMCode.sln --filter "FullyQualifiedName~AMCode.AI.Tests"

# From test project directory
cd ailibrary/AMCode.AI.Tests
dotnet test
```

## Test Coverage

### Coverage Areas

- **Factories**: Provider factory creation and registration
- **Models**: All model classes with property validation
- **Providers**: LM Studio provider implementation
- **Services**: All service classes (cost analyzer, prompt builder, validation, selector)
- **Integration**: End-to-end workflows

### Coverage Goals

- **Unit Test Coverage**: 80%+ for all components
- **Integration Test Coverage**: All major workflows
- **Edge Case Coverage**: Error handling and boundary conditions

## Dependencies

### Internal Dependencies

- **AMCode.AI** - The library being tested (project reference)
- **AMCode.Common** - Common utilities used in tests (project reference)

### External Dependencies

- **xUnit** (2.6.1) - Testing framework
- **Moq** (4.20.69) - Mocking framework
- **FluentAssertions** (6.12.0) - Assertion library
- **Microsoft.NET.Test.Sdk** (17.8.0) - Test SDK
- **coverlet.collector** (6.0.0) - Code coverage

## Test Data

### Mock Data

- Mock AI providers for testing selection logic
- Mock HTTP responses for provider testing
- Sample recipe text for parsing tests
- Test configurations for service initialization

### Test Fixtures

- Service provider setup for integration tests
- Mock logger and HTTP client factory
- Test configuration builders

## Example Tests

### Unit Test Example

```csharp
[Fact]
public void ParsedRecipe_ShouldInitializeWithDefaultValues()
{
    // Act
    var recipe = new ParsedRecipe();
    
    // Assert
    recipe.Title.Should().BeEmpty();
    recipe.Ingredients.Should().NotBeNull().And.BeEmpty();
    recipe.Instructions.Should().NotBeNull().And.BeEmpty();
}
```

### Integration Test Example

```csharp
[Fact]
public void ServiceRegistration_ShouldRegisterAllServices()
{
    // Act & Assert
    _serviceProvider.GetService<IRecipeParserService>().Should().NotBeNull();
    _serviceProvider.GetService<IAIProviderFactory>().Should().NotBeNull();
}
```

### Mocking Example

```csharp
[Fact]
public async Task SelectBestProviderAsync_CostOptimized_ShouldSelectCheapestProvider()
{
    // Arrange
    var selector = new SmartAIProviderSelector(providers, logger, costAnalyzer, AIProviderSelectionStrategy.CostOptimized);
    var request = new AIRequest { Text = "Test recipe", EstimatedTokens = 1000 };
    
    // Act
    var result = await selector.SelectOCRProvider(request);
    
    // Assert
    result.Should().NotBeNull();
    result.ProviderName.Should().Be("CheapestProvider");
}
```

## Related Documentation

- **[AMCode.AI](../AMCode.AI/README.md)** - Library being tested
- **[Root README](../../README.md)** - Project overview

## Notes

- Tests use xUnit framework (not NUnit like some other test projects)
- Integration tests require proper configuration setup
- Provider tests may require API keys for full integration testing
- Mock providers are used extensively to avoid external API calls in unit tests

---

**See Also:**

- [AMCode.AI Library](../AMCode.AI/README.md) - Library overview
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
