# AMCode Exports Library - Development Guide

This guide provides comprehensive information for developers working with the AMCode Exports Library.

## Project Structure

```
AMCode.Exports/
├── Components/
│   ├── Book/                    # Core book/file creation classes
│   │   ├── Csv/                 # CSV-specific implementations
│   │   ├── Excel/               # Excel-specific implementations
│   │   ├── Models/              # Common interfaces and models
│   │   └── Exceptions/          # Custom exceptions
│   ├── BookBuilder/             # High-level book building logic
│   │   ├── Actions/             # Styling actions
│   │   ├── Csv/                 # CSV builder implementations
│   │   ├── Excel/               # Excel builder implementations
│   │   └── Models/              # Builder configuration models
│   ├── Common/                  # Shared utilities and models
│   ├── DataSources/             # Data source implementations
│   ├── ExportBuilder/           # Export builder implementations
│   ├── Extensions/              # Extension methods
│   ├── Results/                 # Export result implementations
│   └── Zip/                     # ZIP archive utilities
├── Scripts/                     # Build scripts (legacy)
└── AMCode.Exports.csproj        # Main project file

AMCode.Exports.UnitTests/         # Unit tests
AMCode.Exports.IntegrationTests/  # Integration tests
AMCode.Exports.SharedTestLibrary/ # Shared test utilities
```

## Development Setup

### Prerequisites

- .NET 5.0 SDK or later
- Visual Studio 2019+ or VS Code with C# extension
- Git

### Local Development

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd exportslibrary
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore AMCode.Exports.sln
   ```

3. **Build the solution**
   ```bash
   dotnet build AMCode.Exports.sln
   ```

4. **Run tests**
   ```bash
   dotnet test AMCode.Exports.sln
   ```

### Package Management

The project uses local package references for AMCode dependencies. Ensure you have the following packages available locally:

- AMCode.Common (2.1.0+)
- AMCode.Columns (1.2.1+)
- AMCode.Storage (1.1.2+)
- AMCode.Xlsx (1.1.0+)

## Architecture Overview

### Core Components

#### 1. Book Classes (`Components/Book/`)
- **Purpose**: Low-level file creation and manipulation
- **Key Classes**:
  - `CsvBook`: Creates CSV files
  - `ExcelBook`: Creates Excel files with styling support
  - `IBook<TColumn>`: Common interface for all book types

#### 2. BookBuilder Classes (`Components/BookBuilder/`)
- **Purpose**: High-level orchestration of book creation
- **Key Classes**:
  - `BookBuilderCommon<TColumn>`: Shared builder logic
  - `CsvBookBuilder`: CSV-specific builder
  - `ExcelBookBuilder`: Excel-specific builder with styling
  - `BookCompiler`: Compiles multiple books into results

#### 3. ExportBuilder Classes (`Components/ExportBuilder/`)
- **Purpose**: Top-level API for creating exports
- **Key Classes**:
  - `CsvExportBuilder`: Simple CSV export API
  - `ExcelExportBuilder`: Excel export with styling API

### Data Flow

```
Data Source → ExportBuilder → BookBuilder → Book → File/Stream
```

1. **Data Source**: Provides data through `ExportDataRangeFetch` delegate
2. **ExportBuilder**: Orchestrates the export process
3. **BookBuilder**: Handles data chunking and book creation
4. **Book**: Creates the actual file content
5. **Result**: Returns file as stream or saves to disk

## Key Interfaces

### IBook<TColumn>
Core interface for creating files:
```csharp
public interface IBook<out TColumn> : IDisposable
    where TColumn : IBookDataColumn
{
    void AddData(IList<ExpandoObject> dataList, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken = default);
    Stream Save();
    void SaveAs(Stream saveAsStream);
    void SetColumns(IEnumerable<string> columns);
}
```

### IExportBuilder<TColumn>
High-level export interface:
```csharp
public interface IExportBuilder<in TColumn> where TColumn : IBookDataColumn
{
    int CalculateNumberOfBooks(int totalRowCount);
    Task<IExportResult> CreateExportAsync(string fileName, int totalRowCount, IEnumerable<TColumn> columns, CancellationToken cancellationToken);
}
```

## Testing Strategy

### Unit Tests (`AMCode.Exports.UnitTests/`)
- Test individual components in isolation
- Mock external dependencies
- Cover edge cases and error conditions
- Focus on business logic validation

### Integration Tests (`AMCode.Exports.IntegrationTests/`)
- Test complete workflows
- Use real file I/O operations
- Test with actual data scenarios
- Verify end-to-end functionality

### Test Structure
```
Components/
├── Book/                    # Book class tests
├── BookBuilder/             # Builder tests
├── ExportBuilder/           # Export builder tests
├── Extensions/              # Extension method tests
└── Results/                 # Result class tests
```

## Code Style and Standards

### Naming Conventions
- **Classes**: PascalCase (e.g., `ExcelBook`, `CsvExportBuilder`)
- **Interfaces**: PascalCase with 'I' prefix (e.g., `IBook`, `IExportBuilder`)
- **Methods**: PascalCase (e.g., `AddData`, `CreateExportAsync`)
- **Properties**: PascalCase (e.g., `MaxRowsPerSheet`, `DataFieldName`)
- **Fields**: camelCase with underscore prefix for private fields (e.g., `_excelApplication`)

### Documentation Standards
- All public APIs must have XML documentation
- Include parameter descriptions and return value information
- Document exceptions that may be thrown
- Provide usage examples for complex APIs

### Error Handling
- Use specific exception types for different error conditions
- Provide meaningful error messages with context
- Include parameter validation with descriptive error messages
- Use `CancellationToken` for long-running operations

## Performance Considerations

### Memory Management
- Use `IDisposable` pattern for resources
- Implement proper cleanup in `Dispose()` methods
- Consider memory usage for large datasets

### Async Operations
- All I/O operations should be async
- Use `CancellationToken` for cancellation support
- Avoid blocking calls in async methods

### Large Dataset Handling
- Implement chunking for large datasets
- Use streaming where possible
- Consider memory vs. performance trade-offs

## Common Patterns

### Factory Pattern
Used for creating book instances:
```csharp
public interface IBookFactory<TColumn> where TColumn : IBookDataColumn
{
    IBook<TColumn> CreateBook();
}
```

### Builder Pattern
Used for complex object construction:
```csharp
var builder = new ExcelExportBuilder(config)
    .WithStyling(stylers)
    .WithMaxRows(10000);
```

### Strategy Pattern
Used for different styling approaches:
```csharp
public interface IExcelBookStyleAction
{
    void Style(IExcelBook book, IStyleActionData styleData);
}
```

## Debugging Tips

### Common Issues
1. **Memory Issues**: Check for proper disposal of resources
2. **Async Deadlocks**: Avoid `.Result` or `.Wait()` on async methods
3. **File Locking**: Ensure streams are properly closed
4. **Large Dataset Performance**: Monitor memory usage and consider chunking

### Debugging Tools
- Use Visual Studio Diagnostic Tools for memory profiling
- Enable detailed logging for troubleshooting
- Use unit tests to isolate issues

## Contributing

### Development Workflow
1. Create feature branch from main
2. Implement changes with tests
3. Run full test suite
4. Update documentation
5. Submit pull request

### Code Review Checklist
- [ ] All tests pass
- [ ] Code follows style guidelines
- [ ] Documentation is updated
- [ ] No breaking changes (or properly documented)
- [ ] Performance impact considered

## Migration Notes

This library was migrated from DemandLink.Exports to AMCode.Exports. Key changes:

- **Namespace**: `DemandLink.Exports` → `AMCode.Exports`
- **Package ID**: `DemandLink.Exports` → `AMCode.Exports`
- **Assembly Name**: `DemandLink.Exports.dll` → `AMCode.Exports.dll`
- **Dependencies**: Updated to use AMCode.* packages
- **Build Process**: Removed PowerShell dependency

### Breaking Changes
- All namespace references must be updated
- Package references must be updated
- Assembly references must be updated

## Troubleshooting

### Build Issues
- Ensure all AMCode dependencies are available
- Check .NET SDK version compatibility
- Verify project references are correct

### Runtime Issues
- Check for proper resource disposal
- Verify data source implementations
- Monitor memory usage for large datasets

### Test Issues
- Ensure test data is properly set up
- Check for file system permissions
- Verify mock implementations are correct
