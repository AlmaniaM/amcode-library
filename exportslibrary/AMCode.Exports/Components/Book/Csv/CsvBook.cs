using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.Util;
using AMCode.Exports.Common.Exceptions;
using AMCode.Exports.Common.Exceptions.Util;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// A class designed to represent a constructible CSV book.
    /// </summary>
    public class CsvBook : ICsvBook
    {
        private readonly StreamWriter csvStream;
        private readonly Stream stream;

        /// <summary>
        /// Create an instance of the <see cref="CsvBook"/> class.
        /// </summary>
        public CsvBook()
        {
            stream = new MemoryStream
            {
                Position = 0
            };
            csvStream = new StreamWriter(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Create an instance of the <see cref="CsvBook"/> class.
        /// </summary>
        /// <param name="stream">Provide a stream to write data to.</param>
        public CsvBook(Stream stream)
        {
            this.stream = stream;
            csvStream = new StreamWriter(this.stream);
        }

        /// <summary>
        /// Add a collection of data <see cref="ExpandoObject"/>s to the CSV book.
        /// </summary>
        /// <param name="dataList">An <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s.</param>
        /// <param name="columns">An <see cref="IEnumerable{T}"/> collection of <see cref="ICsvDataColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <inheritdoc/>
        public void AddData(IList<ExpandoObject> dataList, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var columnsList = columns?.ToList();

            validateAddDataParameters(dataList, columnsList);

            cancellationToken.ThrowIfCancellationRequested();

            for (var currentRowIndex = 0; currentRowIndex < dataList.Count; currentRowIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var dataRow = dataList[currentRowIndex];
                var cellValues = new List<string>();

                columnsList.ForEach((column, columnIndex) =>
                {
                    var cellValue = dataRow.GetValue<object>(column.DataFieldName);
                    var formattedCellValue = (column as ICsvDataColumn).FormatValue(cellValue);
                    cellValues.Add($"\"{formattedCellValue}\"");
                });

                var values = string.Join(",", cellValues);
                csvStream.WriteLine(values);
            }

            cancellationToken.ThrowIfCancellationRequested();
        }

        /// <inheritdoc/>
        public void Dispose() => csvStream.Dispose();

        /// <inheritdoc/>
        public Stream Save()
        {
            var savedStream = new MemoryStream();

            SaveAs(savedStream);

            return savedStream;
        }

        /// <inheritdoc/>
        public void SaveAs(Stream saveAsStream)
        {
            // Save original underlying stream position
            var originalPosition = stream.Position;

            // Save buffered data to underlying stream
            csvStream.Flush();

            // Rewind the underlying stream for copy operation
            stream.Seek(0, SeekOrigin.Begin);

            // Copy the underlying stream to the new stream
            stream.CopyTo(saveAsStream);

            // Set the underlying stream position back to its original state
            stream.Seek(originalPosition, SeekOrigin.Current);

            // Save the data
            saveAsStream.Flush();

            // Rewind the stream so its' ready to use
            saveAsStream.Position = 0;
        }

        /// <inheritdoc/>
        public void SetColumns(IEnumerable<string> columns)
        {
            var columnList = columns?.ToList();

            if (columnList is null || columnList.Count == 0)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IList<string>>(SetColumns);

                if (columnList is null || columnList.Count == 0)
                {
                    if (columnList is null)
                    {
                        throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(columns)));
                    }

                    throw new EmptyCollectionException(header, nameof(columns));
                }
            }

            csvStream.WriteLine(string.Join(",", columnList));
        }

        /// <summary>
        /// Validate the parameters and throw any necessary exceptions.
        /// </summary>
        /// <param name="dataList">An <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s.</param>
        /// <param name="columns">A <see cref="IList{T}"/> of <see cref="IExcelDataColumn"/>s.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="dataList"/> or <paramref name="columns"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="dataList"/> or <paramref name="columns"/> is an empty collection.</exception>
        /// <exception cref="MaxColumnCountExceededException">Thrown when <paramref name="columns"/> count exceeds max allowed columns.</exception>
        /// <exception cref="OperationCanceledException">Thrown when the current operation is canceled by the provided <see cref="CancellationToken"/>.</exception>
        private void validateAddDataParameters(IList<ExpandoObject> dataList, IList<IBookDataColumn> columns)
        {
            if (dataList is null || dataList.Count == 0 || columns is null || columns.Count == 0)
            {
                var header = ExceptionUtil.CreateExceptionHeader<IList<ExpandoObject>, IList<IBookDataColumn>, CancellationToken>(AddData);

                if (dataList is null || dataList.Count == 0)
                {
                    if (dataList is null)
                    {
                        throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(dataList)));
                    }

                    if (dataList.Count == 0)
                    {
                        throw new EmptyCollectionException(header, nameof(dataList));
                    }
                }

                if (columns is null || columns.Count == 0)
                {
                    if (columns is null)
                    {
                        throw new NullReferenceException(ExportsExceptionUtil.CreateNullReferenceExceptionMessage(header, nameof(columns)));
                    }

                    throw new EmptyCollectionException(header, nameof(columns));
                }
            }
        }
    }
}