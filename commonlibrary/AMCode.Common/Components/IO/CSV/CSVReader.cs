using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.IO.CSV.Exceptions;
using AMCode.Common.IO.CSV.Models;
using AMCode.Common.Util;

namespace AMCode.Common.IO.CSV
{
    /// <summary>
    /// A class designed to read CSV files into collections of data.
    /// </summary>
    public class CSVReader : ICSVReader
    {
        private readonly bool hasColumns = true;
        private readonly Func<int, string> getColumnName = (int index) => $"Header{index + 1}";

        /// <summary>
        /// Create an instance of the <see cref="CSVReader"/> class.
        /// </summary>
        public CSVReader() { }

        /// <summary>
        /// Create an instance of the <see cref="CSVReader"/> class.
        /// </summary>
        /// <param name="hasColumns">Whether or not the file should be treated as having columns or not.</param>
        public CSVReader(bool hasColumns)
        {
            this.hasColumns = hasColumns;
        }

        /// <summary>
        /// Create an instance of the <see cref="CSVReader"/> class.
        /// </summary>
        /// <param name="hasColumns">Whether or not the file should be treated as having columns or not.</param>
        /// <param name="getColumnName">Provide a function that returns the columns name to assign for the index when <paramref name="hasColumns"/> is <c>false</c>.</param>
        public CSVReader(bool hasColumns, Func<int, string> getColumnName)
        {
            this.hasColumns = hasColumns;
            this.getColumnName = getColumnName;
        }

        /// <inheritdoc/>
        public IList<string> GetColumnValues(string filePath, string columnName) => GetColumnValues(filePath, columnName, ",");

        /// <inheritdoc/>
        public IList<string> GetColumnValues(string filePath, string columnName, string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, string, string, IList<string>>(GetColumnValues);
                throw new DelimiterNotProvidedException(header, nameof(delimiter));
            }

            using (var stream = File.OpenRead(filePath))
            {
                return GetColumnValues(stream, columnName, delimiter);
            }
        }

        /// <inheritdoc/>
        public IList<string> GetColumnValues(Stream stream, string columnName) => GetColumnValues(stream, columnName, ",");

        /// <inheritdoc/>
        public IList<string> GetColumnValues(Stream stream, string columnName, string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                var header = ExceptionUtil.CreateExceptionHeader<Stream, string, string, IList<string>>(GetColumnValues);
                throw new DelimiterNotProvidedException(header, nameof(delimiter));
            }

            var columnValues = new List<string>();
            var updatedDelimiter = updateDelimiter(delimiter);

            using (var reader = new TextFieldParser(stream)
            {
                Delimiters = new string[] { delimiter },
                HasFieldsEnclosedInQuotes = true,
                TextFieldType = FieldType.Delimited
            })
            {

                var columnNames = Regex.Split(reader.ReadLine(), updatedDelimiter);
                var columnNameIndex = columnNames.ToList().IndexOf(columnName);

                while (reader.EndOfData == false)
                {
                    var commaSplit = $@"{updatedDelimiter}(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)";
                    var data = Regex.Split(reader.ReadLine(), commaSplit);
                    var columnValue = Regex.Replace(data[columnNameIndex], @"^\""|\""$", "");
                    columnValues.Add(columnValue);
                }
            }

            return columnValues;
        }

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(string filePath) => GetExpandoList(filePath, ",", true, null, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(string filePath, string delimiter) => GetExpandoList(filePath, delimiter, true, null, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes)
            => GetExpandoList(filePath, delimiter, removeQuotes, null, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns)
            => GetExpandoList(filePath, delimiter, removeQuotes, lookupColumns, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(string filePath, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, string, bool, IDictionary<string, string>, bool, IList<ExpandoObject>>(GetExpandoList);
                throw new DelimiterNotProvidedException(header, nameof(delimiter));
            }

            using (var stream = File.OpenRead(filePath))
            {
                return GetExpandoList(stream, delimiter, removeQuotes, lookupColumns, ignoreCase);
            }
        }

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(Stream stream) => GetExpandoList(stream, ",", true, null, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter) => GetExpandoList(stream, delimiter, true, null, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter, bool removeQuotes)
            => GetExpandoList(stream, delimiter, removeQuotes, null, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns)
            => GetExpandoList(stream, delimiter, removeQuotes, lookupColumns, false);

        /// <inheritdoc/>
        public IList<ExpandoObject> GetExpandoList(Stream stream, string delimiter, bool removeQuotes, IDictionary<string, string> lookupColumns, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                var header = ExceptionUtil.CreateExceptionHeader<Stream, string, bool, IDictionary<string, string>, bool, IList<ExpandoObject>>(GetExpandoList);
                throw new DelimiterNotProvidedException(header, nameof(delimiter));
            }

            var expandoList = new List<ExpandoObject>();
            var updatedDelimiter = updateDelimiter(delimiter);
            var commaSplit = $@"{updatedDelimiter}(?=(?:[^\""]*\""[^\""]*\"")*[^\""]*$)";

            using (var reader = new TextFieldParser(stream)
            {
                Delimiters = new string[] { delimiter },
                HasFieldsEnclosedInQuotes = true,
                TextFieldType = FieldType.Delimited
            })
            {
                string createColumns()
                {
                    var rowString = reader.PeekChars((int)stream.Position);
                    var headerLength = Regex.Split(rowString, commaSplit).Length;
                    var columns = Enumerable.Range(0, headerLength).Select(index => getColumnName(index));
                    return string.Join(",", columns);
                }

                var columnHeaderRow = hasColumns ? reader.ReadLine() : createColumns();

                if (columnHeaderRow == null)
                {
                    return expandoList;
                }

                var columnNames = Regex.Split(columnHeaderRow, updatedDelimiter);

                while (reader.EndOfData == false)
                {
                    var expando = new ExpandoObject();
                    var data = Regex.Split(reader.ReadLine(), commaSplit);

                    // Loop through all the column names And assign 
                    for (var i = 0; i <= columnNames.Length - 1; i++)
                    {
                        var columnName = columnNames[i];

                        if (lookupColumns != null)
                        {
                            var dictColumnName = columnName;
                            if (ignoreCase)
                            {
                                dictColumnName = lookupColumns.Keys.Where(key => key.ToLower().Equals(dictColumnName.ToLower())).FirstOrDefault();
                            }

                            lookupColumns.TryGetValue(dictColumnName, out columnName);
                        }

                        var val = removeQuotes ? Regex.Replace(data[i], @"^\""|\""$", "") : data[i];
                        expando = expando.AddOrUpdatePropertyWithValue(columnName, val ?? "");
                    }

                    expandoList.Add(expando);
                }
            }

            return expandoList;
        }

        /// <inheritdoc/>
        public IList<string> GetHeaders(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return GetHeaders(stream);
            }
        }

        /// <inheritdoc/>
        public IList<string> GetHeaders(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var headersLine = reader.ReadLine();

                if (headersLine == null)
                {
                    return new List<string>();
                }

                var headers = headersLine.Split(',').ToList();
                reader.Close();

                return headers;
            }
        }

        /// <inheritdoc/>
        public IList<T> GetList<T>(string filePath) where T : new() => GetList<T>(filePath, ",", null, false);

        /// <inheritdoc/>
        public IList<T> GetList<T>(string filePath, string delimiter) where T : new() => GetList<T>(filePath, delimiter, null, false);

        /// <inheritdoc/>
        public IList<T> GetList<T>(string filePath, string delimiter, IDictionary<string, string> lookupColumns)
            where T : new()
            => GetList<T>(filePath, delimiter, lookupColumns, false);

        /// <inheritdoc/>
        public IList<T> GetList<T>(string filePath, string delimiter, IDictionary<string, string> lookupColumns, bool ignoreCase) where T : new()
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                var header = ExceptionUtil.CreateExceptionHeader<string, string, IDictionary<string, string>, bool, IList<T>>(GetList<T>);
                throw new DelimiterNotProvidedException(header, nameof(delimiter));
            }

            using (var stream = File.OpenRead(filePath))
            {
                return GetList<T>(stream, delimiter, lookupColumns, ignoreCase);
            }
        }

        /// <inheritdoc/>
        public IList<T> GetList<T>(Stream stream) where T : new() => GetList<T>(stream, ",", null, false);

        /// <inheritdoc/>
        public IList<T> GetList<T>(Stream stream, string delimiter) where T : new() => GetList<T>(stream, delimiter, null, false);

        /// <inheritdoc/>
        public IList<T> GetList<T>(Stream stream, string delimiter, IDictionary<string, string> lookupColumns) where T : new()
            => GetList<T>(stream, delimiter, lookupColumns, false);

        /// <inheritdoc/>
        public IList<T> GetList<T>(Stream stream, string delimiter, IDictionary<string, string> lookupColumns, bool ignoreCase)
            where T : new()
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                var header = ExceptionUtil.CreateExceptionHeader<Stream, string, IDictionary<string, string>, bool, IList<T>>(GetList<T>);
                throw new DelimiterNotProvidedException(header, nameof(delimiter));
            }

            var tList = new List<T>();
            var updatedDelimiter = updateDelimiter(delimiter);

            using (var reader = new TextFieldParser(stream)
            {
                Delimiters = new string[] { delimiter },
                HasFieldsEnclosedInQuotes = true,
                TextFieldType = FieldType.Delimited
            })
            {
                string[] createColumns()
                {
                    var rowString = reader.PeekChars((int)stream.Position);
                    var headerLength = Regex.Split(rowString, updatedDelimiter).Length;
                    var columns = Enumerable.Range(0, headerLength).Select(index => getColumnName(index));
                    return columns.ToArray();
                }

                var columnNames = hasColumns ? Regex.Split(reader.ReadLine(), updatedDelimiter) : createColumns();

                while (reader.EndOfData == false)
                {
                    // Get an instance of T which Is the object you provide as T.
                    var tItem = new T();
                    var data = reader.ReadFields();

                    for (var j = 0; j <= columnNames.Length - 1; j++)
                    {
                        var columnName = columnNames[j];

                        if (lookupColumns != null)
                        {
                            var dictColumnName = columnName;
                            if (ignoreCase)
                            {
                                dictColumnName = lookupColumns.Keys.Where(key => key.ToLower().Equals(dictColumnName.ToLower())).FirstOrDefault();
                            }

                            lookupColumns.TryGetValue(dictColumnName, out columnName);
                        }

                        var bindings = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
                        if (ignoreCase)
                        {
                            bindings = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Static;
                        }

                        var property = tItem.GetType().GetProperty(columnName, bindings);

                        if (property != null & property.CanWrite)
                        {
                            var currentValue = data[j];
                            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            var isNull = false;
                            if (currentValue == null || (currentValue is string && currentValue.Equals(string.Empty)))
                            {
                                isNull = true;
                            }

                            var result = isNull ? null : Convert.ChangeType(currentValue, property.PropertyType);
                            if (result != null)
                            {
                                property.SetValue(tItem, result, null);
                            }
                        }
                    }

                    tList.Add(tItem);
                }
            }

            return tList;
        }

        /// <summary>
        /// Make sure the delimiter is correctly represented for Regex.
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        private string updateDelimiter(string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                return delimiter;
            }

            return delimiter.Length == 1 ? $@"\{delimiter}" : delimiter;
        }
    }
}