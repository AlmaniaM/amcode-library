using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMCode.Common.IO.Zip.Models;
using ZipLib = System.IO.Compression;

namespace AMCode.Common.IO.Zip
{
    /// <summary>
    /// A class designed for storing <see cref="Stream"/>s and zipping them up into one zip <see cref="Stream"/>.
    /// </summary>
    public class ZipArchive : IZipArchive
    {
        private readonly int streamCopyBufferSize = 81920; // Default Stream.CopyToAsync buffer size

        /// <summary>
        /// Create an instance of the <see cref="ZipArchive"/> class.
        /// </summary>
        public ZipArchive()
        {
            ZipEntries = new List<IZipEntry>();
        }

        /// <summary>
        /// Create an instance of the <see cref="ZipArchive"/> class.
        /// </summary>
        /// <param name="streamCopyBufferSize">The size in bytes the data should be copied at a time from stream to stream. Set to zero or less to copy whole streams at a time.</param>
        public ZipArchive(int streamCopyBufferSize)
            : this()
        {
            this.streamCopyBufferSize = streamCopyBufferSize;
        }

        /// <summary>
        /// Create an instance of the <see cref="ZipArchive"/> class.
        /// </summary>
        /// <param name="zipEntries">Provide an <see cref="IEnumerable{T}"/> collection of <see cref="IZipEntry"/>s to create a zip from.</param>
        public ZipArchive(IEnumerable<IZipEntry> zipEntries)
        {
            ZipEntries = zipEntries.ToList();
        }

        /// <summary>
        /// Create an instance of the <see cref="ZipArchive"/> class.
        /// </summary>
        /// <param name="zipEntries">Provide an <see cref="IEnumerable{T}"/> collection of <see cref="IZipEntry"/>s to create a zip from.</param>
        /// <param name="streamCopyBufferSize">The size in bytes the data should be copied at a time from stream to stream. Set to zero or less to copy whole streams at a time.</param>
        public ZipArchive(IEnumerable<IZipEntry> zipEntries, int streamCopyBufferSize)
            : this(zipEntries.ToList())
        {
            this.streamCopyBufferSize = streamCopyBufferSize;
        }

        /// <inheritdoc/>
        public CompressionLevel Compression { get; set; } = (CompressionLevel)ZipLib.CompressionLevel.Fastest;

        /// <inheritdoc/>
        public IList<IZipEntry> ZipEntries { get; set; }

        /// <inheritdoc/>
        public void AddEntry(IZipEntry zipEntry)
            => ZipEntries.Add(new ZipEntry
            {
                Data = zipEntry.Data,
                File = zipEntry.File,
                Name = zipEntry.Name
            });

        /// <inheritdoc/>
        public void AddEntry(string name, Stream data)
            => ZipEntries.Add(new ZipEntry
            {
                Data = data,
                Name = name
            });

        /// <inheritdoc/>
        public void AddEntry(string name, FileInfo fileInfo)
            => ZipEntries.Add(new ZipEntry
            {
                File = fileInfo,
                Name = name,
            });

        /// <inheritdoc/>
        public void AddEntry(string name, Func<Task<Stream>> getDataFunc)
            => ZipEntries.Add(new ZipEntry
            {
                GetDataAsync = getDataFunc,
                Name = name,
            });

        /// <inheritdoc/>
        public IZipArchiveResult CreateZip(string fileName)
            => CreateZipAsync(fileName).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public IZipArchiveResult CreateZip(string fileName, Stream stream)
            => CreateZipAsync(fileName, stream).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<IZipArchiveResult> CreateZipAsync(string fileName, CancellationToken cancellationToken = default)
            => await createZipAsync(fileName, new MemoryStream(), cancellationToken);

        /// <inheritdoc/>
        public async Task<IZipArchiveResult> CreateZipAsync(string fileName, Stream stream, CancellationToken cancellationToken = default)
            => await createZipAsync(fileName, stream, cancellationToken);

        private async Task<IZipArchiveResult> createZipAsync(string fileName, Stream zipArchiveStream, CancellationToken cancellationToken = default)
        {
            if (ZipEntries is null || ZipEntries.Count == 0)
            {
                return default;
            }

            using (var zipArchive = new ZipLib.ZipArchive(zipArchiveStream, ZipLib.ZipArchiveMode.Create, true))
            {
                for (var i = 0; i < ZipEntries.Count; i++)
                {
                    var zipEntry = ZipEntries.ElementAt(i);

                    using (var zipEntryStream = zipArchive.CreateEntry(zipEntry.Name, (ZipLib.CompressionLevel)Compression).Open())
                    {
                        if (zipEntry.GetDataAsync != null)
                        {
                            using (var dataStream = await zipEntry.GetDataAsync())
                            {
                                await dataStream.CopyToAsync(zipEntryStream, streamCopyBufferSize, cancellationToken);
                            }
                        }
                        else if (zipEntry.Data != null)
                        {
                            await zipEntry.Data.CopyToAsync(zipEntryStream, streamCopyBufferSize, cancellationToken);
                        }
                        else
                        {
                            using (var fileStream = new FileStream(zipEntry.File.FullName, FileMode.Open, FileAccess.Read))
                            {
                                await fileStream.CopyToAsync(zipEntryStream, streamCopyBufferSize, cancellationToken);
                            }
                        }
                    }
                }
            }

            await zipArchiveStream.FlushAsync();
            zipArchiveStream.Seek(0, SeekOrigin.Begin);

            var extensionExists = Path.GetExtension(fileName).ToLowerInvariant().Equals(".zip");
            var zipFileName = extensionExists ? fileName : $"{fileName}.zip";

            return new ZipArchiveResult
            {
                Data = zipArchiveStream,
                Name = zipFileName
            };
        }
    }
}