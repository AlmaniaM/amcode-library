# AMCode.Sql.Builder.UnitTests

**Version:** 1.0  
**Target Framework:** .NET 9.0  
**Last Updated:** 2025-01-27  
**Purpose:** Unit tests for AMCode.Sql.Builder library

---

## Overview

AMCode.Sql.Builder.UnitTests provides comprehensive unit test coverage for the AMCode.Sql.Builder library. The test suite validates SQL query building functionality including SELECT, FROM, WHERE, ORDER BY, and GROUP BY clauses, as well as command construction and extension methods.

## Test Framework

- **Testing Framework:** NUnit 3.13.2
- **Mocking Framework:** Moq 4.16.1
- **Code Coverage:** Coverlet 3.1.2
- **Test SDK:** Microsoft.NET.Test.Sdk 17.0.0

## Test Structure

```
AMCode.Sql.Builder.UnitTests/
├── Components/                   # Component-specific tests
│   ├── Commands/                # Command tests
│   │   ├── CommandBaseTest.cs
│   │   ├── SelectCommandBuilderTest.cs
│   │   └── SelectCommandTest.cs
│   ├── Extensions/              # Extension method tests
│   │   ├── EnumerableExtensionsVerticaTest.cs
│   │   └── StringExtensionsVerticaTest.cs
│   ├── From/                    # FROM clause tests
│   │   ├── FromClauseCommandTest.cs
│   │   ├── FromClauseCommandWithSelectCommandTest.cs
│   │   ├── FromClauseFactoryTest.cs
│   │   └── FromClauseTest.cs
│   ├── GroupBy/                 # GROUP BY clause tests
│   │   ├── GroupByClauseCommandTest.cs
│   │   ├── GroupByClauseFactoryTest.cs
│   │   └── GroupByTest.cs
│   ├── OrderBy/                 # ORDER BY clause tests
│   │   ├── OrderByClauseCommandTest.cs
│   │   ├── OrderByClauseFactoryTest.cs
│   │   └── OrderByClauseTest.cs
│   ├── Select/                  # SELECT clause tests
│   │   ├── SelectClauseCommandTest.cs
│   │   ├── SelectClauseFactoryTest.cs
│   │   └── SelectClauseTest.cs
│   └── Where/                   # WHERE clause tests
│       ├── CompareDateTimeFilterItemTest.cs
│       ├── DefaultFilterConditionOrganizerTest.cs
│       ├── FilterBetweenConditionBuilderBaseTest.cs
│       ├── FilterBetweenConditionDateValuesBuilderTest.cs
│       ├── FilterBetweenConditionIntegerValuesBuilderTest.cs
│       ├── FilterBetweenConditionTest.cs
│       ├── FilterConditionSectionTest.cs
│       ├── FilterInConditionTest.cs
│       ├── FilterIsConditionTest.cs
│       ├── GlobalFiltersFilterConditionOrganizerTest.cs
│       ├── IntegrationWhereClauseTest.cs
│       ├── WhereClauseCommandTest.cs
│       └── WhereClauseTest.cs
└── Globals/                      # Shared test utilities
    ├── Builders/
    │   └── FiltersBuilder.cs    # Test fixture builder
    └── TestFixture.cs            # Base test fixture
```

## Test Categories

### Command Tests

Tests for SQL command construction and composition:

- **CommandBaseTest**: Base command functionality
- **SelectCommandTest**: SELECT command construction and SQL generation
- **SelectCommandBuilderTest**: Fluent builder pattern for SELECT commands

**Key Test Scenarios:**
- Command clause composition
- SQL string generation
- Optional clause handling
- Command duplication prevention

### SELECT Clause Tests

Tests for SELECT clause generation:

- **SelectClauseTest**: Basic SELECT clause creation
- **SelectClauseCommandTest**: SELECT command generation
- **SelectClauseFactoryTest**: Factory pattern for SELECT clauses

**Key Test Scenarios:**
- Column selection
- Custom query name support
- Empty query handling
- Expression generation

### FROM Clause Tests

Tests for FROM clause generation:

- **FromClauseTest**: Basic FROM clause creation
- **FromClauseCommandTest**: FROM command generation
- **FromClauseFactoryTest**: Factory pattern for FROM clauses
- **FromClauseCommandWithSelectCommandTest**: Integration with SELECT commands
- **TableClauseCommandTest**: Table clause handling

**Key Test Scenarios:**
- Table name specification
- Alias support
- Join operations
- Integration with SELECT commands

### WHERE Clause Tests

Comprehensive tests for WHERE clause generation and filtering:

- **WhereClauseTest**: Basic WHERE clause creation
- **WhereClauseCommandTest**: WHERE command generation
- **FilterBetweenConditionTest**: BETWEEN condition handling
- **FilterInConditionTest**: IN condition handling
- **FilterIsConditionTest**: IS NULL/IS NOT NULL conditions
- **FilterConditionSectionTest**: Complex filter condition sections
- **CompareDateTimeFilterItemTest**: DateTime comparison filters
- **DefaultFilterConditionOrganizerTest**: Default filter organization
- **GlobalFiltersFilterConditionOrganizerTest**: Global filter organization
- **FilterBetweenConditionBuilderBaseTest**: Base builder for BETWEEN conditions
- **FilterBetweenConditionDateValuesBuilderTest**: Date value BETWEEN conditions
- **FilterBetweenConditionIntegerValuesBuilderTest**: Integer value BETWEEN conditions
- **IntegrationWhereClauseTest**: Integration scenarios

**Key Test Scenarios:**
- Simple WHERE clause generation
- Multiple filter conditions
- BETWEEN conditions (dates, integers)
- IN conditions
- IS NULL/IS NOT NULL conditions
- Filter condition organization
- Complex filter combinations
- DateTime comparisons

### ORDER BY Clause Tests

Tests for ORDER BY clause generation:

- **OrderByClauseTest**: Basic ORDER BY clause creation
- **OrderByClauseCommandTest**: ORDER BY command generation
- **OrderByClauseFactoryTest**: Factory pattern for ORDER BY clauses

**Key Test Scenarios:**
- Column ordering
- ASC/DESC specification
- Multiple column ordering
- Expression-based ordering

### GROUP BY Clause Tests

Tests for GROUP BY clause generation:

- **GroupByTest**: Basic GROUP BY clause creation
- **GroupByClauseCommandTest**: GROUP BY command generation
- **GroupByClauseFactoryTest**: Factory pattern for GROUP BY clauses

**Key Test Scenarios:**
- Column grouping
- Multiple column grouping
- Expression-based grouping

### Extension Method Tests

Tests for extension methods:

- **EnumerableExtensionsVerticaTest**: Enumerable extensions for Vertica
- **StringExtensionsVerticaTest**: String extensions for Vertica

**Key Test Scenarios:**
- Enumerable operations
- String manipulation for SQL
- Vertica-specific extensions

## Test Utilities

### TestFixture

**Location:** `Globals/TestFixture.cs`

Base test fixture class for building filter structures from JSON files and programmatically.

**Key Features:**
- Load filters from JSON files
- Programmatic filter building
- Fluent builder pattern for filters

**Usage Example:**
```csharp
var filters = new TestFixture()
    .Filters
        .AddFilterWith
            .FilterIdName("FilterId1", "Filter ID #1")
            .FilterName("Filter1", "Filter #1")
            .FilterItem("#1", "1", true, true)
            .Save()
        .Build();
```

### FiltersBuilder

**Location:** `Globals/Builders/FiltersBuilder.cs`

Fluent builder for creating filter structures in tests.

## Running Tests

### Run All Tests

```bash
dotnet test sqlbuilderlibrary/AMCode.Sql.Builder.UnitTests
```

### Run Specific Test Category

```bash
# Run WHERE clause tests
dotnet test --filter "FullyQualifiedName~Where"

# Run SELECT clause tests
dotnet test --filter "FullyQualifiedName~Select"

# Run command tests
dotnet test --filter "FullyQualifiedName~Command"
```

### Run with Code Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Run Specific Test

```bash
dotnet test --filter "FullyQualifiedName~SelectClauseTest.ShouldCreateASelectClause"
```

## Test Coverage

The test suite provides comprehensive coverage for:

- ✅ SELECT clause generation
- ✅ FROM clause generation
- ✅ WHERE clause generation (including complex filters)
- ✅ ORDER BY clause generation
- ✅ GROUP BY clause generation
- ✅ Command composition
- ✅ Factory patterns
- ✅ Extension methods
- ✅ Filter condition organization
- ✅ DateTime and integer comparisons
- ✅ BETWEEN, IN, IS conditions

## Dependencies

### Internal Dependencies

- **AMCode.Sql.Builder** - The library being tested
- **AMCode.Common** (1.0.0) - Common utilities and filter structures

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Test adapter
- **Moq** (4.16.1) - Mocking framework
- **Microsoft.NET.Test.Sdk** (17.0.0) - Test SDK
- **coverlet.collector** (3.1.2) - Code coverage collection

## Test Patterns

### Mocking Pattern

Tests use Moq for creating mock objects:

```csharp
var queriesMoq = new List<Mock<IGetQueryExpression>> { new(), new(), new() };
queriesMoq.ForEach((queryMoq, i) =>
{
    queryMoq.Setup(moq => moq.GetExpression()).Returns($"TestColumn{i + 1}");
    queryMoq.Setup(moq => moq.IsVisible).Returns(true);
});
```

### Test Fixture Pattern

Tests use TestFixture for building filter structures:

```csharp
var filters = new TestFixture()
    .Filters
        .AddFilterWith
            .FilterIdName("FilterId1", "Filter ID #1")
            .FilterName("Filter1", "Filter #1")
            .FilterItem("#1", "1", true, true)
            .Save()
        .Build();
```

### Assertion Pattern

Tests use NUnit assertions:

```csharp
Assert.AreEqual(expected, actual.CreateCommand());
Assert.AreEqual(expected, actual.ToString());
```

## Integration with CI/CD

### Continuous Integration

The test project is designed to run in CI/CD pipelines:

```yaml
# Example GitHub Actions
- name: Run Tests
  run: dotnet test sqlbuilderlibrary/AMCode.Sql.Builder.UnitTests --verbosity normal

- name: Generate Coverage Report
  run: dotnet test --collect:"XPlat Code Coverage" --results-directory:"coverage"
```

## Related Documentation

- [AMCode.Sql.Builder README](../AMCode.Sql.Builder/README.md) - Main library documentation
- [AMCode.Common README](../../commonlibrary/AMCode.Common/README.md) - Common utilities used in tests

## Contributing

When adding new tests:

1. Follow the existing test structure and naming conventions
2. Use TestFixture for filter building when applicable
3. Use Moq for mocking dependencies
4. Include both positive and negative test cases
5. Test edge cases and boundary conditions
6. Ensure tests are independent and can run in any order

## Known Issues

None currently documented.

---

**See Also:**

- [Root README](../../../README.md) - Project overview
- [AMCode.Sql.Builder Documentation](../AMCode.Sql.Builder/README.md) - Library being tested

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

