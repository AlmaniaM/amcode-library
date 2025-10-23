using System.Collections.Generic;
using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// A class designed to configure a <see cref="ExcelBookBuilder"/> instance.
    /// </summary>
    public class ExcelBookBuilderConfig : BookBuilderConfig, IExcelBookBuilderConfig
    {
        /// <inheritdoc/>
        public IList<IColumnStyle> ColumnStyles { get; set; }
    }
}