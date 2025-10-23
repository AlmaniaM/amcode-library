# MongoDB Troubleshooting Guide

This comprehensive troubleshooting guide helps you diagnose and resolve common issues when using the AMCode.Data MongoDB integration.

## Table of Contents

- [Connection Issues](#connection-issues)
- [Authentication Problems](#authentication-problems)
- [Query Performance Issues](#query-performance-issues)
- [Data Transformation Errors](#data-transformation-errors)
- [Memory and Resource Issues](#memory-and-resource-issues)
- [Transaction Problems](#transaction-problems)
- [Error Handling](#error-handling)
- [Debugging Techniques](#debugging-techniques)
- [Common Error Messages](#common-error-messages)
- [Performance Troubleshooting](#performance-troubleshooting)

## Connection Issues

### Problem: Cannot Connect to MongoDB

**Symptoms:**
- `MongoConnectionException` thrown
- Connection timeout errors
- "Server selection timeout" errors

**Diagnosis:**
```csharp
try
{
    var factory = new MongoDataProviderFactory();
    var provider = factory.CreateProvider(connectionString);
    
    // Test connection
    var filter = Builders<BsonDocument>.Filter.Empty;
    await provider.GetExpandoListAsync("test", filter);
}
catch (MongoConnectionException ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
    Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
}
```

**Solutions:**

1. **Verify MongoDB Server Status**
   ```bash
   # Check if MongoDB is running
   sudo systemctl status mongod
   
   # Start MongoDB if not running
   sudo systemctl start mongod
   ```

2. **Check Connection String**
   ```csharp
   // Correct format
   var connectionString = "mongodb://localhost:27017";
   
   // With authentication
   var connectionString = "mongodb://username:password@localhost:27017/database";
   
   // With options
   var connectionString = "mongodb://localhost:27017/?maxPoolSize=100&connectTimeoutMS=10000";
   ```

3. **Network Connectivity**
   ```csharp
   // Test network connectivity
   var client = new MongoClient(connectionString);
   var server = await client.GetServerAsync();
   Console.WriteLine($"Server state: {server.Description.State}");
   ```

4. **Firewall Configuration**
   ```bash
   # Check if port 27017 is open
   netstat -tlnp | grep 27017
   
   # Open port if needed
   sudo ufw allow 27017
   ```

### Problem: Connection Pool Exhaustion

**Symptoms:**
- "Connection pool exhausted" errors
- Slow response times
- High connection count

**Solutions:**

1. **Increase Pool Size**
   ```csharp
   var connectionString = "mongodb://localhost:27017/?maxPoolSize=200&minPoolSize=20";
   ```

2. **Monitor Pool Usage**
   ```csharp
   public class ConnectionPoolMonitor
   {
       public async Task MonitorPoolAsync(IMongoClient client)
       {
           var server = await client.GetServerAsync();
           var pool = server.GetConnectionPool();
           
           Console.WriteLine($"Available connections: {pool.AvailableCount}");
           Console.WriteLine($"Used connections: {pool.UsedCount}");
           Console.WriteLine($"Total connections: {pool.AvailableCount + pool.UsedCount}");
       }
   }
   ```

3. **Implement Connection Retry Logic**
   ```csharp
   public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3)
   {
       for (int i = 0; i < maxRetries; i++)
       {
           try
           {
               return await operation();
           }
           catch (MongoConnectionException ex) when (i < maxRetries - 1)
           {
               await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, i))); // Exponential backoff
           }
       }
       
       throw new InvalidOperationException("Max retries exceeded");
   }
   ```

## Authentication Problems

### Problem: Authentication Failed

**Symptoms:**
- "Authentication failed" errors
- "Invalid credentials" messages
- Connection refused after authentication

**Solutions:**

1. **Verify Credentials**
   ```csharp
   // Test authentication
   var connectionString = "mongodb://username:password@localhost:27017/database";
   var client = new MongoClient(connectionString);
   
   try
   {
       var server = await client.GetServerAsync();
       Console.WriteLine("Authentication successful");
   }
   catch (MongoAuthenticationException ex)
   {
       Console.WriteLine($"Authentication failed: {ex.Message}");
   }
   ```

2. **Check User Permissions**
   ```javascript
   // In MongoDB shell
   use admin
   db.getUser("username")
   db.getUsers()
   ```

3. **Verify Database Access**
   ```csharp
   // Test database access
   var database = client.GetDatabase("your_database");
   var collections = await database.ListCollectionNamesAsync();
   ```

### Problem: SSL/TLS Certificate Issues

**Symptoms:**
- SSL handshake failures
- Certificate validation errors
- "Invalid certificate" messages

**Solutions:**

1. **Disable SSL Validation (Development Only)**
   ```csharp
   var connectionString = "mongodb://localhost:27017/?ssl=false";
   ```

2. **Configure SSL Properly**
   ```csharp
   var connectionString = "mongodb://localhost:27017/?ssl=true&sslVerifyCertificate=false";
   ```

3. **Use Connection String Options**
   ```csharp
   var connectionString = "mongodb://localhost:27017/" +
       "?ssl=true" +
       "&sslVerifyCertificate=false" +
       "&sslAllowInvalidCertificates=true";
   ```

## Query Performance Issues

### Problem: Slow Query Performance

**Symptoms:**
- Queries taking too long
- High CPU usage
- Memory consumption issues

**Diagnosis:**
```csharp
public class QueryPerformanceDiagnostic
{
    public async Task DiagnoseQueryPerformanceAsync(IMongoDataProvider provider)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var filter = Builders<UserDocument>.Filter.Eq(x => x.IsActive, true);
            var users = await provider.GetListOfAsync("users", filter);
            
            stopwatch.Stop();
            Console.WriteLine($"Query took {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Returned {users.Count} documents");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Query failed: {ex.Message}");
        }
    }
}
```

**Solutions:**

1. **Check Index Usage**
   ```javascript
   // In MongoDB shell
   db.users.explain("executionStats").find({isActive: true})
   ```

2. **Create Appropriate Indexes**
   ```javascript
   // Create index
   db.users.createIndex({isActive: 1})
   db.users.createIndex({email: 1})
   db.users.createIndex({isActive: 1, age: 1}) // Compound index
   ```

3. **Optimize Query Filters**
   ```csharp
   // Use indexed fields
   var filter = Builders<UserDocument>.Filter.Eq(x => x.Email, email); // Email is indexed
   
   // Avoid regex on non-indexed fields
   var filter = Builders<UserDocument>.Filter.Regex(x => x.Name, new BsonRegularExpression("^John")); // Prefix match
   ```

4. **Use Projections**
   ```csharp
   // Select only needed fields
   var transforms = new List<IDataTransformColumnDefinition>
   {
       new DocumentTransformDefinition("name", "Name"),
       new DocumentTransformDefinition("email", "Email")
   };
   var users = await provider.GetListOfAsync("users", filter, transforms);
   ```

### Problem: Memory Issues with Large Datasets

**Symptoms:**
- OutOfMemoryException
- High memory usage
- Slow garbage collection

**Solutions:**

1. **Use Pagination**
   ```csharp
   public async Task<IList<UserDocument>> GetUsersPaginatedAsync(int page, int pageSize)
   {
       var filter = Builders<UserDocument>.Filter.Empty;
       var allUsers = await provider.GetListOfAsync("users", filter);
       
       return allUsers.Skip(page * pageSize).Take(pageSize).ToList();
   }
   ```

2. **Stream Large Datasets**
   ```csharp
   public async IAsyncEnumerable<UserDocument> StreamUsersAsync()
   {
       const int batchSize = 1000;
       var skip = 0;
       
       while (true)
       {
           var filter = Builders<UserDocument>.Filter.Empty;
           var users = await provider.GetListOfAsync("users", filter);
           
           var batch = users.Skip(skip).Take(batchSize).ToList();
           if (!batch.Any()) break;
           
           foreach (var user in batch)
           {
               yield return user;
           }
           
           skip += batchSize;
           
           // Force garbage collection
           if (skip % 10000 == 0)
           {
               GC.Collect();
               GC.WaitForPendingFinalizers();
           }
       }
   }
   ```

## Data Transformation Errors

### Problem: Type Conversion Failures

**Symptoms:**
- `MongoTransformationException` thrown
- Data type mismatches
- Serialization errors

**Diagnosis:**
```csharp
try
{
    var filter = Builders<BsonDocument>.Filter.Empty;
    var users = await provider.GetExpandoListAsync("users", filter);
}
catch (MongoTransformationException ex)
{
    Console.WriteLine($"Transformation failed: {ex.Message}");
    Console.WriteLine($"Field: {ex.FieldName}");
    Console.WriteLine($"Value: {ex.Value}");
}
```

**Solutions:**

1. **Handle Null Values**
   ```csharp
   public class SafeDataTransformer
   {
       public object ConvertBsonValue(BsonValue value)
       {
           if (value == null || value.IsBsonNull)
               return null;
           
           try
           {
               return value.BsonType switch
               {
                   BsonType.String => value.AsString,
                   BsonType.Int32 => value.AsInt32,
                   BsonType.Int64 => value.AsInt64,
                   BsonType.Double => value.AsDouble,
                   BsonType.Boolean => value.AsBoolean,
                   BsonType.DateTime => value.ToUniversalTime(),
                   _ => value.ToString()
               };
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Failed to convert {value.BsonType}: {ex.Message}");
               return value.ToString();
           }
       }
   }
   ```

2. **Use Custom Transformations**
   ```csharp
   var transforms = new List<IDataTransformColumnDefinition>
   {
       new DocumentTransformDefinition("_id", "Id", isRequired: true),
       new DocumentTransformDefinition("name", "Name", isRequired: true),
       new DocumentTransformDefinition("age", "Age", isRequired: false, defaultValue: 0),
       new DocumentTransformDefinition("createdAt", "CreatedAt", isRequired: true)
   };
   ```

3. **Validate Data Before Transformation**
   ```csharp
   public class DataValidator
   {
       public bool ValidateDocument(BsonDocument document, IList<IDataTransformColumnDefinition> transforms)
       {
           foreach (var transform in transforms)
           {
               if (transform.IsRequired && !document.Contains(transform.FieldName))
               {
                   throw new MongoTransformationException($"Required field '{transform.FieldName}' is missing");
               }
           }
           return true;
       }
   }
   ```

## Memory and Resource Issues

### Problem: Memory Leaks

**Symptoms:**
- Increasing memory usage over time
- OutOfMemoryException after extended use
- Slow performance degradation

**Solutions:**

1. **Proper Resource Disposal**
   ```csharp
   public class ResourceManager : IDisposable
   {
       private readonly IMongoDataProvider _provider;
       private bool _disposed = false;
       
       public ResourceManager(IMongoDataProvider provider)
       {
           _provider = provider;
       }
       
       public void Dispose()
       {
           if (!_disposed)
           {
               // Dispose resources
               _disposed = true;
           }
       }
   }
   ```

2. **Monitor Memory Usage**
   ```csharp
   public class MemoryMonitor
   {
       public void LogMemoryUsage(string operation)
       {
           var memoryBefore = GC.GetTotalMemory(false);
           Console.WriteLine($"Memory before {operation}: {memoryBefore / 1024 / 1024} MB");
           
           // Perform operation
           
           var memoryAfter = GC.GetTotalMemory(false);
           Console.WriteLine($"Memory after {operation}: {memoryAfter / 1024 / 1024} MB");
           Console.WriteLine($"Memory delta: {(memoryAfter - memoryBefore) / 1024 / 1024} MB");
       }
   }
   ```

3. **Force Garbage Collection**
   ```csharp
   public class GarbageCollectionHelper
   {
       public static void ForceGC()
       {
           GC.Collect();
           GC.WaitForPendingFinalizers();
           GC.Collect();
       }
       
       public static void ForceGCIfNeeded(long thresholdBytes)
       {
           var currentMemory = GC.GetTotalMemory(false);
           if (currentMemory > thresholdBytes)
           {
               ForceGC();
           }
       }
   }
   ```

## Transaction Problems

### Problem: Transaction Failures

**Symptoms:**
- Transaction rollback errors
- Deadlock detection
- Session timeout issues

**Solutions:**

1. **Handle Transaction Timeouts**
   ```csharp
   public async Task<bool> ExecuteTransactionWithTimeoutAsync(Func<Task> operation, TimeSpan timeout)
   {
       using var cts = new CancellationTokenSource(timeout);
       
       try
       {
           await operation();
           return true;
       }
       catch (OperationCanceledException)
       {
           Console.WriteLine("Transaction timed out");
           return false;
       }
       catch (Exception ex)
       {
           Console.WriteLine($"Transaction failed: {ex.Message}");
           return false;
       }
   }
   ```

2. **Implement Retry Logic for Transactions**
   ```csharp
   public async Task<bool> ExecuteTransactionWithRetryAsync(Func<Task> operation, int maxRetries = 3)
   {
       for (int i = 0; i < maxRetries; i++)
       {
           try
           {
               await operation();
               return true;
           }
           catch (MongoWriteConcernException ex) when (i < maxRetries - 1)
           {
               Console.WriteLine($"Transaction attempt {i + 1} failed, retrying...");
               await Task.Delay(TimeSpan.FromMilliseconds(100 * (i + 1)));
           }
       }
       
       return false;
   }
   ```

3. **Monitor Transaction Performance**
   ```csharp
   public class TransactionMonitor
   {
       public async Task<T> MonitorTransactionAsync<T>(Func<Task<T>> operation)
       {
           var stopwatch = Stopwatch.StartNew();
           
           try
           {
               var result = await operation();
               stopwatch.Stop();
               
               Console.WriteLine($"Transaction completed in {stopwatch.ElapsedMilliseconds}ms");
               return result;
           }
           catch (Exception ex)
           {
               stopwatch.Stop();
               Console.WriteLine($"Transaction failed after {stopwatch.ElapsedMilliseconds}ms: {ex.Message}");
               throw;
           }
       }
   }
   ```

## Error Handling

### Problem: Unhandled Exceptions

**Symptoms:**
- Application crashes
- Unclear error messages
- Difficult debugging

**Solutions:**

1. **Comprehensive Error Handling**
   ```csharp
   public class ErrorHandler
   {
       public async Task<T> HandleOperationAsync<T>(Func<Task<T>> operation, string operationName)
       {
           try
           {
               return await operation();
           }
           catch (MongoConnectionException ex)
           {
               Console.WriteLine($"Connection error in {operationName}: {ex.Message}");
               throw new ServiceException($"Database connection failed during {operationName}", ex);
           }
           catch (MongoQueryException ex)
           {
               Console.WriteLine($"Query error in {operationName}: {ex.Message}");
               throw new ServiceException($"Query failed during {operationName}", ex);
           }
           catch (MongoTransformationException ex)
           {
               Console.WriteLine($"Transformation error in {operationName}: {ex.Message}");
               throw new ServiceException($"Data transformation failed during {operationName}", ex);
           }
           catch (Exception ex)
           {
               Console.WriteLine($"Unexpected error in {operationName}: {ex.Message}");
               throw new ServiceException($"Unexpected error during {operationName}", ex);
           }
       }
   }
   ```

2. **Logging Integration**
   ```csharp
   public class LoggingErrorHandler
   {
       private readonly ILogger _logger;
       
       public LoggingErrorHandler(ILogger logger)
       {
           _logger = logger;
       }
       
       public async Task<T> HandleOperationWithLoggingAsync<T>(Func<Task<T>> operation, string operationName)
       {
           try
           {
               _logger.LogInformation("Starting operation: {OperationName}", operationName);
               var result = await operation();
               _logger.LogInformation("Completed operation: {OperationName}", operationName);
               return result;
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Failed operation: {OperationName}", operationName);
               throw;
           }
       }
   }
   ```

## Debugging Techniques

### Problem: Difficult to Debug Issues

**Solutions:**

1. **Enable Detailed Logging**
   ```csharp
   var loggerFactory = LoggerFactory.Create(builder =>
   {
       builder.AddConsole()
              .SetMinimumLevel(LogLevel.Debug);
   });
   
   var logger = loggerFactory.CreateLogger<MongoDataProvider>();
   var provider = new MongoDataProvider(connectionFactory, sessionManager, healthMonitor, logger);
   ```

2. **Use MongoDB Profiler**
   ```javascript
   // Enable profiling
   db.setProfilingLevel(2, { slowms: 100 })
   
   // View profiler results
   db.system.profile.find().sort({ts: -1}).limit(5)
   ```

3. **Implement Debug Helpers**
   ```csharp
   public class DebugHelper
   {
       public static void LogQueryDetails(string collection, FilterDefinition<BsonDocument> filter)
       {
           Console.WriteLine($"Collection: {collection}");
           Console.WriteLine($"Filter: {filter.Render(BsonDocumentSerializer.Instance, BsonSerializer.SerializerRegistry)}");
       }
       
       public static void LogDocumentStructure(BsonDocument document)
       {
           Console.WriteLine("Document structure:");
           foreach (var element in document)
           {
               Console.WriteLine($"  {element.Name}: {element.Value.BsonType}");
           }
       }
   }
   ```

## Common Error Messages

### "Server selection timeout"

**Cause:** MongoDB server is not accessible or not running.

**Solution:**
1. Check if MongoDB is running
2. Verify connection string
3. Check network connectivity
4. Verify firewall settings

### "Authentication failed"

**Cause:** Invalid credentials or user permissions.

**Solution:**
1. Verify username and password
2. Check user permissions
3. Verify authentication database
4. Check SSL/TLS configuration

### "Connection pool exhausted"

**Cause:** Too many concurrent connections or connections not being released.

**Solution:**
1. Increase pool size
2. Implement connection retry logic
3. Ensure proper resource disposal
4. Monitor connection usage

### "Write concern failed"

**Cause:** Write operation couldn't be confirmed by the required number of replica set members.

**Solution:**
1. Check replica set status
2. Adjust write concern settings
3. Verify network connectivity to replica set members

### "Document too large"

**Cause:** Document exceeds MongoDB's 16MB size limit.

**Solution:**
1. Split large documents
2. Use GridFS for large files
3. Optimize document structure
4. Remove unnecessary fields

## Performance Troubleshooting

### Problem: Slow Performance

**Diagnosis Steps:**

1. **Check Query Performance**
   ```csharp
   public async Task DiagnoseQueryPerformanceAsync(IMongoDataProvider provider)
   {
       var stopwatch = Stopwatch.StartNew();
       
       var filter = Builders<UserDocument>.Filter.Eq(x => x.IsActive, true);
       var users = await provider.GetListOfAsync("users", filter);
       
       stopwatch.Stop();
       
       Console.WriteLine($"Query performance:");
       Console.WriteLine($"  Duration: {stopwatch.ElapsedMilliseconds}ms");
       Console.WriteLine($"  Document count: {users.Count}");
       Console.WriteLine($"  Rate: {users.Count * 1000.0 / stopwatch.ElapsedMilliseconds:F2} docs/sec");
   }
   ```

2. **Monitor Resource Usage**
   ```csharp
   public class ResourceMonitor
   {
       public void MonitorResources()
       {
           var process = Process.GetCurrentProcess();
           
           Console.WriteLine($"Memory usage: {process.WorkingSet64 / 1024 / 1024} MB");
           Console.WriteLine($"CPU time: {process.TotalProcessorTime}");
           Console.WriteLine($"Thread count: {process.Threads.Count}");
       }
   }
   ```

3. **Profile Database Operations**
   ```javascript
   // Enable profiling
   db.setProfilingLevel(1, { slowms: 100 })
   
   // Check slow operations
   db.system.profile.find({millis: {$gt: 100}}).sort({ts: -1})
   ```

## Getting Help

### When to Seek Help

- Issues persist after trying solutions
- Performance problems that can't be resolved
- Complex error scenarios
- Integration issues with other systems

### How to Report Issues

1. **Gather Information**
   - Error messages and stack traces
   - MongoDB version and configuration
   - AMCode.Data version
   - Environment details
   - Steps to reproduce

2. **Create Minimal Reproduction**
   - Isolate the problem
   - Create minimal test case
   - Include sample data

3. **Contact Support**
   - Use project repository issues
   - Include all gathered information
   - Provide reproduction steps

### Useful Resources

- **MongoDB Documentation**: https://docs.mongodb.com/
- **MongoDB C# Driver Documentation**: https://mongodb.github.io/mongo-csharp-driver/
- **AMCode.Data Documentation**: See project README
- **Community Forums**: MongoDB community discussions

This troubleshooting guide should help you resolve most common issues with the AMCode.Data MongoDB integration. For complex issues or additional support, please refer to the project repository or community resources.
