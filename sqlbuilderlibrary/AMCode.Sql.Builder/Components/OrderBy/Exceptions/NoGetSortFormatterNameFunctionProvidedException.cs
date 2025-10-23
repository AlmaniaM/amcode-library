using System;
using AMCode.Sql.OrderBy.Models;

namespace AMCode.Sql.OrderBy.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when a <see cref="GetSortFormatterNameFunction"/> can be found.
    /// </summary>
    public class NoGetSortFormatterNameFunctionProvidedException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="NoGetSortFormatterNameFunctionProvidedException"/> class.
        /// </summary>
        public NoGetSortFormatterNameFunctionProvidedException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoGetSortFormatterNameFunctionProvidedException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public NoGetSortFormatterNameFunctionProvidedException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoGetSortFormatterNameFunctionProvidedException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="message">Add a string message.</param>
        public NoGetSortFormatterNameFunctionProvidedException(string header, string message)
            : base($"{header} Error: You provided null where a {nameof(GetSortFormatterNameFunction)} was expected.{message}")
        { }
    }
}