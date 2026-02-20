using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AMCode.Exports.Recipes.Interfaces;
using AMCode.Exports.Recipes.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AMCode.Exports.Recipes
{
    /// <summary>
    /// Multi-sheet Excel workbook builder for recipe exports.
    /// Produces a workbook with four sheets:
    ///   Sheet 1 "Summary"      — one row per recipe (name, category, times, servings, tags)
    ///   Sheet 2 "Ingredients"  — one row per ingredient per recipe
    ///   Sheet 3 "Instructions" — one row per step per recipe
    ///   Sheet 4 "Nutrition"    — one row per recipe (optional, written only when IncludeNutrition = true)
    /// </summary>
    public class RecipeExcelBuilder : IRecipeExcelBuilder
    {
        private readonly List<RecipeExportData> _recipes = new List<RecipeExportData>();
        private RecipeColumnConfiguration _columns = RecipeColumnConfiguration.Default;
        private ExcelFormattingOptions _formatting = ExcelFormattingOptions.Default;
        private bool _includeNutritionSheet = false;

        /// <inheritdoc/>
        public IRecipeExcelBuilder AddRecipe(RecipeExportData recipe)
        {
            if (recipe != null)
            {
                _recipes.Add(recipe);
            }

            return this;
        }

        /// <inheritdoc/>
        public IRecipeExcelBuilder AddRecipes(IEnumerable<RecipeExportData> recipes)
        {
            if (recipes != null)
            {
                _recipes.AddRange(recipes.Where(r => r != null));
            }

            return this;
        }

        /// <inheritdoc/>
        public IRecipeExcelBuilder WithColumns(RecipeColumnConfiguration columns)
        {
            _columns = columns ?? RecipeColumnConfiguration.Default;
            return this;
        }

        /// <inheritdoc/>
        public IRecipeExcelBuilder WithFormatting(ExcelFormattingOptions options)
        {
            _formatting = options ?? ExcelFormattingOptions.Default;
            return this;
        }

        /// <inheritdoc/>
        public IRecipeExcelBuilder IncludeNutritionSheet(bool include = true)
        {
            _includeNutritionSheet = include;
            return this;
        }

        /// <inheritdoc/>
        public byte[] Build()
        {
            using var stream = BuildToStream();
            return ((MemoryStream)stream).ToArray();
        }

        /// <inheritdoc/>
        public Stream BuildToStream()
        {
            var stream = new MemoryStream();
            BuildWorkbook(stream);
            stream.Position = 0;
            return stream;
        }

        private void BuildWorkbook(Stream stream)
        {
            using var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());

            uint sheetId = 1;

            // Sheet 1: Summary
            AddSummarySheet(workbookPart, sheets, ref sheetId);

            // Sheet 2: Ingredients
            if (_columns.IncludeIngredients)
            {
                AddIngredientsSheet(workbookPart, sheets, ref sheetId);
            }

            // Sheet 3: Instructions
            if (_columns.IncludeInstructions)
            {
                AddInstructionsSheet(workbookPart, sheets, ref sheetId);
            }

            // Sheet 4: Nutrition (optional)
            if (_includeNutritionSheet || _columns.IncludeNutrition)
            {
                var recipesWithNutrition = _recipes.Where(r => r.Nutrition != null).ToList();
                if (recipesWithNutrition.Any())
                {
                    AddNutritionSheet(workbookPart, sheets, ref sheetId, recipesWithNutrition);
                }
            }

            workbookPart.Workbook.Save();
        }

        private void AddSummarySheet(WorkbookPart workbookPart, Sheets sheets, ref uint sheetId)
        {
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            var headers = BuildSummaryHeaders();
            AppendHeaderRow(sheetData, headers);

            foreach (var recipe in _recipes)
            {
                var row = new Row();
                if (_columns.IncludeTitle) AppendCell(row, recipe.Title);
                if (_columns.IncludeAuthor) AppendCell(row, recipe.Author);
                if (_columns.IncludeCategory) AppendCell(row, recipe.Category);
                if (_columns.IncludePrepTime) AppendCell(row, recipe.PrepTimeMinutes.ToString());
                if (_columns.IncludeCookTime) AppendCell(row, recipe.CookTimeMinutes.ToString());
                if (_columns.IncludeServings) AppendCell(row, recipe.Servings.ToString());
                if (_columns.IncludeDietaryTags) AppendCell(row, string.Join("; ", recipe.DietaryTags));
                if (_columns.IncludeSource) AppendCell(row, recipe.Source);
                if (_columns.IncludeCreatedDate) AppendCell(row, recipe.CreatedAt.ToString("yyyy-MM-dd"));
                sheetData.Append(row);
            }

            AppendSheet(sheets, worksheetPart, workbookPart, "Summary", sheetId++);
        }

        private void AddIngredientsSheet(WorkbookPart workbookPart, Sheets sheets, ref uint sheetId)
        {
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            AppendHeaderRow(sheetData, new[] { "Recipe Name", "Ingredient", "Quantity", "Unit", "Notes" });

            foreach (var recipe in _recipes)
            {
                if (recipe.Ingredients == null || !recipe.Ingredients.Any())
                {
                    var emptyRow = new Row();
                    AppendCell(emptyRow, recipe.Title);
                    AppendCell(emptyRow, string.Empty);
                    AppendCell(emptyRow, string.Empty);
                    AppendCell(emptyRow, string.Empty);
                    AppendCell(emptyRow, string.Empty);
                    sheetData.Append(emptyRow);
                    continue;
                }

                foreach (var ingredient in recipe.Ingredients)
                {
                    var row = new Row();
                    AppendCell(row, recipe.Title);
                    AppendCell(row, ingredient.Name);
                    AppendCell(row, ingredient.Amount);
                    AppendCell(row, ingredient.Unit);
                    AppendCell(row, ingredient.Notes);
                    sheetData.Append(row);
                }
            }

            AppendSheet(sheets, worksheetPart, workbookPart, "Ingredients", sheetId++);
        }

        private void AddInstructionsSheet(WorkbookPart workbookPart, Sheets sheets, ref uint sheetId)
        {
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            AppendHeaderRow(sheetData, new[] { "Recipe Name", "Step #", "Instruction" });

            foreach (var recipe in _recipes)
            {
                if (recipe.InstructionSteps == null || !recipe.InstructionSteps.Any())
                {
                    var emptyRow = new Row();
                    AppendCell(emptyRow, recipe.Title);
                    AppendCell(emptyRow, string.Empty);
                    AppendCell(emptyRow, string.Empty);
                    sheetData.Append(emptyRow);
                    continue;
                }

                for (int i = 0; i < recipe.InstructionSteps.Count; i++)
                {
                    var row = new Row();
                    AppendCell(row, recipe.Title);
                    AppendCell(row, (i + 1).ToString());
                    AppendCell(row, recipe.InstructionSteps[i]);
                    sheetData.Append(row);
                }
            }

            AppendSheet(sheets, worksheetPart, workbookPart, "Instructions", sheetId++);
        }

        private void AddNutritionSheet(WorkbookPart workbookPart, Sheets sheets, ref uint sheetId, List<RecipeExportData> recipesWithNutrition)
        {
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            AppendHeaderRow(sheetData, new[] { "Recipe Name", "Calories", "Fat (g)", "Carbs (g)", "Protein (g)", "Fiber (g)" });

            foreach (var recipe in recipesWithNutrition)
            {
                var n = recipe.Nutrition;
                var row = new Row();
                AppendCell(row, recipe.Title);
                AppendCell(row, n.CaloriesPerServing?.ToString("F1") ?? string.Empty);
                AppendCell(row, n.FatGrams?.ToString("F1") ?? string.Empty);
                AppendCell(row, n.CarbGrams?.ToString("F1") ?? string.Empty);
                AppendCell(row, n.ProteinGrams?.ToString("F1") ?? string.Empty);
                AppendCell(row, n.FiberGrams?.ToString("F1") ?? string.Empty);
                sheetData.Append(row);
            }

            AppendSheet(sheets, worksheetPart, workbookPart, "Nutrition", sheetId++);
        }

        private IEnumerable<string> BuildSummaryHeaders()
        {
            var headers = new List<string>();
            if (_columns.IncludeTitle) headers.Add("Recipe Name");
            if (_columns.IncludeAuthor) headers.Add("Author");
            if (_columns.IncludeCategory) headers.Add("Category");
            if (_columns.IncludePrepTime) headers.Add("Prep Time (min)");
            if (_columns.IncludeCookTime) headers.Add("Cook Time (min)");
            if (_columns.IncludeServings) headers.Add("Servings");
            if (_columns.IncludeDietaryTags) headers.Add("Dietary Tags");
            if (_columns.IncludeSource) headers.Add("Source");
            if (_columns.IncludeCreatedDate) headers.Add("Created Date");
            return headers;
        }

        private static void AppendHeaderRow(SheetData sheetData, IEnumerable<string> headers)
        {
            var headerRow = new Row();
            foreach (var header in headers)
            {
                AppendCell(headerRow, header);
            }

            sheetData.Append(headerRow);
        }

        private static void AppendCell(Row row, string value)
        {
            row.Append(new Cell
            {
                DataType = CellValues.InlineString,
                InlineString = new InlineString(new Text(value ?? string.Empty))
            });
        }

        private static void AppendSheet(Sheets sheets, WorksheetPart worksheetPart, WorkbookPart workbookPart, string sheetName, uint sheetId)
        {
            var relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            sheets.Append(new Sheet
            {
                Id = relationshipId,
                SheetId = sheetId,
                Name = sheetName
            });
        }
    }
}
