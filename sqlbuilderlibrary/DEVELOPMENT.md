# AMCode SQL Builder Library - Development Guide

This guide provides detailed information for developers working on the AMCode SQL Builder Library.

## Project Structure

```
sqlbuilderlibrary/
├── AMCode.Sql.Builder/                    # Main library project
│   ├── Components/
│   │   ├── Commands/                      # Core command builders
│   │   ├── From/                          # FROM clause implementations
│   │   ├── Select/                        # SELECT clause implementations
│   │   ├── Where/                         # WHERE clause implementations
│   │   ├── GroupBy/                       # GROUP BY clause implementations
│   │   ├── OrderBy/                       # ORDER BY clause implementations
│   │   ├── Extensions/                    # Utility extensions
│   │   └── Helpers/                       # Helper utilities
│   ├── AMCode.Sql.Builder.csproj         # Main project file
│   └── AMCode.Sql.Builder.nuspec         # NuGet package specification
├── AMCode.Sql.Builder.UnitTests/         # Unit tests
├── AMCode.Sql.Builder.IntegrationTests/  # Integration tests
├── local-packages/                        # Local NuGet packages
├── global.json                           # .NET SDK version
├── nuget.config                          # NuGet configuration
└── README.md                             # Project overview
```

## Development Environment Setup

### Prerequisites

1. **.NET 8.0 SDK**: Install the latest .NET 8.0 SDK
2. **IDE**: Visual Studio 2022 or VS Code with C# extension
3. **Git**: For version control

### Local Package Dependencies

The project depends on other AMCode packages that should be built locally:

```bash
# Build AMCode.Common first
cd ../commonlibrary
dotnet pack -c Release -o local-packages

# Build AMCode.Columns (if exists)
cd ../columnslibrary
dotnet pack -c Release -o local-packages

# Return to SQL Builder Library
cd ../sqlbuilderlibrary
dotnet restore
```

## Building and Testing

### Build Commands

```bash
# Restore dependencies
dotnet restore

# Build in Debug mode
dotnet build

# Build in Release mode
dotnet build -c Release

# Clean build artifacts
dotnet clean
```

### Testing Commands

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity normal

# Run specific test project
dotnet test AMCode.Sql.Builder.UnitTests/

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Package Creation

```bash
# Create NuGet package
dotnet pack -c Release -o local-packages

# Verify package contents
dotnet pack -c Release -o local-packages --verbosity normal
```

## Code Architecture

### Command Pattern Implementation

The library uses the Command pattern to build SQL queries:

```csharp
// Base command interface
public interface ICommand
{
    string CreateCommand();
    string CommandType { get; }
    string GetCommandValue();
    bool IsValid { get; }
    string InvalidCommandMessage { get; }
}

// Specific command implementations
public class SelectCommand : CommandBase, ISelectCommand
{
    // Implementation details
}
```

### Clause Architecture

Each SQL clause follows a consistent pattern:

1. **Interface**: Defines the contract (e.g., `ISelectClauseCommand`)
2. **Implementation**: Concrete implementation (e.g., `SelectClauseCommand`)
3. **Factory**: Creates instances (e.g., `SelectClauseFactory`)
4. **Models**: Supporting data structures

### Extension Methods

Utility extensions are organized by functionality:

- `StringExtensionsVertica`: Vertica-specific string handling
- `EnumerableExtensionsVertica`: Collection utilities for Vertica
- `FilterItemExtensions`: Filter-related extensions

## Adding New Features

### 1. Define the Interface

Create the interface in the appropriate `Models` folder:

```csharp
namespace AMCode.Sql.NewFeature.Models
{
    public interface INewFeatureCommand : IClauseCommand
    {
        // Define the contract
    }
}
```

### 2. Implement the Feature

Create the implementation:

```csharp
namespace AMCode.Sql.NewFeature
{
    public class NewFeatureCommand : ColumnExpressionCommandBase, INewFeatureCommand
    {
        // Implementation
    }
}
```

### 3. Create Factory (if needed)

```csharp
namespace AMCode.Sql.NewFeature
{
    public class NewFeatureFactory : INewFeatureFactory
    {
        public INewFeature Create() => new NewFeature();
    }
}
```

### 4. Write Tests

Create comprehensive unit tests:

```csharp
[TestFixture]
public class NewFeatureCommandTest
{
    [Test]
    public void ShouldCreateValidCommand()
    {
        // Test implementation
    }
}
```

### 5. Update Documentation

Update this file and README.md with new feature information.

## Testing Guidelines

### Unit Tests

- Test each public method and property
- Test edge cases and error conditions
- Use descriptive test names
- Follow AAA pattern (Arrange, Act, Assert)

### Integration Tests

- Test complete SQL query generation
- Test complex scenarios with multiple clauses
- Verify generated SQL syntax

### Test Data

- Use meaningful test data
- Create reusable test fixtures
- Mock external dependencies

## Code Style and Standards

### Naming Conventions

- **Classes**: PascalCase (e.g., `SelectCommand`)
- **Interfaces**: PascalCase with 'I' prefix (e.g., `ISelectCommand`)
- **Methods**: PascalCase (e.g., `CreateCommand`)
- **Properties**: PascalCase (e.g., `CommandType`)
- **Fields**: camelCase with underscore prefix for private fields

### Documentation

- Document all public APIs with XML comments
- Include parameter descriptions
- Provide usage examples where helpful
- Update this guide when adding new patterns

### Error Handling

- Use meaningful error messages
- Implement `IValidCommand` for validation
- Provide clear `InvalidCommandMessage` properties

## Debugging

### Common Issues

1. **Namespace Conflicts**: Ensure all using statements are updated
2. **Package Dependencies**: Verify local packages are built and available
3. **Test Failures**: Check for platform-specific issues (Windows vs macOS)

### Debug Configuration

VS Code launch configuration is provided in `.vscode/launch.json` for debugging tests.

## Performance Considerations

- Use `StringBuilder` for complex string concatenation
- Avoid unnecessary object creation in hot paths
- Consider caching for frequently used objects
- Profile with `dotnet-counters` if performance issues arise

## Release Process

1. **Update Version**: Update version in `.nuspec` file
2. **Run Tests**: Ensure all tests pass
3. **Build Package**: Create Release package
4. **Test Package**: Verify package in test project
5. **Document Changes**: Update release notes

## Troubleshooting

### Build Issues

```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore --force

# Clean and rebuild
dotnet clean
dotnet build
```

### Test Issues

```bash
# Run tests with more detail
dotnet test --logger "console;verbosity=detailed"

# Run specific test
dotnet test --filter "TestName"
```

### Package Issues

```bash
# List local packages
dotnet nuget list source

# Clear package cache
dotnet nuget locals all --clear
```

## Contributing

1. Create feature branch from main
2. Implement changes with tests
3. Update documentation
4. Submit pull request with clear description
5. Ensure CI/CD passes

## Support

For questions or issues:
1. Check this documentation
2. Review existing tests for examples
3. Create issue with detailed description
4. Include relevant code snippets and error messages
