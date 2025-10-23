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
using AMCode.Data.UnitTests.Components.MongoDB.Models;

namespace AMCode.Data.UnitTests.Components.MongoDB
{
    /// <summary>
    /// Unit tests for MongoDataProvider
    /// </summary>
    [TestFixture]
    public class MongoDataProviderTest : MongoTestBase
    {
        private MongoDataProvider _mongoDataProvider;
        private Mock<IMongoGenericDataProvider> _mockGenericDataProvider;
        private Mock<IMongoExpandoObjectDataProvider> _mockExpandoObjectDataProvider;
        private Mock<IMongoExecute> _mockExecute;

        [SetUp]
        public void SetUp()
        {
            SetupMocks();
            
            // Setup sub-provider mocks
            _mockGenericDataProvider = new Mock<IMongoGenericDataProvider>();
            _mockExpandoObjectDataProvider = new Mock<IMongoExpandoObjectDataProvider>();
            _mockExecute = new Mock<IMongoExecute>();

            // Create MongoDataProvider with mocked dependencies
            _mongoDataProvider = new MongoDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoDataProvider>);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoDataProvider?.Dispose();
        }

        #region Constructor Tests

        [Test]
        public void Constructor_WithValidParameters_ShouldInitializeSuccessfully()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new MongoDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoDataProvider>));
        }

        [Test]
        public void Constructor_WithNullConnectionFactory_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoDataProvider(
                null,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoDataProvider>));
        }

        [Test]
        public void Constructor_WithNullSessionManager_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoDataProvider(
                MockConnectionFactory.Object,
                null,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoDataProvider>));
        }

        [Test]
        public void Constructor_WithNullHealthMonitor_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                null,
                MockLogger.Object as ILogger<MongoDataProvider>));
        }

        #endregion

        #region Generic Data Provider Tests

        [Test]
        public async Task GetListOfAsync_WithValidParameters_ShouldReturnDocuments()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");
            var expectedDocuments = MongoTestDataFactory.CreateTestDocumentList(3);

            // Note: In real implementation, this would be tested through the actual MongoGenericDataProvider
            // For now, we're testing the interface compliance

            // Act
            var result = await _mongoDataProvider.GetListOfAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<TestDocument>>(result);
        }

        [Test]
        public async Task GetListOfAsync_WithTransforms_ShouldReturnTransformedDocuments()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");
            var transforms = new List<AMCode.Columns.DataTransform.IDataTransformColumnDefinition>();

            // Act
            var result = await _mongoDataProvider.GetListOfAsync(collection, filter, transforms);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<TestDocument>>(result);
        }

        [Test]
        public async Task GetValueOfAsync_WithValidParameters_ShouldReturnValue()
        {
            // Arrange
            var collection = "testCollection";
            var field = "name";
            var filter = CreateMockFilter<TestDocument>("age", 25);

            // Act
            var result = await _mongoDataProvider.GetValueOfAsync<string>(collection, field, filter);

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task InsertOneAsync_WithValidDocument_ShouldReturnInsertedDocument()
        {
            // Arrange
            var collection = "testCollection";
            var document = MongoTestDataFactory.CreateTestDocument();

            // Act
            var result = await _mongoDataProvider.InsertOneAsync(collection, document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<TestDocument>(result);
        }

        [Test]
        public async Task InsertManyAsync_WithValidDocuments_ShouldReturnInsertedDocuments()
        {
            // Arrange
            var collection = "testCollection";
            var documents = MongoTestDataFactory.CreateTestDocumentList(3);

            // Act
            var result = await _mongoDataProvider.InsertManyAsync(collection, documents);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<TestDocument>>(result);
        }

        [Test]
        public async Task ReplaceOneAsync_WithValidParameters_ShouldReturnReplaceResult()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("id", "507f1f77bcf86cd799439011");
            var replacement = MongoTestDataFactory.CreateTestDocument();

            // Act
            var result = await _mongoDataProvider.ReplaceOneAsync(collection, filter, replacement);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReplaceOneResult>(result);
        }

        [Test]
        public async Task DeleteOneAsync_WithValidFilter_ShouldReturnDeleteResult()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("id", "507f1f77bcf86cd799439011");

            // Act
            var result = await _mongoDataProvider.DeleteOneAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DeleteResult>(result);
        }

        [Test]
        public async Task DeleteManyAsync_WithValidFilter_ShouldReturnDeleteResult()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("isActive", false);

            // Act
            var result = await _mongoDataProvider.DeleteManyAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DeleteResult>(result);
        }

        #endregion

        #region ExpandoObject Data Provider Tests

        [Test]
        public async Task GetExpandoListAsync_WithValidFilter_ShouldReturnExpandoObjects()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");

            // Act
            var result = await _mongoDataProvider.GetExpandoListAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<ExpandoObject>>(result);
        }

        [Test]
        public async Task GetExpandoListAsync_WithTransforms_ShouldReturnTransformedExpandoObjects()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var transforms = new List<AMCode.Columns.DataTransform.IDataTransformColumnDefinition>();

            // Act
            var result = await _mongoDataProvider.GetExpandoListAsync(collection, filter, transforms);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<ExpandoObject>>(result);
        }

        [Test]
        public async Task InsertOneExpandoAsync_WithValidDocument_ShouldReturnInsertedExpandoObject()
        {
            // Arrange
            var collection = "testCollection";
            var document = CreateMockExpandoObject();

            // Act
            var result = await _mongoDataProvider.InsertOneExpandoAsync(collection, document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ExpandoObject>(result);
        }

        [Test]
        public async Task InsertManyExpandoAsync_WithValidDocuments_ShouldReturnInsertedExpandoObjects()
        {
            // Arrange
            var collection = "testCollection";
            var documents = CreateMockExpandoObjectList(3);

            // Act
            var result = await _mongoDataProvider.InsertManyExpandoAsync(collection, documents);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<ExpandoObject>>(result);
        }

        #endregion

        #region Execute Tests

        [Test]
        public async Task ExecuteAsync_WithUpdateOperation_ShouldCompleteSuccessfully()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("name", "Test User");
            var update = CreateMockUpdate("age", 30);

            // Act & Assert
            Assert.DoesNotThrow(async () => await _mongoDataProvider.ExecuteAsync(collection, filter, update));
        }

        [Test]
        public async Task ExecuteAsync_WithDeleteOperation_ShouldCompleteSuccessfully()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<BsonDocument>("isActive", false);

            // Act & Assert
            Assert.DoesNotThrow(async () => await _mongoDataProvider.ExecuteAsync(collection, filter));
        }

        [Test]
        public async Task ExecuteBulkWriteAsync_WithValidRequests_ShouldReturnBulkWriteResult()
        {
            // Arrange
            var collection = "testCollection";
            var requests = new List<WriteModel<BsonDocument>>
            {
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument()),
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument()),
                new InsertOneModel<BsonDocument>(CreateMockBsonDocument())
            };

            // Act
            var result = await _mongoDataProvider.ExecuteBulkWriteAsync(collection, requests);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BulkWriteResult<BsonDocument>>(result);
        }

        #endregion

        #region Error Handling Tests

        [Test]
        public async Task GetListOfAsync_WithNullCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var filter = CreateMockFilter<TestDocument>("name", "Test User");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoDataProvider.GetListOfAsync(null, filter));
        }

        [Test]
        public async Task GetListOfAsync_WithEmptyCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var collection = "";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoDataProvider.GetListOfAsync(collection, filter));
        }

        [Test]
        public async Task InsertOneAsync_WithNullDocument_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoDataProvider.InsertOneAsync<TestDocument>(collection, null));
        }

        [Test]
        public async Task InsertManyAsync_WithNullDocuments_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoDataProvider.InsertManyAsync<TestDocument>(collection, null));
        }

        [Test]
        public async Task ReplaceOneAsync_WithNullReplacement_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("id", "507f1f77bcf86cd799439011");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoDataProvider.ReplaceOneAsync(collection, filter, null));
        }

        #endregion

        #region Cancellation Token Tests

        [Test]
        public async Task GetListOfAsync_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<OperationCanceledException>(async () => 
                await _mongoDataProvider.GetListOfAsync(collection, filter, cancellationTokenSource.Token));
        }

        [Test]
        public async Task InsertOneAsync_WithCancellationToken_ShouldRespectCancellation()
        {
            // Arrange
            var collection = "testCollection";
            var document = MongoTestDataFactory.CreateTestDocument();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<OperationCanceledException>(async () => 
                await _mongoDataProvider.InsertOneAsync(collection, document, cancellationTokenSource.Token));
        }

        #endregion

        #region Logging Tests

        [Test]
        public void Constructor_WithLogger_ShouldLogInitialization()
        {
            // Arrange & Act
            var provider = new MongoDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoDataProvider>);

            // Assert
            VerifyOperationLogged("MongoDB data provider initialized");
        }

        #endregion
    }
}
