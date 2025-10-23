using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports.Book;
using AMCode.Exports.BookBuilder;
using AMCode.Exports.BookBuilder.Actions;
using AMCode.Exports.DataSource;
using AMCode.Exports.Results;
using AMCode.Storage;
using AMCode.Xlsx;

namespace AMCode.Exports.ExportBuilder
{
    /// <summary>
    /// A class designed to build simple file exports.
    /// </summary>
    public class ExcelExportBuilder : IExportBuilder<IExcelDataColumn>
    {
        private readonly IBookCompiler bookCompiler;

        /// <summary>
        /// Create an instance of the <see cref="CsvExportBuilder"/> class.
        /// </summary>
        /// <param name="builderConfig">Provide a <see cref="IExcelBookBuilderConfig"/> object to configure the export.</param>
        public ExcelExportBuilder(IExcelBookBuilderConfig builderConfig) : this(builderConfig, ExcelLimitValues.MaxRowCount) { }

        /// <inheritdoc cref="ExcelExportBuilder(IExcelBookBuilderConfig)"/>
        /// <param name="builderConfig">Provide a <see cref="IExcelBookBuilderConfig"/> object to configure the export.</param>
        /// <param name="exportResultFactory">Provide an <see cref="IExportResultFactory"/> for creating the <see cref="IExportResult"/> type.
        /// Set this to <c>null</c> if you want the default.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> object for constructing <see cref="IStreamDataSourceAsync"/> objects.
        /// Set this to <c>null</c> if you want the default.</param>
        public ExcelExportBuilder(IExcelBookBuilderConfig builderConfig, IExportResultFactory exportResultFactory, IExportStreamDataSourceFactory dataSourceFactory)
            : this(builderConfig, ExcelLimitValues.MaxRowCount, exportResultFactory ?? new DataSourceExportResultFactory(), dataSourceFactory ?? new MemoryStreamDataSourceFactory()) { }

        /// <inheritdoc cref="ExcelExportBuilder(IExcelBookBuilderConfig)"/>
        /// <param name="builderConfig">Provide a <see cref="IExcelBookBuilderConfig"/> object to configure the export.</param>
        /// <param name="maxRowsPerFile">The maximum number of rows a file can hold.</param>
        public ExcelExportBuilder(IExcelBookBuilderConfig builderConfig, int maxRowsPerFile)
            : this(builderConfig, getDefaultStylers(), maxRowsPerFile) { }

        /// <inheritdoc cref="ExcelExportBuilder(IExcelBookBuilderConfig)"/>
        /// <param name="builderConfig">Provide a <see cref="IExcelBookBuilderConfig"/> object to configure the export.</param>
        /// <param name="maxRowsPerFile">The maximum number of rows a file can hold.</param>
        /// <param name="exportResultFactory">Provide an <see cref="IExportResultFactory"/> for creating the <see cref="IExportResult"/> type.
        /// Set this to <c>null</c> if you want the default.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> object for constructing <see cref="IStreamDataSourceAsync"/> objects.
        /// Set this to <c>null</c> if you want the default.</param>
        public ExcelExportBuilder(IExcelBookBuilderConfig builderConfig, int maxRowsPerFile, IExportResultFactory exportResultFactory, IExportStreamDataSourceFactory dataSourceFactory)
            : this(builderConfig, getDefaultStylers(), maxRowsPerFile, exportResultFactory ?? new DataSourceExportResultFactory(), dataSourceFactory ?? new MemoryStreamDataSourceFactory()) { }

        /// <inheritdoc cref="ExcelExportBuilder(IExcelBookBuilderConfig)"/>
        /// <param name="builderConfig">Provide a <see cref="IExcelBookBuilderConfig"/> object to configure the export.</param>
        /// <param name="stylers">Provide an <see cref="IList{T}"/> collection of <see cref="IExcelBookStyleAction"/>s for applying specific style options.</param>
        /// <param name="maxRowsPerFile">The maximum number of rows a file can hold.</param>
        public ExcelExportBuilder(IExcelBookBuilderConfig builderConfig, IList<IExcelBookStyleAction> stylers, int maxRowsPerFile)
        {
            var bookBuilderFactory = new BookBuilderFactory(builderConfig, maxRowsPerFile, new SimpleColumnBasedStyler(stylers));
            bookCompiler = new BookCompiler(bookBuilderFactory, maxRowsPerFile);
        }

        /// <inheritdoc cref="ExcelExportBuilder(IExcelBookBuilderConfig)"/>
        /// <param name="builderConfig">Provide a <see cref="IExcelBookBuilderConfig"/> object to configure the export.</param>
        /// <param name="stylers">Provide an <see cref="IList{T}"/> collection of <see cref="IExcelBookStyleAction"/>s for applying specific style options.</param>
        /// <param name="maxRowsPerFile">The maximum number of rows a file can hold.</param>
        /// <param name="exportResultFactory">Provide an <see cref="IExportResultFactory"/> for creating the <see cref="IExportResult"/> type.
        /// Set this to <c>null</c> if you want the default.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> object for constructing <see cref="IStreamDataSourceAsync"/> objects.
        /// Set this to <c>null</c> if you want the default.</param>
        public ExcelExportBuilder(IExcelBookBuilderConfig builderConfig, IList<IExcelBookStyleAction> stylers, int maxRowsPerFile, IExportResultFactory exportResultFactory, IExportStreamDataSourceFactory dataSourceFactory)
            : this(builderConfig, new SimpleColumnBasedStyler(stylers), maxRowsPerFile, exportResultFactory ?? new DataSourceExportResultFactory(), dataSourceFactory ?? new MemoryStreamDataSourceFactory()) { }

        /// <inheritdoc cref="ExcelExportBuilder(IExcelBookBuilderConfig)"/>
        /// <param name="builderConfig">Provide a <see cref="IExcelBookBuilderConfig"/> object to configure the export.</param>
        /// <param name="bookStyler">Provide an <see cref="IExcelBookStyler"/> for styling books.</param>
        /// <param name="maxRowsPerFile">The maximum number of rows a file can hold.</param>
        /// <param name="exportResultFactory">Provide an <see cref="IExportResultFactory"/> for creating the <see cref="IExportResult"/> type.
        /// Set this to <c>null</c> if you want the default.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> object for constructing <see cref="IStreamDataSourceAsync"/> objects.</param>
        public ExcelExportBuilder(IExcelBookBuilderConfig builderConfig, IExcelBookStyler bookStyler, int maxRowsPerFile, IExportResultFactory exportResultFactory, IExportStreamDataSourceFactory dataSourceFactory)
        {
            var bookBuilderFactory = new BookBuilderFactory(builderConfig, maxRowsPerFile, bookStyler, dataSourceFactory ?? new MemoryStreamDataSourceFactory());
            bookCompiler = new BookCompiler(bookBuilderFactory, maxRowsPerFile, exportResultFactory ?? new DataSourceExportResultFactory());
        }

        /// <inheritdoc/>
        public int CalculateNumberOfBooks(int totalRowCount) => bookCompiler.CalculateNumberOfBooks(totalRowCount);

        /// <summary>
        /// Create an Xlsx file export.
        /// </summary>
        /// <param name="fileName">The name to give the file.</param>
        /// <param name="totalRowCount">The total number of records to create.</param>
        /// <param name="excelColumns">Provide an <see cref="IEnumerable{T}"/> collection of <see cref="IExcelDataColumn"/>s for accessing column data
        /// and determining column data <see cref="Type"/>s.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IExportResult"/> object. If <paramref name="totalRowCount"/> is greater than the max allowed file rows
        /// then the <see cref="IExportResult.GetDataAsync"/> <see cref="Stream"/> will be a zip file. Otherwise, it'll be an <see cref="FileType.Xlsx"/> file.</returns>
        public async Task<IExportResult> CreateExportAsync(string fileName, int totalRowCount, IEnumerable<IExcelDataColumn> excelColumns, CancellationToken cancellationToken)
            => await bookCompiler.CompileExcelAsync(fileName, totalRowCount, excelColumns, cancellationToken);

        /// <summary>
        /// Create a default collection of <see cref="IExcelBookStyleAction"/>s.
        /// </summary>
        /// <returns>An <see cref="IList{T}"/> collection of default <see cref="IExcelBookStyleAction"/>s.</returns>
        private static IList<IExcelBookStyleAction> getDefaultStylers()
            => new List<IExcelBookStyleAction>
            {
                new ApplyColumnStylesAction(),
                new ApplyColumnWidthAction(),
                new ApplyBoldHeadersAction()
            };
    }
}