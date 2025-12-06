# AMCode.Common

**Version:** 1.0.0  
**Target Framework:** .NET 8.0  
**Last Updated:** 2025-01-27  
**Purpose:** Common utilities and components library providing foundational functionality for the AMCode ecosystem

---

## Overview

AMCode.Common is a core utility library that provides essential components and helper classes used throughout the AMCode ecosystem. It includes file I/O operations (CSV, JSON, ZIP), email functionality, filtering capabilities, extension methods, dynamic object helpers, and common models. This library serves as the foundation for other AMCode libraries and applications.

## Architecture

AMCode.Common follows a component-based architecture organized by functionality. Each component is self-contained with clear interfaces and implementations. The library emphasizes:

- **Interface-Based Design**: Core functionality exposed through interfaces (ICSVReader, ICSVWriter, IEmailClient, IZipArchive, etc.)
- **Extension Methods**: Rich set of extension methods for common types (String, Enumerable, Dictionary, Object, etc.)
- **Result Pattern**: Type-safe result handling with `Result<T>` and `Result` classes
- **Separation of Concerns**: Components organized by domain (IO, Email, Filter, Extensions, etc.)

### Key Components

- **IO Components**: CSV reading/writing, JSON file operations, ZIP archive management, path utilities
- **Email Components**: Email client interface and MailGun implementation
- **Filter Components**: Filter structures for data filtering operations
- **Extension Methods**: Utility extensions for strings, collections, objects, types, and more
- **Dynamic Object Helpers**: Utilities for working with ExpandoObject and dynamic types
- **Common Models**: Result pattern implementation, cloneable interface

## Features

- **CSV Processing**: Read and write CSV files with flexible delimiter support, quote handling, and type conversion
- **JSON File Operations**: Read and write JSON files with Newtonsoft.Json
- **ZIP Archive Management**: Create and manage ZIP archives with stream support
- **Email Functionality**: Email client interface with MailGun implementation
- **Filter Structures**: Flexible filtering system with filter names, items, and validation
- **Extension Methods**: Comprehensive extension methods for common .NET types
- **Dynamic Object Support**: Helpers for working with ExpandoObject and dynamic types
- **Result Pattern**: Type-safe result handling for operations that can succeed or fail
- **Path Utilities**: File and directory path manipulation utilities
- **Reflection Utilities**: Method info and type extension utilities

## Dependencies

### Internal Dependencies

- None (this is a foundational library)

### External Dependencies

- **Microsoft.CSharp** (4.7.0) - Dynamic language runtime support
- **Newtonsoft.Json** (13.0.1) - JSON serialization and deserialization

## Project Structure

```
AMCode.Common/
├── Components/              # Core component implementations
│   ├── Common/             # Common models and interfaces
│   │   └── Models/         # Result<T>, ICloneable
│   ├── Dynamic/            # Dynamic object helpers
│   ├── Email/              # Email functionality
│   │   ├── IEmailClient.cs
│   │   └── MailGunClient.cs
│   ├── Extensions/         # Extension methods
│   │   ├── StringExtensions.cs
│   │   ├── EnumerableExtensions.cs
│   │   ├── DictionaryExtensions.cs
│   │   ├── ObjectExtensions.cs
│   │   ├── TypeExtensions.cs
│   │   └── [More extensions...]
│   ├── Filter/             # Filter structures
│   │   ├── Filter.cs
│   │   ├── FilterItem.cs
│   │   ├── FilterName.cs
│   │   └── Models/         # IFilter, IFilterItem, IFilterName
│   ├── IO/                 # Input/Output operations
│   │   ├── CSV/            # CSV reading and writing
│   │   │   ├── CSVReader.cs
│   │   │   ├── CSVWriter.cs
│   │   │   ├── Models/     # ICSVReader, ICSVWriter, QuoteOptions
│   │   │   └── Exceptions/ # DelimiterNotProvidedException
│   │   ├── JSON/           # JSON file operations
│   │   │   ├── JsonFileReader.cs
│   │   │   └── JsonFileWriter.cs
│   │   ├── Zip/            # ZIP archive operations
│   │   │   ├── ZipArchive.cs
│   │   │   └── Models/     # IZipArchive, IZipEntry, CompressionLevel
│   │   ├── PathUtils.cs
│   │   └── TextFieldParser.cs
│   ├── Util/               # Utility classes
│   │   ├── Exceptions/
│   │   ├── Reflection/
│   │   └── System/
│   └── Xlsx/               # Excel-related utilities
│       └── ExcelHAlign.cs
├── Scripts/                # Build scripts
└── AMCode.Common.csproj    # Project file
```

## Components

The library is organized into functional components, each with comprehensive documentation:

- **[Common](Components/Common/README.md)** - Common models and interfaces including Result pattern and ICloneable
- **[Email](Components/Email/README.md)** - Email client interface and Mailgun implementation
- **[Extensions](Components/Extensions/README.md)** - Extension methods for strings, collections, objects, types, and more
- **[Filter](Components/Filter/README.md)** - Filter structures for data filtering operations
- **[IO](Components/IO/README.md)** - File I/O operations including CSV, JSON, ZIP, and path utilities

## Key Interfaces

### IEmailClient

**Location:** `Components/Email/IEmailClient.cs`

**Purpose:** Interface for sending email messages asynchronously

**Key Methods:**

- `SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default)` - Send an email message

**See Also:** [Email Component Documentation](Components/Email/README.md)

### ICSVReader

**Location:** `Components/IO/CSV/Models/ICSVReader.cs`

**Purpose:** Interface for reading CSV files with various output formats

**Key Methods:**

- `GetList<T>(string filePath, string delimiter)` - Read CSV into strongly-typed list
- `GetExpandoList(string filePath, string delimiter)` - Read CSV into ExpandoObject list
- `GetColumnValues(string filePath, string columnName, string delimiter)` - Get values from specific column
- `GetHeaders(string filePath)` - Get CSV headers

**See Also:** [IO Component Documentation](Components/IO/README.md)

### ICSVWriter

**Location:** `Components/IO/CSV/Models/ICSVWriter.cs`

**Purpose:** Interface for writing data to CSV files

**Key Methods:**

- `WriteList<T>(string filePath, IList<T> list, string delimiter)` - Write strongly-typed list to CSV
- `WriteExpandoList(string filePath, IList<ExpandoObject> list, string delimiter)` - Write ExpandoObject list to CSV

**See Also:** [IO Component Documentation](Components/IO/README.md)

### IZipArchive

**Location:** `Components/IO/Zip/Models/IZipArchive.cs`

**Purpose:** Interface for creating and managing ZIP archives

**Key Methods:**

- `CreateZipAsync(Stream outputStream, CancellationToken cancellationToken = default)` - Create ZIP archive from entries
- `AddEntry(IZipEntry entry)` - Add entry to archive

**See Also:** [IO Component Documentation](Components/IO/README.md)

### IFilter

**Location:** `Components/Filter/Models/IFilter.cs`

**Purpose:** Interface representing a filter structure with name, items, and validation

**Key Properties:**

- `IFilterName FilterName` - The name of the filter
- `IList<IFilterItem> FilterItems` - List of filter items
- `IFilterName FilterIdName` - ID filter counterpart name
- `bool Required` - Whether filter is required

**See Also:** [Filter Component Documentation](Components/Filter/README.md)

## Key Classes

### Result<T> / Result

**Location:** `Components/Common/Models/Result.cs`

**Purpose:** Type-safe result pattern implementation for operations that can succeed or fail

**Key Responsibilities:**

- Represent operation success or failure
- Provide value on success or error message on failure
- Support both generic (Result<T>) and non-generic (Result) variants

**Usage:**

```csharp
var result = Result<string>.Success("Operation completed");
if (result.IsSuccess)
{
    var value = result.Value;
}

var failure = Result.Failure("Operation failed");
if (failure.IsFailure)
{
    var error = failure.Error;
}
```

**See Also:** [Common Component Documentation](Components/Common/README.md)

### CSVReader

**Location:** `Components/IO/CSV/CSVReader.cs`

**Purpose:** Read CSV files into various formats (strongly-typed lists, ExpandoObject lists, column values)

**Key Responsibilities:**

- Read CSV files from file paths or streams
- Support custom delimiters and quote handling
- Convert CSV data to strongly-typed objects or dynamic objects
- Extract column values and headers

**See Also:** [CSV Components](Components/IO/CSV/README.md)

### CSVWriter

**Location:** `Components/IO/CSV/CSVWriter.cs`

**Purpose:** Write data to CSV files

**Key Responsibilities:**

- Write strongly-typed lists to CSV
- Write ExpandoObject lists to CSV
- Support custom delimiters and quote options

**See Also:** [CSV Components](Components/IO/CSV/README.md)

### ZipArchive

**Location:** `Components/IO/Zip/ZipArchive.cs`

**Purpose:** Create and manage ZIP archives from streams

**Key Responsibilities:**

- Add ZIP entries from streams
- Create ZIP archives asynchronously
- Support configurable buffer sizes for stream copying

**See Also:** [ZIP Components](Components/IO/Zip/README.md)

### Filter

**Location:** `Components/Filter/Filter.cs`

**Purpose:** Represent filter structures with name, items, and validation rules

**Key Responsibilities:**

- Define filter names and ID names
- Manage filter items
- Indicate required/optional status

**See Also:** [Filter Components](Components/Filter/README.md)

## Usage Examples

### Basic Usage - CSV Reading

```csharp
using AMCode.Common.IO.CSV;

// Read CSV into strongly-typed list
var reader = new CSVReader();
var users = reader.GetList<User>("users.csv", ",");

// Read CSV into dynamic objects
var data = reader.GetExpandoList("data.csv", ",");

// Get specific column values
var emails = reader.GetColumnValues("users.csv", "Email", ",");
```

### Basic Usage - CSV Writing

```csharp
using AMCode.Common.IO.CSV;

var writer = new CSVWriter();
var users = new List<User> { /* ... */ };

// Write strongly-typed list to CSV
writer.WriteList("users.csv", users, ",");

// Write dynamic objects to CSV
var data = new List<ExpandoObject> { /* ... */ };
writer.WriteExpandoList("data.csv", data, ",");
```

### Basic Usage - ZIP Archives

```csharp
using AMCode.Common.IO.Zip;
using AMCode.Common.IO.Zip.Models;

var archive = new ZipArchive();
archive.AddEntry(new ZipEntry("file1.txt", stream1));
archive.AddEntry(new ZipEntry("file2.txt", stream2));

using (var outputStream = File.Create("archive.zip"))
{
    await archive.CreateZipAsync(outputStream);
}
```

### Basic Usage - Email

```csharp
using AMCode.Common.Email;
using System.Net.Mail;

IEmailClient emailClient = new MailGunClient(/* configuration */);
var message = new MailMessage("from@example.com", "to@example.com", "Subject", "Body");
await emailClient.SendMessageAsync(message);
```

### Basic Usage - Result Pattern

```csharp
using AMCode.Common.Models;

// Success case
var result = Result<string>.Success("Data retrieved successfully");
if (result.IsSuccess)
{
    Console.WriteLine(result.Value);
}

// Failure case
var errorResult = Result<int>.Failure("Failed to parse number");
if (errorResult.IsFailure)
{
    Console.WriteLine(errorResult.Error);
}

// Non-generic result
var operationResult = Result.Success();
if (operationResult.IsSuccess)
{
    // Operation completed successfully
}
```

### Extension Methods

```csharp
using AMCode.Common.Extensions.Strings;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.Objects;

// String extensions
string text = "  ";
if (text.IsNullEmptyOrWhiteSpace())
{
    // Handle empty string
}

bool equal = "Hello".EqualsIgnoreCase("HELLO"); // true

// Enumerable extensions
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var filtered = numbers.Filter(/* filter logic */);

// Object extensions
var obj = new MyClass();
var cloned = obj.Clone(); // If implements ICloneable
```

### Filter Structures

```csharp
using AMCode.Common.FilterStructures;

var filter = new Filter
{
    FilterName = new FilterName { Name = "StatusFilter" },
    FilterItems = new List<IFilterItem>
    {
        new FilterItem { Value = "Active" },
        new FilterItem { Value = "Pending" }
    },
    Required = true
};
```

## Configuration

AMCode.Common does not require configuration files. Components are configured through constructor parameters or method arguments.

### Dependency Injection Setup

```csharp
using AMCode.Common.Email;
using Microsoft.Extensions.DependencyInjection;

services.AddScoped<IEmailClient, MailGunClient>(provider =>
{
    // Configure MailGun client
    return new MailGunClient(/* configuration */);
});
```

## Testing

### Test Projects

- **AMCode.Common.UnitTests**: Comprehensive unit tests for all components
  - CSV reading/writing tests
  - JSON file operation tests
  - ZIP archive tests
  - Extension method tests
  - Filter structure tests
  - [Test Project README](../AMCode.Common.UnitTests/README.md)

### Running Tests

```bash
dotnet test commonlibrary/AMCode.Common.UnitTests/AMCode.Common.UnitTests.csproj
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Components/Common](Components/Common/README.md) - Common models and interfaces (Result<T>, ICloneable)
- [Components/Dynamic](Components/Dynamic/README.md) - Dynamic object helpers
- [Components/Email](Components/Email/README.md) - Email functionality
- [Components/Extensions](Components/Extensions/README.md) - Extension methods
- [Components/Filter](Components/Filter/README.md) - Filter structures
- [Components/IO](Components/IO/README.md) - Input/Output operations (CSV, JSON, ZIP)

## Related Libraries

- [AMCode.Columns](../columnslibrary/AMCode.Columns/README.md) - Uses AMCode.Common for utilities
- [AMCode.Data](../../datalibrary/AMCode.Data/README.md) - Uses AMCode.Common for common operations
- [AMCode.Storage](../../storagelibrary/AMCode.Storage/README.md) - Uses AMCode.Common for utilities
- [AMCode.Exports](../../exportslibrary/AMCode.Exports/README.md) - Uses AMCode.Common for CSV and ZIP operations

## Migration Notes

- **Version 1.0.0**: Initial release with core functionality
- All components follow interface-based design for easy testing and mocking
- Extension methods are organized by target type namespace

## Known Issues

None currently documented.

## Future Considerations

- Additional email provider implementations
- Enhanced CSV parsing options
- Additional extension methods for common scenarios
- Performance optimizations for large file operations

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
