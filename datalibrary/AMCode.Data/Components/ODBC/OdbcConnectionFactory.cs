using System.Data;
using System.Data.Odbc;

namespace AMCode.Data.Odbc
{
    /// <summary>
    /// A class designed to create an <see cref="OdbcConnection"/> as an <see cref="IDbConnection"/>.
    /// </summary>
    public class OdbcConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString;

        /// <summary>
        /// Creates an instance of a <see cref="OdbcConnectionFactory"/>.
        /// </summary>
        /// <param name="connectionString">A database connection string.</param>
        public OdbcConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Create an <see cref="OdbcConnection"/>.
        /// </summary>
        /// <returns>A <see cref="OdbcConnection"/> instance.</returns>
        public IDbConnection Create() => new OdbcConnection(connectionString);

        /// <summary>
        /// Create an <see cref="OdbcConnection"/> from a given connection string.
        /// </summary>
        /// <param name="connectionString">A connection string to create a <see cref="OdbcConnection"/> from.</param>
        /// <returns>A <see cref="OdbcConnection"/> instance.</returns>
        public IDbConnection Create(string connectionString) => new OdbcConnection(connectionString);
    }
}