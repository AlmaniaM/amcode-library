using System.Collections.Generic;
using AMCode.Common.IO.Zip;

namespace AMCode.Exports.Zip
{
    /// <summary>
    /// A factory class for creating an <see cref="IZipArchive"/> object.
    /// </summary>
    public class ZipArchiveFactory : IZipArchiveFactory
    {
        /// <inheritdoc/>
        public IZipArchive Create() => new ZipArchive(16000);

        /// <inheritdoc/>
        public IZipArchive Create(IEnumerable<IZipEntry> zipEntries) => new ZipArchive(zipEntries, 16000);
    }
}