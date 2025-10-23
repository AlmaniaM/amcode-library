using System;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Exceptions
{
    /// <summary>
    /// A class designed to represent an exception when a <see cref="FilterConditionSectionType"/> cannot be found.
    /// </summary>
    public class NoSuchFilterConditionSectionTypeException : Exception
    {
        /// <summary>
        /// Create an instance of the <see cref="NoSuchFilterConditionSectionTypeException"/> class.
        /// </summary>
        public NoSuchFilterConditionSectionTypeException()
            : base()
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchFilterConditionSectionTypeException"/> class.
        /// </summary>
        /// <param name="message">Add a string message.</param>
        public NoSuchFilterConditionSectionTypeException(string message)
            : base(message)
        { }

        /// <summary>
        /// Create an instance of the <see cref="NoSuchFilterConditionSectionTypeException"/> class.
        /// </summary>
        /// <param name="header">A string header representing the location of where the exception happened.</param>
        /// <param name="value">The value used to search for the <see cref="FilterConditionSectionType"/>.</param>
        public NoSuchFilterConditionSectionTypeException(string header, string value)
            : base($"{header} Error: {nameof(FilterConditionSectionType)} does not contain a value of {value}")
        { }
    }
}