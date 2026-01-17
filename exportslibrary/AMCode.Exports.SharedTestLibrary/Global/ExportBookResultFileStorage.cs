using System.IO;
using System.Threading.Tasks;
using AMCode.Exports;
using AMCode.Exports.Extensions;

namespace AMCode.Exports.SharedTestLibrary.Global
{
    public class ExportBookResultFileStorage : IExportResult
    {
        private readonly string directory;

        public ExportBookResultFileStorage(string directory)
        {
            this.directory = directory;
        }

        /// <inheritdoc/>
        public int Count { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        public FileType FileType { get; set; }

        /// <inheritdoc/>
        public void Dispose() => File.Delete(Path.Combine(directory, this.CreateFileName()));

        /// <summary>
        /// Get a <see cref="Stream"/> from a saved file.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> of type <see cref="Stream"/>.</returns>
        public async Task<Stream> GetDataAsync()
        {
            var fileName = this.CreateFileName();

            using var fileStream = new FileStream(Path.Combine(directory, fileName), FileMode.Open, FileAccess.Read);
            var stream = new MemoryStream();
            await fileStream.CopyToAsync(stream);
            await stream.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        /// <summary>
        /// Save a stream to a file.
        /// </summary>
        /// <param name="data">The <see cref="Stream"/> to save.</param>
        /// <returns>A void <see cref="Task"/>.</returns>
        public async Task SetDataAsync(Stream data)
        {
            var fileName = this.CreateFileName();
            var filePath = Path.Combine(directory, fileName);

            using var fileStream = File.Create(filePath);
            await data.CopyToAsync(fileStream);
            fileStream.Flush();
        }
    }
}