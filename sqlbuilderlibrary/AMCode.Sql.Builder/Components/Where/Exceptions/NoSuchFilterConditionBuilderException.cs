using System;
using AMCode.Sql.Where.Internal;

namespace AMCode.Sql.Where.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when a <see cref="IFilterConditionOrganizer"/> cannot be found.
    /// </summary>
    public class NoSuchFilterConditionBuilderException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="NoSuchFilterConditionBuilderException"/> class.
        /// </summary>
        public NoSuchFilterConditionBuilderException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchFilterConditionBuilderException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public NoSuchFilterConditionBuilderException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchFilterConditionBuilderException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="value">The value used to search for the <see cref="IFilterConditionOrganizer"/>.</param>
        public NoSuchFilterConditionBuilderException(string header, string value)
            : base($"{header} Error: Cannot find {nameof(IFilterConditionOrganizer)} for type {value}")
        { }
    }
}