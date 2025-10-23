using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Threading;
using AMCode.Exports.Common.Exceptions;

namespace AMCode.Exports.Book
{
    /// <summary>
    /// An interface designed to act as a row/cell book/file.
    /// </summary>
    /// <typeparam name="TColumn">An <see cref="IBookDataColumn"/> type of column.</typeparam>
    public interface IBook<out TColumn> : IDisposable
        where TColumn : IBookDataColumn
    {
        /// <summary>
        /// Add a collection of data <see cref="ExpandoObject"/>s to the book.
        /// </summary>
        /// <param name="dataList">An <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s.</param>
        /// <param name="columns">A <see cref="IEnumerable{T}"/> of <see cref="IBookDataColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="dataList"/> or <paramref name="columns"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="dataList"/> or <paramref name="columns"/> is an empty collection.</exception>
        /// <exception cref="OperationCanceledException">Thrown when the current operation is canceled by the provided <see cref="CancellationToken"/>.</exception>
        void AddData(IList<ExpandoObject> dataList, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken = default);

        /// <summary>
        /// Save the current row/cell book as a <see cref="Stream"/>.
        /// </summary>
        /// <returns>A <see cref="Stream"/> object.</returns>
        Stream Save();

        /// <summary>
        /// Save the current row/cell book as the provided <see cref="Stream"/>.
        /// </summary>
        /// <param name="saveAsStream">A <see cref="Stream"/> object to save the row/cell book to.</param>
        void SaveAs(Stream saveAsStream);

        /// <summary>
        /// Set a collection of <see cref="string"/> names to be the row/cell columns.
        /// </summary>
        /// <param name="columns">Provide a <see cref="IEnumerable{T}"/> of <see cref="string"/> column names.</param>
        /// <exception cref="NullReferenceException">Thrown when <paramref name="columns"/> is null.</exception>
        /// <exception cref="EmptyCollectionException">Thrown when <paramref name="columns"/> is an empty collection.</exception>
        void SetColumns(IEnumerable<string> columns);
    }
}