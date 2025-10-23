using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Exports.Book;

namespace AMCode.Exports.BookBuilder
{
    /// <summary>
    /// An interface designed for compiling a single <see cref="IBook{TColumn}"/>
    /// or multiple <see cref="IBook{TColumn}"/>s into a zip archive.
    /// </summary>
    public interface IBookCompiler
    {
        /// <summary>
        /// Calculate the number of books needed to be built.
        /// </summary>
        /// <param name="totalRowCount">The total number of rows to build.</param>
        /// <returns>An <see cref="int"/> representing the number of books needed to be built.</returns>
        int CalculateNumberOfBooks(int totalRowCount);

        /// <summary>
        /// Compile a CSV book into a <see cref="Stream"/> result. If more than one book is required to fulfill the <paramref name="totalRowCount"/> then
        /// a .zip will be compiled instead.
        /// </summary>
        /// <param name="name">The name to give the export.</param>
        /// <param name="totalRowCount">The total number of rows to be compiled.</param>
        /// <param name="columns">An <see cref="IEnumerable{T}"/> collection of <see cref="IBookDataColumn"/>s.</param>
        /// <param name="fileType">The <see cref="FileType"/> to construct.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IExportResult"/> with the data, <see cref="FileType"/>, and file name.</returns>
        Task<IExportResult> CompileBookAsync(string name, int totalRowCount, IEnumerable<IBookDataColumn> columns, FileType fileType, CancellationToken cancellationToken = default);

        /// <summary>
        /// Compile a CSV book into a <see cref="Stream"/> result. If more than one book is required to fulfill the <paramref name="totalRowCount"/> then
        /// a .zip will be compiled instead.
        /// </summary>
        /// <param name="name">The name to give the export.</param>
        /// <param name="totalRowCount">The total number of rows to be compiled.</param>
        /// <param name="csvColumns">An <see cref="IEnumerable{T}"/> collection of <see cref="ICsvDataColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IExportResult"/> with the data, <see cref="FileType"/>, and file name.</returns>
        Task<IExportResult> CompileCsvAsync(string name, int totalRowCount, IEnumerable<ICsvDataColumn> csvColumns, CancellationToken cancellationToken = default);

        /// <summary>
        /// Compile a Xlsx book into a <see cref="Stream"/> result. If more than one book is required to fulfill the <paramref name="totalRowCount"/> then
        /// a .zip will be compiled instead.
        /// </summary>
        /// <param name="name">The name to give the export.</param>
        /// <param name="totalRowCount">The total number of rows to be compiled.</param>
        /// <param name="excelColumns">An <see cref="IEnumerable{T}"/> collection of <see cref="IExcelDataColumn"/>s.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling requests.</param>
        /// <returns>An <see cref="IExportResult"/> with the data, <see cref="FileType"/>, and file name.</returns>
        Task<IExportResult> CompileExcelAsync(string name, int totalRowCount, IEnumerable<IExcelDataColumn> excelColumns, CancellationToken cancellationToken = default);
    }
}