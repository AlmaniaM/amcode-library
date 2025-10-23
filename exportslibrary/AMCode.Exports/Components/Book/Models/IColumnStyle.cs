using AMCode.Xlsx;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed to style a column.
    /// </summary>
    public interface IColumnStyle
    {
        /// <summary>
        /// The column name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The style to apply.
        /// </summary>
        IStyleParam Style { get; set; }

        /// <summary>
        /// The width of the column.
        /// </summary>
        double? Width { get; set; }
    }
}