using System;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when a <see cref="IWhereClauseBuilder"/> cannot be found.
    /// </summary>
    public class NoSuchWhereClauseBuilderException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="NoSuchWhereClauseBuilderException"/> class.
        /// </summary>
        public NoSuchWhereClauseBuilderException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchWhereClauseBuilderException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public NoSuchWhereClauseBuilderException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchWhereClauseBuilderException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="value">The value used to search for the where clause builder.</param>
        public NoSuchWhereClauseBuilderException(string header, string value)
            : base($"{header} Error: Cannot find {nameof(IWhereClauseBuilder)} for {nameof(WhereClauseBuilderType)} of value {value}")
        { }
    }
}