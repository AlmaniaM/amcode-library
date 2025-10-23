using System.IO;
using System.Threading.Tasks;

namespace AMCode.Exports
{
    /// <summary>
    /// A class designed to host the results of an export.
    /// </summary>
    public class InMemoryExportResult : IExportResult
    {
        /// <inheritdoc/>
        public int Count { get; set; }

        /// <summary>
        /// The <see cref="Stream"/> of data representing an export book.
        /// </summary>
        public Stream Data { get; set; }

        /// <inheritdoc/>
        public FileType FileType { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (Data != null)
            {
                Data.Dispose();
            }
        }

        /// <inheritdoc/>
        public Task<Stream> GetDataAsync() => Task.FromResult(Data);

        /// <inheritdoc/>
        public async Task SetDataAsync(Stream data) => await Task.Run(() => Data = data);
    }
}