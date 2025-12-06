# AMCode.OCR.Tests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive unit tests for AMCode.OCR library

---

## Overview

AMCode.OCR.Tests is the test project for the AMCode.OCR library. It provides comprehensive unit test coverage for all components in the AMCode.OCR library, including OCR providers, services, factories, models, and the smart provider selection system.

## Test Structure

The test project mirrors the structure of the AMCode.OCR library, with test classes organized by component:

```
AMCode.OCR.Tests/
├── Providers/                  # OCR provider tests
├── Services/                   # Service implementation tests
├── Factories/                  # Factory pattern tests
├── Models/                     # Model validation tests
└── AMCode.OCR.Tests.csproj
```

## Test Categories

### Provider Tests

**Location:** `Providers/`

Tests for OCR provider implementations:

- **SimpleOCRProviderTests**: Test OCR provider implementation tests
  - Provider name and properties
  - Availability checking
  - Image processing
  - Health checking
  - Cost estimation
  - Capabilities validation

- **TestOCRProvider**: Test implementation of IOCRProvider
  - Extends GenericOCRProvider
  - Provides mock OCR functionality for testing
  - No internet required
  - Fast processing simulation
  - Configurable capabilities

**Note:** Some provider test files are disabled (`.cs.disabled`):
- `AzureComputerVisionOCRServiceTests.cs.disabled`
- `SimpleAzureOCRServiceTests.cs.disabled`

These are likely integration tests that require actual cloud provider credentials.

**Test Files:**
- `SimpleOCRProviderTests.cs` - Test provider tests
- `TestOCRProvider.cs` - Test provider implementation
- `AzureComputerVisionOCRServiceTests.cs.disabled` - Azure provider tests (disabled)
- `SimpleAzureOCRServiceTests.cs.disabled` - Simple Azure tests (disabled)

### Service Tests

**Location:** `Services/`

Tests for service implementations:

- **SmartOCRProviderSelectorTests**: Smart provider selector tests
  - Provider selection strategies (PerformanceOptimized, CostOptimized, Balanced)
  - Provider availability checking
  - Capability matching
  - Cost analysis
  - Performance evaluation
  - Fallback provider selection

- **CostAnalyzerTests**: Cost analyzer service tests
  - Cost calculation
  - Cost comparison
  - Cost optimization
  - Provider cost analysis

**Test Files:**
- `SmartOCRProviderSelectorTests.cs` - Smart selector tests
- `CostAnalyzerTests.cs` - Cost analyzer tests

### Factory Tests

**Location:** `Factories/`

Tests for factory pattern implementations:

- **OCRProviderSelectorFactoryTests**: Provider selector factory tests
  - Factory creation
  - Selector instantiation
  - Strategy configuration
  - Provider registration

**Test Files:**
- `OCRProviderSelectorFactoryTests.cs` - Selector factory tests

### Model Tests

**Location:** `Models/`

Tests for data model validation:

- **OCRResultTests**: OCR result model tests
  - Default value initialization
  - Property setting and validation
  - Text block handling
  - Metadata management
  - Confidence scoring
  - Language detection
  - Processing time tracking
  - Cost calculation

- **OCRRequestTests**: OCR request model tests
  - Request creation
  - Option validation
  - Image size limits
  - Language preferences
  - Feature requirements

- **TextBlockTests**: Text block model tests
  - Text block creation
  - Bounding box handling
  - Confidence scoring
  - Text type detection (handwritten vs printed)

- **BoundingBoxTests**: Bounding box model tests
  - Coordinate validation
  - Box calculations
  - Position and size properties

- **OCRProviderCapabilitiesTests**: Provider capabilities model tests
  - Capability flags
  - Language support
  - Feature availability
  - Performance metrics

**Test Files:**
- `OCRResultTests.cs` - OCR result tests
- `OCRRequestTests.cs` - OCR request tests
- `TextBlockTests.cs` - Text block tests
- `BoundingBoxTests.cs` - Bounding box tests
- `OCRProviderCapabilitiesTests.cs` - Capabilities tests

## Test Infrastructure

### Test Provider

**Location:** `Providers/TestOCRProvider.cs`

A test implementation of `IOCRProvider` that extends `GenericOCRProvider`:

- **Purpose**: Provides mock OCR functionality for unit testing
- **Features**:
  - No internet required
  - Fast processing simulation (50ms delay)
  - Configurable capabilities
  - Test result generation
  - Health check simulation

**Key Properties:**
- `ProviderName`: "Test OCR Provider"
- `RequiresInternet`: false
- `Capabilities`: Full test capability configuration

**Usage:**
```csharp
var provider = new TestOCRProvider(mockLogger.Object, mockHttpClientFactory.Object);
var result = await provider.ProcessImageAsync(imageStream);
```

## Dependencies

### Internal Dependencies

- **AMCode.OCR**: The library being tested (project reference)

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Visual Studio test adapter
- **Microsoft.NET.Test.Sdk** (17.8.0) - Test SDK
- **Moq** (4.20.69) - Mocking framework
- **FluentAssertions** (6.12.0) - Fluent assertion library
- **Microsoft.Extensions.Logging.Abstractions** (8.0.0) - Logging abstractions
- **Microsoft.Extensions.Http** (8.0.0) - HTTP client extensions
- **Microsoft.Extensions.Options** (8.0.0) - Options pattern support
- **coverlet.collector** (6.0.0) - Code coverage collection

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test AMCode.OCR.Tests.csproj

# Run tests with verbose output
dotnet test AMCode.OCR.Tests.csproj --verbosity normal

# Run specific test class
dotnet test AMCode.OCR.Tests.csproj --filter "FullyQualifiedName~SimpleOCRProviderTests"

# Run provider tests only
dotnet test AMCode.OCR.Tests.csproj --filter "FullyQualifiedName~Providers"

# Run service tests only
dotnet test AMCode.OCR.Tests.csproj --filter "FullyQualifiedName~Services"

# Run model tests only
dotnet test AMCode.OCR.Tests.csproj --filter "FullyQualifiedName~Models"

# Run tests with code coverage
dotnet test AMCode.OCR.Tests.csproj --collect:"XPlat Code Coverage"
```

### Visual Studio

1. Open the solution in Visual Studio
2. Open Test Explorer (Test → Test Explorer)
3. Build the solution
4. Run all tests or select specific test categories

### Test Explorer

Tests are organized by namespace and component:

- `AMCode.OCR.Tests.Providers.*` - Provider tests
- `AMCode.OCR.Tests.Services.*` - Service tests
- `AMCode.OCR.Tests.Factories.*` - Factory tests
- `AMCode.OCR.Tests.Models.*` - Model tests

## Test Coverage

The test project provides comprehensive coverage for:

- ✅ Provider implementations (test provider)
- ✅ Provider selection strategies
- ✅ Smart provider selector
- ✅ Cost analyzer
- ✅ Factory patterns
- ✅ All model classes (OCRResult, OCRRequest, TextBlock, BoundingBox, Capabilities)
- ✅ Error handling and edge cases
- ✅ Async/await patterns
- ✅ Cancellation token support

## Test Patterns

### Common Test Patterns

1. **Arrange-Act-Assert (AAA)**: Standard test structure
2. **Fluent Assertions**: Using FluentAssertions for readable assertions
3. **Mock-Based Testing**: Extensive use of Moq for dependency mocking
4. **Test Fixtures**: NUnit test fixtures for test class organization
5. **SetUp Methods**: Common setup for test dependencies
6. **Async Testing**: Comprehensive async/await test coverage

### Example Test Structure

```csharp
[TestFixture]
public class SimpleOCRProviderTests
{
    private Mock<ILogger> _mockLogger = null!;
    private Mock<IHttpClientFactory> _mockHttpClientFactory = null!;
    private TestOCRProvider _provider = null!;

    [SetUp]
    public void Setup()
    {
        // Arrange: Set up mocks and provider
        _mockLogger = new Mock<ILogger>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _provider = new TestOCRProvider(_mockLogger.Object, _mockHttpClientFactory.Object);
    }

    [Test]
    public void ProviderName_ShouldReturnCorrectName()
    {
        // Act
        var result = _provider.ProviderName;

        // Assert
        result.Should().Be("Test OCR Provider");
    }
}
```

### Fluent Assertions Pattern

```csharp
[Test]
public void OCRResult_ShouldInitializeWithDefaultValues()
{
    // Act
    var result = new OCRResult();

    // Assert
    result.Text.Should().BeEmpty();
    result.TextBlocks.Should().NotBeNull();
    result.TextBlocks.Should().BeEmpty();
    result.Confidence.Should().Be(0.0);
    result.ProcessedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
}
```

## Disabled Tests

Some test files are disabled (`.cs.disabled` extension):

- **AzureComputerVisionOCRServiceTests.cs.disabled**: Azure provider integration tests
- **SimpleAzureOCRServiceTests.cs.disabled**: Simple Azure provider tests

These tests likely require actual Azure credentials and are disabled for unit test execution. They may be enabled for integration testing scenarios.

## Continuous Integration

Tests are designed to run in CI/CD pipelines:

- **No External Dependencies**: Tests use mocks and test providers, no actual cloud services required
- **Isolated Tests**: Each test is independent and can run in parallel
- **Fast Execution**: Tests are optimized for speed with mocked dependencies
- **Code Coverage**: Coverlet integration for coverage reporting
- **Fluent Assertions**: Readable test failures for easier debugging

## Troubleshooting

### Common Issues

1. **Mock Setup Errors**: Ensure all Moq mocks are properly configured in SetUp
2. **Async Test Failures**: Verify proper async/await usage and cancellation token handling
3. **Fluent Assertions Errors**: Check assertion syntax and expected values
4. **Provider Availability**: Test provider is always available, but verify mock setup for other providers

### Debugging Tests

1. Set breakpoints in test methods or code under test
2. Use Test Explorer to debug specific tests
3. Check test output for detailed FluentAssertions error messages
4. Review mock setup if tests fail unexpectedly
5. Verify async operations complete properly

## Related Documentation

- [AMCode.OCR README](../AMCode.OCR/README.md) - Library being tested
- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

## Contributing

When adding new tests:

1. Follow existing test patterns and structure
2. Use FluentAssertions for readable assertions
3. Use Moq for dependency mocking
4. Use TestOCRProvider for provider testing
5. Test both success and exception scenarios
6. Include async/await patterns
7. Update this README if adding new test categories

---

**See Also:**

- [AMCode.OCR Library](../AMCode.OCR/README.md) - Library documentation
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
