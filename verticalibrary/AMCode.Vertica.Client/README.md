# AMCode.Vertica.Client

**Version:** 1.0  
**Target Framework:** .NET 9.0  
**Last Updated:** 2025-01-27  
**Purpose:** Mock Vertica database client implementation for local development and testing

---

## Overview

AMCode.Vertica.Client provides a mock implementation of Vertica database connectivity using the standard ADO.NET interfaces. This library is designed for local development scenarios where a full Vertica database connection is not available or required. It implements all standard ADO.NET abstractions (`DbConnection`, `DbCommand`, `DbDataReader`, `DbParameter`, `DbTransaction`) with mock functionality that allows code to compile and run without actual database connectivity.

This library is particularly useful for:
- Local development without Vertica database access
- Unit testing database-dependent code
- Prototyping and development workflows
- CI/CD pipelines where database connections are not available

## Architecture

The library follows the standard ADO.NET provider model, implementing the abstract base classes from `System.Data.Common`. All classes are mock implementations that provide the expected interface contracts without actual database operations.

### Design Principles

- **Standard ADO.NET Compliance**: Implements all required ADO.NET interfaces
- **Mock Functionality**: Provides no-op implementations suitable for development
- **Minimal Dependencies**: Only depends on `System.Data.Common`
- **Namespace Compatibility**: Uses `Vertica.Data.VerticaClient` namespace for compatibility

### Key Components

- **VerticaConnection**: Mock database connection implementation
- **VerticaCommand**: Mock command execution implementation
- **VerticaDataReader**: Mock data reader with empty result sets
- **VerticaParameter**: Parameter implementation for command parameters
- **VerticaParameterCollection**: Collection for managing command parameters
- **VerticaTransaction**: Mock transaction implementation

## Features

- Full ADO.NET interface implementation
- Mock connection management (open/close state tracking)
- Mock command execution (returns empty results)
- Parameter support for command preparation
- Transaction support (mock commit/rollback)
- Zero external database dependencies
- Suitable for local development and testing

## Dependencies

### Internal Dependencies

- None (standalone library)

### External Dependencies

- **System.Data.Common** (4.3.0) - ADO.NET base classes and interfaces

## Project Structure

```
AMCode.Vertica.Client/
├── VerticaConnection.cs          # Database connection implementation
├── VerticaCommand.cs             # Command execution implementation
├── VerticaDataReader.cs          # Data reader implementation
├── VerticaParameter.cs            # Parameter implementation
├── VerticaParameterCollection.cs  # Parameter collection implementation
├── VerticaTransaction.cs          # Transaction implementation
└── AMCode.Vertica.Client.csproj  # Project file
```

## Key Classes

### VerticaConnection

**Location:** `VerticaConnection.cs`

**Purpose:** Mock implementation of database connection. Manages connection state and provides command creation.

**Key Responsibilities:**

- Connection state management (Open/Closed)
- Connection string storage
- Command factory method
- Transaction creation
- Mock database metadata (Database, DataSource, ServerVersion)

**Key Properties:**
- `ConnectionString` - Connection string storage
- `State` - Current connection state (ConnectionState)
- `Database` - Mock database name ("MockDatabase")
- `DataSource` - Mock data source ("MockDataSource")
- `ServerVersion` - Mock server version ("1.0.0")

**Key Methods:**
- `Open()` - Sets connection state to Open
- `Close()` - Sets connection state to Closed
- `ChangeDatabase(string)` - Mock database change (no-op)

**Usage:**
```csharp
using Vertica.Data.VerticaClient;

var connection = new VerticaConnection("connection_string");
connection.Open();
// Use connection
connection.Close();
```

### VerticaCommand

**Location:** `VerticaCommand.cs`

**Purpose:** Mock implementation of database command. Stores command text and parameters but returns mock results.

**Key Responsibilities:**

- Command text storage
- Parameter collection management
- Mock command execution
- Connection association

**Key Properties:**
- `CommandText` - SQL command text
- `CommandTimeout` - Command timeout (default: 30 seconds)
- `CommandType` - Command type (default: Text)
- `Connection` - Associated VerticaConnection

**Key Methods:**
- `ExecuteNonQuery()` - Returns 0 (mock)
- `ExecuteScalar()` - Returns null (mock)
- `ExecuteReader()` - Returns empty VerticaDataReader
- `Cancel()` - Mock cancellation (no-op)
- `Prepare()` - Mock preparation (no-op)

**Usage:**
```csharp
var command = new VerticaCommand("SELECT * FROM table", connection);
var reader = command.ExecuteReader();
// Process reader (will be empty)
```

### VerticaDataReader

**Location:** `VerticaDataReader.cs`

**Purpose:** Mock implementation of data reader. Provides empty result sets with all required ADO.NET methods.

**Key Responsibilities:**

- Empty result set simulation
- Type-safe data access methods
- Reader state management

**Key Properties:**
- `FieldCount` - Returns 0 (no columns)
- `HasRows` - Returns false (no rows)
- `IsClosed` - Returns true (always closed)
- `RecordsAffected` - Returns 0

**Key Methods:**
- `Read()` - Returns false (no rows to read)
- `GetString(int)`, `GetInt32(int)`, etc. - Type-specific getters (return defaults)
- `IsDBNull(int)` - Returns true (all values are null)
- `GetName(int)` - Returns mock column names ("Column0", "Column1", etc.)

**Usage:**
```csharp
var reader = command.ExecuteReader();
while (reader.Read())
{
    // This will never execute (Read() returns false)
}
```

### VerticaParameter

**Location:** `VerticaParameter.cs`

**Purpose:** Mock implementation of command parameter. Stores parameter metadata and values.

**Key Responsibilities:**

- Parameter value storage
- Parameter metadata (name, type, direction, etc.)
- Type management

**Key Properties:**
- `ParameterName` - Parameter name
- `Value` - Parameter value
- `DbType` - Database type
- `Direction` - Parameter direction (default: Input)
- `IsNullable` - Nullability flag (default: true)
- `Size`, `Precision`, `Scale` - Size metadata

**Usage:**
```csharp
var parameter = new VerticaParameter
{
    ParameterName = "@id",
    Value = 123,
    DbType = DbType.Int32
};
command.Parameters.Add(parameter);
```

### VerticaParameterCollection

**Location:** `VerticaParameterCollection.cs`

**Purpose:** Mock implementation of parameter collection. Manages multiple command parameters.

**Key Responsibilities:**

- Parameter storage and retrieval
- Collection operations (add, remove, clear)
- Index-based and name-based access

**Key Methods:**
- `Add(object)` - Add parameter to collection
- `Remove(object)` - Remove parameter
- `Clear()` - Clear all parameters
- `Contains(string)` - Check if parameter exists (returns false - mock)
- `IndexOf(string)` - Find parameter index (returns -1 - mock)

**Usage:**
```csharp
var parameters = command.Parameters;
parameters.Add(new VerticaParameter { ParameterName = "@id", Value = 123 });
```

### VerticaTransaction

**Location:** `VerticaTransaction.cs`

**Purpose:** Mock implementation of database transaction. Provides transaction interface without actual transaction management.

**Key Responsibilities:**

- Transaction state management
- Mock commit/rollback operations
- Isolation level tracking

**Key Properties:**
- `IsolationLevel` - Transaction isolation level
- `Connection` - Associated connection

**Key Methods:**
- `Commit()` - Mock commit (no-op)
- `Rollback()` - Mock rollback (no-op)

**Usage:**
```csharp
using (var transaction = connection.BeginTransaction())
{
    // Perform operations
    transaction.Commit(); // Mock commit
}
```

## Usage Examples

### Basic Connection and Command

```csharp
using Vertica.Data.VerticaClient;
using System.Data;

var connection = new VerticaConnection("Server=localhost;Database=test;");
connection.Open();

var command = new VerticaCommand("SELECT * FROM users", connection);
var reader = command.ExecuteReader();

// Reader will be empty (mock implementation)
while (reader.Read())
{
    // This will not execute
}

reader.Close();
connection.Close();
```

### Parameterized Commands

```csharp
var command = new VerticaCommand("SELECT * FROM users WHERE id = @id", connection);

var parameter = new VerticaParameter
{
    ParameterName = "@id",
    Value = 123,
    DbType = DbType.Int32
};

command.Parameters.Add(parameter);
var reader = command.ExecuteReader();
```

### Transactions

```csharp
using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
{
    var command = connection.CreateCommand();
    command.Transaction = transaction;
    command.CommandText = "INSERT INTO users (name) VALUES (@name)";
    
    // Add parameters and execute
    
    transaction.Commit(); // Mock commit
}
```

### Connection State Management

```csharp
var connection = new VerticaConnection("connection_string");

Console.WriteLine(connection.State); // Closed

connection.Open();
Console.WriteLine(connection.State); // Open

connection.Close();
Console.WriteLine(connection.State); // Closed
```

## Configuration

This library requires no configuration. It is a pure mock implementation that works out of the box for development and testing scenarios.

### Connection String Format

While the library accepts any connection string format, it does not parse or validate it. The connection string is stored but not used for actual connectivity.

```csharp
var connection = new VerticaConnection("any connection string format");
```

## Testing

### Test Projects

This library does not have a dedicated test project. It is designed to be used as a dependency in other projects for testing purposes.

### Using in Tests

```csharp
[Test]
public void TestDatabaseOperation()
{
    // Use mock Vertica client for testing
    var connection = new VerticaConnection();
    connection.Open();
    
    var command = new VerticaCommand("SELECT * FROM table", connection);
    var reader = command.ExecuteReader();
    
    // Assert on mock behavior
    Assert.IsFalse(reader.HasRows);
    Assert.IsTrue(reader.IsClosed);
}
```

## Limitations

### Mock Implementation Constraints

- **No Actual Database Operations**: All database operations are no-ops
- **Empty Result Sets**: Data readers always return empty results
- **No Validation**: Connection strings and SQL are not validated
- **No Error Simulation**: Does not simulate database errors
- **State Tracking Only**: Connection state is tracked but not enforced

### When to Use

✅ **Use this library when:**
- Developing locally without Vertica database access
- Writing unit tests for database-dependent code
- Prototyping database interaction patterns
- Running CI/CD pipelines without database connectivity

❌ **Do not use this library when:**
- You need actual database connectivity
- You need to test real SQL queries
- You need to validate database operations
- You need to test error scenarios

## Related Libraries

- [AMCode.Data](../datalibrary/AMCode.Data/README.md) - Data access layer that may use this client
- [AMCode.Common](../commonlibrary/AMCode.Common/README.md) - Common utilities

## Migration Notes

This is a mock implementation library. If migrating from a real Vertica client:

1. **Connection Strings**: Any format is accepted (not validated)
2. **SQL Queries**: Not executed, only stored
3. **Results**: Always empty (no actual data returned)
4. **Transactions**: Commit/Rollback are no-ops
5. **Errors**: No database errors are simulated

## Known Issues

- Parameter name-based lookups always return null/not found (mock limitation)
- Data reader is always closed and has no rows
- No SQL validation or parsing
- No connection pooling simulation

## Future Considerations

- Enhanced mock data support (configurable result sets)
- Error simulation capabilities
- Connection string validation
- SQL parsing and validation (mock)
- Performance metrics simulation

---

**See Also:**

- [Root README](../../README.md) - Project overview
- [Documentation Plan](../../DOCUMENTATION_PLAN.md) - Documentation strategy

**Last Updated:** 2025-01-27  
**Maintained By:** Development Team
