using System.IO;
using System.Threading.Tasks;

namespace AMCode.Exports.Storage
{
    /// <summary>
    /// An in-memory implementation of <see cref="IStreamDataSourceAsync"/> that stores stream data in a <see cref="MemoryStream"/>.
    /// </summary>
    public class MemoryStreamDataSource : IStreamDataSourceAsync
    {
        private MemoryStream _memoryStream = new MemoryStream();
        private bool _disposed = false;

        /// <inheritdoc/>
        public async Task<Stream> GetStreamAsync()
        {
            _memoryStream.Position = 0;
            return await Task.FromResult<Stream>(_memoryStream);
        }

        /// <inheritdoc/>
        public async Task SetStreamAsync(Stream stream)
        {
            _memoryStream = new MemoryStream();
            await stream.CopyToAsync(_memoryStream);
            _memoryStream.Position = 0;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_disposed)
            {
                _memoryStream?.Dispose();
                _disposed = true;
            }
        }
    }
}
