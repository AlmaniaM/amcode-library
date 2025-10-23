using System.Data;
using Vertica.Data.VerticaClient;

namespace AMCode.Data.Vertica
{
    /// <summary>
    /// A class designed to create a <see cref="VerticaConnection"/> as an <see cref="IDbConnection"/>.
    /// </summary>
    public class VerticaConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        /// <summary>
        /// Creates an instance of a <see cref="VerticaConnectionFactory"/>.
        /// </summary>
        /// <param name="connectionString">A database connection string.</param>
        public VerticaConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Create a <see cref="VerticaConnection"/>.
        /// </summary>
        /// <returns>A <see cref="VerticaConnection"/> instance.</returns>
        public IDbConnection Create() => new VerticaConnection(connectionString);

        /// <summary>
        /// Create a <see cref="VerticaConnection"/> from a given connection string.
        /// </summary>
        /// <param name="connectionString">A connection string to create a <see cref="VerticaConnection"/> from.</param>
        /// <returns>A <see cref="VerticaConnection"/> instance.</returns>
        public IDbConnection Create(string connectionString) => new VerticaConnection(connectionString);
    }
}