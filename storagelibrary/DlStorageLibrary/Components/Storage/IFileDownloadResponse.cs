using System.IO;

namespace DemandLink.Storage
{
    /// <summary>
    /// An interface designed to represent the results of downloading a file.
    /// </summary>
    public interface IFileDownloadResponse
    {
        /// <summary>
        /// The contents of the file as a <see cref="Stream"/>.
        /// </summary>
        Stream Content { get; }

        /// <summary>
        /// The original file name.
        /// </summary>
        string FileName { get; }
    }
}