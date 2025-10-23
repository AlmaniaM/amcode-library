using System;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Factory for creating MongoDB data provider instances.
    /// Provides factory methods for creating MongoDB data providers with different configurations.
    /// </summary>
    public class MongoDataProviderFactory : IMongoDataProviderFactory
    {
        /// <summary>
        /// Creates a MongoDB data provider with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The MongoDB connection string.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        /// <returns>A new MongoDB data provider instance.</returns>
        public IMongoDataProvider CreateProvider(string connectionString, ILogger logger = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
            }

            logger?.LogDebug("Creating MongoDB data provider with connection string");

            try
            {
                var connectionFactory = new MongoConnectionFactory(connectionString, logger as ILogger<MongoConnectionFactory>);
                var client = connectionFactory.GetClient();
                var sessionManager = new MongoSessionManager(client, logger as ILogger<MongoSessionManager>);
                var healthMonitor = new MongoHealthMonitor(client, logger as ILogger<MongoHealthMonitor>);

                return new MongoDataProvider(connectionFactory, sessionManager, healthMonitor, logger as ILogger<MongoDataProvider>);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to create MongoDB data provider with connection string");
                throw new MongoConnectionException("Failed to create MongoDB data provider", ex);
            }
        }

        /// <summary>
        /// Creates a MongoDB data provider with the specified connection string and database name.
        /// </summary>
        /// <param name="connectionString">The MongoDB connection string.</param>
        /// <param name="databaseName">The default database name.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        /// <returns>A new MongoDB data provider instance.</returns>
        public IMongoDataProvider CreateProvider(string connectionString, string databaseName, ILogger logger = null)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be null or empty", nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new ArgumentException("Database name cannot be null or empty", nameof(databaseName));
            }

            logger?.LogDebug("Creating MongoDB data provider with connection string and database: {DatabaseName}", databaseName);

            try
            {
                var connectionFactory = new MongoConnectionFactory(connectionString, logger as ILogger<MongoConnectionFactory>);
                var client = connectionFactory.GetClient();
                var sessionManager = new MongoSessionManager(client, logger as ILogger<MongoSessionManager>);
                var healthMonitor = new MongoHealthMonitor(client, logger as ILogger<MongoHealthMonitor>);

                return new MongoDataProvider(connectionFactory, sessionManager, healthMonitor, logger as ILogger<MongoDataProvider>);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to create MongoDB data provider with connection string and database: {DatabaseName}", databaseName);
                throw new MongoConnectionException($"Failed to create MongoDB data provider for database {databaseName}", ex);
            }
        }

        /// <summary>
        /// Creates a MongoDB data provider with an existing MongoDB client.
        /// </summary>
        /// <param name="client">The MongoDB client instance.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        /// <returns>A new MongoDB data provider instance.</returns>
        public IMongoDataProvider CreateProvider(IMongoClient client, ILogger logger = null)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            logger?.LogDebug("Creating MongoDB data provider with existing client");

            try
            {
                var connectionFactory = new MongoConnectionFactory(client, logger as ILogger<MongoConnectionFactory>);
                var sessionManager = new MongoSessionManager(client, logger as ILogger<MongoSessionManager>);
                var healthMonitor = new MongoHealthMonitor(client, logger as ILogger<MongoHealthMonitor>);

                return new MongoDataProvider(connectionFactory, sessionManager, healthMonitor, logger as ILogger<MongoDataProvider>);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Failed to create MongoDB data provider with existing client");
                throw new MongoConnectionException("Failed to create MongoDB data provider with existing client", ex);
            }
        }
    }
}
