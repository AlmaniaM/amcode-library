using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using AMCode.Data.MongoDB;
using AMCode.Data.MongoDB.Components.Connection;
using AMCode.Data.UnitTests.Components.MongoDB.Models;

namespace AMCode.Data.SQLTests.Components.MongoDB
{
    /// <summary>
    /// MongoDB integration tests using real MongoDB instances
    /// </summary>
    [TestFixture]
    public class MongoIntegrationTest
    {
        private MongoDataProvider _mongoDataProvider;
        private MongoConnectionFactory _connectionFactory;
        private MongoSessionManager _sessionManager;
        private MongoHealthMonitor _healthMonitor;
        private ILogger<MongoDataProvider> _logger;
        private string _testCollectionName;

        [SetUp]
        public void SetUp()
        {
            // Setup MongoDB connection
            var connectionString = MongoTestingEnvironmentInit.GetConnectionString();
            var databaseName = MongoTestingEnvironmentInit.GetDatabaseName();
            
            _connectionFactory = new MongoConnectionFactory(connectionString);
            _sessionManager = new MongoSessionManager(_connectionFactory);
            _healthMonitor = new MongoHealthMonitor(_connectionFactory, new LoggerFactory().CreateLogger<MongoHealthMonitor>());
            _logger = new LoggerFactory().CreateLogger<MongoDataProvider>();

            _mongoDataProvider = new MongoDataProvider(
                _connectionFactory,
                _sessionManager,
                _healthMonitor,
                _logger);

            // Generate unique collection name for each test
            _testCollectionName = $"test_collection_{Guid.NewGuid().ToString("N")[..8]}";
        }

        [TearDown]
        public async Task TearDown()
        {
            // Clean up test data
            try
            {
                var database = _connectionFactory.GetDatabase(MongoTestingEnvironmentInit.GetDatabaseName());
                await database.DropCollectionAsync(_testCollectionName);
            }
            catch (Exception ex)
            {
                // Log cleanup errors but don't fail the test
                Console.WriteLine($"Cleanup error: {ex.Message}");
            }

            _mongoDataProvider?.Dispose();
        }

        #region Connection Tests

        [Test]
        public async Task Connection_ShouldConnectSuccessfully()
        {
            // Act
            var isHealthy = _healthMonitor.IsHealthy();

            // Assert
            Assert.IsTrue(isHealthy, "MongoDB connection should be healthy");
        }

        [Test]
        public async Task Connection_ShouldHandleMultipleOperations()
        {
            // Arrange
            var documents = MongoTestDataFactory.CreateTestDocumentList(5);

            // Act
            var insertedDocuments = await _mongoDataProvider.InsertManyAsync(_testCollectionName, documents);
            var retrievedDocuments = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Empty);

            // Assert
            Assert.IsNotNull(insertedDocuments);
            Assert.IsNotNull(retrievedDocuments);
            Assert.AreEqual(5, insertedDocuments.Count);
            Assert.AreEqual(5, retrievedDocuments.Count);
        }

        #endregion

        #region Generic Data Provider Integration Tests

        [Test]
        public async Task GetListOfAsync_WithRealMongoDB_ShouldReturnDocuments()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(3);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Act
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Empty);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            
            foreach (var doc in result)
            {
                Assert.IsNotNull(doc.Id);
                Assert.IsNotNull(doc.Name);
                Assert.IsNotNull(doc.Email);
                Assert.IsTrue(doc.Age > 0);
            }
        }

        [Test]
        public async Task GetListOfAsync_WithFilter_ShouldReturnFilteredDocuments()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(5);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Act
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Gte(x => x.Age, 23));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count >= 2); // At least 2 documents should have age >= 23
        }

        [Test]
        public async Task InsertOneAsync_WithRealMongoDB_ShouldInsertDocument()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();

            // Act
            var result = await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual(testDocument.Name, result.Name);
            Assert.AreEqual(testDocument.Age, result.Age);
            Assert.AreEqual(testDocument.Email, result.Email);
        }

        [Test]
        public async Task InsertManyAsync_WithRealMongoDB_ShouldInsertMultipleDocuments()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(10);

            // Act
            var result = await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Count);
            
            // Verify all documents were inserted with IDs
            foreach (var doc in result)
            {
                Assert.IsNotNull(doc.Id);
            }
        }

        [Test]
        public async Task ReplaceOneAsync_WithRealMongoDB_ShouldReplaceDocument()
        {
            // Arrange
            var originalDocument = MongoTestDataFactory.CreateTestDocument();
            var insertedDocument = await _mongoDataProvider.InsertOneAsync(_testCollectionName, originalDocument);
            
            var updatedDocument = MongoTestDataFactory.CreateTestDocument();
            updatedDocument.Id = insertedDocument.Id;
            updatedDocument.Name = "Updated Name";
            updatedDocument.Age = 99;

            // Act
            var result = await _mongoDataProvider.ReplaceOneAsync(
                _testCollectionName,
                Builders<TestDocument>.Filter.Eq(x => x.Id, insertedDocument.Id),
                updatedDocument);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsAcknowledged);
            Assert.AreEqual(1, result.MatchedCount);
            Assert.AreEqual(1, result.ModifiedCount);
        }

        [Test]
        public async Task DeleteOneAsync_WithRealMongoDB_ShouldDeleteDocument()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();
            var insertedDocument = await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

            // Act
            var result = await _mongoDataProvider.DeleteOneAsync(
                _testCollectionName,
                Builders<TestDocument>.Filter.Eq(x => x.Id, insertedDocument.Id));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsAcknowledged);
            Assert.AreEqual(1, result.DeletedCount);
        }

        [Test]
        public async Task DeleteManyAsync_WithRealMongoDB_ShouldDeleteMultipleDocuments()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(5);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Act
            var result = await _mongoDataProvider.DeleteManyAsync(
                _testCollectionName,
                Builders<TestDocument>.Filter.Gte(x => x.Age, 23));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsAcknowledged);
            Assert.IsTrue(result.DeletedCount >= 2);
        }

        #endregion

        #region ExpandoObject Integration Tests

        [Test]
        public async Task GetExpandoListAsync_WithRealMongoDB_ShouldReturnExpandoObjects()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(3);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Act
            var result = await _mongoDataProvider.GetExpandoListAsync(
                _testCollectionName,
                Builders<BsonDocument>.Filter.Empty);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            
            foreach (var expando in result)
            {
                var dict = (IDictionary<string, object>)expando;
                Assert.IsTrue(dict.ContainsKey("name"));
                Assert.IsTrue(dict.ContainsKey("age"));
                Assert.IsTrue(dict.ContainsKey("email"));
            }
        }

        [Test]
        public async Task InsertOneExpandoAsync_WithRealMongoDB_ShouldInsertExpandoObject()
        {
            // Arrange
            var expandoObject = CreateTestExpandoObject();

            // Act
            var result = await _mongoDataProvider.InsertOneExpandoAsync(_testCollectionName, expandoObject);

            // Assert
            Assert.IsNotNull(result);
            var dict = (IDictionary<string, object>)result;
            Assert.IsTrue(dict.ContainsKey("name"));
            Assert.AreEqual("Test User", dict["name"]);
        }

        [Test]
        public async Task InsertManyExpandoAsync_WithRealMongoDB_ShouldInsertMultipleExpandoObjects()
        {
            // Arrange
            var expandoObjects = new List<ExpandoObject>
            {
                CreateTestExpandoObject("User 1", 25, "user1@example.com"),
                CreateTestExpandoObject("User 2", 30, "user2@example.com"),
                CreateTestExpandoObject("User 3", 35, "user3@example.com")
            };

            // Act
            var result = await _mongoDataProvider.InsertManyExpandoAsync(_testCollectionName, expandoObjects);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        #endregion

        #region Execute Integration Tests

        [Test]
        public async Task ExecuteAsync_WithUpdateOperation_ShouldUpdateDocuments()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(3);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Act
            await _mongoDataProvider.ExecuteAsync(
                _testCollectionName,
                Builders<BsonDocument>.Filter.Gte("age", 23),
                Builders<BsonDocument>.Update.Set("status", "updated"));

            // Assert
            var updatedDocuments = await _mongoDataProvider.GetExpandoListAsync(
                _testCollectionName,
                Builders<BsonDocument>.Filter.Eq("status", "updated"));

            Assert.IsNotNull(updatedDocuments);
            Assert.IsTrue(updatedDocuments.Count >= 2);
        }

        [Test]
        public async Task ExecuteAsync_WithDeleteOperation_ShouldDeleteDocuments()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(5);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Act
            await _mongoDataProvider.ExecuteAsync(
                _testCollectionName,
                Builders<BsonDocument>.Filter.Gte("age", 25));

            // Assert
            var remainingDocuments = await _mongoDataProvider.GetExpandoListAsync(
                _testCollectionName,
                Builders<BsonDocument>.Filter.Empty);

            Assert.IsNotNull(remainingDocuments);
            Assert.IsTrue(remainingDocuments.Count < 5);
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithRealMongoDB_ShouldExecuteBulkOperations()
        {
            // Arrange
            var requests = new List<WriteModel<BsonDocument>>
            {
                new InsertOneModel<BsonDocument>(CreateTestBsonDocument("User 1", 25, "user1@example.com")),
                new InsertOneModel<BsonDocument>(CreateTestBsonDocument("User 2", 30, "user2@example.com")),
                new InsertOneModel<BsonDocument>(CreateTestBsonDocument("User 3", 35, "user3@example.com")),
                new UpdateOneModel<BsonDocument>(
                    Builders<BsonDocument>.Filter.Eq("name", "User 1"),
                    Builders<BsonDocument>.Update.Set("status", "updated")),
                new DeleteOneModel<BsonDocument>(
                    Builders<BsonDocument>.Filter.Eq("name", "User 3"))
            };

            // Act
            var result = await _mongoDataProvider.ExecuteBulkWriteAsync(_testCollectionName, requests);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsAcknowledged);
            Assert.AreEqual(5, result.ProcessedRequests.Count);
        }

        #endregion

        #region Complex Document Tests

        [Test]
        public async Task ComplexDocument_WithNestedObjects_ShouldHandleCorrectly()
        {
            // Arrange
            var complexDocument = MongoTestDataFactory.CreateComplexTestDocument();

            // Act
            var result = await _mongoDataProvider.InsertOneAsync(_testCollectionName, complexDocument);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual(complexDocument.Title, result.Title);
            Assert.IsNotNull(result.Author);
            Assert.AreEqual(complexDocument.Author.Name, result.Author.Name);
            Assert.IsNotNull(result.Comments);
            Assert.AreEqual(2, result.Comments.Count);
        }

        [Test]
        public async Task ComplexDocument_WithArrayFields_ShouldHandleCorrectly()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();
            testDocument.Tags = new List<string> { "tag1", "tag2", "tag3" };
            testDocument.Metadata = new Dictionary<string, object>
            {
                { "priority", "high" },
                { "category", "test" },
                { "settings", new Dictionary<string, object>
                    {
                        { "enabled", true },
                        { "maxItems", 100 }
                    }
                }
            };

            // Act
            var result = await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Tags);
            Assert.AreEqual(3, result.Tags.Count);
            Assert.IsNotNull(result.Metadata);
            Assert.IsTrue(result.Metadata.ContainsKey("priority"));
        }

        #endregion

        #region Error Handling Tests

        [Test]
        public async Task InvalidCollection_ShouldThrowException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _mongoDataProvider.GetListOfAsync<TestDocument>(null, Builders<TestDocument>.Filter.Empty));
        }

        [Test]
        public async Task InvalidDocument_ShouldThrowException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _mongoDataProvider.InsertOneAsync<TestDocument>(_testCollectionName, null));
        }

        #endregion

        #region Helper Methods

        private ExpandoObject CreateTestExpandoObject(string name = "Test User", int age = 25, string email = "test@example.com")
        {
            dynamic expando = new ExpandoObject();
            expando.name = name;
            expando.age = age;
            expando.email = email;
            expando.createdAt = DateTime.UtcNow;
            expando.isActive = true;
            return expando;
        }

        private BsonDocument CreateTestBsonDocument(string name = "Test User", int age = 25, string email = "test@example.com")
        {
            return new BsonDocument
            {
                { "name", name },
                { "age", age },
                { "email", email },
                { "createdAt", DateTime.UtcNow },
                { "isActive", true }
            };
        }

        #endregion
    }
}
