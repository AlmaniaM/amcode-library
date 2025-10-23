using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Implementation of MongoDB non-query operations and bulk operations.
    /// Provides methods for executing updates, deletes, and bulk write operations.
    /// </summary>
    public class MongoExecute : MongoBaseProvider, IMongoExecute
    {
        /// <summary>
        /// Initializes a new instance of the MongoExecute class.
        /// </summary>
        /// <param name="connectionFactory">The MongoDB connection factory.</param>
        /// <param name="sessionManager">The MongoDB session manager.</param>
        /// <param name="healthMonitor">The MongoDB health monitor.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        public MongoExecute(
            IMongoConnectionFactory connectionFactory,
            IMongoSessionManager sessionManager,
            MongoHealthMonitor healthMonitor,
            ILogger<MongoExecute> logger = null)
            : base(connectionFactory, sessionManager, healthMonitor, logger)
        {
        }

        /// <summary>
        /// Executes an update operation on documents matching the specified filter.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify documents to update.</param>
        /// <param name="update">The update definition to apply.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(string collection, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, CancellationToken cancellationToken = default)
        {
            ValidateNames(collection);

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Executing update operation on collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<BsonDocument>("default", collection);
                    var result = await mongoCollection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);

                    _logger?.LogDebug("Update operation completed on collection: {CollectionName}, matched: {MatchedCount}, modified: {ModifiedCount}", 
                        collection, result.MatchedCount, result.ModifiedCount);
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"ExecuteAsync update for collection {collection}");
                    throw;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Executes a delete operation on documents matching the specified filter.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify documents to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ExecuteAsync(string collection, FilterDefinition<BsonDocument> filter, CancellationToken cancellationToken = default)
        {
            ValidateNames(collection);

            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Executing delete operation on collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<BsonDocument>("default", collection);
                    var result = await mongoCollection.DeleteManyAsync(filter, cancellationToken);

                    _logger?.LogDebug("Delete operation completed on collection: {CollectionName}, deleted: {DeletedCount}", 
                        collection, result.DeletedCount);
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"ExecuteAsync delete for collection {collection}");
                    throw;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Executes a bulk write operation with multiple write requests.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="requests">The list of write model requests to execute.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the bulk write operation.</returns>
        public async Task<BulkWriteResult<BsonDocument>> ExecuteBulkWriteAsync(string collection, IList<WriteModel<BsonDocument>> requests, CancellationToken cancellationToken = default)
        {
            ValidateNames(collection);

            if (requests == null || requests.Count == 0)
            {
                throw new ArgumentException("Requests list cannot be null or empty", nameof(requests));
            }

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Executing bulk write operation on collection: {CollectionName} with {RequestCount} requests", 
                        collection, requests.Count);

                    var mongoCollection = GetCollection<BsonDocument>("default", collection);
                    var result = await mongoCollection.BulkWriteAsync(requests, cancellationToken: cancellationToken);

                    _logger?.LogDebug("Bulk write operation completed on collection: {CollectionName}, processed: {ProcessedCount}, inserted: {InsertedCount}, modified: {ModifiedCount}, deleted: {DeletedCount}", 
                        collection, result.ProcessedRequests.Count, result.InsertedCount, result.ModifiedCount, result.DeletedCount);
                    
                    return result;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"ExecuteBulkWriteAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
        }
    }
}
