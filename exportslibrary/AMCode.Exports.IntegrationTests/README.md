# AMCode.Exports.IntegrationTests

**Version:** 1.0  
**Target Framework:** .NET 9.0  
**Last Updated:** 2025-01-27  
**Purpose:** Integration tests for AMCode.Exports library

---

## Overview

AMCode.Exports.IntegrationTests provides comprehensive integration test coverage for the AMCode.Exports library. These tests validate end-to-end export functionality including CSV and Excel export generation, book builder operations, file storage integration, and data transformation workflows.

## Test Framework

- **Testing Framework:** NUnit 3.13.2
- **Mocking Framework:** Moq 4.17.1
- **Code Coverage:** Coverlet 3.1.2
- **Test SDK:** Microsoft.NET.Test.Sdk 17.1.0

## Test Structure

```
AMCode.Exports.IntegrationTests/
├── Components/                   # Component integration tests
│   ├── BookBuilder/             # Book builder integration tests
│   │   ├── BookCompiler/        # Book compiler tests
│   │   │   ├── BookCompilerCsvTest.cs
│   │   │   ├── BookCompilerCsvFileStorageTest.cs
│   │   │   ├── BookCompilerExcelTest.cs
│   │   │   ├── BookCompilerExcelFileStorageTest.cs
│   │   │   └── Models/
│   │   │       └── TestHelper.cs
│   │   ├── Csv/                 # CSV book builder tests
│   │   │   └── CsvBookBuilderDataTest.cs
│   │   └── Excel/               # Excel book builder tests
│   │       └── ExcelBookBuilderDataTest.cs
│   └── ExportBuilder/           # Export builder integration tests
│       ├── CsvExportBuilderTest.cs
│       └── ExcelExportBuilderTest.cs
├── TestingEnvironmentInit.cs    # Test environment initialization
└── AMCode.Exports.IntegrationTests.csproj
```

## Test Categories

### Book Builder Integration Tests

Tests for book builder components that compile data into export formats:

#### BookCompiler Tests

- **BookCompilerCsvTest**: CSV book compilation integration tests
- **BookCompilerCsvFileStorageTest**: CSV book compilation with file storage integration
- **BookCompilerExcelTest**: Excel book compilation integration tests
- **BookCompilerExcelFileStorageTest**: Excel book compilation with file storage integration

**Key Test Scenarios:**
- Book compilation with data fetching
- Large dataset handling
- File storage integration
- Data transformation workflows
- Column formatting and styling
- Multi-sheet Excel generation

#### BookBuilder Data Tests

- **CsvBookBuilderDataTest**: CSV book builder data handling tests
- **ExcelBookBuilderDataTest**: Excel book builder data handling tests

**Key Test Scenarios:**
- Data column mapping
- Data type handling
- Value formatting
- Header generation
- Row processing

### Export Builder Integration Tests

Tests for export builder components that create export files:

- **CsvExportBuilderTest**: CSV export builder integration tests
- **ExcelExportBuilderTest**: Excel export builder integration tests

**Key Test Scenarios:**
- End-to-end CSV export generation
- End-to-end Excel export generation
- Data source integration
- Export result handling
- File generation and validation
- Large dataset exports

## Test Environment Setup

### TestingEnvironmentInit

**Location:** `TestingEnvironmentInit.cs`

One-time setup fixture that initializes the testing environment, including license registration for Syncfusion Excel components.

**Key Features:**
- License registration from environment variables
- Test environment configuration
- One-time initialization before all tests

**Environment Variable:**
- `DL_XLSX_LIBRARY_LICENSE`: Syncfusion license key for Excel generation

## Dependencies

### Internal Dependencies

- **AMCode.Exports** - The library being tested
- **AMCode.Exports.SharedTestLibrary** - Shared test utilities and helpers
- **AMCode.Columns** - Column management
- **AMCode.Common** - Common utilities
- **AMCode.Storage** - Storage abstractions
- **AMCode.Xlsx** - Excel generation library

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Test adapter
- **Moq** (4.17.1) - Mocking framework
- **Microsoft.NET.Test.Sdk** (17.1.0) - Test SDK
- **coverlet.collector** (3.1.2) - Code coverage collection
- **Syncfusion.XlsIO** - Excel generation (requires license)

## Running Tests

### Prerequisites

1. Set environment variable for Syncfusion license:
   ```bash
   export DL_XLSX_LIBRARY_LICENSE="your-license-key"
   ```

2. Ensure all dependencies are restored:
   ```bash
   dotnet restore exportslibrary/AMCode.Exports.IntegrationTests
   ```

### Run All Tests

```bash
dotnet test exportslibrary/AMCode.Exports.IntegrationTests
```

### Run Specific Test Category

```bash
# Run book builder tests
dotnet test --filter "FullyQualifiedName~BookBuilder"

# Run export builder tests
dotnet test --filter "FullyQualifiedName~ExportBuilder"

# Run CSV tests
dotnet test --filter "FullyQualifiedName~Csv"

# Run Excel tests
dotnet test --filter "FullyQualifiedName~Excel"

# Run file storage tests
dotnet test --filter "FullyQualifiedName~FileStorage"
```

### Run with Code Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run Specific Test

```bash
dotnet test --filter "FullyQualifiedName~BookCompilerCsvTest"
```

## Test Coverage

The integration test suite provides comprehensive coverage for:

- ✅ CSV book compilation
- ✅ Excel book compilation
- ✅ CSV export generation
- ✅ Excel export generation
- ✅ File storage integration
- ✅ Data fetching and transformation
- ✅ Column formatting and styling
- ✅ Large dataset handling
- ✅ Multi-sheet Excel generation
- ✅ Export result validation

## Test Patterns

### Data Fetching Pattern

Tests use async data fetching delegates:

```csharp
FetchDataAsync = (start, count, _) => Task.Run<IList<ExpandoObject>>(() =>
{
    var data = Enumerable.Range(start, count)
        .Select(index =>
        {
            var row = new ExpandoObject()
                .AddOrUpdatePropertyWithValue("Column1", $"Value 1{index}")
                .AddOrUpdatePropertyWithValue("Column2", $"Value 2{index}");
            return row;
        }).ToList();
    return data;
})
```

### Column Configuration Pattern

Tests configure columns with formatters:

```csharp
columns = Enumerable.Range(1, 3).Select(
    index => new CsvDataColumn
    {
        Formatter = valueFormatterMoq.Object,
        DataFieldName = $"Column{index}",
        WorksheetHeaderName = $"Column {index}"
    });
```

### Book Builder Configuration Pattern

Tests configure book builders with data fetching:

```csharp
var bookBuilder = new CsvBookBuilder(new CsvBookFactory(), new BookBuilderConfig
{
    FetchDataAsync = dataFetchDelegate,
    MaxRowsPerDataFetch = 10
});
```

## Integration Points

### Storage Integration

Tests validate integration with storage providers:
- File system storage
- Azure Blob Storage (via AMCode.Storage)
- AWS S3 (via AMCode.Storage)

### Data Source Integration

Tests validate integration with data sources:
- ExpandoObject data sources
- Dynamic data fetching
- Paginated data retrieval

### Column Integration

Tests validate integration with column management:
- AMCode.Columns integration
- Value formatting
- Column mapping

## Known Issues

- Requires Syncfusion license for Excel tests
- Some tests may require specific file system permissions
- Large dataset tests may take significant time

## Troubleshooting

### License Issues

If Excel tests fail with license errors:
1. Verify `DL_XLSX_LIBRARY_LICENSE` environment variable is set
2. Check license key validity
3. Ensure license is set before running tests

### File System Issues

If file storage tests fail:
1. Check write permissions in test directories
2. Verify temp directory access
3. Check disk space availability

### Test Data Issues

If data fetching tests fail:
1. Verify ExpandoObject extensions are available
2. Check data transformation logic
3. Validate column mappings

## Related Documentation

- [AMCode.Exports README](../AMCode.Exports/README.md) - Main library documentation
- [AMCode.Exports.UnitTests README](../AMCode.Exports.UnitTests/README.md) - Unit test documentation
- [AMCode.Exports.SharedTestLibrary README](../AMCode.Exports.SharedTestLibrary/README.md) - Shared test library

## Contributing

When adding new integration tests:

1. Follow the existing test structure and naming conventions
2. Use async data fetching patterns for data sources
3. Mock external dependencies appropriately
4. Include both positive and negative test cases
5. Test edge cases and boundary conditions
6. Ensure tests are independent and can run in any order
7. Document any required environment setup

---

**See Also:**

- [Root README](../../../README.md) - Project overview
- [AMCode.Exports Documentation](../AMCode.Exports/README.md) - Library being tested

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
