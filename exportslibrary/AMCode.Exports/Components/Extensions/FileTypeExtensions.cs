using System;
using AMCode.Common.Util;

namespace AMCode.Exports.Extensions
{
    /// <summary>
    /// An extension method class for the <see cref="FileType"/> enum.
    /// </summary>
    public static class FileTypeExtensions
    {
        /// <summary>
        /// Get the file extension for the given <see cref="FileType"/> value.
        /// </summary>
        /// <param name="fileType">The <see cref="FileType"/> value to create a file extension for.</param>
        /// <returns>A <see cref="string"/> file extension with period (.) included.</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid <see cref="FileType"/> value is passed.</exception>
        public static string CreateFileExtension(this FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Csv:
                    return ".csv";
                case FileType.Xlsx:
                    return ".xlsx";
                case FileType.Zip:
                    return ".zip";
                default:
                    var header = ExceptionUtil.CreateExceptionHeader<FileType, string>(CreateFileExtension);
                    throw new ArgumentException($"{header} Error: Invalid {nameof(FileType)} value. The value \"{fileType}\" is not a valid {nameof(FileType)} value.");
            }
        }
    }
}