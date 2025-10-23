using System;
using System.Collections.Generic;
using System.Dynamic;
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
    /// Unit tests for MongoExpandoObjectDataProvider
    /// </summary>
    [TestFixture]
    public class MongoExpandoObjectDataProviderTest : MongoTestBase
    {
        private MongoExpandoObjectDataProvider _mongoExpandoObjectDataProvider;
        private Mock<IMongoCollection<BsonDocument>> _mockBsonDocumentCollection;

        [SetUp]
        public void SetUp()
        {
            SetupMocks();
            
            // Setup collection mock for BsonDocument operations
            _mockBsonDocumentCollection = new Mock<IMongoCollection<BsonDocument>>();

            MockConnectionFactory.Setup(x => x.GetCollection<BsonDocument>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_mockBsonDocumentCollection.Object);

            // Create MongoExpandoObjectDataProvider
            _mongoExpandoObjectDataProvider = new MongoExpandoObjectDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExpandoObjectDataProvider>);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoExpandoObjectDataProvider?.Dispose();
        }

        #region Constructor Tests

        [Test]
        public void Constructor_WithValidParameters_ShouldInitializeSuccessfully()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new MongoExpandoObjectDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExpandoObjectDataProvider>));
        }

        [Test]
        public void Constructor_WithNullConnectionFactory_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoExpandoObjectDataProvider(
                null,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExpandoObjectDataProvider>));
        }

        [Test]
        public void Constructor_WithNullSessionManager_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoExpandoObjectDataProvider(
                MockConnectionFactory.Object,
                null,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExpandoObjectDataProvider>));
        }

        [Test]
        public void Constructor_WithNullHealthMonitor_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoExpandoObjectDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                null,
                MockLogger.Object as ILogger<MongoExpandoObjectDataProvider>));
        }

        #endregion

        #region GetExpandoListAsync Tests

        [Test]
        public async Task GetExpandoListAsync_WithValidFilter_ShouldReturnExpandoObjects()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var expectedDocuments = CreateMockBsonDocumentList(3);
            var mockCursor = CreateMockAsyncCursor(expectedDocuments);

            _mockBsonDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoExpandoObjectDataProvider.GetExpandoListAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<ExpandoObject>>(result);
            _mockBsonDocumentCollection.Verify(x => x.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetExpandoListAsync_WithTransforms_ShouldReturnTransformedExpandoObjects()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var transforms = new List<AMCode.Columns.DataTransform.IDataTransformColumnDefinition>();
            var expectedDocuments = CreateMockBsonDocumentList(2);
            var mockCursor = CreateMockAsyncCursor(expectedDocuments);

            _mockBsonDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoExpandoObjectDataProvider.GetExpandoListAsync(collection, filter, transforms);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<ExpandoObject>>(result);
        }

        [Test]
        public async Task GetExpandoListAsync_WithEmptyResult_ShouldReturnEmptyList()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "NonExistent");
            var emptyDocuments = new List<BsonDocument>();
            var mockCursor = CreateMockAsyncCursor(emptyDocuments);

            _mockBsonDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoExpandoObjectDataProvider.GetExpandoListAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetExpandoListAsync_WithComplexBsonDocument_ShouldConvertToExpandoObject()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("type", "complex");
            var complexDocument = new BsonDocument
            {
                { "_id", ObjectId.GenerateNewId() },
                { "name", "Complex Document" },
                { "metadata", new BsonDocument
                    {
                        { "version", 1 },
                        { "tags", new BsonArray { "tag1", "tag2", "tag3" } },
                        { "settings", new BsonDocument
                            {
                                { "enabled", true },
                                { "maxItems", 100 }
                            }
                        }
                    }
                },
                { "createdAt", DateTime.UtcNow },
                { "isActive", true }
            };
            var expectedDocuments = new List<BsonDocument> { complexDocument };
            var mockCursor = CreateMockAsyncCursor(expectedDocuments);

            _mockBsonDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoExpandoObjectDataProvider.GetExpandoListAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            
            var expandoObject = result[0];
            var expandoDict = (IDictionary<string, object>)expandoObject;
            
            Assert.IsTrue(expandoDict.ContainsKey("name"));
            Assert.AreEqual("Complex Document", expandoDict["name"]);
            Assert.IsTrue(expandoDict.ContainsKey("isActive"));
            Assert.AreEqual(true, expandoDict["isActive"]);
        }

        #endregion

        #region InsertOneExpandoAsync Tests

        [Test]
        public async Task InsertOneExpandoAsync_WithValidDocument_ShouldReturnInsertedExpandoObject()
        {
            // Arrange
            var collection = "testCollection";
            var document = CreateMockExpandoObject();

            _mockBsonDocumentCollection.Setup(x => x.InsertOneAsync(
                It.IsAny<BsonDocument>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoExpandoObjectDataProvider.InsertOneExpandoAsync(collection, document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ExpandoObject>(result);
            _mockBsonDocumentCollection.Verify(x => x.InsertOneAsync(
                It.IsAny<BsonDocument>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InsertOneExpandoAsync_WithComplexExpandoObject_ShouldConvertToBsonDocument()
        {
            // Arrange
            var collection = "testCollection";
            var document = CreateComplexExpandoObject();

            _mockBsonDocumentCollection.Setup(x => x.InsertOneAsync(
                It.IsAny<BsonDocument>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoExpandoObjectDataProvider.InsertOneExpandoAsync(collection, document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ExpandoObject>(result);
        }

        [Test]
        public async Task InsertOneExpandoAsync_WithEmptyExpandoObject_ShouldHandleGracefully()
        {
            // Arrange
            var collection = "testCollection";
            var document = new ExpandoObject();

            _mockBsonDocumentCollection.Setup(x => x.InsertOneAsync(
                It.IsAny<BsonDocument>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoExpandoObjectDataProvider.InsertOneExpandoAsync(collection, document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ExpandoObject>(result);
        }

        #endregion

        #region InsertManyExpandoAsync Tests

        [Test]
        public async Task InsertManyExpandoAsync_WithValidDocuments_ShouldReturnInsertedExpandoObjects()
        {
            // Arrange
            var collection = "testCollection";
            var documents = CreateMockExpandoObjectList(3);

            _mockBsonDocumentCollection.Setup(x => x.InsertManyAsync(
                It.IsAny<IEnumerable<BsonDocument>>(),
                It.IsAny<InsertManyOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoExpandoObjectDataProvider.InsertManyExpandoAsync(collection, documents);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<ExpandoObject>>(result);
            Assert.AreEqual(3, result.Count);
            _mockBsonDocumentCollection.Verify(x => x.InsertManyAsync(
                It.IsAny<IEnumerable<BsonDocument>>(),
                It.IsAny<InsertManyOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InsertManyExpandoAsync_WithEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            var collection = "testCollection";
            var documents = new List<ExpandoObject>();

            // Act
            var result = await _mongoExpandoObjectDataProvider.InsertManyExpandoAsync(collection, documents);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _mockBsonDocumentCollection.Verify(x => x.InsertManyAsync(
                It.IsAny<IEnumerable<BsonDocument>>(),
                It.IsAny<InsertManyOptions>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task InsertManyExpandoAsync_WithMixedExpandoObjects_ShouldHandleAllTypes()
        {
            // Arrange
            var collection = "testCollection";
            var documents = new List<ExpandoObject>
            {
                CreateMockExpandoObject("User 1", 25, "user1@example.com"),
                CreateComplexExpandoObject(),
                new ExpandoObject() // Empty object
            };

            _mockBsonDocumentCollection.Setup(x => x.InsertManyAsync(
                It.IsAny<IEnumerable<BsonDocument>>(),
                It.IsAny<InsertManyOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoExpandoObjectDataProvider.InsertManyExpandoAsync(collection, documents);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        #endregion

        #region Error Handling Tests

        [Test]
        public async Task GetExpandoListAsync_WithNullCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoExpandoObjectDataProvider.GetExpandoListAsync(null, filter));
        }

        [Test]
        public async Task GetExpandoListAsync_WithEmptyCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var collection = "";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoExpandoObjectDataProvider.GetExpandoListAsync(collection, filter));
        }

        [Test]
        public async Task InsertOneExpandoAsync_WithNullDocument_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoExpandoObjectDataProvider.InsertOneExpandoAsync(collection, null));
        }

        [Test]
        public async Task InsertManyExpandoAsync_WithNullDocuments_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoExpandoObjectDataProvider.InsertManyExpandoAsync(collection, null));
        }

        #endregion

        #region Cancellation Token Tests

        [Test]
        public async Task GetExpandoListAsync_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<OperationCanceledException>(async () => 
                await _mongoExpandoObjectDataProvider.GetExpandoListAsync(collection, filter, cancellationTokenSource.Token));
        }

        [Test]
        public async Task InsertOneExpandoAsync_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            var collection = "testCollection";
            var document = CreateMockExpandoObject();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<OperationCanceledException>(async () => 
                await _mongoExpandoObjectDataProvider.InsertOneExpandoAsync(collection, document, cancellationTokenSource.Token));
        }

        #endregion

        #region Logging Tests

        [Test]
        public void Constructor_WithLogger_ShouldLogInitialization()
        {
            // Arrange & Act
            var provider = new MongoExpandoObjectDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoExpandoObjectDataProvider>);

            // Assert
            VerifyOperationLogged("MongoDB ExpandoObject data provider initialized");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates a complex ExpandoObject for testing nested structures
        /// </summary>
        private ExpandoObject CreateComplexExpandoObject()
        {
            dynamic expando = new ExpandoObject();
            expando.Name = "Complex User";
            expando.Age = 30;
            expando.Email = "complex@example.com";
            expando.CreatedAt = DateTime.UtcNow;
            expando.IsActive = true;
            
            // Add nested object
            dynamic metadata = new ExpandoObject();
            metadata.Version = 1;
            metadata.Tags = new[] { "tag1", "tag2", "tag3" };
            metadata.Settings = new ExpandoObject();
            ((dynamic)metadata.Settings).Enabled = true;
            ((dynamic)metadata.Settings).MaxItems = 100;
            
            expando.Metadata = metadata;
            
            return expando;
        }

        #endregion
    }
}
