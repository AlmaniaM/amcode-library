using System;

namespace AMCode.Data.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when insufficient parameters are passed
    /// to a factory function which creates the <see cref="IDataProvider"/> object.
    /// </summary>
    public class DefaultFactoryMethodParametersMissingException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="DefaultFactoryMethodParametersMissingException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="message">The message to set.</param>
        public DefaultFactoryMethodParametersMissingException(string header, string message)
            : base($"{header} Error: Insufficient parameters for instantiating the object. {message}")
        { }
    }
}