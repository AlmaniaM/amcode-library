using System;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Exports
{
    /// <summary>
    /// An interface that represents an export file result.
    /// </summary>
    public interface IExportResult : IDisposable
    {
        /// <summary>
        /// The number of book entries.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The name of the export.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The type of file the result is.
        /// </summary>
        FileType FileType { get; }

        /// <summary>
        /// Get the <see cref="Stream"/> of data representing the export book.
        /// </summary>
        /// <returns>A <see cref="Task{T}"/> of type <see cref="Stream"/>.</returns>
        Task<Stream> GetDataAsync();

        /// <summary>
        /// Set the <see cref="Stream"/> data representing the export book.
        /// </summary>
        /// <param name="data">A <see cref="Stream"/> of the export book.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        Task SetDataAsync(Stream data);
    }
}