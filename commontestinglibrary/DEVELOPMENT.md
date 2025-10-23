# AMCode.Common.Testing Development Guide

## Local Development Setup

### Prerequisites
- .NET SDK 8.0 or later
- Docker Desktop (for container-based testing)
- Visual Studio Code or Visual Studio

### Initial Setup

1. **Clone and Navigate**
   ```bash
   cd commontestinglibrary
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build Solution**
   ```bash
   dotnet build AMCode.Common.Testing.sln
   ```

### Development Workflow

#### Running Tests
```bash
# Run all tests
dotnet test AMCode.Common.Testing.sln

# Run specific test project
dotnet test AMCode.Common.Testing.UnitTests/AMCode.Common.Testing.UnitTests.csproj

# Run with verbose output
dotnet test --verbosity normal
```

#### Creating NuGet Package
```bash
# Create package in Release mode
dotnet pack AMCode.Common.Testing/AMCode.Common.Testing.csproj -c Release -o local-packages

# The package will be created as:
# local-packages/AMCode.Common.Testing.1.0.0.nupkg
```

#### Local Package Testing
```bash
# Add local package source to test project
dotnet add AMCode.Common.Testing.UnitTests/AMCode.Common.Testing.UnitTests.csproj package AMCode.Common.Testing --source ./local-packages
```

### Project Structure

```
commontestinglibrary/
├── AMCode.Common.Testing/                    # Main library
│   ├── Components/
│   │   ├── Containers/                      # Docker container management
│   │   │   ├── DbContainer.cs              # Database container setup
│   │   │   ├── DockerContainer.cs         # Generic container operations
│   │   │   ├── DockerImage.cs             # Image management
│   │   │   ├── DockerVolume.cs            # Volume management
│   │   │   └── Models/                    # Container-related interfaces and models
│   │   └── IO/                            # File I/O utilities
│   │       └── CSVDataReader.cs           # CSV file reading
│   ├── AMCode.Common.Testing.csproj       # Main project file
│   └── AMCode.Common.Testing.nuspec       # NuGet package specification
├── AMCode.Common.Testing.UnitTests/         # Test project
│   ├── Components/                         # Test implementations
│   │   ├── Containers/                     # Container tests
│   │   └── IO/                            # I/O tests
│   └── AMCode.Common.Testing.UnitTests.csproj
├── local-packages/                         # Local NuGet packages
├── AMCode.Common.Testing.sln               # Solution file
├── global.json                            # .NET SDK version
├── nuget.config                           # NuGet configuration
└── README.md                              # Project documentation
```

### Key Components

#### Container Management
- **DbContainer**: High-level database container management
- **DockerContainer**: Generic Docker container operations
- **DockerImage**: Docker image management
- **DockerVolume**: Docker volume management

#### I/O Utilities
- **CSVDataReader**: Implements IDataReader for CSV files with support for:
  - Custom delimiters
  - Header detection
  - Virtual columns
  - Null value handling

### Testing Strategy

#### Unit Tests
- Test individual components in isolation
- Mock external dependencies (Docker API)
- Focus on business logic validation

#### Integration Tests
- Test Docker container lifecycle
- Verify CSV data reading accuracy
- Test cross-platform compatibility

#### Test Data
- Mock CSV files in `Components/IO/MockFiles/`
- Test containers use lightweight images
- Clean up resources in test teardown

### Code Standards

#### Naming Conventions
- Classes: PascalCase (e.g., `DbContainer`)
- Methods: PascalCase (e.g., `SetupAsync`)
- Properties: PascalCase (e.g., `ContainerName`)
- Private fields: camelCase with underscore prefix (e.g., `_dockerClient`)

#### Async Patterns
- Use `async/await` for all I/O operations
- Return `Task<T>` for async methods
- Use `ConfigureAwait(false)` in library code

#### Error Handling
- Use specific exception types
- Provide meaningful error messages
- Log errors appropriately

### Docker Testing Requirements

#### Environment Variables
For Docker-based tests, set these environment variables:
```bash
# Docker Hub credentials (if using private repositories)
export AMCODE_TEST_DOCKER_USERNAME="your-username"
export AMCODE_TEST_DOCKER_PAT="your-personal-access-token"
```

#### Test Images
- Use lightweight test images (e.g., `nats:latest`)
- Avoid pulling large database images in CI/CD
- Clean up containers and images after tests

### Troubleshooting

#### Common Issues

1. **Docker Connection Failed**
   - Ensure Docker Desktop is running
   - Check Docker daemon accessibility
   - Verify environment variables

2. **Package Restore Failed**
   - Check internet connectivity
   - Verify NuGet configuration
   - Clear NuGet cache: `dotnet nuget locals all --clear`

3. **Test Failures**
   - Check Docker container cleanup
   - Verify test data files exist
   - Review test isolation

#### Debug Commands
```bash
# Check Docker status
docker info

# List running containers
docker ps

# Clean up test containers
docker container prune -f

# Check NuGet sources
dotnet nuget list source
```

### Contributing

1. Follow the established code patterns
2. Write tests for new functionality
3. Update documentation as needed
4. Ensure cross-platform compatibility
5. Test Docker functionality thoroughly
