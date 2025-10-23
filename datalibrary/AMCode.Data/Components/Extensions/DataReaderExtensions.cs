using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;
using AMCode.Columns.Core;
using AMCode.Columns.DataTransform;
using AMCode.Common.Extensions.ExpandoObjects;
using AMCode.Common.Util;

namespace AMCode.Data.Extensions
{
    /// <summary>
    /// A class designed to hold <see cref="IDataReader"/> extension methods.
    /// </summary>
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Maps a DataRow to a generic <see cref="ExpandoObject"/>. The columns from the <see cref="IDataReader"/>
        /// are mapped to keys in the <see cref="ExpandoObject"/>. The column values in the <see cref="IDataReader"/>
        /// are mapped to the key values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the <see cref="ExpandoObject"/>.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<ExpandoObject> ToExpandoList(this IDataReader reader)
        {
            var localReader = reader;
            var items = new List<ExpandoObject>();
            var fieldNames = Enumerable.Range(0, localReader.FieldCount).Select(localReader.GetName).ToArray();

            try
            {
                while (localReader.Read())
                {
                    dynamic expando = new ExpandoObject();

                    // Loop through all the column names and assign 
                    for (var i = 0; i < fieldNames.Length; i++)
                    {
                        var val = localReader.GetValue(i);
                        expando = (expando as ExpandoObject).AddOrUpdatePropertyWithValue(fieldNames[i], val);
                    }

                    items.Add(expando);
                }
            }
            catch (Exception)
            {
                if (!localReader.IsClosed)
                {
                    localReader.Close();
                }

                throw;
            }

            return items;
        }

        /// <summary>
        /// Maps a DataRow to a generic <see cref="ExpandoObject"/>. The columns from the <see cref="IDataReader"/>
        /// are mapped to keys in the <see cref="ExpandoObject"/>. The column values in the <see cref="IDataReader"/>
        /// are mapped to the key values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the <see cref="ExpandoObject"/>.</param>
        /// <param name="columns"/>A <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>s to use for data formatting./>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<ExpandoObject> ToExpandoList(this IDataReader reader, IList<IDataTransformColumnDefinition> columns)
        {
            var items = new List<ExpandoObject>();
            var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToList();
            var fieldNameToColumnDictionary = getFieldNameToColumnDefinitionDictionary(columns, fieldNames);

            while (reader.Read())
            {
                dynamic expando = new ExpandoObject();

                // Loop through all the column names and assign 
                for (var i = 0; i < fieldNames.Count; i++)
                {
                    var fieldName = fieldNames[i];
                    fieldNameToColumnDictionary.TryGetValue(fieldName, out var dataTransformColumnDefinition);
                    var formatter = dataTransformColumnDefinition?.GetFormatter();
                    var value = reader[fieldName];

                    try
                    {
                        value = formatter != null ? formatter.FormatToObject(value) : value;
                    }
                    catch (Exception)
                    {
                        throw new Exception($"[{nameof(DataReaderExtensions)}][{nameof(ToExpandoList)}]({nameof(reader)}, {nameof(columns)}) Error formatting {value} for column {nameof(dataTransformColumnDefinition.PropertyName)}.");
                    }

                    try
                    {
                        expando = (expando as ExpandoObject).AddOrUpdatePropertyWithValue(dataTransformColumnDefinition.PropertyName, value);
                    }
                    catch (Exception)
                    {
                        throw new Exception($"[{nameof(DataReaderExtensions)}][{nameof(ToExpandoList)}]({nameof(reader)}, {nameof(columns)}) Error adding {value} to property name {dataTransformColumnDefinition.PropertyName}.");
                    }
                }

                items.Add(expando);
            }

            return items;
        }

        /// <summary>
        /// Maps a DataRow to a generic <see cref="ExpandoObject"/>. The columns from the <see cref="IDataReader"/>
        /// are mapped to keys in the <see cref="ExpandoObject"/>. The column values in the <see cref="IDataReader"/>
        /// are mapped to the key values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the <see cref="ExpandoObject"/>.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<ExpandoObject> ToExpandoList(this IDataReader reader, CancellationToken cancellationToken)
        {
            var items = new List<ExpandoObject>();
            cancellationToken.ThrowIfCancellationRequested();

            var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToArray();

            cancellationToken.ThrowIfCancellationRequested();

            while (!cancellationToken.IsCancellationRequested && reader.Read())
            {
                cancellationToken.ThrowIfCancellationRequested();
                dynamic expando = new ExpandoObject();

                // Loop through all the column names and assign 
                for (var i = 0; i < fieldNames.Length; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var val = reader.GetValue(i);
                    expando = (expando as ExpandoObject).AddOrUpdatePropertyWithValue(fieldNames[i], val);
                }

                items.Add(expando);

                cancellationToken.ThrowIfCancellationRequested();
            }

            cancellationToken.ThrowIfCancellationRequested();

            return items;
        }

        /// <summary>
        /// Maps a DataRow to a generic <see cref="ExpandoObject"/>. The columns from the <see cref="IDataReader"/>
        /// are mapped to keys in the <see cref="ExpandoObject"/>. The column values in the <see cref="IDataReader"/>
        /// are mapped to the key values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the <see cref="ExpandoObject"/>.</param>
        /// <param name="columns"/>A <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>s to use for data formatting./>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<ExpandoObject> ToExpandoList(this IDataReader reader, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken)
        {
            string createExceptionHeader()
                => ExceptionUtil.CreateExceptionHeader<IList<IDataTransformColumnDefinition>, CancellationToken, IList<ExpandoObject>>(reader.ToExpandoList);

            var items = new List<ExpandoObject>();
            cancellationToken.ThrowIfCancellationRequested();

            var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToList();

            cancellationToken.ThrowIfCancellationRequested();

            var fieldNameToColumnDictionary = getFieldNameToColumnDefinitionDictionary(columns, fieldNames);

            while (!cancellationToken.IsCancellationRequested && reader.Read())
            {
                cancellationToken.ThrowIfCancellationRequested();
                dynamic expando = new ExpandoObject();

                // Loop through all the column names and assign 
                for (var i = 0; i < fieldNames.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var fieldName = fieldNames[i];
                    fieldNameToColumnDictionary.TryGetValue(fieldName, out var dataTransformColumnDefinition);
                    var formatter = dataTransformColumnDefinition?.GetFormatter();
                    var value = reader[fieldName];

                    try
                    {
                        value = formatter != null ? formatter.FormatToObject(value) : value;
                    }
                    catch (Exception)
                    {
                        throw new Exception($"{createExceptionHeader()} Error formatting {value} for column {dataTransformColumnDefinition.PropertyName}.");
                    }

                    try
                    {
                        expando = (expando as ExpandoObject).AddOrUpdatePropertyWithValue(dataTransformColumnDefinition?.PropertyName ?? fieldName, value);
                    }
                    catch (Exception)
                    {
                        throw new Exception($"{createExceptionHeader()} Error adding {value} to property name {dataTransformColumnDefinition.PropertyName}.");
                    }
                }

                items.Add(expando);

                cancellationToken.ThrowIfCancellationRequested();
            }

            cancellationToken.ThrowIfCancellationRequested();

            return items;
        }

        /// <summary>
        /// Maps a <see cref="IDataRecord"/> to the provided generic <see cref="Type"/> <typeparamref name="T"/>.
        /// The columns from the <see cref="IDataReader"/> are mapped to the <typeparamref name="T"/> object properties.
        /// The column values in the <see cref="IDataReader"/> are mapped to the property values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the generic <see cref="Type"/>
        /// <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<T> ToList<T>(this IDataReader reader) where T : new()
        {
            var localReader = reader;
            var items = new List<T>();
            var columnNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();

            try
            {
                while (reader.Read())
                {
                    // Get an instance of T which is the object you provide as T.
                    var item = new T();

                    // Loop through all column names from the data reader.
                    for (var i = 0; i < columnNames.Length; i++)
                    {
                        item = updateItemProperty(localReader, item, columnNames[i]);
                    }

                    items.Add(item);
                }
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                throw;
            }

            return items;
        }

        /// <summary>
        /// Maps a <see cref="IDataRecord"/> to the provided generic <see cref="Type"/> <typeparamref name="T"/>.
        /// The columns from the <see cref="IDataReader"/> are mapped to the <typeparamref name="T"/> object properties.
        /// The column values in the <see cref="IDataReader"/> are mapped to the property values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the generic <see cref="Type"/>
        /// <param name="columns"/>A <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>s to use for data formatting./>
        /// <typeparamref name="T"/></param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<T> ToList<T>(this IDataReader reader, IList<IDataTransformColumnDefinition> columns) where T : new()
        {
            var localReader = reader;
            var items = new List<T>();
            var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            var fieldNameToColumnDictionary = mapToFieldNamesAndColumnDefinitionDictionary<T>(columns, fieldNames);

            try
            {
                while (reader.Read())
                {
                    // Get an instance of T which is the object you provide as T.
                    var item = new T();

                    // Loop through all column names from the data reader.
                    for (var i = 0; i < fieldNames.Count; i++)
                    {
                        var fieldName = fieldNames[i];
                        fieldNameToColumnDictionary.TryGetValue(fieldNames[i], out var column);
                        item = updateItemProperty(localReader, item, fieldName, column?.PropertyName, column?.GetFormatter());
                    }

                    items.Add(item);
                }
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                throw;
            }

            return items;
        }

        /// <summary>
        /// Maps a <see cref="IDataRecord"/> to the provided generic <see cref="Type"/> <typeparamref name="T"/>.
        /// The columns from the <see cref="IDataReader"/> are mapped to the <typeparamref name="T"/> object properties.
        /// The column values in the <see cref="IDataReader"/> are mapped to the property values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the generic <see cref="Type"/>
        /// <typeparamref name="T"/>.</param>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<T> ToList<T>(this IDataReader reader, CancellationToken cancellationToken) where T : new()
        {
            var localReader = reader;
            var items = new List<T>();
            var columnNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();

            try
            {
                while (reader.Read())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // Get an instance of T which is the object you provide as T.
                    var item = new T();

                    // Loop through all column names from the data reader.
                    for (var i = 0; i < columnNames.Length; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        item = updateItemProperty(localReader, item, columnNames[i]);
                    }

                    items.Add(item);
                }
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                throw;
            }

            return items;
        }

        /// <summary>
        /// Maps a <see cref="IDataRecord"/> to the provided generic <see cref="Type"/> <typeparamref name="T"/>.
        /// The columns from the <see cref="IDataReader"/> are mapped to the <typeparamref name="T"/> object properties.
        /// The column values in the <see cref="IDataReader"/> are mapped to the property values.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the generic <see cref="Type"/>
        /// <typeparamref name="T"/>.</param>
        /// <param name="columns"/>A <see cref="IList{T}"/> of <see cref="IDataTransformColumnDefinition"/>s to use for data formatting./>
        /// <param name="cancellationToken">Provide a <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="ExpandoObject"/>'s.</returns>
        public static IList<T> ToList<T>(this IDataReader reader, IList<IDataTransformColumnDefinition> columns, CancellationToken cancellationToken) where T : new()
        {
            var localReader = reader;
            var items = new List<T>();
            var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();
            var fieldNameToColumnDictionary = mapToFieldNamesAndColumnDefinitionDictionary<T>(columns, fieldNames);

            try
            {
                while (reader.Read())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // Get an instance of T which is the object you provide as T.
                    var item = new T();

                    // Loop through all column names from the data reader.
                    for (var i = 0; i < fieldNames.Count; i++)
                    {
                        var fieldName = fieldNames[i];

                        cancellationToken.ThrowIfCancellationRequested();

                        fieldNameToColumnDictionary.TryGetValue(fieldName, out var column);
                        item = updateItemProperty(localReader, item, fieldName, column?.PropertyName, column?.GetFormatter());
                    }

                    items.Add(item);
                }
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                throw;
            }

            return items;
        }

        /// <summary>
        /// Maps the field names to the provided list of <typeparamref name="T"/> objects.
        /// </summary>
        /// <param name="columnDefinitions">A <see cref="IList{T}"/> of <see cref="Type"/> <typeparamref name="T"/>
        /// object that implement the <see cref="IDataTransformColumnDefinition"/> interface.</param>
        /// <param name="fieldNames">A <see cref="IList{T}"/> of <see cref="string"/> field names to map columns to.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="IDataTransformColumnDefinition"/>.</typeparam>
        /// <returns>An <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <typeparamref name="T"/> values.</returns>
        private static IDictionary<string, T> getFieldNameToColumnDefinitionDictionary<T>(IList<T> columnDefinitions, IList<string> fieldNames) where T : IDataTransformColumnDefinition
            => fieldNames.ToDictionary(
                fieldName => fieldName,
                fieldName => columnDefinitions.FirstOrDefault(column => column.FieldName.Equals(fieldName))
            );

        /// <summary>
        /// Filters the <paramref name="fieldNames"/> to the <typeparamref name="T"/> object properties and maps the
        /// field names to the provided list of <typeparamref name="T"/> objects.
        /// </summary>
        /// <param name="columnDefinitions">A <see cref="IList{T}"/> of <see cref="Type"/> <typeparamref name="T"/>
        /// object that implement the <see cref="IColumnDefinition"/> interface.</param>
        /// <param name="fieldNames">A <see cref="IList{T}"/> of <see cref="string"/> field names to map columns to.</param>
        /// <typeparam name="T">Must be of <see cref="Type"/> <see cref="IColumnDefinition"/>.</typeparam>
        /// <returns>An <see cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <typeparamref name="T"/> values.</returns>
        private static IDictionary<string, IDataTransformColumnDefinition> mapToFieldNamesAndColumnDefinitionDictionary<T>(IList<IDataTransformColumnDefinition> columnDefinitions, IList<string> fieldNames)
            where T : new()
        {
            var propertyNames = new T().GetType()
                .GetProperties()
                .Where(property => property.CanWrite)
                .Select(property => property.Name)
                .Where(propertyName => columnDefinitions.FirstOrDefault(column => column.PropertyName.Equals(propertyName)) != null)
                .ToList();

            // We only map the visible columns
            return fieldNames
                .Where(fieldName =>
                {
                    var columnFieldNameToFieldName = columnDefinitions.FirstOrDefault(column => column.FieldName.Equals(fieldName));
                    var propertyNameToColumnPropertyName
                        = propertyNames.FirstOrDefault(propertyName => columnFieldNameToFieldName != null && columnFieldNameToFieldName.PropertyName.Equals(propertyName));
                    if (columnFieldNameToFieldName != null && propertyNameToColumnPropertyName != null)
                    {
                        return true;
                    }

                    return columnDefinitions.FirstOrDefault(column => column.PropertyName.Equals(fieldName)) != null;
                })
                .ToDictionary(
                    fieldName => fieldName,
                    fieldName => columnDefinitions.FirstOrDefault(column => column.FieldName.Equals(fieldName))
                );
        }

        /// <summary>
        /// Update a <typeparamref name="T"/> object's property with the value located in reader[propertyName].
        /// </summary>
        /// <typeparam name="T">The type of object to convert each <see cref="IDataRecord"/> into.</typeparam>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the generic <see cref="Type"/></param>
        /// <param name="item">The item to update.</param>
        /// <param name="propertyName">The item property and <see cref="IDataReader"/> column to access.</param>
        /// <returns>An updated <typeparamref name="T"/> item if appropriate conditions are met.</returns>
        private static T updateItemProperty<T>(IDataReader reader, T item, string propertyName)
        {
            var localReader = reader;

            // Try and get the property name of T that matches the column name from the data reader.
            var property = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            // If property exists and there's a value then assign it to the object property.
            if (property != null && reader[propertyName] != DBNull.Value)
            {
                var convertedValue = new ValueParser().Parse(localReader[propertyName], property.PropertyType);

                property.SetValue(item, convertedValue, null);
            }

            return item;
        }

        /// <summary>
        /// Update a <typeparamref name="T"/> object's property with a formatted value using a
        /// <see cref="IDataTransformColumnDefinition"/> formatter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The <see cref="IDataReader"/> that will be used to map to the generic <see cref="Type"/>
        /// <param name="item">The item to update.</param>
        /// <param name="fieldName"/>The <see cref="string"/> field name to access in the data reader.</param>
        /// <param name="propertyName">The property name of the <typeparamref name="T"/> object to assign the value to.</param>
        /// <param name="formatter">The <see cref="IValueFormatter"/> to use if available.</param>
        /// <returns>An updated <typeparamref name="T"/> item if appropriate conditions are met.</returns>
        private static T updateItemProperty<T>(IDataReader reader, T item, string fieldName, string propertyName, IValueFormatter formatter)
        {
            var localReader = reader;

            // Try and get the property name of T that matches the column name from the data reader.
            var property = item.GetType().GetProperty(propertyName ?? fieldName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var currentValue = localReader[fieldName];

            // If property exists and there's a value then assign it to the object property.
            if (property != null)
            {
                var convertedValue = new ValueParser().Parse(currentValue, property.PropertyType);
                var result = formatter != null ? formatter.FormatToObject(convertedValue) : convertedValue;
                property.SetValue(item, result, null);
            }

            return item;
        }
    }
}