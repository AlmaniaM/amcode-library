# PDF Library Development Guide

This guide provides detailed information for developing and maintaining the PDF functionality within the AMCode.Documents library.

## Overview

The PDF library provides a unified interface for creating and manipulating PDF documents using multiple providers. It follows clean architecture principles and supports both QuestPDF and iTextSharp providers.

## Architecture

### Clean Architecture Layers

```
Pdf/
├── Domain/                    # Core business logic
│   ├── Interfaces/           # PDF contracts and abstractions
│   │   ├── IPdfDocument.cs   # Main document interface
│   │   ├── IPage.cs          # Page interface
│   │   ├── IPdfProvider.cs   # Provider interface
│   │   ├── IPdfContent.cs    # Content interface
│   │   ├── IPdfMetadata.cs   # Metadata interface
│   │   ├── IPdfBuilder.cs    # Builder interface
│   │   └── IPdfValidator.cs  # Validation interface
│   ├── Models/               # PDF entities and value objects
│   │   ├── PdfDocument.cs    # Document implementation
│   │   ├── PdfPage.cs        # Page implementation
│   │   ├── PdfContent.cs     # Content implementation
│   │   ├── PdfMetadata.cs    # Metadata implementation
│   │   ├── PdfTable.cs       # Table implementation
│   │   ├── PdfPages.cs       # Pages collection
│   │   ├── PageSize.cs       # Page size enum
│   │   ├── Margins.cs        # Margins value object
│   │   ├── PageOrientation.cs # Orientation enum
│   │   ├── FontStyle.cs      # Font styling
│   │   └── PageDimensions.cs # Page dimensions
│   └── Validators/           # Business rules and validation
│       └── PdfValidator.cs   # Document validation logic
├── Infrastructure/           # External concerns and adapters
│   ├── Adapters/            # Data adapters
│   │   ├── PdfContentAdapter.cs    # Content adapter
│   │   ├── PdfMetadataAdapter.cs   # Metadata adapter
│   │   └── PdfLogger.cs            # Logging adapter
│   └── Factories/           # Object creation
│       ├── PdfProviderFactory.cs   # Provider factory
│       └── PdfEngineFactory.cs     # Engine factory
├── Providers/               # Provider implementations
│   ├── QuestPdf/           # QuestPDF provider
│   │   ├── QuestPdfProvider.cs     # Provider implementation
│   │   └── QuestPdfEngine.cs       # Engine implementation
│   └── iTextSharp/         # iTextSharp provider
│       ├── iTextSharpProvider.cs   # Provider implementation
│       └── iTextSharpEngine.cs     # Engine implementation
├── Examples/               # Usage examples
│   └── PdfUsageExamples.cs # Comprehensive examples
├── PdfBuilder.cs          # Fluent builder API
├── PdfFactory.cs          # Static factory for easy access
└── README.md              # PDF-specific documentation
```

## Core Concepts

### Document Lifecycle

1. **Creation**: Use `PdfFactory.CreateDocument()` or `PdfBuilder`
2. **Configuration**: Set properties, add pages, configure content
3. **Content Addition**: Add text, shapes, tables, images to pages
4. **Validation**: Validate document structure and content
5. **Rendering**: Save to file or stream (provider-specific)
6. **Cleanup**: Dispose resources properly

### Provider Pattern

The library uses the provider pattern to support multiple PDF libraries:

```csharp
// Configure provider
var logger = new PdfLogger();
var validator = new PdfValidator();
var provider = new QuestPdfProvider(logger, validator);
PdfFactory.SetDefaultProvider(provider);

// Use factory
var result = PdfFactory.CreateDocument();
```

### Result Pattern

All operations return `Result<T>` for consistent error handling:

```csharp
var result = PdfFactory.CreateDocument();
if (result.IsSuccess)
{
    var document = result.Value;
    // Use document
}
else
{
    Console.WriteLine($"Error: {result.Error}");
}
```

## Development Guidelines

### Adding New Features

1. **Define Interface**: Add method to appropriate interface in `Domain/Interfaces/`
2. **Update Models**: Modify models in `Domain/Models/` if needed
3. **Implement Providers**: Add implementation to both QuestPDF and iTextSharp providers
4. **Add Validation**: Update `PdfValidator` if needed
5. **Add Tests**: Create comprehensive tests
6. **Update Examples**: Add usage examples
7. **Update Documentation**: Update this guide and README

### Provider Implementation

When implementing a new provider:

1. **Implement `IPdfProvider`**: Core provider interface
2. **Implement `IPdfEngine`**: Engine-specific implementation
3. **Handle Errors**: Use `Result<T>` pattern consistently
4. **Add Logging**: Use `IPdfLogger` for operations
5. **Add Validation**: Use `IPdfValidator` for validation
6. **Add Tests**: Provider-specific test coverage

### Testing Strategy

#### Unit Tests
- Test individual methods and classes
- Mock dependencies where appropriate
- Test error conditions and edge cases

#### Integration Tests
- Test cross-component functionality
- Test provider switching
- Test end-to-end document creation

#### Performance Tests
- Memory usage with large documents
- Performance with many pages
- Provider comparison benchmarks

### Error Handling

Use the `Result<T>` pattern consistently:

```csharp
public Result<IPdfDocument> CreateDocument()
{
    try
    {
        // Implementation
        return Result<IPdfDocument>.Success(document);
    }
    catch (Exception ex)
    {
        _logger.LogError($"Failed to create document: {ex.Message}", ex);
        return Result<IPdfDocument>.Failure(ex.Message);
    }
}
```

### Logging

Use structured logging with `IPdfLogger`:

```csharp
_logger.LogInformation("Creating PDF document with {PageCount} pages", pageCount);
_logger.LogError("Failed to add text to page {PageNumber}: {Error}", pageNumber, error);
```

### Validation

Use `IPdfValidator` for business rule validation:

```csharp
var validationResult = _validator.ValidateDocument(document);
if (!validationResult.IsSuccess)
{
    return Result<IPdfDocument>.Failure(validationResult.Error);
}
```

## Performance Considerations

### Memory Management

- **Dispose Resources**: Implement `IDisposable` where needed
- **Stream Management**: Use `using` statements for streams
- **Large Documents**: Consider memory usage for large page counts
- **Provider Switching**: Minimize provider switching overhead

### Optimization Guidelines

- **Lazy Loading**: Load content only when needed
- **Batch Operations**: Group related operations
- **Caching**: Cache expensive operations where appropriate
- **Resource Pooling**: Reuse expensive objects

### Provider Selection

Choose the right provider for your use case:

#### QuestPDF
- **Best for**: Modern PDF generation, performance-critical applications
- **Strengths**: Clean API, good performance, modern design
- **Limitations**: Limited PDF reading capabilities

#### iTextSharp
- **Best for**: Complex PDF manipulation, reading existing PDFs
- **Strengths**: Comprehensive features, can read PDFs
- **Limitations**: More complex API, larger footprint

## Common Patterns

### Document Creation

```csharp
// Using factory
var result = PdfFactory.CreateDocument();
if (result.IsSuccess)
{
    var document = result.Value;
    // Configure document
}

// Using builder
var document = new PdfBuilder(provider)
    .WithTitle("My Document")
    .WithAuthor("AMCode")
    .WithPage(page => page.AddText("Hello World", 50, 50))
    .Build();
```

### Content Addition

```csharp
var page = document.Pages.Create();

// Add text
page.AddText("Hello World", 50, 50);

// Add styled text
var fontStyle = new FontStyle
{
    FontSize = 16,
    Bold = true,
    Color = Color.Blue
};
page.AddText("Styled Text", 50, 100, fontStyle);

// Add shapes
page.AddRectangle(100, 150, 200, 100, Color.LightBlue, Color.DarkGray);
page.AddLine(50, 300, 200, 300, Color.Black, 2);

// Add table
var table = page.AddTable(50, 350, 3, 2);
table.SetCellValue(0, 0, "Header 1");
table.SetCellValue(0, 1, "Header 2");
table.SetCellValue(1, 0, "Data 1");
table.SetCellValue(1, 1, "Data 2");
```

### Error Handling

```csharp
var result = PdfFactory.CreateDocument();
if (!result.IsSuccess)
{
    _logger.LogError("Failed to create document: {Error}", result.Error);
    return;
}

var document = result.Value;
try
{
    // Use document
    document.SaveAs("output.pdf");
}
catch (Exception ex)
{
    _logger.LogError("Failed to save document: {Error}", ex.Message);
}
finally
{
    document?.Dispose();
}
```

## Troubleshooting

### Common Issues

1. **Provider Not Configured**: Ensure `PdfFactory.SetDefaultProvider()` is called
2. **Memory Issues**: Check for proper disposal of resources
3. **Validation Failures**: Use `PdfValidator` to identify issues
4. **Provider Errors**: Check provider-specific error messages

### Debugging Tips

1. **Enable Logging**: Use `PdfLogger` to trace operations
2. **Validate Documents**: Use `PdfValidator` to check document structure
3. **Test Providers**: Test with both QuestPDF and iTextSharp
4. **Check Examples**: Refer to `PdfUsageExamples.cs` for patterns

## Resources

- **QuestPDF Documentation**: https://www.questpdf.com/
- **iTextSharp Documentation**: https://itextpdf.com/
- **Clean Architecture**: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
- **Result Pattern**: https://enterprisecraftsmanship.com/posts/functional-c-primitive-obsession/
