using System.Data;
using Vertica.Data.VerticaClient;

namespace AMCode.Data.Vertica
{
    /// <summary>
    /// A factory class designed to create a <see cref="VerticaCommand"/> as an <see cref="IDbCommand"/>.
    /// </summary>
    public class VerticaCommandFactory : IDbCommandFactory
    {
        /// <summary>
        /// Create a <see cref="VerticaCommand"/> object.
        /// </summary>
        /// <param name="connection">The <see cref="VerticaConnection"/> to create the <see cref="VerticaCommand"/> from.</param>
        /// <returns>A <see cref="VerticaCommand"/> object.</returns>
        public IDbCommand Create(IDbConnection connection) => connection.CreateCommand();

        /// <summary>
        /// Create a <see cref="VerticaCommand"/> object.
        /// </summary>
        /// <param name="connection">The <see cref="VerticaConnection"/> to create the command with.</param>
        /// <param name="commandString">The <see cref="string"/> command to assign to the <see cref="VerticaCommand"/>.</param>
        /// <returns>A <see cref="VerticaCommand"/> object.</returns>
        public IDbCommand Create(IDbConnection connection, string commandString) => new VerticaCommand(commandString, (VerticaConnection)connection);
    }
}