# MongoDB Integration Examples

This directory contains comprehensive examples demonstrating how to use the AMCode.Data MongoDB integration for various scenarios.

## Examples Overview

- **Basic Examples**: Simple CRUD operations and common patterns
- **Advanced Examples**: Complex scenarios with transactions, bulk operations, and performance optimization
- **Integration Examples**: Real-world integration patterns and best practices
- **Performance Examples**: Optimization techniques and benchmarking

## Basic Examples

### Example 1: Simple CRUD Operations

```csharp
using AMCode.Data.MongoDB;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

// Define a simple document model
public class UserDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("email")]
    public string Email { get; set; }

    [BsonElement("age")]
    public int Age { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; }
}

public class BasicCrudExample
{
    private readonly IMongoDataProvider _provider;

    public BasicCrudExample()
    {
        var factory = new MongoDataProviderFactory();
        _provider = factory.CreateProvider("mongodb://localhost:27017");
    }

    public async Task RunExample()
    {
        // Create a new user
        var user = new UserDocument
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Age = 30,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Insert the user
        var insertedUser = await _provider.InsertOneAsync("users", user);
        Console.WriteLine($"Inserted user with ID: {insertedUser.Id}");

        // Query users by name
        var filter = Builders<UserDocument>.Filter.Eq(x => x.Name, "John Doe");
        var users = await _provider.GetListOfAsync("users", filter);
        Console.WriteLine($"Found {users.Count} users with name 'John Doe'");

        // Update user age
        user.Age = 31;
        var updateFilter = Builders<UserDocument>.Filter.Eq(x => x.Id, insertedUser.Id);
        var updateResult = await _provider.ReplaceOneAsync("users", updateFilter, user);
        Console.WriteLine($"Updated {updateResult.ModifiedCount} user");

        // Delete user
        var deleteResult = await _provider.DeleteOneAsync("users", updateFilter);
        Console.WriteLine($"Deleted {deleteResult.DeletedCount} user");
    }
}
```

### Example 2: Dynamic Object Operations

```csharp
using System.Dynamic;
using MongoDB.Driver;
using MongoDB.Bson;

public class DynamicObjectExample
{
    private readonly IMongoDataProvider _provider;

    public DynamicObjectExample()
    {
        var factory = new MongoDataProviderFactory();
        _provider = factory.CreateProvider("mongodb://localhost:27017");
    }

    public async Task RunExample()
    {
        // Create dynamic documents
        var users = new List<ExpandoObject>();
        
        for (int i = 1; i <= 5; i++)
        {
            var user = new ExpandoObject();
            var userDict = (IDictionary<string, object>)user;
            
            userDict["name"] = $"User {i}";
            userDict["email"] = $"user{i}@example.com";
            userDict["age"] = 20 + i;
            userDict["department"] = i % 2 == 0 ? "Engineering" : "Marketing";
            userDict["createdAt"] = DateTime.UtcNow.AddDays(-i);
            userDict["isActive"] = true;
            
            users.Add(user);
        }

        // Insert multiple dynamic documents
        var insertedUsers = await _provider.InsertManyExpandoAsync("dynamic_users", users);
        Console.WriteLine($"Inserted {insertedUsers.Count} dynamic users");

        // Query dynamic documents
        var filter = Builders<BsonDocument>.Filter.Eq("department", "Engineering");
        var engineeringUsers = await _provider.GetExpandoListAsync("dynamic_users", filter);
        
        Console.WriteLine($"Found {engineeringUsers.Count} engineering users:");
        foreach (var user in engineeringUsers)
        {
            var userDict = (IDictionary<string, object>)user;
            Console.WriteLine($"- {userDict["name"]} ({userDict["email"]})");
        }

        // Query with range filter
        var ageFilter = Builders<BsonDocument>.Filter.Gte("age", 22) & 
                       Builders<BsonDocument>.Filter.Lte("age", 25);
        var ageRangeUsers = await _provider.GetExpandoListAsync("dynamic_users", ageFilter);
        Console.WriteLine($"Found {ageRangeUsers.Count} users aged 22-25");
    }
}
```

## Advanced Examples

### Example 3: Transaction Support

```csharp
using AMCode.Data.MongoDB.Components.Connection;

public class TransactionExample
{
    private readonly IMongoDataProvider _provider;
    private readonly IMongoSessionManager _sessionManager;

    public TransactionExample()
    {
        var factory = new MongoDataProviderFactory();
        _provider = factory.CreateProvider("mongodb://localhost:27017");
        
        var connectionFactory = new MongoConnectionFactory("mongodb://localhost:27017");
        _sessionManager = new MongoSessionManager(connectionFactory.GetClient());
    }

    public async Task RunExample()
    {
        using var session = await _sessionManager.CreateSessionAsync();
        
        try
        {
            await session.StartTransactionAsync();
            Console.WriteLine("Transaction started");

            // Create user
            var user = new UserDocument
            {
                Name = "Transaction User",
                Email = "tx@example.com",
                Age = 25,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var insertedUser = await _provider.InsertOneAsync("users", user);
            Console.WriteLine($"Created user: {insertedUser.Id}");

            // Create user profile
            var profile = new ExpandoObject();
            var profileDict = (IDictionary<string, object>)profile;
            profileDict["userId"] = insertedUser.Id;
            profileDict["bio"] = "This is a test bio";
            profileDict["avatar"] = "https://example.com/avatar.jpg";
            profileDict["createdAt"] = DateTime.UtcNow;

            var insertedProfile = await _provider.InsertOneExpandoAsync("profiles", profile);
            Console.WriteLine($"Created profile for user: {insertedUser.Id}");

            // Create user settings
            var settings = new ExpandoObject();
            var settingsDict = (IDictionary<string, object>)settings;
            settingsDict["userId"] = insertedUser.Id;
            settingsDict["notifications"] = true;
            settingsDict["theme"] = "dark";
            settingsDict["language"] = "en";

            var insertedSettings = await _provider.InsertOneExpandoAsync("settings", settings);
            Console.WriteLine($"Created settings for user: {insertedUser.Id}");

            // Commit transaction
            await session.CommitTransactionAsync();
            Console.WriteLine("Transaction committed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Transaction failed: {ex.Message}");
            await session.AbortTransactionAsync();
            Console.WriteLine("Transaction aborted");
            throw;
        }
    }
}
```

### Example 4: Bulk Operations

```csharp
using MongoDB.Driver;
using MongoDB.Bson;

public class BulkOperationsExample
{
    private readonly IMongoDataProvider _provider;

    public BulkOperationsExample()
    {
        var factory = new MongoDataProviderFactory();
        _provider = factory.CreateProvider("mongodb://localhost:27017");
    }

    public async Task RunExample()
    {
        // Prepare bulk operations
        var bulkRequests = new List<WriteModel<BsonDocument>>();

        // Insert operations
        for (int i = 1; i <= 10; i++)
        {
            var document = new BsonDocument
            {
                { "name", $"Bulk User {i}" },
                { "email", $"bulk{i}@example.com" },
                { "age", 20 + i },
                { "department", i % 3 == 0 ? "Engineering" : i % 3 == 1 ? "Marketing" : "Sales" },
                { "createdAt", DateTime.UtcNow.AddDays(-i) },
                { "isActive", true }
            };
            bulkRequests.Add(new InsertOneModel<BsonDocument>(document));
        }

        // Update operations
        var updateFilter = Builders<BsonDocument>.Filter.Eq("department", "Engineering");
        var updateDefinition = Builders<BsonDocument>.Update.Set("bonus", 1000);
        bulkRequests.Add(new UpdateManyModel<BsonDocument>(updateFilter, updateDefinition));

        // Delete operations
        var deleteFilter = Builders<BsonDocument>.Filter.Lt("age", 22);
        bulkRequests.Add(new DeleteManyModel<BsonDocument>(deleteFilter));

        // Execute bulk operations
        Console.WriteLine($"Executing {bulkRequests.Count} bulk operations...");
        var bulkResult = await _provider.ExecuteBulkWriteAsync("bulk_users", bulkRequests);

        Console.WriteLine($"Bulk operation results:");
        Console.WriteLine($"- Inserted: {bulkResult.InsertedCount}");
        Console.WriteLine($"- Modified: {bulkResult.ModifiedCount}");
        Console.WriteLine($"- Deleted: {bulkResult.DeletedCount}");
        Console.WriteLine($"- Upserted: {bulkResult.UpsertedCount}");
    }
}
```

### Example 5: Data Transformation

```csharp
using AMCode.Columns.DataTransform;

public class DataTransformationExample
{
    private readonly IMongoDataProvider _provider;

    public DataTransformationExample()
    {
        var factory = new MongoDataProviderFactory();
        _provider = factory.CreateProvider("mongodb://localhost:27017");
    }

    public async Task RunExample()
    {
        // Insert test data with different field names
        var testDocuments = new List<ExpandoObject>();
        
        for (int i = 1; i <= 5; i++)
        {
            var doc = new ExpandoObject();
            var docDict = (IDictionary<string, object>)doc;
            
            docDict["_id"] = ObjectId.GenerateNewId().ToString();
            docDict["user_name"] = $"Transformed User {i}";
            docDict["email_address"] = $"transformed{i}@example.com";
            docDict["age_years"] = 25 + i;
            docDict["created_timestamp"] = DateTime.UtcNow.AddDays(-i);
            docDict["is_user_active"] = true;
            
            testDocuments.Add(doc);
        }

        await _provider.InsertManyExpandoAsync("transformed_users", testDocuments);
        Console.WriteLine("Inserted test documents with different field names");

        // Define transformations
        var transforms = new List<IDataTransformColumnDefinition>
        {
            new DocumentTransformDefinition("_id", "Id", isRequired: true),
            new DocumentTransformDefinition("user_name", "Name", isRequired: true),
            new DocumentTransformDefinition("email_address", "Email", isRequired: true),
            new DocumentTransformDefinition("age_years", "Age", isRequired: false, defaultValue: 0),
            new DocumentTransformDefinition("created_timestamp", "CreatedAt", isRequired: true),
            new DocumentTransformDefinition("is_user_active", "IsActive", isRequired: false, defaultValue: true)
        };

        // Query with transformations
        var filter = Builders<UserDocument>.Filter.Empty;
        var transformedUsers = await _provider.GetListOfAsync("transformed_users", filter, transforms);

        Console.WriteLine($"Retrieved {transformedUsers.Count} transformed users:");
        foreach (var user in transformedUsers)
        {
            Console.WriteLine($"- {user.Name} ({user.Email}) - Age: {user.Age}, Active: {user.IsActive}");
        }
    }
}
```

## Performance Examples

### Example 6: Connection Pooling and Performance

```csharp
using System.Diagnostics;
using AMCode.Data.MongoDB.Components.Connection;

public class PerformanceExample
{
    private readonly IMongoDataProvider _provider;
    private readonly MongoHealthMonitor _healthMonitor;

    public PerformanceExample()
    {
        // Use connection string with optimized pooling
        var connectionString = "mongodb://localhost:27017/?maxPoolSize=100&minPoolSize=10&maxIdleTimeMS=30000";
        var factory = new MongoDataProviderFactory();
        _provider = factory.CreateProvider(connectionString);
        
        var connectionFactory = new MongoConnectionFactory(connectionString);
        _healthMonitor = new MongoHealthMonitor(connectionFactory.GetClient());
    }

    public async Task RunExample()
    {
        // Check connection health
        var isHealthy = await _healthMonitor.IsHealthyAsync();
        Console.WriteLine($"MongoDB connection healthy: {isHealthy}");

        // Performance test: Insert multiple documents
        await PerformanceTest_InsertMany();
        
        // Performance test: Query with different filters
        await PerformanceTest_QueryOperations();
        
        // Performance test: Bulk operations
        await PerformanceTest_BulkOperations();
    }

    private async Task PerformanceTest_InsertMany()
    {
        Console.WriteLine("\n=== Insert Many Performance Test ===");
        
        var documents = new List<UserDocument>();
        for (int i = 1; i <= 1000; i++)
        {
            documents.Add(new UserDocument
            {
                Name = $"Perf User {i}",
                Email = $"perf{i}@example.com",
                Age = 20 + (i % 50),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            });
        }

        var stopwatch = Stopwatch.StartNew();
        var insertedDocs = await _provider.InsertManyAsync("perf_users", documents);
        stopwatch.Stop();

        Console.WriteLine($"Inserted {insertedDocs.Count} documents in {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Rate: {insertedDocs.Count * 1000.0 / stopwatch.ElapsedMilliseconds:F2} docs/sec");
    }

    private async Task PerformanceTest_QueryOperations()
    {
        Console.WriteLine("\n=== Query Performance Test ===");
        
        // Test different query types
        var queries = new[]
        {
            ("Simple Filter", Builders<UserDocument>.Filter.Eq(x => x.IsActive, true)),
            ("Range Filter", Builders<UserDocument>.Filter.Gte(x => x.Age, 25) & Builders<UserDocument>.Filter.Lte(x => x.Age, 35)),
            ("Regex Filter", Builders<UserDocument>.Filter.Regex(x => x.Name, new BsonRegularExpression("Perf User [1-9][0-9][0-9]")))
        };

        foreach (var (name, filter) in queries)
        {
            var stopwatch = Stopwatch.StartNew();
            var results = await _provider.GetListOfAsync("perf_users", filter);
            stopwatch.Stop();

            Console.WriteLine($"{name}: {results.Count} results in {stopwatch.ElapsedMilliseconds}ms");
        }
    }

    private async Task PerformanceTest_BulkOperations()
    {
        Console.WriteLine("\n=== Bulk Operations Performance Test ===");
        
        var bulkRequests = new List<WriteModel<BsonDocument>>();
        
        // Prepare bulk update operations
        for (int i = 1; i <= 100; i++)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("name", $"Perf User {i}");
            var update = Builders<BsonDocument>.Update.Set("lastUpdated", DateTime.UtcNow);
            bulkRequests.Add(new UpdateOneModel<BsonDocument>(filter, update));
        }

        var stopwatch = Stopwatch.StartNew();
        var bulkResult = await _provider.ExecuteBulkWriteAsync("perf_users", bulkRequests);
        stopwatch.Stop();

        Console.WriteLine($"Bulk updated {bulkResult.ModifiedCount} documents in {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"Rate: {bulkResult.ModifiedCount * 1000.0 / stopwatch.ElapsedMilliseconds:F2} ops/sec");
    }
}
```

## Integration Examples

### Example 7: Real-World Integration Pattern

```csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

public class RealWorldIntegrationExample
{
    private readonly IMongoDataProvider _provider;
    private readonly ILogger<RealWorldIntegrationExample> _logger;

    public RealWorldIntegrationExample(IMongoDataProvider provider, ILogger<RealWorldIntegrationExample> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<UserProfile> CreateUserProfileAsync(CreateUserRequest request)
    {
        try
        {
            _logger.LogInformation("Creating user profile for {Email}", request.Email);

            // Validate request
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Email and Name are required");
            }

            // Check if user already exists
            var existingUserFilter = Builders<UserDocument>.Filter.Eq(x => x.Email, request.Email);
            var existingUsers = await _provider.GetListOfAsync("users", existingUserFilter);
            
            if (existingUsers.Any())
            {
                throw new InvalidOperationException($"User with email {request.Email} already exists");
            }

            // Create user document
            var user = new UserDocument
            {
                Name = request.Name,
                Email = request.Email,
                Age = request.Age,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            // Insert user
            var insertedUser = await _provider.InsertOneAsync("users", user);
            _logger.LogInformation("Created user with ID {UserId}", insertedUser.Id);

            // Create user profile
            var profile = new ExpandoObject();
            var profileDict = (IDictionary<string, object>)profile;
            profileDict["userId"] = insertedUser.Id;
            profileDict["bio"] = request.Bio ?? "";
            profileDict["avatarUrl"] = request.AvatarUrl ?? "";
            profileDict["preferences"] = new Dictionary<string, object>
            {
                { "notifications", true },
                { "theme", "light" },
                { "language", "en" }
            };
            profileDict["createdAt"] = DateTime.UtcNow;
            profileDict["updatedAt"] = DateTime.UtcNow;

            var insertedProfile = await _provider.InsertOneExpandoAsync("profiles", profile);
            _logger.LogInformation("Created profile for user {UserId}", insertedUser.Id);

            // Return combined user profile
            return new UserProfile
            {
                User = insertedUser,
                Profile = insertedProfile
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user profile for {Email}", request.Email);
            throw;
        }
    }

    public async Task<List<UserProfile>> SearchUsersAsync(UserSearchCriteria criteria)
    {
        try
        {
            _logger.LogInformation("Searching users with criteria: {Criteria}", criteria);

            // Build search filter
            var filter = Builders<UserDocument>.Filter.Empty;

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                filter &= Builders<UserDocument>.Filter.Regex(x => x.Name, 
                    new BsonRegularExpression(criteria.Name, "i"));
            }

            if (criteria.MinAge.HasValue)
            {
                filter &= Builders<UserDocument>.Filter.Gte(x => x.Age, criteria.MinAge.Value);
            }

            if (criteria.MaxAge.HasValue)
            {
                filter &= Builders<UserDocument>.Filter.Lte(x => x.Age, criteria.MaxAge.Value);
            }

            if (criteria.IsActive.HasValue)
            {
                filter &= Builders<UserDocument>.Filter.Eq(x => x.IsActive, criteria.IsActive.Value);
            }

            // Query users
            var users = await _provider.GetListOfAsync("users", filter);
            _logger.LogInformation("Found {Count} users matching criteria", users.Count);

            // Get profiles for users
            var userProfiles = new List<UserProfile>();
            foreach (var user in users)
            {
                var profileFilter = Builders<BsonDocument>.Filter.Eq("userId", user.Id);
                var profiles = await _provider.GetExpandoListAsync("profiles", profileFilter);
                
                if (profiles.Any())
                {
                    userProfiles.Add(new UserProfile
                    {
                        User = user,
                        Profile = profiles.First()
                    });
                }
            }

            return userProfiles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to search users with criteria: {Criteria}", criteria);
            throw;
        }
    }
}

// Supporting classes
public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }
}

public class UserSearchCriteria
{
    public string Name { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    public bool? IsActive { get; set; }
}

public class UserProfile
{
    public UserDocument User { get; set; }
    public ExpandoObject Profile { get; set; }
}
```

### Example 8: Dependency Injection Setup

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDataProvider(this IServiceCollection services, IConfiguration configuration)
    {
        // Register MongoDB connection factory
        services.AddSingleton<IMongoConnectionFactory>(provider =>
        {
            var connectionString = configuration.GetConnectionString("MongoDB");
            var logger = provider.GetService<ILogger<MongoConnectionFactory>>();
            return new MongoConnectionFactory(connectionString, logger);
        });

        // Register MongoDB session manager
        services.AddSingleton<IMongoSessionManager>(provider =>
        {
            var connectionFactory = provider.GetRequiredService<IMongoConnectionFactory>();
            var logger = provider.GetService<ILogger<MongoSessionManager>>();
            return new MongoSessionManager(connectionFactory.GetClient(), logger);
        });

        // Register MongoDB health monitor
        services.AddSingleton<MongoHealthMonitor>(provider =>
        {
            var connectionFactory = provider.GetRequiredService<IMongoConnectionFactory>();
            var logger = provider.GetService<ILogger<MongoHealthMonitor>>();
            return new MongoHealthMonitor(connectionFactory.GetClient(), logger);
        });

        // Register MongoDB data provider
        services.AddScoped<IMongoDataProvider>(provider =>
        {
            var connectionFactory = provider.GetRequiredService<IMongoConnectionFactory>();
            var sessionManager = provider.GetRequiredService<IMongoSessionManager>();
            var healthMonitor = provider.GetRequiredService<MongoHealthMonitor>();
            var logger = provider.GetService<ILogger<MongoDataProvider>>();
            
            return new MongoDataProvider(connectionFactory, sessionManager, healthMonitor, logger);
        });

        // Register example service
        services.AddScoped<RealWorldIntegrationExample>();

        return services;
    }
}

// Usage in Program.cs or Startup.cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services
        builder.Services.AddLogging();
        builder.Services.AddMongoDataProvider(builder.Configuration);

        var app = builder.Build();

        // Use the service
        app.MapGet("/users", async (RealWorldIntegrationExample userService) =>
        {
            var criteria = new UserSearchCriteria { IsActive = true };
            var users = await userService.SearchUsersAsync(criteria);
            return users;
        });

        app.Run();
    }
}
```

## Running the Examples

### Prerequisites

1. MongoDB server running on localhost:27017
2. .NET 9.0 SDK
3. AMCode.Data library built and referenced

### Setup

1. Ensure MongoDB is running
2. Update connection strings in examples if needed
3. Run individual examples or create a test application

### Example Test Application

```csharp
public class ExampleRunner
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("MongoDB Integration Examples");
        Console.WriteLine("============================");

        try
        {
            // Run basic examples
            await RunBasicExamples();
            
            // Run advanced examples
            await RunAdvancedExamples();
            
            // Run performance examples
            await RunPerformanceExamples();
            
            Console.WriteLine("\nAll examples completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Example failed: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private static async Task RunBasicExamples()
    {
        Console.WriteLine("\n--- Basic Examples ---");
        
        var basicCrud = new BasicCrudExample();
        await basicCrud.RunExample();
        
        var dynamicExample = new DynamicObjectExample();
        await dynamicExample.RunExample();
    }

    private static async Task RunAdvancedExamples()
    {
        Console.WriteLine("\n--- Advanced Examples ---");
        
        var transactionExample = new TransactionExample();
        await transactionExample.RunExample();
        
        var bulkExample = new BulkOperationsExample();
        await bulkExample.RunExample();
        
        var transformExample = new DataTransformationExample();
        await transformExample.RunExample();
    }

    private static async Task RunPerformanceExamples()
    {
        Console.WriteLine("\n--- Performance Examples ---");
        
        var perfExample = new PerformanceExample();
        await perfExample.RunExample();
    }
}
```

This comprehensive set of examples demonstrates all the key features and capabilities of the AMCode.Data MongoDB integration, from basic CRUD operations to advanced performance optimization techniques.
