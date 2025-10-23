using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports.Book;

namespace AMCode.Exports
{
    /// <summary>
    /// An interface designed for building different types of exports.
    /// </summary>
    /// <typeparam name="TColumn">The type of <see cref="IBookDataColumn"/> that will be provided.</typeparam>
    public interface IExportBuilder<in TColumn> where TColumn : IBookDataColumn
    {
        /// <summary>
        /// Calculate the number of books that will be generated for the number of rows given.
        /// </summary>
        /// <param name="totalRowCount">The number of rows to be generated.</param>
        /// <returns>An <see cref="int"/> number of books that will be generated.</returns>
        int CalculateNumberOfBooks(int totalRowCount);

        /// <summary>
        /// Create an export file.
        /// </summary>
        /// <param name="fileName">The name to give the file.</param>
        /// <param name="totalRowCount">The total number of records to create.</param>
        /// <param name="columns">Provide an <see cref="IEnumerable{T}"/> collection of <see cref="IBookDataColumn"/>s.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IExportResult"/> object.</returns>
        Task<IExportResult> CreateExportAsync(string fileName, int totalRowCount, IEnumerable<TColumn> columns, CancellationToken cancellationToken);
    }
}