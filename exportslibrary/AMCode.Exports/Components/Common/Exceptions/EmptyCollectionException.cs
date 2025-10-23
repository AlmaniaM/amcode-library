using System;
using AMCode.Exports.Common.Exceptions.Util;

namespace AMCode.Exports.Common.Exceptions
{
    /// <summary>
    /// A class designed to indicate that an empty collection has been encountered
    /// where a non-empty collection was expected.
    /// </summary>
    public class EmptyCollectionException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="EmptyCollectionException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="parameterName">The name of the collection parameter that was empty.</param>
        public EmptyCollectionException(string header, string parameterName)
            : base(ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(header, parameterName))
        { }
    }
}