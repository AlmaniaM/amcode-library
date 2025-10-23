using AMCode.Xlsx;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A class designed for setting column styles.
    /// </summary>
    public class ColumnStyleParam : StyleParam, IColumnStyleParam
    {
        /// <inheritdoc/>
        public int? ColumnIndex { get; set; }

        /// <inheritdoc/>
        public string ColumnName { get; set; }
    }
}