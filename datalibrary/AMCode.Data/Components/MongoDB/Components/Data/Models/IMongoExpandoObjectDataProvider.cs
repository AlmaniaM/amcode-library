using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using AMCode.Columns.DataTransform;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for dynamic MongoDB data access operations using ExpandoObject.
    /// Provides methods for querying MongoDB collections and mapping results to dynamic objects.
    /// </summary>
    public interface IMongoExpandoObjectDataProvider
    {
        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of ExpandoObjects.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of ExpandoObjects representing the query results.</returns>
        Task<IList<ExpandoObject>> GetExpandoListAsync(string collection, FilterDefinition<BsonDocument> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of ExpandoObjects
        /// with data transformation applied.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="transforms">The list of data transformation definitions to apply.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of ExpandoObjects with transformations applied.</returns>
        Task<IList<ExpandoObject>> GetExpandoListAsync(string collection, FilterDefinition<BsonDocument> filter, IList<IDataTransformColumnDefinition> transforms, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts a single ExpandoObject document into a MongoDB collection.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="document">The ExpandoObject document to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The inserted ExpandoObject with any server-generated fields populated.</returns>
        Task<ExpandoObject> InsertOneExpandoAsync(string collection, ExpandoObject document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts multiple ExpandoObject documents into a MongoDB collection.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection.</param>
        /// <param name="documents">The list of ExpandoObject documents to insert.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The list of inserted ExpandoObjects with any server-generated fields populated.</returns>
        Task<IList<ExpandoObject>> InsertManyExpandoAsync(string collection, IList<ExpandoObject> documents, CancellationToken cancellationToken = default);
    }
}
