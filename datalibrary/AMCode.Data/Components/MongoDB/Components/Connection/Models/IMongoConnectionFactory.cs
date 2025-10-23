using MongoDB.Driver;

namespace AMCode.Data.MongoDB
{
    /// <summary>
    /// Interface for MongoDB connection management and database access.
    /// Provides methods for creating MongoDB clients, databases, and collections.
    /// </summary>
    public interface IMongoConnectionFactory
    {
        /// <summary>
        /// Gets a MongoDB database instance by name.
        /// </summary>
        /// <param name="databaseName">The name of the database.</param>
        /// <returns>An IMongoDatabase instance.</returns>
        IMongoDatabase GetDatabase(string databaseName);

        /// <summary>
        /// Gets a MongoDB collection by database name and collection name.
        /// </summary>
        /// <typeparam name="T">The type of documents in the collection.</typeparam>
        /// <param name="databaseName">The name of the database.</param>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>An IMongoCollection instance.</returns>
        IMongoCollection<T> GetCollection<T>(string databaseName, string collectionName);

        /// <summary>
        /// Gets the MongoDB client instance.
        /// </summary>
        /// <returns>An IMongoClient instance.</returns>
        IMongoClient GetClient();
    }
}
