# AMCode.Storage — Agent Guide

## What This Is

Cloud storage abstraction library (.NET 8) for Azure Blob Storage, AWS S3, and local filesystem. Includes recipe-specific image/document/export management.

## When to Use

- File upload/download to cloud storage
- Recipe image storage and retrieval
- Document and export file management
- Need local filesystem fallback for development

## When NOT to Use

- Database operations → `AMCode.Data`
- Document generation (PDF/DOCX) → `AMCode.Documents`
- Secret/credential storage → `AMCode.Secrets`

## Key Interfaces

| Interface | Purpose |
|-----------|---------|
| `ISimpleFileStorage` | Core: StoreFileAsync, GetFileAsync, DeleteFileAsync, ListFilesAsync |
| `IRecipeImageStorageService` | Recipe-specific: images, documents, exports |

## Implementations

- `AzureBlobStorage` — Azure Blob Storage
- `S3Storage` — AWS S3
- `SimpleLocalStorage` — Local filesystem (dev/testing)

## Configuration

```json
"AMCode": {
  "Storage": {
    "Provider": "AzureBlob",  // or "S3", "Local"
    "AzureBlob": { "ConnectionString": "...", "ContainerName": "recipes" },
    "S3": { "BucketName": "...", "Region": "us-east-1" }
  }
}
```

## Verification

```bash
cd repos/amcode-library
dotnet build storagelibrary/AMCode.Storage/AMCode.Storage.csproj
```
