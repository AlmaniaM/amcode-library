using System.Collections.Generic;
using AMCode.Common.IO.Zip;

namespace AMCode.Exports.Zip
{
    /// <summary>
    /// A factory interface for creating an <see cref="IZipArchive"/> object.
    /// </summary>
    public interface IZipArchiveFactory
    {
        /// <summary>
        /// Create an <see cref="IZipArchive"/> object.
        /// </summary>
        /// <returns>An <see cref="IZipArchive"/> object.</returns>
        IZipArchive Create();

        /// <summary>
        /// Create an <see cref="IZipArchive"/> object.
        /// </summary>
        /// <param name="zipEntries">An <see cref="IEnumerable{T}"/> collection of <see cref="IZipEntry"/> objects.</param>
        /// <returns>An <see cref="IZipArchive"/> object.</returns>
        IZipArchive Create(IEnumerable<IZipEntry> zipEntries);
    }
}