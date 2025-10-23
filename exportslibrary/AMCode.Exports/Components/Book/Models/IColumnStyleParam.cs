using AMCode.Xlsx;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed for setting column styles.
    /// </summary>
    public interface IColumnStyleParam : IStyleParam
    {
        /// <summary>
        /// The index of the column. One-based index.
        /// </summary>
        int? ColumnIndex { get; set; }

        /// <summary>
        /// The name of the column.
        /// </summary>
        string ColumnName { get; set; }
    }
}