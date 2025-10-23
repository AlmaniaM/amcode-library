using System;

namespace AMCode.Common.IO.CSV.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when a delimiter is not provided.
    /// </summary>
    public class DelimiterNotProvidedException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="DelimiterNotProvidedException"/> class.
        /// </summary>
        public DelimiterNotProvidedException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="DelimiterNotProvidedException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public DelimiterNotProvidedException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="DelimiterNotProvidedException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="propertyName">The name of the delimiter parameter.</param>
        public DelimiterNotProvidedException(string header, string propertyName)
            : base($"{header} Error: The delimiter value for {propertyName} cannot be null or empty.")
        { }
    }
}