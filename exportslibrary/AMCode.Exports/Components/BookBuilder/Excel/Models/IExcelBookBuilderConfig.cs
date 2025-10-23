using System.Collections.Generic;
using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// An interface designed to provide configuration data/functionality for an <see cref="IBookBuilder{TColumn}"/> object.
    /// </summary>
    public interface IExcelBookBuilderConfig : IBookBuilderConfig
    {
        /// <summary>
        /// An optional collection of <see cref="IColumnStyle"/>s to apply to export columns.
        /// </summary>
        IList<IColumnStyle> ColumnStyles { get; set; }
    }
}