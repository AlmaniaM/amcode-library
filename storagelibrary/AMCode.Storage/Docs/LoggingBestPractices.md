# AMCode Storage Library Logging Best Practices

## Overview

This guide provides best practices for implementing and configuring logging in the AMCode Storage Library. It covers log levels, performance considerations, configuration examples, testing approaches, and troubleshooting tips.

## Log Levels

### Information
- **Use for**: Normal operation logging
- **Examples**: File operations (CreateFile, DownloadFile, DeleteFile), Performance metrics, Storage provider selection
- **When to use**: Always log successful operations and important state changes

### Warning
- **Use for**: Recoverable errors and unexpected conditions
- **Examples**: File not found (when expected), Storage provider fallback, Container/directory doesn't exist
- **When to use**: When operations can continue but with degraded functionality

### Error
- **Use for**: Unrecoverable errors that prevent operations
- **Examples**: Storage provider failures, File system errors, Network errors (Azure Blob), Authentication failures
- **When to use**: When operations fail and cannot be retried

### Debug
- **Use for**: Detailed debugging information
- **Examples**: Internal state information, Detailed performance metrics, Method entry/exit
- **When to use**: During development and troubleshooting

### Trace
- **Use for**: Very detailed tracing
- **Examples**: Parameter values, Detailed execution flow, Low-level operations
- **When to use**: For deep debugging and performance analysis

## Performance Considerations

### Logging Overhead
- **Impact**: Logging adds overhead to operations (typically 1-5%)
- **Mitigation**: Use appropriate log levels in production
- **Recommendation**: Consider using structured logging for better performance

### File Size Logging
- **Benefit**: Helps with capacity planning and performance analysis
- **Cost**: Minimal overhead for file size calculation
- **Configuration**: Can be disabled with `IncludeFileSizes = false` for high-performance scenarios

### Storage Type Logging
- **Benefit**: Essential for debugging multi-provider scenarios
- **Cost**: Minimal string overhead
- **Configuration**: Can be disabled with `IncludeStorageType = false` if not needed

### Performance Metrics
- **Benefit**: Critical for monitoring and optimization
- **Cost**: Stopwatch overhead (typically < 1ms)
- **Configuration**: Can be disabled with `EnablePerformanceLogging = false`

## Configuration Examples

### Development Environment
```csharp
services.ConfigureStorageLoggingForDevelopment();
// OR
services.ConfigureStorageLogging(options =>
{
    options.EnableDebugLogging = true;
    options.IncludeDetailedMetrics = true;
    options.LogOperationStartAndEnd = true;
}, builder =>
{
    builder.AddConsole();
    builder.AddDebug();
    builder.SetMinimumLevel(LogLevel.Debug);
});
```

### Production Environment
```csharp
services.ConfigureStorageLoggingForProduction();
// OR
services.ConfigureStorageLogging(options =>
{
    options.EnableDebugLogging = false;
    options.IncludeDetailedMetrics = false;
    options.LogOperationStartAndEnd = false;
    options.MaxLogMessageLength = 500;
}, builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});
```

### High-Performance Environment
```csharp
services.ConfigureStorageLoggingForHighPerformance();
// OR
services.ConfigureStorageLogging(options =>
{
    options.EnablePerformanceLogging = false;
    options.EnableOperationLogging = false;
    options.IncludeFileSizes = false;
    options.IncludeStorageType = false;
}, builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Warning);
});
```

### Configuration from appsettings.json
```json
{
  "StorageLogging": {
    "EnablePerformanceLogging": true,
    "EnableOperationLogging": true,
    "EnableErrorLogging": true,
    "EnableDebugLogging": false,
    "LogLevel": "Information",
    "IncludeFileSizes": true,
    "IncludeStorageType": true,
    "IncludeDetailedMetrics": false,
    "MaxLogMessageLength": 1000,
    "LogOperationStartAndEnd": false,
    "IncludeCorrelationId": false
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "AMCode.Storage": "Debug"
    }
  }
}
```

## Testing

### Mock Loggers
- **Use**: `MockStorageLogger`, `MockStorageOperationLogger`, `MockStorageMetricsLogger` for unit tests
- **Benefits**: Verify log messages, test error scenarios, isolate logging behavior
- **Example**:
```csharp
[Test]
public void ShouldLogFileOperation()
{
    // Arrange
    var mockLogger = new MockStorageLogger();
    var operationLogger = new StorageOperationLogger(mockLogger);
    
    // Act
    operationLogger.LogFileOperation("CreateFile", "test.txt", 1024, "Local");
    
    // Assert
    Assert.AreEqual(1, mockLogger.InformationLogs.Count);
    Assert.IsTrue(mockLogger.InformationLogs[0].Contains("CreateFile"));
}
```

### Integration Tests
- **Use**: Real loggers for integration tests
- **Benefits**: Verify log output format, test performance impact, validate configuration
- **Example**:
```csharp
[Test]
public async Task ShouldLogStorageOperations()
{
    // Arrange
    var services = new ServiceCollection();
    services.AddStorageLogging();
    services.AddScoped<ISimpleFileStorage, SimpleLocalStorage>();
    var provider = services.BuildServiceProvider();
    
    var storage = provider.GetRequiredService<ISimpleFileStorage>();
    
    // Act
    using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test"));
    await storage.CreateFileAsync("test.txt", stream);
    
    // Assert - Verify logs are generated (implementation depends on test framework)
}
```

## Troubleshooting

### Common Issues

#### 1. No Logs Appearing
**Symptoms**: Operations complete but no log messages are visible
**Causes**:
- Log level too high (e.g., Debug logs with Information level)
- Logger not registered in DI container
- Console output not visible

**Solutions**:
```csharp
// Check log level configuration
builder.SetMinimumLevel(LogLevel.Debug);

// Verify logger registration
services.AddStorageLogging();

// Check console output
builder.AddConsole();
```

#### 2. Performance Impact
**Symptoms**: Operations slower than expected
**Causes**:
- Too many debug/trace logs in production
- Performance logging enabled for high-frequency operations
- Large log messages

**Solutions**:
```csharp
// Reduce log level
builder.SetMinimumLevel(LogLevel.Information);

// Disable performance logging
options.EnablePerformanceLogging = false;

// Limit log message length
options.MaxLogMessageLength = 500;
```

#### 3. Missing Context
**Symptoms**: Logs don't contain expected information
**Causes**:
- Storage type logging disabled
- File size logging disabled
- Incorrect log message format

**Solutions**:
```csharp
// Enable context logging
options.IncludeStorageType = true;
options.IncludeFileSizes = true;

// Check log message format
logger.LogInformation("Storage Operation: {Operation} | File: {FileName} | Storage: {StorageType}", 
    operation, fileName, storageType);
```

### Debugging Tips

#### 1. Enable Debug Logging
```csharp
builder.SetMinimumLevel(LogLevel.Debug);
options.EnableDebugLogging = true;
```

#### 2. Use Structured Logging
```csharp
logger.LogInformation("Storage Operation: {Operation} | File: {FileName} | Size: {FileSizeBytes} bytes", 
    operation, fileName, fileSizeBytes);
```

#### 3. Check Log Output
- Verify log format matches expectations
- Check for missing information
- Verify performance metrics are being recorded

#### 4. Monitor Metrics
```csharp
var metricsLogger = provider.GetRequiredService<IStorageMetricsLogger>();
var operationCounts = metricsLogger.GetOperationCounts();
var errorCounts = metricsLogger.GetErrorCounts();
var storageUsage = metricsLogger.GetStorageUsage();
```

## Security Considerations

### Sensitive Information
- **Never log**: Connection strings, API keys, passwords, personal data
- **Be careful with**: File paths (may contain sensitive directory names)
- **Use**: Placeholder values for sensitive data

### Log Storage
- **Secure**: Ensure log files are stored securely
- **Retention**: Implement appropriate log retention policies
- **Access**: Control access to log files and log data

## Monitoring and Alerting

### Key Metrics to Monitor
- **Operation counts**: Track usage patterns
- **Error rates**: Monitor for failures
- **Performance**: Track operation durations
- **Storage usage**: Monitor capacity

### Alerting Thresholds
- **Error rate**: > 5% of operations
- **Performance**: Operations taking > 10 seconds
- **Storage usage**: > 80% of capacity

## Best Practices Summary

1. **Use appropriate log levels** for your environment
2. **Enable performance logging** in development, disable in high-performance production
3. **Include context** (file names, storage types, file sizes) for debugging
4. **Test logging behavior** with mock loggers
5. **Monitor metrics** for operational insights
6. **Secure log data** and control access
7. **Configure for environment** (development vs production)
8. **Use structured logging** for better searchability
9. **Implement log retention** policies
10. **Monitor and alert** on key metrics

## Migration Guide

### From No Logging
1. Add logging packages to project
2. Configure logging in Program.cs or Startup.cs
3. Add storage logging services
4. Update storage component constructors
5. Test logging output

### From Custom Logging
1. Replace custom logging with AMCode interfaces
2. Update dependency injection
3. Migrate log messages to new format
4. Update configuration
5. Test and validate

This comprehensive logging implementation provides enterprise-grade observability for the AMCode Storage Library while maintaining performance and flexibility across different deployment scenarios.
