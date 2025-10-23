using System.IO;

namespace DlExportsLibrary.SharedTestLibrary.Extensions
{
    /// <summary>
    /// Contains <see cref="Stream"/> method extensions.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Read the contents of a <see cref="Stream"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read.</param>
        /// <returns>The contents of the provided <see cref="Stream"/> as a <see cref="string"/>.</returns>
        public static string GetString(this Stream stream)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}