namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// A class designed to configure a <see cref="ExcelBookBuilder"/> instance.
    /// </summary>
    public class BookBuilderConfig : IBookBuilderConfig
    {
        /// <inheritdoc/>
        public ExportDataRangeFetch FetchDataAsync { get; set; }

        /// <inheritdoc/>
        public int MaxRowsPerDataFetch { get; set; }
    }
}