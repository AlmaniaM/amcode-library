using System.IO;
using System.Threading.Tasks;
using AMCode.Exports.Storage;

namespace AMCode.Exports.Results
{
    /// <summary>
    /// An interface designed to represent a factory for building <see cref="IExportResult"/> objects.
    /// </summary>
    public interface IExportResultFactory
    {
        /// <summary>
        /// Create an <see cref="IExportResult"/> object.
        /// </summary>
        /// <param name="dataSource">The <see cref="IStreamDataSourceAsync"/> data source.</param>
        /// <param name="fileType">The <see cref="FileType"/>.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="count">The number of files.</param>
        /// <returns>An <see cref="IExportResult"/> object.</returns>
        Task<IExportResult> CreateAsync(IStreamDataSourceAsync dataSource, FileType fileType, string fileName, int count);

        /// <summary>
        /// Create an <see cref="IExportResult"/> object.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> data.</param>
        /// <param name="fileType">The <see cref="FileType"/>.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="count">The number of files.</param>
        /// <returns>An <see cref="IExportResult"/> object.</returns>
        Task<IExportResult> CreateAsync(Stream stream, FileType fileType, string fileName, int count);
    }
}
