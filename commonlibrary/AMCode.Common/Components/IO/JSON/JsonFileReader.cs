using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AMCode.Common.IO.JSON
{
    /// <summary>
    /// A class designed to read JSON files into objects.
    /// </summary>
    public class JsonFileReader
    {
        /// <summary>
        /// Read a JSON file and de-serialize the contents into the provided
        /// object type.
        /// </summary>
        /// <typeparam name="T">The type of object to de-serialize the JSON content into.</typeparam>
        /// <param name="filePath">The <see cref="string"/> path to the JSON file.</param>
        /// <returns>An object of type <typeparamref name="T"/></returns>
        public static T Read<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException();
                }

                using (var stream = new StreamReader(filePath))
                {
                    var content = stream.ReadToEnd();

                    var tObj = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    return tObj;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Asynchronously read a JSON file and de-serialize the contents into the provided
        /// object type.
        /// </summary>
        /// <typeparam name="T">The type of object to de-serialize the JSON content into.</typeparam>
        /// <param name="filePath">The <see cref="string"/> path to the JSON file.</param>
        /// <returns>A <see cref="Task"/> object of type <typeparamref name="T"/></returns>
        public static async Task<T> ReadAsync<T>(string filePath) => await Task.Run(() => Read<T>(filePath));
    }
}