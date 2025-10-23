using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace AMCode.Common.IO.CSV.Models
{
    /// <summary>
    /// An interface representing a CSV file reader.
    /// </summary>
    public interface ICSVReader
    {
        /// <summary>
        /// Get a <see cref="string"/> <see cref="List{T}"/> of column values.
        /// </summary>
        /// <param name="filePath">An absolute <see cref="string"/> path to the file you want to read.</param>
        /// <param name="columnName">The column you want to read.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>'s.</returns>
        IList<string> GetColumnValues(string filePath, string columnName);

        /// <summary>
        /// Get a <see cref="string"/> <see cref="List{T}"/> of column values.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="columnName">The column you want to read.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>'s.</returns>
        IList<string> GetColumnValues(Stream stream, string columnName);

        /// <summary>
        /// Get a <see cref="string"/> <see cref="List{T}"/> of column values.
        /// </summary>
        /// <param name="filePath">An absolute <see cref="string"/> path to the file you want to read.</param>
        /// <param name="columnName">The column you want to read.</param>
        /// <param name="delimiter">The delimiter string.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>'s.</returns>
        IList<string> GetColumnValues(string filePath, string columnName, string delimiter);

        /// <summary>
        /// Get a <see cref="string"/> <see cref="List{T}"/> of column values.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="columnName">The column you want to read.</param>
        /// <param name="delimiter">The delimiter string.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/>'s.</returns>
        IList<string> GetColumnValues(Stream stream, string columnName, string delimiter);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(string filePath);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(Stream stream);

        /// <summary>
        /// Get a <see cref="List{T}"/> of <see cref="ExpandoObject"/>s from a CSV file.
        /// </summary>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(string filePath, string delimiter);

        /// <summary>
        /// Get a <see cref="List{T}"/> of <see cref="ExpandoObject"/>s from a CSV file.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="removeQuotes">True if you want the CSV quites surrounding each value to be removed. False if not.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="removeQuotes">True if you want the CSV quites surrounding each value to be removed. False if not.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter, bool removeQuotes);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="removeQuotes">True if you want the CSV quites surrounding each value to be removed. False if not.</param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of your <see cref="ExpandoObject"/>.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="removeQuotes">True if you want the CSV quites surrounding each value to be removed. False if not.</param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of your <see cref="ExpandoObject"/>.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="removeQuotes">True if you want the CSV quites surrounding each value to be removed. False if not.</param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of your <see cref="ExpandoObject"/>.</param>
        /// <param name="ignoreCase">Provide true if you want to make the column mapping non-case sensitive. False if it should be case sensitive.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns, bool ignoreCase);

        /// <summary>
        /// Gets a <see cref="List{T}"/> of <see cref="ExpandoObject"/>'s.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="removeQuotes">True if you want the CSV quites surrounding each value to be removed. False if not.</param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of your <see cref="ExpandoObject"/>.</param>
        /// <param name="ignoreCase">Provide true if you want to make the column mapping non-case sensitive. False if it should be case sensitive.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="ExpandoObject"/>s.</returns>
        IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns, bool ignoreCase);

        /// <summary>
        /// Get a <see cref="List{T}"/> of <see cref="string"/> header names from the CSV file.
        /// </summary>
        /// <param name="filePath">A <see cref="string"/> path to the file you want to read the headers from.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/> header names.</returns>
        IList<string> GetHeaders(string filePath);

        /// <summary>
        /// Get a <see cref="List{T}"/> of <see cref="string"/> header names from the CSV stream.
        /// </summary>
        /// <param name="stream">A stream with CSV data.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/> header names.</returns>
        IList<string> GetHeaders(Stream stream);

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV file.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <returns>A <see cref="List{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(string filePath) where T : new();

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV stream.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="stream">A stream with CSV data.</param>
        /// <returns>A <see cref="List{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(Stream stream) where T : new();

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV file.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <returns>A <see cref="List{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(string filePath, string delimiter) where T : new();

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV stream.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <returns>A <see cref="List{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(Stream stream, string delimiter) where T : new();

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV file.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of you <typeparamref name="T"/> object.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(string filePath, string delimiter, IDictionary<string, string> lookupColumns) where T : new();

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV stream.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="delimiter">A <see cref="string"/> value for what you want to use as a delimiter.</param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of you <typeparamref name="T"/> object.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(Stream stream, string delimiter, IDictionary<string, string> lookupColumns) where T : new();

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV file.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="filePath">The CSV <see cref="string"/> file path to read from.</param>
        /// <param name="delimiter"></param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of you <typeparamref name="T"/> object.</param>
        /// <param name="ignoreCase">Provide true if you want to make the column mapping non-case sensitive. False if it should be case sensitive.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(string filePath, string delimiter, IDictionary<string, string> lookupColumns, bool ignoreCase) where T : new();

        /// <summary>
        /// Get a <see cref="List{T}"/> of <typeparamref name="T"/> objects from a CSV stream.
        /// </summary>
        /// <typeparam name="T">The <typeparamref name="T"/> object to convert each CSV row into.</typeparam>
        /// <param name="stream">A stream with CSV data.</param>
        /// <param name="delimiter"></param>
        /// <param name="lookupColumns">A <see cref="IDictionary{TKey, TValue}"/> that has a <see cref="string"/> key and value that represent column mappings.
        /// You can have a map that contains a key that corresponds to the column name in the CSV and a value that corresponds to the property name
        /// of you <typeparamref name="T"/> object.</param>
        /// <param name="ignoreCase">Provide true if you want to make the column mapping non-case sensitive. False if it should be case sensitive.</param>
        /// <returns>A <see cref="LinkedList{T}"/> of <typeparamref name="T"/> objects.</returns>
        IList<T> GetList<T>(Stream stream, string delimiter, IDictionary<string, string> lookupColumns, bool ignoreCase) where T : new();
    }
}