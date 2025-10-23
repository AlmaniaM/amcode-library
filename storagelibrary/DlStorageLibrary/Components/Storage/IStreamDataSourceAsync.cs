using System;
using System.IO;
using System.Threading.Tasks;

namespace DemandLink.Storage
{
    /// <summary>
    /// An interface designed to simply store and retrieve <see cref="Stream"/>s and treat them as values. If a value is disposed then
    /// it's impossible to retrieve.
    /// </summary>
    public interface IStreamDataSourceAsync : IDisposable
    {
        /// <summary>
        /// Get the stored <see cref="Stream"/> data.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> of type <see cref="Stream"/>.</returns>
        Task<Stream> GetStreamAsync();

        /// <summary>
        /// Store the provided <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> you want to store.</param>
        /// <returns>A void <see cref="Task"/>.</returns>
        Task SetStreamAsync(Stream stream);
    }
}