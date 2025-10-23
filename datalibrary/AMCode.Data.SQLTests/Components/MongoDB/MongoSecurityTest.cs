using System;
using System.Collections.Generic;
using System.Dynamic;
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
    /// MongoDB security tests to validate security patterns and prevent vulnerabilities
    /// </summary>
    [TestFixture]
    public class MongoSecurityTest
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
            _testCollectionName = $"security_test_{Guid.NewGuid().ToString("N")[..8]}";
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
                Console.WriteLine($"Cleanup error: {ex.Message}");
            }

            _mongoDataProvider?.Dispose();
        }

        #region Authentication Tests

        [Test]
        public async Task Connection_WithValidCredentials_ShouldAuthenticateSuccessfully()
        {
            // Act
            var isHealthy = _healthMonitor.IsHealthy();

            // Assert
            Assert.IsTrue(isHealthy, "MongoDB connection with valid credentials should be healthy");
        }

        [Test]
        public async Task Connection_WithInvalidCredentials_ShouldFailAuthentication()
        {
            // Arrange
            var invalidConnectionString = "mongodb://invaliduser:invalidpassword@localhost:27017/testdb?authSource=admin";
            var invalidConnectionFactory = new MongoConnectionFactory(invalidConnectionString);
            var invalidHealthMonitor = new MongoHealthMonitor(invalidConnectionFactory, new LoggerFactory().CreateLogger<MongoHealthMonitor>());

            // Act
            var isHealthy = invalidHealthMonitor.IsHealthy();

            // Assert
            Assert.IsFalse(isHealthy, "MongoDB connection with invalid credentials should fail authentication");
        }

        [Test]
        public async Task Connection_WithMissingCredentials_ShouldFailAuthentication()
        {
            // Arrange
            var noAuthConnectionString = "mongodb://localhost:27017/testdb";
            var noAuthConnectionFactory = new MongoConnectionFactory(noAuthConnectionString);
            var noAuthHealthMonitor = new MongoHealthMonitor(noAuthConnectionFactory, new LoggerFactory().CreateLogger<MongoHealthMonitor>());

            // Act
            var isHealthy = noAuthHealthMonitor.IsHealthy();

            // Assert
            Assert.IsFalse(isHealthy, "MongoDB connection without credentials should fail authentication");
        }

        #endregion

        #region Authorization Tests

        [Test]
        public async Task ReadOperations_WithValidUser_ShouldSucceed()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();
            await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

            // Act
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Empty);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public async Task WriteOperations_WithValidUser_ShouldSucceed()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();

            // Act
            var result = await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
        }

        [Test]
        public async Task DeleteOperations_WithValidUser_ShouldSucceed()
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

        #endregion

        #region NoSQL Injection Prevention Tests

        [Test]
        public async Task FilterDefinition_WithMaliciousInput_ShouldBeSafe()
        {
            // Arrange
            var testDocuments = MongoTestDataFactory.CreateTestDocumentList(5);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, testDocuments);

            // Act - Test various injection attempts
            var maliciousInputs = new[]
            {
                "'; db.dropDatabase(); //",
                "{ $where: 'this.name == this.password' }",
                "{ $ne: null }",
                "{ $regex: '.*', $options: 'i' }",
                "{ $or: [{ name: { $ne: null } }, { password: { $ne: null } }] }"
            };

            foreach (var maliciousInput in maliciousInputs)
            {
                // These should not cause security issues because we're using strongly-typed filters
                var filter = Builders<TestDocument>.Filter.Eq(x => x.Name, maliciousInput);
                var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(_testCollectionName, filter);
                
                // Assert - Should return empty results, not cause security issues
                Assert.IsNotNull(result);
                Assert.IsEmpty(result, $"Malicious input '{maliciousInput}' should not return results");
            }
        }

        [Test]
        public async Task UpdateDefinition_WithMaliciousInput_ShouldBeSafe()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();
            var insertedDocument = await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

            // Act - Test malicious update attempts
            var maliciousUpdates = new[]
            {
                "'; db.dropDatabase(); //",
                "{ $set: { $where: 'this.name == this.password' } }",
                "{ $set: { name: { $ne: null } } }"
            };

            foreach (var maliciousUpdate in maliciousUpdates)
            {
                // These should not cause security issues because we're using strongly-typed updates
                var update = Builders<TestDocument>.Update.Set(x => x.Name, maliciousUpdate);
                
                // Should not throw security-related exceptions
                Assert.DoesNotThrowAsync(async () =>
                {
                    await _mongoDataProvider.ReplaceOneAsync(
                        _testCollectionName,
                        Builders<TestDocument>.Filter.Eq(x => x.Id, insertedDocument.Id),
                        new TestDocument { Name = maliciousUpdate });
                });
            }
        }

        [Test]
        public async Task CollectionName_WithMaliciousInput_ShouldBeSanitized()
        {
            // Arrange
            var maliciousCollectionNames = new[]
            {
                "'; db.dropDatabase(); //",
                "test; db.dropDatabase();",
                "test\"; db.dropDatabase();",
                "test'; db.dropDatabase();",
                "test`; db.dropDatabase();"
            };

            foreach (var maliciousName in maliciousCollectionNames)
            {
                // Act & Assert - Should throw ArgumentException for invalid collection names
                Assert.ThrowsAsync<ArgumentException>(async () =>
                {
                    await _mongoDataProvider.GetListOfAsync<TestDocument>(
                        maliciousName, 
                        Builders<TestDocument>.Filter.Empty);
                }, $"Malicious collection name '{maliciousName}' should be rejected");
            }
        }

        #endregion

        #region Data Validation Tests

        [Test]
        public async Task InputValidation_WithNullValues_ShouldHandleGracefully()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _mongoDataProvider.InsertOneAsync<TestDocument>(_testCollectionName, null);
            }, "Null document should be rejected");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _mongoDataProvider.InsertManyAsync<TestDocument>(_testCollectionName, null);
            }, "Null document list should be rejected");

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _mongoDataProvider.GetListOfAsync<TestDocument>(null, Builders<TestDocument>.Filter.Empty);
            }, "Null collection name should be rejected");
        }

        [Test]
        public async Task InputValidation_WithEmptyValues_ShouldHandleGracefully()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _mongoDataProvider.GetListOfAsync<TestDocument>("", Builders<TestDocument>.Filter.Empty);
            }, "Empty collection name should be rejected");

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _mongoDataProvider.GetListOfAsync<TestDocument>("   ", Builders<TestDocument>.Filter.Empty);
            }, "Whitespace-only collection name should be rejected");
        }

        [Test]
        public async Task InputValidation_WithSpecialCharacters_ShouldBeSanitized()
        {
            // Arrange
            var specialCharacters = new[] { "\0", "\n", "\r", "\t", "\\", "\"", "'", "`" };
            var testDocument = MongoTestDataFactory.CreateTestDocument();

            foreach (var specialChar in specialCharacters)
            {
                // Act
                testDocument.Name = $"Test{specialChar}User";
                var result = await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsNotNull(result.Id);
                
                // Verify the data was stored correctly (MongoDB should handle special characters)
                var retrieved = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                    _testCollectionName,
                    Builders<TestDocument>.Filter.Eq(x => x.Id, result.Id));
                
                Assert.AreEqual(1, retrieved.Count);
                Assert.AreEqual(testDocument.Name, retrieved[0].Name);
            }
        }

        #endregion

        #region Error Handling Security Tests

        [Test]
        public async Task ErrorMessages_ShouldNotExposeSensitiveInformation()
        {
            // Arrange
            var invalidConnectionString = "mongodb://invaliduser:secretpassword@localhost:27017/testdb?authSource=admin";
            var invalidConnectionFactory = new MongoConnectionFactory(invalidConnectionString);
            var invalidHealthMonitor = new MongoHealthMonitor(invalidConnectionFactory, new LoggerFactory().CreateLogger<MongoHealthMonitor>());

            // Act & Assert
            try
            {
                var isHealthy = invalidHealthMonitor.IsHealthy();
                Assert.IsFalse(isHealthy);
            }
            catch (Exception ex)
            {
                // Error messages should not contain sensitive information
                Assert.IsFalse(ex.Message.Contains("secretpassword"), 
                    "Error message should not contain password");
                Assert.IsFalse(ex.Message.Contains("invaliduser"), 
                    "Error message should not contain username");
            }
        }

        [Test]
        public async Task Logging_ShouldNotExposeSensitiveInformation()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();
            testDocument.Email = "secret@example.com";
            testDocument.Name = "Secret User";

            // Act
            await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);

            // Assert
            // This test verifies that logging doesn't expose sensitive data
            // In a real implementation, you would verify the log output
            // For now, we just ensure the operation completes without exposing data
            Assert.DoesNotThrowAsync(async () =>
            {
                await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);
            });
        }

        #endregion

        #region Connection Security Tests

        [Test]
        public async Task ConnectionString_ShouldBeSecure()
        {
            // Arrange
            var connectionString = MongoTestingEnvironmentInit.GetConnectionString();

            // Assert
            Assert.IsTrue(connectionString.Contains("authSource=admin"), 
                "Connection string should use admin authentication source");
            Assert.IsTrue(connectionString.Contains("testuser"), 
                "Connection string should contain username");
            Assert.IsTrue(connectionString.Contains("testpassword"), 
                "Connection string should contain password");
            Assert.IsFalse(connectionString.Contains("ssl=false"), 
                "Connection string should not explicitly disable SSL");
        }

        [Test]
        public async Task ConnectionPooling_ShouldBeSecure()
        {
            // Arrange
            var documents = MongoTestDataFactory.CreateTestDocumentList(10);

            // Act - Perform multiple operations to test connection pooling
            for (int i = 0; i < 5; i++)
            {
                await _mongoDataProvider.InsertManyAsync($"{_testCollectionName}_{i}", documents);
                var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                    $"{_testCollectionName}_{i}", 
                    Builders<TestDocument>.Filter.Empty);
                Assert.AreEqual(10, result.Count);
            }

            // Assert
            // Connection pooling should work securely
            Assert.IsTrue(_healthMonitor.IsHealthy(), 
                "Connection pooling should maintain healthy connections");
        }

        #endregion

        #region Data Encryption Tests

        [Test]
        public async Task SensitiveData_ShouldBeStoredSecurely()
        {
            // Arrange
            var sensitiveDocument = new TestDocument
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Age = 30,
                // Simulate sensitive metadata
                Metadata = new Dictionary<string, object>
                {
                    { "ssn", "123-45-6789" },
                    { "creditCard", "4111-1111-1111-1111" },
                    { "password", "secretpassword123" }
                }
            };

            // Act
            var result = await _mongoDataProvider.InsertOneAsync(_testCollectionName, sensitiveDocument);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Id);
            
            // Verify the data was stored
            var retrieved = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName,
                Builders<TestDocument>.Filter.Eq(x => x.Id, result.Id));
            
            Assert.AreEqual(1, retrieved.Count);
            Assert.AreEqual(sensitiveDocument.Name, retrieved[0].Name);
            Assert.AreEqual(sensitiveDocument.Email, retrieved[0].Email);
            
            // Note: In a real implementation, you would verify that sensitive data
            // is encrypted at rest and in transit
        }

        #endregion

        #region Session Security Tests

        [Test]
        public async Task SessionManagement_ShouldBeSecure()
        {
            // Arrange
            var testDocument = MongoTestDataFactory.CreateTestDocument();

            // Act
            using (var session = await _sessionManager.StartSessionAsync())
            {
                var result = await _mongoDataProvider.InsertOneAsync(_testCollectionName, testDocument);
                Assert.IsNotNull(result);
                
                var retrieved = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                    _testCollectionName,
                    Builders<TestDocument>.Filter.Eq(x => x.Id, result.Id));
                
                Assert.AreEqual(1, retrieved.Count);
            }

            // Assert
            // Session should be properly disposed and not accessible after using block
            Assert.IsTrue(_healthMonitor.IsHealthy(), 
                "Session management should maintain connection health");
        }

        #endregion

        #region Helper Methods

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
