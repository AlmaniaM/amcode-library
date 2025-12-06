# AMCode.Sql.Builder.IntegrationTests

**Version:** 1.0.0  
**Target Framework:** .NET 9.0  
**Last Updated:** 2025-01-27  
**Purpose:** Integration tests for AMCode.Sql.Builder library, verifying component interactions and SQL query construction

---

## Overview

AMCode.Sql.Builder.IntegrationTests is a test project that validates the integration between different components of the SQL Builder library. These tests ensure that SQL query components (SELECT, FROM, GROUP BY, ORDER BY, LIMIT, OFFSET) work correctly together when building complex SQL queries.

The integration tests focus on:
- Component interaction and composition
- SQL query construction with multiple clauses
- Subquery handling in FROM clauses
- Query builder fluent API correctness
- SQL string generation accuracy

## Architecture

The test project follows NUnit testing patterns with:
- **Test Fixtures**: Organized by component (Select, From)
- **Test Cases**: Parameterized tests for different scenarios
- **Mocking**: Uses Moq for interface mocking
- **Assertions**: Validates SQL string output correctness

### Test Organization

- **Components/Select/**: Tests for SELECT command builder integration
- **Components/From/**: Tests for FROM clause integration with SELECT commands

## Features

- **SELECT Query Building**
  - Basic SELECT queries with FROM clause
  - SELECT with GROUP BY
  - SELECT with GROUP BY and ORDER BY
  - SELECT with LIMIT and OFFSET
  - Complex nested SELECT queries (subqueries)

- **FROM Clause Integration**
  - FROM clause with table names
  - FROM clause with schema and table
  - FROM clause with aliases
  - FROM clause with SELECT subqueries

- **Query Composition**
  - Multiple clause combinations
  - Nested query support
  - Query indentation
  - Query termination (semicolon handling)

## Dependencies

### Internal Dependencies

- **AMCode.Sql.Builder** - The library being tested (project reference)
- **AMCode.Common** (1.0.0) - Common utilities (NuGet package)

### External Dependencies

- **Microsoft.NET.Test.Sdk** (17.0.0) - Test SDK
- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Test adapter for Visual Studio
- **Moq** (4.16.1) - Mocking framework
- **coverlet.collector** (3.1.2) - Code coverage collection

## Project Structure

```
AMCode.Sql.Builder.IntegrationTests/
├── Components/
│   ├── Select/
│   │   └── SelectCommandBuilderTest.cs    # SELECT command builder integration tests
│   └── From/
│       ├── FromClauseTest.cs              # FROM clause basic tests
│       └── FromClauseCommandWithSelectCommandTest.cs  # FROM with subquery tests
└── AMCode.Sql.Builder.IntegrationTests.csproj
```

## Test Coverage

### SelectCommandBuilderTest

**Location:** `Components/Select/SelectCommandBuilderTest.cs`

**Test Scenarios:**

- `ShouldBuildSelectQuery` - Basic SELECT with FROM clause
- `ShouldBuildSelectQueryWithGroupBy` - SELECT with GROUP BY clause
- `ShouldBuildSelectQueryWithGroupByAndOrderBy` - SELECT with GROUP BY and ORDER BY
- `ShouldBuildSelectQueryWithGroupByAndOrderByAndLimitOffset` - Complete query with all clauses
- `ShouldBuildSelectCountQuery` - Nested SELECT query (subquery in FROM)

**Key Validations:**

- SQL string generation correctness
- Clause ordering and formatting
- Semicolon termination handling
- Subquery indentation

### FromClauseTest

**Location:** `Components/From/FromClauseTest.cs`

**Test Scenarios:**

- `ShouldCreateAFromClauseWithTableName` - FROM clause with schema and table
- `ShouldCreateAFromClauseWithSchemaTableNameAndAlias` - FROM clause with alias
- `ShouldCreateAFromClauseWithASelectCommand` - FROM clause with SELECT subquery

**Key Validations:**

- FROM clause string generation
- Alias handling
- Subquery formatting

### FromClauseCommandWithSelectCommandTest

**Location:** `Components/From/FromClauseCommandWithSelectCommandTest.cs`

**Test Scenarios:**

- `ShouldBuildFromClauseCommandWithSubQuery` - FROM clause with SELECT subquery
- `ShouldBuildFromClauseCommandWithSubQueryToString` - String conversion
- `ShouldBuildFromClauseCommandWithSubQueryGetFrom` - FROM clause retrieval

**Key Validations:**

- Subquery integration
- Command interface methods
- String representation consistency

## Usage Examples

### Running All Tests

```bash
# Run all integration tests
dotnet test sqlbuilderlibrary/AMCode.Sql.Builder.IntegrationTests

# Run with code coverage
dotnet test sqlbuilderlibrary/AMCode.Sql.Builder.IntegrationTests --collect:"XPlat Code Coverage"
```

### Running Specific Test Fixtures

```bash
# Run only SELECT tests
dotnet test --filter "FullyQualifiedName~SelectCommandBuilderTest"

# Run only FROM tests
dotnet test --filter "FullyQualifiedName~FromClause"
```

### Example Test Output

```
Test Run Successful.
Total tests: 8
     Passed: 8
 Total time: 0.5 seconds
```

## Test Patterns

### Fluent API Testing

```csharp
var selectCommand = new SelectCommandBuilder()
    .Select(selectClause)
    .From(schemaName, tableName)
    .GroupBy(groupByClause)
    .OrderBy(orderByClause)
    .Offset(1)
    .Limit(100)
    .End(true)
    .Build();

Assert.AreEqual(expectedSql, selectCommand.ToString());
```

### Subquery Testing

```csharp
var subquery = new SelectCommandBuilder()
    .Select(new SelectClauseCommand(columns))
    .From(schemaName, tableName)
    .Build();

var fromClause = new FromClause()
    .CreateClause(subquery, "Alias")
    .CreateCommand();

Assert.AreEqual(expectedFromClause, fromClause);
```

## Configuration

### Test Project Configuration

No special configuration required. Tests use:
- NUnit test framework
- Moq for interface mocking
- Standard .NET test SDK

### Test Data

Tests use:
- Mock objects for `IDataQueryColumnDefinition`
- Test schema and table names
- Parameterized test cases for boolean flags

## Related Projects

- **[AMCode.Sql.Builder](../AMCode.Sql.Builder/README.md)** - The library being tested
- **[AMCode.Sql.Builder.UnitTests](../AMCode.Sql.Builder.UnitTests/README.md)** - Unit tests for individual components
- **[AMCode.Common](../commonlibrary/AMCode.Common/README.md)** - Common utilities dependency

## Testing Strategy

### Integration vs Unit Tests

- **Unit Tests** (AMCode.Sql.Builder.UnitTests): Test individual components in isolation
- **Integration Tests** (This project): Test component interactions and query composition

### Test Coverage Goals

- All clause combinations
- Subquery scenarios
- Edge cases (empty clauses, null handling)
- SQL string formatting

## Known Issues

- None currently identified

## Future Considerations

- Additional integration scenarios
- Performance testing
- Database-specific SQL dialect testing
- Complex query pattern testing

---

**See Also:**

- [AMCode.Sql.Builder README](../AMCode.Sql.Builder/README.md) - Main library documentation
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

