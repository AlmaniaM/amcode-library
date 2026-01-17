# AMCode.Documents Development Guide

This guide provides detailed instructions for developing and maintaining the AMCode.Documents library, which supports both Excel and PDF document creation.

## Development Environment Setup

### Prerequisites

- **.NET 8.0 SDK**: Required for building and testing
- **Visual Studio Code** or **Visual Studio**: Recommended IDEs
- **Git**: For version control

### Local Development Configuration

1. **Clone the repository**
2. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

4. **Run tests**:
   ```bash
   dotnet test
   ```

## Project Structure

### Main Library (`AMCode.Documents`)

The main library is organized into logical components:

#### Excel Support (`SyncfusionIo/Excel/` and `Xlsx/`)
- **Application**: Core Excel application management
- **Drawing**: Color management and drawing utilities
- **Extensions**: Extension methods for enhanced functionality
- **Licensing**: License management for Syncfusion components
- **Range**: Cell range operations and management
- **Style**: Styling, formatting, and visual appearance
- **Util**: Utility classes and helper methods
- **Workbook**: Workbook-level operations
- **Worksheet**: Worksheet-level operations

#### PDF Support (`Pdf/`)
- **Domain**: Core PDF interfaces, models, and validators
- **Infrastructure**: Adapters, factories, and logging
- **Providers**: QuestPDF and iTextSharp implementations
- **Examples**: Usage examples and demonstrations

#### Common Components (`Common/`)
- **Drawing**: Shared color and drawing utilities
- **Models**: Common data structures and result patterns
- **Enums**: Shared enumerations

### Test Project (`AMCode.Documents.UnitTests`)

Comprehensive test coverage including:

#### Excel Tests
- Unit tests for all public APIs
- Integration tests for complex scenarios
- Edge case testing
- Cross-platform compatibility tests

#### PDF Tests
- Unit tests for document creation and management
- Provider-specific tests (QuestPDF & iTextSharp)
- Integration tests for cross-component functionality
- Performance tests for memory usage and speed
- Fluent API builder pattern tests

## Building and Packaging

### Local Package Creation

Create a local NuGet package for testing:

```bash
dotnet pack AMCode.Documents/AMCode.Documents.csproj -c Release -o local-packages
```

### Package Dependencies

The library depends on:
- **AMCode.Common** (1.0.0): Core AMCode functionality
- **Syncfusion.XlsIO.Net.Core** (19.4.0.54): Excel file format support
- **QuestPDF** (2024.12.4): Modern PDF generation
- **iTextSharp** (5.5.13.3): Comprehensive PDF manipulation

## Testing Strategy

### Test Categories

1. **Unit Tests**: Test individual methods and classes
2. **Integration Tests**: Test component interactions
3. **Cross-Platform Tests**: Ensure compatibility across operating systems

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "ClassName=TestClassName"

# Run PDF-specific tests
dotnet test --filter "FullyQualifiedName~Pdf"

# Run Excel-specific tests
dotnet test --filter "FullyQualifiedName~Xlsx"
```

### Test Requirements

- **100% test coverage** for all public APIs
- **Cross-platform compatibility** verification
- **Performance benchmarks** for critical operations
- **Memory leak detection** for long-running operations

## PDF Development Guidelines

### Provider Selection

The PDF library supports two providers with different strengths:

#### QuestPDF Provider
- **Best for**: Modern, clean PDF generation
- **Strengths**: Fluent API, good performance, modern design
- **Use when**: Creating new PDFs from scratch, performance is critical

#### iTextSharp Provider
- **Best for**: Complex PDF manipulation and reading
- **Strengths**: Comprehensive PDF features, can read existing PDFs
- **Use when**: Need advanced PDF features or reading existing documents

### PDF Architecture

The PDF library follows clean architecture principles:

```
Pdf/
├── Domain/           # Core business logic
│   ├── Interfaces/   # PDF contracts
│   ├── Models/       # PDF entities
│   └── Validators/   # Validation logic
├── Infrastructure/   # External concerns
│   ├── Adapters/     # Data adapters
│   └── Factories/    # Object creation
└── Providers/        # Provider implementations
    ├── QuestPdf/     # QuestPDF implementation
    └── iTextSharp/   # iTextSharp implementation
```

### Adding New PDF Features

1. **Define Interface**: Add method to appropriate interface in `Domain/Interfaces/`
2. **Update Models**: Modify models in `Domain/Models/` if needed
3. **Implement Providers**: Add implementation to both QuestPDF and iTextSharp providers
4. **Add Tests**: Create comprehensive tests for new functionality
5. **Update Examples**: Add usage examples to `Examples/PdfUsageExamples.cs`

### PDF Performance Considerations

- **Memory Management**: PDF documents can be memory-intensive
- **Provider Switching**: Consider performance impact when switching providers
- **Large Documents**: Test with large page counts and complex content
- **Stream Handling**: Use appropriate stream management for file operations

## Code Standards

### Naming Conventions

- **Namespaces**: 
  - Excel: `AMCode.Documents.Xlsx.ComponentName`
  - PDF: `AMCode.Pdf.ComponentName`
- **Classes**: PascalCase (e.g., `ExcelApplication`, `PdfDocument`)
- **Methods**: PascalCase (e.g., `SetText`, `AddText`)
- **Properties**: PascalCase (e.g., `Workbooks`, `Pages`)
- **Fields**: camelCase with underscore prefix (e.g., `_internalField`)

### Documentation Standards

- **XML Documentation**: Required for all public APIs
- **Code Comments**: Explain complex logic and business rules
- **README Updates**: Update when adding new features
- **API Documentation**: Keep current with implementation

### Error Handling

- **ArgumentException**: For invalid parameters
- **IndexOutOfRangeException**: For invalid indices
- **Custom Exceptions**: For domain-specific errors
- **Graceful Degradation**: Handle errors without crashing

## Performance Considerations

### Memory Management

- **IDisposable Pattern**: Implement for resources that need cleanup
- **Stream Management**: Properly dispose of file streams
- **Large File Handling**: Consider memory usage for large Excel files

### Optimization Guidelines

- **Lazy Loading**: Load data only when needed
- **Caching**: Cache frequently accessed data
- **Batch Operations**: Group related operations
- **Resource Pooling**: Reuse expensive objects

## Cross-Platform Development

### Platform-Specific Considerations

- **Path Separators**: Use `Path.Combine()` for file paths
- **Line Endings**: Handle different line ending formats
- **File Permissions**: Consider platform-specific file access
- **Environment Variables**: Use cross-platform environment variable access

### Testing on Multiple Platforms

- **Windows**: Primary development platform
- **macOS**: Verify compatibility
- **Linux**: Test on common distributions

## Debugging and Troubleshooting

### Common Issues

1. **Syncfusion License**: Ensure proper license configuration
2. **File Access**: Check file permissions and locks
3. **Memory Issues**: Monitor memory usage for large files
4. **Cross-Platform Paths**: Verify path construction

### Debugging Tools

- **Visual Studio Debugger**: Primary debugging tool
- **dotnet trace**: For performance analysis
- **dotnet dump**: For memory analysis
- **Logging**: Use structured logging for complex scenarios

## Release Process

### Version Management

- **Semantic Versioning**: Follow semver.org guidelines
- **Changelog**: Maintain detailed change log
- **Breaking Changes**: Document and communicate clearly

### Release Checklist

- [ ] All tests pass
- [ ] Documentation updated
- [ ] Version numbers incremented
- [ ] Changelog updated
- [ ] Package created and tested
- [ ] Cross-platform verification

## Contributing

### Pull Request Process

1. **Create Feature Branch**: From `main` branch
2. **Implement Changes**: Follow coding standards
3. **Add Tests**: Ensure comprehensive test coverage
4. **Update Documentation**: Keep docs current
5. **Submit PR**: With detailed description

### Code Review Guidelines

- **Functionality**: Verify correct implementation
- **Performance**: Check for performance regressions
- **Security**: Review for security vulnerabilities
- **Maintainability**: Ensure code is maintainable
- **Documentation**: Verify documentation completeness

## Support and Maintenance

### Issue Tracking

- **Bug Reports**: Detailed reproduction steps
- **Feature Requests**: Clear use case description
- **Performance Issues**: Include profiling data

### Long-term Maintenance

- **Dependency Updates**: Regular security updates
- **Platform Support**: Maintain cross-platform compatibility
- **Performance Monitoring**: Track performance metrics
- **User Feedback**: Incorporate user suggestions

## Resources

- **AMCode Documentation**: Main project documentation
- **Syncfusion Documentation**: Excel library reference
- **.NET Standard Guide**: Cross-platform development
- **NuGet Package Creation**: Package development guide
