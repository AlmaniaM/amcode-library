namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed to provide information about a CSV column for setting cell data.
    /// </summary>
    public interface ICsvDataColumn : IBookDataColumn
    {
        /// <summary>
        /// Format an object value into a string.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A <see cref="string"/> value.</returns>
        string FormatValue(object value);
    }
}