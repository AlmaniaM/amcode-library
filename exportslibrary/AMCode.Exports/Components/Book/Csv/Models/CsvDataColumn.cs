using AMCode.Columns.Core;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A class designed to provide information about a CSV column for setting cell data.
    /// </summary>
    public class CsvDataColumn : ICsvDataColumn
    {
        /// <inheritdoc/>
        public string DataFieldName { get; set; }

        /// <summary>
        /// Get the value formatter
        /// </summary>
        public IColumnValueFormatter<object, string> Formatter { get; set; }

        /// <inheritdoc/>
        public string WorksheetHeaderName { get; set; }

        /// <inheritdoc/>
        public string FormatValue(object value) => Formatter.FormatToObject(value).ToString();
    }
}