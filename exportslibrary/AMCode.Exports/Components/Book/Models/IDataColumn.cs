namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed to provide information about a column for setting cell data.
    /// </summary>
    public interface IBookDataColumn
    {
        /// <summary>
        /// The name of the field for accessing data from the object.
        /// </summary>
        string DataFieldName { get; set; }

        /// <summary>
        /// The column header name to give the worksheet. If no value is provided then <see cref="DataFieldName"/> will be used.
        /// </summary>
        string WorksheetHeaderName { get; set; }
    }
}