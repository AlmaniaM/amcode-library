using System.IO;

namespace AMCode.Common.IO.Zip.Models
{
    /// <summary>
    /// A class designed to store the results of a zip archive.
    /// </summary>
    public class ZipArchiveResult : IZipArchiveResult
    {
        /// <inheritdoc/>
        public Stream Data { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <summary>
        /// Dispose of the <see cref="Data"/> property.
        /// </summary>
        public void Dispose() => Data?.Dispose();
    }
}