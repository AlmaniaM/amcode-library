# AMCode Library Monorepo — Agent Guide

## What This Is

Collection of reusable .NET libraries powering the RecipeOCR kitchen management app and other AMCode projects. Each library is independently buildable and follows a multi-cloud abstraction pattern.

## Module Decision Tree

**"I need to..."**

| Need | Library | Directory |
|------|---------|-----------|
| Call an AI model (text, chat, JSON, vision, embeddings) | **AMCode.AI** | `ailibrary/AMCode.AI/` |
| Create a pipeline for an AI-powered feature | **AMCode.AI** (Pipelines) | `ailibrary/AMCode.AI/Pipelines/` |
| Extract text from images (OCR) | **AMCode.OCR** | `ocrlibrary/AMCode.OCR/` |
| Store/retrieve files (images, docs) | **AMCode.Storage** | `storagelibrary/AMCode.Storage/` |
| Generate PDF, DOCX, or XLSX files | **AMCode.Documents** | `documentlibrary/AMCode.Documents/` |
| Export recipes to Excel/CSV | **AMCode.Exports** | `exportslibrary/AMCode.Exports/` |
| Access MongoDB | **AMCode.Data** | `datalibrary/AMCode.Data/` |
| Manage API keys/secrets | **AMCode.Secrets** | `secretslibrary/AMCode.Secrets/` |
| Use web middleware (validation, logging) | **AMCode.Web** | `weblibrary/AMCode.Web/` |
| Write integration tests with Docker | **AMCode.Common.Testing** | `commontestinglibrary/AMCode.Common.Testing/` |
| Use shared Result<T> / utilities | **AMCode.Common** | `commonlibrary/AMCode.Common/` |

## Build Instructions

```bash
# Build a specific library
dotnet build ailibrary/AMCode.AI/AMCode.AI.csproj

# Build all libraries (from monorepo root)
dotnet build

# Run tests for a specific library
dotnet test ailibrary/AMCode.AI.Tests/AMCode.AI.Tests.csproj
```

## Dependency Graph

```
AMCode.Common ← (used by all)
  ├── AMCode.Columns
  ├── AMCode.Vertica.Client
  ├── AMCode.Data ← (AMCode.Columns, AMCode.Vertica.Client)
  ├── AMCode.AI ← (AMCode.Common)
  ├── AMCode.OCR ← (AMCode.Common)
  ├── AMCode.Storage ← (AMCode.Common)
  ├── AMCode.Documents ← (AMCode.Common)
  ├── AMCode.Exports ← (AMCode.Common, AMCode.Documents, AMCode.Storage, AMCode.Columns)
  ├── AMCode.Secrets ← (AMCode.Common)
  ├── AMCode.Web ← (AMCode.Common)
  └── AMCode.Common.Testing ← (AMCode.Common)
```

## Key Architectural Patterns

1. **Multi-cloud abstraction**: Each library abstracts 2+ cloud providers behind a unified interface
2. **Configuration-driven**: Provider selection via `appsettings.json`, not code changes
3. **DI extension methods**: Each library has `services.AddXxx(configuration)` registration
4. **GenericAIProvider base class**: All AI providers extend this abstract class
5. **Pipeline pattern**: `IAIPipeline<TInput, TOutput>` for composable AI tasks with config-driven provider selection

## Per-Module AGENTS.md

Each library directory contains its own `AGENTS.md` with detailed interface documentation, usage examples, and verification commands.
