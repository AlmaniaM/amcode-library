# AMCode Common Library - Development Guide

This guide provides comprehensive instructions for building and developing the AMCode Common Library locally.

## Prerequisites

- **Required .NET SDK**: Version 5.0.402 (as specified in `global.json`)
- **Recommended IDE**: Visual Studio 2022, VS Code with C# extension, or JetBrains Rider
- **Optional Tools**: 
  - `dotnet-format` for code formatting
  - Git for version control

## Building the Project Locally

### 1. Restore NuGet Packages
```bash
dotnet restore
```

### 2. Build the Solution
```bash
dotnet build
```

### 3. Run Tests
```bash
dotnet test
```

### 4. Create NuGet Package
```bash
dotnet pack -c Release
```

The package will be created in `AMCode.Common/bin/Release/AMCode.Common.{version}.nupkg`.

## Referencing This Library in Other Projects

### Option A: Local NuGet Package (Recommended for Testing)

1. **Build and Pack the Library**:
   ```bash
   dotnet pack -c Release -o local-packages
   ```

2. **Add Local Package Source** to your consuming project's `nuget.config`:
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <packageSources>
       <clear />
       <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
       <add key="Local" value="./local-packages" />
     </packageSources>
   </configuration>
   ```

3. **Reference the Package** in your consuming project's `.csproj`:
   ```xml
   <PackageReference Include="AMCode.Common" Version="2.3.4" />
   ```

### Option B: Project Reference (For Active Development)

Add a direct project reference in your consuming project's `.csproj`:
```xml
<ProjectReference Include="..\..\path\to\commonlibrary\AMCode.Common\AMCode.Common.csproj" />
```

**Benefits**: Changes are immediately reflected without rebuilding packages.

### Option C: DLL Reference (For Specific Scenarios)

1. **Build the Library**:
   ```bash
   dotnet build -c Release
   ```

2. **Reference the Built DLL** in your consuming project's `.csproj`:
   ```xml
   <Reference Include="AMCode.Common">
     <HintPath>..\..\path\to\commonlibrary\AMCode.Common\bin\Release\netstandard2.0\AMCode.Common.dll</HintPath>
   </Reference>
   ```

## Local Package Management

### Directory Structure
```
commonlibrary/
├── local-packages/          # Local NuGet packages directory
│   ├── .gitignore          # Excludes package files
│   └── README.md           # Usage instructions
└── ...
```

### Organizing Local Packages
- Store all locally built packages in the `local-packages/` directory
- Use semantic versioning for local testing (e.g., `2.3.4-local.1`)
- Clean up old packages regularly to avoid confusion

### Versioning for Local Testing
```bash
# Create a local test version
dotnet pack -c Release -o local-packages --version-suffix "local.1"
```

## Referencing Other Local AMCode Libraries

When working with multiple AMCode libraries in the same workspace:

### Best Practices
1. **Consistent Package Sources**: Use the same `nuget.config` across all projects
2. **Version Coordination**: Keep versions synchronized across related libraries
3. **Dependency Order**: Build libraries in dependency order (dependencies first)

### Example Multi-Library Setup
```
workspace/
├── commonlibrary/           # Base library
├── datalibrary/            # Depends on commonlibrary
├── storagelibrary/         # Depends on commonlibrary
└── myproject/              # Depends on all libraries
```

**Build Order**:
1. `commonlibrary` → `local-packages/`
2. `datalibrary` → `local-packages/`
3. `storagelibrary` → `local-packages/`
4. `myproject` (references all packages)

## Development Workflow

### Making Changes
1. **Create Feature Branch**: `git checkout -b feature/my-feature`
2. **Make Changes**: Implement your feature or fix
3. **Write Tests**: Add unit tests for new functionality
4. **Run Tests**: `dotnet test` to ensure all tests pass
5. **Build Solution**: `dotnet build` to check for compilation errors
6. **Code Formatting**: Use `dotnet format` if available

### Testing Changes
```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run specific test project
dotnet test AMCode.Common.UnitTests/
```

### Building for Release
```bash
# Clean previous builds
dotnet clean

# Build in Release mode
dotnet build -c Release

# Create release package
dotnet pack -c Release -o local-packages
```

## Troubleshooting

### Common Build Errors

#### NuGet Package Not Found
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

#### Version Conflicts
- Check `global.json` for SDK version requirements
- Ensure all projects target compatible frameworks
- Verify package references are consistent

#### Local Package Issues
```bash
# Remove old local packages
rm local-packages/*.nupkg

# Rebuild and repack
dotnet clean
dotnet build -c Release
dotnet pack -c Release -o local-packages
```

### Resolving Dependency Issues
1. **Check Package Sources**: Verify `nuget.config` includes all required sources
2. **Update Package Versions**: Ensure all packages are compatible
3. **Clean and Rebuild**: Clear caches and rebuild from scratch

### Handling Outdated Local Packages
- Regularly clean the `local-packages/` directory
- Use version suffixes for local testing
- Document which packages are for local development vs. production

## Code Quality Guidelines

### Testing Requirements
- All new features must have corresponding unit tests
- Bug fixes must include regression tests
- Maintain test coverage above 80%

### Code Style
- Follow the `.editorconfig` settings
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Keep methods focused and small

### Namespace Structure
- Follow the established `AMCode.Common.*` pattern
- Group related functionality in appropriate namespaces
- Maintain consistency with existing code organization

## Additional Resources

- [.NET Standard Documentation](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
- [NuGet Package Creation](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package)
- [NUnit Testing Framework](https://nunit.org/)
- [EditorConfig](https://editorconfig.org/)
