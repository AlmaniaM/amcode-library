# AMCode.Data.UnitTests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive unit tests for AMCode.Data library

---

## Overview

AMCode.Data.UnitTests is the test project for the AMCode.Data library. It provides comprehensive unit test coverage for all components in the AMCode.Data library, including data providers (generic and ExpandoObject), MongoDB integration, ODBC support, Vertica integration, extension methods, and logging infrastructure.

## Test Structure

The test project mirrors the structure of the AMCode.Data library, with test classes organized by component:

```
AMCode.Data.UnitTests/
├── Components/
│   ├── Data/                  # Core data provider tests
│   ├── Extensions/            # Extension method tests
│   ├── MongoDB/               # MongoDB provider tests
│   ├── Odbc/                  # ODBC provider tests
│   └── Vertica/               # Vertica provider tests
├── Logging/                   # Logging infrastructure tests
└── AMCode.Data.UnitTests.csproj
```

## Test Categories

### Data Provider Tests

**Location:** `Components/Data/`

Tests for core data provider functionality:

- **DataProviderTest**: Main data provider tests
  - Provider initialization
  - Factory integration
  - Provider coordination
  - Error handling

- **GenericDataProviderTest**: Strongly-typed data provider tests
  - Type mapping
  - Object creation
  - Property mapping
  - Type conversion

- **GenericDataProviderFactoryTest**: Generic provider factory tests
  - Factory creation
  - Provider instantiation
  - Configuration handling

- **ExpandoObjectDataProviderTest**: Dynamic object data provider tests
  - ExpandoObject creation
  - Dynamic property access
  - Dynamic type handling

- **ExpandoObjectDataProviderFactoryTest**: ExpandoObject provider factory tests
  - Factory creation
  - Dynamic provider instantiation

- **DataProviderFactoryTest**: Main data provider factory tests
  - Factory pattern implementation
  - Provider creation
  - Factory configuration

- **DbBridgeTest**: Database bridge tests
  - Connection management
  - Command creation
  - Bridge abstraction

**Test Files:**
- `DataProviderTest.cs` - Main data provider tests
- `GenericDataProviderTest.cs` - Generic provider tests
- `GenericDataProviderFactoryTest.cs` - Generic factory tests
- `ExpandoObjectDataProviderTest.cs` - ExpandoObject provider tests
- `ExpandoObjectDataProviderFactoryTest.cs` - ExpandoObject factory tests
- `DataProviderFactoryTest.cs` - Data provider factory tests
- `DbBridgeTest.cs` - Database bridge tests
- `Models/DataProviderTestMock.cs` - Mock classes for testing

### Extension Method Tests

**Location:** `Components/Extensions/`

Tests for data reader and value parsing extensions:

- **DataReaderExtensionsTest**: Data reader extension method tests
  - Data reader to ExpandoObject conversion
  - Column mapping
  - Type conversion
  - Property mapping
  - Multiple row handling
  - Delimiter handling (comma, pipe)
  - Header handling (with/without spaces)

- **ValueParserTest**: Value parsing utility tests
  - Type parsing
  - Value conversion
  - Null handling
  - Edge case handling

**Test Files:**
- `DataReaderExtensionsTest.cs` - Data reader extension tests
- `ValueParserTest.cs` - Value parser tests
- `MockFiles/` - CSV test files (6+ test CSV files)
- `Models/` - Test models and helpers
  - `ColumnNameWithProperty.cs` - Column name with property mapping
  - `ExtensionsTestMocks.cs` - Mock classes
  - `LocalTestHelpers.cs` - Test helper utilities

### MongoDB Tests

**Location:** `Components/MongoDB/`

Comprehensive tests for MongoDB integration:

- **MongoDataProviderTest**: MongoDB data provider tests
  - Document operations
  - Query execution
  - Collection operations
  - MongoDB-specific functionality

- **MongoGenericDataProviderTest**: MongoDB generic provider tests
  - Strongly-typed MongoDB operations
  - Type mapping for MongoDB documents
  - Object serialization/deserialization

- **MongoExpandoObjectDataProviderTest**: MongoDB ExpandoObject provider tests
  - Dynamic object operations with MongoDB
  - ExpandoObject document handling
  - Dynamic property access

- **MongoExecuteTest**: MongoDB execute operation tests
  - Insert, update, delete operations
  - Bulk operations
  - Transaction support

**Test Infrastructure:**
- **MongoTestBase**: Abstract base class for MongoDB tests
  - Common mock setup
  - MongoDB client mocking
  - Database and collection mocking
  - Session management mocking
  - Health monitor mocking

**Test Files:**
- `MongoDataProviderTest.cs` - MongoDB data provider tests
- `MongoGenericDataProviderTest.cs` - MongoDB generic provider tests
- `MongoExpandoObjectDataProviderTest.cs` - MongoDB ExpandoObject tests
- `MongoExecuteTest.cs` - MongoDB execute tests
- `MongoTestBase.cs` - MongoDB test base class
- `Models/MongoTestModels.cs` - MongoDB test models

### ODBC Tests

**Location:** `Components/Odbc/`

Tests for ODBC database provider support:

- **OdbcConnectionFactoryTest**: ODBC connection factory tests
  - Connection factory creation
  - Connection string handling
  - ODBC-specific configuration

- **OdbcCommandFactoryTest**: ODBC command factory tests
  - Command factory creation
  - Command instantiation
  - ODBC command configuration

**Test Files:**
- `OdbcConnectionFactoryTest.cs` - ODBC connection factory tests
- `OdbcCommandFactoryTest.cs` - ODBC command factory tests

### Vertica Tests

**Location:** `Components/Vertica/`

Tests for Vertica database provider support:

- **VerticaConnectionFactoryTest**: Vertica connection factory tests
  - Connection factory creation
  - Vertica connection handling
  - Connection string configuration

- **VerticaCommandFactoryTest**: Vertica command factory tests
  - Command factory creation
  - Vertica command instantiation
  - Command configuration

**Test Files:**
- `VerticaConnectionFactoryTest.cs` - Vertica connection factory tests
- `VerticaCommandFactoryTest.cs` - Vertica command factory tests

### Logging Tests

**Location:** `Logging/`

Tests for logging infrastructure:

- **DataProviderLoggingTest**: Data provider logging tests
  - Logging integration
  - Log message generation
  - Log level handling
  - Structured logging

- **ConsoleLoggerTest**: Console logger tests
  - Console output
  - Log formatting
  - Console logger configuration

- **NoOpLoggerTest**: No-op logger tests
  - Logger interface compliance
  - Null logger behavior
  - Performance testing

**Test Files:**
- `DataProviderLoggingTest.cs` - Data provider logging tests
- `ConsoleLoggerTest.cs` - Console logger tests
- `NoOpLoggerTest.cs` - No-op logger tests

## Test Infrastructure

### MongoDB Test Base

**Location:** `Components/MongoDB/MongoTestBase.cs`

Abstract base class providing common MongoDB test infrastructure:

- **SetupMocks**: Sets up common MongoDB mocks (client, database, collection, session manager, health monitor)
- **MockMongoClient**: Mocked MongoDB client
- **MockMongoDatabase**: Mocked MongoDB database
- **MockMongoCollection**: Mocked MongoDB collection
- **MockSessionManager**: Mocked session manager
- **MockHealthMonitor**: Mocked health monitor
- **MockLogger**: Mocked logger

### Test Helpers

Extension test helpers:

- **LocalTestHelpers**: `Components/Extensions/Models/LocalTestHelpers.cs`
  - CSV file reading utilities
  - Test data preparation
  - Helper methods for extension tests

## Test Data

### Mock Files

The test project includes CSV mock files for extension tests:

- **DataReaderExtensionsTest CSV Files**: 6+ CSV files with different formats
  - Various delimiters (comma, pipe)
  - Different header formats (with/without spaces)
  - Multiple row scenarios
  - Type value testing
  - Object value testing

### Test Models

- **MongoTestModels**: MongoDB-specific test models
- **ExtensionsTestMocks**: Extension test mock classes
- **DataProviderTestMock**: Data provider test mocks
- **ColumnNameWithProperty**: Column mapping test model

## Dependencies

### Internal Dependencies

- **AMCode.Data**: The library being tested (project reference)
- **AMCode.Common.UnitTests**: Common test utilities (project reference)

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Visual Studio test adapter
- **Microsoft.NET.Test.Sdk** (17.1.0) - Test SDK
- **Moq** (4.18.2) - Mocking framework
- **coverlet.collector** (3.1.2) - Code coverage collection

## Running Tests

### Command Line

```bash
# Run all tests
dotnet test AMCode.Data.UnitTests.csproj

# Run tests with verbose output
dotnet test AMCode.Data.UnitTests.csproj --verbosity normal

# Run specific test class
dotnet test AMCode.Data.UnitTests.csproj --filter "FullyQualifiedName~DataProviderTest"

# Run MongoDB tests only
dotnet test AMCode.Data.UnitTests.csproj --filter "FullyQualifiedName~Mongo"

# Run tests with code coverage
dotnet test AMCode.Data.UnitTests.csproj --collect:"XPlat Code Coverage"
```

### Visual Studio

1. Open the solution in Visual Studio
2. Open Test Explorer (Test → Test Explorer)
3. Build the solution
4. Run all tests or select specific test categories

### Test Explorer

Tests are organized by namespace and component:

- `AMCode.Data.UnitTests.Data.*` - Data provider tests
- `AMCode.Data.UnitTests.Extensions.*` - Extension method tests
- `AMCode.Data.UnitTests.Components.MongoDB.*` - MongoDB tests
- `AMCode.Data.UnitTests.Components.Odbc.*` - ODBC tests
- `AMCode.Data.UnitTests.Components.Vertica.*` - Vertica tests
- `AMCode.Data.UnitTests.Logging.*` - Logging tests

## Test Coverage

The test project provides comprehensive coverage for:

- ✅ All data provider types (Generic, ExpandoObject, Main DataProvider)
- ✅ All provider factories
- ✅ MongoDB integration (all operations)
- ✅ ODBC provider support
- ✅ Vertica provider support
- ✅ Extension methods (DataReader, ValueParser)
- ✅ Logging infrastructure
- ✅ Database bridge abstraction
- ✅ Error handling and edge cases
- ✅ Async/await patterns
- ✅ Cancellation token support

## Test Patterns

### Common Test Patterns

1. **Mock-Based Testing**: Extensive use of Moq for dependency mocking
2. **Factory Pattern Testing**: Testing factory creation and provider instantiation
3. **Provider Abstraction**: Testing provider interfaces and implementations
4. **Async Testing**: Comprehensive async/await test coverage
5. **Integration-Style Unit Tests**: MongoDB tests with mocked clients
6. **Extension Method Testing**: Testing extension methods with various data formats

### Example Test Structure

```csharp
[TestFixture]
public class DataProviderTest
{
    private IDataProvider dataProvider;
    private Mock<IDbBridge> dbBridgeMoq;
    private Mock<IDbExecuteFactory> dbExecuteFactoryMoq;

    [SetUp]
    public void SetUp()
    {
        // Arrange: Set up mocks
        dbBridgeMoq = new Mock<IDbBridge>();
        dbExecuteFactoryMoq = new Mock<IDbExecuteFactory>();
        // ... setup factories
        
        // Create provider with mocked dependencies
        dataProvider = new DataProvider(
            dbExecuteFactoryMoq.Object,
            expandoObjectDataProviderFactoryMoq.Object,
            genericDataProviderFactoryMoq.Object
        );
    }

    [Test]
    public async Task ShouldExecuteQuery()
    {
        // Arrange
        var query = "SELECT * FROM table";
        
        // Act
        var result = await dataProvider.ExecuteAsync(query);
        
        // Assert
        Assert.IsNotNull(result);
    }
}
```

### MongoDB Test Pattern

```csharp
public class MongoDataProviderTest : MongoTestBase
{
    [SetUp]
    public override void SetupMocks()
    {
        base.SetupMocks();
        // Additional MongoDB-specific setup
    }

    [Test]
    public async Task ShouldQueryMongoCollection()
    {
        // Arrange: Use base class mocks
        var provider = new MongoDataProvider(
            MockConnectionFactory.Object,
            MockSessionManager.Object,
            MockLogger.Object
        );
        
        // Act & Assert
        // Test MongoDB operations
    }
}
```

## Continuous Integration

Tests are designed to run in CI/CD pipelines:

- **No External Dependencies**: Tests use mocks, no actual database connections required
- **Isolated Tests**: Each test is independent and can run in parallel
- **Mock Data**: All test data is included in the project
- **Fast Execution**: Tests are optimized for speed with mocked dependencies
- **Code Coverage**: Coverlet integration for coverage reporting

## Troubleshooting

### Common Issues

1. **Mock Files Not Found**: Ensure CSV mock files are copied to output directory (configured in `.csproj`)
2. **MongoDB Mock Issues**: Use `MongoTestBase` for proper MongoDB mock setup
3. **Factory Creation Errors**: Verify all factory dependencies are properly mocked
4. **Async Test Failures**: Ensure proper async/await usage and cancellation token handling

### Debugging Tests

1. Set breakpoints in test methods or code under test
2. Use Test Explorer to debug specific tests
3. Check test output for detailed error messages
4. Review mock setup if tests fail unexpectedly
5. Verify CSV mock file contents for extension tests

## Related Documentation

- [AMCode.Data README](../AMCode.Data/README.md) - Library being tested
- [AMCode.Common.UnitTests README](../../commonlibrary/AMCode.Common.UnitTests/README.md) - Common test utilities
- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

## Contributing

When adding new tests:

1. Follow existing test patterns and structure
2. Use appropriate test base classes (e.g., `MongoTestBase` for MongoDB tests)
3. Include mock files for complex test scenarios
4. Ensure tests are isolated and can run independently
5. Add test data to appropriate MockFiles directories
6. Update this README if adding new test categories
7. Use Moq for dependency mocking
8. Follow async/await patterns for async operations

---

**See Also:**

- [AMCode.Data Library](../AMCode.Data/README.md) - Library documentation
- [Root README](../../README.md) - Project overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

