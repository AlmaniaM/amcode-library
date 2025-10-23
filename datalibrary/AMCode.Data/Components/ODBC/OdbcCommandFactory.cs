using System.Data;
using System.Data.Odbc;

namespace AMCode.Data.Odbc
{
    /// <summary>
    /// A factory class designed to create an <see cref="OdbcCommand"/> as an <see cref="IDbCommand"/>.
    /// </summary>
    public class OdbcCommandFactory : IDbCommandFactory
    {
        /// <summary>
        /// Create an <see cref="OdbcCommand"/> object.
        /// </summary>
        /// <param name="connection">The <see cref="OdbcConnection"/> to create the <see cref="OdbcCommand"/> from.</param>
        /// <returns>A <see cref="OdbcCommand"/> object.</returns>
        public IDbCommand Create(IDbConnection connection) => connection.CreateCommand();

        /// <summary>
        /// Create an <see cref="OdbcCommand"/> object.
        /// </summary>
        /// <param name="connection">The <see cref="OdbcConnection"/> to create the command with.</param>
        /// <param name="commandString">The <see cref="string"/> command to assign to the <see cref="OdbcCommand"/>.</param>
        /// <returns>A <see cref="OdbcCommand"/> object.</returns>
        public IDbCommand Create(IDbConnection connection, string commandString) => new OdbcCommand(commandString, (OdbcConnection)connection);
    }
}