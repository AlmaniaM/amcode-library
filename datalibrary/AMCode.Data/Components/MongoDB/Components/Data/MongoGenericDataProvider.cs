using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;
using AMCode.Columns.DataTransform;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Implementation of strongly-typed MongoDB data access operations.
    /// Provides methods for querying MongoDB collections and mapping results to strongly-typed objects.
    /// </summary>
    public class MongoGenericDataProvider : MongoBaseProvider, IMongoGenericDataProvider
    {
        /// <summary>
        /// Initializes a new instance of the MongoGenericDataProvider class.
        /// </summary>
        /// <param name="connectionFactory">The MongoDB connection factory.</param>
        /// <param name="sessionManager">The MongoDB session manager.</param>
        /// <param name="healthMonitor">The MongoDB health monitor.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        public MongoGenericDataProvider(
            IMongoConnectionFactory connectionFactory,
            IMongoSessionManager sessionManager,
            MongoHealthMonitor healthMonitor,
            ILogger<MongoGenericDataProvider> logger = null)
            : base(connectionFactory, sessionManager, healthMonitor, logger)
        {
        }

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
            return await GetListOfAsync<T>(collection, filter, null, cancellationToken);
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
            ValidateNames(collection);

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Executing MongoDB query on collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<T>("default", collection);
                    var cursor = await mongoCollection.FindAsync(filter, cancellationToken: cancellationToken);
                    var documents = await cursor.ToListAsync(cancellationToken);

                    _logger?.LogDebug("Retrieved {Count} documents from collection: {CollectionName}", documents.Count, collection);

                    // Apply transformations if provided
                    if (transforms != null && transforms.Any())
                    {
                        return ApplyTransformations(documents, transforms);
                    }

                    return documents;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"GetListOfAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            if (string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentException("Field name cannot be null or empty", nameof(field));
            }

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Getting value of field {FieldName} from collection: {CollectionName}", field, collection);

                    var mongoCollection = GetCollection<T>("default", collection);
                    var projection = Builders<T>.Projection.Include(field);
                    var cursor = await mongoCollection.FindAsync(filter, new FindOptions<T, T> { Projection = projection }, cancellationToken);
                    var document = await cursor.FirstOrDefaultAsync(cancellationToken);

                    if (document == null)
                    {
                        _logger?.LogDebug("No document found matching filter in collection: {CollectionName}", collection);
                        return default(T);
                    }

                    // Use reflection to get the field value
                    var property = typeof(T).GetProperty(field);
                    if (property != null)
                    {
                        var value = property.GetValue(document);
                        return (T)Convert.ChangeType(value, typeof(T));
                    }

                    throw new MongoQueryException($"Field {field} not found in type {typeof(T).Name}");
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"GetValueOfAsync for field {field} in collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Inserting document into collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<T>("default", collection);
                    await mongoCollection.InsertOneAsync(document, cancellationToken: cancellationToken);

                    _logger?.LogDebug("Successfully inserted document into collection: {CollectionName}", collection);
                    return document;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"InsertOneAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            if (documents == null || !documents.Any())
            {
                throw new ArgumentException("Documents list cannot be null or empty", nameof(documents));
            }

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Inserting {Count} documents into collection: {CollectionName}", documents.Count, collection);

                    var mongoCollection = GetCollection<T>("default", collection);
                    await mongoCollection.InsertManyAsync(documents, cancellationToken: cancellationToken);

                    _logger?.LogDebug("Successfully inserted {Count} documents into collection: {CollectionName}", documents.Count, collection);
                    return documents;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"InsertManyAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            if (replacement == null)
            {
                throw new ArgumentNullException(nameof(replacement));
            }

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Replacing document in collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<T>("default", collection);
                    var result = await mongoCollection.ReplaceOneAsync(filter, replacement, cancellationToken: cancellationToken);

                    _logger?.LogDebug("Replace operation completed in collection: {CollectionName}, matched: {MatchedCount}, modified: {ModifiedCount}", 
                        collection, result.MatchedCount, result.ModifiedCount);
                    
                    return result;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"ReplaceOneAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Deleting document from collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<T>("default", collection);
                    var result = await mongoCollection.DeleteOneAsync(filter, cancellationToken);

                    _logger?.LogDebug("Delete operation completed in collection: {CollectionName}, deleted: {DeletedCount}", 
                        collection, result.DeletedCount);
                    
                    return result;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"DeleteOneAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Deleting documents from collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<T>("default", collection);
                    var result = await mongoCollection.DeleteManyAsync(filter, cancellationToken);

                    _logger?.LogDebug("Delete operation completed in collection: {CollectionName}, deleted: {DeletedCount}", 
                        collection, result.DeletedCount);
                    
                    return result;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"DeleteManyAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
        }

        private IList<T> ApplyTransformations<T>(IList<T> documents, IList<IDataTransformColumnDefinition> transforms) where T : new()
        {
            try
            {
                _logger?.LogDebug("Applying {Count} transformations to {DocumentCount} documents", transforms.Count, documents.Count);

                // For now, return the documents as-is
                // TODO: Implement actual transformation logic based on IDataTransformColumnDefinition
                // This would involve mapping MongoDB document fields to C# object properties
                // and applying any custom transformations defined in the transforms list

                return documents;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to apply transformations to documents");
                throw new MongoTransformationException("Failed to apply transformations to documents", ex);
            }
        }
    }
}
