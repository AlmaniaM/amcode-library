using System.Data;

namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a factory for creating the <see cref="IDbConnection"/> object.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Create an <see cref="IDbConnection"/>.
        /// </summary>
        /// <returns>A <see cref="IDbConnection"/> instance.</returns>
        IDbConnection Create();

        /// <summary>
        /// Create an <see cref="IDbConnection"/> from a given connection string.
        /// </summary>
        /// <param name="connectionString">A connection string to create a <see cref="IDbConnection"/> from.</param>
        /// <returns>A <see cref="IDbConnection"/> instance.</returns>
        IDbConnection Create(string connectionString);
    }
}