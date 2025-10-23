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
using AMCode.Data.UnitTests.Components.MongoDB.Models;

namespace AMCode.Data.UnitTests.Components.MongoDB
{
    /// <summary>
    /// Unit tests for MongoGenericDataProvider
    /// </summary>
    [TestFixture]
    public class MongoGenericDataProviderTest : MongoTestBase
    {
        private MongoGenericDataProvider _mongoGenericDataProvider;
        private Mock<IMongoCollection<TestDocument>> _mockTestDocumentCollection;
        private Mock<IMongoCollection<ComplexTestDocument>> _mockComplexDocumentCollection;

        [SetUp]
        public void SetUp()
        {
            SetupMocks();
            
            // Setup collection mocks for different document types
            _mockTestDocumentCollection = new Mock<IMongoCollection<TestDocument>>();
            _mockComplexDocumentCollection = new Mock<IMongoCollection<ComplexTestDocument>>();

            MockConnectionFactory.Setup(x => x.GetCollection<TestDocument>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_mockTestDocumentCollection.Object);
            MockConnectionFactory.Setup(x => x.GetCollection<ComplexTestDocument>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_mockComplexDocumentCollection.Object);

            // Create MongoGenericDataProvider
            _mongoGenericDataProvider = new MongoGenericDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoGenericDataProvider>);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoGenericDataProvider?.Dispose();
        }

        #region Constructor Tests

        [Test]
        public void Constructor_WithValidParameters_ShouldInitializeSuccessfully()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => new MongoGenericDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoGenericDataProvider>));
        }

        [Test]
        public void Constructor_WithNullConnectionFactory_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoGenericDataProvider(
                null,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoGenericDataProvider>));
        }

        [Test]
        public void Constructor_WithNullSessionManager_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoGenericDataProvider(
                MockConnectionFactory.Object,
                null,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoGenericDataProvider>));
        }

        [Test]
        public void Constructor_WithNullHealthMonitor_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new MongoGenericDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                null,
                MockLogger.Object as ILogger<MongoGenericDataProvider>));
        }

        #endregion

        #region GetListOfAsync Tests

        [Test]
        public async Task GetListOfAsync_WithValidParameters_ShouldReturnDocuments()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");
            var expectedDocuments = MongoTestDataFactory.CreateTestDocumentList(3);
            var mockCursor = CreateMockAsyncCursor(expectedDocuments);
            var mockFindFluent = CreateMockFindFluent(mockCursor);

            _mockTestDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<FindOptions<TestDocument, TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoGenericDataProvider.GetListOfAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<TestDocument>>(result);
            _mockTestDocumentCollection.Verify(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<FindOptions<TestDocument, TestDocument>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetListOfAsync_WithTransforms_ShouldReturnTransformedDocuments()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");
            var transforms = new List<AMCode.Columns.DataTransform.IDataTransformColumnDefinition>();
            var expectedDocuments = MongoTestDataFactory.CreateTestDocumentList(2);
            var mockCursor = CreateMockAsyncCursor(expectedDocuments);

            _mockTestDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<FindOptions<TestDocument, TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoGenericDataProvider.GetListOfAsync(collection, filter, transforms);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<TestDocument>>(result);
        }

        [Test]
        public async Task GetListOfAsync_WithComplexDocument_ShouldReturnComplexDocuments()
        {
            // Arrange
            var collection = "complexCollection";
            var filter = CreateMockFilter<ComplexTestDocument>("title", "Test Article");
            var expectedDocuments = MongoTestDataFactory.CreateComplexTestDocumentList(2);
            var mockCursor = CreateMockAsyncCursor(expectedDocuments);

            _mockComplexDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<ComplexTestDocument>>(),
                It.IsAny<FindOptions<ComplexTestDocument, ComplexTestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoGenericDataProvider.GetListOfAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<ComplexTestDocument>>(result);
        }

        [Test]
        public async Task GetListOfAsync_WithEmptyResult_ShouldReturnEmptyList()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("name", "NonExistent");
            var emptyDocuments = new List<TestDocument>();
            var mockCursor = CreateMockAsyncCursor(emptyDocuments);

            _mockTestDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<FindOptions<TestDocument, TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockCursor));

            // Act
            var result = await _mongoGenericDataProvider.GetListOfAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        #endregion

        #region GetValueOfAsync Tests

        [Test]
        public async Task GetValueOfAsync_WithValidParameters_ShouldReturnValue()
        {
            // Arrange
            var collection = "testCollection";
            var field = "name";
            var filter = CreateMockFilter<TestDocument>("age", 25);
            var expectedValue = "Test User";

            _mockTestDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<FindOptions<TestDocument, TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(CreateMockAsyncCursor(new List<TestDocument> { new TestDocument { Name = expectedValue } })));

            // Act
            var result = await _mongoGenericDataProvider.GetValueOfAsync<string>(collection, field, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public async Task GetValueOfAsync_WithNumericField_ShouldReturnNumericValue()
        {
            // Arrange
            var collection = "testCollection";
            var field = "age";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");
            var expectedValue = 25;

            _mockTestDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<FindOptions<TestDocument, TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(CreateMockAsyncCursor(new List<TestDocument> { new TestDocument { Age = expectedValue } })));

            // Act
            var result = await _mongoGenericDataProvider.GetValueOfAsync<int>(collection, field, filter);

            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        public async Task GetValueOfAsync_WithNoMatchingDocument_ShouldReturnDefaultValue()
        {
            // Arrange
            var collection = "testCollection";
            var field = "name";
            var filter = CreateMockFilter<TestDocument>("age", 999);
            var emptyDocuments = new List<TestDocument>();

            _mockTestDocumentCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<FindOptions<TestDocument, TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(CreateMockAsyncCursor(emptyDocuments)));

            // Act
            var result = await _mongoGenericDataProvider.GetValueOfAsync<string>(collection, field, filter);

            // Assert
            Assert.IsNull(result);
        }

        #endregion

        #region InsertOneAsync Tests

        [Test]
        public async Task InsertOneAsync_WithValidDocument_ShouldReturnInsertedDocument()
        {
            // Arrange
            var collection = "testCollection";
            var document = MongoTestDataFactory.CreateTestDocument();
            var insertedDocument = MongoTestDataFactory.CreateTestDocument();
            insertedDocument.Id = "507f1f77bcf86cd799439011";

            _mockTestDocumentCollection.Setup(x => x.InsertOneAsync(
                It.IsAny<TestDocument>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoGenericDataProvider.InsertOneAsync(collection, document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<TestDocument>(result);
            _mockTestDocumentCollection.Verify(x => x.InsertOneAsync(
                It.IsAny<TestDocument>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InsertOneAsync_WithComplexDocument_ShouldReturnInsertedDocument()
        {
            // Arrange
            var collection = "complexCollection";
            var document = MongoTestDataFactory.CreateComplexTestDocument();

            _mockComplexDocumentCollection.Setup(x => x.InsertOneAsync(
                It.IsAny<ComplexTestDocument>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoGenericDataProvider.InsertOneAsync(collection, document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ComplexTestDocument>(result);
        }

        #endregion

        #region InsertManyAsync Tests

        [Test]
        public async Task InsertManyAsync_WithValidDocuments_ShouldReturnInsertedDocuments()
        {
            // Arrange
            var collection = "testCollection";
            var documents = MongoTestDataFactory.CreateTestDocumentList(3);

            _mockTestDocumentCollection.Setup(x => x.InsertManyAsync(
                It.IsAny<IEnumerable<TestDocument>>(),
                It.IsAny<InsertManyOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _mongoGenericDataProvider.InsertManyAsync(collection, documents);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IList<TestDocument>>(result);
            Assert.AreEqual(3, result.Count);
            _mockTestDocumentCollection.Verify(x => x.InsertManyAsync(
                It.IsAny<IEnumerable<TestDocument>>(),
                It.IsAny<InsertManyOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task InsertManyAsync_WithEmptyList_ShouldReturnEmptyList()
        {
            // Arrange
            var collection = "testCollection";
            var documents = new List<TestDocument>();

            // Act
            var result = await _mongoGenericDataProvider.InsertManyAsync(collection, documents);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
            _mockTestDocumentCollection.Verify(x => x.InsertManyAsync(
                It.IsAny<IEnumerable<TestDocument>>(),
                It.IsAny<InsertManyOptions>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region ReplaceOneAsync Tests

        [Test]
        public async Task ReplaceOneAsync_WithValidParameters_ShouldReturnReplaceResult()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("id", "507f1f77bcf86cd799439011");
            var replacement = MongoTestDataFactory.CreateTestDocument();
            var replaceResult = CreateMockReplaceOneResult();

            _mockTestDocumentCollection.Setup(x => x.ReplaceOneAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<TestDocument>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(replaceResult));

            // Act
            var result = await _mongoGenericDataProvider.ReplaceOneAsync(collection, filter, replacement);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ReplaceOneResult>(result);
            _mockTestDocumentCollection.Verify(x => x.ReplaceOneAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<TestDocument>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region DeleteOneAsync Tests

        [Test]
        public async Task DeleteOneAsync_WithValidFilter_ShouldReturnDeleteResult()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("id", "507f1f77bcf86cd799439011");
            var deleteResult = CreateMockDeleteResult();

            _mockTestDocumentCollection.Setup(x => x.DeleteOneAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(deleteResult));

            // Act
            var result = await _mongoGenericDataProvider.DeleteOneAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DeleteResult>(result);
            _mockTestDocumentCollection.Verify(x => x.DeleteOneAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region DeleteManyAsync Tests

        [Test]
        public async Task DeleteManyAsync_WithValidFilter_ShouldReturnDeleteResult()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("isActive", false);
            var deleteResult = CreateMockDeleteResult(deletedCount: 5);

            _mockTestDocumentCollection.Setup(x => x.DeleteManyAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(deleteResult));

            // Act
            var result = await _mongoGenericDataProvider.DeleteManyAsync(collection, filter);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<DeleteResult>(result);
            _mockTestDocumentCollection.Verify(x => x.DeleteManyAsync(
                It.IsAny<FilterDefinition<TestDocument>>(),
                It.IsAny<CancellationToken>()), Times.Once);
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
                await _mongoGenericDataProvider.GetListOfAsync(null, filter));
        }

        [Test]
        public async Task GetListOfAsync_WithEmptyCollection_ShouldThrowArgumentException()
        {
            // Arrange
            var collection = "";
            var filter = CreateMockFilter<TestDocument>("name", "Test User");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await _mongoGenericDataProvider.GetListOfAsync(collection, filter));
        }

        [Test]
        public async Task InsertOneAsync_WithNullDocument_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoGenericDataProvider.InsertOneAsync<TestDocument>(collection, null));
        }

        [Test]
        public async Task InsertManyAsync_WithNullDocuments_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoGenericDataProvider.InsertManyAsync<TestDocument>(collection, null));
        }

        [Test]
        public async Task ReplaceOneAsync_WithNullReplacement_ShouldThrowArgumentNullException()
        {
            // Arrange
            var collection = "testCollection";
            var filter = CreateMockFilter<TestDocument>("id", "507f1f77bcf86cd799439011");

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _mongoGenericDataProvider.ReplaceOneAsync(collection, filter, null));
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
                await _mongoGenericDataProvider.GetListOfAsync(collection, filter, cancellationTokenSource.Token));
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
                await _mongoGenericDataProvider.InsertOneAsync(collection, document, cancellationTokenSource.Token));
        }

        #endregion

        #region Logging Tests

        [Test]
        public void Constructor_WithLogger_ShouldLogInitialization()
        {
            // Arrange & Act
            var provider = new MongoGenericDataProvider(
                MockConnectionFactory.Object,
                MockSessionManager.Object,
                MockHealthMonitor.Object,
                MockLogger.Object as ILogger<MongoGenericDataProvider>);

            // Assert
            VerifyOperationLogged("MongoDB generic data provider initialized");
        }

        #endregion
    }
}
