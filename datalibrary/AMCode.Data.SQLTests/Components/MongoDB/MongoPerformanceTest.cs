using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// MongoDB performance tests to validate performance benchmarks
    /// </summary>
    [TestFixture]
    public class MongoPerformanceTest
    {
        private MongoDataProvider _mongoDataProvider;
        private MongoConnectionFactory _connectionFactory;
        private MongoSessionManager _sessionManager;
        private MongoHealthMonitor _healthMonitor;
        private ILogger<MongoDataProvider> _logger;
        private string _testCollectionName;

        // Performance benchmarks from the plan
        private const int SmallDatasetMaxResponseTime = 100; // ms
        private const int MediumDatasetMaxResponseTime = 500; // ms
        private const int LargeDatasetMaxResponseTime = 2000; // ms
        private const int MinOperationsPerSecond = 1000;
        private const int TargetOperationsPerSecond = 2000;

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
            _testCollectionName = $"perf_test_{Guid.NewGuid().ToString("N")[..8]}";
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

        #region Response Time Tests

        [Test]
        public async Task GetListOfAsync_SmallDataset_ShouldMeetResponseTimeTarget()
        {
            // Arrange
            var smallDataset = MongoTestDataFactory.CreateTestDocumentList(50);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, smallDataset);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Empty);
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(50, result.Count);
            Assert.Less(stopwatch.ElapsedMilliseconds, SmallDatasetMaxResponseTime, 
                $"Small dataset query took {stopwatch.ElapsedMilliseconds}ms, expected < {SmallDatasetMaxResponseTime}ms");
        }

        [Test]
        public async Task GetListOfAsync_MediumDataset_ShouldMeetResponseTimeTarget()
        {
            // Arrange
            var mediumDataset = MongoTestDataFactory.CreateTestDocumentList(1000);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, mediumDataset);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Empty);
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1000, result.Count);
            Assert.Less(stopwatch.ElapsedMilliseconds, MediumDatasetMaxResponseTime, 
                $"Medium dataset query took {stopwatch.ElapsedMilliseconds}ms, expected < {MediumDatasetMaxResponseTime}ms");
        }

        [Test]
        public async Task GetListOfAsync_LargeDataset_ShouldMeetResponseTimeTarget()
        {
            // Arrange
            var largeDataset = MongoTestDataFactory.CreateTestDocumentList(10000);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, largeDataset);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Empty);
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10000, result.Count);
            Assert.Less(stopwatch.ElapsedMilliseconds, LargeDatasetMaxResponseTime, 
                $"Large dataset query took {stopwatch.ElapsedMilliseconds}ms, expected < {LargeDatasetMaxResponseTime}ms");
        }

        [Test]
        public async Task GetListOfAsync_WithFilter_ShouldMeetResponseTimeTarget()
        {
            // Arrange
            var dataset = MongoTestDataFactory.CreateTestDocumentList(5000);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, dataset);

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Gte(x => x.Age, 25));
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.Less(stopwatch.ElapsedMilliseconds, MediumDatasetMaxResponseTime, 
                $"Filtered query took {stopwatch.ElapsedMilliseconds}ms, expected < {MediumDatasetMaxResponseTime}ms");
        }

        #endregion

        #region Throughput Tests

        [Test]
        public async Task InsertManyAsync_Throughput_ShouldMeetMinimumTarget()
        {
            // Arrange
            var batchSize = 100;
            var batches = 10;
            var totalDocuments = batchSize * batches;

            // Act
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < batches; i++)
            {
                var batch = MongoTestDataFactory.CreateTestDocumentList(batchSize);
                await _mongoDataProvider.InsertManyAsync(_testCollectionName, batch);
            }
            stopwatch.Stop();

            // Assert
            var operationsPerSecond = totalDocuments / stopwatch.Elapsed.TotalSeconds;
            Assert.GreaterOrEqual(operationsPerSecond, MinOperationsPerSecond, 
                $"Throughput was {operationsPerSecond:F0} ops/sec, expected >= {MinOperationsPerSecond} ops/sec");
        }

        [Test]
        public async Task BulkWriteAsync_Throughput_ShouldMeetMinimumTarget()
        {
            // Arrange
            var batchSize = 1000;
            var requests = new List<WriteModel<BsonDocument>>();
            
            for (int i = 0; i < batchSize; i++)
            {
                var document = CreateTestBsonDocument($"User {i}", 20 + i, $"user{i}@example.com");
                requests.Add(new InsertOneModel<BsonDocument>(document));
            }

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = await _mongoDataProvider.ExecuteBulkWriteAsync(_testCollectionName, requests);
            stopwatch.Stop();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsAcknowledged);
            var operationsPerSecond = batchSize / stopwatch.Elapsed.TotalSeconds;
            Assert.GreaterOrEqual(operationsPerSecond, MinOperationsPerSecond, 
                $"Bulk write throughput was {operationsPerSecond:F0} ops/sec, expected >= {MinOperationsPerSecond} ops/sec");
        }

        [Test]
        public async Task ConcurrentOperations_Throughput_ShouldMeetMinimumTarget()
        {
            // Arrange
            var concurrentTasks = 10;
            var documentsPerTask = 100;
            var totalDocuments = concurrentTasks * documentsPerTask;

            // Act
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task>();
            
            for (int i = 0; i < concurrentTasks; i++)
            {
                var taskIndex = i;
                var task = Task.Run(async () =>
                {
                    var documents = MongoTestDataFactory.CreateTestDocumentList(documentsPerTask);
                    await _mongoDataProvider.InsertManyAsync($"{_testCollectionName}_{taskIndex}", documents);
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            var operationsPerSecond = totalDocuments / stopwatch.Elapsed.TotalSeconds;
            Assert.GreaterOrEqual(operationsPerSecond, MinOperationsPerSecond, 
                $"Concurrent operations throughput was {operationsPerSecond:F0} ops/sec, expected >= {MinOperationsPerSecond} ops/sec");
        }

        #endregion

        #region Memory Usage Tests

        [Test]
        public async Task LargeDataset_MemoryUsage_ShouldBeEfficient()
        {
            // Arrange
            var largeDataset = MongoTestDataFactory.CreateTestDocumentList(5000);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, largeDataset);

            // Act
            var memoryBefore = GC.GetTotalMemory(false);
            var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                _testCollectionName, 
                Builders<TestDocument>.Filter.Empty);
            var memoryAfter = GC.GetTotalMemory(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5000, result.Count);
            
            var memoryUsed = memoryAfter - memoryBefore;
            var memoryPerDocument = memoryUsed / 5000.0;
            
            // Should use less than 1KB per document
            Assert.Less(memoryPerDocument, 1024, 
                $"Memory usage was {memoryPerDocument:F0} bytes per document, expected < 1024 bytes");
        }

        [Test]
        public async Task ComplexDocument_MemoryUsage_ShouldBeEfficient()
        {
            // Arrange
            var complexDocuments = MongoTestDataFactory.CreateComplexTestDocumentList(1000);
            await _mongoDataProvider.InsertManyAsync(_testCollectionName, complexDocuments);

            // Act
            var memoryBefore = GC.GetTotalMemory(false);
            var result = await _mongoDataProvider.GetListOfAsync<ComplexTestDocument>(
                _testCollectionName, 
                Builders<ComplexTestDocument>.Filter.Empty);
            var memoryAfter = GC.GetTotalMemory(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1000, result.Count);
            
            var memoryUsed = memoryAfter - memoryBefore;
            var memoryPerDocument = memoryUsed / 1000.0;
            
            // Complex documents should use less than 2KB per document
            Assert.Less(memoryPerDocument, 2048, 
                $"Memory usage was {memoryPerDocument:F0} bytes per complex document, expected < 2048 bytes");
        }

        #endregion

        #region Connection Efficiency Tests

        [Test]
        public async Task MultipleOperations_ConnectionEfficiency_ShouldReuseConnections()
        {
            // Arrange
            var operations = 100;
            var documents = MongoTestDataFactory.CreateTestDocumentList(10);

            // Act
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < operations; i++)
            {
                await _mongoDataProvider.InsertManyAsync($"{_testCollectionName}_{i}", documents);
                var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                    $"{_testCollectionName}_{i}", 
                    Builders<TestDocument>.Filter.Empty);
                Assert.AreEqual(10, result.Count);
            }
            stopwatch.Stop();

            // Assert
            var averageTimePerOperation = stopwatch.ElapsedMilliseconds / operations;
            Assert.Less(averageTimePerOperation, 50, 
                $"Average time per operation was {averageTimePerOperation}ms, expected < 50ms (connection reuse)");
        }

        [Test]
        public async Task SessionManagement_Efficiency_ShouldBeOptimal()
        {
            // Arrange
            var documents = MongoTestDataFactory.CreateTestDocumentList(100);

            // Act
            var stopwatch = Stopwatch.StartNew();
            
            // Test session creation and reuse
            using (var session = await _sessionManager.StartSessionAsync())
            {
                await _mongoDataProvider.InsertManyAsync(_testCollectionName, documents);
                var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                    _testCollectionName, 
                    Builders<TestDocument>.Filter.Empty);
                Assert.AreEqual(100, result.Count);
            }
            
            stopwatch.Stop();

            // Assert
            Assert.Less(stopwatch.ElapsedMilliseconds, 200, 
                $"Session operations took {stopwatch.ElapsedMilliseconds}ms, expected < 200ms");
        }

        #endregion

        #region Scalability Tests

        [Test]
        public async Task Scalability_WithIncreasingLoad_ShouldMaintainPerformance()
        {
            // Arrange
            var loadLevels = new[] { 100, 500, 1000, 2000 };
            var results = new List<double>();

            // Act
            foreach (var load in loadLevels)
            {
                var documents = MongoTestDataFactory.CreateTestDocumentList(load);
                var stopwatch = Stopwatch.StartNew();
                
                await _mongoDataProvider.InsertManyAsync($"{_testCollectionName}_{load}", documents);
                var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                    $"{_testCollectionName}_{load}", 
                    Builders<TestDocument>.Filter.Empty);
                
                stopwatch.Stop();
                
                var operationsPerSecond = load / stopwatch.Elapsed.TotalSeconds;
                results.Add(operationsPerSecond);
                
                Assert.AreEqual(load, result.Count);
            }

            // Assert
            // Performance should not degrade significantly with increased load
            var minPerformance = results.Min();
            var maxPerformance = results.Max();
            var performanceRatio = minPerformance / maxPerformance;
            
            Assert.Greater(performanceRatio, 0.5, 
                $"Performance degradation ratio was {performanceRatio:F2}, expected > 0.5 (min: {minPerformance:F0}, max: {maxPerformance:F0})");
        }

        [Test]
        public async Task ConcurrentUsers_Performance_ShouldScaleLinearly()
        {
            // Arrange
            var concurrentUsers = new[] { 1, 5, 10, 20 };
            var documentsPerUser = 50;
            var results = new List<double>();

            // Act
            foreach (var users in concurrentUsers)
            {
                var tasks = new List<Task>();
                var stopwatch = Stopwatch.StartNew();
                
                for (int i = 0; i < users; i++)
                {
                    var userIndex = i;
                    var task = Task.Run(async () =>
                    {
                        var documents = MongoTestDataFactory.CreateTestDocumentList(documentsPerUser);
                        await _mongoDataProvider.InsertManyAsync($"{_testCollectionName}_{users}_{userIndex}", documents);
                        
                        var result = await _mongoDataProvider.GetListOfAsync<TestDocument>(
                            $"{_testCollectionName}_{users}_{userIndex}", 
                            Builders<TestDocument>.Filter.Empty);
                        
                        Assert.AreEqual(documentsPerUser, result.Count);
                    });
                    tasks.Add(task);
                }
                
                await Task.WhenAll(tasks);
                stopwatch.Stop();
                
                var totalOperations = users * documentsPerUser * 2; // Insert + Query
                var operationsPerSecond = totalOperations / stopwatch.Elapsed.TotalSeconds;
                results.Add(operationsPerSecond);
            }

            // Assert
            // Performance should scale reasonably with concurrent users
            var singleUserPerformance = results[0];
            var multiUserPerformance = results.Last();
            var scalingFactor = multiUserPerformance / singleUserPerformance;
            
            Assert.Greater(scalingFactor, 0.3, 
                $"Scaling factor was {scalingFactor:F2}, expected > 0.3 (single: {singleUserPerformance:F0}, multi: {multiUserPerformance:F0})");
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
