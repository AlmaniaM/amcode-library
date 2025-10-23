using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Implementation of MongoDB session management and transaction support.
    /// Provides methods for creating and managing MongoDB sessions and transactions.
    /// </summary>
    public class MongoSessionManager : IMongoSessionManager
    {
        private readonly IMongoClient _client;
        private readonly ILogger<MongoSessionManager> _logger;

        /// <summary>
        /// Initializes a new instance of the MongoSessionManager class.
        /// </summary>
        /// <param name="client">The MongoDB client instance.</param>
        /// <param name="logger">Optional logger for session events.</param>
        public MongoSessionManager(IMongoClient client, ILogger<MongoSessionManager> logger = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
            
            _logger?.LogInformation("MongoDB session manager initialized");
        }

        /// <summary>
        /// Creates a new MongoDB session.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A new IClientSessionHandle instance.</returns>
        public async Task<IClientSessionHandle> CreateSessionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger?.LogDebug("Creating new MongoDB session");
                var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
                
                _logger?.LogDebug("MongoDB session created successfully");
                return session;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to create MongoDB session");
                throw;
            }
        }

        /// <summary>
        /// Executes an operation within a transaction.
        /// </summary>
        /// <param name="operation">The operation to execute within the transaction.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteInTransactionAsync(Func<IClientSessionHandle, Task> operation, CancellationToken cancellationToken = default)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            using var session = await CreateSessionAsync(cancellationToken);
            
            try
            {
                _logger?.LogDebug("Starting MongoDB transaction");
                await session.WithTransactionAsync(async (s, ct) =>
                {
                    await operation(s);
                    return true; // Transaction completed successfully
                }, cancellationToken: cancellationToken);
                
                _logger?.LogDebug("MongoDB transaction completed successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "MongoDB transaction failed");
                throw;
            }
        }

        /// <summary>
        /// Executes an operation within a transaction and returns a result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="operation">The operation to execute within the transaction.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the operation.</returns>
        public async Task<T> ExecuteInTransactionAsync<T>(Func<IClientSessionHandle, Task<T>> operation, CancellationToken cancellationToken = default)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            using var session = await CreateSessionAsync(cancellationToken);
            
            try
            {
                _logger?.LogDebug("Starting MongoDB transaction with return value");
                var result = await session.WithTransactionAsync(async (s, ct) =>
                {
                    return await operation(s);
                }, cancellationToken: cancellationToken);
                
                _logger?.LogDebug("MongoDB transaction completed successfully with result");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "MongoDB transaction failed");
                throw;
            }
        }
    }
}
