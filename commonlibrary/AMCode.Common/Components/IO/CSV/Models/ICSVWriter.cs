using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace AMCode.Common.IO.CSV.Models
{
    /// <summary>
    /// An interface representing a CSV file writer.
    /// </summary>
    public interface ICSVWriter
    {
        /// <summary>
        /// Writes a <seealso cref="IList{T}"/> of <see cref="ExpandoObject"/>s to a filePath.
        /// </summary>
        /// <param name="expandoList">The <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s that contain the data to write
        /// to the list of files.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv(IList<ExpandoObject> expandoList, string filePath);

        /// <summary>
        /// Writes a <seealso cref="IList{T}"/> of <see cref="ExpandoObject"/>s to a filePath.
        /// </summary>
        /// <param name="expandoList">The <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s that contain the data to write
        /// to the list of files.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="headers">A list of headers you want to set instead of the default which uses the first object properties.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv(IList<ExpandoObject> expandoList, string filePath, IList<string> headers);

        /// <summary>
        /// Writes a <seealso cref="IList{T}"/> of <see cref="ExpandoObject"/>s to a filePath.
        /// </summary>
        /// <param name="expandoList">The <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s that contain the data to write
        /// to the list of files.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter);

        /// <summary>
        /// Writes a <seealso cref="IList{T}"/> of <see cref="ExpandoObject"/>s to a filePath.
        /// </summary>
        /// <param name="expandoList">The <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s that contain the data to write
        /// to the list of files.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter, QuoteOption quoteOption);

        /// <summary>
        /// Writes a <seealso cref="IList{T}"/> of <see cref="ExpandoObject"/>s to a filePath.
        /// </summary>
        /// <param name="expandoList">The <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s that contain the data to write
        /// to the list of files.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <param name="alternateHeaders">Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="string"/>
        /// values. the keys should represent the keys in the provided <see cref="List{T}"/> for each <see cref="ExpandoObject"/>. The
        /// values should be the header name you want to be placed in the CSV file.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders);

        /// <summary>
        /// Writes a <seealso cref="IList{T}"/> of <see cref="ExpandoObject"/>s to a filePath.
        /// </summary>
        /// <param name="expandoList">The <see cref="IList{T}"/> of <see cref="ExpandoObject"/>s that contain the data to write
        /// to the list of files.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <param name="alternateHeaders">Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="string"/>
        /// values. the keys should represent the keys in the provided <see cref="List{T}"/> for each <see cref="ExpandoObject"/>. The
        /// values should be the header name you want to be placed in the CSV file.</param>
        /// <param name="headers">A list of headers you want to set instead of the default which uses the first object properties.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv(IList<ExpandoObject> expandoList, string filePath, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders, IList<string> headers);

        /// <summary>
        /// Writes a CSV to a provided <see cref="Stream"/> from a <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s.
        /// </summary>
        /// <param name="expandoList">the <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s that contains the data to write
        /// to the list of files.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        void CreateCsv(IList<ExpandoObject> expandoList, Stream stream);

        /// <summary>
        /// Writes a CSV to a provided <see cref="Stream"/> from a <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s.
        /// </summary>
        /// <param name="expandoList">the <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s that contains the data to write
        /// to the list of files.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="headers">A list of headers you want to set instead of the default which uses the first object properties.</param>
        void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, IList<string> headers);

        /// <summary>
        /// Writes a CSV to a provided <see cref="Stream"/> from a <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s.
        /// </summary>
        /// <param name="expandoList">the <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s that contains the data to write
        /// to the list of files.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter);

        /// <summary>
        /// Writes a CSV to a provided <see cref="Stream"/> from a <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s.
        /// </summary>
        /// <param name="expandoList">the <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s that contains the data to write
        /// to the list of files.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter, QuoteOption quoteOption);

        /// <summary>
        /// Writes a CSV to a provided <see cref="Stream"/> from a <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s.
        /// </summary>
        /// <param name="expandoList">the <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s that contains the data to write
        /// to the list of files.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <param name="alternateHeaders">Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="string"/>
        /// values. The keys should represent the keys in the provided <see cref="List{T}"/> for each <see cref="ExpandoObject"/>. The
        /// values should be the header name you want to be placed in the CSV file.</param>
        void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders);

        /// <summary>
        /// Writes a CSV to a provided <see cref="Stream"/> from a <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s.
        /// </summary>
        /// <param name="expandoList">the <seealso cref="IList{T}"/> <see cref="ExpandoObject"/>s that contains the data to write
        /// to the list of files.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <param name="alternateHeaders">Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="string"/>
        /// values. The keys should represent the keys in the provided <see cref="List{T}"/> for each <see cref="ExpandoObject"/>. The
        /// values should be the header name you want to be placed in the CSV file.</param>
        /// <param name="headers">A list of headers you want to set instead of the default which uses the first object properties.</param>
        void CreateCsv(IList<ExpandoObject> expandoList, Stream stream, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders, IList<string> headers);

        /// <summary>
        /// Create a CSV file from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv<T>(IList<T> objList, string filePath) where T : new();

        /// <summary>
        /// Create a CSV file from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv<T>(IList<T> objList, string filePath, string delimiter) where T : new();

        /// <summary>
        /// Create a CSV file from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv<T>(IList<T> objList, string filePath, string delimiter, QuoteOption quoteOption) where T : new();

        /// <summary>
        /// Create a CSV file from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="filePath">The <see cref="string"/> path of the file you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <param name="alternateHeaders">Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="string"/>
        /// values. The keys should represent the keys in the provided <see cref="List{T}"/> for each <see cref="ExpandoObject"/>. The
        /// values should be the header name you want to be placed in the CSV file.</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        string CreateCsv<T>(IList<T> objList, string filePath, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders) where T : new();

        /// <summary>
        /// Create a CSV file from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        void CreateCsv<T>(IList<T> objList, Stream stream) where T : new();

        /// <summary>
        /// Create a CSV file from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        void CreateCsv<T>(IList<T> objList, Stream stream, string delimiter) where T : new();

        /// <summary>
        /// Create a CSV file from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values</param>
        /// <returns>The <see cref="string"/> file path to the saved CSV.</returns>
        void CreateCsv<T>(IList<T> objList, Stream stream, string delimiter, QuoteOption quoteOption) where T : new();

        /// <summary>
        /// Create a CSV from a <see cref="List{T}"/> of <typeparamref name="T"/> objects.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object each row in the CSV will be built from.</typeparam>
        /// <param name="objList">The <typeparamref name="T"/> <see cref="List{T}"/> to write to the file.</param>
        /// <param name="stream">The <see cref="Stream"/> you want to write.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="quoteOption">A <see cref="QuoteOption"/> value. When set to <see cref="QuoteOption.AddQuotes"/> then
        /// every value will be surrounded with double quotes. When set to <see cref="QuoteOption.Auto"/> then only values
        /// that have a character like the provided delimiter will have double quotes surrounding them.</param>
        /// <param name="alternateHeaders">Provide a <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="string"/>
        /// values. The keys should represent the keys in the provided <see cref="List{T}"/> for each <see cref="ExpandoObject"/>. The
        /// values should be the header name you want to be placed in the CSV file.</param>
        void CreateCsv<T>(IList<T> objList, Stream stream, string delimiter, QuoteOption quoteOption, IDictionary<string, string> alternateHeaders) where T : new();

        /// <summary>
        /// Save a CSV file without as a UTF-8 encoding.
        /// </summary>
        /// <param name="fileName">The file to read and save again.</param>
        void SaveAsUTF8WithoutByteOrderMark(string fileName);

        /// <summary>
        /// Save a CSV file without as a UTF-8 encoding.
        /// </summary>
        /// <param name="fileName">The file to read and save again.</param>
        /// <param name="encoding">The <see cref="Encoding"/> to use.</param>
        void SaveAsUTF8WithoutByteOrderMark(string fileName, Encoding encoding);
    }
}