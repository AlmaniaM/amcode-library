# AMCode.Exports — Agent Guide

## What This Is

Recipe and data export engine (.NET 9) supporting Excel and CSV formats with custom column configuration, shopping list generation, and styling. Builds on `AMCode.Documents` and `AMCode.Storage`.

## When to Use

- Export recipes to Excel/CSV
- Generate shopping lists from recipe ingredients
- Need custom column configuration for tabular exports

## When NOT to Use

- Low-level document generation → `AMCode.Documents`
- PDF/DOCX generation → `AMCode.Documents`
- File storage → `AMCode.Storage`

## Key Interfaces

| Interface | Purpose |
|-----------|---------|
| `IRecipeExportBuilder` | ExportRecipesAsync, ExportShoppingListAsync |
| `IBookBuilder` / `IBookBuilderFactory` | Document composition builders |
| `IExcelBookBuilderConfig` | Excel-specific styling configuration |

## Verification

```bash
cd repos/amcode-library
dotnet build exportslibrary/AMCode.Exports/AMCode.Exports.csproj
```
