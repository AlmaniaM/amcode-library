using System.IO;
using System.Threading.Tasks;
using AMCode.Storage;
using AMCode.Storage.Local;

namespace AMCode.Exports.DataSource
{
    /// <summary>
    /// A class designed to create instances of the <see cref="FileStreamDataSource"/> class.
    /// </summary>
    public class FileStreamDataSourceFactory : IExportStreamDataSourceFactory
    {
        private readonly string directory;

        /// <summary>
        /// Create an instance of the <see cref="FileStreamDataSourceFactory"/> class.
        /// </summary>
        public FileStreamDataSourceFactory() : this(Directory.GetCurrentDirectory()) { }

        /// <summary>
        /// Create an instance of the <see cref="FileStreamDataSourceFactory"/> class.
        /// </summary>
        /// <param name="directory">The absolute path to the directory where you wish to store files in.</param>
        public FileStreamDataSourceFactory(string directory)
        {
            this.directory = directory;
        }

        /// <summary>
        /// Create an instance of the <see cref="FileStreamDataSource"/> and store the provided Stream.
        /// </summary>
        /// <inheritdoc/>
        public async Task<IStreamDataSourceAsync> CreateAsync(Stream stream)
        {
            var dataSource = new FileStreamDataSource(directory);
            await dataSource.SetStreamAsync(stream);
            return dataSource;
        }
    }
}