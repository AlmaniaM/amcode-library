using System;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Common.IO.Zip
{
    /// <summary>
    /// A class designed to act as a single <see cref="IZipArchive"/> entry.
    /// </summary>
    public class ZipEntry : IZipEntry
    {
        /// <inheritdoc/>
        public Stream Data { get; set; }

        /// <inheritdoc/>
        public FileInfo File { get; set; }

        /// <inheritdoc/>
        public Func<Task<Stream>> GetDataAsync { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }
    }
}