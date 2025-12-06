# AMCode Library - Comprehensive Documentation Plan

**Created:** 2025-01-27
**Purpose:** Systematic documentation of all AMCode library modules for LLM consumption
**Status:** Planning Phase

---

## Executive Summary

This plan provides a systematic approach for LLMs to discover, document, and maintain comprehensive README files throughout the AMCode library ecosystem. The goal is to create a hierarchical documentation structure that enables fast context understanding without duplication.

### Objectives

1. **Complete Coverage**: Every library, module, and significant subfolder has documentation
2. **Hierarchical Structure**: Index READMEs point to detailed READMEs (no duplication)
3. **LLM-Optimized**: Documentation structured for efficient LLM consumption
4. **Maintainable**: Clear instructions for keeping documentation up-to-date
5. **Discoverable**: Easy navigation from root to specific classes/components

---

## Documentation Structure

### Hierarchy Levels

```
amcode-library/
├── README.md                          # Root index (points to library READMEs)
│
├── {library-name}/                    # Library directory
│   ├── README.md                      # Library index (points to subfolder READMEs)
│   ├── {subfolder}/                   # Subfolder (e.g., Providers/, Services/)
│   │   └── README.md                  # Subfolder details (classes, interfaces)
│   └── {library-name}.csproj          # Project file
│
└── .cursor/
    └── rules/
        └── documentation-maintenance.mdc  # Maintenance instructions
```

### Documentation Types

1. **Root Index README** (`amcode-library/README.md`)

   - Project overview
   - Library catalog with links
   - Technology stack summary
   - Quick start guide
   - Links to library READMEs

2. **Library Index README** (`{library}/README.md`)

   - Library purpose and scope
   - Architecture overview
   - Key features
   - Dependencies
   - Folder structure with links
   - Quick usage examples
   - Links to subfolder READMEs

3. **Subfolder README** (`{library}/{subfolder}/README.md`)
   - Subfolder purpose
   - Class/interface catalog
   - Key responsibilities
   - Usage patterns
   - Related components

---

## Phase 1: Root Documentation

### Task 1.1: Create Root Index README

**Location:** `amcode-library/README.md`

**Content Structure:**

- Project overview
- Technology stack (`.NET 8.0`, `.NET 9.0`)
- Library catalog (all 12+ libraries)
- Solution structure
- Build and test instructions
- Links to each library README

**Status:** ⏳ Pending

---

## Phase 2: Core Libraries Documentation

### Task 2.1: AMCode.Common

**Location:** `commonlibrary/AMCode.Common/README.md`

**Subfolders to Document:**

- `Components/` - 43 files
- `Scripts/` - Build scripts

**Status:** ⏳ Pending

### Task 2.2: AMCode.Columns

**Location:** `columnslibrary/AMCode.Columns/README.md`

**Subfolders to Document:**

- `Builder/` - Builder pattern
- `Core/` - Core components
- `DataTransform/` - Data transformation

**Status:** ⏳ Pending

### Task 2.3: AMCode.Vertica.Client

**Location:** `verticalibrary/AMCode.Vertica.Client/README.md`

**Status:** ⏳ Pending

### Task 2.4: AMCode.Common.Testing

**Location:** `commontestinglibrary/AMCode.Common.Testing/README.md`

**Subfolders to Document:**

- `Components/` - Testing utilities

**Status:** ⏳ Pending

---

## Phase 3: Data & Storage Libraries

### Task 3.1: AMCode.Data

**Location:** `datalibrary/AMCode.Data/README.md`

**Subfolders to Document:**

- MongoDB implementations
- ODBC implementations
- Repository patterns

**Status:** ⏳ Pending

### Task 3.2: AMCode.Storage

**Location:** `storagelibrary/AMCode.Storage/README.md`

**Subfolders to Document:**

- `Components/Storage/` - AzureBlobStorage, S3Storage, SimpleLocalStorage
- `Interfaces/` - Storage interfaces
- `Recipes/` - Recipe-specific storage

**Status:** ⏳ Pending

---

## Phase 4: SQL & Export Libraries

### Task 4.1: AMCode.Sql.Builder

**Location:** `sqlbuilderlibrary/AMCode.Sql.Builder/README.md`

**Subfolders to Document:**

- SQL builder components
- Query construction patterns

**Status:** ⏳ Pending

### Task 4.2: AMCode.Exports

**Location:** `exportslibrary/AMCode.Exports/README.md`

**Subfolders to Document:**

- `Components/Book/` - Excel, CSV book implementations
- `Components/BookBuilder/` - Builder patterns
- `Components/ExportBuilder/` - Export builders
- `Components/Results/` - Export results
- `Components/Zip/` - ZIP archive support
- `Recipes/` - Recipe export functionality

**Status:** ⏳ Pending

---

## Phase 5: Document Generation Libraries

### Task 5.1: AMCode.Documents

**Location:** `documentlibrary/AMCode.Documents/README.md`

**Subfolders to Document:**

- `Common/` - Shared models, enums, logging
- `Docx/` - DOCX generation (Domain, Infrastructure, Providers)
- `Pdf/` - PDF generation (Domain, Infrastructure, Providers)
- `Xlsx/` - Excel generation (Domain, Infrastructure, Providers)
- `Recipes/` - Recipe document generation
- `SyncfusionIo/Excel/` - Syncfusion Excel components

**Status:** ⏳ Pending

---

## Phase 6: AI & OCR Libraries

### Task 6.1: AMCode.OCR

**Location:** `ocrlibrary/AMCode.OCR/README.md`

**Subfolders to Document:**

- `Configurations/` - Provider configurations
- `Enums/` - OCR enums
- `Extensions/` - Service collection extensions
- `Factories/` - Provider factories
- `Models/` - OCR models (OCRResult, TextBlock, etc.)
- `Providers/` - Provider implementations (Google, Azure, AWS, PaddleOCR)
- `Services/` - OCR services (Smart selector, Cost analyzer, etc.)

**Status:** ⏳ Pending

### Task 6.2: AMCode.AI

**Location:** `ailibrary/AMCode.AI/README.md`

**Subfolders to Document:**

- `Configurations/` - AI provider configurations
- `Enums/` - AI enums
- `Extensions/` - Service collection extensions
- `Factories/` - Provider factories
- `Models/` - AI models (AICompletionRequest, ParsedRecipe, etc.)
- `Providers/` - Provider implementations (OpenAI, Anthropic, AWS Bedrock, Ollama, etc.)
- `Services/` - AI services (Smart selector, Recipe parser, Cost analyzer, etc.)

**Status:** ⏳ Pending

---

## Phase 7: Testing Projects

### Task 7.1: Test Project Documentation

**Locations:**

- `commonlibrary/AMCode.Common.UnitTests/README.md`
- `datalibrary/AMCode.Data.UnitTests/README.md`
- `datalibrary/AMCode.Data.SQLTests/README.md`
- `storagelibrary/AMCode.Storage.UnitTests/README.md`
- `sqlbuilderlibrary/AMCode.Sql.Builder.UnitTests/README.md`
- `sqlbuilderlibrary/AMCode.Sql.Builder.IntegrationTests/README.md`
- `exportslibrary/AMCode.Exports.UnitTests/README.md`
- `exportslibrary/AMCode.Exports.IntegrationTests/README.md`
- `documentlibrary/AMCode.Documents.UnitTests/README.md`
- `ocrlibrary/AMCode.OCR.Tests/README.md`
- `ailibrary/AMCode.AI.Tests/README.md`

**Status:** ⏳ Pending

---

## Phase 8: Special Projects

### Task 8.1: Phase 5B Testing

**Location:** `phase-5b-testing/AMCode.Phase5B.Testing/README.md`

**Status:** ⏳ Pending

---

## Documentation Templates

### Template 1: Root Index README

See: `DOCUMENTATION_TEMPLATES/ROOT_README_TEMPLATE.md`

### Template 2: Library Index README

See: `DOCUMENTATION_TEMPLATES/LIBRARY_README_TEMPLATE.md`

### Template 3: Subfolder README

See: `DOCUMENTATION_TEMPLATES/SUBFOLDER_README_TEMPLATE.md`

---

## Maintenance Instructions

### When to Update Documentation

1. **New Classes/Interfaces Added**: Update subfolder README
2. **New Subfolders Added**: Create new subfolder README, update library README
3. **New Libraries Added**: Create library README, update root README
4. **Architecture Changes**: Update all affected READMEs
5. **Dependency Changes**: Update library README dependencies section
6. **Breaking Changes**: Update library README migration notes

### Maintenance Workflow

1. **Code Changes**: Make code changes
2. **Update Documentation**: Update relevant README(s)
3. **Verify Links**: Ensure all links still work
4. **Check Hierarchy**: Ensure no duplication, proper linking

### Maintenance Rules File

**Location:** `.cursor/rules/documentation-maintenance.mdc`

This file contains mandatory rules for LLMs to follow when making code changes.

---

## Execution Plan for LLMs

### Step-by-Step Discovery Process

1. **Start with Root**: Read `AMCode.sln` and understand project structure
2. **For Each Library**:
   - Read `.csproj` file to understand dependencies
   - List all folders and files
   - Identify architecture patterns
   - Document key classes and interfaces
   - Create library README
3. **For Each Subfolder**:
   - List all classes/interfaces
   - Document responsibilities
   - Document usage patterns
   - Create subfolder README
4. **Link Everything**: Ensure proper linking from root → library → subfolder

### Discovery Checklist

For each library:

- [ ] Read `.csproj` file
- [ ] List all folders
- [ ] List all `.cs` files
- [ ] Identify public interfaces
- [ ] Identify key classes
- [ ] Document architecture pattern
- [ ] Document dependencies
- [ ] Create library README
- [ ] Create subfolder READMEs
- [ ] Update parent README with links

---

## Progress Tracking

### Status Legend

- ⏳ **Pending**: Not started
- 🔄 **In Progress**: Currently being documented
- ✅ **Complete**: Documentation finished and verified
- ⚠️ **Needs Update**: Documentation exists but needs refresh

### Current Status

| Library               | Root README | Library README | Subfolder READMEs | Status |
| --------------------- | ----------- | -------------- | ----------------- | ------ |
| Root                  | ⏳          | N/A            | N/A               | ⏳     |
| AMCode.Common         | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.Columns        | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.Vertica.Client | N/A         | ⏳             | N/A               | ⏳     |
| AMCode.Common.Testing | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.Data           | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.Storage        | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.Sql.Builder    | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.Exports        | N/A         | ✅             | ⏳                | 🔄     |
| AMCode.Documents      | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.OCR            | N/A         | ⏳             | ⏳                | ⏳     |
| AMCode.AI             | N/A         | ⏳             | ⏳                | ⏳     |

---

## Next Steps

1. **Create Documentation Templates** (Phase 0)
2. **Create Maintenance Rules** (Phase 0)
3. **Execute Phase 1**: Root documentation
4. **Execute Phase 2-8**: Library documentation (can be parallelized)
5. **Verify All Links**: Ensure proper linking
6. **Final Review**: Check for completeness and accuracy

---

**Last Updated:** 2025-01-27
**Maintained By:** Development Team
