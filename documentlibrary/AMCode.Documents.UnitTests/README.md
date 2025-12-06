# AMCode.Documents.UnitTests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive unit and integration tests for the AMCode.Documents library covering PDF, DOCX, Excel, and common document operations

---

## Overview

AMCode.Documents.UnitTests is the test project for the AMCode.Documents library. It provides comprehensive test coverage for document generation functionality including PDF creation, DOCX generation, Excel operations, and common document utilities. The test suite uses NUnit framework and includes unit tests, integration tests, error handling tests, and performance tests.

## Test Framework

- **Testing Framework:** NUnit 3.13.2
- **Test Adapter:** NUnit3TestAdapter 4.2.1
- **Mocking Framework:** Moq 4.20.69
- **Code Coverage:** Coverlet 3.1.2
- **Test SDK:** Microsoft.NET.Test.Sdk 17.1.0

## Test Categories

### Common Tests (`Common/`)

Tests for common document utilities and shared components:

- **ColorTests.cs** - Color creation and manipulation (RGB, ARGB, hex conversion)
- **FontStyleTests.cs** - Font style and formatting tests
- **BorderStyleTests.cs** - Border style and formatting tests
- **PageSizeTests.cs** - Page size and dimension tests
- **MarginsTests.cs** - Margin and spacing tests
- **EnumTests.cs** - Enumeration value tests

### PDF Tests (`Pdf/`)

Comprehensive tests for PDF document generation:

- **PdfFactoryTests.cs** - PDF factory and provider management
- **PdfDocumentTests.cs** - PDF document creation and manipulation
- **PdfPageTests.cs** - PDF page operations
- **PdfBuilderTests.cs** - PDF builder pattern tests
- **PdfProviderTests.cs** - Provider implementations (QuestPDF, iTextSharp)
- **PdfValidatorTests.cs** - PDF validation and verification
- **PdfIntegrationTests.cs** - PDF integration scenarios
- **Performance Tests** (`Pdf/Performance/`):
  - PdfPerformanceTests.cs - General PDF performance
  - PdfOptimizedPerformanceTests.cs - Optimized PDF operations

### OpenXML/Excel Tests (`OpenXml/`)

Tests for Excel document operations using OpenXML:

- **ExcelApplicationTests.cs** - Excel application initialization
- **WorkbookTests.cs** - Workbook creation and operations
- **WorkbooksTests.cs** - Workbook collection management
- **WorksheetTests.cs** - Worksheet operations
- **WorksheetsTests.cs** - Worksheet collection management
- **RangeTests.cs** - Cell range operations
- **StyleTests.cs** - Excel styling and formatting
- **BordersTests.cs** (disabled) - Border formatting tests

### Error Handling Tests (`ErrorHandling/`)

Tests for error handling and edge cases:

- **InvalidInputTests.cs** - Invalid input handling (negative values, nulls, empty strings)
- **FileSystemErrorTests.cs** - File system error scenarios
- **FormatErrorTests.cs** - Format and parsing errors
- **MemoryErrorTests.cs** - Memory-related error handling

### Integration Tests (`Integration/`)

Cross-format and multi-format integration tests:

- **FormatConversionTests.cs** - Format conversion between Excel, DOCX, and PDF
- **FormatComparisonTests.cs** (disabled) - Format comparison tests
- **CrossFormatCompatibilityTests.cs** (disabled) - Cross-format compatibility
- **MultiFormatDocumentTests.cs** (disabled) - Multi-format document operations
- **PerformanceComparisonTests.cs** (disabled) - Performance comparisons
- **StyleConsistencyTests.cs** (disabled) - Style consistency across formats

### Performance Tests (`Performance/`)

Performance and scalability tests:

- **PdfPerformanceTests.cs** - PDF generation performance
- **DocxPerformanceTests.cs** - DOCX generation performance
- **ExcelPerformanceTests.cs** - Excel generation performance
- **RecipeProcessingPerformanceTests.cs** - Recipe-specific performance tests
- **ConcurrentProcessingTests.cs** - Concurrent document processing
- **MemoryUsageTests.cs** - Memory usage and optimization
- **TestHelpers.cs** - Performance test utilities

### Factory Tests (`Factories/`)

Tests for document factory implementations:

- **PdfFactoryTests.cs** - PDF factory operations
- **ExcelFactoryTests.cs** - Excel factory operations
- **DocumentFactoryTests.cs** (disabled) - General document factory

### Utilities (`Utilities/`)

Test utilities and helper classes.

## Test Structure

```
AMCode.Documents.UnitTests/
├── Common/                    # Common utility tests
├── Pdf/                       # PDF generation tests
│   ├── Integration/          # PDF integration tests
│   └── Performance/         # PDF performance tests
├── OpenXml/                  # Excel/OpenXML tests
├── ErrorHandling/            # Error handling tests
├── Integration/              # Cross-format integration tests
├── Performance/              # Performance and scalability tests
├── Factories/                # Factory pattern tests
├── Utilities/                # Test utilities
├── TestingEnvironmentInit.cs # Test environment setup
└── AMCode.Documents.UnitTests.csproj
```

## Dependencies

### Internal Dependencies

- **AMCode.Documents** - The library being tested (project reference)
- **AMCode.Common** (1.0.0) - Common utilities used in tests

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Test adapter for Visual Studio
- **Moq** (4.20.69) - Mocking framework
- **Microsoft.NET.Test.Sdk** (17.1.0) - Test SDK
- **coverlet.collector** (3.1.2) - Code coverage collection

## Running Tests

### Run All Tests

```bash
# From solution root
dotnet test documentlibrary/AMCode.Documents.UnitTests/AMCode.Documents.UnitTests.csproj

# From test project directory
cd documentlibrary/AMCode.Documents.UnitTests
dotnet test
```

### Run Specific Test Categories

```bash
# Run only PDF tests
dotnet test --filter "FullyQualifiedName~Pdf"

# Run only Common tests
dotnet test --filter "FullyQualifiedName~Common"

# Run only Error Handling tests
dotnet test --filter "FullyQualifiedName~ErrorHandling"

# Run only Integration tests
dotnet test --filter "FullyQualifiedName~Integration"
```

### Run with Code Coverage

```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Run Performance Tests

```bash
# Run performance tests
dotnet test --filter "FullyQualifiedName~Performance"
```

## Test Environment Setup

The test project includes `TestingEnvironmentInit.cs` which handles test environment initialization:

- **License Registration**: Registers Syncfusion XLSX library license from environment variable `DL_XLSX_LIBRARY_LICENSE`
- **One-Time Setup**: Runs once before all tests using NUnit's `[OneTimeSetUp]` attribute

### Environment Variables

- `DL_XLSX_LIBRARY_LICENSE` - Syncfusion XLSX library license key (required for Excel tests)

## Test Coverage

### Current Test Coverage Areas

- ✅ PDF document generation (QuestPDF, iTextSharp providers)
- ✅ Excel/OpenXML operations (workbooks, worksheets, ranges, styles)
- ✅ Common utilities (colors, fonts, borders, page sizes, margins)
- ✅ Error handling and edge cases
- ✅ Format conversion and integration
- ✅ Performance and scalability
- ✅ Factory patterns
- ✅ Concurrent processing

### Disabled Tests

Some tests are currently disabled (marked with `.disabled` extension or excluded from compilation):

- `Factories/DocumentFactoryTests.cs.disabled`
- `OpenXml/BordersTests.cs.disabled`
- Several Integration tests (excluded in `.csproj`)

These tests may be re-enabled as the library evolves.

## Test Patterns

### Common Test Patterns

```csharp
[TestFixture]
public class ExampleTests
{
    [SetUp]
    public void Setup()
    {
        // Initialize test dependencies
    }

    [TearDown]
    public void TearDown()
    {
        // Cleanup test resources
    }

    [Test]
    public void Should_DoSomething_When_Condition()
    {
        // Arrange
        var input = "test";
        
        // Act
        var result = MethodUnderTest(input);
        
        // Assert
        Assert.That(result, Is.Not.Null);
    }
}
```

### Mocking with Moq

```csharp
var mockLogger = new Mock<IPdfLogger>();
mockLogger.Setup(x => x.Log(It.IsAny<string>())).Verifiable();

var service = new PdfService(mockLogger.Object);
// Test service...
```

## Integration with CI/CD

### GitHub Actions Example

```yaml
- name: Run Document Tests
  run: |
    dotnet test documentlibrary/AMCode.Documents.UnitTests/AMCode.Documents.UnitTests.csproj \
      --configuration Release \
      --collect:"XPlat Code Coverage" \
      --results-directory:"TestResults"
```

## Known Issues

- Some integration tests are disabled pending library feature completion
- Performance tests may require longer execution time
- Excel tests require Syncfusion license environment variable

## Related Documentation

- [AMCode.Documents Library README](../AMCode.Documents/README.md) - Main library documentation
- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

---

**See Also:**

- [AMCode.Documents Library](../AMCode.Documents/README.md) - Library being tested
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
