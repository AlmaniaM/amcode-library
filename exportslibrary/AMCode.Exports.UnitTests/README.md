# AMCode.Exports.UnitTests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive unit tests for AMCode.Exports library

---

## Overview

AMCode.Exports.UnitTests is the test project for the AMCode.Exports library. It provides comprehensive unit test coverage for all components in the AMCode.Exports library, including Excel and CSV book operations, book builders, export builders, data sources, results, extensions, and recipe-specific export functionality.

## Test Structure

The test project mirrors the structure of the AMCode.Exports library, with test classes organized by component:

```
AMCode.Exports.UnitTests/
├── Components/
│   ├── Book/                  # Book implementation tests
│   │   ├── Csv/              # CSV book tests
│   │   └── Excel/            # Excel book tests
│   ├── BookBuilder/          # Book builder tests
│   │   ├── Actions/          # Builder action tests
│   │   ├── BookCompiler/     # Book compiler tests
│   │   ├── Csv/              # CSV builder tests
│   │   ├── Excel/             # Excel builder tests
│   │   └── Mocks/             # Mock classes
│   ├── Extensions/           # Extension method tests
│   ├── DataSources/          # Data source factory tests (some excluded)
│   ├── ExportBuilder/        # Export builder tests (some excluded)
│   └── Results/              # Export result tests (some excluded)
├── Global/                   # Global test helpers
├── Recipes/                  # Recipe export tests
└── AMCode.Exports.UnitTests.csproj
```

**Note:** Some test files are excluded from compilation (see `.csproj` for details). This documentation covers the active test files.

## Test Categories

### Book Tests

**Location:** `Components/Book/`

Tests for book implementations (Excel and CSV):

#### CSV Book Tests

**Location:** `Components/Book/Csv/`

- **CsvBookAddDataTest**: CSV book data addition tests
  - Adding rows to CSV books
  - Data formatting
  - Column handling

- **CsvBookColumnsTest**: CSV book column configuration tests
  - Column definition
  - Column ordering
  - Column metadata

- **CsvBookSaveTest**: CSV book save operation tests
  - File saving
  - Stream generation
  - File format validation

**Test Files:**
- `CsvBookAddDataTest.cs` - CSV data addition tests
- `CsvBookColumnsTest.cs` - CSV column tests
- `CsvBookSaveTest.cs` - CSV save tests

#### Excel Book Tests

**Location:** `Components/Book/Excel/`

- **ExcelBookAddDataTest**: Excel book data addition tests
  - Adding rows to Excel books
  - Data formatting
  - Cell value handling

- **ExcelBookColumnsTest**: Excel book column configuration tests
  - Column definition
  - Column ordering
  - Column styling

- **ExcelBookRangeTest**: Excel book range operation tests
  - Range selection
  - Range operations
  - Cell range handling

- **ExcelBookSaveTest**: Excel book save operation tests
  - File saving
  - Stream generation
  - Excel format validation

- **ExcelBookTotalsTest**: Excel book totals calculation tests
  - Total row generation
  - Formula calculations
  - Summary operations

**Test Files:**
- `ExcelBookAddDataTest.cs` - Excel data addition tests
- `ExcelBookColumnsTest.cs` - Excel column tests
- `ExcelBookRangeTest.cs` - Excel range tests
- `ExcelBookSaveTest.cs` - Excel save tests
- `ExcelBookTotalsTest.cs` - Excel totals tests

### Book Builder Tests

**Location:** `Components/BookBuilder/`

Tests for book builder functionality:

- **BookBuilderCommonHelpersTest**: Common book builder helper tests
  - Builder configuration
  - Data fetching
  - Builder utilities

- **BookCompiler Tests**: Book compilation tests
  - **BookCompilerGenericTypeTest**: Generic type compilation
  - **BookCompilerCsvTest**: CSV compilation
  - **BookCompilerExcelTest**: Excel compilation
  - **BookCompilerExceptionsTest**: Compilation exception handling

- **CSV Book Builder Tests**: `Components/BookBuilder/Csv/`
  - **CsvBookBuilderDataTest**: CSV builder data operations
  - **CsvBookBuilderCustomStreamDataSourceTest**: Custom stream data source
  - **CsvBookBuilderConstructorExceptionsTest**: Constructor exception handling
  - **CsvBookBuilderExceptionsTest**: General exception handling

- **Excel Book Builder Tests**: `Components/BookBuilder/Excel/`
  - **ExcelBookBuilderDataTest**: Excel builder data operations
  - **ExcelBookBuilderCustomDataSourceFactoryTest**: Custom data source factory
  - **ExcelBookBuilderConstructorExceptionsTest**: Constructor exception handling
  - **ExcelBookBuilderExceptionsTest**: General exception handling

- **Builder Action Tests**: `Components/BookBuilder/Actions/`
  - **ApplyBoldHeadersActionTest**: Bold header styling action
  - **ApplyColumnStylesActionTest**: Column style application
  - **ApplyColumnWidthActionTest**: Column width configuration

**Test Files:**
- `BookBuilderCommonHelpersTest.cs` - Common builder helpers
- `BookCompiler/BookCompilerGenericTypeTest.cs` - Generic compilation
- `BookCompiler/BookCompilerCsvTest.cs` - CSV compilation
- `BookCompiler/BookCompilerExcelTest.cs` - Excel compilation
- `BookCompiler/BookCompilerExceptionsTest.cs` - Compilation exceptions
- `Csv/CsvBookBuilderDataTest.cs` - CSV builder data
- `Csv/CsvBookBuilderCustomStreamDataSourceTest.cs` - CSV custom streams
- `Csv/CsvBookBuilderConstructorExceptionsTest.cs` - CSV constructor exceptions
- `Csv/CsvBookBuilderExceptionsTest.cs` - CSV exceptions
- `Excel/ExcelBookBuilderDataTest.cs` - Excel builder data
- `Excel/ExcelBookBuilderCustomDataSourceFactoryTest.cs` - Excel custom factories
- `Excel/ExcelBookBuilderConstructorExceptionsTest.cs` - Excel constructor exceptions
- `Excel/ExcelBookBuilderExceptionsTest.cs` - Excel exceptions
- `Actions/ApplyBoldHeadersActionTest.cs` - Bold headers action
- `Actions/ApplyColumnStylesActionTest.cs` - Column styles action
- `Actions/ApplyColumnWidthActionTest.cs` - Column width action
- `Mocks/TestBookBuilder.cs` - Mock book builder
- `Mocks/TestDataColumn.cs` - Mock data column

### Extension Tests

**Location:** `Components/Extensions/`

Tests for export result extensions:

- **ExportResultExtensionTest**: Export result extension method tests
  - Result manipulation
  - Stream operations
  - Result utilities

**Test Files:**
- `ExportResultExtensionTest.cs` - Export result extension tests
- `ExportResultExtensions.cs` - Extension method implementations

### Data Source Tests

**Location:** `Components/DataSources/`

Tests for data source factories:

- **FileStreamDataSourceFactoryTest**: File stream data source factory tests
  - File stream creation
  - Factory pattern implementation
  - Stream management

- **MemoryStreamDataSourceFactoryTest**: Memory stream data source factory tests
  - Memory stream creation
  - Factory pattern implementation
  - In-memory stream handling

**Test Files:**
- `FileStreamDataSourceFactoryTest.cs` - File stream factory tests
- `MemoryStreamDataSourceFactoryTest.cs` - Memory stream factory tests

### Export Builder Tests

**Location:** `Components/ExportBuilder/`

Tests for export builder functionality:

- **BookBuilderFactoryTest**: Book builder factory tests
  - Factory creation
  - Builder instantiation
  - Factory configuration

- **CsvExportBuilderTest**: CSV export builder tests
  - CSV export creation
  - Builder configuration
  - CSV-specific functionality

- **ExcelExportBuilderTest**: Excel export builder tests
  - Excel export creation
  - Builder configuration
  - Excel-specific functionality

**Test Files:**
- `BookBuilderFactoryTest.cs` - Book builder factory tests
- `CsvExportBuilderTest.cs` - CSV export builder tests
- `ExcelExportBuilderTest.cs` - Excel export builder tests

### Result Tests

**Location:** `Components/Results/`

Tests for export result functionality:

- **DataSourceExportResultTest**: Data source export result tests
  - Result creation
  - Stream management
  - Result properties

- **DataSourceExportResultFactoryTest**: Export result factory tests
  - Factory creation
  - Result instantiation
  - Factory configuration

**Test Files:**
- `DataSourceExportResultTest.cs` - Export result tests
- `DataSourceExportResultFactoryTest.cs` - Result factory tests

### Recipe Export Tests

**Location:** `Recipes/`

Specialized tests for recipe export functionality:

- **RecipeExportBuilderExcelTests**: Recipe export builder Excel tests
  - Recipe data export
  - Excel format for recipes
  - Recipe-specific column handling
  - Shopping list export

**Test Files:**
- `RecipeExportBuilderExcelTests.cs` - Recipe export Excel tests

## Test Infrastructure

### Global Test Helpers

**Location:** `Global/`

- **GlobalTestHelper**: Global test helper utilities
  - Excel test resource creation
  - Common test setup
  - Resource management

- **ExcelTestResources**: Excel test resource container
  - Excel application references
  - Workbook and worksheet references
  - Style references

**Test Files:**
- `GlobalTestHelper.cs` - Global test helper
- `ExcelTestResources.cs` - Excel test resources

### Mock Classes

**Location:** `Components/BookBuilder/Mocks/`

- **TestBookBuilder**: Mock book builder for testing
- **TestDataColumn**: Mock data column for testing

## Dependencies

### Internal Dependencies

- **AMCode.Exports**: The library being tested (project reference)
- **AMCode.Exports.SharedTestLibrary**: Shared test utilities (project reference)

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Visual Studio test adapter
- **Microsoft.NET.Test.Sdk** (17.1.0) - Test SDK
- **Moq** (4.20.69) - Mocking framework
- **coverlet.collector** (3.1.2) - Code coverage collection

### Library Dependencies (via AMCode.Exports)

- **AMCode.Documents.Xlsx** - Excel file generation
- **AMCode.Columns** - Column definitions
- **AMCode.Common** - Common utilities
- **AMCode.Storage** - Storage abstractions
- **Syncfusion.XlsIO** - Excel file operations

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test AMCode.Exports.UnitTests.csproj

# Run tests with verbose output
dotnet test AMCode.Exports.UnitTests.csproj --verbosity normal

# Run specific test class
dotnet test AMCode.Exports.UnitTests.csproj --filter "FullyQualifiedName~ExcelBookSaveTest"

# Run Excel tests only
dotnet test AMCode.Exports.UnitTests.csproj --filter "FullyQualifiedName~Excel"

# Run CSV tests only
dotnet test AMCode.Exports.UnitTests.csproj --filter "FullyQualifiedName~Csv"

# Run tests with code coverage
dotnet test AMCode.Exports.UnitTests.csproj --collect:"XPlat Code Coverage"
```

### Visual Studio

1. Open the solution in Visual Studio
2. Open Test Explorer (Test → Test Explorer)
3. Build the solution
4. Run all tests or select specific test categories

### Test Explorer

Tests are organized by namespace and component:

- `AMCode.Exports.UnitTests.Book.*` - Book implementation tests
- `AMCode.Exports.UnitTests.BookBuilder.*` - Book builder tests
- `AMCode.Exports.UnitTests.Extensions.*` - Extension tests
- `AMCode.Exports.UnitTests.Global.*` - Global test helpers

## Test Coverage

The test project provides comprehensive coverage for:

- ✅ Excel book operations (add data, columns, ranges, save, totals)
- ✅ CSV book operations (add data, columns, save)
- ✅ Book builder functionality (CSV and Excel)
- ✅ Book compiler (generic, CSV, Excel, exceptions)
- ✅ Builder actions (styling, column width, headers)
- ✅ Export builders (CSV, Excel)
- ✅ Data source factories (file stream, memory stream)
- ✅ Export results and factories
- ✅ Extension methods
- ✅ Recipe export functionality
- ✅ Error handling and exception scenarios
- ✅ Stream operations
- ✅ Async/await patterns

## Test Patterns

### Common Test Patterns

1. **Arrange-Act-Assert (AAA)**: Standard test structure
2. **Test Fixtures**: NUnit test fixtures for test class organization
3. **SetUp/TearDown**: Test initialization and cleanup (especially for Excel resources)
4. **Mock-Based Testing**: Use of Moq for dependency mocking
5. **Builder Pattern Testing**: Testing builder pattern implementations
6. **Stream Testing**: Testing stream-based operations
7. **Exception Testing**: Comprehensive exception scenario coverage

### Example Test Structure

```csharp
[TestFixture]
public class ExcelBookSaveTest
{
    private IExcelApplication excelApplication;
    private ExcelBook excelBook;

    [SetUp]
    public void SetUp()
    {
        // Arrange: Create Excel application and book
        excelApplication = new ExcelApplication();
        excelBook = new ExcelBook(excelApplication);
    }

    [TearDown]
    public void TearDown() => excelApplication.Dispose();

    [Test]
    public void ShouldGetStreamWhenSaving()
    {
        // Arrange
        excelBook.SetColumns(new List<string> { "Column1" });
        
        // Act
        var stream = excelBook.Save();
        
        // Assert
        Assert.IsTrue(stream.Length > 0);
    }
}
```

### Excel Test Pattern

```csharp
[TestFixture]
public class ExcelBookTest
{
    private ExcelTestResources resources;

    [SetUp]
    public void SetUp()
    {
        // Use global test helper for Excel resources
        resources = GlobalTestHelper.GetExcelTestResources();
    }

    [TearDown]
    public void TearDown()
    {
        // Dispose Excel resources
        resources.Application?.Dispose();
    }

    [Test]
    public void ShouldCreateExcelBook()
    {
        // Test using Excel resources
    }
}
```

## Excluded Tests

Some test files are excluded from compilation (see `.csproj` for details):

- Some BookBuilder tests (excluded components)
- Some DataSources tests
- Some ExportBuilder tests
- Some Results tests

These exclusions are intentional and reflect components that have been removed or refactored from the main library.

## Continuous Integration

Tests are designed to run in CI/CD pipelines:

- **No External Dependencies**: Tests use mocks and in-memory operations
- **Isolated Tests**: Each test is independent and can run in parallel
- **Fast Execution**: Tests are optimized for speed
- **Code Coverage**: Coverlet integration for coverage reporting
- **Resource Cleanup**: Proper disposal of Excel resources in TearDown

## Troubleshooting

### Common Issues

1. **Excel Resource Leaks**: Ensure proper disposal in TearDown methods
2. **Stream Disposal**: Verify streams are properly disposed after tests
3. **Mock Setup Errors**: Check Moq setup for all dependencies
4. **File System Issues**: Use in-memory streams when possible
5. **Excel Application Errors**: Use GlobalTestHelper for consistent Excel setup

### Debugging Tests

1. Set breakpoints in test methods or code under test
2. Use Test Explorer to debug specific tests
3. Check test output for detailed error messages
4. Review Excel resource disposal if tests hang
5. Verify stream operations are properly closed

## Related Documentation

- [AMCode.Exports README](../AMCode.Exports/README.md) - Library being tested
- [AMCode.Exports.SharedTestLibrary README](../AMCode.Exports.SharedTestLibrary/README.md) - Shared test utilities
- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

## Contributing

When adding new tests:

1. Follow existing test patterns and structure
2. Use GlobalTestHelper for Excel resources
3. Ensure proper resource disposal in TearDown
4. Use Moq for dependency mocking
5. Test both success and exception scenarios
6. Include stream operation tests
7. Update this README if adding new test categories

---

**See Also:**

- [AMCode.Exports Library](../AMCode.Exports/README.md) - Library documentation
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
