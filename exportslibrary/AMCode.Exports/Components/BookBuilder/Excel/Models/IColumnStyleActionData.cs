using System.Collections.Generic;
using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// An interface designed for applying column styles.
    /// </summary>
    public interface IColumnStyleActionData : IStyleActionData
    {
        /// <summary>
        /// A collection of <see cref="IColumnStyle"/>s.
        /// </summary>
        IEnumerable<IColumnStyle> ColumnStyles { get; set; }

        /// <summary>
        /// The number of columns in the sheet.
        /// </summary>
        int ColumnCount { get; set; }
    }
}