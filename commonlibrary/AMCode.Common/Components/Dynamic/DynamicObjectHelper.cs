using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using AMCode.Common.Extensions.ExpandoObjects;

namespace AMCode.Common.Dynamic
{
    /// <summary>
    /// A static class that hold only Extension Methods for the <see cref="ExpandoObject"/> <see cref="Type"/>.
    /// </summary>
    public static class DynamicObjectHelper
    {
        #region Dynamic Object Creation

        /// <summary>
        /// Method that builds a single <seealso cref="ExpandoObject"/> from a row in a datareader.
        /// </summary>
        /// 
        /// <param name="reader">A populated data reader. By populated I mean the data reader should have already
        /// called reader.NextResult() before calling this method. You should not call this method if you haven't 
        /// called NextResult() because this method is designed to work inside a loop that is looping through all
        /// the data reader results.</param>
        /// 
        /// <param name="columnNames">An optional list of field names from the data reader. If the array is null then 
        /// they will be populated from the data reader itself. For faster performance it's better to get the list of 
        /// names from the data reader prior to calling this method and you should only call the data reader GetNames()
        /// method once for the entire operation to get the performance benefit.</param>
        /// 
        /// <returns>An ExpandoObject populated with a list of column names as properties and their corresponding
        /// values from the data reader.</returns>
        public static ExpandoObject BuildExpandoFromRow(IDataReader reader, string[] columnNames = null)
        {
            // Get the field names if the list is null
            columnNames = columnNames ?? Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();
            dynamic expando = new ExpandoObject();

            // Loop through all the column names and assign 
            for (var i = 0; i < columnNames.Length; i++)
            {
                var val = reader.GetValue(i);
                expando = (expando as ExpandoObject).AddOrUpdatePropertyWithValue(columnNames[i], val is DBNull ? "" : val);
            }

            return expando;
        }

        /// <summary>
        /// Creates a single <seealso cref="ExpandoObject"/> from a given string property name and an 
        /// <seealso cref="object"/> value.
        /// </summary>
        /// <param name="prop">The name of the property to create.</param>
        /// <param name="val">The value to assign to the property.</param>
        /// <returns>An <see cref="ExpandoObject"/> with the provided property and value.</returns>
        public static ExpandoObject CreateExpandoObject(string prop, object val)
        {
            dynamic expando = new ExpandoObject();
            expando = (expando as ExpandoObject).AddOrUpdatePropertyWithValue(prop, val is null ? "" : val);
            return expando;
        }

        /// <summary>
        /// Method that builds a single <seealso cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys
        /// and <see cref="object"/> values from an <seealso cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="expando">An ExpandoObject to build the Dictionary from.</param>
        /// <returns>An Dictionary object.</returns>
        public static Dictionary<string, object> ExpandoToDictionary(ExpandoObject expando)
        {
            var dict = (IDictionary<string, object>)expando;

            // Loop through all the column names and assign the value to its corresponding key.
            return dict.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);
        }

        #endregion
    }
}