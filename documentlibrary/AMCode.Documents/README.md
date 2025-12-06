# AMCode.Documents

**Version:** 1.1.0
**Target Framework:** .NET 8.0
**Last Updated:** 2025-01-27
**Purpose:** Comprehensive document generation library supporting PDF, DOCX, and Excel with recipe-specific capabilities

---

## Overview

AMCode.Documents is a comprehensive document generation library that provides unified APIs for creating PDF, DOCX (Word), and Excel documents. The library follows Clean Architecture principles with clear separation between domain logic and infrastructure implementations. It supports multiple providers for each document type, allowing flexibility in choosing the underlying technology. The library includes specialized recipe document generation capabilities and comprehensive logging for all document operations.

## Architecture

The library implements Clean Architecture with clear separation between:

- **Domain Layer**: Core interfaces, models, and business logic
- **Infrastructure Layer**: Adapters, factories, and provider implementations
- **Provider Layer**: Specific implementations (QuestPDF, iTextSharp, OpenXML, Syncfusion)

The architecture supports multiple providers for each document type, allowing runtime selection and easy provider switching. All operations use Result-based error handling and comprehensive logging.

### Key Components

- **Document Factories**: Static factories for easy document creation (PdfFactory, DocumentFactory)
- **Fluent Builders**: Builder pattern for complex document construction (IPdfBuilder, IDocumentBuilder)
- **Provider System**: Multiple providers per document type with provider registration
- **Logging System**: Comprehensive logging for all document operations
- **Recipe Support**: Domain-specific document generation for recipes

## Features

- **PDF Generation**: Support for QuestPDF and iTextSharp providers
- **DOCX Generation**: OpenXML-based Word document creation
- **Excel Generation**: OpenXML and Syncfusion Excel support
- **Fluent API**: Builder pattern for intuitive document construction
- **Multiple Providers**: Runtime provider selection and registration
- **Comprehensive Logging**: Structured logging for all operations
- **Recipe Documents**: Specialized recipe document generation
- **Performance Optimization**: Memory-efficient document creation
- **Result-Based Error Handling**: Consistent error handling across all operations
- **Dependency Injection Ready**: Full DI support with extension methods

## Dependencies

### Internal Dependencies

- **AMCode.Common** - Common utilities and Result types

### External Dependencies

- **QuestPDF** (2025.1.0) - Modern PDF generation library
- **iTextSharp** (5.5.13.3) - PDF generation library
- **DocumentFormat.OpenXml** (3.0.2) - OpenXML document manipulation
- **Syncfusion.XlsIO.Net.Core** (19.4.0.54) - Excel generation
- **Microsoft.Extensions.DependencyInjection** (8.0.0) - Dependency injection
- **Microsoft.Extensions.Configuration** (8.0.0) - Configuration support
- **Microsoft.Extensions.Logging** (8.0.0) - Logging abstractions
- **NUnit** (3.13.2) - Testing framework
- **Moq** (4.20.69) - Mocking framework

## Project Structure

```
AMCode.Documents/
├── Common/                        # Shared components
│   ├── README.md                 # [Common Components Documentation](Common/README.md)
│   ├── Drawing/                  # Drawing utilities
│   │   └── Color.cs             # Color model
│   ├── Enums/                    # Shared enumerations
│   │   ├── BorderLineStyle.cs
│   │   ├── BorderSides.cs
│   │   └── HorizontalAlignment.cs
│   ├── Logging/                  # Logging infrastructure
│   │   ├── IDocumentLogger.cs   # Main logging interface
│   │   ├── IDocumentLoggerFactory.cs
│   │   ├── IDocumentLoggerProvider.cs
│   │   └── Infrastructure/      # Logger implementations
│   └── Models/                   # Shared models
│       ├── BorderStyle.cs
│       ├── FontStyle.cs
│       ├── Margins.cs
│       ├── PageSize.cs
│       └── Result.cs
├── Pdf/                          # PDF generation
│   ├── README.md                 # [PDF Documentation](Pdf/README.md)
│   ├── PdfFactory.cs            # Static PDF factory
│   ├── PdfBuilder.cs            # Fluent PDF builder
│   ├── Domain/                   # PDF domain layer
│   │   ├── Interfaces/          # Domain interfaces
│   │   ├── Models/              # Domain models
│   │   └── Validators/          # PDF validators
│   ├── Infrastructure/           # PDF infrastructure
│   │   ├── Adapters/            # Provider adapters
│   │   ├── Factories/           # PDF factories
│   │   └── Performance/          # Performance optimizations
│   └── Providers/                # PDF providers
│       ├── QuestPdf/           # QuestPDF provider
│       └── iTextSharp/         # iTextSharp provider
├── Docx/                         # DOCX generation
│   ├── README.md                 # [DOCX Documentation](Docx/README.md)
│   ├── DocumentFactory.cs       # Static DOCX factory
│   ├── Domain/                   # DOCX domain layer
│   │   ├── Interfaces/          # Domain interfaces
│   │   ├── Models/              # Domain models
│   │   └── Validators/          # Document validators
│   ├── Infrastructure/           # DOCX infrastructure
│   │   ├── Adapters/            # Provider adapters
│   │   └── Factories/           # Document factories
│   └── Providers/                # DOCX providers
│       └── OpenXml/             # OpenXML provider
├── Xlsx/                         # Excel generation
│   ├── README.md                 # [Excel Documentation](Xlsx/README.md)
│   ├── Domain/                   # Excel domain layer
│   │   ├── Interfaces/          # Domain interfaces
│   │   ├── Models/              # Domain models
│   │   └── Validators/          # Excel validators
│   ├── Infrastructure/           # Excel infrastructure
│   │   ├── Adapters/            # Provider adapters
│   │   └── Factories/           # Workbook factories
│   └── Providers/                # Excel providers
│       └── OpenXml/             # OpenXML provider
├── Recipes/                      # Recipe document generation
│   ├── README.md                 # [Recipe Documentation](Recipes/README.md)
│   ├── Interfaces/              # Recipe interfaces
│   ├── Models/                  # Recipe models
│   └── Services/                # Recipe services
├── SyncfusionIo/                 # Syncfusion components
│   └── Excel/                    # Syncfusion Excel
│       └── Components/          # Excel components
└── AMCode.Documents.csproj      # Project file
```

## Key Interfaces

### IDocumentLogger

**Location:** `Common/Logging/IDocumentLogger.cs`

**Purpose:** Unified logging interface for all document operations across PDF, DOCX, and Excel.

**Key Methods:**

- `LogDocumentOperation(string, DocumentType, object)` - Logs document operations
- `LogDocumentCreation(string, TimeSpan, object)` - Logs document creation with metrics
- `LogContentOperation(string, string, object)` - Logs content operations
- `LogFormattingOperation(string, object)` - Logs formatting operations
- `LogFileOperation(string, string, long)` - Logs file operations
- `LogProviderOperation(string, string, object)` - Logs provider-specific operations
- `LogDocumentPerformance(string, TimeSpan, object)` - Logs performance metrics
- `LogDocumentError(string, Exception, DocumentType, object)` - Logs errors
- `WithDocumentContext(string, DocumentType)` - Creates scoped logger
- `WithOperationContext(string, string)` - Creates operation-scoped logger

**See Also:** [Common README](Common/README.md)

### IPdfBuilder

**Location:** `Pdf/Domain/Interfaces/IPdfBuilder.cs`

**Purpose:** Fluent builder interface for constructing PDF documents with method chaining.

**Key Methods:**

- `WithTitle(string)` - Sets document title
- `WithAuthor(string)` - Sets document author
- `WithSubject(string)` - Sets document subject
- `WithKeywords(string)` - Sets document keywords
- `WithPage(Action<IPage>)` - Adds page with default configuration
- `WithPage(PageSize, Action<IPage>)` - Adds page with specific size
- `Build()` - Builds the PDF document

**See Also:** [PDF README](Pdf/README.md)

### IDocumentBuilder

**Location:** `Docx/Domain/Interfaces/IDocumentBuilder.cs`

**Purpose:** Fluent builder interface for constructing DOCX documents with method chaining.

**Key Methods:**

- `WithTitle(string)` - Sets document title
- `WithAuthor(string)` - Sets document author
- `WithSubject(string)` - Sets document subject
- `WithKeywords(string)` - Sets document keywords
- `WithComments(string)` - Sets document comments
- `WithParagraph(Action<IParagraphBuilder>)` - Adds paragraph
- `WithTable(int, int, Action<ITable>)` - Adds table
- `Build()` - Builds the document

**See Also:** [DOCX README](Docx/README.md)

## Key Classes

### PdfFactory

**Location:** `Pdf/PdfFactory.cs`

**Purpose:** Static factory for creating PDF documents with provider management.

**Key Responsibilities:**

- Manage default and named PDF providers
- Create PDF documents with or without properties
- Support custom loggers and validators
- Provide provider registration and selection

**Key Methods:**

- `SetDefaultProvider(IPdfProvider)` - Sets default provider
- `RegisterProvider(string, IPdfProvider)` - Registers named provider
- `CreateDocument(IPdfLogger)` - Creates document with logger
- `CreateDocument(IPdfProperties, IPdfLogger)` - Creates document with properties
- `CreateDocument(string, IPdfLogger)` - Creates document using named provider

**See Also:** [PDF README](Pdf/README.md)

### DocumentFactory

**Location:** `Docx/DocumentFactory.cs`

**Purpose:** Static factory for creating and opening DOCX documents.

**Key Responsibilities:**

- Create new Word documents
- Open existing documents from streams or file paths
- Create documents with tables and formatted content
- Validate file paths and document formats

**Key Methods:**

- `CreateDocument()` - Creates new document
- `CreateDocument(string, string)` - Creates document with title and content
- `OpenDocument(Stream)` - Opens document from stream
- `OpenDocument(string)` - Opens document from file path
- `CreateDocumentWithTable(string, int, int)` - Creates document with table
- `CreateFormattedDocument(string, string, FontStyle)` - Creates formatted document

**See Also:** [DOCX README](Docx/README.md)

## Usage Examples

### PDF Generation - Basic Usage

```csharp
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Logging;

// Setup provider
var questPdfProvider = new QuestPdfEngine();
PdfFactory.SetDefaultProvider(questPdfProvider);

// Create PDF with factory
var logger = new PdfDocumentLogger();
var result = PdfFactory.CreateDocument(logger);

if (result.IsSuccess)
{
    var pdf = result.Value;
    // Add content to PDF
    pdf.AddPage(page => {
        page.AddText("Hello, World!");
    });

    // Save PDF
    await pdf.SaveAsync("output.pdf");
}
```

### PDF Generation - Fluent Builder

```csharp
using AMCode.Documents.Pdf;
using AMCode.Documents.Common.Models;

// Setup provider
PdfFactory.SetDefaultProvider(new QuestPdfEngine());

// Build PDF with fluent API
var builder = new PdfBuilder();
var result = builder
    .WithTitle("My Document")
    .WithAuthor("John Doe")
    .WithPage(page => {
        page.AddText("Document Content", new FontStyle { Size = 12 });
        page.AddTable(3, 2);
    })
    .Build();

if (result.IsSuccess)
{
    await result.Value.SaveAsync("document.pdf");
}
```

### DOCX Generation - Basic Usage

```csharp
using AMCode.Docx;

// Create new document
var document = DocumentFactory.CreateDocument("My Document", "Hello, World!");

// Add paragraph
var paragraph = document.Paragraphs.Create();
paragraph.Text = "This is a paragraph.";

// Add table
var table = document.Tables.Create(3, 2);
table.SetCellValue(0, 0, "Header 1");
table.SetCellValue(0, 1, "Header 2");

// Save document
document.Save("output.docx");
```

### DOCX Generation - Fluent Builder

```csharp
using AMCode.Docx;
using AMCode.Documents.Common.Models;

// Build document with fluent API
var builder = new DocumentBuilder();
var result = builder
    .WithTitle("Recipe Document")
    .WithAuthor("Chef")
    .WithParagraph(p => {
        p.AddText("Recipe Title", new FontStyle { Bold = true, Size = 16 });
    })
    .WithTable(5, 3, table => {
        // Configure table
    })
    .Build();

if (result.IsSuccess)
{
    var document = result.Value;
    // Use document
}
```

### Excel Generation

```csharp
using AMCode.Documents.Xlsx;

// Create workbook (example - Xlsx may be disabled)
// var workbook = ExcelFactory.CreateWorkbook();
// var worksheet = workbook.Worksheets.Create("Sheet1");
// worksheet.SetCellValue(0, 0, "Data");
// workbook.Save("output.xlsx");
```

### Recipe Document Generation

```csharp
using AMCode.Documents.Recipes;

// Create recipe document service
var recipeService = new RecipeDocumentService();

// Generate recipe PDF
var recipe = new RecipeModel { /* recipe data */ };
var pdfResult = await recipeService.GenerateRecipePdfAsync(recipe);

if (pdfResult.IsSuccess)
{
    await pdfResult.Value.SaveAsync("recipe.pdf");
}
```

## Configuration

### appsettings.json Example

```json
{
  "DocumentGeneration": {
    "Pdf": {
      "DefaultProvider": "QuestPdf",
      "Providers": {
        "QuestPdf": {
          "Type": "QuestPdf"
        },
        "iTextSharp": {
          "Type": "iTextSharp"
        }
      }
    },
    "Docx": {
      "Provider": "OpenXml"
    },
    "Logging": {
      "Level": "Information",
      "EnablePerformanceLogging": true,
      "EnableFileLogging": true
    }
  }
}
```

### Dependency Injection Setup

```csharp
using AMCode.Documents.Common.Logging;
using AMCode.Documents.Common.Logging.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

// Register document logging
services.AddDocumentLogging(options =>
{
    options.Level = DocumentLogLevel.Information;
    options.EnablePerformanceLogging = true;
    options.EnableFileLogging = true;
});

// Register PDF providers
services.AddScoped<IPdfProvider, QuestPdfEngine>();
// Or
services.AddScoped<IPdfProvider, iTextSharpEngine>();

// Register document factories
services.AddScoped<IDocumentFactory, DocumentFactory>();
```

## Testing

### Test Projects

- **AMCode.Documents.UnitTests**: Unit tests for document generation
  - Tests for PDF, DOCX, Excel generation
  - Provider-specific tests
  - Builder pattern tests
  - Mock-based testing

### Running Tests

```bash
dotnet test documentlibrary/AMCode.Documents.UnitTests
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Common](Common/README.md) - Shared components, models, enums, and logging infrastructure
- [PDF](Pdf/README.md) - PDF generation with QuestPDF and iTextSharp providers
- [DOCX](Docx/README.md) - DOCX generation with OpenXML provider
- [Excel](Xlsx/README.md) - Excel generation with OpenXML and Syncfusion providers
- [Recipes](Recipes/README.md) - Recipe-specific document generation

## Related Libraries

- [AMCode.Common](../commonlibrary/AMCode.Common/README.md) - Common utilities and Result types used by this library
- [AMCode.Exports](../../exportslibrary/AMCode.Exports/README.md) - Export functionality that may use document generation

## Migration Notes

### Version 1.1.0

- Added recipe-specific document generation features
- Enhanced logging infrastructure
- Performance optimizations for large documents
- Improved provider management

### Upgrading from Previous Versions

- Provider registration API has been enhanced
- Logging interface has been expanded
- Some internal APIs have changed (check migration guide if upgrading)

## Known Issues

- Excel (Xlsx) generation may be disabled in some configurations
- Some providers may have platform-specific limitations

## Future Considerations

- Additional PDF providers
- Enhanced Excel generation support
- Document template system
- Document merging capabilities
- Advanced formatting options
- Document conversion between formats

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy
- [PDF Development Guide](PDF_DEVELOPMENT.md) - PDF-specific development guide

**Last Updated:** 2025-01-27
**Maintained By:** Development Team
