using System;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when a <see cref="IWhereClause"/> cannot be found.
    /// </summary>
    public class NoSuchWhereClauseException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="NoSuchWhereClauseException"/> class.
        /// </summary>
        public NoSuchWhereClauseException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchWhereClauseException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public NoSuchWhereClauseException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchWhereClauseException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="value">The value used to search for the <see cref="IWhereClause"/>.</param>
        public NoSuchWhereClauseException(string header, string value)
            : base($"{header} Error: Cannot find {nameof(IWhereClause)} for {nameof(WhereClauseBuilderType)} of value {value}")
        { }
    }
}