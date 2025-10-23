# AMCode Common Testing Components Library

This project contains a number of common testing components that are frequently used across application test projects.

## Overview

The AMCode.Common.Testing library provides essential testing utilities including:
- Docker container management for database testing
- CSV data reading capabilities
- Common testing patterns and helpers

## Local Development Setup

This project is configured for local development with simplified NuGet configuration.

### Prerequisites

- .NET SDK 8.0 or later
- Docker (for container-based testing)
- Access to AMCode.Common library

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build AMCode.Common.Testing.sln

# Run tests
dotnet test AMCode.Common.Testing.sln

# Create NuGet package
dotnet pack AMCode.Common.Testing/AMCode.Common.Testing.csproj -c Release -o local-packages
```

## Usage

### Docker Container Testing

The library provides utilities for managing Docker containers in tests:

```csharp
using AMCode.Common.Testing.Containers;

var dockerClient = new DockerClientConfiguration().CreateClient();
var dbContainer = new DbContainer(dockerClient);

var setupParams = new ContainerSetupParams
{
    ContainerName = "test-db",
    ImageName = "postgres:latest",
    // ... other configuration
};

await dbContainer.SetupAsync(setupParams);
// ... run tests
await dbContainer.TeardownAsync("test-db");
```

### CSV Data Reading

For testing with CSV data:

```csharp
using AMCode.Common.Testing.IO;

var csvReader = new CSVDataReader("test-data.csv");
while (csvReader.Read())
{
    var data = csvReader.Line;
    // Process CSV data
}
```

## Development Guidelines

- Follow the established namespace structure: `AMCode.Common.Testing.*`
- Write comprehensive tests for all new features
- Use Docker containers for integration testing
- Maintain cross-platform compatibility
- Document all public APIs

## Project Structure

```
AMCode.Common.Testing/
├── Components/
│   ├── Containers/          # Docker container management
│   └── IO/                  # File I/O utilities
├── AMCode.Common.Testing.csproj
└── AMCode.Common.Testing.nuspec

AMCode.Common.Testing.UnitTests/
├── Components/              # Test implementations
└── AMCode.Common.Testing.UnitTests.csproj
```

## Dependencies

- AMCode.Common (1.0.0) - Core AMCode utilities
- Docker.DotNet (3.125.5) - Docker API client
- Newtonsoft.Json (13.0.1) - JSON serialization