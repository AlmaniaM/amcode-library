# IO

**Location:** `AMCode.Common/Components/IO/`  
**Last Updated:** 2025-01-27  
**Purpose:** File I/O operations including CSV reading/writing, JSON file operations, ZIP archive management, and path utilities

---

## Overview

The IO component provides comprehensive file input/output operations for the AMCode.Common library. It includes specialized readers and writers for CSV and JSON formats, ZIP archive creation and management, path manipulation utilities, and text field parsing capabilities. All operations support both file paths and streams, making the component flexible for various I/O scenarios.

## Responsibilities

- **CSV Processing**: Read and write CSV files with flexible delimiter support, quote handling, and type conversion
- **JSON File Operations**: Read and write JSON files with Newtonsoft.Json integration
- **ZIP Archive Management**: Create and manage ZIP archives with stream support and compression levels
- **Path Utilities**: File and directory path manipulation and combination utilities
- **Text Field Parsing**: Parse delimited text fields from files and streams

## Class Catalog

### Interfaces

#### ICSVReader

**File:** `CSV/Models/ICSVReader.cs`

**Purpose:** Interface for reading CSV files into various formats including lists of strings, ExpandoObjects, and strongly-typed objects.

**Key Members:**
```csharp
public interface ICSVReader
{
    IList<string> GetColumnValues(string filePath, string columnName);
    IList<string> GetColumnValues(Stream stream, string columnName);
    IList<string> GetColumnValues(string filePath, string columnName, string delimiter);
    IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes);
    IList<T> GetList<T>(string filePath, string delimiter) where T : new();
    IList<string> GetHeaders(string filePath);
}
```

**Usage:**
```csharp
ICSVReader reader = new CSVReader();
var data = reader.GetList<MyModel>("data.csv", ",");
var headers = reader.GetHeaders("data.csv");
```

**Implementations:**
- [CSVReader](#csvreader) - Main CSV reading implementation

---

#### ICSVWriter

**File:** `CSV/Models/ICSVWriter.cs`

**Purpose:** Interface for writing data to CSV files with flexible formatting options.

**Key Members:**
```csharp
public interface ICSVWriter
{
    void Write<T>(string filePath, IList<T> data);
    void Write<T>(Stream stream, IList<T> data);
    void Write<T>(string filePath, IList<T> data, string delimiter);
    void Write<T>(Stream stream, IList<T> data, string delimiter);
}
```

**Usage:**
```csharp
ICSVWriter writer = new CSVWriter();
var data = new List<MyModel> { /* ... */ };
writer.Write("output.csv", data, ",");
```

**Implementations:**
- [CSVWriter](#csvwriter) - Main CSV writing implementation

---

#### IZipArchive

**File:** `Zip/Models/IZipArchive.cs`

**Purpose:** Interface for creating and managing ZIP archives with support for multiple entries, compression levels, and stream operations.

**Key Members:**
```csharp
public interface IZipArchive
{
    CompressionLevel Compression { get; set; }
    IList<IZipEntry> ZipEntries { get; set; }
    void AddEntry(IZipEntry zipEntry);
    void AddEntry(string name, Stream data);
    IZipArchiveResult CreateZip(string fileName);
    Task<IZipArchiveResult> CreateZipAsync(string fileName, CancellationToken cancellationToken = default);
}
```

**Usage:**
```csharp
IZipArchive archive = new ZipArchive();
archive.AddEntry("file1.txt", stream1);
archive.AddEntry("file2.txt", stream2);
var result = archive.CreateZip("archive.zip");
```

**Implementations:**
- [ZipArchive](#ziparchive) - Main ZIP archive implementation

---

### Classes

#### CSVReader

**File:** `CSV/CSVReader.cs`

**Purpose:** Reads CSV files and converts them into various formats including lists of strings, ExpandoObjects, and strongly-typed objects. Supports custom delimiters, quote handling, and column mapping.

**Key Responsibilities:**
- Read CSV files from file paths or streams
- Convert CSV rows to ExpandoObjects or strongly-typed objects
- Extract column values and headers
- Support custom delimiters and quote handling
- Provide column name mapping and case-insensitive matching

**Key Members:**
```csharp
public class CSVReader : ICSVReader
{
    public CSVReader();
    public CSVReader(bool hasColumns);
    public CSVReader(bool hasColumns, Func<int, string> getColumnName);
    public IList<string> GetColumnValues(string filePath, string columnName);
    public IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes);
    public IList<T> GetList<T>(string filePath, string delimiter) where T : new();
    public IList<string> GetHeaders(string filePath);
}
```

**Usage:**
```csharp
var reader = new CSVReader(hasColumns: true);
var data = reader.GetList<Person>("people.csv", ",");
var headers = reader.GetHeaders("people.csv");
```

**Dependencies:**
- `AMCode.Common.Extensions.ExpandoObjects` - For ExpandoObject extensions
- `AMCode.Common.Util` - For utility functions

**Related Components:**
- [CSVWriter](#csvwriter) - For writing CSV files
- [ICSVReader](#icsvreader) - Interface definition

---

#### CSVWriter

**File:** `CSV/CSVWriter.cs`

**Purpose:** Writes data collections to CSV files with support for custom delimiters and quote options.

**Key Responsibilities:**
- Write strongly-typed objects to CSV files
- Support custom delimiters and quote handling
- Write to file paths or streams
- Handle property mapping and serialization

**Key Members:**
```csharp
public class CSVWriter : ICSVWriter
{
    public CSVWriter();
    public void Write<T>(string filePath, IList<T> data);
    public void Write<T>(Stream stream, IList<T> data, string delimiter);
}
```

**Usage:**
```csharp
var writer = new CSVWriter();
var people = new List<Person> { /* ... */ };
writer.Write("output.csv", people, ",");
```

**Dependencies:**
- Reflection for property access

**Related Components:**
- [CSVReader](#csvreader) - For reading CSV files
- [ICSVWriter](#icsvwriter) - Interface definition

---

#### JsonFileReader

**File:** `JSON/JsonFileReader.cs`

**Purpose:** Reads JSON files and deserializes them into strongly-typed objects using Newtonsoft.Json.

**Key Responsibilities:**
- Read JSON files from file paths
- Deserialize JSON content to strongly-typed objects
- Support synchronous and asynchronous operations
- Handle null values and error cases

**Key Members:**
```csharp
public class JsonFileReader
{
    public static T Read<T>(string filePath);
    public static async Task<T> ReadAsync<T>(string filePath);
}
```

**Usage:**
```csharp
var config = JsonFileReader.Read<AppConfig>("config.json");
var data = await JsonFileReader.ReadAsync<MyModel>("data.json");
```

**Dependencies:**
- `Newtonsoft.Json` - For JSON deserialization

**Related Components:**
- [JsonFileWriter](#jsonfilewriter) - For writing JSON files

---

#### JsonFileWriter

**File:** `JSON/JsonFileWriter.cs`

**Purpose:** Writes objects to JSON files using Newtonsoft.Json serialization.

**Key Responsibilities:**
- Serialize objects to JSON format
- Write JSON to file paths
- Support synchronous and asynchronous operations
- Handle formatting and indentation

**Key Members:**
```csharp
public class JsonFileWriter
{
    public static void Write<T>(string filePath, T obj);
    public static async Task WriteAsync<T>(string filePath, T obj);
}
```

**Usage:**
```csharp
var config = new AppConfig { /* ... */ };
JsonFileWriter.Write("config.json", config);
await JsonFileWriter.WriteAsync("data.json", myObject);
```

**Dependencies:**
- `Newtonsoft.Json` - For JSON serialization

**Related Components:**
- [JsonFileReader](#jsonfilereader) - For reading JSON files

---

#### ZipArchive

**File:** `Zip/ZipArchive.cs`

**Purpose:** Creates and manages ZIP archives with support for multiple entries, compression levels, and stream operations.

**Key Responsibilities:**
- Add entries to ZIP archives (files, streams, or async data sources)
- Create ZIP files with configurable compression levels
- Support synchronous and asynchronous ZIP creation
- Manage ZIP entry collections

**Key Members:**
```csharp
public class ZipArchive : IZipArchive
{
    public CompressionLevel Compression { get; set; }
    public IList<IZipEntry> ZipEntries { get; set; }
    public void AddEntry(IZipEntry zipEntry);
    public void AddEntry(string name, Stream data);
    public void AddEntry(string name, FileInfo fileInfo);
    public IZipArchiveResult CreateZip(string fileName);
    public Task<IZipArchiveResult> CreateZipAsync(string fileName, CancellationToken cancellationToken = default);
}
```

**Usage:**
```csharp
var archive = new ZipArchive();
archive.Compression = CompressionLevel.Optimal;
archive.AddEntry("file1.txt", stream1);
archive.AddEntry("file2.txt", new FileInfo("path/to/file.txt"));
var result = await archive.CreateZipAsync("archive.zip");
```

**Dependencies:**
- `System.IO.Compression` - For ZIP functionality

**Related Components:**
- [IZipArchive](#iziparchive) - Interface definition
- [IZipEntry](#izipentry) - ZIP entry interface

---

#### PathUtils

**File:** `PathUtils.cs`

**Purpose:** Provides static utility methods for combining and manipulating file paths.

**Key Responsibilities:**
- Combine multiple path fragments into complete paths
- Normalize path separators (handle both `\` and `/`)
- Support variable number of path arguments

**Key Members:**
```csharp
public static class PathUtils
{
    public static string CombinePaths(params string[] paths);
}
```

**Usage:**
```csharp
var path = PathUtils.CombinePaths("C:", "Users", "John", "Documents", "file.txt");
// Returns: "C:\Users\John\Documents\file.txt"
```

**Dependencies:**
- `System.IO` - For Path.Combine

**Related Components:**
- None (standalone utility)

---

#### TextFieldParser

**File:** `TextFieldParser.cs`

**Purpose:** Parses delimited text fields from files and streams, similar to Visual Basic's TextFieldParser.

**Key Responsibilities:**
- Parse delimited text fields from files
- Support various delimiters
- Handle quoted fields and escape sequences
- Read fields line by line

**Key Members:**
```csharp
public class TextFieldParser
{
    public TextFieldParser(string path);
    public TextFieldParser(Stream stream);
    public string[] ReadFields();
    public bool EndOfData { get; }
}
```

**Usage:**
```csharp
using (var parser = new TextFieldParser("data.txt"))
{
    parser.Delimiters = new[] { "," };
    while (!parser.EndOfData)
    {
        string[] fields = parser.ReadFields();
        // Process fields
    }
}
```

**Dependencies:**
- `System.IO` - For file and stream operations

**Related Components:**
- [CSVReader](#csvreader) - Alternative CSV reading approach

---

### Models

#### IZipEntry

**File:** `Zip/Models/IZipEntry.cs`

**Purpose:** Represents a single entry in a ZIP archive.

**Properties:**
- `string Name` - The name/path of the entry in the ZIP
- `Stream Data` - The stream containing the entry data
- `FileInfo FileInfo` - Optional file information for file-based entries

**Usage:**
```csharp
IZipEntry entry = new ZipEntry
{
    Name = "document.txt",
    Data = fileStream
};
```

---

#### IZipArchiveResult

**File:** `Zip/Models/IZipArchiveResult.cs`

**Purpose:** Represents the result of creating a ZIP archive.

**Properties:**
- `bool Success` - Whether the ZIP creation succeeded
- `string FilePath` - Path to the created ZIP file
- `Stream ZipStream` - Stream containing the ZIP data
- `Exception Error` - Any error that occurred during creation

**Usage:**
```csharp
var result = archive.CreateZip("archive.zip");
if (result.Success)
{
    // Use result.FilePath or result.ZipStream
}
```

---

#### QuoteOptions

**File:** `CSV/Models/QuoteOptions.cs`

**Purpose:** Enumeration defining quote handling options for CSV operations.

**Values:**
- `None` - No quote handling
- `RemoveQuotes` - Remove quotes from values
- `PreserveQuotes` - Keep quotes in values

**Usage:**
```csharp
var options = QuoteOptions.RemoveQuotes;
```

---

#### CompressionLevel

**File:** `Zip/Models/CompressionLevel.cs`

**Purpose:** Enumeration defining compression levels for ZIP archives.

**Values:**
- `NoCompression` - No compression
- `Fastest` - Fastest compression (default)
- `Optimal` - Best compression ratio
- `SmallestSize` - Smallest file size

**Usage:**
```csharp
archive.Compression = CompressionLevel.Optimal;
```

---

### Exceptions

#### DelimiterNotProvidedException

**File:** `CSV/Exceptions/DelimiterNotProvidedException.cs`

**Purpose:** Exception thrown when a CSV operation requires a delimiter but none is provided.

**Usage:**
```csharp
try
{
    reader.GetExpandoList("file.csv", null);
}
catch (DelimiterNotProvidedException ex)
{
    // Handle missing delimiter
}
```

---

## Architecture Patterns

- **Interface-Based Design**: All major components expose interfaces (ICSVReader, ICSVWriter, IZipArchive) for testability and flexibility
- **Stream Support**: Most operations support both file paths and streams for flexibility
- **Async/Await Pattern**: JSON and ZIP operations support both synchronous and asynchronous methods
- **Result Pattern**: ZIP operations return result objects with success status and error information
- **Factory Pattern**: CSV readers can be configured with different options via constructors

## Usage Patterns

### Pattern 1: Reading CSV to Strongly-Typed Objects

```csharp
var reader = new CSVReader(hasColumns: true);
var people = reader.GetList<Person>("people.csv", ",");
foreach (var person in people)
{
    Console.WriteLine($"{person.Name}: {person.Email}");
}
```

### Pattern 2: Reading CSV to ExpandoObjects

```csharp
var reader = new CSVReader();
var data = reader.GetExpandoList("data.csv", ",", removeQuotes: true);
foreach (dynamic row in data)
{
    Console.WriteLine($"{row.Column1}: {row.Column2}");
}
```

### Pattern 3: Writing Objects to CSV

```csharp
var writer = new CSVWriter();
var people = new List<Person> { /* ... */ };
writer.Write("output.csv", people, ",");
```

### Pattern 4: JSON File Operations

```csharp
// Read
var config = JsonFileReader.Read<AppConfig>("config.json");

// Write
var newConfig = new AppConfig { /* ... */ };
JsonFileWriter.Write("config.json", newConfig);
```

### Pattern 5: Creating ZIP Archives

```csharp
var archive = new ZipArchive();
archive.Compression = CompressionLevel.Optimal;
archive.AddEntry("file1.txt", stream1);
archive.AddEntry("file2.txt", new FileInfo("path/to/file.txt"));
var result = await archive.CreateZipAsync("archive.zip");
if (result.Success)
{
    Console.WriteLine($"Created: {result.FilePath}");
}
```

### Pattern 6: Path Combination

```csharp
var path = PathUtils.CombinePaths("C:", "Users", "John", "Documents", "file.txt");
```

---

## Dependencies

### Internal Dependencies

- `AMCode.Common.Extensions.ExpandoObjects` - For ExpandoObject extension methods used in CSV reading
- `AMCode.Common.Util` - For utility functions

### External Dependencies

- `Newtonsoft.Json` - For JSON serialization/deserialization
- `System.IO.Compression` - For ZIP archive functionality

---

## Related Components

### Within Same Library

- [Extensions](../Extensions/README.md) - Extension methods used by CSV operations
- [Common](../Common/README.md) - Common models and interfaces

### In Other Libraries

- None (IO is a foundational component)

---

## Testing

### Test Coverage

- Unit tests: `AMCode.Common.UnitTests/Components/IO/Tests/`
- Integration tests: Not applicable (file I/O operations)

### Example Test

```csharp
[Test]
public void CSVReader_GetList_ReturnsTypedObjects()
{
    var reader = new CSVReader(hasColumns: true);
    var data = reader.GetList<Person>("test.csv", ",");
    Assert.IsNotNull(data);
    Assert.Greater(data.Count, 0);
}
```

---

## Notes

- **CSV Delimiter Support**: The CSV reader and writer support custom delimiters, not just commas
- **Stream Support**: Most operations support both file paths and streams for flexibility
- **Async Operations**: JSON and ZIP operations support async/await for better performance
- **Quote Handling**: CSV operations support flexible quote handling options
- **Compression Levels**: ZIP archives support multiple compression levels for different use cases
- **Path Normalization**: PathUtils handles both Windows (`\`) and Unix (`/`) path separators

---

**See Also:**
- [Library README](../../README.md) - AMCode.Common library overview
- [Root README](../../../../README.md) - AMCode library overview

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team

