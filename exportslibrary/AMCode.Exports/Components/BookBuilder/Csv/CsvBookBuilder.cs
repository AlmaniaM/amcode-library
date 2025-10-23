using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.DataSource;
using AMCode.Storage;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// A class designed to build an CSV book.
    /// </summary>
    public class CsvBookBuilder : IBookBuilder<ICsvDataColumn>
    {
        private readonly BookBuilderCommon<ICsvDataColumn> builderCommon;
        private readonly IExportStreamDataSourceFactory dataSourceFactory;

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookBuilder"/> class.
        /// </summary>
        /// <param name="bookFactory">An <see cref="ICsvBookFactory"/> object.</param>
        /// <param name="builderConfig">An <see cref="BookBuilderConfig"/> object.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="BookBuilderConfig"/> or <see cref="BookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        public CsvBookBuilder(ICsvBookFactory bookFactory, IBookBuilderConfig builderConfig)
        {
            builderCommon = new BookBuilderCommon<ICsvDataColumn>(bookFactory, builderConfig);
            dataSourceFactory = new MemoryStreamDataSourceFactory();

            builderCommon.ValidateConstructorParameters<CsvBookBuilder, ICsvBookFactory, IBookBuilderConfig>(bookFactory, builderConfig);
        }

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookBuilder"/> class.
        /// </summary>
        /// <param name="bookFactory">An <see cref="ICsvBookFactory"/> object.</param>
        /// <param name="builderConfig">An <see cref="BookBuilderConfig"/> object.</param>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> for constructing an <see cref="IStreamDataSourceAsync"/> object.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="BookBuilderConfig"/> or <see cref="BookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        public CsvBookBuilder(ICsvBookFactory bookFactory, IBookBuilderConfig builderConfig, IExportStreamDataSourceFactory dataSourceFactory)
        {
            builderCommon = new BookBuilderCommon<ICsvDataColumn>(bookFactory, builderConfig);
            this.dataSourceFactory = dataSourceFactory;

            builderCommon.ValidateConstructorParameters<CsvBookBuilder, ICsvBookFactory, IBookBuilderConfig>(bookFactory, builderConfig);

            validateConstructorParameter(
                dataSourceFactory,
                () => ExceptionUtil.CreateConstructorExceptionHeader<CsvBookBuilder, ICsvBookFactory, IBookBuilderConfig, IExportStreamDataSourceFactory>()
            );
        }

        /// <summary>
        /// Build a CSV export.
        /// </summary>
        /// <returns>A CSV <see cref="Stream"/> <see cref="Task"/>.</returns>
        /// <inheritdoc/>
        public async Task<IStreamDataSourceAsync> BuildBookAsync(int startRow, int count, IEnumerable<ICsvDataColumn> columns, CancellationToken cancellationToken = default)
        {
            builderCommon.ValidateAddBookDataParameters(startRow, columns, ExceptionUtil.CreateExceptionHeader<int, int, IEnumerable<ICsvDataColumn>, CancellationToken, Task>(BuildBookAsync));

            var startingPoints = builderCommon.CalculateStartingRows(startRow, count);

            using (var csvBook = builderCommon.BookFactory.CreateBook())
            {
                csvBook.SetColumns(columns.Select(column => column.WorksheetHeaderName));

                cancellationToken.ThrowIfCancellationRequested();

                await builderCommon.AddBookData(csvBook, startingPoints, count, columns, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                return await dataSourceFactory.CreateAsync(csvBook.Save());
            }
        }

        /// <summary>
        /// Build a CSV export.
        /// </summary>
        /// <returns>A CSV <see cref="Stream"/> <see cref="Task"/>.</returns>
        /// <inheritdoc/>
        public Task<IStreamDataSourceAsync> BuildBookAsync(int startRow, int count, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken = default)
            => BuildBookAsync(startRow, count, (IEnumerable<ICsvDataColumn>)columns, cancellationToken);

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