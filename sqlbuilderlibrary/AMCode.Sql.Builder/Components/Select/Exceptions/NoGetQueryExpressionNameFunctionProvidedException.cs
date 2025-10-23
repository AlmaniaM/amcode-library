using System;

namespace AMCode.Sql.Select.Extensions
{
    /// <summary>
    /// A class designed to represent an exception when a <see cref="GetQueryExpressionNameFunction"/> cannot be found.
    /// </summary>
    public class NoGetQueryExpressionNameFunctionProvidedException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="NoGetQueryExpressionNameFunctionProvidedException"/> class.
        /// </summary>
        public NoGetQueryExpressionNameFunctionProvidedException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoGetQueryExpressionNameFunctionProvidedException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public NoGetQueryExpressionNameFunctionProvidedException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoGetQueryExpressionNameFunctionProvidedException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="message">Add a string message.</param>
        public NoGetQueryExpressionNameFunctionProvidedException(string header, string message)
            : base($"{header} Error: You provided null where a {nameof(GetQueryExpressionNameFunction)} was expected.{message}")
        { }
    }
}