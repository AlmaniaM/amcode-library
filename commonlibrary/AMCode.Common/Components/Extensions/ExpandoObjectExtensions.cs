using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;

namespace AMCode.Common.Extensions.ExpandoObjects
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="ExpandoObject"/> <see cref="Type"/>.
    /// </summary>
    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Deep copy an <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="expando">The <see cref="ExpandoObject"/> that you want to copy.</param>
        /// <returns>A new copy of the provided <see cref="ExpandoObject"/>.</returns>
        public static ExpandoObject Copy(this ExpandoObject expando)
        {
            var serialized = JsonConvert.SerializeObject(expando);
            var deserialized = JsonConvert.DeserializeObject<ExpandoObject>(serialized);
            return deserialized;
        }

        /// <summary>
        /// Remove a property from the <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="expando">The <see cref="ExpandoObject"/> to remove the property from.</param>
        /// <param name="key">Then <see cref="string"/> name of the property to remove.</param>
        /// <returns>The provided <see cref="ExpandoObject"/> with the property removed.</returns>
        public static bool Remove(this ExpandoObject expando, string key) => ((IDictionary<string, object>)expando).Remove(key);

        /// <summary>
        /// Returns a <see cref="List{T}"/> of <see cref="string"/> keys from the <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="expando">The <see cref="ExpandoObject"/> to work on.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/> values from the <see cref="ExpandoObject"/>.</returns>
        public static List<string> Keys(this ExpandoObject expando)
        {
            IDictionary<string, object> localObj = expando;
            var expandoKeys = localObj.Keys.ToList();
            return expandoKeys;
        }

        /// <summary>
        /// Method that adds a property to the expando object or assigns a value to an existing one.
        /// </summary>
        /// <param name="expando">The expando object to be modified and returned.</param>
        /// <param name="propertyName">The string name of the property to add or modify.</param>
        /// <param name="value">The object value to be assigned to the property.</param>
        /// <returns>An ExpandoObject with the new property value.</returns>
        public static ExpandoObject AddOrUpdatePropertyWithValue(this ExpandoObject expando, string propertyName, object value)
        {
            var tempExpando = (IDictionary<string, object>)expando;
            tempExpando[propertyName] = value;
            return (ExpandoObject)tempExpando;
        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> of <see cref="string"/> values from the <see cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="obj">The <see cref="ExpandoObject"/> to work on.</param>
        /// <returns>A <see cref="List{T}"/> of <see cref="string"/> values from the <see cref="ExpandoObject"/>.</returns>
        public static List<object> Values(this ExpandoObject obj)
        {
            IDictionary<string, object> localObj = obj;
            var expandoValues = localObj.Values.ToList();
            return expandoValues;
        }

        /// <summary>
        /// Extension method to retrieve a value from an expando object. If the value is <c>null</c> 
        /// or <seealso cref="DBNull"/> Then the <paramref name="obj"/> will be set to it's default value provided
        /// by default(T).
        /// </summary>
        /// <typeparam name="T">The data type to convert to.</typeparam>
        /// <param name="obj">The <see cref="ExpandoObject"/> to get the value from.</param>
        /// <param name="key">The <see cref="string"/> key of the value.</param>
        public static T GetValue<T>(this ExpandoObject obj, string key)
        {
            ((IDictionary<string, object>)obj).TryGetValue(key, out var outObj);

            if (outObj == null || outObj is DBNull)
            {
                return default;
            }
            else
            {
                if (outObj is T t)
                {
                    return t;
                }
                else
                {
                    try
                    {
                        return (T)Convert.ChangeType(outObj, typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        try
                        {
                            var converter = TypeDescriptor.GetConverter(typeof(T));
                            return (T)converter.ConvertFromInvariantString(outObj.ToString());
                        }
                        catch (Exception)
                        {
                            return default;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method that builds a single <seealso cref="IDictionary{TKey, TValue}"/> of <see cref="string"/> keys and <see cref="object"/>
        /// values from an <seealso cref="ExpandoObject"/>.
        /// </summary>
        /// <param name="expando">An ExpandoObject to build the Dictionary from.</param>
        /// <returns>An Dictionary object.</returns>
        public static Dictionary<string, object> ToDictionary(this ExpandoObject expando)
        {
            var dict = (IDictionary<string, object>)expando;

            // Loop through all the column names and assign the value to its corresponding key.
            return dict.ToDictionary(keyValue => keyValue.Key, keyValue => keyValue.Value);
        }

        /// <summary>
        /// Extension method to retrieve a value from an expando object. If the value is <code>null</code> 
        /// or <seealso cref="DBNull"/> Then the <paramref name="o"/> will be set to it's default value provided
        /// by default(T).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="key"></param>
        /// <param name="o"></param>
        /// <returns>True if retrieving the value was successful. False if not.</returns>
        public static bool TryGetValue<T>(this ExpandoObject obj, string key, out T o)
        {
            ((IDictionary<string, object>)obj).TryGetValue(key, out var outObj);
            if (outObj == null || outObj is DBNull)
            {
                o = default;
                return false;
            }
            else
            {
                if (outObj is T t)
                {
                    o = t;
                    return true;
                }
                else
                {
                    try
                    {
                        o = (T)Convert.ChangeType(outObj, typeof(T));
                        return true;
                    }
                    catch (InvalidCastException)
                    {
                        try
                        {
                            var converter = TypeDescriptor.GetConverter(typeof(T));
                            o = (T)converter.ConvertFromInvariantString(outObj.ToString());
                            return true;
                        }
                        catch (Exception)
                        {
                            o = default;
                            return false;
                        }
                    }
                }
            }
        }
    }
}