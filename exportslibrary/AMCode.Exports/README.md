# AMCode.Exports

A comprehensive .NET library for exporting data to various formats, with a focus on Excel and CSV exports. This library provides a flexible and extensible framework for creating data exports with support for custom column configurations, styling, and multiple output formats.

## Features

- **Excel Export**: Full Excel file generation with support for custom column configurations
- **Recipe Export**: Specialized export functionality for recipe data and shopping lists
- **Custom Column Configuration**: Flexible column mapping and formatting options
- **Type Safety**: Strongly typed interfaces and comprehensive error handling
- **Extensible Architecture**: Easy to extend for new export formats and data types

## Installation

The library is available as a NuGet package. Add the following to your project file:

```xml
<PackageReference Include="AMCode.Exports" Version="1.0.0" />
```

## Quick Start

### Basic Excel Export

```csharp
using AMCode.Exports.Recipes;
using AMCode.Exports.Recipes.Models;
using Microsoft.Extensions.Logging;

// Create a logger (using your preferred logging framework)
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<RecipeExportBuilder>();

// Create the export builder
var exportBuilder = new RecipeExportBuilder(logger);

// Create sample recipes
var recipes = new List<Recipe>
{
    new Recipe
    {
        Title = "Spaghetti Carbonara",
        Category = "Pasta",
        PrepTimeMinutes = 15,
        CookTimeMinutes = 20,
        Servings = 4,
        Difficulty = "Medium",
        Cuisine = "Italian",
        Ingredients = new List<Ingredient>
        {
            new Ingredient { Name = "Spaghetti", Amount = "200", Unit = "g" },
            new Ingredient { Name = "Eggs", Amount = "2", Unit = "large" },
            new Ingredient { Name = "Pancetta", Amount = "100", Unit = "g" }
        },
        Instructions = "Cook pasta. Fry pancetta. Mix eggs and cheese. Combine all.",
        Tags = new List<string> { "dinner", "quick" },
        CreatedAt = DateTime.Now
    }
};

// Export to Excel
var options = new RecipeExportOptions
{
    Format = "excel",
    Title = "My Recipe Collection"
};

var result = await exportBuilder.ExportRecipesAsync(recipes, options);

if (result.IsSuccess)
{
    // Save the Excel file
    using var fileStream = File.Create("recipes.xlsx");
    await result.Value.CopyToAsync(fileStream);
    Console.WriteLine("Excel file created successfully!");
}
else
{
    Console.WriteLine($"Export failed: {result.Error}");
}
```

### Custom Column Configuration

```csharp
// Create custom column configuration
var columnConfig = new RecipeColumnConfiguration
{
    IncludeTitle = true,
    IncludeCategory = true,
    IncludeTiming = true,
    IncludeServings = true,
    IncludeIngredients = true,
    IncludeInstructions = false, // Exclude instructions for a summary view
    IncludeTags = true,
    IncludeMetadata = true
};

// Export with custom columns
var customResult = await exportBuilder.ExportRecipesAsync(recipes, options, columnConfig);
```

### Shopping List Export

```csharp
// Export ingredients as a shopping list
var shoppingListOptions = new RecipeExportOptions
{
    Format = "excel",
    Title = "Shopping List"
};

var shoppingListResult = await exportBuilder.ExportShoppingListAsync(recipes, shoppingListOptions);
```

## API Reference

### RecipeExportBuilder

The main class for creating recipe exports.

#### Constructor

```csharp
public RecipeExportBuilder(ILogger<RecipeExportBuilder> logger)
```

#### Methods

##### ExportRecipesAsync

Exports a collection of recipes to the specified format.

```csharp
public async Task<Result<Stream>> ExportRecipesAsync(
    IEnumerable<Recipe> recipes, 
    RecipeExportOptions options)
```

**Parameters:**
- `recipes`: Collection of recipes to export
- `options`: Export configuration options

**Returns:** `Result<Stream>` containing the export data or error information

##### ExportRecipesAsync (with custom columns)

Exports recipes with custom column configuration.

```csharp
public async Task<Result<Stream>> ExportRecipesAsync(
    IEnumerable<Recipe> recipes, 
    RecipeExportOptions options,
    RecipeColumnConfiguration columnConfig)
```

**Parameters:**
- `recipes`: Collection of recipes to export
- `options`: Export configuration options
- `columnConfig`: Custom column configuration

**Returns:** `Result<Stream>` containing the export data or error information

##### ExportShoppingListAsync

Exports recipe ingredients as a shopping list.

```csharp
public async Task<Result<Stream>> ExportShoppingListAsync(
    IEnumerable<Recipe> recipes, 
    RecipeExportOptions options)
```

**Parameters:**
- `recipes`: Collection of recipes to extract ingredients from
- `options`: Export configuration options

**Returns:** `Result<Stream>` containing the shopping list export data or error information

### Models

#### Recipe

Represents a recipe with all its properties.

```csharp
public class Recipe
{
    public string Title { get; set; }
    public string Category { get; set; }
    public int PrepTimeMinutes { get; set; }
    public int CookTimeMinutes { get; set; }
    public int Servings { get; set; }
    public string Difficulty { get; set; }
    public string Cuisine { get; set; }
    public List<Ingredient> Ingredients { get; set; }
    public string Instructions { get; set; }
    public List<string> Tags { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string Notes { get; set; }
}
```

#### Ingredient

Represents an ingredient in a recipe.

```csharp
public class Ingredient
{
    public string Name { get; set; }
    public string Amount { get; set; }
    public string Unit { get; set; }
    public string Notes { get; set; }
    public bool IsOptional { get; set; }
}
```

#### RecipeExportOptions

Configuration options for recipe exports.

```csharp
public class RecipeExportOptions
{
    public string Format { get; set; }           // "excel" or "csv"
    public string Title { get; set; }            // Export title
    public bool IncludeImages { get; set; }      // Include images (future feature)
    public bool IncludeMetadata { get; set; }    // Include creation/modification dates
    public string SortBy { get; set; }           // Sort field
    public bool SortAscending { get; set; }      // Sort direction
    public bool IncludeNutritionInfo { get; set; } // Include nutrition data (future feature)
    public bool IncludeCookingTips { get; set; }  // Include cooking tips (future feature)
    public string GroupBy { get; set; }          // Group by field
    public bool ConsolidateIngredients { get; set; } // Consolidate duplicate ingredients
}
```

#### RecipeColumnConfiguration

Configuration for customizing which columns to include in exports.

```csharp
public class RecipeColumnConfiguration
{
    public bool IncludeTitle { get; set; }
    public bool IncludeCategory { get; set; }
    public bool IncludeTiming { get; set; }      // Prep and cook time
    public bool IncludeServings { get; set; }
    public bool IncludeIngredients { get; set; }
    public bool IncludeInstructions { get; set; }
    public bool IncludeTags { get; set; }
    public bool IncludeMetadata { get; set; }    // Created/modified dates
    public bool IncludeDifficulty { get; set; }
    public bool IncludeCuisine { get; set; }
    public bool IncludeRating { get; set; }      // Future feature
    public bool IncludeNotes { get; set; }
}
```

## Excel Export Features

### Default Columns

When using the default configuration, Excel exports include the following columns:

- **Recipe Title**: The name of the recipe
- **Category**: Recipe category (e.g., "Dinner", "Dessert")
- **Prep Time (min)**: Preparation time in minutes
- **Cook Time (min)**: Cooking time in minutes
- **Servings**: Number of servings
- **Difficulty**: Difficulty level (Easy, Medium, Hard)
- **Cuisine**: Cuisine type (Italian, Mexican, etc.)
- **Ingredients**: List of ingredients
- **Instructions**: Cooking instructions
- **Tags**: Recipe tags
- **Created Date**: When the recipe was created

### Custom Column Configuration

You can customize which columns appear in your Excel export by using the `RecipeColumnConfiguration` class:

```csharp
var columnConfig = new RecipeColumnConfiguration
{
    IncludeTitle = true,
    IncludeCategory = true,
    IncludeTiming = true,        // Includes both prep and cook time
    IncludeServings = true,
    IncludeIngredients = true,
    IncludeInstructions = false, // Exclude instructions for a summary view
    IncludeTags = true,
    IncludeMetadata = true,      // Includes creation date
    IncludeDifficulty = true,
    IncludeCuisine = true,
    IncludeNotes = true
};
```

### Shopping List Columns

When exporting shopping lists, the following columns are included:

- **Ingredient**: Ingredient name
- **Amount**: Required amount
- **Unit**: Unit of measurement
- **Optional**: Whether the ingredient is optional
- **Notes**: Additional notes about the ingredient

## Error Handling

The library uses the `Result<T>` pattern for robust error handling:

```csharp
var result = await exportBuilder.ExportRecipesAsync(recipes, options);

if (result.IsSuccess)
{
    // Use the exported data
    using var stream = result.Value;
    // Process the stream...
}
else
{
    // Handle the error
    Console.WriteLine($"Export failed: {result.Error}");
    // Log the error, show user message, etc.
}
```

## Dependencies

- **AMCode.Xlsx**: Excel file generation and manipulation
- **AMCode.Common**: Common utilities and result types
- **Microsoft.Extensions.Logging**: Logging framework

## Examples

### Complete Example: Recipe Collection Export

```csharp
using AMCode.Exports.Recipes;
using AMCode.Exports.Recipes.Models;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup logging
        var loggerFactory = LoggerFactory.Create(builder => 
            builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        var logger = loggerFactory.CreateLogger<RecipeExportBuilder>();

        // Create export builder
        var exportBuilder = new RecipeExportBuilder(logger);

        // Create sample recipes
        var recipes = CreateSampleRecipes();

        // Export all recipes to Excel
        var allRecipesOptions = new RecipeExportOptions
        {
            Format = "excel",
            Title = "Complete Recipe Collection",
            IncludeMetadata = true
        };

        var allRecipesResult = await exportBuilder.ExportRecipesAsync(recipes, allRecipesOptions);
        await SaveResult(allRecipesResult, "all_recipes.xlsx");

        // Export only dinner recipes with custom columns
        var dinnerRecipes = recipes.Where(r => r.Category == "Dinner").ToList();
        var dinnerColumnConfig = new RecipeColumnConfiguration
        {
            IncludeTitle = true,
            IncludeCategory = true,
            IncludeTiming = true,
            IncludeServings = true,
            IncludeIngredients = true,
            IncludeInstructions = false, // Summary view
            IncludeTags = true
        };

        var dinnerOptions = new RecipeExportOptions
        {
            Format = "excel",
            Title = "Dinner Recipes Summary"
        };

        var dinnerResult = await exportBuilder.ExportRecipesAsync(
            dinnerRecipes, dinnerOptions, dinnerColumnConfig);
        await SaveResult(dinnerResult, "dinner_recipes.xlsx");

        // Export shopping list
        var shoppingListOptions = new RecipeExportOptions
        {
            Format = "excel",
            Title = "Weekly Shopping List"
        };

        var shoppingListResult = await exportBuilder.ExportShoppingListAsync(recipes, shoppingListOptions);
        await SaveResult(shoppingListResult, "shopping_list.xlsx");
    }

    static List<Recipe> CreateSampleRecipes()
    {
        return new List<Recipe>
        {
            new Recipe
            {
                Title = "Spaghetti Carbonara",
                Category = "Dinner",
                PrepTimeMinutes = 15,
                CookTimeMinutes = 20,
                Servings = 4,
                Difficulty = "Medium",
                Cuisine = "Italian",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Spaghetti", Amount = "200", Unit = "g" },
                    new Ingredient { Name = "Eggs", Amount = "2", Unit = "large" },
                    new Ingredient { Name = "Pancetta", Amount = "100", Unit = "g" },
                    new Ingredient { Name = "Parmesan Cheese", Amount = "50", Unit = "g" }
                },
                Instructions = "Cook pasta according to package directions...",
                Tags = new List<string> { "dinner", "quick", "pasta" },
                CreatedAt = DateTime.Now.AddDays(-5)
            },
            new Recipe
            {
                Title = "Chocolate Chip Cookies",
                Category = "Dessert",
                PrepTimeMinutes = 20,
                CookTimeMinutes = 12,
                Servings = 24,
                Difficulty = "Easy",
                Cuisine = "American",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Flour", Amount = "2.5", Unit = "cups" },
                    new Ingredient { Name = "Butter", Amount = "1", Unit = "cup" },
                    new Ingredient { Name = "Chocolate Chips", Amount = "1", Unit = "cup" },
                    new Ingredient { Name = "Brown Sugar", Amount = "0.75", Unit = "cup" }
                },
                Instructions = "Preheat oven to 375°F. Mix dry ingredients...",
                Tags = new List<string> { "dessert", "baking", "cookies" },
                CreatedAt = DateTime.Now.AddDays(-2)
            }
        };
    }

    static async Task SaveResult(Result<Stream> result, string fileName)
    {
        if (result.IsSuccess)
        {
            using var fileStream = File.Create(fileName);
            await result.Value.CopyToAsync(fileStream);
            Console.WriteLine($"Successfully created {fileName}");
        }
        else
        {
            Console.WriteLine($"Failed to create {fileName}: {result.Error}");
        }
    }
}
```

## License

This library is part of the AMCode suite and is licensed under the terms specified in the main AMCode license agreement.

## Contributing

Contributions are welcome! Please see the main AMCode repository for contribution guidelines.

## Support

For support and questions, please refer to the main AMCode documentation or create an issue in the repository.
