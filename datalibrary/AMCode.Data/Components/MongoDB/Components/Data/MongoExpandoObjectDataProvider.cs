using System;
using System.Collections.Generic;
using System.Dynamic;
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
    /// Implementation of dynamic MongoDB data access operations using ExpandoObject.
    /// Provides methods for querying MongoDB collections and mapping results to dynamic objects.
    /// </summary>
    public class MongoExpandoObjectDataProvider : MongoBaseProvider, IMongoExpandoObjectDataProvider
    {
        /// <summary>
        /// Initializes a new instance of the MongoExpandoObjectDataProvider class.
        /// </summary>
        /// <param name="connectionFactory">The MongoDB connection factory.</param>
        /// <param name="sessionManager">The MongoDB session manager.</param>
        /// <param name="healthMonitor">The MongoDB health monitor.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        public MongoExpandoObjectDataProvider(
            IMongoConnectionFactory connectionFactory,
            IMongoSessionManager sessionManager,
            MongoHealthMonitor healthMonitor,
            ILogger<MongoExpandoObjectDataProvider> logger = null)
            : base(connectionFactory, sessionManager, healthMonitor, logger)
        {
        }

        /// <summary>
        /// Executes a filter on a MongoDB collection and maps the result to a list of ExpandoObjects.
        /// </summary>
        /// <param name="collection">The name of the MongoDB collection to query.</param>
        /// <param name="filter">The filter definition for the query.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of ExpandoObjects representing the query results.</returns>
        public async Task<IList<ExpandoObject>> GetExpandoListAsync(string collection, FilterDefinition<BsonDocument> filter, CancellationToken cancellationToken = default)
        {
            return await GetExpandoListAsync(collection, filter, null, cancellationToken);
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
            ValidateNames(collection);

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Executing MongoDB query on collection: {CollectionName} for ExpandoObjects", collection);

                    var mongoCollection = GetCollection<BsonDocument>("default", collection);
                    var cursor = await mongoCollection.FindAsync(filter, cancellationToken: cancellationToken);
                    var documents = await cursor.ToListAsync(cancellationToken);

                    _logger?.LogDebug("Retrieved {Count} documents from collection: {CollectionName}", documents.Count, collection);

                    // Convert BsonDocuments to ExpandoObjects
                    var expandoObjects = documents.Select(ConvertToExpandoObject).ToList();

                    // Apply transformations if provided
                    if (transforms != null && transforms.Any())
                    {
                        return ApplyTransformations(expandoObjects, transforms);
                    }

                    return expandoObjects;
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"GetExpandoListAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Inserting ExpandoObject document into collection: {CollectionName}", collection);

                    var mongoCollection = GetCollection<BsonDocument>("default", collection);
                    var bsonDocument = ConvertToBsonDocument(document);
                    
                    await mongoCollection.InsertOneAsync(bsonDocument, cancellationToken: cancellationToken);

                    _logger?.LogDebug("Successfully inserted ExpandoObject document into collection: {CollectionName}", collection);
                    
                    // Convert back to ExpandoObject to include any server-generated fields
                    return ConvertToExpandoObject(bsonDocument);
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"InsertOneExpandoAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
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
            ValidateNames(collection);

            if (documents == null || !documents.Any())
            {
                throw new ArgumentException("Documents list cannot be null or empty", nameof(documents));
            }

            return await ExecuteWithMonitoringAsync(async () =>
            {
                try
                {
                    _logger?.LogDebug("Inserting {Count} ExpandoObject documents into collection: {CollectionName}", documents.Count, collection);

                    var mongoCollection = GetCollection<BsonDocument>("default", collection);
                    var bsonDocuments = documents.Select(ConvertToBsonDocument).ToList();
                    
                    await mongoCollection.InsertManyAsync(bsonDocuments, cancellationToken: cancellationToken);

                    _logger?.LogDebug("Successfully inserted {Count} ExpandoObject documents into collection: {CollectionName}", documents.Count, collection);
                    
                    // Convert back to ExpandoObjects to include any server-generated fields
                    return bsonDocuments.Select(ConvertToExpandoObject).ToList();
                }
                catch (Exception ex)
                {
                    HandleMongoException(ex, $"InsertManyExpandoAsync for collection {collection}");
                    throw;
                }
            }, cancellationToken);
        }

        /// <summary>
        /// Converts a BsonDocument to an ExpandoObject.
        /// </summary>
        /// <param name="bsonDocument">The BsonDocument to convert.</param>
        /// <returns>An ExpandoObject representation of the BsonDocument.</returns>
        private ExpandoObject ConvertToExpandoObject(BsonDocument bsonDocument)
        {
            try
            {
                var expando = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expando;

                foreach (var element in bsonDocument)
                {
                    dictionary[element.Name] = ConvertBsonValueToObject(element.Value);
                }

                return expando;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to convert BsonDocument to ExpandoObject");
                throw new MongoTransformationException("Failed to convert BsonDocument to ExpandoObject", ex);
            }
        }

        /// <summary>
        /// Converts an ExpandoObject to a BsonDocument.
        /// </summary>
        /// <param name="expandoObject">The ExpandoObject to convert.</param>
        /// <returns>A BsonDocument representation of the ExpandoObject.</returns>
        private BsonDocument ConvertToBsonDocument(ExpandoObject expandoObject)
        {
            try
            {
                var bsonDocument = new BsonDocument();
                var dictionary = (IDictionary<string, object>)expandoObject;

                foreach (var kvp in dictionary)
                {
                    bsonDocument[kvp.Key] = ConvertObjectToBsonValue(kvp.Value);
                }

                return bsonDocument;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to convert ExpandoObject to BsonDocument");
                throw new MongoTransformationException("Failed to convert ExpandoObject to BsonDocument", ex);
            }
        }

        /// <summary>
        /// Converts a BsonValue to a C# object.
        /// </summary>
        /// <param name="bsonValue">The BsonValue to convert.</param>
        /// <returns>A C# object representation of the BsonValue.</returns>
        private object ConvertBsonValueToObject(BsonValue bsonValue)
        {
            if (bsonValue == null || bsonValue.IsBsonNull)
            {
                return null;
            }

            switch (bsonValue.BsonType)
            {
                case BsonType.Document:
                    return ConvertToExpandoObject(bsonValue.AsBsonDocument);
                
                case BsonType.Array:
                    return bsonValue.AsBsonArray.Select(ConvertBsonValueToObject).ToList();
                
                case BsonType.String:
                    return bsonValue.AsString;
                
                case BsonType.Int32:
                    return bsonValue.AsInt32;
                
                case BsonType.Int64:
                    return bsonValue.AsInt64;
                
                case BsonType.Double:
                    return bsonValue.AsDouble;
                
                case BsonType.Decimal128:
                    return Decimal128.ToDecimal(bsonValue.AsDecimal128);
                
                case BsonType.Boolean:
                    return bsonValue.AsBoolean;
                
                case BsonType.DateTime:
                    return bsonValue.ToUniversalTime();
                
                case BsonType.ObjectId:
                    return bsonValue.AsObjectId.ToString();
                
                case BsonType.Binary:
                    return bsonValue.AsBsonBinaryData.Bytes;
                
                default:
                    return bsonValue.ToJson();
            }
        }

        /// <summary>
        /// Converts a C# object to a BsonValue.
        /// </summary>
        /// <param name="value">The C# object to convert.</param>
        /// <returns>A BsonValue representation of the C# object.</returns>
        private BsonValue ConvertObjectToBsonValue(object value)
        {
            if (value == null)
            {
                return BsonNull.Value;
            }

            switch (value)
            {
                case string str:
                    return new BsonString(str);
                
                case int intVal:
                    return new BsonInt32(intVal);
                
                case long longVal:
                    return new BsonInt64(longVal);
                
                case double doubleVal:
                    return new BsonDouble(doubleVal);
                
                case decimal decimalVal:
                    return new BsonDecimal128(decimalVal);
                
                case bool boolVal:
                    return new BsonBoolean(boolVal);
                
                case DateTime dateTimeVal:
                    return new BsonDateTime(dateTimeVal);
                
                case byte[] bytes:
                    return new BsonBinaryData(bytes);
                
                case ExpandoObject expando:
                    return ConvertToBsonDocument(expando);
                
                case System.Collections.IEnumerable enumerable:
                    var array = new BsonArray();
                    foreach (var item in enumerable)
                    {
                        array.Add(ConvertObjectToBsonValue(item));
                    }
                    return array;
                
                default:
                    return BsonValue.Create(value);
            }
        }

        /// <summary>
        /// Applies transformations to a list of ExpandoObjects.
        /// </summary>
        /// <param name="expandoObjects">The list of ExpandoObjects to transform.</param>
        /// <param name="transforms">The list of transformation definitions.</param>
        /// <returns>The transformed list of ExpandoObjects.</returns>
        private IList<ExpandoObject> ApplyTransformations(IList<ExpandoObject> expandoObjects, IList<IDataTransformColumnDefinition> transforms)
        {
            try
            {
                _logger?.LogDebug("Applying {Count} transformations to {DocumentCount} ExpandoObjects", transforms.Count, expandoObjects.Count);

                // For now, return the ExpandoObjects as-is
                // TODO: Implement actual transformation logic based on IDataTransformColumnDefinition
                // This would involve mapping MongoDB document fields to ExpandoObject properties
                // and applying any custom transformations defined in the transforms list

                return expandoObjects;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to apply transformations to ExpandoObjects");
                throw new MongoTransformationException("Failed to apply transformations to ExpandoObjects", ex);
            }
        }
    }
}
