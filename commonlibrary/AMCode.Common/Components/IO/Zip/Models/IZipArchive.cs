using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.IO.Zip.Models;

namespace AMCode.Common.IO.Zip
{
    /// <summary>
    /// An interface designed to compile a zip archive.
    /// </summary>
    public interface IZipArchive
    {
        /// <summary>
        /// Set the <see cref="CompressionLevel"/> to use for the zip. Default is <see cref="CompressionLevel.Fastest"/>
        /// </summary>
        CompressionLevel Compression { get; set; }

        /// <summary>
        /// The <see cref="IList{T}"/> collection of the available <see cref="IZipEntry"/> objects.
        /// </summary>
        IList<IZipEntry> ZipEntries { get; set; }

        /// <summary>
        /// Add a zip entry.
        /// </summary>
        /// <param name="zipEntry">The <see cref="IZipEntry"/> to add</param>
        void AddEntry(IZipEntry zipEntry);

        /// <summary>
        /// Add a zip entry.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="data">The <see cref="Stream"/> data.</param>
        void AddEntry(string name, Stream data);

        /// <summary>
        /// Add a zip entry.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="fileInfo">The <see cref="FileInfo"/> of the file to add.</param>
        void AddEntry(string name, FileInfo fileInfo);

        /// <summary>
        /// Add a zip entry.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="getDataFunc">The <see cref="Func{TResult}"/> of type <see cref="Task{TResult}"/> <see cref="Stream"/> to fetch
        /// the data with.</param>
        void AddEntry(string name, Func<Task<Stream>> getDataFunc);

        /// <summary>
        /// Creates a zip from the added zip entries.
        /// </summary>
        /// <param name="fileName">The name to give the zipped file</param>
        IZipArchiveResult CreateZip(string fileName);

        /// <summary>
        /// Creates a zip from the added zip entries and copies it to the provided stream.
        /// </summary>
        /// <param name="fileName">The name to give the zipped file</param>
        /// <param name="stream">The <see cref="Stream"/> to copy the zip file to.</param>
        IZipArchiveResult CreateZip(string fileName, Stream stream);

        /// <summary>
        /// Creates a zip from the added zip entries.
        /// </summary>
        /// <param name="fileName">The name to give the zipped file</param>
        /// <param name="cancellationToken">Provide an optional <see cref="CancellationToken"/> for canceling the operation.</param>
        Task<IZipArchiveResult> CreateZipAsync(string fileName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a zip from the added zip entries and copies it to the provided stream.
        /// </summary>
        /// <param name="fileName">The name to give the zipped file</param>
        /// <param name="stream">The <see cref="Stream"/> to copy the zip file to.</param>
        /// <param name="cancellationToken">Provide an optional <see cref="CancellationToken"/> for canceling the operation.</param>
        Task<IZipArchiveResult> CreateZipAsync(string fileName, Stream stream, CancellationToken cancellationToken = default);
    }
}