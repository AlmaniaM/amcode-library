using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// MongoDB connection health monitoring and retry logic.
    /// Provides health checks, connection status monitoring, and automatic retry capabilities.
    /// </summary>
    public class MongoHealthMonitor
    {
        private readonly IMongoClient _client;
        private readonly ILogger<MongoHealthMonitor> _logger;
        private readonly int _maxRetryAttempts;
        private readonly TimeSpan _retryDelay;

        /// <summary>
        /// Initializes a new instance of the MongoHealthMonitor class.
        /// </summary>
        /// <param name="client">The MongoDB client to monitor.</param>
        /// <param name="logger">Optional logger for health monitoring events.</param>
        /// <param name="maxRetryAttempts">Maximum number of retry attempts for failed operations.</param>
        /// <param name="retryDelay">Delay between retry attempts.</param>
        public MongoHealthMonitor(
            IMongoClient client, 
            ILogger<MongoHealthMonitor> logger = null, 
            int maxRetryAttempts = 3, 
            TimeSpan? retryDelay = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
            _maxRetryAttempts = maxRetryAttempts;
            _retryDelay = retryDelay ?? TimeSpan.FromSeconds(1);
            
            _logger?.LogInformation("MongoDB health monitor initialized with max retry attempts: {MaxRetryAttempts}, retry delay: {RetryDelay}", 
                _maxRetryAttempts, _retryDelay);
        }

        /// <summary>
        /// Checks the health of the MongoDB connection.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>True if the connection is healthy, false otherwise.</returns>
        public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger?.LogDebug("Checking MongoDB connection health");
                
                // Ping the database to check connectivity
                await _client.ListDatabaseNamesAsync(cancellationToken);
                
                _logger?.LogDebug("MongoDB connection is healthy");
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "MongoDB connection health check failed");
                return false;
            }
        }

        /// <summary>
        /// Executes an operation with retry logic.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteWithRetryAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            await ExecuteWithRetryAsync(async () =>
            {
                await operation();
                return true;
            }, cancellationToken);
        }

        /// <summary>
        /// Executes an operation with retry logic and returns a result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            Exception lastException = null;

            for (int attempt = 1; attempt <= _maxRetryAttempts; attempt++)
            {
                try
                {
                    _logger?.LogDebug("Executing operation with retry logic, attempt {Attempt} of {MaxAttempts}", 
                        attempt, _maxRetryAttempts);

                    var result = await operation();
                    
                    if (attempt > 1)
                    {
                        _logger?.LogInformation("Operation succeeded on attempt {Attempt}", attempt);
                    }
                    
                    return result;
                }
                catch (Exception ex) when (IsRetryableException(ex) && attempt < _maxRetryAttempts)
                {
                    lastException = ex;
                    _logger?.LogWarning(ex, "Operation failed on attempt {Attempt}, retrying in {RetryDelay}ms", 
                        attempt, _retryDelay.TotalMilliseconds);
                    
                    await Task.Delay(_retryDelay, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Operation failed on attempt {Attempt} and is not retryable", attempt);
                    throw;
                }
            }

            _logger?.LogError(lastException, "Operation failed after {MaxAttempts} attempts", _maxRetryAttempts);
            throw new MongoConnectionException($"Operation failed after {_maxRetryAttempts} attempts", lastException);
        }

        /// <summary>
        /// Gets the current connection status.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A connection status object.</returns>
        public async Task<MongoConnectionStatus> GetConnectionStatusAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var isHealthy = await IsHealthyAsync(cancellationToken);
                var serverInfo = await GetServerInfoAsync(cancellationToken);
                
                return new MongoConnectionStatus
                {
                    IsHealthy = isHealthy,
                    ServerVersion = serverInfo?.Version,
                    ServerType = serverInfo?.Type,
                    LastChecked = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get connection status");
                return new MongoConnectionStatus
                {
                    IsHealthy = false,
                    LastChecked = DateTime.UtcNow,
                    Error = ex.Message
                };
            }
        }

        private async Task<MongoServerInfo> GetServerInfoAsync(CancellationToken cancellationToken)
        {
            try
            {
                var adminDb = _client.GetDatabase("admin");
                var result = await adminDb.RunCommandAsync<BsonDocument>("{buildInfo: 1}", cancellationToken: cancellationToken);
                
                return new MongoServerInfo
                {
                    Version = result.GetValue("version", "Unknown").AsString,
                    Type = result.GetValue("type", "Unknown").AsString
                };
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Failed to get server info");
                return null;
            }
        }

        private static bool IsRetryableException(Exception ex)
        {
            // Check for common retryable MongoDB exceptions
            return ex is MongoConnectionException ||
                   ex is TimeoutException ||
                   ex is MongoServerException ||
                   ex is MongoCommandException ||
                   (ex.InnerException != null && IsRetryableException(ex.InnerException));
        }
    }

    /// <summary>
    /// Represents the status of a MongoDB connection.
    /// </summary>
    public class MongoConnectionStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether the connection is healthy.
        /// </summary>
        public bool IsHealthy { get; set; }

        /// <summary>
        /// Gets or sets the server version.
        /// </summary>
        public string ServerVersion { get; set; }

        /// <summary>
        /// Gets or sets the server type.
        /// </summary>
        public string ServerType { get; set; }

        /// <summary>
        /// Gets or sets the last time the status was checked.
        /// </summary>
        public DateTime LastChecked { get; set; }

        /// <summary>
        /// Gets or sets any error message if the connection is not healthy.
        /// </summary>
        public string Error { get; set; }
    }

    /// <summary>
    /// Represents MongoDB server information.
    /// </summary>
    public class MongoServerInfo
    {
        /// <summary>
        /// Gets or sets the server version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the server type.
        /// </summary>
        public string Type { get; set; }
    }
}
