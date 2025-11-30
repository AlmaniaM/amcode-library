# AMCode Library - Development Tools

This directory contains scripts for managing dependencies, building, and testing the AMCode Library projects on Ubuntu 20.04+ and macOS.

## Available Scripts

### 1. `project-cli.sh` ⭐ **NEW - Unified CLI**
Unified command-line interface for building and testing individual projects.

**Usage:**
```bash
# List all projects
./tools/project-cli.sh list

# List only library projects
./tools/project-cli.sh list-libs

# List only test projects
./tools/project-cli.sh list-tests

# Build a specific project
./tools/project-cli.sh build AMCode.Common
./tools/project-cli.sh build Common -c Debug

# Test a specific project
./tools/project-cli.sh test AMCode.Common.UnitTests
./tools/project-cli.sh test Common -v detailed

# Build using project path
./tools/project-cli.sh build commonlibrary/AMCode.Common/AMCode.Common.csproj
```

**What it does:**
- Provides a unified CLI for all project operations
- Supports project name matching (exact or partial)
- Supports project paths (relative or absolute)
- Lists available projects with details (framework, path)
- Builds individual projects with configuration options
- Tests individual projects with verbosity control

**Project Matching:**
- Exact name: `AMCode.Common`
- Partial name: `Common` (matches any project containing "Common")
- Full path: `commonlibrary/AMCode.Common/AMCode.Common.csproj`
- Directory path: `commonlibrary/AMCode.Common/` (finds .csproj inside)

### 2. `install-dependencies.sh`
Installs/updates .NET SDK and restores NuGet packages for all projects.

**Usage:**
```bash
./tools/install-dependencies.sh
```

**What it does:**
- Detects the operating system (Ubuntu 20.04+ or macOS)
- Analyzes project files to determine required .NET SDK versions
- Installs missing .NET SDK versions (supports .NET 8.0 and 9.0)
- Restores all NuGet packages for the solution

**Requirements:**
- Ubuntu 20.04+: Requires `wget` and `sudo` access
- macOS: Requires Homebrew (recommended) or manual .NET installation

### 2. `update-dependencies.sh`
Checks for outdated NuGet packages across all projects.

**Usage:**
```bash
./tools/update-dependencies.sh
```

**What it does:**
- Installs/updates `dotnet-outdated-tool` globally
- Scans all projects for outdated packages
- Displays a report of packages that can be updated
- Restores packages after checking

**Note:** This script only reports outdated packages. To update, manually edit `.csproj` files with new package versions.

### 3. `build-all.sh`
Builds all projects in the solution, or a specific project if specified.

**Usage:**
```bash
# Build all projects in Release mode (default)
./tools/build-all.sh

# Build all projects in Debug mode
./tools/build-all.sh Debug

# Build a specific project
./tools/build-all.sh Release AMCode.Common
./tools/build-all.sh Debug Common
```

**What it does:**
- Builds the entire solution (`AMCode.sln`) if it exists and no specific project is given
- Or builds all individual library projects (excluding test projects)
- If a project name is provided, builds only that project
- Supports project name matching (exact or partial)
- Reports build success/failure for each project
- Exits with error code if any build fails

### 4. `test-all.sh`
Runs all test projects in the solution, or a specific test project if specified.

**Usage:**
```bash
# Run all tests in Release mode with normal verbosity (default)
./tools/test-all.sh

# Run all tests in Debug mode
./tools/test-all.sh Debug

# Run all tests with detailed output
./tools/test-all.sh Release detailed

# Run a specific test project
./tools/test-all.sh Release normal AMCode.Common.UnitTests
./tools/test-all.sh Debug detailed Common
```

**What it does:**
- Finds all test projects (projects with `*Test*.csproj`, `*Tests.csproj`, etc.)
- If a project name is provided, runs tests only for that project
- Supports project name matching (exact or partial)
- Runs tests for each project
- Collects code coverage data
- Generates test results in `TestResults/` directory
- Reports summary of passed/failed tests

**Verbosity levels:** `quiet`, `minimal`, `normal`, `detailed`, `diagnostic`

## Quick Start

### First Time Setup

1. **Install dependencies:**
   ```bash
   chmod +x tools/*.sh
   ./tools/install-dependencies.sh
   ```

2. **List available projects:**
   ```bash
   ./tools/project-cli.sh list
   ```

3. **Build all projects:**
   ```bash
   ./tools/build-all.sh
   ```

4. **Run all tests:**
   ```bash
   ./tools/test-all.sh
   ```

### Daily Development Workflow

#### Option A: Using the Unified CLI (Recommended)
```bash
# List projects to find what you need
./tools/project-cli.sh list-libs
./tools/project-cli.sh list-tests

# Build a specific project
./tools/project-cli.sh build AMCode.Common -c Debug

# Test a specific project
./tools/project-cli.sh test AMCode.Common.UnitTests -v detailed
```

#### Option B: Using Individual Scripts
```bash
# Update dependencies (optional)
./tools/update-dependencies.sh

# Build a specific project
./tools/build-all.sh Debug AMCode.Common

# Test a specific project
./tools/test-all.sh Debug detailed AMCode.Common.UnitTests

# Or build/test all projects
./tools/build-all.sh Debug
./tools/test-all.sh Debug
```

### Working with Individual Projects

**Find a project:**
```bash
# List all projects
./tools/project-cli.sh list

# List only libraries
./tools/project-cli.sh list-libs

# List only tests
./tools/project-cli.sh list-tests
```

**Build a project:**
```bash
# By exact name
./tools/project-cli.sh build AMCode.Common

# By partial name (matches any project containing "Common")
./tools/project-cli.sh build Common

# With Debug configuration
./tools/project-cli.sh build Common -c Debug

# Using build-all.sh
./tools/build-all.sh Release AMCode.Common
```

**Test a project:**
```bash
# By exact name
./tools/project-cli.sh test AMCode.Common.UnitTests

# By partial name
./tools/project-cli.sh test Common

# With detailed verbosity
./tools/project-cli.sh test Common -v detailed

# Using test-all.sh
./tools/test-all.sh Release normal AMCode.Common.UnitTests
```

## Project Structure

The AMCode Library solution contains multiple projects:

### Core Libraries
- `AMCode.Common` - Common utilities and components
- `AMCode.Columns` - Column management
- `AMCode.Vertica.Client` - Vertica database client
- `AMCode.Data` - Data access layer
- `AMCode.Storage` - Storage abstractions
- `AMCode.Sql.Builder` - SQL query builder
- `AMCode.Exports` - Export functionality
- `AMCode.Documents` - Document generation
- `AMCode.OCR` - OCR services
- `AMCode.AI` - AI service integrations

### Test Projects
Each library has corresponding test projects:
- `*UnitTests` - Unit tests
- `*IntegrationTests` - Integration tests
- `*SQLTests` - SQL-specific tests

## Requirements

### .NET SDK Versions
The solution uses multiple .NET versions:
- **.NET 8.0** - Used by most projects
- **.NET 9.0** - Used by some newer projects

The `install-dependencies.sh` script automatically detects and installs the required versions.

### Operating Systems
- **Ubuntu 20.04+** - Fully supported
- **macOS** - Fully supported (Homebrew recommended)

## Troubleshooting

### .NET SDK Not Found
If you see "`.NET SDK is not installed`", run:
```bash
./tools/install-dependencies.sh
```

### Build Failures
1. Ensure dependencies are installed:
   ```bash
   ./tools/install-dependencies.sh
   ```

2. Try cleaning and rebuilding:
   ```bash
   dotnet clean AMCode.sln
   ./tools/build-all.sh
   ```

### Test Failures
1. Ensure projects are built:
   ```bash
   ./tools/build-all.sh
   ```

2. Run tests with detailed output:
   ```bash
   ./tools/test-all.sh Debug detailed
   ```

### Permission Denied
Make scripts executable:
```bash
chmod +x tools/*.sh
```

## Project Selection Methods

All scripts support multiple ways to specify projects:

1. **Exact Project Name**: `AMCode.Common`
2. **Partial Name**: `Common` (matches any project containing "Common")
3. **Project Path**: `commonlibrary/AMCode.Common/AMCode.Common.csproj`
4. **Directory Path**: `commonlibrary/AMCode.Common/` (automatically finds .csproj)

The unified CLI (`project-cli.sh`) provides the most flexible project selection with helpful error messages and suggestions.

## Notes

- All scripts use colored output for better readability
- Scripts exit on error (`set -e`) to prevent partial execution
- Test results and coverage reports are saved in `TestResults/` directory
- The solution supports both Debug and Release configurations
- The unified CLI (`project-cli.sh`) is recommended for individual project operations
- `build-all.sh` and `test-all.sh` can also work with individual projects when a project name is provided

