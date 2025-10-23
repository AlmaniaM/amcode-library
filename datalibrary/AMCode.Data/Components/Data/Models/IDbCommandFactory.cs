using System.Data;

namespace AMCode.Data
{
    /// <summary>
    /// An interface that represents a factory for creating the <see cref="IDbCommand"/> object.
    /// </summary>
    public interface IDbCommandFactory
    {
        /// <summary>
        /// Create an <see cref="IDbCommand"/> object.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> to create the <see cref="IDbCommand"/> from.</param>
        /// <returns>A <see cref="IDbCommand"/> object.</returns>
        IDbCommand Create(IDbConnection connection);

        /// <summary>
        /// Create an <see cref="IDbCommand"/> object.
        /// </summary>
        /// <param name="connection">The <see cref="IDbConnection"/> to creat the command with.</param>
        /// <param name="commandString">The <see cref="string"/> command to assign to the <see cref="IDbCommand"/>.</param>
        /// <returns>A <see cref="IDbCommand"/> object.</returns>
        IDbCommand Create(IDbConnection connection, string commandString);
    }
}