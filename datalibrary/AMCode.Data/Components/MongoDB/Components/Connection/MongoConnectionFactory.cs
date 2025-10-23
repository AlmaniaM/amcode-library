using System;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Implementation of MongoDB connection factory.
    /// Provides MongoDB client, database, and collection access with connection pooling.
    /// </summary>
    public class MongoConnectionFactory : IMongoConnectionFactory
    {
        private readonly IMongoClient _client;
        private readonly ILogger<MongoConnectionFactory> _logger;

        /// <summary>
        /// Initializes a new instance of the MongoConnectionFactory class.
        /// </summary>
        /// <param name="connectionString">The MongoDB connection string.</param>
        /// <param name="logger">Optional logger for connection events.</param>
        public MongoConnectionFactory(string connectionString, ILogger<MongoConnectionFactory> logger = null)
        {
            _logger = logger;
            _client = new MongoClient(connectionString);
            
            _logger?.LogInformation("MongoDB connection factory initialized with connection string: {ConnectionString}", 
                MaskConnectionString(connectionString));
        }

        /// <summary>
        /// Initializes a new instance of the MongoConnectionFactory class with an existing client.
        /// </summary>
        /// <param name="client">The MongoDB client instance.</param>
        /// <param name="logger">Optional logger for connection events.</param>
        public MongoConnectionFactory(IMongoClient client, ILogger<MongoConnectionFactory> logger = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
            
            _logger?.LogInformation("MongoDB connection factory initialized with existing client");
        }

        /// <summary>
        /// Gets a MongoDB database instance by name.
        /// </summary>
        /// <param name="databaseName">The name of the database.</param>
        /// <returns>An IMongoDatabase instance.</returns>
        public IMongoDatabase GetDatabase(string databaseName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("Database name cannot be null or empty", nameof(databaseName));
            }

            _logger?.LogDebug("Getting database: {DatabaseName}", databaseName);
            return _client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Gets a MongoDB collection by database name and collection name.
        /// </summary>
        /// <typeparam name="T">The type of documents in the collection.</typeparam>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>An IMongoCollection instance.</returns>
        public IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName)
        {
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("Database name cannot be null or empty", nameof(databaseName));
            }

            if (string.IsNullOrWhiteSpace(collectionName))
            {
                throw new ArgumentException("Collection name cannot be null or empty", nameof(collectionName));
            }

            _logger?.LogDebug("Getting collection: {CollectionName} in database: {DatabaseName}", collectionName, databaseName);
            var database = GetDatabase(databaseName);
            return database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Gets the MongoDB client instance.
        /// </summary>
        /// <returns>An IMongoClient instance.</returns>
        public IMongoClient GetClient()
        {
            _logger?.LogDebug("Getting MongoDB client");
            return _client;
        }

        /// <summary>
        /// Masks sensitive information in connection strings for logging.
        /// </summary>
        /// <param name="connectionString">The connection string to mask.</param>
        /// <returns>A masked connection string safe for logging.</returns>
        private static string MaskConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }

            // Simple masking - replace password=xxx with password=***
            return System.Text.RegularExpressions.Regex.Replace(
                connectionString, 
                @"password=([^;]+)", 
                "password=***", 
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}
