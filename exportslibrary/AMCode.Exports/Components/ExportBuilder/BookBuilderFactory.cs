using System.Collections.Generic;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.DataSource;
using AMCode.Exports.Storage;

namespace AMCode.Exports.ExportBuilder
{
    /// <summary>
    /// A factory class designed to build <see cref="IBookBuilder{TColumn}"/>s.
    /// </summary>
    public class BookBuilderFactory : IBookBuilderFactory
    {
        private readonly IBookBuilderConfig bookBuilderConfig;
        private readonly IExcelBookStyler excelBookStyler;
        private readonly IExportStreamDataSourceFactory dataSourceFactory;
        private readonly int maxRowsPerBook;

        /// <summary>
        /// Create an instance of the <see cref="BookBuilderFactory"/> class.
        /// </summary>
        /// <param name="bookBuilderConfig">A <see cref="IBookBuilderConfig"/> object.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per book.</param>
        public BookBuilderFactory(IBookBuilderConfig bookBuilderConfig, int maxRowsPerBook)
            : this(bookBuilderConfig, maxRowsPerBook, new MemoryStreamDataSourceFactory()) { }

        /// <summary>
        /// Create an instance of the <see cref="BookBuilderFactory"/> class.
        /// </summary>
        /// <param name="bookBuilderConfig">A <see cref="IBookBuilderConfig"/> object.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per book.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> object for constructing <see cref="IStreamDataSourceAsync"/> objects.</param>
        public BookBuilderFactory(IBookBuilderConfig bookBuilderConfig, int maxRowsPerBook, IExportStreamDataSourceFactory dataSourceFactory)
        {
            this.bookBuilderConfig = bookBuilderConfig;
            this.maxRowsPerBook = maxRowsPerBook;
            this.dataSourceFactory = dataSourceFactory;
            excelBookStyler = new SimpleColumnBasedStyler(new List<IExcelBookStyleAction>());
        }

        /// <summary>
        /// Create an instance of the <see cref="BookBuilderFactory"/> class.
        /// </summary>
        /// <param name="bookBuilderConfig">A <see cref="IBookBuilderConfig"/> object.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per book.</param>
        /// <param name="excelBookStyler">Provide an <see cref="IExcelBookStyler"/> for styling Excel exports.</param>
        public BookBuilderFactory(IExcelBookBuilderConfig bookBuilderConfig, int maxRowsPerBook, IExcelBookStyler excelBookStyler)
            : this(bookBuilderConfig, maxRowsPerBook, excelBookStyler, new MemoryStreamDataSourceFactory()) { }

        /// <summary>
        /// Create an instance of the <see cref="BookBuilderFactory"/> class.
        /// </summary>
        /// <param name="bookBuilderConfig">A <see cref="IBookBuilderConfig"/> object.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per book.</param>
        /// <param name="excelBookStyler">Provide an <see cref="IExcelBookStyler"/> for styling Excel exports.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> object for constructing <see cref="IStreamDataSourceAsync"/> objects.</param>
        public BookBuilderFactory(IExcelBookBuilderConfig bookBuilderConfig, int maxRowsPerBook, IExcelBookStyler excelBookStyler, IExportStreamDataSourceFactory dataSourceFactory)
        {
            this.bookBuilderConfig = bookBuilderConfig;
            this.maxRowsPerBook = maxRowsPerBook;
            this.excelBookStyler = excelBookStyler;
            this.dataSourceFactory = dataSourceFactory;
        }

        /// <summary>
        /// Create an <see cref="IBookBuilder"/> based on the provided <see cref="FileType"/> value.
        /// </summary>
        /// <inheritdoc/>
        public IBookBuilder CreateBuilder(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Csv:
                    return new CsvBookBuilder(new CsvBookFactory(), bookBuilderConfig, dataSourceFactory);
                case FileType.Xlsx:
                    var excelBookFactory = new ExcelBookFactory(maxRowsPerBook);

                    if (bookBuilderConfig is IExcelBookBuilderConfig excelConfig)
                    {
                        return new ExcelBookBuilder(excelBookFactory, excelConfig, excelBookStyler, dataSourceFactory);
                    }

                    return new ExcelBookBuilder(
                        excelBookFactory,
                        new ExcelBookBuilderConfig
                        {
                            FetchDataAsync = bookBuilderConfig.FetchDataAsync,
                            MaxRowsPerDataFetch = bookBuilderConfig.MaxRowsPerDataFetch
                        },
                        excelBookStyler,
                        dataSourceFactory
                    );
                default:
                    return default;
            }
        }
    }
}
