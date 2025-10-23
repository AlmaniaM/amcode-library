using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.IO.Zip;
using AMCode.Common.Util;
using AMCode.Exports.Book;
using AMCode.Exports.Common;
using AMCode.Exports.Common.Exceptions.Util;
using AMCode.Exports.Extensions;
using AMCode.Exports.Results;
using AMCode.Exports.Zip;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// A class designed for compiling a single <see cref="IBook{TColumn}"/>
    /// or multiple <see cref="IBook{TColumn}"/>s into a zip archive.
    /// </summary>
    public class BookCompiler : IBookCompiler
    {
        private readonly IBookBuilderFactory bookBuilderFactory;
        private readonly IExportResultFactory exportResultFactory;
        private readonly int maxRowsPerBook;
        private readonly IZipArchiveFactory zipArchiveFactory;

        /// <summary>
        /// Create an instance of the <see cref="BookCompiler"/> class.
        /// </summary>
        /// <param name="bookBuilderFactory">A <see cref="IBookBuilderFactory"/> object for creating <see cref="IBookBuilder{TColumn}"/>s.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per <see cref="IBook{TColumn}"/>.</param>
        public BookCompiler(IBookBuilderFactory bookBuilderFactory, int maxRowsPerBook)
            : this(bookBuilderFactory, maxRowsPerBook, new DataSourceExportResultFactory()) { }

        /// <inheritdoc cref="BookCompiler(IBookBuilderFactory, int)"/>
        /// <param name="bookBuilderFactory">A <see cref="IBookBuilderFactory"/> object for creating <see cref="IBookBuilder{TColumn}"/>s.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per <see cref="IBook{TColumn}"/>.</param>
        /// <param name="exportResultFactory">Provide an <see cref="IExportResultFactory"/> for creating <see cref="IExportResult"/> objects.</param>
        public BookCompiler(IBookBuilderFactory bookBuilderFactory, int maxRowsPerBook, IExportResultFactory exportResultFactory)
            : this(bookBuilderFactory, maxRowsPerBook, new ZipArchiveFactory(), exportResultFactory) { }

        /// <inheritdoc cref="BookCompiler(IBookBuilderFactory, int)"/>
        /// <param name="bookBuilderFactory">A <see cref="IBookBuilderFactory"/> object for creating <see cref="IBookBuilder{TColumn}"/>s.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per <see cref="IBook{TColumn}"/>.</param>
        /// <param name="zipArchiveFactory">Provide an <see cref="IZipArchiveFactory"/> for creating <see cref="IZipArchive"/> objects.</param>
        public BookCompiler(IBookBuilderFactory bookBuilderFactory, int maxRowsPerBook, IZipArchiveFactory zipArchiveFactory)
            : this(bookBuilderFactory, maxRowsPerBook, zipArchiveFactory, new DataSourceExportResultFactory()) { }

        /// <inheritdoc cref="BookCompiler(IBookBuilderFactory, int)"/>
        /// <param name="bookBuilderFactory">A <see cref="IBookBuilderFactory"/> object for creating <see cref="IBookBuilder{TColumn}"/>s.</param>
        /// <param name="maxRowsPerBook">The maximum number of rows per <see cref="IBook{TColumn}"/>.</param>
        /// <param name="zipArchiveFactory">Provide an <see cref="IZipArchiveFactory"/> for creating <see cref="IZipArchive"/> objects.</param>
        /// <param name="exportResultFactory">Provide an <see cref="IExportResultFactory"/> for creating <see cref="IExportResult"/> objects.</param>
        public BookCompiler(IBookBuilderFactory bookBuilderFactory, int maxRowsPerBook, IZipArchiveFactory zipArchiveFactory, IExportResultFactory exportResultFactory)
        {
            this.bookBuilderFactory = bookBuilderFactory;
            this.maxRowsPerBook = maxRowsPerBook;
            this.zipArchiveFactory = zipArchiveFactory;
            this.exportResultFactory = exportResultFactory;
        }

        /// <summary>
        /// Calculate the number of books needed to be built.
        /// </summary>
        /// <param name="totalRowCount">The total number of rows to build.</param>
        /// <returns>A <see cref="int"/> representing the number of books needed to be built.</returns>
        public int CalculateNumberOfBooks(int totalRowCount)
            => ExportsCommon.CalculateNumberOfChunks(totalRowCount, maxRowsPerBook);

        /// <inheritdoc/>
        public async Task<IExportResult> CompileBookAsync(string name, int totalRowCount, IEnumerable<IBookDataColumn> columns, FileType fileType, CancellationToken cancellationToken = default)
        {
            validateCompileBookAsyncParameters(totalRowCount, columns, fileType);

            cancellationToken.ThrowIfCancellationRequested();

            var booksDictionary = await createIndexedBookDictionary(totalRowCount, columns, fileType, name, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (booksDictionary.Count > 1)
            {
                return await createZipResult(name, booksDictionary, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return booksDictionary[0];
        }

        /// <inheritdoc/>
        public async Task<IExportResult> CompileCsvAsync(string name, int totalRowCount, IEnumerable<ICsvDataColumn> csvColumns, CancellationToken cancellationToken = default)
            => await CompileBookAsync(name, totalRowCount, csvColumns, FileType.Csv, cancellationToken);

        /// <inheritdoc/>
        public async Task<IExportResult> CompileExcelAsync(string name, int totalRowCount, IEnumerable<IExcelDataColumn> excelColumns, CancellationToken cancellationToken = default)
            => await CompileBookAsync(name, totalRowCount, excelColumns, FileType.Xlsx, cancellationToken);

        /// <summary>
        /// Create an <see cref="IDictionary{TKey, TValue}"/> of <c>int</c> keys (which are the order index of each book) and
        /// <see cref="IExportResult"/> values (which are the books).
        /// </summary>
        /// <param name="totalRowCount">The total number of rows to be compiled.</param>
        /// <param name="columns">An <see cref="IEnumerable{T}"/> collection of <see cref="IBookDataColumn"/>s.</param>
        /// <param name="fileType">The <see cref="FileType"/> to construct.</param>
        /// <param name="name">The name to give the export.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IDictionary{TKey, TValue}"/> of <c>int</c> index keys and <see cref="IExportResult"/> book values.</returns>
        private async Task<IDictionary<int, IExportResult>> createIndexedBookDictionary(int totalRowCount, IEnumerable<IBookDataColumn> columns, FileType fileType, string name, CancellationToken cancellationToken)
        {
            var numberOfBooks = CalculateNumberOfBooks(totalRowCount);
            var booksDictionary = new ConcurrentDictionary<int, IExportResult>();
            var startingPoints = ExportsCommon.CalculateChunkStartingPoints(0, maxRowsPerBook, totalRowCount).ToList();

            for (var i = 0; i < numberOfBooks; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var bookBuilder = bookBuilderFactory.CreateBuilder(fileType);
                var startingRow = startingPoints.ElementAt(i);

                var rowCount = i == numberOfBooks - 1 ? totalRowCount - startingRow : maxRowsPerBook;

                var bookResult = await exportResultFactory.CreateAsync(await bookBuilder.BuildBookAsync(startingRow, rowCount, columns, cancellationToken), fileType, name, 1);

                booksDictionary[i] = bookResult;
            }

            return booksDictionary;
        }

        /// <summary>
        /// Create a zip file of all the books.
        /// </summary>
        /// <param name="name">The name to give the export.</param>
        /// <param name="booksDictionary">An <see cref="IDictionary{TKey, TValue}"/> of <c>int</c> book index keys and book
        /// <see cref="IExportResult"/> values.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IExportResult"/> of a zip file.</returns>
        private async Task<IExportResult> createZipResult(string name, IDictionary<int, IExportResult> booksDictionary, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var zipEntries = booksDictionary
                .OrderBy(keyValue => keyValue.Key)
                .Select((keyValue, index) =>
                {
                    var originalFileName = keyValue.Value.CreateFileName();
                    var extension = Path.GetExtension(originalFileName);

                    var newFileName = $"{index + 1}-{Path.GetFileNameWithoutExtension(originalFileName)}{extension}";

                    return new ZipEntry
                    {
                        GetDataAsync = keyValue.Value.GetDataAsync,
                        Name = newFileName
                    };
                });

            cancellationToken.ThrowIfCancellationRequested();

            var zipArchive = zipArchiveFactory.Create(zipEntries);
            using (var zipResult = await zipArchive.CreateZipAsync(name))
            {
                cancellationToken.ThrowIfCancellationRequested();

                booksDictionary.ForEach(entry => entry.Value.Dispose());

                var exportResult = await exportResultFactory.CreateAsync(zipResult.Data, FileType.Zip, name, zipArchive.ZipEntries.Count);

                return exportResult;
            }
        }

        /// <summary>
        /// Validates some parameters for the <see cref="CompileBookAsync(string, int, IEnumerable{IBookDataColumn}, FileType, CancellationToken)"/> function.
        /// </summary>
        /// <param name="totalRowCount">The total row count to validate.</param>
        /// <param name="columns">The columns collection to validate.</param>
        /// <param name="fileType">The file type to validate.</param>
        private void validateCompileBookAsyncParameters(int totalRowCount, IEnumerable<IBookDataColumn> columns, FileType fileType)
        {
            var columnList = columns?.ToList();

            if (totalRowCount <= 0 || columnList is null || columnList.Count == 0 || !Enum.IsDefined(typeof(FileType), fileType))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, int, IEnumerable<IBookDataColumn>, FileType, CancellationToken, Task>(CompileBookAsync);

                if (totalRowCount <= 0)
                {
                    throw new ArgumentException(
                        ExportsExceptionUtil.CreateLessThanEqualToZeroExceptionMessage(header, nameof(totalRowCount))
                    );
                }

                if (columns is null)
                {
                    throw new NullReferenceException(
                        ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(columns))
                    );
                }

                if (columnList.Count == 0)
                {
                    throw new ArgumentException(
                        ExportsExceptionUtil.CreateEmptyCollectionExceptionMessage(header, nameof(columns))
                    );
                }

                throw new ArgumentException(
                    $"{header} Error: Parameter \"{nameof(fileType)}\" has a value of \"{fileType}\" which does not exit in the type {typeof(FileType)}"
                );
            }
        }
    }
}