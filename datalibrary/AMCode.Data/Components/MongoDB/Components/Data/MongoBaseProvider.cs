using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using AMCode.Data.MongoDB;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Base class for MongoDB data providers.
    /// Provides common functionality for MongoDB operations including connection management,
    /// error handling, and logging.
    /// </summary>
    public abstract class MongoBaseProvider
    {
        protected readonly IMongoConnectionFactory _connectionFactory;
        protected readonly IMongoSessionManager _sessionManager;
        protected readonly MongoHealthMonitor _healthMonitor;
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the MongoBaseProvider class.
        /// </summary>
        /// <param name="connectionFactory">The MongoDB connection factory.</param>
        /// <param name="sessionManager">The MongoDB session manager.</param>
        /// <param name="healthMonitor">The MongoDB health monitor.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        protected MongoBaseProvider(
            IMongoConnectionFactory connectionFactory,
            IMongoSessionManager sessionManager,
            MongoHealthMonitor healthMonitor,
            ILogger logger)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _healthMonitor = healthMonitor ?? throw new ArgumentNullException(nameof(healthMonitor));
            _logger = logger;
            
            _logger?.LogDebug("MongoDB base provider initialized");
        }

        /// <summary>
        /// Executes an operation with health monitoring and retry logic.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the operation.</returns>
        protected async Task<T> ExecuteWithMonitoringAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger?.LogDebug("Executing MongoDB operation with monitoring");
                
                // Check health before operation
                var isHealthy = await _healthMonitor.IsHealthyAsync(cancellationToken);
                if (!isHealthy)
                {
                    _logger?.LogWarning("MongoDB connection is not healthy, proceeding with operation anyway");
                }

                // Execute with retry logic
                var result = await _healthMonitor.ExecuteWithRetryAsync(operation, cancellationToken);
                
                _logger?.LogDebug("MongoDB operation completed successfully");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "MongoDB operation failed");
                throw;
            }
        }

        /// <summary>
        /// Executes an operation with health monitoring and retry logic.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected async Task ExecuteWithMonitoringAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            await ExecuteWithMonitoringAsync(async () =>
            {
                await operation();
                return true;
            }, cancellationToken);
        }

        /// <summary>
        /// Gets a MongoDB collection with error handling.
        /// </summary>
        /// <typeparam name="T">The type of documents in the collection.</typeparam>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>The MongoDB collection.</returns>
        protected IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
        {
            try
            {
                _logger?.LogDebug("Getting MongoDB collection: {CollectionName} in database: {DatabaseName}", 
                    collectionName, databaseName);
                
                return _connectionFactory.GetCollection<T>(databaseName, collectionName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get MongoDB collection: {CollectionName} in database: {DatabaseName}", 
                    collectionName, databaseName);
                throw new MongoConnectionException($"Failed to get collection {collectionName} in database {databaseName}", ex);
            }
        }

        /// <summary>
        /// Validates collection and database names.
        /// </summary>
        /// <param name="collectionName">The collection name to validate.</param>
        /// <param name="databaseName">The database name to validate.</param>
        protected void ValidateNames(string collectionName, string databaseName = null)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentException("Collection name cannot be null or empty", nameof(collectionName));
            }

            if (!string.IsNullOrWhiteSpace(databaseName) && string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("Database name cannot be empty", nameof(databaseName));
            }
        }

        /// <summary>
        /// Handles MongoDB exceptions and converts them to appropriate AMCode exceptions.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="operation">The operation that failed.</param>
        protected void HandleMongoException(Exception ex, string operation)
        {
            _logger?.LogError(ex, "MongoDB operation failed: {Operation}", operation);

            switch (ex)
            {
                case MongoConnectionException mongoConnectionEx:
                    throw mongoConnectionEx;
                case MongoQueryException mongoQueryEx:
                    throw mongoQueryEx;
                case MongoTransformationException mongoTransformEx:
                    throw mongoTransformEx;
                case MongoCommandException mongoEx:
                    throw new MongoQueryException($"MongoDB command error during {operation}: {mongoEx.Message}", mongoEx);
                case MongoServerException mongoEx:
                    throw new MongoQueryException($"MongoDB server error during {operation}: {mongoEx.Message}", mongoEx);
                case TimeoutException timeoutEx:
                    throw new MongoConnectionException($"MongoDB operation timed out during {operation}", timeoutEx);
                default:
                    throw new MongoQueryException($"Unexpected error during {operation}: {ex.Message}", ex);
            }
        }
    }
}
