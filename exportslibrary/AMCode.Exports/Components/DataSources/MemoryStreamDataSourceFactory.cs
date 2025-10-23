using System.IO;
using System.Threading.Tasks;
using AMCode.Storage;
using AMCode.Storage.Memory;

namespace AMCode.Exports.DataSource
{
    /// <summary>
    /// A class designed to create instances of the <see cref="MemoryStreamDataSource"/> class.
    /// </summary>
    public class MemoryStreamDataSourceFactory : IExportStreamDataSourceFactory
    {
        /// <summary>
        /// Create an instance of the <see cref="MemoryStreamDataSource"/> and store the provided Stream.
        /// </summary>
        /// <inheritdoc/>
        public async Task<IStreamDataSourceAsync> CreateAsync(Stream stream)
        {
            var dataSource = new MemoryStreamDataSource();
            await dataSource.SetStreamAsync(stream);
            return dataSource;
        }
    }
}