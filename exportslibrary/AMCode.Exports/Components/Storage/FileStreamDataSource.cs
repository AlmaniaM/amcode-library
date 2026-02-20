using System;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Exports.Storage
{
    /// <summary>
    /// A file-based implementation of <see cref="IStreamDataSourceAsync"/> that stores stream data to a temporary file.
    /// </summary>
    public class FileStreamDataSource : IStreamDataSourceAsync
    {
        private readonly string _directory;
        private string _filePath;
        private bool _disposed = false;

        /// <summary>
        /// Create an instance of the <see cref="FileStreamDataSource"/> class using the current directory.
        /// </summary>
        public FileStreamDataSource() : this(Directory.GetCurrentDirectory()) { }

        /// <summary>
        /// Create an instance of the <see cref="FileStreamDataSource"/> class.
        /// </summary>
        /// <param name="directory">The absolute path to the directory where the file will be stored.</param>
        public FileStreamDataSource(string directory)
        {
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
        }

        /// <inheritdoc/>
        public async Task<Stream> GetStreamAsync()
        {
            if (_filePath == null || !File.Exists(_filePath))
            {
                return await Task.FromResult<Stream>(Stream.Null);
            }

            var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            return fileStream;
        }

        /// <inheritdoc/>
        public async Task SetStreamAsync(Stream stream)
        {
            _filePath = Path.Combine(_directory, $"{Guid.NewGuid()}.tmp");
            using var fileStream = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_filePath != null && File.Exists(_filePath))
                {
                    try { File.Delete(_filePath); } catch { }
                }

                _disposed = true;
            }
        }
    }
}
