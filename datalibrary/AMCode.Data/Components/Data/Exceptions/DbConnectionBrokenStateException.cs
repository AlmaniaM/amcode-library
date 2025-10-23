using System;
using System.Data;

namespace AMCode.Data.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when a <see cref="IDbConnection"/> is provided who's
    /// <see cref="IDbCommand.Connection"/> is <see cref="ConnectionState.Broken"/>.
    /// </summary>
    public class DbConnectionBrokenStateException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="DbConnectionBrokenStateException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="message">The message to set.</param>
        public DbConnectionBrokenStateException(string header, string message)
            : base($"{header} Error: The provided connection state is broken. {message}")
        { }
    }
}