using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AMCode.Common.IO.JSON
{
    /// <summary>
    /// A class designed to write objects to JSON files.
    /// </summary>
    public class JsonFileWriter
    {
        /// <summary>
        /// Write an object to the given file path.
        /// </summary>
        /// <param name="filePath">A <see cref="string"/> file path with the file name.</param>
        /// <param name="o">Any <see cref="ISerializable"/> object.</param>
        public static void Write(string filePath, object o)
        {
            try
            {
                var json = JsonConvert.SerializeObject(o, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously write an object to the given file path.
        /// </summary>
        /// <param name="filePath">A <see cref="string"/> file path with the file name.</param>
        /// <param name="o">Any <see cref="ISerializable"/> object.</param>
        /// <returns>A <see cref="Task"/></returns>
        public static Task WriteAsync(string filePath, object o) => Task.Run(() => Write(filePath, o));
    }
}