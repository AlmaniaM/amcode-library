using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using AMCode.Columns.DataTransform;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for strongly-typed MongoDB data access operations.
    /// Provides methods for querying MongoDB collections and mapping results to strongly-typed objects.
    /// </summary>
    public interface IMongoGenericDataProvider
    {
        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of strongly-typed objects.
        /// </summary>
        /// <typeparam name="T">The type of object to map the results to.</typeparam>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of strongly-typed objects.</returns>
        Task<IList<T>> GetListOfAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();

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
        Task<IList<T>> GetListOfAsync<T>(string collection, FilterDefinition<T> filter, IList<IDataTransformColumnDefinition> transforms, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Gets a single value from a specific field in a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the field value.</typeparam>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="field">The name of the field to retrieve.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The value of the specified field.</returns>
        Task<T> GetValueOfAsync<T>(string collection, string field, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Inserts a single document into a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the document to insert.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="document">The document to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The inserted document with any server-generated fields populated.</returns>
        Task<T> InsertOneAsync<T>(string collection, T document, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Inserts multiple documents into a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the documents to insert.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="documents">The list of documents to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The list of inserted documents with any server-generated fields populated.</returns>
        Task<IList<T>> InsertManyAsync<T>(string collection, IList<T> documents, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Replaces a single document in a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the document to replace.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify the document to replace.</param>
        /// <param name="replacement">The replacement document.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the replace operation.</returns>
        Task<ReplaceOneResult> ReplaceOneAsync<T>(string collection, FilterDefinition<T> filter, T replacement, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Deletes a single document from a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the document to delete.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify the document to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the delete operation.</returns>
        Task<DeleteResult> DeleteOneAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();

        /// <summary>
        /// Deletes multiple documents from a MongoDB collection.
        /// </summary>
        /// <typeparam name="T">The type of the documents to delete.</typeparam>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="filter">The filter definition to identify the documents to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The result of the delete operation.</returns>
        Task<DeleteResult> DeleteManyAsync<T>(string collection, FilterDefinition<T> filter, CancellationToken cancellationToken = default) where T : new();
    }
}
