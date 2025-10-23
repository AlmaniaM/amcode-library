using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;
using AMCode.Columns.DataTransform;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Main MongoDB data provider that combines all MongoDB operations.
    /// Provides high-level abstractions for MongoDB document operations that mirror
    /// the existing SQL provider capabilities.
    /// </summary>
    public class MongoDataProvider : MongoBaseProvider, IMongoDataProvider
    {
        private readonly IMongoGenericDataProvider _genericDataProvider;
        private readonly IMongoExpandoObjectDataProvider _expandoObjectDataProvider;
        private readonly IMongoExecute _execute;

        /// <summary>
        /// Initializes a new instance of the MongoDataProvider class.
        /// </summary>
        /// <param name="connectionFactory">The MongoDB connection factory.</param>
        /// <param name="sessionManager">The MongoDB session manager.</param>
        /// <param name="healthMonitor">The MongoDB health monitor.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        public MongoDataProvider(
            IMongoConnectionFactory connectionFactory,
            IMongoSessionManager sessionManager,
            MongoHealthMonitor healthMonitor,
            ILogger<MongoDataProvider> logger = null)
            : base(connectionFactory, sessionManager, healthMonitor, logger)
        {
            _genericDataProvider = new MongoGenericDataProvider(connectionFactory, sessionManager, healthMonitor, logger as ILogger<MongoGenericDataProvider>);
            _expandoObjectDataProvider = new MongoExpandoObjectDataProvider(connectionFactory, sessionManager, healthMonitor, logger as ILogger<MongoExpandoObjectDataProvider>);
            _execute = new MongoExecute(connectionFactory, sessionManager, healthMonitor, logger as ILogger<MongoExecute>);
            
            _logger?.LogInformation("MongoDB data provider initialized with all sub-providers");
        }

        #region IMongoGenericDataProvider Implementation

        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of strongly-typed objects.
        /// </summary>
        /// <typeparam name="T">The type of object to map the results to.</typeparam>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of strongly-typed objects.</returns>
        public async Task<IList<T>> GetListOfAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.GetListOfAsync(collection, filter, cancellationToken);
        }

        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of strongly-typed objects
        /// with data transformation applied.
        /// </summary>
        /// <typeparam name="T">The type of object to map the results to.</typeparam>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="transforms">The list of data transformation definitions to apply.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of strongly-typed objects with transformations applied.</returns>
        public async Task<IList<T>> GetListOfAsync<T>(string collection, FilterDefinition<T> filter, IList<IDataTransformColumnDefinition> transforms, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.GetListOfAsync(collection, filter, transforms, cancellationToken);
        }

        /// <summary>
        /// Gets a single value from a specific field in a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the field value.</typeparam>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="field">The name of the field to retrieve.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The value of the specified field.</returns>
        public async Task<T> GetValueOfAsync<T>(string collection, string field, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.GetValueOfAsync(collection, field, filter, cancellationToken);
        }

        /// <summary>
        /// Inserts a single document into a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the document to insert.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="document">The document to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The inserted document with any server-generated fields populated.</returns>
        public async Task<T> InsertOneAsync<T>(string collection, T document, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.InsertOneAsync(collection, document, cancellationToken);
        }

        /// <summary>
        /// Inserts multiple documents into a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the documents to insert.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="documents">The list of documents to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The list of inserted documents with any server-generated fields populated.</returns>
        public async Task<IList<T>> InsertManyAsync<T>(string collection, IList<T> documents, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.InsertManyAsync(collection, documents, cancellationToken);
        }

        /// <summary>
        /// Replaces a single document in a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the document to replace.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify the document to replace.</param>
        /// <param name="replacement">The replacement document.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the replace operation.</returns>
        public async Task<ReplaceOneResult> ReplaceOneAsync<T>(string collection, FilterDefinition<T> filter, T replacement, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.ReplaceOneAsync(collection, filter, replacement, cancellationToken);
        }

        /// <summary>
        /// Deletes a single document from a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the document to delete.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify the document to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the delete operation.</returns>
        public async Task<DeleteResult> DeleteOneAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.DeleteOneAsync(collection, filter, cancellationToken);
        }

        /// <summary>
        /// Deletes multiple documents from a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the documents to delete.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify the documents to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the delete operation.</returns>
        public async Task<DeleteResult> DeleteManyAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new()
        {
            return await _genericDataProvider.DeleteManyAsync(collection, filter, cancellationToken);
        }

        #endregion

        #region IMongoExpandoObjectDataProvider Implementation

        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of ExpandoObjects.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of ExpandoObjects representing the query results.</returns>
        public async Task<IList<ExpandoObject>> GetExpandoListAsync(string collection, FilterDefinition<BsonDocument> filter, CancellationToken cancellationToken = default)
        {
            return await _expandoObjectDataProvider.GetExpandoListAsync(collection, filter, cancellationToken);
        }

        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of ExpandoObjects
        /// with data transformation applied.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="transforms">The list of data transformation definitions to apply.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of ExpandoObjects with transformations applied.</returns>
        public async Task<IList<ExpandoObject>> GetExpandoListAsync(string collection, FilterDefinition<BsonDocument> filter, IList<IDataTransformColumnDefinition> transforms, CancellationToken cancellationToken = default)
        {
            return await _expandoObjectDataProvider.GetExpandoListAsync(collection, filter, transforms, cancellationToken);
        }

        /// <summary>
        /// Inserts a single ExpandoObject document into a MongoDB collection.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="document">The ExpandoObject document to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The inserted ExpandoObject with any server-generated fields populated.</returns>
        public async Task<ExpandoObject> InsertOneExpandoAsync(string collection, ExpandoObject document, CancellationToken cancellationToken = default)
        {
            return await _expandoObjectDataProvider.InsertOneExpandoAsync(collection, document, cancellationToken);
        }

        /// <summary>
        /// Inserts multiple ExpandoObject documents into a MongoDB collection.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="documents">The list of ExpandoObject documents to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The list of inserted ExpandoObjects with any server-generated fields populated.</returns>
        public async Task<IList<ExpandoObject>> InsertManyExpandoAsync(string collection, IList<ExpandoObject> documents, CancellationToken cancellationToken = default)
        {
            return await _expandoObjectDataProvider.InsertManyExpandoAsync(collection, documents, cancellationToken);
        }

        #endregion

        #region IMongoExecute Implementation

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
            await _execute.ExecuteAsync(collection, filter, update, cancellationToken);
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
            await _execute.ExecuteAsync(collection, filter, cancellationToken);
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
            return await _execute.ExecuteBulkWriteAsync(collection, requests, cancellationToken);
        }

        #endregion
    }
}
