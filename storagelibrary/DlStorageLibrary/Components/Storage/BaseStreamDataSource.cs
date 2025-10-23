using System;
using System.IO;
using System.Threading.Tasks;

namespace DemandLink.Storage
{
    /// <summary>
    /// A class designed to store <see cref="Stream"/>s as files locally.
    /// </summary>
    public class BaseStreamDataSource : IStreamDataSourceAsync
    {
        /// <summary>
        /// The underlying <see cref="ISimpleFileStorage"/> object used to store and retrieve the <see cref="Stream"/>s.
        /// </summary>
        protected readonly ISimpleFileStorage storage;

        /// <summary>
        /// Create an instance of the <see cref="BaseStreamDataSource"/> class.
        /// </summary>
        /// <param name="storage">Provide an <see cref="ISimpleFileStorage"/> object for storing the <see cref="Stream"/> data.</param>
        public BaseStreamDataSource(ISimpleFileStorage storage)
        {
            FileName = Guid.NewGuid().ToString();
            this.storage = storage;
        }

        /// <summary>
        /// The name of the file.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Delete the underlying file.
        /// </summary>
        public void Dispose() => storage.DeleteFileAsync(FileName).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<Stream> GetStreamAsync() => await storage.DownloadFileAsync(FileName);

        /// <inheritdoc/>
        public async Task SetStreamAsync(Stream stream) => await storage.CreateFileAsync(FileName, stream, overwrite: true);
    }
}