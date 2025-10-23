using System.Collections.Generic;
using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// An interface designed for applying column styles.
    /// </summary>
    public class ColumnStyleActionData : IColumnStyleActionData
    {
        /// <inheritdoc/>
        public IEnumerable<IColumnStyle> ColumnStyles { get; set; }

        /// <inheritdoc/>
        public int ColumnCount { get; set; }
    }
}