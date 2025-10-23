using System;

namespace AMCode.Columns.Core
{
    /// <summary>
    /// Represents a value formatter for column data.
    /// </summary>
    public interface IValueFormatter
    {
        /// <summary>
        /// Formats a value to a string representation.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted string representation of the value.</returns>
        string Format(object value);

        /// <summary>
        /// Formats a value to an object representation.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The formatted object representation of the value.</returns>
        object FormatToObject(object value);
    }
}
