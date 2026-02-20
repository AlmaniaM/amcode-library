using System.IO;
using System.Threading.Tasks;
using AMCode.Exports.Storage;

namespace AMCode.Exports.DataSource
{
    /// <summary>
    /// An interface designed to create <see cref="IStreamDataSourceAsync"/> objects.
    /// </summary>
    public interface IExportStreamDataSourceFactory
    {
        /// <summary>
        /// Create an <see cref="IStreamDataSourceAsync"/> object.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to store.</param>
        /// <returns>A <see cref="Task{TResult}"/> of an <see cref="IStreamDataSourceAsync"/> object.</returns>
        Task<IStreamDataSourceAsync> CreateAsync(Stream stream);
    }
}
