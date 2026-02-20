using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.DataSource;
using AMCode.Exports.Results;
using AMCode.Exports.Storage;

namespace AMCode.Exports.ExportBuilder
{
    /// <summary>
    /// A class designed to build simple file exports.
    /// </summary>
    public class CsvExportBuilder : IExportBuilder<ICsvDataColumn>
    {
        private readonly IBookCompiler bookCompiler;

        /// <summary>
        /// Create an instance of the <see cref="CsvExportBuilder"/> class. This constructor will result in storing the book <see cref="Stream"/> data in memory.
        /// </summary>
        /// <param name="builderConfig">Provide a <see cref="IBookBuilderConfig"/> object to configure the export.</param>
        public CsvExportBuilder(IBookBuilderConfig builderConfig)
        {
            var bookBuilderFactory = new BookBuilderFactory(builderConfig, int.MaxValue);
            bookCompiler = new BookCompiler(bookBuilderFactory, int.MaxValue);
        }

        /// <summary>
        /// Create an instance of the <see cref="CsvExportBuilder"/> class.
        /// </summary>
        /// <param name="builderConfig">Provide a <see cref="IBookBuilderConfig"/> object to configure the export.</param>
        /// <param name="exportResultFactory">Provide an <see cref="IExportResultFactory"/> for creating the <see cref="IExportResult"/> type.
        /// Set this to <c>null</c> if you want the default.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> object for constructing <see cref="IStreamDataSourceAsync"/> objects.
        /// Set this to <c>null</c> if you want the default.</param>
        public CsvExportBuilder(IBookBuilderConfig builderConfig, IExportResultFactory exportResultFactory, IExportStreamDataSourceFactory dataSourceFactory)
        {
            var bookBuilderFactory = new BookBuilderFactory(builderConfig, int.MaxValue, dataSourceFactory ?? new MemoryStreamDataSourceFactory());
            bookCompiler = new BookCompiler(bookBuilderFactory, int.MaxValue, exportResultFactory ?? new DataSourceExportResultFactory());
        }

        /// <inheritdoc/>
        public int CalculateNumberOfBooks(int totalRowCount) => bookCompiler.CalculateNumberOfBooks(totalRowCount);

        /// <summary>
        /// Create an CSV file export.
        /// </summary>
        /// <param name="fileName">The name to give the file.</param>
        /// <param name="totalRowCount">The total number of records to create.</param>
        /// <param name="csvColumns">Provide an <see cref="IEnumerable{T}"/> collection of <see cref="ICsvDataColumn"/>s for accessing column data.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IExportResult"/> object. If <paramref name="totalRowCount"/> is greater than the max allowed file rows
        /// then the <see cref="IExportResult.GetDataAsync"/> <see cref="Stream"/> will be a zip file. Otherwise, it'll be an <see cref="FileType.Csv"/> file.</returns>
        public async Task<IExportResult> CreateExportAsync(string fileName, int totalRowCount, IEnumerable<ICsvDataColumn> csvColumns, CancellationToken cancellationToken)
            => await bookCompiler.CompileCsvAsync(fileName, totalRowCount, csvColumns, cancellationToken);
    }
}
