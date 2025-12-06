# AMCode.Data

**Version:** 1.0
**Target Framework:** .NET 9.0
**Last Updated:** 2025-01-27
**Purpose:** Comprehensive data access layer providing unified interfaces for multiple database providers (MongoDB, ODBC, Vertica) with strongly-typed and dynamic object support

---

## Overview

AMCode.Data is a flexible, provider-agnostic data access library that abstracts database operations across multiple database systems. It provides a unified interface for executing queries, mapping results to strongly-typed objects or dynamic ExpandoObjects, and managing database connections. The library supports MongoDB, ODBC-compatible databases, and Vertica, with a clean architecture that makes it easy to add new providers.

## Architecture

The library follows a factory pattern with provider abstractions:

- **Provider Factories**: Create data providers for different database systems
- **Data Providers**: Execute queries and return results in various formats
- **Generic Data Providers**: Map query results to strongly-typed objects
- **ExpandoObject Data Providers**: Return dynamic objects for flexible data access
- **Database Bridges**: Abstract connection and command creation
- **Logging Infrastructure**: Optional structured logging for operations

### Key Components

- **DataProvider**: Main entry point combining generic and dynamic data access
- **IGenericDataProvider**: Interface for strongly-typed data operations
- **IExpandoObjectDataProvider**: Interface for dynamic object operations
- **IDbExecute**: Interface for executing non-query operations
- **MongoDB Components**: Full MongoDB integration with document operations
- **ODBC Components**: Support for ODBC-compatible databases
- **Vertica Components**: Support for Vertica database operations
- **Logging**: Optional structured logging infrastructure

## Features

- **Multi-Provider Support**: MongoDB, ODBC, and Vertica database support
- **Strongly-Typed Operations**: Map query results to strongly-typed objects
- **Dynamic Object Support**: Use ExpandoObject for flexible, schema-less access
- **Async/Await Support**: Full asynchronous support for all operations
- **Data Transformation**: Custom column mapping and data transformation
- **Connection Management**: Robust connection handling and pooling
- **Optional Logging**: Structured logging infrastructure (completely optional)
- **Query Cancellation**: Support for cancellation tokens
- **Error Handling**: Comprehensive exception handling with custom exceptions

## Dependencies

### Internal Dependencies

- **AMCode.Common** - Common utilities and components
- **AMCode.Columns** - Column management and data transformation
- **AMCode.Vertica.Client** - Vertica database client

### External Dependencies

- **MongoDB.Driver** (3.5.0) - MongoDB database access
- **Microsoft.Extensions.Configuration.Abstractions** (8.0.0) - Configuration support
- **Microsoft.Extensions.DependencyInjection.Abstractions** (8.0.0) - Dependency injection
- **Microsoft.Extensions.Options** (8.0.0) - Options pattern
- **Microsoft.CSharp** (4.7.0) - C# language support
- **Newtonsoft.Json** (13.0.1) - JSON serialization
- **System.Data.Odbc** (6.0.0) - ODBC database access

## Project Structure

```
AMCode.Data/
├── Components/
│   ├── Data/                          # Core data provider interfaces and implementations
│   │   ├── Models/                     # Core interfaces (IDataProvider, IGenericDataProvider, etc.)
│   │   ├── DataProvider.cs             # Main data provider implementation
│   │   ├── GenericDataProvider.cs      # Strongly-typed data provider
│   │   ├── ExpandoObjectDataProvider.cs # Dynamic object data provider
│   │   ├── DbBridge.cs                 # Database bridge abstraction
│   │   ├── DbExecute.cs                # Non-query execution
│   │   ├── *Factory.cs                 # Factory implementations
│   │   └── Exceptions/                 # Custom exceptions
│   ├── MongoDB/                        # MongoDB integration
│   │   ├── Components/
│   │   │   ├── Connection/             # Connection management
│   │   │   ├── Data/                    # Data providers for MongoDB
│   │   │   ├── Extensions/              # MongoDB extensions
│   │   │   └── Transformation/         # Document transformation
│   │   ├── README.md                    # MongoDB documentation
│   │   ├── MIGRATION_GUIDE.md           # Migration guide
│   │   ├── PERFORMANCE_GUIDE.md         # Performance guide
│   │   └── TROUBLESHOOTING_GUIDE.md     # Troubleshooting guide
│   ├── ODBC/                           # ODBC provider components
│   │   ├── OdbcConnectionFactory.cs
│   │   ├── OdbcCommandFactory.cs
│   │   └── OdbcDataReaderProviderFactory.cs
│   ├── Vertica/                        # Vertica provider components
│   │   ├── VerticaConnectionFactory.cs
│   │   ├── VerticaCommandFactory.cs
│   │   ├── VerticaDataReaderProviderFactory.cs
│   │   └── VerticaQueryCancellation.cs
│   └── Extensions/                     # Extension methods
│       ├── DataReaderExtensions.cs
│       └── ValueParser.cs
├── Logging/                             # Logging infrastructure
│   ├── Infrastructure/                 # Logger implementations
│   ├── Interfaces/                     # Logger interfaces
│   ├── Configuration/                   # Logging configuration
│   ├── Models/                          # Logging models
│   ├── Extensions/                      # DI extensions
│   └── README.md                        # Logging documentation
└── Scripts/                             # Build scripts
```

## Key Interfaces

### IDataProvider

**Location:** `Components/Data/Models/IDataProvider.cs`

**Purpose:** Main interface combining generic data access, dynamic object access, and execution capabilities.

**Key Methods:**

- `GetListOfAsync<T>()` - Get strongly-typed list of objects
- `GetExpandoListAsync()` - Get list of dynamic ExpandoObjects
- `ExecuteAsync()` - Execute non-query operations
- `GetValueOfAsync<T>()` - Get single value from query

**See Also:** [Data Components](Components/Data/README.md)

### IGenericDataProvider

**Location:** `Components/Data/Models/IGenericDataProvider.cs`

**Purpose:** Interface for strongly-typed data operations that map query results to object properties.

**Key Methods:**

- `GetListOfAsync<T>(string query)` - Execute query and map to strongly-typed objects
- `GetListOfAsync<T>(string query, IList<IDataTransformColumnDefinition> columns)` - With column transformation
- `GetValueOfAsync<T>(string columnName, string query)` - Get single column value

**See Also:** [Data Components](Components/Data/README.md)

### IExpandoObjectDataProvider

**Location:** `Components/Data/Models/IExpandoObjectDataProvider.cs`

**Purpose:** Interface for dynamic object operations using ExpandoObject for flexible data access.

**Key Methods:**

- `GetExpandoListAsync(string query)` - Get list of ExpandoObjects
- `GetExpandoListAsync(string query, IList<IDataTransformColumnDefinition> columns)` - With transformation

**See Also:** [Data Components](Components/Data/README.md)

### IDbExecute

**Location:** `Components/Data/Models/IDbExecute.cs`

**Purpose:** Interface for executing non-query database operations (INSERT, UPDATE, DELETE).

**Key Methods:**

- `ExecuteAsync(string query)` - Execute non-query SQL command
- `ExecuteAsync(string query, IDbConnection connection)` - With explicit connection

**See Also:** [Data Components](Components/Data/README.md)

## Key Classes

### DataProvider

**Location:** `Components/Data/DataProvider.cs`

**Purpose:** Main data provider class that combines generic data access, dynamic object access, and execution capabilities.

**Key Responsibilities:**

- Unified interface for all data operations
- Delegates to specialized providers (generic, expando, execute)
- Optional logging support
- Performance tracking

**Usage:**
```csharp
var dataProvider = new DataProvider(
    dbExecuteFactory,
    expandoProviderFactory,
    genericProviderFactory,
    logger
);

// Strongly-typed access
var users = await dataProvider.GetListOfAsync<User>("SELECT * FROM Users");

// Dynamic access
var dynamicUsers = await dataProvider.GetExpandoListAsync("SELECT * FROM Users");

// Execute non-query
await dataProvider.ExecuteAsync("INSERT INTO Users (Name) VALUES ('John')");
```

### GenericDataProvider

**Location:** `Components/Data/GenericDataProvider.cs`

**Purpose:** Implementation for strongly-typed data operations that maps database columns to object properties.

**Key Responsibilities:**

- Execute queries and map results to strongly-typed objects
- Support column transformation via IDataTransformColumnDefinition
- Handle connection management
- Support cancellation tokens

**See Also:** [Data Components](Components/Data/README.md)

### ExpandoObjectDataProvider

**Location:** `Components/Data/ExpandoObjectDataProvider.cs`

**Purpose:** Implementation for dynamic object operations using ExpandoObject.

**Key Responsibilities:**

- Execute queries and return dynamic ExpandoObjects
- Support column transformation
- Flexible schema-less data access

**See Also:** [Data Components](Components/Data/README.md)

## Usage Examples

### Basic Usage - Strongly-Typed

```csharp
using AMCode.Data;

// Create provider factory
var factory = new GenericDataProviderFactory(
    connectionFactory,
    commandFactory,
    dataReaderProviderFactory
);

var provider = factory.CreateProvider("connectionString");

// Execute query and map to strongly-typed objects
var users = await provider.GetListOfAsync<User>("SELECT * FROM Users");

// Get single value
var count = await provider.GetValueOfAsync<int>("Count", "SELECT COUNT(*) as Count FROM Users");
```

### Basic Usage - Dynamic Objects

```csharp
using AMCode.Data;
using System.Dynamic;

var factory = new ExpandoObjectDataProviderFactory(
    connectionFactory,
    commandFactory,
    dataReaderProviderFactory
);

var provider = factory.CreateProvider("connectionString");

// Execute query and get dynamic objects
var users = await provider.GetExpandoListAsync("SELECT * FROM Users");

foreach (dynamic user in users)
{
    Console.WriteLine(user.Name); // Dynamic property access
    Console.WriteLine(user.Email);
}
```

### Using DataProvider (Unified Interface)

```csharp
using AMCode.Data;

var dataProvider = new DataProvider(
    dbExecuteFactory,
    expandoProviderFactory,
    genericProviderFactory,
    logger
);

// Strongly-typed
var users = await dataProvider.GetListOfAsync<User>("SELECT * FROM Users");

// Dynamic
var dynamicUsers = await dataProvider.GetExpandoListAsync("SELECT * FROM Users");

// Execute
await dataProvider.ExecuteAsync("UPDATE Users SET Active = 1 WHERE Id = @id");
```

### MongoDB Usage

```csharp
using AMCode.Data.MongoDB;
using MongoDB.Driver;

var factory = new MongoDataProviderFactory();
var provider = factory.CreateProvider("mongodb://localhost:27017");

// Strongly-typed MongoDB operations
var filter = Builders<MyDocument>.Filter.Eq(x => x.Name, "John");
var documents = await provider.GetListOfAsync<MyDocument>("users", filter);

// Dynamic MongoDB operations
var dynamicFilter = Builders<BsonDocument>.Filter.Eq("name", "John");
var dynamicResults = await provider.GetExpandoListAsync("users", dynamicFilter);
```

### With Logging

```csharp
using AMCode.Data;
using AMCode.Data.Logging;
using AMCode.Data.Logging.Infrastructure.Console;

// Create logger
var logger = new ConsoleLogger("MyApp", new ConsoleLoggerProvider());

// Create DataProvider with logging
var dataProvider = new DataProvider(
    dbExecuteFactory,
    expandoProviderFactory,
    genericProviderFactory,
    logger
);

// Operations are automatically logged
await dataProvider.ExecuteAsync("SELECT * FROM Users");
```

### Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using AMCode.Data.Logging.Extensions;

// Register logging services
services.AddAMCodeDataLogging();
services.AddAMCodeDataConsoleLogging();

// Register data providers
services.AddSingleton<IDataProviderFactory, DataProviderFactory>();
services.AddSingleton<IDataProvider, DataProvider>();
```

## Configuration

### Connection Strings

**ODBC/Vertica:**
```
Server=localhost;Database=mydb;User=user;Password=pass
```

**MongoDB:**
```
mongodb://localhost:27017
mongodb://user:pass@localhost:27017/database
```

### appsettings.json Example

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=mydb;User=user;Password=pass",
    "MongoDB": "mongodb://localhost:27017"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "AMCode.Data": "Debug"
    }
  }
}
```

## Testing

### Test Projects

- **AMCode.Data.UnitTests**: Unit tests for data providers
- **AMCode.Data.SQLTests**: SQL-specific integration tests

### Running Tests

```bash
# Run all tests
dotnet test AMCode.Data.UnitTests
dotnet test AMCode.Data.SQLTests

# Run specific test project
dotnet test datalibrary/AMCode.Data.UnitTests/AMCode.Data.UnitTests.csproj
```

## Subfolder Documentation

For detailed documentation on specific components:

- [Data Components](Components/Data/README.md) - Core data provider interfaces and implementations
- [MongoDB Integration](Components/MongoDB/README.md) - MongoDB provider documentation, migration guide, performance guide
- [Logging Infrastructure](Logging/README.md) - Logging system documentation

## Related Libraries

- [AMCode.Common](../commonlibrary/AMCode.Common/README.md) - Common utilities used by this library
- [AMCode.Columns](../columnslibrary/AMCode.Columns/README.md) - Column management and data transformation
- [AMCode.Vertica.Client](../verticalibrary/AMCode.Vertica.Client/README.md) - Vertica database client

## Migration Notes

### From Direct Database Access

When migrating from direct database access to AMCode.Data:

1. Replace direct `IDbConnection` usage with provider factories
2. Use `GetListOfAsync<T>()` instead of manual data reader mapping
3. Leverage ExpandoObject for dynamic scenarios
4. Use `ExecuteAsync()` for non-query operations

### Adding New Providers

To add support for a new database provider:

1. Implement `IDbConnectionFactory`, `IDbCommandFactory`, `IDbDataReaderProviderFactory`
2. Create provider factory implementing `IGenericDataProviderFactory` or `IExpandoObjectDataProviderFactory`
3. Register factories in dependency injection container

## Known Issues

- MongoDB provider requires MongoDB.Driver 3.5.0 or compatible
- ODBC provider requires System.Data.Odbc 6.0.0 or later
- Some database-specific features may not be available through the unified interface

## Future Considerations

- Additional database provider support (PostgreSQL, MySQL, etc.)
- Enhanced connection pooling strategies
- Query result caching
- Batch operation support
- Advanced transaction management

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy
- [MongoDB Guide](Components/MongoDB/README.md) - MongoDB-specific documentation
- [Logging Guide](Logging/README.md) - Logging infrastructure documentation

**Last Updated:** 2025-01-27
**Maintained By:** Development Team

