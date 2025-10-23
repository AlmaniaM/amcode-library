using System.IO;

namespace DemandLink.Storage
{
    /// <summary>
    /// A class that represents a response from a file download 
    /// </summary>
    public class DefaultFileDownloadResponse : IFileDownloadResponse
    {
        /// <inheritdoc/>
        public Stream Content { get; set; }

        /// <inheritdoc/>
        public string FileName { get; set; }
    }
}