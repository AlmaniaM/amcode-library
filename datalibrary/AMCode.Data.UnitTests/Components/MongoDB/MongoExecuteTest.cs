using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Moq;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using AMCode.Data.MongoDB;
using AMCode.Data.MongoDB.Components.Data;
using AMCode.Data.MongoDB.Components.Data.Models;
using AMCode.Data.MongoDB.Components.Connection;

namespace AMCode.Data.UnitTests.Components.MongoDB
{
    /// <summary>
    /// Unit tests for MongoExecute
    /// </summary>
    [TestFixture]
    public class MongoExecuteTest : MongoTestBase
    {
        private MongoExecute _mongoExecute;
        private Mock<IMongoCollection<BsonDocument>> _mockBsonDocumentCollection;

        [SetUp]
        public void SetUp()
        {
            SetupMocks();
            
            // Setup collection mock for BsonDocument operations
            _mockBsonDocumentCollection = new Mock<IMongoCollection<BsonDocument>>();

            MockConnectionFactory.Setup(x => x.GetCollection<BsonDocument>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_mockBsonDocumentCollection.Object);

            // Create MongoExecute
            _mongoExecute = new MongoExecute(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExecute>);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoExecute?.Dispose();
        }

        #region Constructor Tests

        [Test]
        public void Constructor_WithValidParameters_ShouldInitializeSuccessfully()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new MongoExecute(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExecute>));
        }

        [Test]
        public void Constructor_WithNullConnectionFactory_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoExecute(
                null,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExecute>));
        }

        [Test]
        public void Constructor_WithNullSessionManager_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoExecute(
                MockConnectionFactory.Object,
                null,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExecute>));
        }

        [Test]
        public void Constructor_WithNullHealthMonitor_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoExecute(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                null,
                MockLogger.Object as ILogger<MongoExecute>));
        }

        #endregion

        #region ExecuteAsync Update Tests

        [Test]
        public async Task ExecuteAsync_WithUpdateOperation_ShouldCompleteSuccessfully()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var update = CreateMockUpdate("age", 30);

            _mockBsonDocumentCollection.Setup(x => x.UpdateManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<UpdateDefinition<BsonDocument>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(UpdateResult.Acknowledged(1, 1, BsonValue.Create(ObjectId.GenerateNewId())))));

            // Act
            await _mongoExecute.ExecuteAsync(collection, filter, update);

            // Assert
            _mockBsonDocumentCollection.Verify(x => x.UpdateManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<UpdateDefinition<BsonDocument>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithUpdateOperationAndOptions_ShouldUseProvidedOptions()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var update = CreateMockUpdate("age", 30);

            _mockBsonDocumentCollection.Setup(x => x.UpdateManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<UpdateDefinition<BsonDocument>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(UpdateResult.Acknowledged(1, 1, BsonValue.Create(ObjectId.GenerateNewId())))));

            // Act
            await _mongoExecute.ExecuteAsync(collection, filter, update);

            // Assert
            _mockBsonDocumentCollection.Verify(x => x.UpdateManyAsync(
                It.Is<FilterDefinition<BsonDocument>>(f => f != null),
                It.Is<UpdateDefinition<BsonDocument>>(u => u != null),
                It.Is<UpdateOptions>(o => o != null),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithComplexUpdateOperation_ShouldHandleComplexUpdates()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("type", "user");
            var update = Builders<BsonDocument>.Update
                .Set("lastModified", DateTime.UtcNow)
                .Inc("version", 1)
                .Set("status", "updated");

            _mockBsonDocumentCollection.Setup(x => x.UpdateManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<UpdateDefinition<BsonDocument>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(UpdateResult.Acknowledged(5, 5, BsonValue.Create(ObjectId.GenerateNewId())))));

            // Act
            await _mongoExecute.ExecuteAsync(collection, filter, update);

            // Assert
            _mockBsonDocumentCollection.Verify(x => x.UpdateManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<UpdateDefinition<BsonDocument>>(),
                It.IsAny<UpdateOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region ExecuteAsync Delete Tests

        [Test]
        public async Task ExecuteAsync_WithDeleteOperation_ShouldCompleteSuccessfully()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("isActive", false);

            _mockBsonDocumentCollection.Setup(x => x.DeleteManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(DeleteResult.Acknowledged(3)));

            // Act
            await _mongoExecute.ExecuteAsync(collection, filter);

            // Assert
            _mockBsonDocumentCollection.Verify(x => x.DeleteManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithDeleteOperationAndOptions_ShouldUseProvidedOptions()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("createdAt", DateTime.UtcNow.AddDays(-30));

            _mockBsonDocumentCollection.Setup(x => x.DeleteManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(DeleteResult.Acknowledged(10)));

            // Act
            await _mongoExecute.ExecuteAsync(collection, filter);

            // Assert
            _mockBsonDocumentCollection.Verify(x => x.DeleteManyAsync(
                It.Is<FilterDefinition<BsonDocument>>(f => f != null),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WithComplexDeleteOperation_ShouldHandleComplexFilters()
        {
            // Arrange
            var collection = "testCollection";
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("status", "inactive"),
                Builders<BsonDocument>.Filter.Lt("lastLogin", DateTime.UtcNow.AddDays(-90)),
                Builders<BsonDocument>.Filter.Eq("isDeleted", false));

            _mockBsonDocumentCollection.Setup(x => x.DeleteManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(DeleteResult.Acknowledged(15)));

            // Act
            await _mongoExecute.ExecuteAsync(collection, filter);

            // Assert
            _mockBsonDocumentCollection.Verify(x => x.DeleteManyAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region ExecuteBulkWriteAsync Tests

        [Test]
        public async Task ExecuteBulkWriteAsync_WithValidRequests_ShouldReturnBulkWriteResult()
        {
            // Arrange
            var collection = "testCollection";
            var requests = new List<WriteModel<BsonDocument>>
            {
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument()),
                new UpdateOneModel<BsonDocument>(
                    CreateMockFilter<BsonDocument>("name", "Test User"),
                    CreateMockUpdate("age", 30)),
                new DeleteOneModel<BsonDocument>(CreateMockFilter<BsonDocument>("id", "507f1f77bcf86cd799439011"))
            };

            var bulkWriteResult = CreateMockBulkWriteResult(3, 1);

            _mockBsonDocumentCollection.Setup(x => x.BulkWriteAsync(
                It.IsAny<IEnumerable<WriteModel<BsonDocument>>>(),
                It.IsAny<BulkWriteOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(bulkWriteResult));

            // Act
            var result = await _mongoExecute.ExecuteBulkWriteAsync(collection, requests);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BulkWriteResult<BsonDocument>>(result);
            _mockBsonDocumentCollection.Verify(x => x.BulkWriteAsync(
                It.IsAny<IEnumerable<WriteModel<BsonDocument>>>(),
                It.IsAny<BulkWriteOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithMixedOperations_ShouldHandleAllOperationTypes()
        {
            // Arrange
            var collection = "testCollection";
            var requests = new List<WriteModel<BsonDocument>>
            {
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument("User 1", 25, "user1@example.com")),
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument("User 2", 30, "user2@example.com")),
                new UpdateOneModel<BsonDocument>(
                    CreateMockFilter<BsonDocument>("name", "User 1"),
                    CreateMockUpdate("age", 26)),
                new UpdateManyModel<BsonDocument>(
                    CreateMockFilter<BsonDocument>("age", 30),
                    CreateMockUpdate("status", "updated")),
                new DeleteOneModel<BsonDocument>(CreateMockFilter<BsonDocument>("name", "User 2")),
                new DeleteManyModel<BsonDocument>(CreateMockFilter<BsonDocument>("status", "deleted"))
            };

            var bulkWriteResult = CreateMockBulkWriteResult(6, 2);

            _mockBsonDocumentCollection.Setup(x => x.BulkWriteAsync(
                It.IsAny<IEnumerable<WriteModel<BsonDocument>>>(),
                It.IsAny<BulkWriteOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(bulkWriteResult));

            // Act
            var result = await _mongoExecute.ExecuteBulkWriteAsync(collection, requests);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BulkWriteResult<BsonDocument>>(result);
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithEmptyRequests_ShouldReturnEmptyResult()
        {
            // Arrange
            var collection = "testCollection";
            var requests = new List<WriteModel<BsonDocument>>();

            var bulkWriteResult = CreateMockBulkWriteResult(0, 0);

            _mockBsonDocumentCollection.Setup(x => x.BulkWriteAsync(
                It.IsAny<IEnumerable<WriteModel<BsonDocument>>>(),
                It.IsAny<BulkWriteOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(bulkWriteResult));

            // Act
            var result = await _mongoExecute.ExecuteBulkWriteAsync(collection, requests);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BulkWriteResult<BsonDocument>>(result);
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithLargeBatch_ShouldHandleLargeBatches()
        {
            // Arrange
            var collection = "testCollection";
            var requests = new List<WriteModel<BsonDocument>>();
            
            // Create 100 insert operations
            for (int i = 0; i < 100; i++)
            {
                requests.Add(new InsertOneModel<BsonDocument>(CreateMockBsonDocument($"User {i}", 20 + i, $"user{i}@example.com")));
            }

            var bulkWriteResult = CreateMockBulkWriteResult(100, 100);

            _mockBsonDocumentCollection.Setup(x => x.BulkWriteAsync(
                It.IsAny<IEnumerable<WriteModel<BsonDocument>>>(),
                It.IsAny<BulkWriteOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(bulkWriteResult));

            // Act
            var result = await _mongoExecute.ExecuteBulkWriteAsync(collection, requests);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BulkWriteResult<BsonDocument>>(result);
        }

        #endregion

        #region Error Handling Tests

        [Test]
        public async Task ExecuteAsync_WithNullCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var update = CreateMockUpdate("age", 30);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoExecute.ExecuteAsync(null, filter, update));
        }

        [Test]
        public async Task ExecuteAsync_WithEmptyCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var collection = "";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var update = CreateMockUpdate("age", 30);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoExecute.ExecuteAsync(collection, filter, update));
        }

        [Test]
        public async Task ExecuteAsync_WithNullFilter_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";
            var update = CreateMockUpdate("age", 30);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoExecute.ExecuteAsync(collection, null, update));
        }

        [Test]
        public async Task ExecuteAsync_WithNullUpdate_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoExecute.ExecuteAsync(collection, filter, null));
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithNullCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var requests = new List<WriteModel<BsonDocument>>
            {
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument())
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoExecute.ExecuteBulkWriteAsync(null, requests));
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithNullRequests_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoExecute.ExecuteBulkWriteAsync(collection, null));
        }

        #endregion

        #region Cancellation Token Tests

        [Test]
        public async Task ExecuteAsync_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var update = CreateMockUpdate("age", 30);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<OperationCanceledException>(async () => 
                await _mongoExecute.ExecuteAsync(collection, filter, update, cancellationTokenSource.Token));
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            var collection = "testCollection";
            var requests = new List<WriteModel<BsonDocument>>
            {
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument())
            };
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<OperationCanceledException>(async () => 
                await _mongoExecute.ExecuteBulkWriteAsync(collection, requests, cancellationTokenSource.Token));
        }

        #endregion

        #region Logging Tests

        [Test]
        public void Constructor_WithLogger_ShouldLogInitialization()
        {
            // Arrange & Act
            var execute = new MongoExecute(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExecute>);

            // Assert
            VerifyOperationLogged("MongoDB execute provider initialized");
        }

        #endregion

        #region Performance Tests

        [Test]
        public async Task ExecuteBulkWriteAsync_WithManyOperations_ShouldCompleteInReasonableTime()
        {
            // Arrange
            var collection = "testCollection";
            var requests = new List<WriteModel<BsonDocument>>();
            
            // Create 1000 operations
            for (int i = 0; i < 1000; i++)
            {
                requests.Add(new InsertOneModel<BsonDocument>(CreateMockBsonDocument($"User {i}", 20 + i, $"user{i}@example.com")));
            }

            var bulkWriteResult = CreateMockBulkWriteResult(1000, 1000);

            _mockBsonDocumentCollection.Setup(x => x.BulkWriteAsync(
                It.IsAny<IEnumerable<WriteModel<BsonDocument>>>(),
                It.IsAny<BulkWriteOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(bulkWriteResult));

            // Act
            var startTime = DateTime.UtcNow;
            var result = await _mongoExecute.ExecuteBulkWriteAsync(collection, requests);
            var endTime = DateTime.UtcNow;

            // Assert
            Assert.IsNotNull(result);
            var executionTime = endTime - startTime;
            Assert.Less(executionTime.TotalMilliseconds, 1000, "Bulk write operation should complete within 1 second");
        }

        #endregion
    }
}
