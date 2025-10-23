using System;
using System.Collections.Generic;

namespace AMCode.Common.Extensions.Dictionary
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="IDictionary{TKey, TValue}"/> <see cref="Type"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Get the value <typeparamref name="T"/> that corresponds to the key <typeparamref name="K"/>.
        /// </summary>
        /// <typeparam name="K">The <see cref="Type"/> of the key.</typeparam>
        /// <typeparam name="T">The <see cref="Type"/> to convert the result to.</typeparam>
        /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> to search in.</param>
        /// <param name="key">The key to search for.</param>
        /// <returns>The value if it's found. If not found then the default value is returned.</returns>
        public static T GetValue<K, T>(this IDictionary<K, T> dictionary, K key)
        {
            if (dictionary == null)
            {
                throw new NullReferenceException($"[{nameof(DictionaryExtensions)}][{nameof(GetValue)}]({nameof(key)}) Cannot access a dictionary that's null");
            }

            var success = dictionary.TryGetValue(key, out var value);

            if (!success)
            {
                return default;
            }

            return value;
        }
    }
}