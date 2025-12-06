# AMCode.Exports.SharedTestLibrary

**Version:** 1.0.0  
**Target Framework:** .NET 9.0  
**Last Updated:** 2025-01-27  
**Purpose:** Shared test library for AMCode.Exports integration and unit tests

---

## Overview

AMCode.Exports.SharedTestLibrary is a shared test library that provides common test utilities, helpers, and base classes for both AMCode.Exports.UnitTests and AMCode.Exports.IntegrationTests projects. It contains reusable test infrastructure to avoid code duplication across test projects.

## Purpose

This library serves as a shared foundation for testing the AMCode.Exports library by providing:

- **Test Helper Base Classes**: Abstract base classes for test setup and teardown
- **Common Test Models**: Shared models and utilities used across tests
- **File Storage Helpers**: Utilities for managing test file storage and cleanup
- **Stream Extensions**: Test-specific stream manipulation utilities

## Project Structure

```
AMCode.Exports.SharedTestLibrary/
├── Global/
│   ├── TestHelperBase.cs              # Abstract base class for test helpers
│   └── ExportBookResultFileStorage.cs # File storage helper for export results
├── Components/
│   ├── Common/
│   │   └── Models/
│   │       └── TestHelper.cs          # Common test helper utilities
│   └── Extensions/
│       └── StreamExtensions.cs        # Stream extension methods for testing
└── AMCode.Exports.SharedTestLibrary.csproj
```

## Key Components

### TestHelperBase

**Location:** `Global/TestHelperBase.cs`

**Purpose:** Abstract base class providing common test helper functionality for file path resolution and test directory management.

**Key Methods:**

- `GetMockFilesPath(TestContext)` - Get path to mock files directory
- `GetTestDirectoryPath(TestContext)` - Abstract method to get test directory path
- `GetTestWorkDirectoryPath(TestContext)` - Get path to test work directory
- `GetFilePath(string, TestContext)` - Get absolute path to a mock file

**Usage:**

```csharp
public class MyTestHelper : TestHelperBase
{
    public override string GetTestDirectoryPath(TestContext testContext)
    {
        // Return test directory path
        return Path.Combine(testContext.TestDirectory, "MyTest");
    }
    
    public void TestMethod()
    {
        var mockFilePath = GetFilePath("test-data.csv", TestContext.CurrentContext);
        // Use mock file...
    }
}
```

### ExportBookResultFileStorage

**Location:** `Global/ExportBookResultFileStorage.cs`

**Purpose:** Helper class for managing export book result file storage during testing.

**Key Responsibilities:**

- Store export results for verification
- Manage test file cleanup
- Provide file path resolution for export outputs

### TestHelper

**Location:** `Components/Common/Models/TestHelper.cs`

**Purpose:** Common test helper utilities and models used across export tests.

**Key Features:**

- Shared test data models
- Common assertion helpers
- Test data generation utilities

### StreamExtensions

**Location:** `Components/Extensions/StreamExtensions.cs`

**Purpose:** Extension methods for stream manipulation in test scenarios.

**Key Features:**

- Stream reading and writing helpers
- Stream comparison utilities
- Test-specific stream operations

## Dependencies

### Internal Dependencies

- **AMCode.Exports** - The library being tested

### External Dependencies

- **NUnit** (3.13.2) - Testing framework

## Usage in Test Projects

### AMCode.Exports.UnitTests

The unit tests project references this shared library to use:

- `TestHelperBase` for file path management
- Common test models and utilities
- Stream extension methods

### AMCode.Exports.IntegrationTests

The integration tests project references this shared library to use:

- `ExportBookResultFileStorage` for result file management
- Test helper base classes
- Shared test infrastructure

## Example Usage

### Using TestHelperBase

```csharp
[TestFixture]
public class ExportTests : TestHelperBase
{
    public override string GetTestDirectoryPath(TestContext testContext)
    {
        return Path.Combine(testContext.TestDirectory, "ExportTests");
    }
    
    [Test]
    public void TestExport()
    {
        var mockFile = GetFilePath("input-data.csv", TestContext.CurrentContext);
        var workDir = GetTestWorkDirectoryPath(TestContext.CurrentContext);
        
        // Perform export test...
    }
}
```

### Using ExportBookResultFileStorage

```csharp
[Test]
public void TestExportBook()
{
    var storage = new ExportBookResultFileStorage(TestContext.CurrentContext);
    var resultPath = storage.GetResultFilePath("export-result.xlsx");
    
    // Perform export and verify result...
}
```

## Benefits

### Code Reusability

- Shared test infrastructure reduces duplication
- Common patterns implemented once and reused
- Consistent test setup across test projects

### Maintainability

- Changes to test infrastructure made in one place
- Easier to update test patterns across all test projects
- Centralized test utility management

### Consistency

- Uniform test patterns across unit and integration tests
- Standardized file path resolution
- Consistent test data management

## Related Documentation

- [AMCode.Exports README](../AMCode.Exports/README.md) - Main library documentation
- [AMCode.Exports.UnitTests README](../AMCode.Exports.UnitTests/README.md) - Unit tests documentation
- [AMCode.Exports.IntegrationTests README](../AMCode.Exports.IntegrationTests/README.md) - Integration tests documentation
- [Root README](../../README.md) - Project overview

---

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

