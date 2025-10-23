using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// An <c>abstract class</c> designed to contain common functionality for an <see cref="IBook{TColumn}"/> object.
    /// </summary>
    /// <typeparam name="TColumn">The type of <see cref="IBookDataColumn"/> you are building</typeparam>
    public class BookBuilderCommon<TColumn>
        where TColumn : IBookDataColumn
    {
        /// <summary>
        /// An <see cref="BookBuilderConfig"/> for <see cref="ExcelBookBuilder"/> configurations.
        /// </summary>
        protected readonly IBookBuilderConfig builderConfig;

        /// <summary>
        /// Create an instance of the <see cref="ExcelBookBuilder"/> class.
        /// </summary>
        /// <param name="bookFactory">An <see cref="IBookFactory{TColumn}"/> object.</param>
        /// <param name="builderConfig">An <see cref="BookBuilderConfig"/> object.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="BookBuilderConfig"/> or <see cref="BookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        public BookBuilderCommon(IBookFactory<TColumn> bookFactory, IBookBuilderConfig builderConfig)
        {
            BookFactory = bookFactory;
            this.builderConfig = builderConfig;
        }

        /// <summary>
        /// An <see cref="IBookFactory{TColumn}"/> object for creating <see cref="IBook{TColumn}"/>s.
        /// </summary>
        public IBookFactory<TColumn> BookFactory { get; }

        /// <summary>
        /// Calculate the starting record numbers for all files.
        /// </summary>
        /// <param name="startRow">The first starting row.</param>
        /// <param name="totalRowCount">The total row count for the file.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="int"/> starting row values.</returns>
        public IList<int> CalculateStartingRows(int startRow, int totalRowCount)
            => ExportsCommon.CalculateChunkStartingPoints(startRow, builderConfig.MaxRowsPerDataFetch, totalRowCount).ToList();

        /// <summary>
        /// Add data to the provided <see cref="IBook{TColumn}"/> object.
        /// </summary>
        /// <param name="book">A <see cref="IBook{TColumn}"/> to add data to.</param>
        /// <param name="startingPoints">A collection of record starting points for pulling data.</param>
        /// <param name="totalRowCount">The total number of records to fetch.</param>
        /// <param name="columns">An <see cref="IEnumerable{T}"/> collection of <typeparamref name="TColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling operations.</param>
        /// <returns>A <see cref="Task"/> object.</returns>
        public async Task AddBookData(IBook<TColumn> book, IList<int> startingPoints, int totalRowCount, IEnumerable<TColumn> columns, CancellationToken cancellationToken)
        {
            var totalRowsLeft = totalRowCount;

            int updateRowsToFetch(int rowCount, out int rowsToFetch)
            {
                if (rowCount <= builderConfig.MaxRowsPerDataFetch)
                {
                    rowsToFetch = rowCount;
                    return 0;
                }

                rowsToFetch = builderConfig.MaxRowsPerDataFetch;
                return rowCount - builderConfig.MaxRowsPerDataFetch;
            }

            for (var i = 0; i < startingPoints.Count; i++)
            {
                var startingPoing = startingPoints[i];

                cancellationToken.ThrowIfCancellationRequested();

                totalRowsLeft = updateRowsToFetch(totalRowsLeft, out var numberOfRowsToFetch);

                book.AddData(await getData(startingPoing, numberOfRowsToFetch, cancellationToken), (IEnumerable<IBookDataColumn>)columns, cancellationToken);
            }
        }

        /// <summary>
        /// Validate the parameters of an <see cref="AddBookData(IBook{TColumn}, IList{int}, int, IEnumerable{TColumn}, CancellationToken)"/> method.
        /// </summary>
        /// <param name="startRow">The starting row index.</param>
        /// <param name="columns">An <see cref="IEnumerable{T}"/> collection of <typeparamref name="TColumn"/>s.</param>
        /// <param name="exceptionHeader">Provide a <see cref="string"/> exception header indicating what function is calling this validate method.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the starting row is less than zero.</exception>
        /// <exception cref="NullReferenceException">Thrown when the collection of columns is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the collection of columns has no items.</exception>
        internal void ValidateAddBookDataParameters(int startRow, IEnumerable<TColumn> columns, string exceptionHeader)
        {
            if (startRow < 0 || columns is null || columns.Count() == 0)
            {
                if (startRow < 0)
                {
                    throw new ArgumentOutOfRangeException($"{exceptionHeader} Error: Starting row index cannot be less than zero.");
                }

                if (columns is null)
                {
                    throw new NullReferenceException($"{exceptionHeader} Error: Columns parameter cannot be null.");
                }
                else
                {
                    throw new ArgumentException($"{exceptionHeader} Error: Columns parameter cannot have zero columns.");
                }
            }
        }

        /// <summary>
        /// Validates the <see cref="IBookBuilderConfig"/> parameter.
        /// </summary>
        /// <typeparam name="TConstructor">The <c>class</c> <see cref="Type"/> to generate the constructor exception header for.</typeparam>
        /// <typeparam name="T1">The type of <see cref="IBookFactory{TColumn}"/> the constructor takes.</typeparam>
        /// <typeparam name="T2">A third extra constructor parameter type.</typeparam>
        /// <param name="bookFactory">An <see cref="IBookFactory{TColumn}"/> object.</param>
        /// <param name="builderConfig">The <see cref="IBookBuilderConfig"/> parameter passed into the constructor.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="IExcelBookFactory"/>, <see cref="IBookBuilderConfig"/>,
        /// or <see cref="IBookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        internal void ValidateConstructorParameters<TConstructor, T1, T2>(IBookFactory<TColumn> bookFactory, IBookBuilderConfig builderConfig)
            where TConstructor : IBookBuilder<TColumn>
            where T1 : IBookFactory<TColumn>
            where T2 : IBookBuilderConfig
            => validateConstructorParameters(bookFactory, builderConfig, () => ExceptionUtil.CreateConstructorExceptionHeader<TConstructor, T1, T2>());

        /// <summary>
        /// Validates the <see cref="IBookBuilderConfig"/> parameter.
        /// </summary>
        /// <typeparam name="TConstructor">The <c>class</c> <see cref="Type"/> to generate the constructor exception header for.</typeparam>
        /// <typeparam name="T1">The type of <see cref="IBookFactory{TColumn}"/> the constructor takes.</typeparam>
        /// <typeparam name="T2">The type of <see cref="IBookBuilderConfig"/> object.</typeparam>
        /// <typeparam name="T3">A third extra constructor parameter type.</typeparam>
        /// <param name="bookFactory">An <see cref="IBookFactory{TColumn}"/> object.</param>
        /// <param name="builderConfig">The <see cref="IBookBuilderConfig"/> parameter passed into the constructor.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="IExcelBookFactory"/>, <see cref="IBookBuilderConfig"/>,
        /// or <see cref="IBookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        internal void ValidateConstructorParameters<TConstructor, T1, T2, T3>(IBookFactory<TColumn> bookFactory, IBookBuilderConfig builderConfig)
            where TConstructor : IBookBuilder<TColumn>
            where T1 : IBookFactory<TColumn>
            where T2 : IBookBuilderConfig
            => validateConstructorParameters(bookFactory, builderConfig, () => ExceptionUtil.CreateConstructorExceptionHeader<TConstructor, T1, T2, T3>());

        /// <summary>
        /// Validates the <see cref="IBookBuilderConfig"/> parameter.
        /// </summary>
        /// <typeparam name="TConstructor">The <c>class</c> <see cref="Type"/> to generate the constructor exception header for.</typeparam>
        /// <typeparam name="T1">The type of <see cref="IBookFactory{TColumn}"/> the constructor takes.</typeparam>
        /// <typeparam name="T2">The type of <see cref="IBookBuilderConfig"/> object.</typeparam>
        /// <typeparam name="T3">A third extra constructor parameter type.</typeparam>
        /// <typeparam name="T4">A fourth extra constructor parameter type.</typeparam>
        /// <param name="bookFactory">An <see cref="IBookFactory{TColumn}"/> object.</param>
        /// <param name="builderConfig">The <see cref="IBookBuilderConfig"/> parameter passed into the constructor.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="IExcelBookFactory"/>, <see cref="IBookBuilderConfig"/>,
        /// or <see cref="IBookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        internal void ValidateConstructorParameters<TConstructor, T1, T2, T3, T4>(IBookFactory<TColumn> bookFactory, IBookBuilderConfig builderConfig)
            where TConstructor : IBookBuilder<TColumn>
            where T1 : IBookFactory<TColumn>
            where T2 : IBookBuilderConfig
            => validateConstructorParameters(bookFactory, builderConfig, () => ExceptionUtil.CreateConstructorExceptionHeader<TConstructor, T1, T2, T3, T4>());

        /// <summary>
        /// Get a collection of <see cref="ExpandoObject"/>s.
        /// </summary>
        /// <param name="startingPoing">The start row index in the back-end.</param>
        /// <param name="numberOfRowsToFetch">The number of records to fetch.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling the request.</param>
        /// <returns>A <see cref="Task{TResult}"/> of type <see cref="IList{T}"/> <see cref="ExpandoObject"/>.</returns>
        private async Task<IList<ExpandoObject>> getData(int startingPoing, int numberOfRowsToFetch, CancellationToken cancellationToken)
            => await builderConfig.FetchDataAsync(startingPoing, numberOfRowsToFetch, cancellationToken);

        /// <summary>
        /// Validates the <see cref="IBookBuilderConfig"/> parameter.
        /// </summary>
        /// <param name="bookFactory">An <see cref="IBookFactory{TColumn}"/> object.</param>
        /// <param name="builderConfig">The <see cref="IBookBuilderConfig"/> parameter passed into the constructor.</param>
        /// <param name="generateHeader">A function to generate the <see cref="Exception"/> header to pass to the <see cref="Exception"/> message.</param>
        /// <exception cref="NullReferenceException">Thrown when either the <see cref="IExcelBookFactory"/>, <see cref="IBookBuilderConfig"/>,
        /// or <see cref="IBookBuilderConfig.FetchDataAsync"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown when the <see cref="IBookBuilderConfig.MaxRowsPerDataFetch"/> is less than or equal to zero.</exception>
        private void validateConstructorParameters(IBookFactory<TColumn> bookFactory, IBookBuilderConfig builderConfig, Func<string> generateHeader)
        {
            if (bookFactory is null || builderConfig is null || builderConfig.FetchDataAsync is null || builderConfig.MaxRowsPerDataFetch <= 0)
            {
                var header = generateHeader();

                if (bookFactory is null)
                {
                    throw new NullReferenceException($"{header} Error: Parameter for \"{nameof(IExcelBookFactory)}\" cannot be null.");
                }

                if (builderConfig is null)
                {
                    throw new NullReferenceException($"{header} Error: Parameter for \"{nameof(IBookBuilderConfig)}\" cannot be null.");
                }

                if (builderConfig.FetchDataAsync is null)
                {
                    throw new NullReferenceException($"{header} Error: Parameter for \"{nameof(builderConfig.FetchDataAsync)}\" cannot be null.");
                }

                if (builderConfig.MaxRowsPerDataFetch <= 0)
                {
                    throw new ArgumentException($"{header} Error: Parameter for \"{nameof(builderConfig.MaxRowsPerDataFetch)}\" cannot be less than or equal to zero.");
                }
            }
        }
    }
}