using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DemandLink.Common.Util;

namespace DemandLink.Storage.Local
{
    /// <summary>
    /// A class designed to store files locally.
    /// </summary>
    public class SimpleLocalStorage : ISimpleFileStorage
    {
        private readonly string baseDirectory;

        /// <summary>
        /// Create an instance of the <see cref="SimpleLocalStorage"/> class.
        /// </summary>
        public SimpleLocalStorage() : this(Directory.GetCurrentDirectory()) { }

        /// <summary>
        /// Create an instance of the <see cref="SimpleLocalStorage"/> class.
        /// </summary>
        /// <param name="baseDirectory">The base directory where to store the files.</param>
        public SimpleLocalStorage(string baseDirectory)
        {
            this.baseDirectory = baseDirectory;
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">Thrown when a file already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        public async Task<string> CreateFileAsync(string fileName, Stream stream, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            createDirectoryIfNotExists();

            var filePath = Path.Combine(baseDirectory, fileName);

            if (overwrite && File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else if (!overwrite && File.Exists(filePath))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, Stream, bool, CancellationToken, Task<string>>(CreateFileAsync);
                throw new Exception($"{header} Error: Cannot overwrite existing file. If you wish to overwrite the file then set the {nameof(overwrite)} parameter to true.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var fs = new FileStream(Path.Combine(baseDirectory, fileName), FileMode.CreateNew))
            {
                if (stream.CanRead)
                {
                    await stream.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
            }

            return fileName;
        }

        /// <inheritdoc/>
        public Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(baseDirectory))
            {
                return Task.FromResult(false);
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                File.Delete(createFilePath(fileName));
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        /// <exception cref="FileDoesNotExistException">Thrown when a file cannot be found.</exception>
        public async Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var filePath = createFilePath(fileName);

            if (!File.Exists(filePath))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, CancellationToken, Task<Stream>>(DownloadFileAsync);
                throw new FileDoesNotExistException($"{header} Error: Cannot find file at \"{filePath}\".");
            }

            cancellationToken.ThrowIfCancellationRequested();

            using (var fs = new FileStream(createFilePath(fileName), FileMode.Open, FileAccess.Read))
            {
                var memoryStream = new MemoryStream();
                await fs.CopyToAsync(memoryStream);

                if (cancellationToken.IsCancellationRequested)
                {
                    memoryStream.Dispose();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                await memoryStream.FlushAsync();
                memoryStream.Seek(0, SeekOrigin.Begin);

                return memoryStream;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string fileName, CancellationToken cancellationToken = default)
            => await Task.FromResult(File.Exists(createFilePath(fileName)));

        /// <summary>
        /// Create a directory if it doesn't exist.
        /// </summary>
        private void createDirectoryIfNotExists()
        {
            if (!Directory.Exists(baseDirectory))
            {
                Directory.CreateDirectory(baseDirectory);
            }
        }

        /// <summary>
        /// Create an absolute file path from the given file name.
        /// </summary>
        /// <param name="fileName">A file name to create a path for.</param>
        /// <returns></returns>
        private string createFilePath(string fileName) => Path.Combine(baseDirectory, fileName);
    }
}