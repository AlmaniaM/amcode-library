using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DemandLink.Common.Extensions.Enumerables;
using DemandLink.Common.Util;

namespace DemandLink.Storage.Memory
{
    /// <summary>
    /// A class designed to store <see cref="Stream"/>s in memory.
    /// </summary>
    public class StreamStorage : ISimpleFileStorage, IDisposable
    {
        private readonly IDictionary<string, Stream> streamStore;
        private const int BUFFER_SIZE = 16000;

        /// <summary>
        /// Create an instance of the <see cref="StreamStorage"/> class.
        /// </summary>
        public StreamStorage() : this(new Dictionary<string, Stream>()) { }

        /// <summary>
        /// Create an instance of the <see cref="StreamStorage"/> class.
        /// </summary>
        /// <param name="streamStore">The <see cref="IDictionary{TKey, TValue}"/> to use for storing the <see cref="Stream"/>s.</param>
        public StreamStorage(IDictionary<string, Stream> streamStore)
        {
            this.streamStore = streamStore;
        }

        /// <inheritdoc/>
        /// <exception cref="Exception">Thrown when a file already exists and <paramref name="overwrite"/> is <c>false</c>.</exception>
        public async Task<string> CreateFileAsync(string fileName, Stream stream, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            if (overwrite && streamStore.ContainsKey(fileName))
            {
                streamStore.Remove(fileName);
            }
            else if (!overwrite && streamStore.ContainsKey(fileName))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, Stream, bool, CancellationToken, Task<string>>(CreateFileAsync);
                throw new Exception($"{header} Error: Cannot overwrite existing stream. If you wish to overwrite the file then set the {nameof(overwrite)} parameter to true.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, BUFFER_SIZE);
            await memoryStream.FlushAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);

            streamStore.Add(fileName, memoryStream);

            return fileName;
        }

        /// <inheritdoc/>
        public Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            if (!streamStore.ContainsKey(fileName))
            {
                return Task.FromResult(false);
            }

            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                streamStore.Remove(fileName);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Dispose of all streams and the dictionary.
        /// </summary>
        public void Dispose()
        {
            streamStore?.Keys.ForEach(key => streamStore[key].Dispose());
            streamStore.Clear();
        }

        /// <inheritdoc/>
        /// <exception cref="FileDoesNotExistException">Thrown when a file cannot be found.</exception>
        public Task<Stream> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
        {
            if (!streamStore.ContainsKey(fileName))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, CancellationToken, Task<Stream>>(DownloadFileAsync);
                throw new FileDoesNotExistException($"{header} Error: Cannot find file \"{fileName}\".");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(streamStore[fileName]);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string fileName, CancellationToken cancellationToken = default)
            => await Task.FromResult(streamStore.ContainsKey(fileName));
    }
}