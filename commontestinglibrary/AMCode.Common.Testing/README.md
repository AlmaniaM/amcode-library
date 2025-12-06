# AMCode.Common.Testing

**Version:** 1.0.0  
**Target Framework:** .NET 9.0  
**Last Updated:** 2025-01-27  
**Purpose:** Testing utilities and helpers for Docker container management and CSV data reading

---

## Overview

AMCode.Common.Testing provides essential testing utilities for .NET applications, focusing on Docker container lifecycle management and CSV file data reading. This library simplifies integration testing by providing abstractions for managing Docker containers (especially database containers) and reading test data from CSV files.

The library is designed to support automated testing scenarios where test environments require isolated database containers or need to read structured test data from CSV files.

## Architecture

The library follows a clean architecture pattern with clear separation between:
- **Container Management**: Docker container lifecycle operations (create, start, stop, remove)
- **Data Reading**: CSV file parsing and data access via IDataReader interface
- **Model Abstractions**: Interfaces and models for container configuration and state management

### Key Components

- **Containers**: Docker container management utilities for integration testing
  - `DbContainer`: Specialized container for database testing (e.g., Vertica)
  - `DockerContainer`: General-purpose Docker container operations
  - `DockerImage`: Docker image management
  - `DockerVolume`: Docker volume management
- **IO**: File and data reading utilities
  - `CSVDataReader`: CSV file reader implementing IDataReader interface

## Features

- **Docker Container Management**
  - Create, start, stop, and remove Docker containers
  - Database container setup and teardown
  - Container state monitoring and inspection
  - Volume management for persistent data
  - Image management and pulling
- **CSV Data Reading**
  - Read CSV files as IDataReader
  - Support for custom delimiters
  - Virtual column support for test data augmentation
  - Header management and renaming
  - Null handling for empty values
- **Testing Utilities**
  - Container lifecycle management for integration tests
  - Test data loading from CSV files
  - Container state checking and waiting

## Dependencies

### Internal Dependencies

- **AMCode.Common** (1.0.0) - Common utilities and extensions used by testing components

### External Dependencies

- **Docker.DotNet** (3.125.5) - Docker API client for .NET
- **Newtonsoft.Json** (13.0.1) - JSON serialization

## Project Structure

```
AMCode.Common.Testing/
├── Components/
│   ├── Containers/              # Docker container management
│   │   ├── Models/              # Container interfaces and models
│   │   │   ├── ContainerState.cs
│   │   │   ├── IContainerSetupParams.cs
│   │   │   ├── IContainerState.cs
│   │   │   ├── IDbContainer.cs
│   │   │   ├── IDockerContainer.cs
│   │   │   ├── IDockerImage.cs
│   │   │   ├── IDockerRunParams.cs
│   │   │   ├── IDockerVolume.cs
│   │   │   └── IGetContainerStateParams.cs
│   │   ├── DbContainer.cs       # Database container wrapper
│   │   ├── DockerContainer.cs   # General Docker container operations
│   │   ├── DockerImage.cs        # Docker image management
│   │   └── DockerVolume.cs       # Docker volume management
│   └── IO/
│       └── CSVDataReader.cs     # CSV file reader
├── AMCode.Common.Testing.csproj  # Project file
└── AMCode.Common.Testing.nuspec # NuGet package specification
```

## Key Interfaces

### IDbContainer

**Location:** `Components/Containers/Models/IDbContainer.cs`

**Purpose:** Interface for managing database containers in Docker

**Key Methods:**

- `SetupAsync(IContainerSetupParams)` - Start up a Docker container with a database
- `TeardownAsync(string, bool)` - Shut down a running container
- `TeardownAndRemoveAsync(string)` - Shut down and remove a container

**See Also:** [Containers README](Components/Containers/README.md)

### IDockerContainer

**Location:** `Components/Containers/Models/IDockerContainer.cs`

**Purpose:** Interface for general Docker container operations

**Key Methods:**

- `RunAsync(IDockerRunParams)` - Create and start a Docker container
- `StopAsync(string, bool)` - Stop a running container
- `RemoveAsync(string)` - Remove a container
- `GetContainerStateAsync(IGetContainerStateParams)` - Get container state
- `IsRunningAsync(string)` - Check if container is running

**See Also:** [Containers README](Components/Containers/README.md)

## Key Classes

### DbContainer

**Location:** `Components/Containers/DbContainer.cs`

**Purpose:** Specialized container class for database testing scenarios

**Key Responsibilities:**

- Setup database containers with proper configuration
- Manage container volumes for database persistence
- Handle environment variables for database configuration
- Provide teardown methods for cleanup

**Usage Example:**

```csharp
var dockerClient = new DockerClientConfiguration().CreateClient();
var dbContainer = new DbContainer(dockerClient);

var setupParams = new ContainerSetupParams
{
    ContainerName = "test-vertica-db",
    ImageName = "vertica/vertica-ce",
    ImageTag = "latest",
    PortMappings = new Dictionary<string, string> { { "5433", "5433" } },
    DbName = new KeyValuePair<string, string>("DBNAME", "testdb"),
    DbPassword = new KeyValuePair<string, string>("DBPASSWORD", "password")
};

await dbContainer.SetupAsync(setupParams);
// ... run tests ...
await dbContainer.TeardownAsync("test-vertica-db");
```

### DockerContainer

**Location:** `Components/Containers/DockerContainer.cs`

**Purpose:** General-purpose Docker container management

**Key Responsibilities:**

- Create and start containers
- Stop and remove containers
- Inspect container state
- Wait for container state changes

### CSVDataReader

**Location:** `Components/IO/CSVDataReader.cs`

**Purpose:** Read CSV files as IDataReader for test data loading

**Key Responsibilities:**

- Parse CSV files with custom delimiters
- Support header rows or auto-generate headers
- Provide virtual columns for test data augmentation
- Implement IDataReader interface for seamless data access

**Usage Example:**

```csharp
using (var reader = new CSVDataReader("test-data.csv", ',', firstRowHeader: true))
{
    while (reader.Read())
    {
        var id = reader.GetInt32(reader.GetOrdinal("Id"));
        var name = reader.GetString(reader.GetOrdinal("Name"));
        // Process test data
    }
}
```

## Usage Examples

### Basic Usage - Database Container Setup

```csharp
using AMCode.Common.Testing.Containers;
using Docker.DotNet;

var dockerClient = new DockerClientConfiguration().CreateClient();
var dbContainer = new DbContainer(dockerClient);

var setupParams = new ContainerSetupParams
{
    ContainerName = "integration-test-db",
    ImageName = "vertica/vertica-ce",
    ImageTag = "latest",
    PortMappings = new Dictionary<string, string> { { "5433", "5433" } },
    ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.StopExistingAndRunNew,
    AutoRemoveContainer = false
};

var success = await dbContainer.SetupAsync(setupParams);
if (success)
{
    // Run integration tests
    // ...
    
    // Cleanup
    await dbContainer.TeardownAndRemoveAsync("integration-test-db");
}
```

### Advanced Usage - CSV Data Reading with Virtual Columns

```csharp
using AMCode.Common.Testing.IO;

var reader = new CSVDataReader("test-data.csv", delimiter: ',', firstRowHeader: true, emptyAsNull: true);

// Add virtual column before reading
reader.AddVirtualColumn("TestRunId", Guid.NewGuid().ToString());
reader.RenameCSVHeader("OldColumnName", "NewColumnName");

while (reader.Read())
{
    var testRunId = reader.GetString(reader.GetOrdinal("TestRunId"));
    var csvData = reader.GetString(reader.GetOrdinal("DataColumn"));
    
    // Process test data with augmented virtual column
}

reader.Dispose();
```

### Container State Monitoring

```csharp
using AMCode.Common.Testing.Containers;
using AMCode.Common.Testing.Containers.Models;

var dockerClient = new DockerClientConfiguration().CreateClient();
var container = new DockerContainer(dockerClient);

var stateParams = new GetContainerStateParams { ContainerName = "my-container" };
var state = await container.GetContainerStateAsync(stateParams);

if (state.Running)
{
    Console.WriteLine($"Container is running (PID: {state.Pid})");
}
else if (state.Status == "exited")
{
    Console.WriteLine($"Container exited with code: {state.ExitCode}");
}
```

## Configuration

### Docker Client Configuration

```csharp
using Docker.DotNet;

// Local Docker daemon
var dockerClient = new DockerClientConfiguration().CreateClient();

// Remote Docker daemon
var dockerClient = new DockerClientConfiguration(
    new Uri("tcp://remote-docker-host:2376")
).CreateClient();
```

### Container Setup Parameters

```csharp
var setupParams = new ContainerSetupParams
{
    ContainerName = "test-container",
    ImageName = "my-image",
    ImageTag = "latest",
    PortMappings = new Dictionary<string, string>
    {
        { "8080", "80" },  // Host port 8080 -> Container port 80
        { "5433", "5433" } // Host port 5433 -> Container port 5433
    },
    VolumeMaps = new Dictionary<string, string>
    {
        { "volume-name", "/container/path" }
    },
    EnvironmentVariables = new Dictionary<string, string>
    {
        { "ENV_VAR", "value" }
    },
    ActionForWhenAlreadyRunning = DockerRunAlreadyRunningAction.StopExistingAndRunNew,
    AutoRemoveContainer = true
};
```

## Testing

### Test Projects

- **AMCode.Common.Testing.UnitTests**: Unit tests for testing utilities
  - Container management tests
  - CSV reader tests
  - [Test Project README](../AMCode.Common.Testing.UnitTests/README.md)

### Running Tests

```bash
dotnet test AMCode.Common.Testing.UnitTests
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Containers](Components/Containers/README.md) - Docker container management utilities
- [IO](Components/IO/README.md) - CSV data reading utilities

## Related Libraries

- [AMCode.Common](../commonlibrary/AMCode.Common/README.md) - Common utilities and extensions used by this library

## Migration Notes

- **.NET 9.0**: This library targets .NET 9.0. Ensure your test projects target compatible frameworks.
- **Docker.DotNet**: Version 3.125.5 is required. Ensure Docker daemon is accessible.
- **Container Lifecycle**: Always call teardown methods in test cleanup to avoid resource leaks.

## Known Issues

- CSV reader requires file to be accessible at construction time
- Container operations require Docker daemon to be running
- Virtual columns must be added before first Read() call

## Future Considerations

- Support for additional container orchestration platforms
- Enhanced CSV reader with more data type conversions
- Container health check utilities
- Test data generation helpers

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
