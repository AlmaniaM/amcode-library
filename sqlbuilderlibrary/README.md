# AMCode SQL Builder Library

This project provides an API for building SQL clause commands. Example clause commands would be SELECT, FROM, WHERE, etc...

## Features

- **SELECT Clause Builder**: Create SELECT statements with column expressions
- **FROM Clause Builder**: Support for tables, schemas, aliases, and subqueries
- **WHERE Clause Builder**: Complex filtering with IN, BETWEEN, and custom conditions
- **GROUP BY Clause Builder**: Grouping expressions and aggregations
- **ORDER BY Clause Builder**: Sorting with custom formatters
- **Command Builder Pattern**: Fluent API for building complex SQL queries

## Quick Start

```csharp
using AMCode.Sql.Commands;
using AMCode.Sql.Select;
using AMCode.Sql.From;
using AMCode.Sql.Where;

// Build a simple SELECT query
var selectCommand = new SelectCommandBuilder()
    .Select(new[] { "column1", "column2", "COUNT(*)" })
    .From("schema", "table", "t")
    .Where(whereClause)
    .Build();

string sql = selectCommand.CreateCommand();
```

## Local Development Setup

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code with C# extension

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Create NuGet package
dotnet pack -c Release -o local-packages
```

### Package Dependencies

This library depends on:
- `AMCode.Common` - Common utilities and extensions
- `AMCode.Columns` - Column definition interfaces

## Architecture

The library follows a command pattern with the following main components:

- **Commands**: Core SQL command builders (SelectCommand, CommandBase)
- **Clauses**: Individual SQL clause implementations (Select, From, Where, GroupBy, OrderBy)
- **Extensions**: Utility extensions for string handling and filtering
- **Models**: Interfaces and data models for type safety

## Testing

The project includes comprehensive unit tests and integration tests:

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test complete SQL query generation
- **Test Coverage**: Aim for high test coverage on all public APIs

## Contributing

When contributing to this project:

1. Write tests for new features
2. Follow the existing code architecture
3. Update documentation for public APIs
4. Ensure all tests pass before submitting

## License

This project is part of the AMCode library suite.