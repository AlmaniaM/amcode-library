using System;
using System.Collections.Generic;
using System.Reflection;

namespace AMCode.Common.Extensions.Types
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="Type"/> <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        private readonly static IDictionary<Type, bool> primitiveTypes = new Dictionary<Type, bool>
        {
            [typeof(byte)] = true,
            [typeof(int)] = true,
            [typeof(long)] = true,
            [typeof(double)] = true,
            [typeof(decimal)] = true,
            [typeof(string)] = true,
            [typeof(char)] = true,
            [typeof(bool)] = true,
            [typeof(float)] = true,
            [typeof(sbyte)] = true,
            [typeof(short)] = true,
            [typeof(uint)] = true,
            [typeof(ulong)] = true,
            [typeof(ushort)] = true,
        };

        /// <summary>
        /// Check to see if the provided <see cref="Type"/> is a <see cref="DateTime"/> type.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to determine if it's <see cref="DateTime"/> or not.</param>
        /// <returns><c>True</c> if the provided <see cref="Type"/> is a <see cref="DateTime"/> type.</returns>
        public static bool IsDate(this Type type) => !type.IsSimple() && (type.Equals(typeof(DateTime)) || type.Equals(typeof(DateTime?)));

        /// <summary>
        /// Check to see if the provided <see cref="Type"/> is a primitive like type such as
        /// <see cref="string"/>, <see cref="decimal"/>, <see cref="long"/>, etc...
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to check</param>
        /// <returns>If it's primitive then <c>true</c>. Otherwise, <c>false</c>.</returns>
        public static bool IsSimple(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // Nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }

            return typeInfo.IsPrimitive || typeInfo.IsEnum || primitiveTypes.ContainsKey(type);
        }
    }
}