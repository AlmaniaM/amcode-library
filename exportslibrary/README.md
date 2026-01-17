# AMCode Exports Library

A comprehensive library for building Excel and CSV file exports with advanced styling and formatting capabilities.

## Features

- **Excel Export**: Create professional Excel (.xlsx) files with custom styling, formatting, and multiple worksheets
- **CSV Export**: Generate CSV files with proper encoding and formatting
- **Advanced Styling**: Apply custom fonts, colors, borders, and column widths
- **Large Dataset Support**: Handle large datasets with automatic sheet splitting and ZIP compression
- **Async Operations**: Full async/await support for better performance
- **Flexible Data Sources**: Support for various data input formats including ExpandoObject collections

## Quick Start

### Recipe Excel Export

The library includes specialized recipe export functionality:

```csharp
using AMCode.Exports.Recipes;
using AMCode.Exports.Recipes.Models;
using Microsoft.Extensions.Logging;

// Create logger (you can use any ILogger implementation)
var logger = new MockLogger<RecipeExportBuilder>();

// Create recipe export builder
var builder = new RecipeExportBuilder(logger);

// Create sample recipes
var recipes = new List<Recipe>
{
    new Recipe
    {
        Title = "Chocolate Chip Cookies",
        Category = "Dessert",
        PrepTimeMinutes = 15,
        CookTimeMinutes = 12,
        Servings = 24,
        Difficulty = "Easy",
        Cuisine = "American",
        Ingredients = new List<RecipeIngredient>
        {
            new RecipeIngredient { Name = "Flour", Amount = "2", Unit = "cups", Text = "2 cups all-purpose flour" },
            new RecipeIngredient { Name = "Sugar", Amount = "1", Unit = "cup", Text = "1 cup granulated sugar" }
        },
        Instructions = new List<string> { "Preheat oven to 375°F", "Mix ingredients", "Bake for 12 minutes" },
        Tags = new List<string> { "dessert", "cookies", "easy" },
        CreatedAt = DateTime.Now
    }
};

// Export recipes to Excel
var options = new RecipeExportOptions 
{ 
    Format = "excel", 
    Title = "My Recipe Collection" 
};

var result = await builder.ExportRecipesAsync(recipes, options);

if (result.IsSuccess)
{
    // Save the Excel file
    using (var fileStream = File.Create("recipes.xlsx"))
    {
        await result.Value.CopyToAsync(fileStream);
    }
    Console.WriteLine("Excel file created successfully!");
}
else
{
    Console.WriteLine($"Export failed: {result.ErrorMessage}");
}
```

### Shopping List Export

Export recipe ingredients as a shopping list:

```csharp
// Export shopping list
var shoppingListResult = await builder.ExportShoppingListAsync(recipes, options);

if (shoppingListResult.IsSuccess)
{
    using (var fileStream = File.Create("shopping-list.xlsx"))
    {
        await shoppingListResult.Value.CopyToAsync(fileStream);
    }
    Console.WriteLine("Shopping list created successfully!");
}
```

### Custom Column Configuration

Control which recipe fields are included in the export:

```csharp
var columnConfig = new RecipeColumnConfiguration
{
    IncludeTitle = true,
    IncludeCategory = true,
    IncludeTiming = true,
    IncludeServings = true,
    IncludeIngredients = true,
    IncludeInstructions = true,
    IncludeTags = false,
    IncludeMetadata = false
};

var result = await builder.ExportRecipesAsync(recipes, options, columnConfig);
```

### Basic Excel Export (Advanced)

For more complex scenarios, use the full Excel export builder:

```csharp
using AMCode.Exports.Book;
using AMCode.Exports.ExportBuilder;

// Create export builder
var builderConfig = new ExcelBookBuilderConfig
{
    FetchDataAsync = async (start, count, cancellationToken) => 
    {
        // Your data fetching logic here
        return await GetDataFromDatabase(start, count, cancellationToken);
    },
    MaxRowsPerDataFetch = 1000
};

var exportBuilder = new ExcelExportBuilder(builderConfig);

// Define columns
var columns = new List<IExcelDataColumn>
{
    new ExcelDataColumn { DataFieldName = "Name", WorksheetHeaderName = "Full Name" },
    new ExcelDataColumn { DataFieldName = "Email", WorksheetHeaderName = "Email Address" },
    new ExcelDataColumn { DataFieldName = "CreatedDate", WorksheetHeaderName = "Created Date" }
};

// Create export
var result = await exportBuilder.CreateExportAsync("Users", totalRowCount, columns, cancellationToken);
```

### Basic CSV Export

```csharp
using AMCode.Exports.Book;
using AMCode.Exports.ExportBuilder;

// Create CSV export builder
var builderConfig = new BookBuilderConfig
{
    FetchDataAsync = async (start, count, cancellationToken) => 
    {
        return await GetDataFromDatabase(start, count, cancellationToken);
    },
    MaxRowsPerDataFetch = 1000
};

var exportBuilder = new CsvExportBuilder(builderConfig);

// Define columns
var columns = new List<ICsvDataColumn>
{
    new CsvDataColumn { DataFieldName = "Name", WorksheetHeaderName = "Full Name" },
    new CsvDataColumn { DataFieldName = "Email", WorksheetHeaderName = "Email Address" }
};

// Create export
var result = await exportBuilder.CreateExportAsync("Users", totalRowCount, columns, cancellationToken);
```

## Advanced Features

### Custom Styling

```csharp
// Define custom styles
var columnStyles = new List<IColumnStyle>
{
    new ColumnStyle 
    { 
        Name = "Name", 
        Style = new StyleParam { Bold = true, FontSize = 12 },
        Width = 150
    },
    new ColumnStyle 
    { 
        Name = "Email", 
        Style = new StyleParam { Color = Color.LightBlue },
        Width = 200
    }
};

var builderConfig = new ExcelBookBuilderConfig
{
    FetchDataAsync = fetchDataFunction,
    MaxRowsPerDataFetch = 1000,
    ColumnStyles = columnStyles
};
```

### Large Dataset Handling

The library automatically handles large datasets by:
- Splitting data across multiple Excel sheets when row limits are exceeded
- Creating ZIP archives when multiple files are generated
- Providing progress tracking through cancellation tokens

## Installation

This library is distributed as a NuGet package. Add it to your project:

```xml
<PackageReference Include="AMCode.Exports" Version="1.2.2" />
```

## Dependencies

- AMCode.Common (2.1.0+)
- AMCode.Columns (1.2.1+)
- AMCode.Storage (1.1.2+)
- AMCode.Documents.Xlsx (1.1.0+)

## License

This library is part of the AMCode suite of libraries.

## Support

For questions, issues, or contributions, please refer to the development documentation or contact the AMCode development team.