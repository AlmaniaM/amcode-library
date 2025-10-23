using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO.CSV.Models;

namespace AMCode.Common.IO.CSV
{
    /// <summary>
    /// A class designed to write collections of data to a CSV file.
    /// </summary>
    public class CSVWriter : ICSVWriter
    {
        /// <summary>
        /// The buffer size used by the internal stream writer.
        /// </summary>
        public const int BUFFER_SIZE = 1024;

        /// <inheritdoc/>
        public string CreateCsv(IList<ExpandoObject> expandoList, string filePath)
            => CreateCsv(expandoList, filePath, ",", QuoteOption.Auto, null);

        /// <inheritdoc/>
        public string CreateCsv(IList<ExpandoObject> expandoList, string filePath, IList<string> headers)
            => CreateCsv(expandoList, filePath, ",", QuoteOption.Auto, null, headers);

        /// <inheritdoc/>
        public string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter)
            => CreateCsv(expandoList, filePath, delimiter, QuoteOption.Auto, null);

        /// <inheritdoc/>
        public string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter, QuoteOption quoteOption)
            => CreateCsv(expandoList, filePath, delimiter, quoteOption, null);

        /// <inheritdoc/>
        public string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders)
            => CreateCsv(expandoList, filePath, delimiter, quoteOption, alternateHeaders, null);

        /// <inheritdoc/>
        public string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders, IList<string> headers)
        {
            using (var sw = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                CreateCsv(expandoList, sw, delimiter, quoteOption, alternateHeaders, headers);
            }

            return filePath;
        }

        /// <inheritdoc/>
        public void CreateCsv(IList<ExpandoObject> expandoList, Stream stream)
            => CreateCsv(expandoList, stream, ",", QuoteOption.Auto, null);

        /// <inheritdoc/>
        public void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, IList<string> headers)
            => CreateCsv(expandoList, stream, ",", QuoteOption.Auto, null, headers);

        /// <inheritdoc/>
        public void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter)
            => CreateCsv(expandoList, stream, delimiter, QuoteOption.Auto, null);

        /// <inheritdoc/>
        public void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter, QuoteOption quoteOption)
            => CreateCsv(expandoList, stream, delimiter, quoteOption, null);

        /// <inheritdoc/>
        public void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders)
            => CreateCsv(expandoList, stream, delimiter, quoteOption, alternateHeaders, null);

        /// <inheritdoc/>
        public void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders, IList<string> headers)
        {
            var expandoObj = expandoList.First();

            using (var sw = new StreamWriter(stream, Encoding.Default, BUFFER_SIZE, true))
            {
                var headerString = getMappedColumnsAsString(alternateHeaders, headers ?? expandoObj.Keys(), delimiter);
                sw.WriteLine(headerString);

                for (var i = 0; i <= expandoList.Count() - 1; i++)
                {
                    var expando = expandoList[i];
                    object[] values = expando.Values().Select(value =>
                    {
                        var quote = getStringQuote(quoteOption, value.ToString(), delimiter);
                        return $"{quote}{value}{quote}";
                    }).ToArray();
                    sw.WriteLine(string.Join(delimiter, values));
                }

                sw.Flush();
            }
        }

        /// <inheritdoc/>
        public string CreateCsv<T>(IList<T> objList, string filePath)
            where T : new()
            => CreateCsv(objList, filePath, ",", QuoteOption.Auto);

        /// <inheritdoc/>
        public string CreateCsv<T>(IList<T> objList, string filePath, string delimiter)
            where T : new()
            => CreateCsv(objList, filePath, delimiter, QuoteOption.Auto);

        /// <inheritdoc/>
        public string CreateCsv<T>(IList<T> objList, string filePath, string delimiter, QuoteOption quoteOption)
            where T : new()
            => CreateCsv<T>(objList, filePath, delimiter, quoteOption, null);

        /// <inheritdoc/>
        public string CreateCsv<T>(IList<T> objList, string filePath, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders) where T : new()
        {
            using (var sw = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write))
            {
                CreateCsv(objList, sw, delimiter, quoteOption, alternateHeaders);
            }

            return filePath;
        }

        /// <inheritdoc/>
        public void CreateCsv<T>(IList<T> objList, Stream stream)
            where T : new()
            => CreateCsv(objList, stream, ",", QuoteOption.Auto);

        /// <inheritdoc/>
        public void CreateCsv<T>(IList<T> objList, Stream stream, string delimiter)
            where T : new()
            => CreateCsv(objList, stream, delimiter, QuoteOption.Auto);

        /// <inheritdoc/>
        public void CreateCsv<T>(IList<T> objList, Stream stream, string delimiter, QuoteOption quoteOption)
            where T : new()
            => CreateCsv<T>(objList, stream, delimiter, quoteOption, null);

        /// <inheritdoc/>
        public void CreateCsv<T>(IList<T> objList, Stream stream, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders) where T : new()
        {
            var tObject = objList.First();

            using (var sw = new StreamWriter(stream, Encoding.Default, BUFFER_SIZE, true))
            {
                IList<PropertyInfo> propertyInfoList = tObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToList();

                var headerString = getMappedColumnsAsString(alternateHeaders, propertyInfoList.Select(x => $"{x.Name}").ToList(), delimiter);
                sw.WriteLine(headerString);

                for (var i = 0; i <= objList.Count() - 1; i++)
                {
                    var currObject = objList[i];
                    object[] values = propertyInfoList.Select((PropertyInfo pi) =>
                    {
                        var value = pi.GetValue(currObject);
                        var quote = getStringQuote(quoteOption, value.ToString(), delimiter);
                        return $"{quote}{value}{quote}";
                    }).ToArray();
                    sw.WriteLine(string.Join(delimiter, values));
                }

                sw.Flush();
            }
        }

        /// <inheritdoc/>
        public void SaveAsUTF8WithoutByteOrderMark(string fileName)
            => SaveAsUTF8WithoutByteOrderMark(fileName, null);

        /// <inheritdoc/>
        public void SaveAsUTF8WithoutByteOrderMark(string fileName, Encoding encoding)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(
                    $"[{nameof(CSVWriter)}][{nameof(SaveAsUTF8WithoutByteOrderMark)}]({nameof(fileName)}, {nameof(encoding)}) Error: The parameter {nameof(fileName)} is null."
                );
            }

            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            File.WriteAllText(fileName, File.ReadAllText(fileName, encoding), new UTF8Encoding(false));
        }

        /// <summary>
        /// Maps the current headers to alternate headers if provided.
        /// </summary>
        /// <param name="alternateHeaders">Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="string"/>
        /// values. The keys should represent the keys in the provided <see cref="List{T}"/> for each <see cref="ExpandoObject"/>. The
        /// values should be the header name you want to be placed in the CSV file.</param>
        /// <param name="headers">The original list of header names.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <returns>A <see cref="string"/> joined by the provided delimiter.</returns>
        private string getMappedColumnsAsString(IDictionary<string, string> alternateHeaders, IList<string> headers, string delimiter)
        {
            if (alternateHeaders == null)
            {
                return string.Join(delimiter, headers.Select(x => $"{x}"));
            }

            return string.Join(delimiter, headers.Select(x =>
            {
                alternateHeaders.TryGetValue(x, out var newHeader);
                if (newHeader == null)
                {
                    throw new Exception("No alternate header provided for key: {x}.");
                }

                return $"{newHeader}";
            }));
        }

        /// <summary>
        /// Adds a quote if the value has a delimiter in it when the <see cref="QuoteOption.Auto"/> is provided.
        /// If <see cref="QuoteOption.AddQuotes"/> is provided then an extra quote will be included for all values.
        /// </summary>
        /// <param name="quoteOption">The desired <see cref="QuoteOption"/>.</param>
        /// <param name="value">The <see cref="string"/> value to evaluate.</param>
        /// <param name="delimiter">The <see cref="string"/> delimiter to check for.</param>
        /// <returns>A <see cref="string"/> that may or may not have an escaped quote inside.</returns>
        private string getStringQuote(QuoteOption quoteOption, string value, string delimiter)
        {
            var quote = string.Empty;

            if (quoteOption == QuoteOption.AddQuotes)
            {
                quote = "\"";
            }
            else if (quoteOption == QuoteOption.Auto)
            {
                quote = value.IndexOf(delimiter) > -1 ? "\"" : string.Empty;
            }

            return quote;
        }
    }
}