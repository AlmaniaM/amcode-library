using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for MongoDB non-query operations and bulk operations.
    /// Provides methods for executing updates, deletes, and bulk write operations.
    /// </summary>
    public interface IMongoExecute
    {
        /// <summary>
        /// Executes an update operation on documents matching the specified filter.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify documents to update.</param>
        /// <param name="update">The update definition to apply.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteAsync(string collection, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a delete operation on documents matching the specified filter.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify documents to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ExecuteAsync(string collection, FilterDefinition<BsonDocument> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a bulk write operation with multiple write requests.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="requests">The list of write model requests to execute.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the bulk write operation.</returns>
        Task<BulkWriteResult<BsonDocument>> ExecuteBulkWriteAsync(string collection, IList<WriteModel<BsonDocument>> requests, CancellationToken cancellationToken = default);
    }
}
