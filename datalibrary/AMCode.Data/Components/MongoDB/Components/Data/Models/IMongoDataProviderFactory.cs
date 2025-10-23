using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for creating MongoDB data provider instances.
    /// Provides factory methods for creating MongoDB data providers with different configurations.
    /// </summary>
    public interface IMongoDataProviderFactory
    {
        /// <summary>
        /// Creates a MongoDB data provider with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The MongoDB connection string.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        /// <returns>A new MongoDB data provider instance.</returns>
        IMongoDataProvider CreateProvider(string connectionString, ILogger logger = null);

        /// <summary>
        /// Creates a MongoDB data provider with the specified connection string and database name.
        /// </summary>
        /// <param name="connectionString">The MongoDB connection string.</param>
        /// <param name="databaseName">The default database name.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        /// <returns>A new MongoDB data provider instance.</returns>
        IMongoDataProvider CreateProvider(string connectionString, string databaseName, ILogger logger = null);

        /// <summary>
        /// Creates a MongoDB data provider with an existing MongoDB client.
        /// </summary>
        /// <param name="client">The MongoDB client instance.</param>
        /// <param name="logger">Optional logger for the provider.</param>
        /// <returns>A new MongoDB data provider instance.</returns>
        IMongoDataProvider CreateProvider(IMongoClient client, ILogger logger = null);
    }
}
