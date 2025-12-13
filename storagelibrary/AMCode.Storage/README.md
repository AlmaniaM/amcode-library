# AMCode.Storage

**Version:** 1.1.2  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Multi-provider storage abstractions with recipe-specific storage capabilities

---

## Overview

AMCode.Storage provides a unified storage abstraction layer supporting multiple storage providers (Azure Blob Storage, AWS S3, and Local File System). The library implements Clean Architecture principles with a provider-agnostic interface, making it easy to switch between storage backends. It includes specialized recipe storage services for managing recipe images, documents, and exports.

## Architecture

The library follows Clean Architecture with a clear separation between interfaces, implementations, and domain-specific services. It uses the Strategy pattern to support multiple storage providers through a common interface (`ISimpleFileStorage`). The recipe-specific storage service (`RecipeImageStorageService`) builds on top of the base storage abstraction to provide domain-specific functionality.

### Key Components

- **Storage Interfaces**: Core contracts for file storage operations (ISimpleFileStorage, IStorageLogger)
- **Storage Providers**: Multiple implementations (AzureBlobStorage, S3Storage, SimpleLocalStorage)
- **Recipe Storage**: Domain-specific storage service for recipe images, documents, and exports
- **Logging**: Comprehensive logging support for storage operations

## Features

- Multi-provider support (Azure Blob Storage, AWS S3, Local File System)
- **Signed URL Generation**: Generate time-limited, read-only URLs for secure access
  - Azure: SAS tokens with `BlobSasBuilder`
  - S3: Presigned URLs with `GetPreSignedUrlRequest`
- Recipe-specific storage service for images, documents, and exports
- Comprehensive logging for all storage operations
- Result-based error handling using AMCode.Common.Results
- Async/await support for all operations
- File existence checking and directory listing
- Dependency injection ready
- Extensible architecture for custom storage providers

## Dependencies

### Internal Dependencies

- **AMCode.Common** - Common utilities and Result types

### External Dependencies

- **Azure.Storage.Blobs** (12.10.0) - Azure Blob Storage SDK
- **AWSSDK.S3** (3.7.400.50) - AWS S3 SDK
- **Microsoft.Extensions.Logging** (9.0.0) - Logging abstractions
- **Microsoft.Extensions.DependencyInjection** (9.0.0) - Dependency injection
- **Microsoft.Extensions.Configuration** (9.0.0) - Configuration support

## Project Structure

```
AMCode.Storage/
├── Interfaces/                    # Storage interfaces
│   ├── README.md                 # [Interfaces Documentation](Interfaces/README.md)
│   ├── ISimpleFileStorage.cs     # Main storage interface
│   ├── IStorageLogger.cs         # Logging interface
│   ├── IStorageMetricsLogger.cs  # Metrics logging interface
│   └── IStorageOperationLogger.cs # Operation logging interface
├── Components/                    # Storage implementations
│   └── Storage/
│       ├── README.md             # [Storage Providers Documentation](Components/Storage/README.md)
│       ├── AzureBlobStorage.cs   # Azure Blob Storage implementation
│       ├── S3Storage.cs          # AWS S3 implementation
│       └── SimpleLocalStorage.cs # Local file system implementation
├── Recipes/                       # Recipe-specific storage
│   ├── README.md                 # [Recipe Storage Documentation](Recipes/README.md)
│   ├── Interfaces/
│   │   └── IRecipeImageStorageService.cs # Recipe storage interface
│   ├── Models/
│   │   ├── RecipeImageType.cs    # Image type enumeration
│   │   ├── RecipeDocumentType.cs # Document type enumeration
│   │   ├── RecipeExportType.cs   # Export type enumeration
│   │   └── RecipeImageStorageOptions.cs # Configuration options
│   ├── RecipeImageStorageService.cs # Recipe storage implementation
│   └── Extensions/
│       └── RecipeStorageServiceCollectionExtensions.cs # DI extensions
├── Docs/
│   └── LoggingBestPractices.md   # Logging documentation
└── AMCode.Storage.csproj         # Project file
```

## Key Interfaces

### ISimpleFileStorage

**Location:** `Interfaces/ISimpleFileStorage.cs`

**Purpose:** Core interface for file storage operations, providing a unified API across all storage providers.

**Key Methods:**
- `StoreFileAsync(Stream, string, string)` - Stores a file and returns the path
- `GetFileAsync(string)` - Retrieves a file stream
- `DeleteFileAsync(string)` - Deletes a file
- `FileExistsAsync(string)` - Checks if a file exists
- `ListFilesAsync(string)` - Lists files in a directory

**See Also:** [Interfaces README](Interfaces/README.md)

### IRecipeImageStorageService

**Location:** `Recipes/Interfaces/IRecipeImageStorageService.cs`

**Purpose:** Domain-specific interface for recipe-related storage operations, including images, documents, and exports.

**Key Methods:**
- `StoreRecipeImageAsync(string, Stream, string, RecipeImageType)` - Stores recipe images
- `GetRecipeImageAsync(string, RecipeImageType)` - Retrieves recipe images
- `DeleteRecipeImageAsync(string, RecipeImageType)` - Deletes recipe images
- `ListRecipeImagesAsync(string)` - Lists all images for a recipe
- `StoreRecipeDocumentAsync(string, Stream, string, RecipeDocumentType)` - Stores recipe documents
- `StoreRecipeExportAsync(string, Stream, string, RecipeExportType)` - Stores recipe exports

**See Also:** [Recipes README](Recipes/README.md)

### IStorageLogger

**Location:** `Interfaces/IStorageLogger.cs`

**Purpose:** Logging interface for storage operations, providing structured logging capabilities.

**Key Methods:**
- `LogInformation(string, params object[])` - Logs informational messages
- `LogWarning(string, params object[])` - Logs warning messages
- `LogError(string, params object[])` - Logs error messages
- `LogError(Exception, string, params object[])` - Logs errors with exceptions
- `LogDebug(string, params object[])` - Logs debug messages
- `LogTrace(string, params object[])` - Logs trace messages

**See Also:** [Interfaces README](Interfaces/README.md)

## Key Classes

### AzureBlobStorage

**Location:** `Components/Storage/AzureBlobStorage.cs`

**Purpose:** Azure Blob Storage implementation of ISimpleFileStorage.

**Key Responsibilities:**
- Store, retrieve, delete files in Azure Blob Storage
- Manage blob containers
- Generate blob URLs
- Handle Azure-specific errors and exceptions

**Key Features:**
- Automatic container creation
- Path normalization for blob storage
- Account name extraction from connection strings
- Comprehensive error handling
- Signed URL generation with SAS tokens

**Signed URL Generation:**

Generate SAS tokens for secure, time-limited access:

```csharp
var signedUrl = azureStorage.GenerateSignedBlobUrl("images/photo.jpg", expiryMinutes: 15);
// Returns: https://account.blob.core.windows.net/container/images/photo.jpg?sv=...&sp=r&sig=...
```

**Security Properties**:
- Read-only permission
- Single blob scope
- Configurable expiration
- Account key credentials

**See Also:** [Storage Providers README](Components/Storage/README.md)

### S3Storage

**Location:** `Components/Storage/S3Storage.cs`

**Purpose:** AWS S3 implementation of ISimpleFileStorage.

**Key Responsibilities:**
- Store, retrieve, delete files in AWS S3
- Manage S3 buckets
- Handle S3-specific operations
- Manage S3 access permissions
- Presigned URL generation

**Presigned URL Generation:**

Generate presigned URLs for secure, time-limited access:

```csharp
var presignedUrl = s3Storage.GeneratePresignedUrl("images/photo.jpg", expiryMinutes: 15);
// Returns: https://bucket.s3.region.amazonaws.com/images/photo.jpg?X-Amz-Algorithm=...
```

**Security Properties**:
- GET-only permission
- Single object scope
- Configurable expiration
- Access key credentials

**See Also:** [Storage Providers README](Components/Storage/README.md)

### SimpleLocalStorage

**Location:** `Components/Storage/SimpleLocalStorage.cs`

**Purpose:** Local file system implementation of ISimpleFileStorage for development and testing.

**Key Responsibilities:**
- Store, retrieve, delete files on local file system
- Manage directory structure
- Create directories as needed
- Provide local file path access

**Key Features:**
- Configurable base path
- Automatic directory creation
- File system error handling

**See Also:** [Storage Providers README](Components/Storage/README.md)

### RecipeImageStorageService

**Location:** `Recipes/RecipeImageStorageService.cs`

**Purpose:** Service for managing recipe-specific storage operations including images, documents, and exports.

**Key Responsibilities:**
- Manage recipe image storage with type support (main, thumbnail, etc.)
- Store and retrieve recipe documents (PDF, DOCX)
- Store and retrieve recipe exports (Excel, CSV, PDF)
- Organize files by recipe ID and type
- Apply storage options and validation

**Key Features:**
- Recipe ID-based organization
- Support for multiple image types
- Document and export type management
- Configurable storage options
- Comprehensive error handling

**See Also:** [Recipes README](Recipes/README.md)

## Usage Examples

### Basic Usage - Azure Blob Storage

```csharp
using AMCode.Storage.Interfaces;
using AMCode.Storage.Components.Storage;
using AMCode.Storage.Interfaces;
using System.IO;

// Create storage instance
var logger = new StorageLogger(); // Implement IStorageLogger
var storage = new AzureBlobStorage(
    connectionString: "DefaultEndpointsProtocol=https;AccountName=...",
    containerName: "my-container",
    logger: logger
);

// Store a file
using (var fileStream = File.OpenRead("image.jpg"))
{
    var result = await storage.StoreFileAsync(fileStream, "image.jpg", "uploads");
    if (result.IsSuccess)
    {
        Console.WriteLine($"File stored at: {result.Value}");
    }
}

// Retrieve a file
var getResult = await storage.GetFileAsync("uploads/image.jpg");
if (getResult.IsSuccess)
{
    // Use the stream
    using (var stream = getResult.Value)
    {
        // Process file
    }
}
```

### Basic Usage - Local Storage

```csharp
using AMCode.Storage.Interfaces;
using AMCode.Storage.Components.Storage;

// Create local storage instance
var logger = new StorageLogger();
var storage = new SimpleLocalStorage(logger, basePath: "C:\\MyApp\\Storage");

// Store a file
using (var fileStream = File.OpenRead("document.pdf"))
{
    var result = await storage.StoreFileAsync(fileStream, "document.pdf", "documents");
    if (result.IsSuccess)
    {
        Console.WriteLine($"File stored at: {result.Value}");
    }
}
```

### Recipe Storage Usage

```csharp
using AMCode.Storage.Recipes;
using AMCode.Storage.Recipes.Interfaces;
using AMCode.Storage.Recipes.Models;
using AMCode.Storage.Components.Storage;
using AMCode.Storage.Interfaces;

// Setup storage
var fileStorage = new AzureBlobStorage(connectionString, containerName, logger);
var options = new RecipeImageStorageOptions
{
    BasePath = "recipe-storage",
    MaxImageSizeMB = 10,
    GenerateThumbnails = true
};

var recipeStorage = new RecipeImageStorageService(fileStorage, logger, options);

// Store recipe image
using (var imageStream = File.OpenRead("recipe-photo.jpg"))
{
    var result = await recipeStorage.StoreRecipeImageAsync(
        recipeId: "recipe-123",
        imageStream: imageStream,
        fileName: "recipe-photo.jpg",
        imageType: RecipeImageType.Main
    );
    
    if (result.IsSuccess)
    {
        Console.WriteLine($"Recipe image stored at: {result.Value}");
    }
}

// Retrieve recipe image
var imageResult = await recipeStorage.GetRecipeImageAsync("recipe-123", RecipeImageType.Main);
if (imageResult.IsSuccess)
{
    using (var stream = imageResult.Value)
    {
        // Use the image stream
    }
}

// Store recipe document
using (var docStream = File.OpenRead("recipe.pdf"))
{
    var docResult = await recipeStorage.StoreRecipeDocumentAsync(
        recipeId: "recipe-123",
        documentStream: docStream,
        fileName: "recipe.pdf",
        documentType: RecipeDocumentType.PDF
    );
}
```

## Configuration

### appsettings.json Example

```json
{
  "Storage": {
    "Provider": "AzureBlob",
    "AzureBlob": {
      "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=...",
      "ContainerName": "my-container"
    },
    "S3": {
      "BucketName": "my-bucket",
      "Region": "us-east-1",
      "AccessKeyId": "...",
      "SecretAccessKey": "..."
    },
    "Local": {
      "BasePath": "C:\\MyApp\\Storage"
    },
    "RecipeStorage": {
      "BasePath": "recipe-storage",
      "MaxImageSizeMB": 10,
      "MaxDocumentSizeMB": 50,
      "GenerateThumbnails": true,
      "ThumbnailWidth": 300,
      "ThumbnailHeight": 300
    }
  }
}
```

### Dependency Injection Setup

```csharp
using AMCode.Storage.Interfaces;
using AMCode.Storage.Components.Storage;
using AMCode.Storage.Recipes;
using AMCode.Storage.Recipes.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Register storage services
services.AddScoped<IStorageLogger, StorageLogger>(); // Implement IStorageLogger

// Register storage provider
var storageConfig = configuration.GetSection("Storage");
if (storageConfig["Provider"] == "AzureBlob")
{
    var connectionString = storageConfig["AzureBlob:ConnectionString"];
    var containerName = storageConfig["AzureBlob:ContainerName"];
    services.AddScoped<ISimpleFileStorage>(sp =>
    {
        var logger = sp.GetRequiredService<IStorageLogger>();
        return new AzureBlobStorage(connectionString, containerName, logger);
    });
}
else if (storageConfig["Provider"] == "S3")
{
    // Register S3Storage
}
else
{
    // Register SimpleLocalStorage
}

// Register recipe storage
services.AddRecipeStorage(options =>
{
    options.BasePath = storageConfig["RecipeStorage:BasePath"];
    options.MaxImageSizeMB = int.Parse(storageConfig["RecipeStorage:MaxImageSizeMB"]);
    options.GenerateThumbnails = bool.Parse(storageConfig["RecipeStorage:GenerateThumbnails"]);
});
```

## Testing

### Test Projects

- **AMCode.Storage.UnitTests**: Unit tests for storage implementations
  - Tests for AzureBlobStorage, S3Storage, SimpleLocalStorage
  - Tests for RecipeImageStorageService
  - Mock-based testing with IStorageLogger

### Running Tests

```bash
dotnet test storagelibrary/AMCode.Storage.UnitTests
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Interfaces](Interfaces/README.md) - Storage interfaces (ISimpleFileStorage, IStorageLogger, etc.)
- [Storage Providers](Components/Storage/README.md) - Storage implementations (AzureBlobStorage, S3Storage, SimpleLocalStorage)
- [Recipe Storage](Recipes/README.md) - Recipe-specific storage service and models

## Related Libraries

- [AMCode.Common](../commonlibrary/AMCode.Common/README.md) - Common utilities and Result types used by this library
- [AMCode.Exports](../../exportslibrary/AMCode.Exports/README.md) - Export functionality that may use storage services

## Migration Notes

### Version 1.1.2

- Added recipe-specific storage features
- Added RecipeImageStorageService for managing recipe images, documents, and exports
- Added RecipeImageStorageOptions for configuration
- Enhanced logging support

### Upgrading from Previous Versions

No breaking changes. The recipe storage features are additive and don't affect existing storage provider implementations.

## Known Issues

None currently documented.

## Future Considerations

- Additional storage providers (Google Cloud Storage, etc.)
- File versioning support
- Automatic file compression
- Image thumbnail generation
- File metadata support
- Storage metrics and monitoring

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy
- [Logging Best Practices](Docs/LoggingBestPractices.md) - Logging guidelines

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

