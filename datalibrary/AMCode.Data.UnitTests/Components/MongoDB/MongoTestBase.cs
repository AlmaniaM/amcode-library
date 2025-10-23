using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Moq;
using Microsoft.Extensions.Logging;
using AMCode.Data.MongoDB;
using AMCode.Data.MongoDB.Components.Connection;
using AMCode.Data.MongoDB.Components.Data.Models;

namespace AMCode.Data.UnitTests.Components.MongoDB
{
    /// <summary>
    /// Base class for MongoDB unit tests providing common setup and utilities
    /// </summary>
    public abstract class MongoTestBase
    {
        protected Mock<IMongoConnectionFactory> MockConnectionFactory;
        protected Mock<IMongoSessionManager> MockSessionManager;
        protected Mock<MongoHealthMonitor> MockHealthMonitor;
        protected Mock<ILogger> MockLogger;
        protected Mock<IMongoClient> MockMongoClient;
        protected Mock<IMongoDatabase> MockMongoDatabase;
        protected Mock<IMongoCollection<BsonDocument>> MockMongoCollection;

        /// <summary>
        /// Sets up common mocks for MongoDB testing
        /// </summary>
        protected virtual void SetupMocks()
        {
            // Setup logger mock
            MockLogger = new Mock<ILogger>();
            MockLogger.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()));

            // Setup MongoDB client mock
            MockMongoClient = new Mock<IMongoClient>();
            MockMongoDatabase = new Mock<IMongoDatabase>();
            MockMongoCollection = new Mock<IMongoCollection<BsonDocument>>();

            MockMongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
                .Returns(MockMongoDatabase.Object);

            MockMongoDatabase.Setup(x => x.GetCollection<BsonDocument>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(MockMongoCollection.Object);

            // Setup connection factory mock
            MockConnectionFactory = new Mock<IMongoConnectionFactory>();
            MockConnectionFactory.Setup(x => x.GetClient()).Returns(MockMongoClient.Object);
            MockConnectionFactory.Setup(x => x.GetDatabase(It.IsAny<string>())).Returns(MockMongoDatabase.Object);
            MockConnectionFactory.Setup(x => x.GetCollection<BsonDocument>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(MockMongoCollection.Object);

            // Setup session manager mock
            MockSessionManager = new Mock<IMongoSessionManager>();
            MockSessionManager.Setup(x => x.StartSession(It.IsAny<ClientSessionOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Mock.Of<IClientSessionHandle>()));

            // Setup health monitor mock
            MockHealthMonitor = new Mock<MongoHealthMonitor>(MockConnectionFactory.Object, MockLogger.Object);
            MockHealthMonitor.Setup(x => x.IsHealthy()).Returns(true);
        }

        /// <summary>
        /// Creates a mock filter definition for testing
        /// </summary>
        protected FilterDefinition<T> CreateMockFilter<T>(string field, object value)
        {
            return Builders<T>.Filter.Eq(field, value);
        }

        /// <summary>
        /// Creates a mock update definition for testing
        /// </summary>
        protected UpdateDefinition<BsonDocument> CreateMockUpdate(string field, object value)
        {
            return Builders<BsonDocument>.Update.Set(field, value);
        }

        /// <summary>
        /// Creates a mock ExpandoObject for testing
        /// </summary>
        protected ExpandoObject CreateMockExpandoObject(string name = "Test", int age = 25, string email = "test@example.com")
        {
            dynamic expando = new ExpandoObject();
            expando.Name = name;
            expando.Age = age;
            expando.Email = email;
            expando.CreatedAt = DateTime.UtcNow;
            expando.IsActive = true;
            return expando;
        }

        /// <summary>
        /// Creates a list of mock ExpandoObjects for testing
        /// </summary>
        protected List<ExpandoObject> CreateMockExpandoObjectList(int count = 5)
        {
            var expandoObjects = new List<ExpandoObject>();
            for (int i = 0; i < count; i++)
            {
                expandoObjects.Add(CreateMockExpandoObject($"User {i}", 20 + i, $"user{i}@example.com"));
            }
            return expandoObjects;
        }

        /// <summary>
        /// Creates a mock BsonDocument for testing
        /// </summary>
        protected BsonDocument CreateMockBsonDocument(string name = "Test", int age = 25, string email = "test@example.com")
        {
            return new BsonDocument
            {
                { "_id", ObjectId.GenerateNewId() },
                { "name", name },
                { "age", age },
                { "email", email },
                { "createdAt", DateTime.UtcNow },
                { "isActive", true }
            };
        }

        /// <summary>
        /// Creates a list of mock BsonDocuments for testing
        /// </summary>
        protected List<BsonDocument> CreateMockBsonDocumentList(int count = 5)
        {
            var documents = new List<BsonDocument>();
            for (int i = 0; i < count; i++)
            {
                documents.Add(CreateMockBsonDocument($"User {i}", 20 + i, $"user{i}@example.com"));
            }
            return documents;
        }

        /// <summary>
        /// Verifies that a MongoDB operation was logged
        /// </summary>
        protected void VerifyOperationLogged(string operationName)
        {
            MockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(operationName)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        /// <summary>
        /// Verifies that an error was logged
        /// </summary>
        protected void VerifyErrorLogged(string errorMessage)
        {
            MockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(errorMessage)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        /// <summary>
        /// Creates a mock IAsyncCursor for testing
        /// </summary>
        protected IAsyncCursor<T> CreateMockAsyncCursor<T>(IEnumerable<T> documents)
        {
            var mockCursor = new Mock<IAsyncCursor<T>>();
            mockCursor.Setup(x => x.Current).Returns(documents);
            mockCursor.Setup(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true);
            mockCursor.Setup(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true));
            return mockCursor.Object;
        }

        /// <summary>
        /// Creates a mock FindFluent for testing
        /// </summary>
        protected IFindFluent<T, T> CreateMockFindFluent<T>(IAsyncCursor<T> cursor)
        {
            var mockFindFluent = new Mock<IFindFluent<T, T>>();
            mockFindFluent.Setup(x => x.ToCursorAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(cursor));
            return mockFindFluent.Object;
        }

        /// <summary>
        /// Creates a mock ReplaceOneResult for testing
        /// </summary>
        protected ReplaceOneResult CreateMockReplaceOneResult(bool isAcknowledged = true, long matchedCount = 1, long modifiedCount = 1)
        {
            return ReplaceOneResult.Acknowledged(matchedCount, modifiedCount, BsonValue.Create(ObjectId.GenerateNewId()));
        }

        /// <summary>
        /// Creates a mock DeleteResult for testing
        /// </summary>
        protected DeleteResult CreateMockDeleteResult(bool isAcknowledged = true, long deletedCount = 1)
        {
            return DeleteResult.Acknowledged(deletedCount);
        }

        /// <summary>
        /// Creates a mock BulkWriteResult for testing
        /// </summary>
        protected BulkWriteResult<BsonDocument> CreateMockBulkWriteResult(int processedCount = 5, int insertedCount = 5)
        {
            var requests = new List<WriteModel<BsonDocument>>();
            for (int i = 0; i < processedCount; i++)
            {
                requests.Add(new InsertOneModel<BsonDocument>(CreateMockBsonDocument()));
            }

            return BulkWriteResult<BsonDocument>.Acknowledged(
                processedCount,
                insertedCount,
                0, 0, 0, 0, 0,
                new List<WriteModel<BsonDocument>>(),
                new List<BulkWriteError>());
        }
    }
}
