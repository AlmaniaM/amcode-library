# AMCode.Data.SQLTests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** SQL-specific integration tests for AMCode.Data library, testing database operations with real Vertica, MongoDB, and ODBC connections

---

## Overview

AMCode.Data.SQLTests is a comprehensive test project focused on SQL and database integration testing for the AMCode.Data library. Unlike unit tests that use mocks, these tests execute against real database instances (Vertica, MongoDB, ODBC) to validate end-to-end functionality, performance, and security aspects of the data access layer.

This test project is essential for:
- Validating database connectivity and query execution
- Testing data provider implementations with real databases
- Performance testing under realistic conditions
- Security testing for database operations
- Integration testing across different database providers
- Validating query cancellation and connection management

## Architecture

The test project follows a structured approach with:
- **Docker-based Test Environment**: Uses Docker containers for isolated database testing
- **Test Fixtures**: NUnit test fixtures for setup and teardown
- **Provider Tests**: Separate test classes for each database provider
- **Mock Providers**: Utility classes for creating test doubles where appropriate
- **Environment Initialization**: Automated setup and teardown of test databases

### Key Components

- **TestingEnvironmentInit**: Docker container management for Vertica database
- **MoqProvider**: Utility for creating mock objects and test data providers
- **Provider Test Classes**: Tests for Vertica, MongoDB, and ODBC providers
- **Data Provider Tests**: Tests for generic and ExpandoObject data providers
- **Integration Tests**: End-to-end database operation tests

## Features

- **Real Database Testing**: Tests execute against actual database instances
- **Docker Integration**: Automated Docker container setup for Vertica testing
- **Multi-Provider Support**: Tests for Vertica, MongoDB, and ODBC providers
- **Performance Testing**: MongoDB performance benchmarks
- **Security Testing**: MongoDB security validation tests
- **Query Cancellation**: Tests for query cancellation functionality
- **Connection Management**: Tests for connection pooling and lifecycle
- **Data Mapping**: Tests for strongly-typed and dynamic object mapping

## Dependencies

### Internal Dependencies

- **AMCode.Data** - The data access library being tested
- **AMCode.Common.Testing** - Testing utilities and container management

### External Dependencies

- **Microsoft.NET.Test.Sdk** (17.1.0) - Test SDK
- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - Test adapter
- **Moq** (4.17.1) - Mocking framework
- **Docker.DotNet** (3.125.5) - Docker API client for container management
- **System.Configuration.ConfigurationManager** (6.0.0) - Configuration management
- **coverlet.collector** (3.1.2) - Code coverage collection

## Project Structure

```
AMCode.Data.SQLTests/
├── Components/
│   ├── Data/                        # Data provider tests
│   │   ├── DbBridgeTest.cs         # Database bridge tests
│   │   ├── DbExecuteTest.cs        # Execute operation tests
│   │   ├── GenericDataProviderTest.cs # Generic provider tests
│   │   ├── GenericDataProviderWithColumnsTest.cs
│   │   ├── ExpandoObjectDataProviderTest.cs
│   │   ├── ExpandoObjectDataProviderWithColumnsTest.cs
│   │   └── Models/                 # Test models
│   ├── MongoDB/                     # MongoDB integration tests
│   │   ├── MongoIntegrationTest.cs # MongoDB integration tests
│   │   ├── MongoPerformanceTest.cs # Performance benchmarks
│   │   ├── MongoSecurityTest.cs    # Security tests
│   │   └── MongoTestingEnvironmentInit.cs
│   ├── Odbc/                        # ODBC provider tests
│   │   └── OdbcDataReaderProviderFactoryTest.cs
│   └── Vertica/                      # Vertica provider tests
│       ├── VerticaDataReaderProviderFactoryTest.cs
│       └── VerticaQueryCancellationTest.cs
├── Globals/
│   └── MoqProvider.cs              # Mock object provider utilities
├── Testing/
│   └── ContainerMocks.cs           # Container mock utilities
├── TestingEnvironmentInit.cs       # Docker container setup/teardown
└── AMCode.Data.SQLTests.csproj      # Project file
```

## Test Categories

### Data Provider Tests

**Location:** `Components/Data/`

Tests for core data provider functionality:

- **DbBridgeTest**: Tests database bridge operations (connect, query, execute)
- **DbExecuteTest**: Tests non-query database operations
- **GenericDataProviderTest**: Tests strongly-typed data retrieval
- **GenericDataProviderWithColumnsTest**: Tests column mapping functionality
- **ExpandoObjectDataProviderTest**: Tests dynamic object data retrieval
- **ExpandoObjectDataProviderWithColumnsTest**: Tests dynamic object column mapping

**Key Test Scenarios:**
- Query execution and result retrieval
- Multiple queries on single connection
- Custom connection handling
- Data type mapping
- Column name to property mapping
- Null value handling

### MongoDB Tests

**Location:** `Components/MongoDB/`

Integration tests for MongoDB data provider:

- **MongoIntegrationTest**: End-to-end MongoDB operations
- **MongoPerformanceTest**: Performance benchmarks and optimization
- **MongoSecurityTest**: Security validation and access control
- **MongoTestingEnvironmentInit**: MongoDB test environment setup

**Key Test Scenarios:**
- Connection management
- CRUD operations
- Query execution
- Performance metrics
- Security validation
- Error handling

### Vertica Tests

**Location:** `Components/Vertica/`

Tests for Vertica database provider:

- **VerticaDataReaderProviderFactoryTest**: Factory and connection tests
- **VerticaQueryCancellationTest**: Query cancellation functionality

**Key Test Scenarios:**
- Connection creation and management
- Query cancellation
- Connection string handling
- Provider factory functionality

### ODBC Tests

**Location:** `Components/Odbc/`

Tests for ODBC database provider:

- **OdbcDataReaderProviderFactoryTest**: ODBC provider factory tests

**Key Test Scenarios:**
- ODBC connection creation
- Provider factory functionality
- Cross-platform compatibility

## Running Tests

### Prerequisites

1. **Docker**: Required for Vertica database container
2. **Environment Variables**:
   - `TEST_CONTAINER_VERTICA_CONNECTION_STRING` - Vertica connection string
   - `AMCODE_TEST_DOCKER_USERNAME` - Docker registry username
   - `AMCODE_TEST_DOCKER_PAT` - Docker registry password/token
3. **Database Access**: MongoDB instance (if testing MongoDB)

### Running All Tests

```bash
# Run all SQL tests
dotnet test AMCode.Data.SQLTests.csproj

# Run with detailed output
dotnet test AMCode.Data.SQLTests.csproj --verbosity detailed

# Run specific test class
dotnet test AMCode.Data.SQLTests.csproj --filter "FullyQualifiedName~DbBridgeTest"
```

### Running Specific Test Categories

```bash
# Run only Vertica tests
dotnet test --filter "FullyQualifiedName~Vertica"

# Run only MongoDB tests
dotnet test --filter "FullyQualifiedName~MongoDB"

# Run only data provider tests
dotnet test --filter "FullyQualifiedName~Data"
```

### Docker Container Setup

The test project automatically manages Docker containers for Vertica testing:

- **Container Name**: `dl-data-library-vertica`
- **Image**: `demandlink/dl-data-library-vertica:1.0.0`
- **Port**: 5433
- **Database**: `demandlink`

The `TestingEnvironmentInit` class handles:
- Container startup and initialization
- Waiting for database readiness
- Container teardown after tests

## Test Utilities

### MoqProvider

**Location:** `Globals/MoqProvider.cs`

Utility class for creating mock objects and test data providers:

**Key Methods:**
- `CreateDbBridge()` - Create database bridge instance
- `CreateDbConnection()` - Create database connection
- `CreateDbExecute()` - Create execute operation handler
- `CreateGenericDataProvider()` - Create generic data provider
- `CreateExpandoObjectDataProvider()` - Create dynamic data provider
- `CreateDataReaderProviderFactoryMoq()` - Create mock factory

**Usage Example:**
```csharp
using AMCode.Data.SQLTests.Globals;

var dataProvider = MoqProvider.CreateGenericDataProvider();
var results = await dataProvider.GetListOfAsync<MyModel>("SELECT * FROM table");
```

### TestingEnvironmentInit

**Location:** `TestingEnvironmentInit.cs`

NUnit `[SetUpFixture]` that manages Docker container lifecycle:

- **OneTimeSetUp**: Starts Vertica Docker container
- **OneTimeTearDown**: Stops and removes container
- Waits for database readiness before tests run

## Test Data

### Static Test Data

The tests use static test data tables in the Vertica database:

- **AMCode.Data.tbl_DataProvider_Static_Data**: Static test data table
- Tables are created and dropped during test execution
- Unique table names using GUIDs to avoid conflicts

### Mock Data

**Location:** `Components/Data/MockFiles/ExpectedMockData.cs`

Predefined mock data for testing data mapping and transformation.

## Configuration

### Connection Strings

Connection strings are configured via:

1. **Environment Variables**: `TEST_CONTAINER_VERTICA_CONNECTION_STRING`
2. **App.config**: For Vertica connection string (legacy)
3. **MongoDB**: Configured in `MongoTestingEnvironmentInit`

### Docker Configuration

Docker container configuration in `TestingEnvironmentInit`:

```csharp
var runSetupParams = new ContainerSetupParams
{
    ContainerName = "dl-data-library-vertica",
    ImageName = "demandlink/dl-data-library-vertica",
    ImageTag = "1.0.0",
    PortMappings = new Dictionary<string, string> { ["5433"] = "5433/tcp" }
};
```

## Test Coverage

### Covered Functionality

- ✅ Database connection management
- ✅ Query execution (SELECT)
- ✅ Non-query execution (INSERT, UPDATE, DELETE, DDL)
- ✅ Strongly-typed data mapping
- ✅ Dynamic object data mapping
- ✅ Column mapping and transformation
- ✅ Query cancellation
- ✅ Connection pooling
- ✅ Multi-query execution
- ✅ Error handling
- ✅ MongoDB integration
- ✅ Vertica integration
- ✅ ODBC integration

### Performance Testing

MongoDB performance tests measure:
- Query execution time
- Connection overhead
- Data retrieval performance
- Bulk operation performance

## Known Limitations

- **Docker Dependency**: Requires Docker for Vertica tests
- **Environment Setup**: Requires environment variables for Docker registry
- **Database Availability**: Tests require accessible database instances
- **Platform Differences**: Some tests may behave differently on Windows vs Linux

## Troubleshooting

### Docker Container Issues

If Docker container fails to start:
1. Verify Docker is running
2. Check Docker registry credentials
3. Verify image exists: `demandlink/dl-data-library-vertica:1.0.0`
4. Check port 5433 is available

### Connection String Issues

If connection tests fail:
1. Verify `TEST_CONTAINER_VERTICA_CONNECTION_STRING` is set
2. Check connection string format
3. Verify database is accessible
4. Check firewall/network settings

### Test Failures

Common causes:
1. Database not ready (wait for container initialization)
2. Test data not available
3. Connection pool exhaustion
4. Timeout issues

## Related Projects

- [AMCode.Data](../AMCode.Data/README.md) - The data access library being tested
- [AMCode.Data.UnitTests](../AMCode.Data.UnitTests/README.md) - Unit tests for AMCode.Data
- [AMCode.Common.Testing](../../commontestinglibrary/AMCode.Common.Testing/README.md) - Testing utilities

## Contributing

When adding new tests:

1. Follow existing test structure and naming conventions
2. Use `MoqProvider` for creating test objects
3. Clean up test data after tests complete
4. Add appropriate test categories and descriptions
5. Update this README with new test scenarios

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [AMCode.Data Documentation](../AMCode.Data/README.md) - Data library documentation

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

