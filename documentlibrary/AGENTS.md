# AMCode.Documents — Agent Guide

## What This Is

Document generation library (.NET 8) for PDF, DOCX, and XLSX formats. Uses QuestPDF for PDFs, OpenXml for Word documents, and Syncfusion for spreadsheets. Provides fluent builder patterns.

## When to Use

- Generate recipe PDFs for printing/sharing
- Create Word documents for recipe exports
- Build spreadsheets for data export
- Need fluent builder API for document composition

## When NOT to Use

- High-level recipe export orchestration → `AMCode.Exports`
- File storage (saving generated docs) → `AMCode.Storage`
- Data formatting/aggregation → do in your app layer

## Key Interfaces

| Interface | Purpose |
|-----------|---------|
| `IPdfBuilder` | Fluent PDF builder (WithTitle, WithPage, Build) |
| `IDocumentBuilder` | DOCX builder for Word documents |
| `IWorkbookBuilder` / `IWorksheetBuilder` | XLSX spreadsheet builders |

## Dependencies

- QuestPDF 2025.1.0 (PDF)
- DocumentFormat.OpenXml 3.0.2 (DOCX)
- Syncfusion.XlsIO.Net.Core (XLSX)
- iTextSharp 5.5.13.3 (legacy PDF)

## Verification

```bash
cd repos/amcode-library
dotnet build documentlibrary/AMCode.Documents/AMCode.Documents.csproj
```
