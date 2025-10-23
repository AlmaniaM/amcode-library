using System.IO;

namespace AMCode.Exports.Extensions
{
    /// <summary>
    /// An extension method class for the <see cref="IExportResult"/> object.
    /// </summary>
    public static class ExportResultExtensions
    {
        /// <summary>
        /// Create a file name from the given <see cref="IExportResult"/> object.
        /// </summary>
        /// <param name="exportBookResult">The <see cref="IExportResult"/> object to build the file name from.</param>
        /// <returns>A <see cref="string"/> file name.</returns>
        public static string CreateFileName(this IExportResult exportBookResult)
            => CreateFileName(exportBookResult, string.Empty);

        /// <summary>
        /// Create a file name from the given <see cref="IExportResult"/> object.
        /// </summary>
        /// <param name="exportBookResult">The <see cref="IExportResult"/> object to build the file name from.</param>
        /// <param name="appendText">Any extra text you want to append.</param>
        /// <returns>A <see cref="string"/> file name.</returns>
        public static string CreateFileName(this IExportResult exportBookResult, string appendText)
        {
            var extensionName = exportBookResult.FileType.CreateFileExtension();
            var fileName = Path.GetFileNameWithoutExtension(exportBookResult.Name);
            return $"{fileName}{appendText}{extensionName}";
        }
    }
}