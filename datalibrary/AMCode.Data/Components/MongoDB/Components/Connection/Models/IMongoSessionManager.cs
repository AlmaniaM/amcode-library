using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for MongoDB session management and transaction support.
    /// Provides methods for creating and managing MongoDB sessions and transactions.
    /// </summary>
    public interface IMongoSessionManager
    {
        /// <summary>
        /// Creates a new MongoDB session.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A new IClientSessionHandle instance.</returns>
        Task<IClientSessionHandle> CreateSessionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes an operation within a transaction.
        /// </summary>
        /// <param name="operation">The operation to execute within the transaction.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteInTransactionAsync(Func<IClientSessionHandle, Task> operation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes an operation within a transaction and returns a result.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="operation">The operation to execute within the transaction.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the operation.</returns>
        Task<T> ExecuteInTransactionAsync<T>(Func<IClientSessionHandle, Task<T>> operation, CancellationToken cancellationToken = default);
    }
}
