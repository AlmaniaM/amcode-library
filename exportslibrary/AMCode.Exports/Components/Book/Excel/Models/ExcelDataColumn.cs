using System;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A class designed to provide information about a column for setting cell data.
    /// </summary>
    public class ExcelDataColumn : IExcelDataColumn
    {
        /// <inheritdoc/>
        public Type DataType { get; set; } = typeof(string);

        /// <inheritdoc/>
        public string DataFieldName { get; set; }

        /// <inheritdoc/>
        public string WorksheetHeaderName { get; set; }
    }
}