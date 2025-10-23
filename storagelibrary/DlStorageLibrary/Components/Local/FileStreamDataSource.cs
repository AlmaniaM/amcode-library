using System.IO;

namespace DemandLink.Storage.Local
{
    /// <summary>
    /// A class designed to store <see cref="Stream"/>s as files locally.
    /// </summary>
    public class FileStreamDataSource : BaseStreamDataSource, IStreamDataSourceAsync
    {
        /// <summary>
        /// Create an instance of the <see cref="FileStreamDataSource"/> class.
        /// </summary>
        public FileStreamDataSource() : this(Directory.GetCurrentDirectory()) { }

        /// <summary>
        /// Create an instance of the <see cref="FileStreamDataSource"/> class.
        /// </summary>
        /// <param name="directory">Provide the absolute directory path where to store the files.</param>
        public FileStreamDataSource(string directory) : base(new SimpleLocalStorage(directory)) { }
    }
}