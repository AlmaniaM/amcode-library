using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DemandLink.Storage
{
    /// <summary>
    /// An interface designed to represent a simple file store, fetch, and delete operations object.
    /// </summary>
    public interface ISimpleFileStorage
    {
        /// <summary>
        /// Create a file.
        /// </summary>
        /// <param name="fileName">The name to use when storing the file.</param>
        /// <param name="stream">A <see cref="Stream"/> of the file data.</param>
        /// <param name="overwrite">Whether or not to overwrite an existing file with the same name.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling the request.</param>
        /// <exception cref="CannotAccessStorageException">When the process cannot access the storage location.</exception>
        /// <returns>An <see cref="string"/> new file name <see cref="Task"/>.</returns>
        Task<string> CreateFileAsync(string fileName, Stream stream, bool overwrite = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling the request.</param>
        /// <returns>Will be <c>true</c> if the file exists and was deleted. Otherwise <c>false</c>.</returns>
        Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Download a <see cref="Stream"/> of the file.
        /// </summary>
        /// <param name="fileName">The name file you are looking to download.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling the request.</param>
        /// <exception cref="FileNotFoundException">When the specified file name does not exist.</exception>
        /// <returns>An <see cref="Task"/> that when finished returns a <see cref="Stream"/> of the file data..</returns>
        Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if the given file name exists.
        /// </summary>
        /// <param name="fileName">The name file you are looking to download.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> for canceling the request.</param>
        /// <returns>A <see cref="Task"/> that returns <c>true</c> if the file name exists and <c>false</c> if not.</returns>
        Task<bool> ExistsAsync(string fileName, CancellationToken cancellationToken = default);
    }
}