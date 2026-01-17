# AMCode Library - Project Understanding

## Project Overview

The **AMCode Library** is a comprehensive .NET solution containing multiple reusable libraries for data processing, document generation, AI services, OCR, storage, and export functionality. It's designed as a modular library ecosystem with clear separation between core libraries and their corresponding test projects.

## Technology Stack

### .NET Versions
- **Primary**: .NET 8.0 (used by most projects)
- **Secondary**: .NET 9.0 (used by newer projects like AMCode.Data, AMCode.Sql.Builder, AMCode.Exports)
- **SDK Version**: 8.0.413 (specified in some `global.json` files)

### Project Structure
- **Solution File**: `AMCode.sln` (main solution file)
- **Architecture**: Multi-project solution with library and test project pairs
- **Package Management**: NuGet packages with local package sources

## Project Catalog

### Core Libraries

1. **AMCode.Common** (`commonlibrary/AMCode.Common/`)
   - Target Framework: .NET 8.0
   - Purpose: Common utilities and components
   - Dependencies: Microsoft.CSharp, Newtonsoft.Json
   - Test Project: `AMCode.Common.UnitTests`

2. **AMCode.Columns** (`columnslibrary/AMCode.Columns/`)
   - Purpose: Column management and data transformation
   - Test Project: None specified in solution

3. **AMCode.Vertica.Client** (`verticalibrary/AMCode.Vertica.Client/`)
   - Purpose: Vertica database client
   - Test Project: None specified in solution

4. **AMCode.Data** (`datalibrary/AMCode.Data/`)
   - Target Framework: .NET 9.0
   - Purpose: Data access layer with MongoDB, ODBC support
   - Dependencies: MongoDB.Driver, Microsoft.Extensions.*, System.Data.Odbc
   - Test Projects: `AMCode.Data.UnitTests`, `AMCode.Data.SQLTests`

5. **AMCode.Storage** (`storagelibrary/AMCode.Storage/`)
   - Purpose: Storage abstractions
   - Test Project: `AMCode.Storage.UnitTests`

6. **AMCode.Sql.Builder** (`sqlbuilderlibrary/AMCode.Sql.Builder/`)
   - Target Framework: .NET 9.0
   - Purpose: SQL query builder
   - Dependencies: AMCode.Common (local package)
   - Test Projects: `AMCode.Sql.Builder.UnitTests`, `AMCode.Sql.Builder.IntegrationTests`

7. **AMCode.Exports** (`exportslibrary/AMCode.Exports/`)
   - Target Framework: .NET 9.0
   - Purpose: Export functionality (Excel, CSV)
   - Dependencies: AMCode.Common, AMCode.Columns, AMCode.Storage, AMCode.Documents.Xlsx
   - Test Projects: `AMCode.Exports.UnitTests`, `AMCode.Exports.IntegrationTests`, `AMCode.Exports.SharedTestLibrary`

8. **AMCode.Documents** (`documentlibrary/AMCode.Documents/`)
   - Target Framework: .NET 8.0
   - Purpose: Document generation (PDF, DOCX)
   - Test Project: `AMCode.Documents.UnitTests`

9. **AMCode.OCR** (`ocrlibrary/AMCode.OCR/`)
   - Target Framework: .NET 8.0
   - Purpose: Multi-cloud OCR services (Azure, AWS, Google)
   - Dependencies: Azure Cognitive Services, AWS Textract, Google Cloud Vision
   - Test Project: `AMCode.OCR.Tests`

10. **AMCode.AI** (`ailibrary/AMCode.AI/`)
    - Target Framework: .NET 8.0
    - Purpose: Multi-cloud AI service integrations (OpenAI, Anthropic, Grok, AWS Bedrock, Ollama, Hugging Face)
    - Dependencies: OpenAI SDK, Anthropic SDK, AWS Bedrock, Microsoft.Extensions.*
    - Test Project: `AMCode.AI.Tests`

11. **AMCode.Common.Testing** (`commontestinglibrary/AMCode.Common.Testing/`)
    - Purpose: Common testing utilities
    - Test Project: `AMCode.Common.Testing.UnitTests`

12. **AMCode.Phase5B.Testing** (`phase-5b-testing/AMCode.Phase5B.Testing/`)
    - Purpose: Phase 5B specific testing

## Testing Strategy

### Test Project Naming Conventions
- `*UnitTests` - Unit test projects
- `*Tests` - General test projects
- `*IntegrationTests` - Integration test projects
- `*SQLTests` - SQL-specific test projects

### Testing Frameworks
- **NUnit** - Primary testing framework (version 3.13.2)
- **NUnit3TestAdapter** - Test adapter (version 4.2.1)
- **Microsoft.NET.Test.Sdk** - Test SDK (version 17.1.0)
- **Moq** - Mocking framework (version 4.18.2 - 4.20.69)
- **Coverlet** - Code coverage collection

### Test Organization
- Each library has corresponding test projects
- Test projects reference their parent library projects
- Some libraries have multiple test projects (unit + integration)

## Build Configuration

### Build Configurations
- **Debug** - Development builds
- **Release** - Production builds

### Build Output
- Libraries are packable (NuGet packages)
- Packages generated in `bin/Release/` or `bin/Debug/`
- Local packages stored in `local-packages/` directories

## Package Management

### NuGet Configuration
- Some projects have `nuget.config` files
- Local package sources configured for development
- Package references use version pinning

### Local Package Development
- Projects can reference local NuGet packages
- `local-packages/` directories contain built packages
- Package creation via `dotnet pack`

## Development Workflow

### Prerequisites
1. .NET SDK 8.0 and/or 9.0 installed
2. NuGet package restore
3. Build solution or individual projects
4. Run tests

### Common Operations
- **Restore**: `dotnet restore AMCode.sln`
- **Build**: `dotnet build AMCode.sln -c Release`
- **Test**: `dotnet test AMCode.sln`
- **Pack**: `dotnet pack -c Release`

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

## File Structure

```
amcode-library/
├── AMCode.sln                    # Main solution file
├── tools/                        # Development tools (newly created)
├── packages/                     # Built NuGet packages
├── commonlibrary/                # Common library
│   ├── AMCode.Common/
│   ├── AMCode.Common.UnitTests/
│   └── global.json
├── columnslibrary/               # Columns library
├── verticalibrary/               # Vertica client
├── datalibrary/                  # Data access
│   ├── AMCode.Data/
│   ├── AMCode.Data.UnitTests/
│   └── AMCode.Data.SQLTests/
├── storagelibrary/              # Storage abstractions
├── sqlbuilderlibrary/            # SQL builder
├── exportslibrary/               # Export functionality
├── documentlibrary/              # Document generation
├── ocrlibrary/                   # OCR services
├── ailibrary/                    # AI services
├── commontestinglibrary/         # Testing utilities
└── phase-5b-testing/             # Phase 5B tests
```

## Key Characteristics

1. **Multi-Target Framework**: Projects use both .NET 8.0 and 9.0
2. **Modular Design**: Clear separation between libraries and tests
3. **Local Package Development**: Supports local NuGet package development
4. **Comprehensive Testing**: Unit, integration, and SQL-specific tests
5. **Cloud Integration**: Multiple cloud service integrations (AWS, Azure, Google)
6. **Document Generation**: PDF and DOCX generation capabilities
7. **Data Export**: Excel and CSV export functionality

## Development Tools

The `tools/` directory contains scripts for:
- **install-dependencies.sh**: Install .NET SDK and restore packages
- **update-dependencies.sh**: Check for outdated packages
- **build-all.sh**: Build all projects
- **test-all.sh**: Run all test projects

## Notes

- Some projects have `global.json` files specifying SDK versions
- Projects use `GenerateDocumentationFile` for XML documentation
- Some projects have `.nuspec` files for NuGet package metadata
- Test projects include mock files and test data
- Some libraries have separate solution files (e.g., `AMCode.Common.sln`)

