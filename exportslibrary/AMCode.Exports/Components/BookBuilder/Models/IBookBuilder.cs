using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports.Book;
using AMCode.Storage;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// An interface designed for building an <see cref="IBook{TColumn}"/> object.
    /// </summary>
    public interface IBookBuilder
    {
        /// <summary>
        /// Build an <see cref="IBook{TColumn}"/>.
        /// </summary>
        /// <param name="startRow">The first row starting point.</param>
        /// <param name="count">The total number of rows to add.</param>
        /// <param name="columns">A collection of <see cref="IBookDataColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling operations.</param>
        /// <returns>A <see cref="Task"/> of an <see cref="IStreamDataSourceAsync"/> object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the starting row is less than zero.</exception>
        /// <exception cref="NullReferenceException">Thrown when the collection of columns is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the collection of columns has no items.</exception>
        Task<IStreamDataSourceAsync> BuildBookAsync(int startRow, int count, IEnumerable<IBookDataColumn> columns, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// An interface designed for building an <see cref="IBook{TColumn}"/> object.
    /// </summary>
    /// <typeparam name="TColumn">The type of <see cref="IBookDataColumn"/> you are building</typeparam>
    public interface IBookBuilder<in TColumn> : IBookBuilder
        where TColumn : IBookDataColumn
    {
        /// <summary>
        /// Build an <see cref="IBook{TColumn}"/>.
        /// </summary>
        /// <param name="startRow">The first row starting point.</param>
        /// <param name="count">The total number of rows to add.</param>
        /// <param name="columns">A collection of <typeparamref name="TColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling operations.</param>
        /// <returns>A <see cref="Task"/> of an <see cref="IStreamDataSourceAsync"/> object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the starting row is less than zero.</exception>
        /// <exception cref="NullReferenceException">Thrown when the collection of columns is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the collection of columns has no items.</exception>
        Task<IStreamDataSourceAsync> BuildBookAsync(int startRow, int count, IEnumerable<TColumn> columns, CancellationToken cancellationToken = default);
    }
}