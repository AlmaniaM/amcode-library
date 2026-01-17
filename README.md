# AMCode Library Ecosystem

**Version:** 1.0  
**Last Updated:** 2025-01-27  
**Purpose:** Comprehensive .NET library ecosystem for data processing, document generation, AI services, OCR, storage, and export functionality

---

## Overview

The AMCode Library is a modular .NET solution containing multiple reusable libraries designed with Clean Architecture principles. Each library is independently versioned and can be used standalone or as part of the complete ecosystem.

## Technology Stack

- **Primary Framework:** .NET 8.0 (most libraries)
- **Secondary Framework:** .NET 9.0 (newer libraries: AMCode.Data, AMCode.Sql.Builder, AMCode.Exports)
- **Package Management:** NuGet
- **Testing Framework:** NUnit
- **Architecture Pattern:** Clean Architecture with multi-provider support

## Solution Structure

```
AMCode.sln
├── Core Libraries
│   ├── AMCode.Common
│   ├── AMCode.Columns
│   ├── AMCode.Vertica.Client
│   └── AMCode.Common.Testing
├── Data & Storage
│   ├── AMCode.Data
│   └── AMCode.Storage
├── SQL & Export
│   ├── AMCode.Sql.Builder
│   └── AMCode.Exports
├── Document Generation
│   └── AMCode.Documents
└── AI & OCR
    ├── AMCode.OCR
    └── AMCode.AI
```

## Library Catalog

### Core Libraries

1. **[AMCode.Common](commonlibrary/AMCode.Common/README.md)**
   - Common utilities and components
   - Target: .NET 8.0
   - Dependencies: Microsoft.CSharp, Newtonsoft.Json
   - Test Project: AMCode.Common.UnitTests

2. **[AMCode.Columns](columnslibrary/AMCode.Columns/README.md)**
   - Column management and data transformation
   - Builder pattern for column definitions
   - Core column types and data transformation utilities

3. **[AMCode.Vertica.Client](verticalibrary/AMCode.Vertica.Client/README.md)**
   - Vertica database client
   - Database connectivity and query execution

4. **[AMCode.Common.Testing](commontestinglibrary/AMCode.Common.Testing/README.md)**
   - Testing utilities and helpers
   - Test data generators
   - Common testing infrastructure
   - Test Project: AMCode.Common.Testing.UnitTests

### Data & Storage Libraries

5. **[AMCode.Data](datalibrary/AMCode.Data/README.md)**
   - Data access layer
   - MongoDB and ODBC support
   - Target: .NET 9.0
   - Dependencies: MongoDB.Driver, Microsoft.Extensions.*, System.Data.Odbc
   - Test Projects: AMCode.Data.UnitTests, AMCode.Data.SQLTests

6. **[AMCode.Storage](storagelibrary/AMCode.Storage/README.md)**
   - Storage abstractions
   - Multi-provider support (Azure Blob, S3, Local)
   - Target: .NET 8.0
   - Test Project: AMCode.Storage.UnitTests

### SQL & Export Libraries

7. **[AMCode.Sql.Builder](sqlbuilderlibrary/AMCode.Sql.Builder/README.md)**
   - SQL query builder
   - Fluent API for SQL construction
   - Target: .NET 9.0
   - Dependencies: AMCode.Common (local package)
   - Test Projects: AMCode.Sql.Builder.UnitTests, AMCode.Sql.Builder.IntegrationTests

8. **[AMCode.Exports](exportslibrary/AMCode.Exports/README.md)**
   - Export functionality (Excel, CSV, ZIP)
   - Target: .NET 9.0
   - Dependencies: AMCode.Common, AMCode.Columns, AMCode.Storage
   - Test Projects: AMCode.Exports.UnitTests, AMCode.Exports.IntegrationTests, AMCode.Exports.SharedTestLibrary

### Document Generation Libraries

9. **[AMCode.Documents](documentlibrary/AMCode.Documents/README.md)**
   - Document generation (PDF, DOCX, Excel)
   - Multiple provider support
   - Target: .NET 8.0
   - Test Project: AMCode.Documents.UnitTests

### AI & OCR Libraries

10. **[AMCode.OCR](ocrlibrary/AMCode.OCR/README.md)**
    - Multi-provider OCR support
    - Providers: Google Cloud Vision, Azure Cognitive Services, AWS Textract, PaddleOCR
    - Target: .NET 8.0
    - Dependencies: Azure Cognitive Services, AWS Textract, Google Cloud Vision
    - Test Project: AMCode.OCR.Tests

11. **[AMCode.AI](ailibrary/AMCode.AI/README.md)**
    - Multi-provider AI/LLM support
    - Providers: OpenAI, Anthropic, AWS Bedrock, Ollama, HuggingFace, Grok
    - Recipe parsing service
    - Target: .NET 8.0
    - Dependencies: OpenAI SDK, Anthropic SDK, AWS Bedrock, Microsoft.Extensions.*
    - Test Project: AMCode.AI.Tests

### Special Projects

12. **[AMCode.Phase5B.Testing](phase-5b-testing/AMCode.Phase5B.Testing/README.md)**
    - Phase 5B specific testing utilities
    - Specialized testing scenarios

### Development Dependencies

**AMCode.Documents.Xlsx** (Development/Mock Library)
    - **Status**: Temporary mock library for local development
    - **Location**: `exportslibrary/temp-packages/AMCode.Documents.Xlsx.csproj`
    - **Purpose**: Provides interface stubs for Excel functionality during development
    - **Target**: .NET 8.0
    - **Note**: This is a mock/temporary library containing only interface definitions. It's used by AMCode.Exports for local development and testing. The actual AMCode.Documents.Xlsx library may be external or in development.
    - **Used By**: AMCode.Exports

## Quick Start

### Prerequisites

- .NET SDK 8.0 or 9.0
- NuGet package manager

### Building the Solution

```bash
# Restore packages
dotnet restore AMCode.sln

# Build all projects
dotnet build AMCode.sln -c Release

# Or use build script
./tools/build-all.sh
```

### Running Tests

```bash
# Run all tests
dotnet test AMCode.sln

# Or use test script
./tools/test-all.sh
```

### Creating Packages

```bash
# Create packages for all projects
dotnet pack AMCode.sln -c Release

# Or use package script
./CreatePackages.sh
# Or Windows
./CreatePackages.bat
# Or PowerShell
./CreatePackages.ps1
```

## Development Workflow

1. **Restore Dependencies**: `dotnet restore AMCode.sln`
2. **Build Solution**: `dotnet build AMCode.sln`
3. **Run Tests**: `dotnet test AMCode.sln`
4. **Create Package**: `dotnet pack -c Release`

## Project Dependencies

### Internal Dependencies

- Projects reference each other via `ProjectReference`
- Some projects use local NuGet packages (e.g., AMCode.Common)
- Dependency chain: Common → Columns → Data/Storage → Exports

### External Dependencies

- **Microsoft.Extensions.*** - Dependency injection, configuration, logging
- **Newtonsoft.Json** - JSON serialization
- **MongoDB.Driver** - MongoDB database access
- **System.Data.Odbc** - ODBC database access
- **OpenAI SDK** - OpenAI API integration
- **Anthropic SDK** - Anthropic API integration
- **AWS SDKs** - AWS services (Bedrock, Textract)
- **Google Cloud Vision** - Google OCR services
- **Azure Cognitive Services** - Azure OCR services

## Testing Strategy

### Test Project Organization

- **Unit Tests**: `*UnitTests` projects for isolated component testing
- **Integration Tests**: `*IntegrationTests` projects for system integration testing
- **SQL Tests**: `*SQLTests` projects for database-specific testing
- **General Tests**: `*Tests` projects for comprehensive testing

### Testing Frameworks

- **NUnit** - Primary testing framework (version 3.13.2)
- **NUnit3TestAdapter** - Test adapter (version 4.2.1)
- **Microsoft.NET.Test.Sdk** - Test SDK (version 17.1.0)
- **Moq** - Mocking framework (version 4.18.2 - 4.20.69)
- **Coverlet** - Code coverage collection

## Documentation

Each library has comprehensive documentation:

- **Library README**: Overview, architecture, usage examples
- **Subfolder READMEs**: Detailed class/interface documentation
- **Integration Guides**: Provider-specific integration documentation

## Contributing

1. Make your changes
2. Run unit tests: `dotnet test`
3. Build solution: `dotnet build`
4. Update relevant documentation
5. Create pull request

## Related Documentation

- [Documentation Plan](DOCUMENTATION_PLAN.md) - Complete documentation strategy
- [Project Understanding](tools/PROJECT_UNDERSTANDING.md) - Detailed project analysis
- [Development Tools](tools/README.md) - Development scripts and utilities

---

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
