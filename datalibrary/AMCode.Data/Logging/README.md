# AMCode.Data Logging

This directory contains the logging infrastructure for the AMCode.Data library, providing optional, structured logging capabilities for database operations.

## Features

- **Optional Logging**: Logging is completely optional and doesn't break existing functionality
- **Structured Logging**: JSON-formatted logs with searchable properties
- **Performance Tracking**: Automatic timing of database operations
- **Error Context**: Rich error information with correlation IDs
- **Multiple Output Formats**: Console, file, and structured logging
- **Sensitive Data Masking**: Automatic masking of passwords and connection strings
- **Backward Compatibility**: Existing code continues to work without changes

## Quick Start

### Basic Usage

```csharp
using AMCode.Data;
using AMCode.Data.Logging;
using AMCode.Data.Logging.Infrastructure;
using AMCode.Data.Logging.Infrastructure.Console;

// Create a logger
var logger = new ConsoleLogger("MyApp", new ConsoleLoggerProvider());

// Create DataProvider with logging
var dataProvider = new DataProvider(
    dbExecuteFactory,
    expandoProviderFactory,
    genericProviderFactory,
    logger
);

// Use as normal - logging happens automatically
await dataProvider.ExecuteAsync("SELECT * FROM Users");
```

### Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using AMCode.Data.Logging.Extensions;

// Register logging services
services.AddAMCodeDataLogging();
services.AddAMCodeDataConsoleLogging();

// Use in your services
public class MyService
{
    private readonly IDataProvider _dataProvider;
    private readonly ILogger<MyService> _logger;

    public MyService(IDataProvider dataProvider, ILogger<MyService> logger)
    {
        _dataProvider = dataProvider;
        _logger = logger;
    }
}
```

## Configuration

### appsettings.json

```json
{
  "AMCodeData": {
    "Logging": {
      "MinimumLevel": "Information",
      "EnableConsole": true,
      "EnableStructuredLogging": true,
      "EnablePerformanceLogging": true,
      "EnableCorrelationId": true,
      "EnableSensitiveDataMasking": true,
      "SensitiveDataKeys": ["password", "connectionstring", "secret"],
      "CategoryLevels": {
        "AMCode.Data.Components.Data.DataProvider": "Information"
      }
    }
  }
}
```

### Programmatic Configuration

```csharp
var configuration = new LoggingConfiguration
{
    MinimumLevel = LogLevel.Information,
    EnableConsole = true,
    EnableStructuredLogging = true,
    EnablePerformanceLogging = true,
    EnableCorrelationId = true,
    SensitiveDataKeys = new[] { "password", "connectionstring" }
};

var logger = new ConsoleLogger("MyApp", new ConsoleLoggerProvider(configuration));
```

## Log Levels

- **Trace**: Very detailed logs for debugging
- **Debug**: Detailed logs for development
- **Information**: General information about operations
- **Warning**: Warning messages for potential issues
- **Error**: Error messages for failed operations
- **Critical**: Critical errors that require immediate attention

## Log Structure

### Console Output (Structured)

```json
{
  "timestamp": "2024-01-15T10:30:45.123Z",
  "level": "Information",
  "message": "Executing query",
  "category": "AMCode.Data.Components.Data.DataProvider",
  "correlationId": "abc123-def456-ghi789",
  "operation": "QueryExecution",
  "properties": {
    "Query": "SELECT * FROM Users",
    "ResultCount": 150
  },
  "duration": "00:00:00.150",
  "application": "AMCode.Data",
  "version": "1.0.0"
}
```

### Console Output (Formatted)

```
2024-01-15 10:30:45.123 INFO  [AMCode.Data.Components.Data.DataProvider] [abc123-def456-ghi789]: Executing query
```

## Performance Logging

Performance metrics are automatically logged for all database operations:

- **QueryExecution**: Time taken to execute non-query SQL
- **ExpandoObjectQuery**: Time taken to execute queries returning ExpandoObjects
- **GenericQuery**: Time taken to execute queries returning typed objects
- **ValueQuery**: Time taken to execute single-value queries

## Error Logging

Errors are logged with full context:

- Exception details and stack trace
- Query that caused the error
- Correlation ID for tracing
- Operation type and parameters

## Sensitive Data Masking

Sensitive data is automatically masked in logs:

- Passwords
- Connection strings
- API keys
- Tokens
- Any custom sensitive keys

## Testing

The logging system includes comprehensive unit tests:

```csharp
[Test]
public void DataProvider_WithLogging_LogsOperations()
{
    // Test logging behavior
}
```

## Migration Guide

### Existing Code (No Changes Required)

```csharp
// This continues to work exactly as before
var dataProvider = new DataProvider(
    dbExecuteFactory,
    expandoProviderFactory,
    genericProviderFactory
);
```

### Adding Logging (Optional)

```csharp
// Add logging by providing a logger
var dataProvider = new DataProvider(
    dbExecuteFactory,
    expandoProviderFactory,
    genericProviderFactory,
    logger  // Optional parameter
);
```

## Best Practices

1. **Use Appropriate Log Levels**: Don't log everything at Information level
2. **Include Context**: Provide meaningful context in log messages
3. **Use Correlation IDs**: Enable correlation IDs for request tracing
4. **Configure Sensitive Data Masking**: Always enable for production
5. **Monitor Performance**: Use performance logging to identify slow queries
6. **Test Logging**: Include logging in your unit tests

## Troubleshooting

### Logs Not Appearing

1. Check log level configuration
2. Verify console output is enabled
3. Check category-specific log levels

### Performance Impact

1. Use appropriate log levels
2. Disable logging in production if not needed
3. Use structured logging for better performance

### Sensitive Data in Logs

1. Enable sensitive data masking
2. Add custom sensitive keys to configuration
3. Review log output before deployment

## Support

For issues or questions about the logging system:

1. Check the unit tests for examples
2. Review the configuration options
3. Check the console output for error messages
