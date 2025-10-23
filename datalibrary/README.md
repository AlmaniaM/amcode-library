# AMCode Database Query Library

A comprehensive C# library for accessing databases and querying data into static and dynamic objects. This library provides a flexible data access layer with support for multiple database providers including **MongoDB**, Vertica, and ODBC connections.

## Features

- **Generic Data Providers**: Query data into strongly-typed objects or dynamic ExpandoObjects
- **Multiple Database Support**: Built-in support for **MongoDB**, Vertica, and ODBC connections
- **MongoDB Integration**: High-level abstractions for MongoDB document operations with consistent API patterns
- **Flexible Data Transformation**: Convert database results to various object types
- **Async Support**: Full async/await support for all database operations
- **Connection Management**: Robust connection handling with automatic cleanup and health monitoring
- **Session & Transaction Support**: Built-in MongoDB session and transaction management
- **Extensible Architecture**: Easy to extend with custom database providers

## Quick Start

### Prerequisites

- .NET 9.0 SDK or later
- Access to your target database (MongoDB, Vertica, ODBC-compatible databases)

### Installation

1. **Local Development Setup**: This project is configured for local development with local NuGet packages.

2. **Include in Your Project**: Add a reference to the AMCode.Data library:

```xml
<PackageReference Include="AMCode.Data" Version="1.1.0" />
```

### Basic Usage

#### SQL Databases (Vertica, ODBC)

```csharp
using AMCode.Data;

// Create a data provider
var provider = new GenericDataProvider<MyModel>();

// Query data
var results = await provider.QueryAsync("SELECT * FROM MyTable");

// Or use ExpandoObject for dynamic data
var dynamicProvider = new ExpandoObjectDataProvider();
var dynamicResults = await dynamicProvider.QueryAsync("SELECT * FROM MyTable");
```

#### MongoDB

```csharp
using AMCode.Data.MongoDB;
using MongoDB.Driver;

// Create a MongoDB data provider
var factory = new MongoDataProviderFactory();
var provider = factory.CreateProvider("mongodb://localhost:27017");

// Strongly-typed operations
var filter = Builders<UserDocument>.Filter.Eq(x => x.Name, "John");
var users = await provider.GetListOfAsync<UserDocument>("users", filter);

// Dynamic object operations
var dynamicFilter = Builders<BsonDocument>.Filter.Eq("name", "John");
var dynamicUsers = await provider.GetExpandoListAsync("users", dynamicFilter);

// Insert operations
var newUser = new UserDocument { Name = "Jane", Email = "jane@example.com" };
var insertedUser = await provider.InsertOneAsync("users", newUser);
```

## Development

### Building the Project

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test

# Create NuGet package
dotnet pack --configuration Release
```

### Project Structure

- `AMCode.Data/` - Main library project
  - `Components/MongoDB/` - MongoDB integration components
  - `Components/SQL/` - SQL database components
- `AMCode.Data.UnitTests/` - Unit tests
- `AMCode.Data.SQLTests/` - Integration tests with database
- `local-packages/` - Local NuGet package output

### Testing

The project includes comprehensive unit and integration tests:

- **Unit Tests**: Test individual components in isolation using mocks
- **SQL Tests**: Integration tests that require a database connection

For SQL tests, you'll need to configure your database connection in the test configuration.

## Configuration

### Database Connections

#### SQL Databases (Vertica, ODBC)

Configure your database connections using standard .NET connection strings:

```csharp
var connectionString = "Server=myserver;Database=mydb;...";
var provider = new GenericDataProvider<MyModel>(connectionString);
```

#### MongoDB

Configure MongoDB connections using MongoDB connection strings:

```csharp
// Basic connection
var connectionString = "mongodb://localhost:27017";

// With authentication
var connectionString = "mongodb://username:password@localhost:27017/database";

// With options
var connectionString = "mongodb://localhost:27017/?maxPoolSize=100&connectTimeoutMS=10000";

var factory = new MongoDataProviderFactory();
var provider = factory.CreateProvider(connectionString);
```

### Environment Variables

The library respects standard .NET configuration patterns and can be configured through:
- `appsettings.json`
- Environment variables
- Configuration providers

## Dependencies

- **AMCode.Columns** - Data transformation utilities
- **AMCode.Common** - Common utilities and extensions
- **AMCode.Vertica.Client** - Vertica database client
- **MongoDB.Driver** - MongoDB C# driver (included)
- **Newtonsoft.Json** - JSON serialization
- **System.Data.Odbc** - ODBC database support

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new functionality
5. Ensure all tests pass
6. Submit a pull request

## License

This project is part of the AMCode library suite. See the main AMCode repository for licensing information.

## MongoDB Integration

The AMCode.Data library includes comprehensive MongoDB integration with:

- **High-Level Abstractions**: Consistent API patterns matching SQL providers
- **Strongly-Typed Operations**: Type-safe document operations with automatic mapping
- **Dynamic Object Support**: Flexible ExpandoObject operations for schema-less data
- **Data Transformation**: Built-in field mapping and data transformation capabilities
- **Session & Transaction Support**: Built-in MongoDB session and transaction management
- **Connection Management**: Robust connection pooling and health monitoring
- **Comprehensive Error Handling**: Custom exceptions with detailed error context

### MongoDB Documentation

- **[MongoDB Integration Guide](AMCode.Data/Components/MongoDB/README.md)** - Complete API documentation and usage examples
- **[Migration Guide](AMCode.Data/Components/MongoDB/MIGRATION_GUIDE.md)** - Migrate from raw MongoDB driver
- **[Performance Guide](AMCode.Data/Components/MongoDB/PERFORMANCE_GUIDE.md)** - Performance benchmarks and optimization
- **[Troubleshooting Guide](AMCode.Data/Components/MongoDB/TROUBLESHOOTING_GUIDE.md)** - Common issues and solutions
- **[Examples](AMCode.Data/Components/MongoDB/Examples/README.md)** - Comprehensive usage examples

## Support

For issues and questions:
1. Check the existing issues in the repository
2. Create a new issue with detailed information
3. Include sample code and error messages when possible
4. For MongoDB-specific issues, refer to the MongoDB documentation and troubleshooting guide