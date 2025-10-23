using System;
using System.IO;
using System.Threading.Tasks;

namespace AMCode.Common.IO.Zip
{
    /// <summary>
    /// An interface designed to act as a single <see cref="IZipArchive"/> entry.
    /// </summary>
    public interface IZipEntry
    {
        /// <summary>
        /// The data <see cref="Stream"/>.
        /// </summary>
        /// <remarks>
        /// If this is set then <see cref="File"/> will not be used.
        /// </remarks>
        Stream Data { get; set; }

        /// <summary>
        /// The <see cref="FileInfo"/> of the file to add.
        /// </summary>
        /// <remarks>
        /// If a <see cref="Data"/> is set then this value will not be used.
        /// </remarks>
        FileInfo File { get; set; }

        /// <summary>
        /// Get or set a function that will return the <see cref="Stream"/> of file data to zip.
        /// </summary>
        Func<Task<Stream>> GetDataAsync { get; set; }

        /// <summary>
        /// The name of the file.
        /// </summary>
        string Name { get; set; }
    }
}