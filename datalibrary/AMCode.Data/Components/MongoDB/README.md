# AMCode.Data MongoDB Integration

A comprehensive MongoDB data access layer for the AMCode.Data library, providing high-level abstractions for MongoDB document operations that mirror the existing SQL provider capabilities.

## Features

- **Strongly-Typed Operations**: Query MongoDB collections and map results to strongly-typed objects
- **Dynamic Object Support**: Use ExpandoObject for flexible, schema-less data access
- **Data Transformation**: Convert MongoDB documents with custom field mapping and transformations
- **Async/Await Support**: Full asynchronous support for all MongoDB operations
- **Connection Management**: Robust connection handling with pooling and health monitoring
- **Session & Transaction Support**: MongoDB sessions and transaction management
- **Comprehensive Error Handling**: Custom exceptions with detailed error context
- **Structured Logging**: Detailed logging for operations, performance, and debugging

## Quick Start

### Prerequisites

- .NET 9.0 SDK or later
- MongoDB server (local or cloud)
- MongoDB.Driver NuGet package (included)

### Installation

The MongoDB integration is included in the AMCode.Data library. No additional installation required.

### Basic Usage

```csharp
using AMCode.Data.MongoDB;
using MongoDB.Driver;

// Create a MongoDB data provider
var factory = new MongoDataProviderFactory();
var provider = factory.CreateProvider("mongodb://localhost:27017");

// Strongly-typed operations
var filter = Builders<MyDocument>.Filter.Eq(x => x.Name, "John");
var documents = await provider.GetListOfAsync<MyDocument>("users", filter);

// Dynamic object operations
var dynamicFilter = Builders<BsonDocument>.Filter.Eq("name", "John");
var dynamicResults = await provider.GetExpandoListAsync("users", dynamicFilter);
```

## Architecture

### Core Components

- **MongoDataProvider**: Main provider combining all MongoDB operations
- **MongoGenericDataProvider**: Strongly-typed data access operations
- **MongoExpandoObjectDataProvider**: Dynamic object mapping operations
- **MongoExecute**: Non-query operations (updates, deletes, bulk operations)
- **MongoConnectionFactory**: Connection management and database access
- **MongoSessionManager**: Session and transaction management
- **MongoHealthMonitor**: Connection health monitoring

### Design Patterns

- **Provider Pattern**: Consistent with existing SQL providers
- **Factory Pattern**: For creating provider instances
- **Repository Pattern**: High-level data access abstractions
- **Strategy Pattern**: For different data transformation strategies

## API Reference

### IMongoDataProvider

The main interface combining all MongoDB operations:

```csharp
public interface IMongoDataProvider : IMongoExecute, IMongoExpandoObjectDataProvider, IMongoGenericDataProvider
{
}
```

### IMongoGenericDataProvider

Strongly-typed data access operations:

```csharp
// Query operations
Task<IList<T>> GetListOfAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();
Task<IList<T>> GetListOfAsync<T>(string collection, FilterDefinition<T> filter, IList<IDataTransformColumnDefinition> transforms, CancellationToken cancellationToken = default) where T : new();
Task<T> GetValueOfAsync<T>(string collection, string field, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();

// Insert operations
Task<T> InsertOneAsync<T>(string collection, T document, CancellationToken cancellationToken = default) where T : new();
Task<IList<T>> InsertManyAsync<T>(string collection, IList<T> documents, CancellationToken cancellationToken = default) where T : new();

// Update operations
Task<ReplaceOneResult> ReplaceOneAsync<T>(string collection, FilterDefinition<T> filter, T replacement, CancellationToken cancellationToken = default) where T : new();

// Delete operations
Task<DeleteResult> DeleteOneAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();
Task<DeleteResult> DeleteManyAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();
```

### IMongoExpandoObjectDataProvider

Dynamic object mapping operations:

```csharp
// Query operations
Task<IList<ExpandoObject>> GetExpandoListAsync(string collection, FilterDefinition<BsonDocument> filter, CancellationToken cancellationToken = default);
Task<IList<ExpandoObject>> GetExpandoListAsync(string collection, FilterDefinition<BsonDocument> filter, IList<IDataTransformColumnDefinition> transforms, CancellationToken cancellationToken = default);

// Insert operations
Task<ExpandoObject> InsertOneExpandoAsync(string collection, ExpandoObject document, CancellationToken cancellationToken = default);
Task<IList<ExpandoObject>> InsertManyExpandoAsync(string collection, IList<ExpandoObject> documents, CancellationToken cancellationToken = default);
```

### IMongoExecute

Non-query operations:

```csharp
// Update operations
Task ExecuteAsync(string collection, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, CancellationToken cancellationToken = default);

// Delete operations
Task ExecuteAsync(string collection, FilterDefinition<BsonDocument> filter, CancellationToken cancellationToken = default);

// Bulk operations
Task<BulkWriteResult<BsonDocument>> ExecuteBulkWriteAsync(string collection, IList<WriteModel<BsonDocument>> requests, CancellationToken cancellationToken = default);
```

## Examples

### Basic CRUD Operations

```csharp
using AMCode.Data.MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;

// Create provider
var factory = new MongoDataProviderFactory();
var provider = factory.CreateProvider("mongodb://localhost:27017");

// Create a document
var user = new UserDocument
{
    Name = "John Doe",
    Email = "john@example.com",
    Age = 30,
    CreatedAt = DateTime.UtcNow
};

// Insert document
var insertedUser = await provider.InsertOneAsync("users", user);

// Query documents
var filter = Builders<UserDocument>.Filter.Eq(x => x.Name, "John Doe");
var users = await provider.GetListOfAsync("users", filter);

// Update document
var updateFilter = Builders<UserDocument>.Filter.Eq(x => x.Id, insertedUser.Id);
var updatedUser = await provider.ReplaceOneAsync("users", updateFilter, user);

// Delete document
var deleteFilter = Builders<UserDocument>.Filter.Eq(x => x.Id, insertedUser.Id);
var deleteResult = await provider.DeleteOneAsync("users", deleteFilter);
```

### Dynamic Object Operations

```csharp
// Create dynamic document
var dynamicUser = new ExpandoObject();
var userDict = (IDictionary<string, object>)dynamicUser;
userDict["name"] = "Jane Doe";
userDict["email"] = "jane@example.com";
userDict["age"] = 25;
userDict["createdAt"] = DateTime.UtcNow;

// Insert dynamic document
var insertedDynamicUser = await provider.InsertOneExpandoAsync("users", dynamicUser);

// Query dynamic documents
var dynamicFilter = Builders<BsonDocument>.Filter.Eq("name", "Jane Doe");
var dynamicUsers = await provider.GetExpandoListAsync("users", dynamicFilter);

// Access dynamic properties
foreach (var user in dynamicUsers)
{
    var userDict = (IDictionary<string, object>)user;
    Console.WriteLine($"Name: {userDict["name"]}, Email: {userDict["email"]}");
}
```

### Data Transformation

```csharp
// Define transformations
var transforms = new List<IDataTransformColumnDefinition>
{
    new DocumentTransformDefinition("_id", "Id", isRequired: true),
    new DocumentTransformDefinition("user_name", "Name", isRequired: true),
    new DocumentTransformDefinition("email_address", "Email", isRequired: true),
    new DocumentTransformDefinition("age_years", "Age", isRequired: false, defaultValue: 0)
};

// Query with transformations
var filter = Builders<UserDocument>.Filter.Empty;
var transformedUsers = await provider.GetListOfAsync("users", filter, transforms);
```

### Bulk Operations

```csharp
// Prepare bulk operations
var bulkRequests = new List<WriteModel<BsonDocument>>
{
    new InsertOneModel<BsonDocument>(new BsonDocument { { "name", "User1" }, { "email", "user1@example.com" } }),
    new InsertOneModel<BsonDocument>(new BsonDocument { { "name", "User2" }, { "email", "user2@example.com" } }),
    new UpdateOneModel<BsonDocument>(
        Builders<BsonDocument>.Filter.Eq("name", "User1"),
        Builders<BsonDocument>.Update.Set("status", "active")
    )
};

// Execute bulk operations
var bulkResult = await provider.ExecuteBulkWriteAsync("users", bulkRequests);
Console.WriteLine($"Inserted: {bulkResult.InsertedCount}, Modified: {bulkResult.ModifiedCount}");
```

### Session and Transaction Support

```csharp
// Create session manager
var connectionFactory = new MongoConnectionFactory("mongodb://localhost:27017");
var sessionManager = new MongoSessionManager(connectionFactory.GetClient());

// Use transaction
using var session = await sessionManager.CreateSessionAsync();
await session.StartTransactionAsync();

try
{
    // Perform operations within transaction
    var user = new UserDocument { Name = "Transaction User", Email = "tx@example.com" };
    await provider.InsertOneAsync("users", user);
    
    var profile = new ProfileDocument { UserId = user.Id, Bio = "Test bio" };
    await provider.InsertOneAsync("profiles", profile);
    
    // Commit transaction
    await session.CommitTransactionAsync();
}
catch (Exception ex)
{
    // Rollback transaction
    await session.AbortTransactionAsync();
    throw;
}
```

## Error Handling

The MongoDB integration provides comprehensive error handling with custom exceptions:

```csharp
try
{
    var users = await provider.GetListOfAsync<UserDocument>("users", filter);
}
catch (MongoConnectionException ex)
{
    // Handle connection issues
    Console.WriteLine($"Connection error: {ex.Message}");
}
catch (MongoQueryException ex)
{
    // Handle query issues
    Console.WriteLine($"Query error: {ex.Message}");
}
catch (MongoTransformationException ex)
{
    // Handle data transformation issues
    Console.WriteLine($"Transformation error: {ex.Message}");
}
```

## Performance Considerations

### Connection Pooling

The MongoDB integration leverages the MongoDB driver's built-in connection pooling:

```csharp
// Connection string with pooling options
var connectionString = "mongodb://localhost:27017/?maxPoolSize=100&minPoolSize=10";
var provider = factory.CreateProvider(connectionString);
```

### Async Operations

All operations are asynchronous and support cancellation:

```csharp
using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));

try
{
    var users = await provider.GetListOfAsync<UserDocument>("users", filter, cancellationTokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was cancelled");
}
```

### Health Monitoring

Monitor connection health:

```csharp
var healthMonitor = new MongoHealthMonitor(connectionFactory.GetClient());
var isHealthy = await healthMonitor.IsHealthyAsync();

if (!isHealthy)
{
    Console.WriteLine("MongoDB connection is not healthy");
}
```

## Testing

### Unit Testing

```csharp
[Test]
public async Task GetListOfAsync_WithValidFilter_ShouldReturnDocuments()
{
    // Arrange
    var mockCollection = new Mock<IMongoCollection<UserDocument>>();
    var mockCursor = new Mock<IAsyncCursor<UserDocument>>();
    var testDocuments = new List<UserDocument> { new UserDocument { Name = "Test User" } };
    
    mockCursor.Setup(x => x.Current).Returns(testDocuments);
    mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
              .Returns(true)
              .Returns(false);
    
    mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<UserDocument>>(), 
                                        It.IsAny<FindOptions<UserDocument, UserDocument>>(), 
                                        It.IsAny<CancellationToken>()))
                  .ReturnsAsync(mockCursor.Object);
    
    // Act
    var result = await provider.GetListOfAsync("users", filter);
    
    // Assert
    Assert.That(result, Has.Count.EqualTo(1));
    Assert.That(result[0].Name, Is.EqualTo("Test User"));
}
```

### Integration Testing

```csharp
[Test]
public async Task IntegrationTest_CRUDOperations_ShouldWorkCorrectly()
{
    // Arrange
    var testUser = new UserDocument
    {
        Name = "Integration Test User",
        Email = "integration@test.com",
        Age = 25
    };
    
    // Act & Assert
    var insertedUser = await provider.InsertOneAsync("test_users", testUser);
    Assert.That(insertedUser.Id, Is.Not.Null);
    
    var retrievedUsers = await provider.GetListOfAsync<UserDocument>("test_users", 
        Builders<UserDocument>.Filter.Eq(x => x.Id, insertedUser.Id));
    Assert.That(retrievedUsers, Has.Count.EqualTo(1));
    
    var deleteResult = await provider.DeleteOneAsync("test_users", 
        Builders<UserDocument>.Filter.Eq(x => x.Id, insertedUser.Id));
    Assert.That(deleteResult.DeletedCount, Is.EqualTo(1));
}
```

## Best Practices

### Document Design

1. **Use Strongly-Typed Models**: Define clear document models with proper BSON attributes
2. **Consistent Naming**: Use consistent field naming conventions
3. **Index Strategy**: Create appropriate indexes for query performance
4. **Schema Validation**: Use MongoDB schema validation for data integrity

### Performance Optimization

1. **Connection Pooling**: Configure appropriate pool sizes
2. **Query Optimization**: Use efficient filters and projections
3. **Bulk Operations**: Use bulk operations for multiple documents
4. **Async Patterns**: Always use async/await for I/O operations

### Error Handling

1. **Specific Exceptions**: Catch specific MongoDB exceptions
2. **Retry Logic**: Implement retry logic for transient failures
3. **Logging**: Use structured logging for debugging
4. **Graceful Degradation**: Handle partial failures gracefully

## Migration from Raw MongoDB Driver

### Before (Raw Driver)

```csharp
var client = new MongoClient("mongodb://localhost:27017");
var database = client.GetDatabase("mydb");
var collection = database.GetCollection<BsonDocument>("users");

var filter = Builders<BsonDocument>.Filter.Eq("name", "John");
var documents = await collection.Find(filter).ToListAsync();

foreach (var doc in documents)
{
    var name = doc["name"].AsString;
    var email = doc["email"].AsString;
    // Manual property access...
}
```

### After (AMCode.Data MongoDB)

```csharp
var factory = new MongoDataProviderFactory();
var provider = factory.CreateProvider("mongodb://localhost:27017");

var filter = Builders<UserDocument>.Filter.Eq(x => x.Name, "John");
var users = await provider.GetListOfAsync<UserDocument>("users", filter);

foreach (var user in users)
{
    var name = user.Name;  // Strongly-typed access
    var email = user.Email;
    // Type-safe property access...
}
```

## Troubleshooting

### Common Issues

1. **Connection Failures**
   - Verify MongoDB server is running
   - Check connection string format
   - Ensure network connectivity

2. **Authentication Errors**
   - Verify username/password
   - Check database permissions
   - Ensure authentication database is correct

3. **Query Performance**
   - Check index usage
   - Optimize filter conditions
   - Use projections for large documents

4. **Memory Issues**
   - Use pagination for large result sets
   - Dispose of cursors properly
   - Monitor memory usage patterns

### Debugging

Enable detailed logging:

```csharp
var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
var logger = loggerFactory.CreateLogger<MongoDataProvider>();

var provider = new MongoDataProvider(connectionFactory, sessionManager, healthMonitor, logger);
```

## Contributing

When contributing to the MongoDB integration:

1. Follow existing code patterns and conventions
2. Add comprehensive unit tests for new features
3. Update documentation for API changes
4. Ensure backward compatibility
5. Follow the established error handling patterns

## License

This MongoDB integration is part of the AMCode.Data library and follows the same licensing terms.
