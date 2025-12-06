# AMCode.Sql.Builder

**Version:** 1.0.0
**Target Framework:** .NET 9.0
**Last Updated:** 2025-01-27
**Purpose:** Fluent SQL query builder for constructing type-safe SQL SELECT statements with support for complex clauses, subqueries, and filtering

---

## Overview

AMCode.Sql.Builder is a comprehensive SQL query builder library that provides a fluent, type-safe API for constructing SQL SELECT statements. The library follows the builder pattern to enable intuitive, chainable method calls for building complex SQL queries programmatically.

This library is designed for scenarios where:

- You need to build SQL queries dynamically based on runtime conditions
- You want type-safe query construction without string concatenation
- You require support for complex WHERE clauses, subqueries, and aggregations
- You need validation and error messages for invalid queries
- You want formatted SQL output with proper indentation

## Architecture

The library follows a command pattern with clause-based composition. Each SQL clause (SELECT, FROM, WHERE, GROUP BY, ORDER BY) is implemented as a separate, composable component that can be combined to build complete queries.

### Design Principles

- **Fluent API**: Chainable methods for intuitive query construction
- **Type Safety**: Strong typing throughout to prevent runtime SQL errors
- **Composability**: Clauses are independent and can be combined flexibly
- **Validation**: Built-in validation with descriptive error messages
- **Factory Pattern**: Factory classes for creating clause instances
- **Extensibility**: Interface-based design allows for custom implementations

### Key Components

- **SelectCommandBuilder**: Main fluent builder for SELECT queries
- **SelectCommand**: Command object that generates SQL strings
- **Clause Builders**: Select, From, Where, OrderBy, GroupBy clause builders
- **Command Base**: Base class providing common command functionality
- **Filter Condition Builders**: Specialized builders for WHERE clause conditions

## Features

- **Fluent Query Building**: Chainable methods for intuitive SQL construction
- **SELECT Clause**: Support for column expressions, calculations, and aggregations
- **FROM Clause**: Tables, schemas, aliases, and subquery support
- **WHERE Clause**: Complex filtering with IN, BETWEEN, and custom conditions
- **GROUP BY Clause**: Grouping expressions with primary column support
- **ORDER BY Clause**: Sorting with custom formatters and sort providers
- **Subquery Support**: Nested SELECT queries in FROM clauses
- **Pagination**: LIMIT and OFFSET support
- **Validation**: Automatic validation with descriptive error messages
- **Formatting**: Configurable indentation for readable SQL output
- **Filter Organization**: Support for last-selected filters and global filters

## Dependencies

### Internal Dependencies

- **AMCode.Common** (1.0.0) - Common utilities, extensions, and filter structures

### External Dependencies

- **coverlet.collector** (3.1.2) - Code coverage collection (test dependency only)

## Project Structure

```
AMCode.Sql.Builder/
├── Components/
│   ├── Commands/                    # Core command classes
│   │   ├── SelectCommand.cs        # SELECT command implementation
│   │   ├── SelectCommandBuilder.cs # Fluent builder for SELECT
│   │   ├── CommandBase.cs          # Base command class
│   │   └── Models/                 # Command interfaces
│   ├── Select/                      # SELECT clause components
│   │   ├── SelectClause.cs         # SELECT clause builder
│   │   ├── SelectClauseCommand.cs  # SELECT clause command
│   │   └── Models/                 # SELECT interfaces
│   ├── From/                        # FROM clause components
│   │   ├── FromClause.cs           # FROM clause builder
│   │   ├── FromClauseCommand.cs    # FROM clause command
│   │   └── Models/                 # FROM interfaces
│   ├── Where/                       # WHERE clause components
│   │   ├── WhereClause.cs          # WHERE clause builder
│   │   ├── WhereClauseBuilder.cs   # WHERE clause builder implementation
│   │   ├── ConditionBuilders/      # Filter condition builders
│   │   ├── Factories/              # WHERE clause factories
│   │   └── Models/                 # WHERE interfaces
│   ├── OrderBy/                     # ORDER BY clause components
│   │   ├── OrderByClause.cs        # ORDER BY clause builder
│   │   └── Models/                 # ORDER BY interfaces
│   ├── GroupBy/                     # GROUP BY clause components
│   │   ├── GroupByClause.cs        # GROUP BY clause builder
│   │   └── Models/                 # GROUP BY interfaces
│   ├── Extensions/                  # Utility extensions
│   ├── Helpers/                     # Helper utilities
│   └── Mocks/                       # Mock interfaces for testing
└── AMCode.Sql.Builder.csproj        # Project file
```

## Key Interfaces

### ISelectCommandBuilder

**Location:** `Components/Commands/Models/ISelectCommandBuilder.cs`

**Purpose:** Fluent interface for building SELECT commands with chainable methods.

**Key Methods:**

- `Select(IEnumerable<string>)` - Add column expressions
- `Select(ISelectClauseCommand)` - Add SELECT clause
- `From(string schema, string table)` - Add table source
- `From(string schema, string table, string alias)` - Add table with alias
- `From(ISelectCommand, string alias)` - Add subquery source
- `Where(IWhereClauseCommand)` - Add WHERE clause
- `GroupBy(IGroupByClauseCommand)` - Add GROUP BY clause
- `OrderBy(IOrderByClauseCommand)` - Add ORDER BY clause
- `Limit(int?)` - Add LIMIT clause
- `Offset(int?)` - Add OFFSET clause
- `Indent(int)` - Set indentation level
- `End(bool)` - Add semicolon terminator
- `Build()` - Build the final command

**See Also:** [SelectCommandBuilder](Components/Commands/SelectCommandBuilder.cs)

### ISelectCommand

**Location:** `Components/Commands/Models/ISelectCommand.cs`

**Purpose:** Represents a complete SELECT command with all clauses.

**Key Properties:**

- `Select` - SELECT clause
- `From` - FROM clause
- `Where` - WHERE clause
- `GroupBy` - GROUP BY clause
- `OrderBy` - ORDER BY clause
- `Limit` - Record limit
- `Offset` - Record offset
- `IndentLevel` - Indentation level
- `EndCommand` - Whether to add semicolon

**Key Methods:**

- `CreateCommand()` - Generate SQL string
- `IsValid` - Validation status
- `InvalidCommandMessage` - Error message if invalid

**See Also:** [SelectCommand](Components/Commands/SelectCommand.cs)

### IWhereClause

**Location:** `Components/Where/Models/IWhereClause.cs`

**Purpose:** Interface for creating WHERE clauses from filter collections.

**Key Methods:**

- `CreateClause(IList<IFilter>)` - Create WHERE clause from filters
- `CreateClause(IList<IFilter>, string alias)` - Create with table alias
- `CreateClause(IWhereClauseParam)` - Create from parameter object

**See Also:** [WhereClause](Components/Where/WhereClause.cs)

## Key Classes

### SelectCommandBuilder

**Location:** `Components/Commands/SelectCommandBuilder.cs`

**Purpose:** Fluent builder class for constructing SELECT commands.

**Key Responsibilities:**

- Provides chainable methods for building queries
- Manages command state during construction
- Creates immutable SelectCommand instances

**Usage Example:**

```csharp
var builder = new SelectCommandBuilder();
var command = builder
    .Select(new[] { "id", "name", "COUNT(*)" })
    .From("dbo", "Users", "u")
    .Where(whereClause)
    .OrderBy(orderByClause)
    .Limit(100)
    .Build();
```

### SelectCommand

**Location:** `Components/Commands/SelectCommand.cs`

**Purpose:** Immutable command object representing a complete SELECT query.

**Key Responsibilities:**

- Stores all query clauses and configuration
- Generates SQL string from components
- Validates query completeness
- Provides formatted output with indentation

**Usage Example:**

```csharp
var command = new SelectCommand();
command.Select = selectClause;
command.From = fromClause;
command.Where = whereClause;
string sql = command.CreateCommand();
```

### WhereClauseBuilder

**Location:** `Components/Where/WhereClauseBuilder.cs`

**Purpose:** Builds WHERE clauses with support for filter condition sections.

**Key Responsibilities:**

- Organizes filter conditions into sections
- Supports last-selected filters (OR) and default filters (AND)
- Combines conditions with appropriate operators
- Creates WHERE clause commands

**Usage Example:**

```csharp
var builder = new WhereClauseBuilder();
builder.AddFilterCondition(filterSection, FilterConditionSectionType.LastSelected);
builder.AddFilterCondition(filterSection, FilterConditionSectionType.Default);
var whereClause = builder.CreateWhereClause();
```

## Usage Examples

### Basic Usage

```csharp
using AMCode.Sql.Commands;
using AMCode.Sql.Select;
using AMCode.Sql.From;

// Build a simple SELECT query
var command = new SelectCommandBuilder()
    .Select(new[] { "id", "name", "email" })
    .From("dbo", "Users")
    .Build();

string sql = command.CreateCommand();
// Output: SELECT id, name, email
//         FROM dbo.Users
```

### With WHERE Clause

```csharp
using AMCode.Sql.Commands;
using AMCode.Sql.Where;

// Build query with WHERE clause
var whereClause = whereClauseFactory.CreateClause(selectedFilters);
var command = new SelectCommandBuilder()
    .Select(new[] { "id", "name" })
    .From("dbo", "Users", "u")
    .Where(whereClause)
    .Build();

string sql = command.CreateCommand();
```

### With Subquery

```csharp
// Build subquery
var subquery = new SelectCommandBuilder()
    .Select(new[] { "MAX(id)" })
    .From("dbo", "Users")
    .Build();

// Use subquery in FROM clause
var command = new SelectCommandBuilder()
    .Select(new[] { "*" })
    .From(subquery, "sub")
    .Build();
```

### With Pagination

```csharp
var command = new SelectCommandBuilder()
    .Select(new[] { "id", "name" })
    .From("dbo", "Users")
    .OrderBy(orderByClause)
    .Offset(10)
    .Limit(20)
    .Build();

string sql = command.CreateCommand();
// Includes: OFFSET 10 LIMIT 20
```

### With GROUP BY and ORDER BY

```csharp
var command = new SelectCommandBuilder()
    .Select(new[] { "category", "COUNT(*)" })
    .From("dbo", "Products")
    .GroupBy(groupByClause)
    .OrderBy(orderByClause)
    .Build();
```

### Formatted Output

```csharp
var command = new SelectCommandBuilder()
    .Select(new[] { "id", "name" })
    .From("dbo", "Users")
    .Indent(2)  // 2 levels of indentation
    .End(true)  // Add semicolon
    .Build();

string sql = command.CreateCommand();
// Output with proper indentation and semicolon
```

### Complex WHERE Clause

```csharp
using AMCode.Sql.Where;
using AMCode.Common.FilterStructures;

// Create WHERE clause from filters
var whereClause = new WhereClause(
    whereClauseBuilderFactory,
    filterConditionOrganizerFactory,
    WhereClauseBuilderType.Default
);

var whereCommand = whereClause.CreateClause(selectedFilters, "u");
var command = new SelectCommandBuilder()
    .Select(new[] { "*" })
    .From("dbo", "Users", "u")
    .Where(whereCommand)
    .Build();
```

## Configuration

### Dependency Injection Setup

```csharp
using AMCode.Sql.Where;
using Microsoft.Extensions.DependencyInjection;

services.AddSingleton<IWhereClauseBuilderFactory, WhereClauseBuilderFactory>();
services.AddSingleton<IFilterConditionOrganizerFactory, FilterConditionOrganizerFactory>();
services.AddTransient<IWhereClause, WhereClause>();
```

### Factory Configuration

```csharp
// Create WHERE clause factory
var whereClauseBuilderFactory = new WhereClauseBuilderFactory();
var filterConditionOrganizerFactory = new FilterConditionOrganizerFactory();

var whereClause = new WhereClause(
    whereClauseBuilderFactory,
    filterConditionOrganizerFactory,
    WhereClauseBuilderType.Default
);
```

## Testing

### Test Projects

- **AMCode.Sql.Builder.UnitTests**: Unit tests for individual components

  - Command builder tests
  - Clause builder tests
  - Filter condition tests
  - [Test Project README](../AMCode.Sql.Builder.UnitTests/README.md)

- **AMCode.Sql.Builder.IntegrationTests**: Integration tests for complete queries
  - End-to-end query generation
  - Complex query scenarios
  - [Test Project README](../AMCode.Sql.Builder.IntegrationTests/README.md)

### Running Tests

```bash
# Run all tests
dotnet test AMCode.Sql.Builder.sln

# Run unit tests only
dotnet test AMCode.Sql.Builder.UnitTests

# Run integration tests only
dotnet test AMCode.Sql.Builder.IntegrationTests
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Commands](Components/Commands/README.md) - Command builders and base classes
- [Select](Components/Select/README.md) - SELECT clause implementation
- [From](Components/From/README.md) - FROM clause implementation
- [Where](Components/Where/README.md) - WHERE clause implementation
- [OrderBy](Components/OrderBy/README.md) - ORDER BY clause implementation
- [GroupBy](Components/GroupBy/README.md) - GROUP BY clause implementation

## Related Libraries

- [AMCode.Common](../../commonlibrary/AMCode.Common/README.md) - Common utilities and filter structures
- [AMCode.Columns](../../columnslibrary/AMCode.Columns/README.md) - Column definition interfaces
- [AMCode.Data](../../datalibrary/AMCode.Data/README.md) - Data access layer that may use this builder

## Migration Notes

### From String Concatenation

If migrating from string-based SQL construction:

1. **Replace String Building**: Use `SelectCommandBuilder` instead of string concatenation
2. **Use Fluent API**: Chain methods instead of building strings
3. **Leverage Validation**: Use `IsValid` property to check query validity
4. **Handle Errors**: Use `InvalidCommandMessage` for error reporting

### Breaking Changes

- **Interface Changes**: Check interface compatibility when upgrading
- **Factory Dependencies**: Ensure factory instances are properly configured
- **Filter Structures**: Verify filter structure compatibility with AMCode.Common

## Known Issues

- **Subquery Nesting**: Deep nesting may impact performance
- **Complex WHERE Clauses**: Very complex WHERE clauses may generate verbose SQL
- **Validation**: Some edge cases may not be caught by validation

## Future Considerations

Potential enhancements:

- Support for INSERT, UPDATE, DELETE commands
- JOIN clause support
- UNION clause support
- Parameterized query support
- Query optimization hints
- Database-specific SQL generation

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27
**Maintained By:** Development Team
