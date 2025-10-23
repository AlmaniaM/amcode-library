# AMCode.Data Development Guide

This guide provides detailed information for developers working on the AMCode.Data library.

## Development Environment Setup

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code with C# extension
- Git for version control
- Docker (for integration tests)

### Local Development Configuration

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd csharp-libs/datalibrary
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the Solution**
   ```bash
   dotnet build
   ```

4. **Run Tests**
   ```bash
   # Unit tests only
   dotnet test AMCode.Data.UnitTests
   
   # All tests
   dotnet test
   ```

## Project Architecture

### Core Components

- **Data Providers**: Main interfaces for data access
  - `IGenericDataProvider<T>` - Strongly-typed data access
  - `IExpandoObjectDataProvider` - Dynamic object data access

- **Database Bridges**: Database-specific implementations
  - `IDbBridge` - Generic database operations
  - `DbBridge` - Base implementation

- **Connection Factories**: Database connection management
  - `IDbConnectionFactory` - Connection creation
  - `IDbCommandFactory` - Command creation

- **Extensions**: Utility methods
  - `DataReaderExtensions` - IDataReader extensions
  - `ValueParser` - Type conversion utilities

### Database Providers

- **Vertica**: Specialized Vertica database support
- **ODBC**: Generic ODBC database support

## Development Workflow

### Making Changes

1. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Make Your Changes**
   - Follow existing code patterns
   - Add appropriate tests
   - Update documentation if needed

3. **Test Your Changes**
   ```bash
   dotnet test
   ```

4. **Build and Package**
   ```bash
   dotnet pack --configuration Release
   ```

### Code Standards

- **Naming Conventions**: Follow C# naming conventions
- **Documentation**: Add XML documentation for public APIs
- **Error Handling**: Use appropriate exception types
- **Async Patterns**: Use async/await consistently

## Testing Strategy

### Unit Tests

- Located in `AMCode.Data.UnitTests/`
- Test individual components in isolation
- Use Moq for mocking dependencies
- Aim for high code coverage

### Integration Tests

- Located in `AMCode.Data.SQLTests/`
- Test against real database connections
- Use Docker containers for test databases
- Include setup/teardown for test data

### Test Data Management

- Use CSV files for test data in `MockFiles/` directories
- Create test models in `Models/` directories
- Use consistent naming patterns for test classes

## Package Management

### Local Packages

The project uses local NuGet packages stored in `local-packages/`:

```xml
<PackageSource Include="Local" value="./local-packages" />
```

### Dependencies

Key dependencies:
- `AMCode.Columns` - Data transformation
- `AMCode.Common` - Common utilities
- `AMCode.Vertica.Client` - Vertica client
- `Newtonsoft.Json` - JSON handling

### Version Management

- Update version numbers in `.csproj` files
- Update package references as needed
- Test compatibility with dependency updates

## Debugging

### VS Code Configuration

The project includes VS Code configuration:
- `launch.json` - Debug configurations
- `tasks.json` - Build tasks

### Common Issues

1. **Connection String Issues**
   - Verify connection strings in test configuration
   - Check database accessibility

2. **Package Resolution**
   - Ensure local packages are built
   - Check NuGet configuration

3. **Test Failures**
   - Verify test data files are present
   - Check database test environment

## Performance Considerations

### Connection Management

- Use connection pooling where possible
- Dispose connections properly
- Consider async patterns for I/O operations

### Memory Management

- Dispose IDisposable objects
- Use appropriate data structures
- Consider streaming for large datasets

## Deployment

### NuGet Package Creation

```bash
# Create release package
dotnet pack --configuration Release --output ./local-packages

# Package will be created as AMCode.Data.{version}.nupkg
```

### Version Updates

1. Update version in `.csproj` files
2. Update `CHANGELOG.md` if applicable
3. Create Git tag for release
4. Build and publish package

## Troubleshooting

### Build Issues

- Ensure .NET 8.0 SDK is installed
- Check NuGet package sources
- Verify project references

### Test Issues

- Check database connectivity
- Verify test data files
- Review test configuration

### Runtime Issues

- Check connection strings
- Verify database permissions
- Review error logs

## Contributing Guidelines

1. **Code Quality**: Maintain high code quality standards
2. **Testing**: Add tests for new functionality
3. **Documentation**: Update documentation as needed
4. **Review Process**: All changes require code review
5. **Commit Messages**: Use clear, descriptive commit messages

## Resources

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [NUnit Testing Framework](https://nunit.org/)
- [Moq Mocking Library](https://github.com/moq/moq4)
- [Vertica Documentation](https://www.vertica.com/docs/)
