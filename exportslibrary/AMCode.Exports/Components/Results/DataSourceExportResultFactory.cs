using System.IO;
using System.Threading.Tasks;
using AMCode.Exports.DataSource;
using AMCode.Storage;

namespace AMCode.Exports.Results
{
    /// <summary>
    /// A class designed to represent a factory for building <see cref="IExportResult"/> objects.
    /// </summary>
    public class DataSourceExportResultFactory : IExportResultFactory
    {
        private readonly IExportStreamDataSourceFactory dataSourceFactory;

        /// <summary>
        /// Create an instance of the <see cref="DataSourceExportResultFactory"/> class.
        /// </summary>
        public DataSourceExportResultFactory() : this(new MemoryStreamDataSourceFactory()) { }

        /// <inheritdoc cref="DataSourceExportResultFactory()"/>
        /// <param name="dataSourceFactory">Provide an <see cref="IExportStreamDataSourceFactory"/> for constructing <see cref="IStreamDataSourceAsync"/> objects.</param>
        public DataSourceExportResultFactory(IExportStreamDataSourceFactory dataSourceFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
        }

        /// <inheritdoc/>
        public Task<IExportResult> CreateAsync(IStreamDataSourceAsync dataSource, FileType fileType, string fileName, int count)
        {
            var exportResult = new DataSourceExportResult(dataSource)
            {
                Count = count,
                FileType = fileType,
                Name = fileName,
            };

            return Task.FromResult<IExportResult>(exportResult);
        }

        /// <inheritdoc/>
        public async Task<IExportResult> CreateAsync(Stream stream, FileType fileType, string fileName, int count)
        {
            var dataSource = await dataSourceFactory.CreateAsync(stream);

            return new DataSourceExportResult(dataSource)
            {
                Count = count,
                FileType = fileType,
                Name = fileName,
            };
        }
    }
}