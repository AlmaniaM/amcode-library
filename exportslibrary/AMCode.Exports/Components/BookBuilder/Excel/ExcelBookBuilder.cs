using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.DataSource;
using AMCode.Exports.Storage;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// A class designed to build an Excel book.
    /// </summary>
    public class ExcelBookBuilder : IBookBuilder<IExcelDataColumn>
    {
        private readonly BookBuilderCommon<IExcelDataColumn> builderCommon;
        private readonly IExcelBookBuilderConfig excelBuilderConfig;
        private readonly IExcelBookStyler excelBookStyler;
        private readonly IExportStreamDataSourceFactory dataSourceFactory;
        private readonly IList<string> generatedFilePaths;

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookBuilder"/> class.
        /// </summary>
        /// <param name="bookFactory">An <see cref="IBookFactory{TColumn}"/> of type <see cref="IExcelDataColumn"/> object.</param>
        /// <param name="excelBuilderConfig">An <see cref="IExcelBookBuilderConfig"/> object.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="IExcelBookBuilderConfig"/> or <see cref="BookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        public ExcelBookBuilder(IExcelBookFactory bookFactory, IExcelBookBuilderConfig excelBuilderConfig)
        {
            builderCommon = new BookBuilderCommon<IExcelDataColumn>(bookFactory, excelBuilderConfig);
            builderCommon.ValidateConstructorParameters<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig>(bookFactory, excelBuilderConfig);

            dataSourceFactory = new MemoryStreamDataSourceFactory();
            this.excelBuilderConfig = excelBuilderConfig;
        }

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookBuilder"/> class.
        /// </summary>
        /// <param name="bookFactory">An <see cref="IBookFactory{TColumn}"/> of type <see cref="IExcelDataColumn"/> object.</param>
        /// <param name="excelBuilderConfig">An <see cref="IExcelBookBuilderConfig"/> object.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> for constructing an <see cref="IStreamDataSourceAsync"/> object.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="IExcelBookBuilderConfig"/>, <see cref="BookBuilderConfig.FetchDataAsync"/>, or
        /// <see cref="IExportStreamDataSourceFactory"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        public ExcelBookBuilder(IExcelBookFactory bookFactory, IExcelBookBuilderConfig excelBuilderConfig, IExportStreamDataSourceFactory dataSourceFactory)
        {
            builderCommon = new BookBuilderCommon<IExcelDataColumn>(bookFactory, excelBuilderConfig);
            builderCommon.ValidateConstructorParameters<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExportStreamDataSourceFactory>(bookFactory, excelBuilderConfig);

            validateConstructorParameter(
                dataSourceFactory,
                () => ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExportStreamDataSourceFactory>()
            );

            this.excelBuilderConfig = excelBuilderConfig;
            this.dataSourceFactory = dataSourceFactory;
            generatedFilePaths = new List<string>();
        }

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookBuilder"/> class.
        /// </summary>
        /// <param name="bookFactory">An <see cref="IExcelBookFactory"/> object.</param>
        /// <param name="excelBuilderConfig">An <see cref="IExcelBookBuilderConfig"/> object.</param>
        /// <param name="excelBookStyler">An <see cref="IExcelBookStyler"/> object for applying styles to an <see cref="IExcelBook"/> object.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="BookBuilderConfig"/> or <see cref="BookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        public ExcelBookBuilder(IExcelBookFactory bookFactory, IExcelBookBuilderConfig excelBuilderConfig, IExcelBookStyler excelBookStyler)
        {
            builderCommon = new BookBuilderCommon<IExcelDataColumn>(bookFactory, excelBuilderConfig);
            builderCommon.ValidateConstructorParameters<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExcelBookStyler>(bookFactory, excelBuilderConfig);

            dataSourceFactory = new MemoryStreamDataSourceFactory();

            validateConstructorParameter(
                excelBookStyler,
                dataSourceFactory,
                () => ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExcelBookStyler>()
            );

            this.excelBuilderConfig = excelBuilderConfig;
            this.excelBookStyler = excelBookStyler;
        }

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookBuilder"/> class.
        /// </summary>
        /// <param name="bookFactory">An <see cref="IExcelBookFactory"/> object.</param>
        /// <param name="excelBuilderConfig">An <see cref="IExcelBookBuilderConfig"/> object.</param>
        /// <param name="excelBookStyler">An <see cref="IExcelBookStyler"/> object for applying styles to an <see cref="IExcelBook"/> object.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> for constructing an <see cref="IStreamDataSourceAsync"/> object.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="BookBuilderConfig"/>, <see cref="BookBuilderConfig.FetchDataAsync"/>, or
        /// <see cref="IExportStreamDataSourceFactory"/> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        public ExcelBookBuilder(IExcelBookFactory bookFactory, IExcelBookBuilderConfig excelBuilderConfig, IExcelBookStyler excelBookStyler, IExportStreamDataSourceFactory dataSourceFactory)
        {
            builderCommon = new BookBuilderCommon<IExcelDataColumn>(bookFactory, excelBuilderConfig);
            builderCommon.ValidateConstructorParameters<
                ExcelBookBuilder,
                IExcelBookFactory,
                IExcelBookBuilderConfig,
                IExcelBookStyler,
                IExportStreamDataSourceFactory>(bookFactory, excelBuilderConfig);

            validateConstructorParameter(
                excelBookStyler,
                dataSourceFactory,
                () => ExceptionUtil.CreateConstructorExceptionHeader<ExcelBookBuilder, IExcelBookFactory, IExcelBookBuilderConfig, IExcelBookStyler, IExportStreamDataSourceFactory>()
            );

            this.excelBuilderConfig = excelBuilderConfig;
            this.excelBookStyler = excelBookStyler;
            this.dataSourceFactory = dataSourceFactory;
            generatedFilePaths = new List<string>();
        }

        /// <summary>
        /// Build an Excel export.
        /// </summary>
        /// <returns>An Excel <see cref="Stream"/> <see cref="Task"/>.</returns>
        /// <inheritdoc/>
        public async Task<IStreamDataSourceAsync> BuildBookAsync(int startRow, int count, IEnumerable<IExcelDataColumn> columns, CancellationToken cancellationToken = default)
        {
            builderCommon.ValidateAddBookDataParameters(startRow, columns, ExceptionUtil.CreateExceptionHeader<int, int, IEnumerable<IExcelDataColumn>, CancellationToken, Task>(BuildBookAsync));

            var startingPoints = builderCommon.CalculateStartingRows(startRow, count);

            using (var excelBook = builderCommon.BookFactory.CreateBook())
            {
                excelBook.SetColumns(columns.Select(column => column.WorksheetHeaderName));

                cancellationToken.ThrowIfCancellationRequested();

                await builderCommon.AddBookData(excelBook, startingPoints, count, columns, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                if (excelBookStyler != null && excelBuilderConfig.ColumnStyles != null)
                {
                    excelBookStyler.ApplyStyles(
                        (IExcelBook)excelBook,
                        new ColumnStyleActionData
                        {
                            ColumnCount = columns.Count(),
                            ColumnStyles = excelBuilderConfig.ColumnStyles,
                        }
                    );
                }

                cancellationToken.ThrowIfCancellationRequested();

                return await dataSourceFactory.CreateAsync(excelBook.Save());
            }
        }

        /// <summary>
        /// Build an Excel export.
        /// </summary>
        /// <returns>An Excel <see cref="Stream"/> <see cref="Task"/>.</returns>
        /// <inheritdoc/>
        public Task<IStreamDataSourceAsync> BuildBookAsync(int startRow, int count, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken = default)
            => BuildBookAsync(startRow, count, (IEnumerable<IExcelDataColumn>)columns, cancellationToken);

        /// <inheritdoc/>
        public void Dispose() => generatedFilePaths?.ForEach(filePath => File.Delete(filePath));

        /// <summary>
        /// Validates the <see cref="IBookBuilderConfig"/> parameter.
        /// </summary>
        /// <param name="excelBookStyler">An <see cref="IExcelBookStyler"/> object.</param>
        /// <param name="dataSourceFactory">An <see cref="IExportStreamDataSourceFactory"/> object.</param>
        /// <param name="getHeader">A <see cref="Func{TResult}"/> that provides the constructor exception header.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="IExcelBookFactory"/>, <see cref="IBookBuilderConfig"/>,
        /// <see cref="IBookBuilderConfig.FetchDataAsync"/>, or <see cref="IExcelBookStyler"/> are <c>null</c>
        /// or when the <see cref="IExportStreamDataSourceFactory"/> is <c>null</c>.</exception>
        private void validateConstructorParameter(IExcelBookStyler excelBookStyler, IExportStreamDataSourceFactory dataSourceFactory, Func<string> getHeader)
        {
            if (excelBookStyler is null)
            {
                throw new NullReferenceException($"{getHeader()} Error: Parameter for \"{nameof(IExcelBookStyler)}\" cannot be null.");
            }

            validateConstructorParameter(dataSourceFactory, getHeader);
        }

        /// <summary>
        /// Validates the <see cref="IBookBuilderConfig"/> parameter.
        /// </summary>
        /// <param name="dataSourceFactory">An <see cref="IExportStreamDataSourceFactory"/> object.</param>
        /// <param name="getHeader">A <see cref="Func{TResult}"/> that provides the constructor exception header.</param>
        /// <exception cref="NullReferenceException">Thrown when the <see cref="IExportStreamDataSourceFactory"/> is null.</exception>
        private void validateConstructorParameter(IExportStreamDataSourceFactory dataSourceFactory, Func<string> getHeader)
        {
            if (dataSourceFactory is null)
            {
                throw new NullReferenceException($"{getHeader()} Error: Parameter for \"{nameof(IExportStreamDataSourceFactory)}\" cannot be null.");
            }
        }
    }
}
