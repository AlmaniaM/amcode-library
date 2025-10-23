# AMCode Storage Library - Development Guide

This guide provides detailed instructions for developing and maintaining the AMCode Storage Library.

## Project Structure

```
storagelibrary/
├── AMCode.Storage/                    # Main library project
│   ├── Components/
│   │   ├── AzureBlob/                 # Azure Blob Storage implementations
│   │   ├── InMemory/                  # In-memory storage implementations
│   │   ├── Local/                     # Local file storage implementations
│   │   └── Storage/                   # Core interfaces and base classes
│   ├── AMCode.Storage.csproj
│   └── AMCode.Storage.nuspec
├── AMCode.Storage.UnitTests/           # Unit tests
│   ├── Components/                    # Test implementations for each storage type
│   ├── Globals/                       # Test utilities and resources
│   └── AMCode.Storage.UnitTests.csproj
├── AMCode.Storage.sln                 # Solution file
├── global.json                        # .NET SDK version
├── nuget.config                       # NuGet package sources
├── local-packages/                    # Local package output directory
└── README.md                          # Project overview
```

## Development Setup

### Prerequisites

1. **.NET 8.0 SDK**: Install from [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
2. **Docker**: Required for running Azurite emulator for Azure Blob Storage tests
3. **IDE**: Visual Studio, VS Code, or Rider

### Local Development

1. **Clone and Setup**:
   ```bash
   git clone <repository-url>
   cd storagelibrary
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Build Solution**:
   ```bash
   dotnet build AMCode.Storage.sln
   ```

4. **Run Tests**:
   ```bash
   # Start Azurite emulator
   docker run -d -p 10000:10000 mcr.microsoft.com/azure-storage/azurite
   
   # Run tests
   dotnet test AMCode.Storage.sln
   ```

## Testing

### Test Structure

The test project follows the same structure as the main library:

- **AzureBlob Tests**: Integration tests with Azurite emulator
- **Local Tests**: File system tests with temporary directories
- **InMemory Tests**: Pure unit tests with in-memory storage
- **Globals**: Shared test utilities and resources

### Running Tests

```bash
# All tests
dotnet test AMCode.Storage.sln

# Specific test project
dotnet test AMCode.Storage.UnitTests/AMCode.Storage.UnitTests.csproj

# With verbose output
dotnet test AMCode.Storage.sln --verbosity normal

# With coverage
dotnet test AMCode.Storage.sln --collect:"XPlat Code Coverage"
```

### Test Dependencies

- **Azurite**: Azure Storage emulator for blob storage tests
- **TestHelper**: Utility class for managing test directories and mock files
- **TestResources**: Connection strings and test configuration

## Building and Packaging

### Build Configuration

The project supports both Debug and Release configurations:

```bash
# Debug build
dotnet build AMCode.Storage.sln -c Debug

# Release build
dotnet build AMCode.Storage.sln -c Release
```

### Package Creation

```bash
# Create NuGet package
dotnet pack AMCode.Storage/AMCode.Storage.csproj -c Release -o local-packages

# Package will be created as: AMCode.Storage.1.1.2.nupkg
```

### Local Package Testing

To test the package locally:

1. **Build Package**:
   ```bash
   dotnet pack AMCode.Storage/AMCode.Storage.csproj -c Release -o local-packages
   ```

2. **Reference in Test Project**:
   ```xml
   <PackageReference Include="AMCode.Storage" Version="1.1.2" />
   ```

3. **Restore and Test**:
   ```bash
   dotnet restore
   dotnet test
   ```

## Code Organization

### Namespace Structure

- `AMCode.Storage` - Core interfaces and base classes
- `AMCode.Storage.AzureBlob` - Azure Blob Storage implementations
- `AMCode.Storage.Local` - Local file storage implementations
- `AMCode.Storage.Memory` - In-memory storage implementations

### Key Interfaces

- `IStreamDataSourceAsync` - Main interface for stream storage
- `ISimpleFileStorage` - Lower-level file storage interface
- `IFileDownloadResponse` - Response interface for file downloads

### Implementation Classes

- `BaseStreamDataSource` - Base implementation of `IStreamDataSourceAsync`
- `AzureBlobStreamDataSource` - Azure Blob Storage implementation
- `FileStreamDataSource` - Local file system implementation
- `MemoryStreamDataSource` - In-memory implementation

## Dependencies

### External Dependencies

- **Azure.Storage.Blobs** (12.10.0) - Azure Blob Storage client
- **AMCode.Common** (1.0.0) - Common utilities and extensions

### Development Dependencies

- **NUnit** (3.13.2) - Testing framework
- **NUnit3TestAdapter** (4.1.0) - Test adapter for Visual Studio
- **Microsoft.NET.Test.Sdk** (17.0.0) - Test SDK
- **coverlet.collector** (3.1.0) - Code coverage collection

## Configuration

### NuGet Configuration

The `nuget.config` file configures package sources:

```xml
<packageSources>
  <clear />
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  <add key="Local" value="./local-packages" />
</packageSources>
```

### SDK Version

The `global.json` file specifies the .NET SDK version:

```json
{
  "sdk": {
    "version": "8.0.413"
  }
}
```

## Troubleshooting

### Common Issues

1. **Azurite Connection Issues**:
   - Ensure Docker is running
   - Check that port 10000 is available
   - Verify Azurite container is running: `docker ps`

2. **Test Failures**:
   - Check that test directories exist
   - Verify mock files are copied to output directory
   - Ensure proper path separators for cross-platform compatibility

3. **Build Issues**:
   - Clean solution: `dotnet clean`
   - Restore packages: `dotnet restore`
   - Rebuild: `dotnet build`

### Debug Configuration

For debugging in VS Code, the `.vscode/launch.json` file is configured to run tests:

```json
{
  "program": "${workspaceFolder}/AMCode.Storage.UnitTests/bin/Debug/net8.0/AMCode.Storage.UnitTests.dll",
  "cwd": "${workspaceFolder}/AMCode.Storage.UnitTests"
}
```

## Contributing

### Code Style

- Follow C# naming conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Write unit tests for new functionality

### Pull Request Process

1. Create feature branch from main
2. Implement changes with tests
3. Ensure all tests pass
4. Update documentation if needed
5. Submit pull request

### Version Management

- Update version in `AMCode.Storage.nuspec`
- Follow semantic versioning (Major.Minor.Patch)
- Update changelog for significant changes
