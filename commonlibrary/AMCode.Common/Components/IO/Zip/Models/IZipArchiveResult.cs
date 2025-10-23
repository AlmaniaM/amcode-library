using System;
using System.IO;

namespace AMCode.Common.IO.Zip.Models
{
    /// <summary>
    /// An interface designed to store the results of a zip archive.
    /// </summary>
    public interface IZipArchiveResult : IDisposable
    {
        /// <summary>
        /// The zip data.
        /// </summary>
        Stream Data { get; set; }

        /// <summary>
        /// The name of the file.
        /// </summary>
        string Name { get; set; }
    }
}