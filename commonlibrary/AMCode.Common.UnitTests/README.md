# AMCode.Common.UnitTests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive unit tests for AMCode.Common library

---

## Overview

AMCode.Common.UnitTests is the test project for the AMCode.Common library. It provides comprehensive unit test coverage for all components in the AMCode.Common library, including IO operations (CSV, JSON, ZIP), email functionality, filtering, extension methods, dynamic object helpers, and utility classes.

## Test Structure

The test project mirrors the structure of the AMCode.Common library, with test classes organized by component:

```
AMCode.Common.UnitTests/
├── Components/
│   ├── Dynamic/              # Dynamic object helper tests
│   ├── Emails/               # Email client tests
│   ├── Extensions/           # Extension method tests
│   ├── Filter/               # Filter component tests
│   ├── IO/                   # File I/O tests
│   │   ├── CSV/             # CSV reader/writer tests
│   │   ├── JSON/            # JSON file reader/writer tests
│   │   └── Zip/             # ZIP archive tests
│   └── Util/                # Utility class tests
├── Globals/                  # Global test helpers
└── AMCode.Common.UnitTests.csproj
```

## Test Categories

### Dynamic Object Tests

**Location:** `Components/Dynamic/`

Tests for dynamic object helper functionality:

- **DynamicObjectHelperTest**: Tests for dynamic object creation and manipulation
  - CSV data reader integration
  - Dynamic object property access
  - Type conversion with dynamic objects

**Test Files:**
- `DynamicObjectHelperTest.cs` - Main dynamic object helper tests
- `Models/CSVDataReader.cs` - CSV data reader model for testing
- `Models/TestHelper.cs` - Test helper utilities
- `MockFiles/` - CSV mock files for testing

### Email Tests

**Location:** `Components/Emails/`

Tests for email client functionality:

- **MailGunClientTest**: Tests for MailGun email client implementation
  - Email sending functionality
  - Configuration validation
  - Error handling

**Test Files:**
- `MailGunClientTest.cs` - MailGun client tests

### Extension Method Tests

**Location:** `Components/Extensions/`

Comprehensive tests for all extension methods:

- **StringExtensionsTest**: String extension method tests
  - Null/empty/whitespace checks
  - Case-insensitive comparisons
  - Delimiter splitting with comma handling
  - String manipulation utilities

- **EnumerableExtensionsTest**: Collection extension tests
  - Collection operations
  - Filtering and transformation
  - Aggregation methods

- **DictionaryExtensionsTest**: Dictionary extension tests
  - Dictionary operations
  - Key/value manipulation
  - Merge and update operations

- **DynamicObjectExtensionsTest**: Dynamic object extension tests
  - Property access
  - Value retrieval with type conversion
  - Dynamic property manipulation

- **FilterExtensionsTest**: Filter extension tests
  - Filter building
  - Filter validation
  - Filter operations

- **FilterItemExtensionsTest**: Filter item extension tests
  - Filter item creation
  - Filter item validation
  - Filter item operations

- **FilterNameExtensionsTest**: Filter name extension tests
  - Filter name validation
  - Filter name operations

- **ObjectExtensionsTest**: Object extension tests
  - Object cloning
  - Property access
  - Type conversion

- **StreamExtensionsTest**: Stream extension tests
  - Stream operations
  - Stream reading/writing
  - Stream utilities

- **TypeExtensionsTest**: Type extension tests
  - Type information
  - Type operations
  - Reflection utilities

- **MethodInfoExtensionsTest**: Method info extension tests
  - Method information
  - Method invocation
  - Reflection operations

**Test Files:**
- `StringExtensionsTest.cs` - String extension tests
- `EnumerableExtensionsTest.cs` - Enumerable extension tests
- `DictionaryExtensionsTest.cs` - Dictionary extension tests
- `DynamicObjectExtensionsTest.cs` - Dynamic object extension tests
- `FilterExtensionsTest.cs` - Filter extension tests
- `FilterItemExtensionsTest.cs` - Filter item extension tests
- `FilterNameExtensionsTest.cs` - Filter name extension tests
- `ObjectExtensionsTest.cs` - Object extension tests
- `StreamExtensionsTest.cs` - Stream extension tests
- `TypeExtensionsTest.cs` - Type extension tests
- `MethodInfoExtensionsTest.cs` - Method info extension tests
- `Mocks/` - Mock classes for testing

### Filter Tests

**Location:** `Components/Filter/`

Tests for filter component functionality:

- **FilterDeserializationTest**: Filter deserialization tests
  - JSON filter deserialization
  - Filter structure validation
  - Filter parsing

**Test Files:**
- `FilterDeserializationTest.cs` - Filter deserialization tests
- `MockFiles/filter-data.json` - Filter test data
- `Models/TestHelper.cs` - Test helper utilities

### IO Tests

**Location:** `Components/IO/`

Comprehensive file I/O operation tests:

#### CSV Tests

**Location:** `Components/IO/CSV/`

- **CSVReaderTest**: CSV reader functionality tests
  - Empty CSV file handling
  - CSV file reading with various delimiters (comma, pipe)
  - Header handling (with/without spaces, with/without headers)
  - Data type conversion
  - Quote handling
  - Edge cases (commas in data, empty cells)

- **CSVWriterTest**: CSV writer functionality tests
  - CSV file writing
  - Delimiter handling
  - Quote escaping
  - Multiple row writing

**Test Files:**
- `CSVReaderTest.cs` - CSV reader tests
- `CSVWriterTest.cs` - CSV writer tests
- `MockFiles/` - CSV test files (10+ test CSV files)
- `Models/` - Test models and helpers
- `TestWorkDirectory/` - Temporary test files

#### JSON Tests

**Location:** `Components/IO/JSON/`

- **JsonFileReaderTest**: JSON file reader tests
  - JSON file reading
  - JSON object parsing
  - Error handling

- **JsonFileWriterTest**: JSON file writer tests
  - JSON file writing
  - JSON serialization
  - File operations

**Test Files:**
- `JsonFileReaderTest.cs` - JSON reader tests
- `JsonFileWriterTest.cs` - JSON writer tests
- `MockFiles/JsonFileReaderTest_Json_Object_Mock.json` - JSON test data
- `Models/JsonObjectMock.cs` - JSON test models
- `TestWorkDirectory/` - Temporary test files

#### ZIP Tests

**Location:** `Components/IO/Zip/`

- **ZipArchiveTest**: ZIP archive functionality tests
  - ZIP archive creation
  - File compression
  - Archive extraction
  - Stream operations

**Test Files:**
- `ZipArchiveTest.cs` - ZIP archive tests
- `Models/TestHelper.cs` - Test helper utilities
- `TestWorkDirectory/` - Temporary test files

#### Path Utilities Tests

**Location:** `Components/IO/`

- **PathUtilsTest**: Path utility function tests
  - Path manipulation
  - File path operations
  - Directory operations

**Test Files:**
- `PathUtilsTest.cs` - Path utility tests

#### Text Field Parser Tests

**Location:** `Components/IO/`

- **TextFieldParserTest**: Text field parser tests
  - Text field parsing
  - Delimiter handling
  - Field extraction

**Test Files:**
- `TextFieldParserTest.cs` - Text field parser tests

### Utility Tests

**Location:** `Components/Util/`

Tests for utility classes:

- **ExceptionHeaderTest**: Exception header utility tests
  - Exception formatting
  - Error message generation
  - Stack trace handling

- **MethodInfoTest**: Method info utility tests
  - Method reflection
  - Method invocation
  - Method information retrieval

- **EnvironmentTest**: Environment utility tests
  - Environment variable access
  - System information
  - Environment configuration

**Test Files:**
- `Exceptions/ExceptionHeaderTest.cs` - Exception header tests
- `Reflection/MethodInfoTest.cs` - Method info tests
- `System/EnvironmentTest.cs` - Environment tests
- `Mocks/MethodTemplateMock.cs` - Mock classes

## Test Infrastructure

### Test Helper Base

**Location:** `Globals/TestHelperBase.cs`

Abstract base class providing common test helper functionality:

- **GetMockFilesPath**: Get path to mock files directory
- **GetTestDirectoryPath**: Get test directory path (abstract, implemented by test helpers)
- **GetTestWorkDirectoryPath**: Get or create test work directory
- **GetFilePath**: Get absolute path to a mock file

### Test Helpers

Each test category has its own test helper class that extends `TestHelperBase`:

- **CSV TestHelper**: `Components/IO/CSV/Models/TestHelper.cs`
- **JSON TestHelper**: `Components/IO/JSON/Models/TestHelper.cs`
- **ZIP TestHelper**: `Components/IO/Zip/Models/TestHelper.cs`
- **Dynamic TestHelper**: `Components/Dynamic/Models/TestHelper.cs`
- **Filter TestHelper**: `Components/Filter/Models/TestHelper.cs`

## Test Data

### Mock Files

The test project includes various mock files for testing:

- **CSV Mock Files**: 10+ CSV files with different formats, delimiters, and edge cases
- **JSON Mock Files**: JSON test data files
- **Filter Mock Files**: Filter JSON test data

### Test Work Directories

Temporary directories for test file operations:

- `Components/IO/CSV/TestWorkDirectory/` - CSV test output
- `Components/IO/JSON/TestWorkDirectory/` - JSON test output
- `Components/IO/Zip/TestWorkDirectory/` - ZIP test output

## Dependencies

### Internal Dependencies

- **AMCode.Common**: The library being tested (project reference)

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Visual Studio test adapter
- **Microsoft.NET.Test.Sdk** (17.1.0) - Test SDK
- **Moq** (4.18.2) - Mocking framework

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test AMCode.Common.UnitTests.csproj

# Run tests with verbose output
dotnet test AMCode.Common.UnitTests.csproj --verbosity normal

# Run specific test class
dotnet test AMCode.Common.UnitTests.csproj --filter "FullyQualifiedName~StringExtensionsTest"

# Run tests with code coverage
dotnet test AMCode.Common.UnitTests.csproj --collect:"XPlat Code Coverage"
```

### Visual Studio

1. Open the solution in Visual Studio
2. Open Test Explorer (Test → Test Explorer)
3. Build the solution
4. Run all tests or select specific tests

### Test Explorer

Tests are organized by namespace and test class in Test Explorer:

- `AMCode.Common.UnitTests.Extensions.*` - Extension method tests
- `AMCode.Common.UnitTests.IO.*` - IO operation tests
- `AMCode.Common.UnitTests.Dynamic.*` - Dynamic object tests
- `AMCode.Common.UnitTests.Filter.*` - Filter tests
- `AMCode.Common.UnitTests.Util.*` - Utility tests

## Test Coverage

The test project provides comprehensive coverage for:

- ✅ All extension methods (String, Enumerable, Dictionary, Object, etc.)
- ✅ All IO operations (CSV, JSON, ZIP)
- ✅ Email functionality
- ✅ Filter components
- ✅ Dynamic object helpers
- ✅ Utility classes
- ✅ Error handling and edge cases

## Test Patterns

### Common Test Patterns

1. **Arrange-Act-Assert (AAA)**: Standard test structure
2. **Test Fixtures**: NUnit test fixtures for test class organization
3. **Test Cases**: Parameterized tests using `[TestCase]` attribute
4. **SetUp/TearDown**: Test initialization and cleanup
5. **Mock Files**: External test data files for complex scenarios
6. **Test Helpers**: Reusable test utility classes

### Example Test Structure

```csharp
[TestFixture]
public class StringExtensionsTest
{
    [Test]
    public void ShouldPassIsNullEmptyOrWhiteSpace()
    {
        // Arrange
        string nullString = null;
        
        // Act & Assert
        Assert.IsTrue("".IsNullEmptyOrWhiteSpace());
        Assert.IsTrue(nullString.IsNullEmptyOrWhiteSpace());
        Assert.IsTrue(" ".IsNullEmptyOrWhiteSpace());
    }

    [TestCase("|")]
    [TestCase("&")]
    [TestCase("#")]
    public void ShouldSplitStringWhileIgnoringCommas(string delimiter)
    {
        // Arrange
        var text = $"1, 2{delimiter}3{delimiter}4,5,6";
        
        // Act
        var splitText = text.SplitIgnoreComma(delimiter);
        
        // Assert
        Assert.AreEqual(3, splitText.Length);
        Assert.AreEqual("1, 2", splitText[0]);
    }
}
```

## Continuous Integration

Tests are designed to run in CI/CD pipelines:

- **No External Dependencies**: Tests don't require external services
- **Isolated Tests**: Each test is independent and can run in parallel
- **Mock Data**: All test data is included in the project
- **Fast Execution**: Tests are optimized for speed

## Troubleshooting

### Common Issues

1. **Mock Files Not Found**: Ensure mock files are copied to output directory (configured in `.csproj`)
2. **Test Work Directory Errors**: Test helpers automatically create work directories
3. **Path Issues**: Use `TestHelper` classes for path resolution
4. **File Locking**: Ensure tests properly dispose of file streams

### Debugging Tests

1. Set breakpoints in test methods
2. Use Test Explorer to debug specific tests
3. Check test output for detailed error messages
4. Review mock file contents if file-based tests fail

## Related Documentation

- [AMCode.Common README](../AMCode.Common/README.md) - Library being tested
- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

## Contributing

When adding new tests:

1. Follow existing test patterns and structure
2. Use appropriate test helpers for file operations
3. Include mock files for complex test scenarios
4. Ensure tests are isolated and can run independently
5. Add test data to appropriate MockFiles directories
6. Update this README if adding new test categories

---

**See Also:**

- [AMCode.Common Library](../AMCode.Common/README.md) - Library documentation
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

