using System;
using System.ComponentModel;
using System.Reflection;
using AMCode.Common.Extensions.Types;
using AMCode.Common.Util;
using Newtonsoft.Json;

namespace AMCode.Data.Extensions
{
    /// <summary>
    /// A class designed for parsing objects into their corresponding types.
    /// </summary>
    public class ValueParser
    {
        private delegate object InnerValueParser(object valueToParse, Type type, bool isNull);

        /// <summary>
        /// Parses the provided <paramref name="valueToParse"/> <see cref="object"/> into its <paramref name="type"/> <see cref="Type"/>.
        /// </summary>
        /// <param name="valueToParse">The <see cref="object"/> value to parse.</param>
        /// <param name="type">The <see cref="Type"/> to parse the value to.</param>
        /// <returns>A parsed <see cref="object"/> based on the provided <see cref="Type"/>.</returns>
        public object Parse(object valueToParse, Type type)
        {
            var parseValue = getInnerParser(type);
            return parseValue(valueToParse, type, valueToParse == null || (valueToParse is string && valueToParse.Equals(string.Empty)));
        }

        /// <summary>
        /// Tries to convert a value into a different <see cref="Type"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="type">The <see cref="Type"/> to convert the value to.</param>
        /// <returns>A converted value as an <see cref="object"/>.</returns>
        private object convert(string value, Type type) => TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);

        /// <summary>
        /// Get an <see cref="InnerValueParser"/> based on the provided <see cref="Type"/>.
        /// </summary>
        /// <param name="type">A <see cref="Type"/> for which you want an <see cref="InnerValueParser"/> for.</param>
        /// <returns>An <see cref="InnerValueParser"/> that should handle parsing your <see cref="object"/> value based on the <see cref="Type"/>.</returns>
        private InnerValueParser getInnerParser(Type type)
        {
            if (type.IsSimple())
            {
                return parseSimpleValue;
            }
            else if (type.IsDate())
            {
                return parseDateTimeValue;
            }
            else if (type == typeof(Type))
            {
                return parseTypeValue;
            }
            else
            {
                return parseStringToObject;
            }
        }

        /// <summary>
        /// Parses the provided <paramref name="valueToParse"/> <see cref="object"/> into its <paramref name="type"/> <see cref="Type"/>.
        /// This parses <see cref="DateTime"/> types only by running a simple conversion.
        /// </summary>
        /// <param name="valueToParse">The <see cref="object"/> value to parse.</param>
        /// <param name="type">The <see cref="Type"/> to parse the value to.</param>
        /// <param name="isNull">Whether or not the <paramref name="valueToParse"/> is considered to be <c>null</c>.</param>
        /// <returns>A parsed <see cref="object"/> based on the provided <see cref="Type"/>.</returns>
        private object parseDateTimeValue(object valueToParse, Type type, bool isNull)
        {
            if (isNull)
            {
                return valueToParse;
            }

            return valueToParse.GetType().IsDate() ? valueToParse : convert(valueToParse.ToString(), type);
        }

        /// <summary>
        /// Parses the provided <paramref name="valueToParse"/> <see cref="object"/> into its <paramref name="type"/> <see cref="Type"/>.
        /// This parses simple types only by running a simple conversion.
        /// </summary>
        /// <param name="valueToParse">The <see cref="object"/> value to parse.</param>
        /// <param name="type">The <see cref="Type"/> to parse the value to.</param>
        /// <param name="isNull">Whether or not the <paramref name="valueToParse"/> is considered to be <c>null</c>.</param>
        /// <returns>A parsed <see cref="object"/> based on the provided <see cref="Type"/>.</returns>
        private object parseSimpleValue(object valueToParse, Type type, bool isNull)
        {
            try
            {
                return isNull ? null : Convert.ChangeType(valueToParse, type);
            }
            catch (InvalidCastException)
            {
                return convert(valueToParse?.ToString(), type);
            }
        }

        /// <summary>
        /// Parses the provided <paramref name="valueToParse"/> <see cref="object"/> into its <paramref name="type"/> <see cref="Type"/>.
        /// This parses a <see cref="string"/> into an <see cref="object"/> of <see cref="PropertyInfo.PropertyType"/>.
        /// </summary>
        /// <param name="valueToParse">The <see cref="object"/> value to parse.</param>
        /// <param name="type">The <see cref="Type"/> to parse the value to.</param>
        /// <param name="isNull">Whether or not the <paramref name="valueToParse"/> is considered to be <c>null</c>.</param>
        /// <returns>A parsed <see cref="object"/> based on the provided <see cref="Type"/>.</returns>
        /// <exception cref="Exception">Thrown when a <see cref="string"/> cannot be de-serialized into its corresponding <see cref="PropertyInfo.PropertyType"/>.</exception>
        private object parseStringToObject(object valueToParse, Type type, bool isNull)
        {
            try
            {
                return JsonConvert.DeserializeObject(valueToParse.ToString(), type);
            }
            catch (Exception)
            {
                var header = ExceptionUtil.CreateExceptionHeader<object, Type, object>(Parse);
                throw new Exception($"{header} Error: Could not de-serialize data for property type {type} value {valueToParse}.");
            }
        }

        /// <summary>
        /// Parses the provided <paramref name="valueToParse"/> <see cref="object"/> into its <paramref name="type"/> <see cref="Type"/>.
        /// This parses a <see cref="string"/> into a <see cref="Type"/> only.
        /// </summary>
        /// <param name="valueToParse">The <see cref="object"/> value to parse.</param>
        /// <param name="type">The <see cref="Type"/> to parse the value to.</param>
        /// <param name="isNull">Whether or not the <paramref name="valueToParse"/> is considered to be <c>null</c>.</param>
        /// <returns>A parsed <see cref="object"/> based on the provided <see cref="Type"/>.</returns>
        /// <exception cref="Exception">Thrown when a <see cref="string"/> cannot be parsed into its corresponding <see cref="Type"/>.</exception>
        private object parseTypeValue(object valueToParse, Type type, bool isNull)
        {
            try
            {
                return isNull ? null : Type.GetType(valueToParse.ToString(), true, true);
            }
            catch (Exception)
            {
                var header = ExceptionUtil.CreateExceptionHeader<object, Type, object>(Parse);
                throw new Exception($"{header} Error: The value \"{valueToParse}\" could not be converted to the {nameof(Type)} class for property {type}.");
            }
        }
    }
}