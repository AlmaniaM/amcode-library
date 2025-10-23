using System;
using System.IO;
using AMCode.Common.Extensions.Strings;
using Newtonsoft.Json;

namespace AMCode.Common.Extensions.Objects
{
    /// <summary>
    /// A class designed for providing <see cref="object"/> extension methods.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Deep copy an object.
        /// </summary>
        /// <typeparam name="T">The object <see cref="Type"/>.</typeparam>
        /// <param name="obj">The object you want to copy.</param>
        /// <returns>A deep copy of the provided object.</returns>
        public static T DeepCopy<T>(this T obj) => deepCopy(obj, typeof(T));

        /// <summary>
        /// Deep copy an object.
        /// </summary>
        /// <typeparam name="TInterface">The object <see cref="Type"/>.</typeparam>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="obj">The object you want to copy.</param>
        /// <returns>A deep copy of the provided object.</returns>
        public static TInterface DeepCopy<TInterface, TClass>(this TInterface obj)
            where TInterface : class
            where TClass : class, TInterface => deepCopy(obj, typeof(TClass));

        /// <summary>
        /// Checks whether or not this object is assignable to the provided
        /// <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> you are trying to convert this object to.</typeparam>
        /// <param name="source">Any object.</param>
        /// <param name="destination">A variable of type <typeparamref name="T"/> to assign the converted value to.</param>
        /// <returns>If conversion successful then true. Otherwise, false.</returns>
        public static bool Is<T>(this object source, out T destination)
        {
            if (source is T convertedObject)
            {
                destination = convertedObject;
                return true;
            }
            else
            {
                destination = default;
                return false;
            }
        }

        /// <summary>
        /// Deep copy an object.
        /// </summary>
        /// <typeparam name="T">The object <see cref="Type"/>.</typeparam>
        /// <param name="obj">The object you want to copy.</param>
        /// <param name="type">The <see cref="Type"/> to de-serialize the object to.</param>
        /// <returns>A deep copy of the provided object.</returns>
        private static T deepCopy<T>(T obj, Type type)
        {
            using (var stream = JsonConvert.SerializeObject(obj).ToStream())
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var newObject = (T)new JsonSerializer().Deserialize(jsonReader, type);
                return newObject;
            }
        }
    }
}