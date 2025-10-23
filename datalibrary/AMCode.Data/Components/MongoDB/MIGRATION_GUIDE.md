# Migration Guide: From Raw MongoDB Driver to AMCode.Data MongoDB Integration

This guide helps you migrate from using the raw MongoDB C# driver to the AMCode.Data MongoDB integration, providing a consistent API that mirrors the existing SQL provider capabilities.

## Overview

The AMCode.Data MongoDB integration provides:
- **Consistent API**: Same patterns as existing SQL providers
- **Strongly-Typed Operations**: Type-safe document operations
- **Dynamic Object Support**: Flexible ExpandoObject operations
- **Data Transformation**: Built-in field mapping and transformations
- **Enhanced Error Handling**: Custom exceptions with detailed context
- **Connection Management**: Robust connection pooling and health monitoring
- **Session Support**: Built-in transaction management

## Migration Benefits

### Before (Raw MongoDB Driver)
- Manual connection management
- Verbose query building
- Manual type conversion
- Basic error handling
- No transaction abstraction
- Inconsistent patterns

### After (AMCode.Data MongoDB)
- Automatic connection management
- Fluent query building
- Automatic type conversion
- Comprehensive error handling
- Built-in transaction support
- Consistent patterns with SQL providers

## Step-by-Step Migration

### Step 1: Update Dependencies

#### Before
```xml
<PackageReference Include="MongoDB.Driver" Version="2.19.0" />
```

#### After
```xml
<PackageReference Include="AMCode.Data" Version="1.1.0" />
<!-- MongoDB.Driver is included as a dependency -->
```

### Step 2: Update Using Statements

#### Before
```csharp
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
```

#### After
```csharp
using AMCode.Data.MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
```

### Step 3: Replace Connection Management

#### Before
```csharp
public class UserRepository
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<UserDocument> _collection;

    public UserRepository(string connectionString)
    {
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase("mydb");
        _collection = _database.GetCollection<UserDocument>("users");
    }
}
```

#### After
```csharp
public class UserRepository
{
    private readonly IMongoDataProvider _provider;

    public UserRepository(string connectionString)
    {
        var factory = new MongoDataProviderFactory();
        _provider = factory.CreateProvider(connectionString);
    }
}
```

### Step 4: Migrate CRUD Operations

#### Insert Operations

##### Before
```csharp
public async Task<UserDocument> CreateUserAsync(UserDocument user)
{
    await _collection.InsertOneAsync(user);
    return user;
}

public async Task CreateUsersAsync(IEnumerable<UserDocument> users)
{
    await _collection.InsertManyAsync(users);
}
```

##### After
```csharp
public async Task<UserDocument> CreateUserAsync(UserDocument user)
{
    return await _provider.InsertOneAsync("users", user);
}

public async Task<IList<UserDocument>> CreateUsersAsync(IList<UserDocument> users)
{
    return await _provider.InsertManyAsync("users", users);
}
```

#### Query Operations

##### Before
```csharp
public async Task<List<UserDocument>> GetUsersAsync()
{
    return await _collection.Find(_ => true).ToListAsync();
}

public async Task<UserDocument> GetUserByIdAsync(string id)
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
    return await _collection.Find(filter).FirstOrDefaultAsync();
}

public async Task<List<UserDocument>> GetActiveUsersAsync()
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.IsActive, true);
    return await _collection.Find(filter).ToListAsync();
}
```

##### After
```csharp
public async Task<IList<UserDocument>> GetUsersAsync()
{
    var filter = Builders<UserDocument>.Filter.Empty;
    return await _provider.GetListOfAsync("users", filter);
}

public async Task<UserDocument> GetUserByIdAsync(string id)
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
    var users = await _provider.GetListOfAsync("users", filter);
    return users.FirstOrDefault();
}

public async Task<IList<UserDocument>> GetActiveUsersAsync()
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.IsActive, true);
    return await _provider.GetListOfAsync("users", filter);
}
```

#### Update Operations

##### Before
```csharp
public async Task<bool> UpdateUserAsync(string id, UserDocument user)
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
    var result = await _collection.ReplaceOneAsync(filter, user);
    return result.ModifiedCount > 0;
}

public async Task<long> UpdateUserAgeAsync(string id, int newAge)
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
    var update = Builders<UserDocument>.Update.Set(x => x.Age, newAge);
    var result = await _collection.UpdateOneAsync(filter, update);
    return result.ModifiedCount;
}
```

##### After
```csharp
public async Task<bool> UpdateUserAsync(string id, UserDocument user)
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
    var result = await _provider.ReplaceOneAsync("users", filter, user);
    return result.ModifiedCount > 0;
}

public async Task<long> UpdateUserAgeAsync(string id, int newAge)
{
    var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
    var update = Builders<BsonDocument>.Update.Set("age", newAge);
    await _provider.ExecuteAsync("users", filter, update);
    return 1; // AMCode.Data doesn't return modified count for ExecuteAsync
}
```

#### Delete Operations

##### Before
```csharp
public async Task<bool> DeleteUserAsync(string id)
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
    var result = await _collection.DeleteOneAsync(filter);
    return result.DeletedCount > 0;
}

public async Task<long> DeleteInactiveUsersAsync()
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.IsActive, false);
    var result = await _collection.DeleteManyAsync(filter);
    return result.DeletedCount;
}
```

##### After
```csharp
public async Task<bool> DeleteUserAsync(string id)
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
    var result = await _provider.DeleteOneAsync("users", filter);
    return result.DeletedCount > 0;
}

public async Task<long> DeleteInactiveUsersAsync()
{
    var filter = Builders<UserDocument>.Filter.Eq(x => x.IsActive, false);
    var result = await _provider.DeleteManyAsync("users", filter);
    return result.DeletedCount;
}
```

### Step 5: Migrate Dynamic Object Operations

#### Before
```csharp
public async Task<List<BsonDocument>> GetDynamicUsersAsync()
{
    var collection = _database.GetCollection<BsonDocument>("users");
    return await collection.Find(_ => true).ToListAsync();
}

public async Task<BsonDocument> CreateDynamicUserAsync(Dictionary<string, object> userData)
{
    var collection = _database.GetCollection<BsonDocument>("users");
    var document = new BsonDocument(userData);
    await collection.InsertOneAsync(document);
    return document;
}
```

#### After
```csharp
public async Task<IList<ExpandoObject>> GetDynamicUsersAsync()
{
    var filter = Builders<BsonDocument>.Filter.Empty;
    return await _provider.GetExpandoListAsync("users", filter);
}

public async Task<ExpandoObject> CreateDynamicUserAsync(Dictionary<string, object> userData)
{
    var expando = new ExpandoObject();
    var expandoDict = (IDictionary<string, object>)expando;
    
    foreach (var kvp in userData)
    {
        expandoDict[kvp.Key] = kvp.Value;
    }
    
    return await _provider.InsertOneExpandoAsync("users", expando);
}
```

### Step 6: Migrate Bulk Operations

#### Before
```csharp
public async Task<BulkWriteResult<UserDocument>> BulkUpdateUsersAsync(List<UserDocument> users)
{
    var requests = new List<WriteModel<UserDocument>>();
    
    foreach (var user in users)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, user.Id);
        var update = Builders<UserDocument>.Update
            .Set(x => x.Name, user.Name)
            .Set(x => x.Email, user.Email)
            .Set(x => x.Age, user.Age);
        
        requests.Add(new UpdateOneModel<UserDocument>(filter, update));
    }
    
    return await _collection.BulkWriteAsync(requests);
}
```

#### After
```csharp
public async Task<BulkWriteResult<BsonDocument>> BulkUpdateUsersAsync(List<UserDocument> users)
{
    var requests = new List<WriteModel<BsonDocument>>();
    
    foreach (var user in users)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(user.Id));
        var update = Builders<BsonDocument>.Update
            .Set("name", user.Name)
            .Set("email", user.Email)
            .Set("age", user.Age);
        
        requests.Add(new UpdateOneModel<BsonDocument>(filter, update));
    }
    
    return await _provider.ExecuteBulkWriteAsync("users", requests);
}
```

### Step 7: Migrate Transaction Support

#### Before
```csharp
public async Task<bool> CreateUserWithProfileAsync(UserDocument user, ProfileDocument profile)
{
    using var session = await _client.StartSessionAsync();
    session.StartTransaction();
    
    try
    {
        await _collection.InsertOneAsync(session, user);
        await _profileCollection.InsertOneAsync(session, profile);
        
        await session.CommitTransactionAsync();
        return true;
    }
    catch
    {
        await session.AbortTransactionAsync();
        return false;
    }
}
```

#### After
```csharp
public async Task<bool> CreateUserWithProfileAsync(UserDocument user, ProfileDocument profile)
{
    var connectionFactory = new MongoConnectionFactory("mongodb://localhost:27017");
    var sessionManager = new MongoSessionManager(connectionFactory.GetClient());
    
    using var session = await sessionManager.CreateSessionAsync();
    await session.StartTransactionAsync();
    
    try
    {
        await _provider.InsertOneAsync("users", user);
        await _provider.InsertOneAsync("profiles", profile);
        
        await session.CommitTransactionAsync();
        return true;
    }
    catch
    {
        await session.AbortTransactionAsync();
        return false;
    }
}
```

### Step 8: Migrate Error Handling

#### Before
```csharp
public async Task<UserDocument> GetUserAsync(string id)
{
    try
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    catch (MongoConnectionException ex)
    {
        // Handle connection issues
        throw new InvalidOperationException("Database connection failed", ex);
    }
    catch (MongoQueryException ex)
    {
        // Handle query issues
        throw new InvalidOperationException("Query execution failed", ex);
    }
    catch (Exception ex)
    {
        // Handle other issues
        throw new InvalidOperationException("Unexpected error occurred", ex);
    }
}
```

#### After
```csharp
public async Task<UserDocument> GetUserAsync(string id)
{
    try
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
        var users = await _provider.GetListOfAsync("users", filter);
        return users.FirstOrDefault();
    }
    catch (MongoConnectionException ex)
    {
        // AMCode.Data provides specific MongoDB exceptions
        throw new InvalidOperationException("Database connection failed", ex);
    }
    catch (MongoQueryException ex)
    {
        // AMCode.Data provides specific MongoDB exceptions
        throw new InvalidOperationException("Query execution failed", ex);
    }
    catch (MongoTransformationException ex)
    {
        // AMCode.Data provides transformation-specific exceptions
        throw new InvalidOperationException("Data transformation failed", ex);
    }
    catch (Exception ex)
    {
        // Handle other issues
        throw new InvalidOperationException("Unexpected error occurred", ex);
    }
}
```

## Advanced Migration Patterns

### Repository Pattern Migration

#### Before
```csharp
public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<UserDocument> _collection;

    public UserRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<UserDocument>("users");
    }

    public async Task<UserDocument> GetByIdAsync(string id)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<UserDocument>> GetByEmailAsync(string email)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Email, email);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<UserDocument> CreateAsync(UserDocument user)
    {
        await _collection.InsertOneAsync(user);
        return user;
    }

    public async Task<bool> UpdateAsync(UserDocument user)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, user.Id);
        var result = await _collection.ReplaceOneAsync(filter, user);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
}
```

#### After
```csharp
public class UserRepository : IUserRepository
{
    private readonly IMongoDataProvider _provider;

    public UserRepository(IMongoDataProvider provider)
    {
        _provider = provider;
    }

    public async Task<UserDocument> GetByIdAsync(string id)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
        var users = await _provider.GetListOfAsync("users", filter);
        return users.FirstOrDefault();
    }

    public async Task<IList<UserDocument>> GetByEmailAsync(string email)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Email, email);
        return await _provider.GetListOfAsync("users", filter);
    }

    public async Task<UserDocument> CreateAsync(UserDocument user)
    {
        return await _provider.InsertOneAsync("users", user);
    }

    public async Task<bool> UpdateAsync(UserDocument user)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, user.Id);
        var result = await _provider.ReplaceOneAsync("users", filter, user);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Id, id);
        var result = await _provider.DeleteOneAsync("users", filter);
        return result.DeletedCount > 0;
    }
}
```

### Service Layer Migration

#### Before
```csharp
public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMongoCollection<BsonDocument> _auditCollection;

    public UserService(IUserRepository userRepository, IMongoDatabase database)
    {
        _userRepository = userRepository;
        _auditCollection = database.GetCollection<BsonDocument>("audit_logs");
    }

    public async Task<UserDocument> CreateUserAsync(CreateUserRequest request)
    {
        var user = new UserDocument
        {
            Name = request.Name,
            Email = request.Email,
            Age = request.Age,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser = await _userRepository.CreateAsync(user);

        // Log audit
        var auditLog = new BsonDocument
        {
            { "action", "CREATE_USER" },
            { "userId", createdUser.Id },
            { "timestamp", DateTime.UtcNow },
            { "details", new BsonDocument { { "email", request.Email } } }
        };
        await _auditCollection.InsertOneAsync(auditLog);

        return createdUser;
    }
}
```

#### After
```csharp
public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMongoDataProvider _provider;

    public UserService(IUserRepository userRepository, IMongoDataProvider provider)
    {
        _userRepository = userRepository;
        _provider = provider;
    }

    public async Task<UserDocument> CreateUserAsync(CreateUserRequest request)
    {
        var user = new UserDocument
        {
            Name = request.Name,
            Email = request.Email,
            Age = request.Age,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser = await _userRepository.CreateAsync(user);

        // Log audit using ExpandoObject
        var auditLog = new ExpandoObject();
        var auditDict = (IDictionary<string, object>)auditLog;
        auditDict["action"] = "CREATE_USER";
        auditDict["userId"] = createdUser.Id;
        auditDict["timestamp"] = DateTime.UtcNow;
        auditDict["details"] = new Dictionary<string, object> { { "email", request.Email } };
        
        await _provider.InsertOneExpandoAsync("audit_logs", auditLog);

        return createdUser;
    }
}
```

## Performance Considerations

### Connection String Optimization

#### Before
```csharp
var client = new MongoClient("mongodb://localhost:27017");
```

#### After
```csharp
var connectionString = "mongodb://localhost:27017/?maxPoolSize=100&minPoolSize=10&maxIdleTimeMS=30000";
var factory = new MongoDataProviderFactory();
var provider = factory.CreateProvider(connectionString);
```

### Query Optimization

#### Before
```csharp
// Manual projection
var projection = Builders<UserDocument>.Projection
    .Include(x => x.Name)
    .Include(x => x.Email)
    .Exclude(x => x.Id);
var users = await _collection.Find(filter).Project(projection).ToListAsync();
```

#### After
```csharp
// Use data transformation for field selection
var transforms = new List<IDataTransformColumnDefinition>
{
    new DocumentTransformDefinition("name", "Name"),
    new DocumentTransformDefinition("email", "Email")
};
var users = await _provider.GetListOfAsync("users", filter, transforms);
```

## Testing Migration

### Unit Test Migration

#### Before
```csharp
[Test]
public async Task GetUserById_ShouldReturnUser()
{
    // Arrange
    var mockCollection = new Mock<IMongoCollection<UserDocument>>();
    var mockCursor = new Mock<IAsyncCursor<UserDocument>>();
    var testUser = new UserDocument { Id = "123", Name = "Test User" };
    
    mockCursor.Setup(x => x.Current).Returns(new[] { testUser });
    mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>()))
              .Returns(true)
              .Returns(false);
    
    mockCollection.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<UserDocument>>(), 
                                        It.IsAny<FindOptions<UserDocument, UserDocument>>(), 
                                        It.IsAny<CancellationToken>()))
                  .ReturnsAsync(mockCursor.Object);
    
    var repository = new UserRepository(mockCollection.Object);
    
    // Act
    var result = await repository.GetByIdAsync("123");
    
    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Name, Is.EqualTo("Test User"));
}
```

#### After
```csharp
[Test]
public async Task GetUserById_ShouldReturnUser()
{
    // Arrange
    var mockProvider = new Mock<IMongoDataProvider>();
    var testUser = new UserDocument { Id = "123", Name = "Test User" };
    
    mockProvider.Setup(x => x.GetListOfAsync<UserDocument>(It.IsAny<string>(), 
                                                         It.IsAny<FilterDefinition<UserDocument>>(), 
                                                         It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UserDocument> { testUser });
    
    var repository = new UserRepository(mockProvider.Object);
    
    // Act
    var result = await repository.GetByIdAsync("123");
    
    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Name, Is.EqualTo("Test User"));
}
```

## Common Migration Issues and Solutions

### Issue 1: Collection Name Management

**Problem**: Raw driver uses collection objects, AMCode.Data uses string names.

**Solution**: Use consistent collection naming strategy.

```csharp
// Define collection names as constants
public static class CollectionNames
{
    public const string Users = "users";
    public const string Profiles = "profiles";
    public const string AuditLogs = "audit_logs";
}
```

### Issue 2: Type Conversion Differences

**Problem**: Different type conversion behavior between raw driver and AMCode.Data.

**Solution**: Use explicit type conversion when needed.

```csharp
// For complex type conversions, use custom converters
public class CustomBsonConverter
{
    public static BsonDocument ToBsonDocument<T>(T obj)
    {
        return obj.ToBsonDocument();
    }
    
    public static T FromBsonDocument<T>(BsonDocument doc)
    {
        return BsonSerializer.Deserialize<T>(doc);
    }
}
```

### Issue 3: Error Handling Differences

**Problem**: Different exception types and error contexts.

**Solution**: Update error handling to use AMCode.Data exceptions.

```csharp
try
{
    var result = await _provider.GetListOfAsync("users", filter);
}
catch (MongoConnectionException ex)
{
    // Handle connection issues
    _logger.LogError(ex, "MongoDB connection failed");
    throw new ServiceException("Database unavailable", ex);
}
catch (MongoQueryException ex)
{
    // Handle query issues
    _logger.LogError(ex, "MongoDB query failed");
    throw new ServiceException("Query execution failed", ex);
}
```

## Migration Checklist

- [ ] Update NuGet packages
- [ ] Replace connection management
- [ ] Migrate CRUD operations
- [ ] Update error handling
- [ ] Migrate bulk operations
- [ ] Update transaction support
- [ ] Update unit tests
- [ ] Update integration tests
- [ ] Performance testing
- [ ] Documentation updates

## Post-Migration Validation

1. **Functional Testing**: Verify all operations work correctly
2. **Performance Testing**: Ensure performance is maintained or improved
3. **Error Handling**: Test error scenarios and exception handling
4. **Integration Testing**: Test with real MongoDB instances
5. **Load Testing**: Verify performance under load

## Support and Resources

- **Documentation**: See AMCode.Data MongoDB README
- **Examples**: Check the Examples directory
- **Issues**: Report issues through the project repository
- **Community**: Join the AMCode community discussions

This migration guide provides a comprehensive path from raw MongoDB driver usage to the AMCode.Data MongoDB integration, ensuring a smooth transition while gaining the benefits of a consistent, well-architected data access layer.
