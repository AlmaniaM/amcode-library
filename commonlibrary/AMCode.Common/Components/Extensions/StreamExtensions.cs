using System.IO;
using System.Threading.Tasks;

namespace AMCode.Common.Extensions.Streams
{
    /// <summary>
    /// A static class for <see cref="Stream"/> extension methods.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Read the provided <see cref="Stream"/> into a <see cref="byte"/> array.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read.</param>
        /// <returns>A <see cref="byte"/> array.</returns>
        public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            var originalPosition = 0L;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    var array = ms.ToArray();
                    return array;
                }
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}