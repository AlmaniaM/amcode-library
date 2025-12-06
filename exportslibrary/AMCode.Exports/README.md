# AMCode.Exports

**Version:** 1.2.2
**Target Framework:** .NET 9.0
**Last Updated:** 2025-01-27
**Purpose:** Comprehensive export library for creating Excel and CSV files with support for custom column configurations, styling, recipe exports, and ZIP archives

---

## Overview

AMCode.Exports is a flexible and extensible .NET library for exporting data to various formats, primarily Excel (XLSX) and CSV. The library provides a clean architecture with support for custom column configurations, styling, multiple data sources, and specialized export functionality for recipes and shopping lists. It's designed to handle large datasets efficiently with support for splitting exports across multiple files.

## Architecture

The library follows a builder pattern with clear separation of concerns:

- **Export Builders**: High-level builders for creating exports (Excel, CSV, Recipe-specific)
- **Book Builders**: Lower-level builders for constructing workbook structures
- **Books**: Represents the actual export files (ExcelBook, CsvBook)
- **Data Columns**: Column definitions with metadata and styling
- **Results**: Export results with stream management
- **Data Sources**: Factory pattern for different data source types
- **ZIP Support**: Archive creation for multiple file exports

### Key Components

- **SimpleExcelExportBuilder**: Simplified Excel export builder using AMCode.Xlsx
- **RecipeExportBuilder**: Specialized builder for recipe and shopping list exports
- **ExcelBook/CsvBook**: Book implementations for different formats
- **Book Builders**: Advanced builders with styling and configuration
- **Export Results**: Result types for managing export streams
- **ZIP Archives**: Support for creating ZIP files from multiple exports

## Features

- **Excel Export**: Full Excel (XLSX) file generation with styling support
- **CSV Export**: Comma-separated value file generation
- **Recipe Export**: Specialized export functionality for recipe data
- **Shopping List Export**: Extract and export ingredients as shopping lists
- **Custom Column Configuration**: Flexible column mapping and formatting
- **Styling Support**: Column styles, widths, and formatting options
- **Large Dataset Support**: Automatic splitting across multiple files
- **ZIP Archive Support**: Package multiple exports into ZIP files
- **Stream-Based**: Memory-efficient stream-based operations
- **Async/Await**: Full asynchronous support for all operations
- **Type Safety**: Strongly typed interfaces and comprehensive error handling
- **Extensible Architecture**: Easy to extend for new export formats

## Dependencies

### Internal Dependencies

- **AMCode.Common** - Common utilities and result types
- **AMCode.Columns** - Column management and data transformation
- **AMCode.Storage** - Storage abstractions for file operations
- **AMCode.Xlsx** - Excel file generation and manipulation

### External Dependencies

- **Microsoft.Extensions.Logging** (9.0.0) - Logging framework
- **Microsoft.Extensions.Logging.Abstractions** (9.0.0) - Logging abstractions
- **NUnit** (3.13.2) - Testing framework (test dependencies)
- **Moq** (4.20.69) - Mocking framework (test dependencies)

## Project Structure

```
AMCode.Exports/
├── Components/
│   ├── Adapters/                      # Format adapters
│   │   └── XlsxAdapter.cs
│   ├── Book/                          # Book implementations
│   │   ├── Excel/                     # Excel book implementation
│   │   │   ├── ExcelBook.cs
│   │   │   ├── ExcelBookFactory.cs
│   │   │   └── Models/                # Excel-specific models
│   │   ├── Csv/                       # CSV book implementation
│   │   │   ├── CsvBook.cs
│   │   │   ├── CsvBookFactory.cs
│   │   │   └── Models/                # CSV-specific models
│   │   ├── Models/                     # Common book interfaces
│   │   └── Exceptions/                # Book-related exceptions
│   ├── BookBuilder/                   # Advanced book builders
│   │   ├── Excel/                      # Excel book builder
│   │   ├── Csv/                        # CSV book builder
│   │   ├── Actions/                    # Builder actions (styling, etc.)
│   │   ├── Models/                     # Builder models
│   │   ├── BookCompiler.cs
│   │   └── BookBuilderCommon.cs
│   ├── Common/                         # Common utilities
│   │   ├── ExportsCommon.cs
│   │   ├── Exceptions/                 # Common exceptions
│   │   └── Models/                     # Common models
│   ├── DataSources/                    # Data source factories
│   │   ├── FileStreamDataSourceFactory.cs
│   │   ├── MemoryStreamDataSourceFactory.cs
│   │   └── IExportStreamDataSourceFactory.cs
│   ├── ExportBuilder/                  # High-level export builders
│   │   ├── SimpleExcelExportBuilder.cs
│   │   ├── ExcelExportBuilder.cs
│   │   ├── CsvExportBuilder.cs
│   │   └── BookBuilderFactory.cs
│   ├── Extensions/                     # Extension methods
│   │   ├── ExportResultExtensions.cs
│   │   └── FileTypeExtensions.cs
│   ├── Results/                        # Export result types
│   │   ├── InMemoryExportResult.cs
│   │   ├── DataSourceExportResult.cs
│   │   ├── DataSourceExportResultFactory.cs
│   │   └── Models/                     # Result interfaces
│   └── Zip/                            # ZIP archive support
│       ├── ZipArchiveFactory.cs
│       └── IZipArchiveFactory.cs
├── Recipes/                            # Recipe-specific exports
│   ├── RecipeExportBuilder.cs          # Main recipe export builder
│   ├── Interfaces/
│   │   └── IRecipeExportBuilder.cs
│   ├── Models/                         # Recipe models
│   │   ├── Recipe.cs
│   │   ├── RecipeExportOptions.cs
│   │   ├── RecipeColumnConfiguration.cs
│   │   └── ShoppingListItem.cs
│   ├── Extensions/                     # DI extensions
│   │   └── RecipeExportServiceCollectionExtensions.cs
│   └── Tests/                          # Recipe export tests
└── Scripts/                            # Build scripts
```

## Key Interfaces

### IBook<TColumn>

**Location:** `Components/Book/Models/IBook.cs`

**Purpose:** Interface representing a row/cell book/file that can hold data and be saved to a stream.

**Key Methods:**

- `AddData(IList<ExpandoObject>, IEnumerable<IBookDataColumn>, CancellationToken)` - Add data to the book
- `SetColumns(IEnumerable<string>)` - Set column names
- `Save()` - Save book as stream
- `SaveAs(Stream)` - Save book to provided stream

**See Also:** [Book Components](Components/Book/README.md)

### IExportBuilder<TColumn>

**Location:** `Components/Common/Models/IExportBuilder.cs`

**Purpose:** Interface for building different types of exports with support for large datasets.

**Key Methods:**

- `CalculateNumberOfBooks(int totalRowCount)` - Calculate number of books needed
- `CreateExportAsync(string fileName, int totalRowCount, IEnumerable<TColumn> columns, CancellationToken)` - Create export file

**See Also:** [Export Builder Components](Components/ExportBuilder/README.md)

### IRecipeExportBuilder

**Location:** `Recipes/Interfaces/IRecipeExportBuilder.cs`

**Purpose:** Interface for recipe-specific export operations.

**Key Methods:**

- `ExportRecipesAsync(IEnumerable<Recipe>, RecipeExportOptions)` - Export recipes
- `ExportRecipesAsync(IEnumerable<Recipe>, RecipeExportOptions, RecipeColumnConfiguration)` - Export with custom columns
- `ExportShoppingListAsync(IEnumerable<Recipe>, RecipeExportOptions)` - Export shopping list

**See Also:** [Recipe Export Documentation](#recipe-exports)

### IExportResult

**Location:** `Components/Results/Models/IExportResult.cs`

**Purpose:** Interface representing an export file result with stream management.

**Key Properties:**

- `Count` - Number of book entries
- `Name` - Export name
- `FileType` - Type of file (Excel, CSV, ZIP)

**Key Methods:**

- `GetDataAsync()` - Get export data stream
- `SetDataAsync(Stream)` - Set export data stream

**See Also:** [Results Components](Components/Results/README.md)

## Key Classes

### SimpleExcelExportBuilder

**Location:** `Components/ExportBuilder/SimpleExcelExportBuilder.cs`

**Purpose:** Simplified Excel export builder that directly uses AMCode.Xlsx for creating Excel files.

**Key Responsibilities:**

- Create Excel exports with configurable row limits
- Support for large datasets (splits across multiple files)
- Direct integration with AMCode.Xlsx library

**Usage:**

```csharp
var builder = new SimpleExcelExportBuilder(maxRowsPerFile: 1000000);
var result = await builder.CreateExportAsync("export.xlsx", totalRows, columns, cancellationToken);
```

### RecipeExportBuilder

**Location:** `Recipes/RecipeExportBuilder.cs`

**Purpose:** Specialized builder for exporting recipes and shopping lists to Excel format.

**Key Responsibilities:**

- Export recipe collections to Excel
- Export shopping lists from recipes
- Support custom column configurations
- Handle recipe-specific data transformations

**Usage:**

```csharp
var builder = new RecipeExportBuilder(logger);
var result = await builder.ExportRecipesAsync(recipes, options);
```

### ExcelBook

**Location:** `Components/Book/Excel/ExcelBook.cs`

**Purpose:** Implementation of IBook for Excel files with styling and formatting support.

**Key Responsibilities:**

- Manage Excel workbook structure
- Add data rows with column mapping
- Apply column styles and formatting
- Save to stream

**See Also:** [Book Components](Components/Book/README.md)

### CsvBook

**Location:** `Components/Book/Csv/CsvBook.cs`

**Purpose:** Implementation of IBook for CSV files.

**Key Responsibilities:**

- Manage CSV file structure
- Add data rows
- Handle CSV formatting and escaping
- Save to stream

**See Also:** [Book Components](Components/Book/README.md)

## Usage Examples

### Basic Excel Export

```csharp
using AMCode.Exports.ExportBuilder;
using AMCode.Exports.Book;

var builder = new SimpleExcelExportBuilder();
var columns = new List<IExcelDataColumn>
{
    new ExcelDataColumn { WorksheetHeaderName = "Name", DataType = typeof(string) },
    new ExcelDataColumn { WorksheetHeaderName = "Age", DataType = typeof(int) }
};

var result = await builder.CreateExportAsync("users.xlsx", 1000, columns, cancellationToken);
var stream = await result.GetDataAsync();

// Save to file
using var fileStream = File.Create("users.xlsx");
await stream.CopyToAsync(fileStream);
```

### Recipe Export

```csharp
using AMCode.Exports.Recipes;
using AMCode.Exports.Recipes.Models;
using Microsoft.Extensions.Logging;

var logger = LoggerFactory.Create(builder => builder.AddConsole())
    .CreateLogger<RecipeExportBuilder>();

var builder = new RecipeExportBuilder(logger);

var recipes = new List<Recipe>
{
    new Recipe
    {
        Title = "Spaghetti Carbonara",
        Category = "Dinner",
        PrepTimeMinutes = 15,
        CookTimeMinutes = 20,
        Servings = 4,
        Ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "Spaghetti", Amount = "200", Unit = "g" },
            new Ingredient { Name = "Eggs", Amount = "2", Unit = "large" }
        }
    }
};

var options = new RecipeExportOptions
{
    Format = "excel",
    Title = "My Recipe Collection"
};

var result = await builder.ExportRecipesAsync(recipes, options);

if (result.IsSuccess)
{
    using var fileStream = File.Create("recipes.xlsx");
    await result.Value.CopyToAsync(fileStream);
}
```

### Custom Column Configuration

```csharp
var columnConfig = new RecipeColumnConfiguration
{
    IncludeTitle = true,
    IncludeCategory = true,
    IncludeTiming = true,
    IncludeServings = true,
    IncludeIngredients = true,
    IncludeInstructions = false, // Exclude instructions
    IncludeTags = true,
    IncludeMetadata = true
};

var result = await builder.ExportRecipesAsync(recipes, options, columnConfig);
```

### Shopping List Export

```csharp
var shoppingListOptions = new RecipeExportOptions
{
    Format = "excel",
    Title = "Shopping List"
};

var result = await builder.ExportShoppingListAsync(recipes, shoppingListOptions);
```

### Advanced Book Builder (with Styling)

```csharp
using AMCode.Exports.BookBuilder;

var config = new ExcelBookBuilderConfig
{
    MaxRowsPerFile = 1000000,
    ApplyBoldHeaders = true,
    ColumnWidths = new Dictionary<string, int>
    {
        { "Name", 20 },
        { "Age", 10 }
    }
};

var builder = new ExcelBookBuilder(config);
// Use builder to create styled Excel exports
```

## Configuration

### Recipe Export Options

```csharp
var options = new RecipeExportOptions
{
    Format = "excel",                    // Export format
    Title = "Recipe Collection",          // Export title
    IncludeImages = false,               // Include images (future)
    IncludeMetadata = true,              // Include creation dates
    SortBy = "Title",                    // Sort field
    SortAscending = true,                // Sort direction
    GroupBy = "Category",                // Group by field
    ConsolidateIngredients = true        // Consolidate duplicate ingredients
};
```

### Column Configuration

```csharp
var columnConfig = new RecipeColumnConfiguration
{
    IncludeTitle = true,
    IncludeCategory = true,
    IncludeTiming = true,               // Prep and cook time
    IncludeServings = true,
    IncludeIngredients = true,
    IncludeInstructions = true,
    IncludeTags = true,
    IncludeMetadata = true,             // Created/modified dates
    IncludeDifficulty = true,
    IncludeCuisine = true,
    IncludeNotes = true
};
```

### Dependency Injection

```csharp
using AMCode.Exports.Recipes.Extensions;
using Microsoft.Extensions.DependencyInjection;

services.AddRecipeExportServices();
services.AddLogging();
```

## Testing

### Test Projects

- **AMCode.Exports.UnitTests**: Unit tests for export components
- **AMCode.Exports.IntegrationTests**: Integration tests for export workflows
- **AMCode.Exports.SharedTestLibrary**: Shared test utilities

### Running Tests

```bash
# Run all tests
dotnet test AMCode.Exports.UnitTests
dotnet test AMCode.Exports.IntegrationTests

# Run specific test project
dotnet test exportslibrary/AMCode.Exports.UnitTests/AMCode.Exports.UnitTests.csproj
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Book Components](Components/Book/README.md) - Book implementations and factories
- [Book Builder Components](Components/BookBuilder/README.md) - Advanced book builders with styling
- [Export Builder Components](Components/ExportBuilder/README.md) - High-level export builders
- [Results Components](Components/Results/README.md) - Export result types and management

## Recipe Exports

### Default Columns

Excel exports include the following columns by default:

- **Recipe Title**: Name of the recipe
- **Category**: Recipe category
- **Prep Time (min)**: Preparation time
- **Cook Time (min)**: Cooking time
- **Servings**: Number of servings
- **Difficulty**: Difficulty level
- **Cuisine**: Cuisine type
- **Ingredients**: List of ingredients
- **Instructions**: Cooking instructions
- **Tags**: Recipe tags
- **Created Date**: Creation timestamp

### Shopping List Columns

Shopping list exports include:

- **Ingredient**: Ingredient name
- **Amount**: Required amount
- **Unit**: Unit of measurement
- **Optional**: Whether ingredient is optional
- **Notes**: Additional notes

## Related Libraries

- [AMCode.Common](../commonlibrary/AMCode.Common/README.md) - Common utilities and result types
- [AMCode.Columns](../columnslibrary/AMCode.Columns/README.md) - Column management
- [AMCode.Storage](../storagelibrary/AMCode.Storage/README.md) - Storage abstractions
- [AMCode.Xlsx](../xlsxlibrary/AMCode.Xlsx/README.md) - Excel file generation

## Migration Notes

### From Previous Versions

When migrating to version 1.2.2:

1. **Recipe Export Features**: New recipe-specific export functionality added
2. **SimpleExcelExportBuilder**: New simplified Excel builder using AMCode.Xlsx
3. **Dependency Updates**: Updated to .NET 9.0 and latest package versions
4. **Result Pattern**: Uses `Result<T>` pattern for error handling

### Breaking Changes

- Some internal components may have been refactored
- Check test projects for migration examples

## Known Issues

- Some components depend on AMCode.Storage.Local/Memory which may not be available
- Large file exports may require significant memory
- CSV escaping for complex data may need attention

## Future Considerations

- Additional export formats (PDF, JSON, XML)
- Enhanced styling options
- Template-based exports
- Image embedding support
- Performance optimizations for very large datasets
- Streaming exports for memory efficiency

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27
**Maintained By:** Development Team
