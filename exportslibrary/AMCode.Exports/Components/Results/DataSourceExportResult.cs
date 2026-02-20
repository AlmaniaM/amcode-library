using System.IO;
using System.Threading.Tasks;
using AMCode.Exports.DataSource;
using AMCode.Exports.Storage;

namespace AMCode.Exports
{
    /// <summary>
    /// A class designed to host the results of an export.
    /// </summary>
    public class DataSourceExportResult : IExportResult
    {
        private readonly IStreamDataSourceAsync dataSource;

        /// <summary>
        /// Create an instance of the <see cref="DataSourceExportResult"/> class.
        /// </summary>
        public DataSourceExportResult() : this(new MemoryStreamDataSource()) { }

        /// <summary>
        /// Create an instance of the <see cref="DataSourceExportResult"/> class.
        /// </summary>
        /// <param name="dataSource">Provide an <see cref="IStreamDataSourceAsync"/> object for storing <see cref="Stream"/>s.</param>
        public DataSourceExportResult(IStreamDataSourceAsync dataSource)
        {
            this.dataSource = dataSource;
        }

        /// <inheritdoc/>
        public int Count { get; set; }

        /// <inheritdoc/>
        public FileType FileType { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public void Dispose() => dataSource?.Dispose();

        /// <inheritdoc/>
        public async Task<Stream> GetDataAsync() => await dataSource.GetStreamAsync();

        /// <inheritdoc/>
        public async Task SetDataAsync(Stream data) => await dataSource.SetStreamAsync(data);
    }
}
