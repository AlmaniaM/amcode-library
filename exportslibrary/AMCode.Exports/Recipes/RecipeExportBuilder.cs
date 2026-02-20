using AMCode.Common.Models;
using AMCode.Exports.Book;
using AMCode.Exports.ExportBuilder;
using AMCode.Exports.Recipes.Models;
using AMCode.Exports.Recipes.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AMCode.Exports.Recipes
{
    /// <summary>
    /// Builder for creating recipe exports in various formats
    /// </summary>
    public class RecipeExportBuilder : IRecipeExportBuilder
    {
        private readonly SimpleExcelExportBuilder _excelBuilder;
        private readonly IRecipeExcelBuilder _recipeExcelBuilder;
        private readonly IRecipeCsvBuilder _recipeCsvBuilder;
        private readonly ILogger<RecipeExportBuilder> _logger;

        public RecipeExportBuilder(ILogger<RecipeExportBuilder> logger)
        {
            _excelBuilder = new SimpleExcelExportBuilder();
            _recipeExcelBuilder = new RecipeExcelBuilder();
            _recipeCsvBuilder = new RecipeCsvBuilder();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        /// <summary>
        /// Exports recipes to the specified format
        /// </summary>
        /// <param name="recipes">Collection of recipes to export</param>
        /// <param name="options">Export options</param>
        /// <returns>Result containing the export stream</returns>
        public async Task<Result<Stream>> ExportRecipesAsync(
            IEnumerable<Recipe> recipes, 
            RecipeExportOptions options)
        {
            try
            {
                _logger.LogInformation("Exporting {Count} recipes to {Format}", recipes.Count(), options.Format);
                
                return options.Format.ToLower() switch
                {
                    "excel" => await ExportToExcelAsync(recipes, options),
                    "csv" => await ExportToCsvAsync(recipes, options, RecipeColumnConfiguration.Default),
                    _ => Result<Stream>.Failure($"Unsupported format: {options.Format}. Supported formats: Excel, CSV.")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export recipes");
                return Result<Stream>.Failure($"Export failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Exports recipes with custom column configuration
        /// </summary>
        /// <param name="recipes">Collection of recipes to export</param>
        /// <param name="options">Export options</param>
        /// <param name="columnConfig">Custom column configuration</param>
        /// <returns>Result containing the export stream</returns>
        public async Task<Result<Stream>> ExportRecipesAsync(
            IEnumerable<Recipe> recipes,
            RecipeExportOptions options,
            RecipeColumnConfiguration columnConfig)
        {
            try
            {
                _logger.LogInformation("Exporting {Count} recipes with custom columns to {Format}",
                    recipes.Count(), options.Format);

                return options.Format.ToLower() switch
                {
                    "excel" => await ExportToExcelWithCustomColumnsAsync(recipes, options, columnConfig),
                    "csv" => await ExportToCsvAsync(recipes, options, columnConfig),
                    _ => Result<Stream>.Failure($"Unsupported format: {options.Format}. Supported formats: Excel, CSV.")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export recipes with custom columns");
                return Result<Stream>.Failure($"Export failed: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Exports recipe ingredients as a shopping list
        /// </summary>
        /// <param name="recipes">Collection of recipes</param>
        /// <param name="options">Export options</param>
        /// <returns>Result containing the shopping list export stream</returns>
        public async Task<Result<Stream>> ExportShoppingListAsync(
            IEnumerable<Recipe> recipes, 
            RecipeExportOptions options)
        {
            try
            {
                _logger.LogInformation("Exporting shopping list for {Count} recipes", recipes.Count());
                
                var shoppingList = ConsolidateIngredients(recipes);
                var shoppingListOptions = new RecipeExportOptions
                {
                    Format = options.Format,
                    Title = "Shopping List",
                    IncludeImages = false,
                    IncludeMetadata = false
                };
                
                return options.Format.ToLower() switch
                {
                    "excel" => await ExportShoppingListToExcelAsync(shoppingList, shoppingListOptions),
                    "csv" => await ExportShoppingListToCsvAsync(shoppingList, shoppingListOptions),
                    _ => Result<Stream>.Failure($"Unsupported format: {options.Format}. Supported formats: Excel, CSV.")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export shopping list");
                return Result<Stream>.Failure($"Shopping list export failed: {ex.Message}");
            }
        }
        
        // CSV export via RecipeCsvBuilder

        private Task<Result<Stream>> ExportToCsvAsync(
            IEnumerable<Recipe> recipes,
            RecipeExportOptions options,
            RecipeColumnConfiguration columnConfig)
        {
            try
            {
                var exportDataList = recipes.Select(RecipeExportData.FromRecipe).Where(r => r != null).ToList();
                _recipeCsvBuilder.AddRecipes(exportDataList).WithColumns(columnConfig);
                var stream = _recipeCsvBuilder.BuildToStream();
                return Task.FromResult(Result<Stream>.Success(stream));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export recipes to CSV");
                return Task.FromResult(Result<Stream>.Failure($"CSV export failed: {ex.Message}"));
            }
        }

        private Task<Result<Stream>> ExportShoppingListToCsvAsync(
            IEnumerable<ShoppingListItem> shoppingList,
            RecipeExportOptions options)
        {
            try
            {
                // Build shopping list CSV manually using RecipeCsvBuilder with ingredient-only data
                var exportDataList = shoppingList.Select(item => new RecipeExportData
                {
                    Title = options.Title ?? "Shopping List",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient { Name = item.Name, Amount = item.Amount, Unit = item.Unit, Notes = item.Notes }
                    }
                }).ToList();

                var csvBuilder = new RecipeCsvBuilder();
                csvBuilder.AddRecipes(exportDataList).WithExpandedIngredients(true);
                var stream = csvBuilder.BuildToStream();
                return Task.FromResult(Result<Stream>.Success(stream));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export shopping list to CSV");
                return Task.FromResult(Result<Stream>.Failure($"Shopping list CSV export failed: {ex.Message}"));
            }
        }

        // Excel export methods now enabled with SimpleExcelExportBuilder

        private async Task<Result<Stream>> ExportToExcelAsync(IEnumerable<Recipe> recipes, RecipeExportOptions options)
        {
            try
            {
                var columns = GetDefaultExcelColumns();
                var result = await _excelBuilder.CreateExportAsync(
                    options.Title ?? "Recipe Export", 
                    recipes.Count(), 
                    columns, 
                    CancellationToken.None);
                    
                return Result<Stream>.Success(await result.GetDataAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export recipes to Excel");
                return Result<Stream>.Failure($"Excel export failed: {ex.Message}");
            }
        }

        private async Task<Result<Stream>> ExportToExcelWithCustomColumnsAsync(
            IEnumerable<Recipe> recipes, 
            RecipeExportOptions options,
            RecipeColumnConfiguration columnConfig)
        {
            try
            {
                var columns = GetCustomExcelColumns(columnConfig);
                var result = await _excelBuilder.CreateExportAsync(
                    options.Title ?? "Recipe Export", 
                    recipes.Count(), 
                    columns, 
                    CancellationToken.None);
                    
                return Result<Stream>.Success(await result.GetDataAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export recipes to Excel with custom columns");
                return Result<Stream>.Failure($"Excel export failed: {ex.Message}");
            }
        }

        private async Task<Result<Stream>> ExportShoppingListToExcelAsync(
            IEnumerable<ShoppingListItem> shoppingList, 
            RecipeExportOptions options)
        {
            try
            {
                var columns = GetShoppingListExcelColumns();
                var result = await _excelBuilder.CreateExportAsync(
                    options.Title ?? "Shopping List", 
                    shoppingList.Count(), 
                    columns, 
                    CancellationToken.None);
                    
                return Result<Stream>.Success(await result.GetDataAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to export shopping list to Excel");
                return Result<Stream>.Failure($"Excel export failed: {ex.Message}");
            }
        }
        
        // CSV column configuration methods removed - focusing on Excel export only
        
        // Excel column configuration methods

        private List<IExcelDataColumn> GetDefaultExcelColumns()
        {
            return new List<IExcelDataColumn>
            {
                new ExcelDataColumn { DataFieldName = "Title", WorksheetHeaderName = "Recipe Title", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "Category", WorksheetHeaderName = "Category", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "PrepTimeMinutes", WorksheetHeaderName = "Prep Time (min)", DataType = typeof(int) },
                new ExcelDataColumn { DataFieldName = "CookTimeMinutes", WorksheetHeaderName = "Cook Time (min)", DataType = typeof(int) },
                new ExcelDataColumn { DataFieldName = "Servings", WorksheetHeaderName = "Servings", DataType = typeof(int) },
                new ExcelDataColumn { DataFieldName = "Difficulty", WorksheetHeaderName = "Difficulty", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "Cuisine", WorksheetHeaderName = "Cuisine", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "Ingredients", WorksheetHeaderName = "Ingredients", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "Instructions", WorksheetHeaderName = "Instructions", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "Tags", WorksheetHeaderName = "Tags", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "CreatedAt", WorksheetHeaderName = "Created Date", DataType = typeof(DateTime) }
            };
        }

        private List<IExcelDataColumn> GetCustomExcelColumns(RecipeColumnConfiguration columnConfig)
        {
            var columns = new List<IExcelDataColumn>();
            
            if (columnConfig.IncludeTitle)
                columns.Add(new ExcelDataColumn { DataFieldName = "Title", WorksheetHeaderName = "Recipe Title", DataType = typeof(string) });
            
            if (columnConfig.IncludeCategory)
                columns.Add(new ExcelDataColumn { DataFieldName = "Category", WorksheetHeaderName = "Category", DataType = typeof(string) });
            
            if (columnConfig.IncludeTiming)
            {
                columns.Add(new ExcelDataColumn { DataFieldName = "PrepTimeMinutes", WorksheetHeaderName = "Prep Time (min)", DataType = typeof(int) });
                columns.Add(new ExcelDataColumn { DataFieldName = "CookTimeMinutes", WorksheetHeaderName = "Cook Time (min)", DataType = typeof(int) });
            }
            
            if (columnConfig.IncludeServings)
                columns.Add(new ExcelDataColumn { DataFieldName = "Servings", WorksheetHeaderName = "Servings", DataType = typeof(int) });
            
            if (columnConfig.IncludeIngredients)
                columns.Add(new ExcelDataColumn { DataFieldName = "Ingredients", WorksheetHeaderName = "Ingredients", DataType = typeof(string) });
            
            if (columnConfig.IncludeInstructions)
                columns.Add(new ExcelDataColumn { DataFieldName = "Instructions", WorksheetHeaderName = "Instructions", DataType = typeof(string) });
            
            if (columnConfig.IncludeTags)
                columns.Add(new ExcelDataColumn { DataFieldName = "Tags", WorksheetHeaderName = "Tags", DataType = typeof(string) });
            
            if (columnConfig.IncludeMetadata)
                columns.Add(new ExcelDataColumn { DataFieldName = "CreatedAt", WorksheetHeaderName = "Created Date", DataType = typeof(DateTime) });
            
            return columns;
        }

        private List<IExcelDataColumn> GetShoppingListExcelColumns()
        {
            return new List<IExcelDataColumn>
            {
                new ExcelDataColumn { DataFieldName = "Name", WorksheetHeaderName = "Ingredient", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "Amount", WorksheetHeaderName = "Amount", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "Unit", WorksheetHeaderName = "Unit", DataType = typeof(string) },
                new ExcelDataColumn { DataFieldName = "IsOptional", WorksheetHeaderName = "Optional", DataType = typeof(bool) },
                new ExcelDataColumn { DataFieldName = "Notes", WorksheetHeaderName = "Notes", DataType = typeof(string) }
            };
        }

        private List<ShoppingListItem> ConsolidateIngredients(IEnumerable<Recipe> recipes)
        {
            var ingredientMap = new Dictionary<string, ShoppingListItem>();
            
            foreach (var recipe in recipes)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    var key = ingredient.Name.ToLowerInvariant();
                    if (ingredientMap.ContainsKey(key))
                    {
                        // Combine amounts if possible
                        var existing = ingredientMap[key];
                        if (!string.IsNullOrEmpty(ingredient.Amount) && !string.IsNullOrEmpty(existing.Amount))
                        {
                            // Simple consolidation - in a real implementation, you'd want more sophisticated unit conversion
                            existing.Amount = $"{existing.Amount} + {ingredient.Amount}";
                        }
                    }
                    else
                    {
                        ingredientMap[key] = new ShoppingListItem
                        {
                            Name = ingredient.Name,
                            Amount = ingredient.Amount,
                            Unit = ingredient.Unit,
                            IsOptional = ingredient.IsOptional,
                            Notes = ingredient.Notes
                        };
                    }
                }
            }
            
            return ingredientMap.Values.OrderBy(i => i.Name).ToList();
        }
    }
}
