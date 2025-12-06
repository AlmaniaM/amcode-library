# AMCode.Storage.UnitTests

**Version:** 1.0
**Target Framework:** .NET 8.0
**Last Updated:** 2025-01-27
**Purpose:** Comprehensive unit tests for the AMCode.Storage library covering all storage providers (Azure Blob, S3, Local, In-Memory) and logging infrastructure

---

## Overview

AMCode.Storage.UnitTests provides comprehensive test coverage for the AMCode.Storage library. The test suite validates functionality across all storage providers including Azure Blob Storage, AWS S3, Local file storage, and In-Memory storage. It also includes extensive tests for the logging infrastructure and stream data source implementations.

## Test Coverage

### Storage Provider Tests

- **Azure Blob Storage**: Tests for Azure Blob Storage operations using Azurite (local Azure Storage emulator)
- **AWS S3 Storage**: Tests for S3 storage operations
- **Local Storage**: Tests for local file system storage operations
- **In-Memory Storage**: Tests for in-memory stream storage

### Component Tests

- **Stream Data Sources**: Tests for Azure Blob, Local, and In-Memory stream data sources
- **Logging Infrastructure**: Tests for storage logging, metrics logging, and operation logging
- **Storage Operations**: Create, read, update, delete operations for all providers

## Architecture

The test project follows NUnit testing patterns with:

- **Test Fixtures**: Organized by component (Storage, Logging, etc.)
- **Test Helpers**: Reusable test utilities and helpers
- **Mock Data**: JSON mock files for test data
- **Docker Integration**: Azurite container for Azure Blob Storage testing
- **Test Isolation**: Each test fixture manages its own setup and teardown

### Test Organization

```
AMCode.Storage.UnitTests/
├── Components/
│   ├── Storage/                    # Storage provider tests
│   │   ├── AzureBlobStorageTests.cs
│   │   └── S3StorageTests.cs
│   ├── AzureBlob/                 # Azure Blob specific tests
│   │   ├── AzureBlobStorageTest.cs
│   │   ├── AzureBlobStreamDataSourceTests.cs
│   │   └── Mocks/                 # Mock JSON files
│   ├── Local/                      # Local storage tests
│   │   ├── SimpleLocalStorageTest.cs
│   │   ├── FileStreamDataSourceTest.cs
│   │   └── Mocks/                 # Mock JSON files
│   └── InMemory/                   # In-memory storage tests
│       ├── StreamStorageTest.cs
│       ├── StreamDataSourceTest.cs
│       └── Mocks/                 # Mock JSON files
├── Logging/                        # Logging infrastructure tests
│   ├── StorageLoggerTests.cs
│   ├── StorageMetricsLoggerTests.cs
│   ├── StorageOperationLoggerTests.cs
│   └── LoggingTestBase.cs
├── Globals/                        # Global test utilities
│   ├── TestHelper.cs
│   └── TestResources.cs
├── Mocks/                          # Shared mock utilities
│   └── TestingContainersMock.cs
└── TestingEnvironmentInit.cs      # Docker environment setup
```

## Dependencies

### Internal Dependencies

- **AMCode.Storage** - The library being tested
- **AMCode.Common** (1.0.0) - Common utilities

### External Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.1.0) - Test adapter for Visual Studio
- **Microsoft.NET.Test.Sdk** (17.0.0) - Test SDK
- **Azure.Storage.Blobs** (12.10.0) - Azure Blob Storage SDK
- **AWSSDK.S3** (3.7.400.50) - AWS S3 SDK
- **coverlet.collector** (3.1.0) - Code coverage collection

## Test Fixtures

### AzureBlobStorageTests

**Location:** `Components/Storage/AzureBlobStorageTests.cs`

**Purpose:** Tests for Azure Blob Storage provider operations.

**Test Coverage:**

- Constructor validation
- File creation operations
- File download operations
- File deletion operations
- Error handling
- Connection string validation

**Dependencies:** Requires Azurite Docker container (automatically started)

### S3StorageTests

**Location:** `Components/Storage/S3StorageTests.cs`

**Purpose:** Tests for AWS S3 storage provider operations.

**Test Coverage:**

- Constructor validation
- S3 client initialization
- Storage operations
- Error handling
- Credential validation

### SimpleLocalStorageTest

**Location:** `Components/Local/SimpleLocalStorageTest.cs`

**Purpose:** Tests for local file system storage operations.

**Test Coverage:**

- File save operations
- File download operations
- File deletion operations
- Directory management
- File path handling

**Test Data:** Uses mock JSON files from `Mocks/` directory

### StreamStorageTest

**Location:** `Components/InMemory/StreamStorageTest.cs`

**Purpose:** Tests for in-memory stream storage operations.

**Test Coverage:**

- Stream save operations
- Stream retrieval operations
- Stream deletion operations
- Dictionary-based storage management

### AzureBlobStreamDataSourceTests

**Location:** `Components/AzureBlob/AzureBlobStreamDataSourceTests.cs`

**Purpose:** Tests for Azure Blob stream data source operations.

**Test Coverage:**

- Stream data source creation
- Stream operations
- Azure Blob integration

### FileStreamDataSourceTest

**Location:** `Components/Local/FileStreamDataSourceTest.cs`

**Purpose:** Tests for local file stream data source operations.

**Test Coverage:**

- File stream operations
- Local file system integration
- Stream management

### StorageLoggerTests

**Location:** `Logging/StorageLoggerTests.cs`

**Purpose:** Tests for storage logging infrastructure.

**Test Coverage:**

- Information logging
- Warning logging
- Error logging
- Log message formatting
- Log level handling

### StorageMetricsLoggerTests

**Location:** `Logging/StorageMetricsLoggerTests.cs`

**Purpose:** Tests for storage metrics logging.

**Test Coverage:**

- Metrics collection
- Performance metrics
- Operation metrics
- Metrics aggregation

### StorageOperationLoggerTests

**Location:** `Logging/StorageOperationLoggerTests.cs`

**Purpose:** Tests for storage operation logging.

**Test Coverage:**

- Operation start/end logging
- Operation duration tracking
- Operation result logging
- Error operation logging

## Test Utilities

### TestHelper

**Location:** `Globals/TestHelper.cs`

**Purpose:** Helper class for managing test directories and mock files.

**Key Methods:**

- `GetMockFilesDirectoryPath(TestContext)` - Get path to mock files directory
- `GetTestDirectoryPath(TestContext)` - Get test directory path
- `GetTestWorkDirectoryPath(TestContext)` - Get test work directory path
- `GetMockFilePath(TestContext, string)` - Get path to specific mock file

### LocalTestHelper

**Location:** `Components/{Provider}/Models/LocalTestHelper.cs`

**Purpose:** Provider-specific test helpers for managing test environments.

**Usage:**
```csharp
var testHelper = new LocalTestHelper();
var mockFilePath = testHelper.GetMockFilePath(TestContext.CurrentContext, "test-file.json");
var workDirectory = testHelper.GetTestWorkDirectoryPath(TestContext.CurrentContext);
```

### Mock Storage Loggers

**Location:** `Logging/MockStorageLogger.cs`, `MockStorageMetricsLogger.cs`, `MockStorageOperationLogger.cs`

**Purpose:** Mock implementations of storage loggers for testing.

**Usage:**
```csharp
var mockLogger = new MockStorageLogger();
var storage = new AzureBlobStorage(connectionString, containerName, mockLogger);
// Verify logs after operations
Assert.That(mockLogger.LogEntries.Count, Is.GreaterThan(0));
```

## Docker Integration

### Azurite Container

The test suite uses Azurite (Azure Storage emulator) for testing Azure Blob Storage operations without requiring actual Azure credentials.

**Setup:** `TestingEnvironmentInit.cs` automatically:
- Starts Azurite Docker container before tests
- Configures container with proper ports and volumes
- Stops and removes container after tests

**Container Details:**
- **Image:** `mcr.microsoft.com/azure-storage/azurite:latest`
- **Container Name:** `dl-storage-library-azurite`
- **Port:** `10000` (blob service)
- **Volume:** `amcode-volume-dl-storage-library`

**Prerequisites:**
- Docker must be installed and running
- Docker client must be accessible

## Running Tests

### Run All Tests

```bash
# Run all tests
dotnet test AMCode.Storage.UnitTests.csproj

# Run with coverage
dotnet test AMCode.Storage.UnitTests.csproj --collect:"XPlat Code Coverage"
```

### Run Specific Test Fixtures

```bash
# Run Azure Blob Storage tests
dotnet test --filter "FullyQualifiedName~AzureBlobStorageTests"

# Run Local Storage tests
dotnet test --filter "FullyQualifiedName~SimpleLocalStorageTest"

# Run Logging tests
dotnet test --filter "FullyQualifiedName~StorageLoggerTests"
```

### Run Tests in Visual Studio

1. Open Test Explorer
2. Build the solution
3. Select tests to run
4. Click "Run All" or run specific tests

## Test Data

### Mock Files

The test project includes mock JSON files for testing:

- **Azure Blob Mocks:** `Components/AzureBlob/Mocks/`
  - `test-blob-save.json`
  - `test-blob-download.json`
  - `test-blob-delete.json`

- **Local Storage Mocks:** `Components/Local/Mocks/`
  - `test-local-save.json`
  - `test-local-download.json`
  - `test-local-delete.json`

- **In-Memory Mocks:** `Components/InMemory/Mocks/`
  - `test-in-memory-save.json`
  - `test-in-memory-download.json`
  - `test-in-memory-delete.json`

### Test Work Directories

Each provider test creates temporary work directories:

- `Components/Local/TestWorkDirectory/`
- `Components/InMemory/TestWorkDirectory/`

These directories are cleaned up in `TearDown` methods.

## Test Patterns

### Setup and Teardown

```csharp
[SetUp]
public void SetUp()
{
    // Initialize test dependencies
    _mockLogger = new MockStorageLogger();
    _storage = new AzureBlobStorage(connectionString, containerName, _mockLogger);
}

[TearDown]
public void TearDown()
{
    // Clean up test resources
    _storage?.Dispose();
}
```

### Async Test Pattern

```csharp
[Test]
public async Task ShouldSaveFile()
{
    // Arrange
    var fileName = "test.txt";
    var content = "test content";
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

    // Act
    var result = await _storage.CreateFileAsync(fileName, stream);

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.IsSuccess, Is.True);
}
```

### Mock Logger Verification

```csharp
[Test]
public async Task ShouldLogOperation()
{
    // Act
    await _storage.CreateFileAsync("test.txt", stream);

    // Assert
    Assert.That(_mockLogger.LogEntries.Count, Is.GreaterThan(0));
    Assert.That(_mockLogger.LogEntries[0].Message, Contains.Substring("CreateFile"));
}
```

## Code Coverage

The test project includes code coverage collection using Coverlet:

```bash
# Generate coverage report
dotnet test --collect:"XPlat Code Coverage"

# View coverage report
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage" -reporttypes:Html
```

## Continuous Integration

### CI/CD Requirements

- **Docker**: Must be available for Azurite container
- **.NET SDK**: .NET 8.0 SDK required
- **Test SDK**: Microsoft.NET.Test.Sdk included

### CI Configuration Example

```yaml
- name: Run Tests
  run: |
    docker run -d -p 10000:10000 --name azurite mcr.microsoft.com/azure-storage/azurite
    dotnet test --collect:"XPlat Code Coverage"
```

## Troubleshooting

### Docker Issues

**Problem:** Tests fail with Docker connection errors

**Solution:**
- Ensure Docker is installed and running
- Verify Docker daemon is accessible
- Check container name conflicts

### Azurite Connection Issues

**Problem:** Azure Blob Storage tests fail to connect to Azurite

**Solution:**
- Verify Azurite container is running: `docker ps`
- Check port 10000 is not in use
- Review `TestingEnvironmentInit.cs` for container configuration

### Test Data Issues

**Problem:** Tests fail due to missing mock files

**Solution:**
- Ensure mock JSON files are copied to output directory
- Check `CopyToOutputDirectory` settings in `.csproj`
- Verify file paths in test helpers

## Related Documentation

- [AMCode.Storage](../AMCode.Storage/README.md) - Main storage library documentation
- [AMCode.Common.UnitTests](../../commonlibrary/AMCode.Common.UnitTests/README.md) - Common testing utilities

## Contributing

When adding new tests:

1. Follow existing test patterns and naming conventions
2. Use appropriate test helpers and utilities
3. Include proper setup and teardown
4. Add mock data files if needed
5. Update this README with new test fixtures

---

**See Also:**

- [Root README](../../../README.md) - Project overview
- [Documentation Plan](../../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27
**Maintained By:** Development Team

