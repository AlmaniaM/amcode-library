# AMCode.Common.Testing.UnitTests

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive unit tests for AMCode.Common.Testing library

---

## Overview

AMCode.Common.Testing.UnitTests is the test project for the AMCode.Common.Testing library. It provides comprehensive unit test coverage for all components in the AMCode.Common.Testing library, including Docker container management, CSV data reader functionality, and test helper utilities.

## Test Structure

The test project is organized by component:

```
AMCode.Common.Testing.UnitTests/
├── Components/
│   ├── Containers/              # Docker container tests
│   │   ├── DockerContainerTest.cs
│   │   ├── DockerImageTest.cs
│   │   ├── DockerVolumeTest.cs
│   │   ├── DbContainerTest.cs
│   │   ├── DbContainerTearDownTest.cs
│   │   └── Models/
│   │       ├── DbContainerHelpers.cs
│   │       └── TestContainerResources.cs
│   └── IO/                     # CSV data reader tests
│       ├── CSVDataReaderTest.cs
│       ├── MockFiles/          # Test CSV files
│       └── Models/
│           └── LocalTestHelpers.cs
├── Globals/
│   └── TestFixture.cs          # Global test fixture
└── AMCode.Common.Testing.UnitTests.csproj
```

## Test Categories

### Docker Container Tests

**Location:** `Components/Containers/`

Tests for Docker container management functionality:

- **DockerContainerTest**: Tests for Docker container lifecycle management
  - Container creation and startup
  - Container stopping and removal
  - Container status checking
  - Container parameter configuration

- **DockerImageTest**: Tests for Docker image operations
  - Image pulling and management
  - Image tagging and identification

- **DockerVolumeTest**: Tests for Docker volume management
  - Volume creation and cleanup
  - Volume mounting and access

- **DbContainerTest**: Tests for database container functionality
  - Database container setup
  - Database connection testing
  - Container resource management

- **DbContainerTearDownTest**: Tests for database container teardown
  - Proper cleanup procedures
  - Resource disposal

**Test Files:**
- `DockerContainerTest.cs` - Main Docker container tests
- `DockerImageTest.cs` - Docker image tests
- `DockerVolumeTest.cs` - Docker volume tests
- `DbContainerTest.cs` - Database container tests
- `DbContainerTearDownTest.cs` - Container teardown tests
- `Models/DbContainerHelpers.cs` - Database container helper utilities
- `Models/TestContainerResources.cs` - Test container resource models

**Dependencies:**
- `Docker.DotNet` (3.125.5) - Docker API client
- `Docker.DotNet.BasicAuth` (3.125.3) - Docker authentication
- `AMCode.Common.Testing` - Testing library being tested

### CSV Data Reader Tests

**Location:** `Components/IO/`

Tests for CSV data reader functionality:

- **CSVDataReaderTest**: Tests for CSV file reading
  - Header reading with various delimiters (comma, tab, pipe)
  - Data row reading and parsing
  - Handling headers with spaces
  - Handling empty values
  - IDataReader interface implementation

**Test Files:**
- `CSVDataReaderTest.cs` - Main CSV data reader tests
- `MockFiles/` - Test CSV files with various formats:
  - `CSVDataReaderTest_Comma_Delimited_Five_Rows.csv`
  - `CSVDataReaderTest_Tab_Delimited_Five_Rows.csv`
  - `CSVDataReaderTest_Pipe_Delimited_Five_Rows.csv`
  - `CSVDataReaderTest_Comma_Delimited_Five_Rows_With_Empty_Values.csv`
  - `CSVDataReaderTest_Five_Rows_No_Headers.csv`
  - `CSVDataReaderTest_Five_Rows_Space_In_Headers.csv`
- `Models/LocalTestHelpers.cs` - Test helper utilities for file path resolution

**Test Scenarios:**
- Reading headers with different delimiters
- Creating IDataReader instances
- Reading data rows
- Handling headers with spaces
- Handling empty values
- Reading files without headers

### Global Test Fixtures

**Location:** `Globals/`

- **TestFixture**: Global test fixture for test setup and teardown
  - One-time setup and teardown
  - Shared test resources
  - Test environment configuration

## Dependencies

### Internal Dependencies

- **AMCode.Common.Testing** - The library being tested
- **AMCode.Common** (1.0.0) - Common utilities

### External Dependencies

- **Docker.DotNet** (3.125.5) - Docker API client for container tests
- **Docker.DotNet.BasicAuth** (3.125.3) - Docker authentication
- **Newtonsoft.Json** (13.0.1) - JSON serialization
- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.2.1) - NUnit test adapter
- **Microsoft.NET.Test.Sdk** (17.0.0) - Test SDK

## Running Tests

### Prerequisites

- Docker must be installed and running
- .NET SDK 8.0 or later

### Run All Tests

```bash
dotnet test AMCode.Common.Testing.UnitTests.csproj
```

### Run Specific Test Category

```bash
# Run container tests only
dotnet test --filter "FullyQualifiedName~Containers"

# Run CSV tests only
dotnet test --filter "FullyQualifiedName~IO"
```

## Test Data

### Mock Files

Test CSV files are located in `Components/IO/MockFiles/` and are automatically copied to the output directory during build. These files test various CSV formats:

- Comma-delimited files
- Tab-delimited files
- Pipe-delimited files
- Files with empty values
- Files without headers
- Files with spaces in headers

## Test Coverage

### Container Tests

- ✅ Docker container lifecycle (create, start, stop, remove)
- ✅ Docker image operations
- ✅ Docker volume management
- ✅ Database container setup and teardown
- ✅ Container resource cleanup

### CSV Reader Tests

- ✅ Header reading with multiple delimiters
- ✅ Data row reading
- ✅ IDataReader interface implementation
- ✅ Handling special cases (spaces, empty values, no headers)

## Related Documentation

- [AMCode.Common.Testing README](../AMCode.Common.Testing/README.md) - Library being tested
- [AMCode.Common README](../../commonlibrary/AMCode.Common/README.md) - Common library
- [Root README](../../README.md) - Project overview

---

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

