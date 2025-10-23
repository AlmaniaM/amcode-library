using System;
using System.IO;
using System.Linq;
using System.Text;
using AMCode.Common.IO;

namespace AMCode.Common.Extensions.Strings
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="string"/> <see cref="Type"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks to see if the string is either null, empty, or white space. If it's any of the 
        /// described values then it'll return s true. Otherwise, it'll return as false.
        /// </summary>
        /// <param name="s">The <see cref="string"/> you want to check.</param>
        /// <returns>A <see cref="bool"/> of true if the string is either null, empty, or white space. False if otherwise.</returns>
        public static bool IsNullEmptyOrWhiteSpace(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(s))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Compares two strings ignoring case sensitivity.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s, string s2)
        {
            if (s == null)
            {
                return false;
            }

            var isStringEqual = string.Equals(s, s2, StringComparison.InvariantCultureIgnoreCase);
            return isStringEqual;
        }

        /// <summary>
        /// Split a string on the provided delimiter.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to split.</param>
        /// <param name="delimiter">The <see cref="string"/> delimiter to use.</param>
        /// <returns></returns>
        public static string[] SplitIgnoreComma(this string str, string delimiter)
        {
            var strArray = str.SplitIgnoreComma(delimiter, false, false);
            return strArray;
        }

        /// <summary>
        /// Split a <see cref="string"/> on a certain delimiter.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to split.</param>
        /// <param name="delimiter">The <see cref="string"/> delimiter to use.</param>
        /// <param name="emptyStringAsNull">Set to true if you want empty string values returned as <c>null</c>.</param>
        /// <returns>An <see cref="Array"/> of <see cref="string"/>s split from the original <see cref="string"/>.</returns>
        public static string[] SplitIgnoreComma(this string str, string delimiter, bool emptyStringAsNull) => str.SplitIgnoreComma(delimiter, emptyStringAsNull, false);

        /// <summary>
        /// Split a <see cref="string"/> on a certain delimiter.
        /// </summary>
        /// <param name="str">The <see cref="string"/> to split.</param>
        /// <param name="delimiter">The <see cref="string"/> delimiter to use.</param>
        /// <param name="emptyStringAsNull">Set to true if you want empty string values returned as <c>null</c>.</param>
        /// <param name="addQuotes">Set to true if you want to surround the string with quotes (") before processing it.</param>
        /// <returns>An <see cref="Array"/> of <see cref="string"/>s split from the original <see cref="string"/>.</returns>
        public static string[] SplitIgnoreComma(this string str, string delimiter, bool emptyStringAsNull, bool addQuotes)
        {
            using (var stream = (addQuotes ? $"\"{str}\"" : $"{str}").GetStream())

            {
                using (var reader = new TextFieldParser(stream)
                {
                    HasFieldsEnclosedInQuotes = true,
                    Delimiters = new string[] { delimiter },
                    TextFieldType = FieldType.Delimited
                })
                {
                    if (emptyStringAsNull)
                    {
                        var strArray = reader.ReadFields().Select(field => string.IsNullOrWhiteSpace(field) ? null : field).ToArray();
                        return strArray;
                    }
                    else
                    {
                        var strArray = reader.ReadFields();
                        return strArray;
                    }
                }
            }
        }

        /// <summary>
        /// Create a <see cref="Stream"/> from a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> you want to create a <see cref="Stream"/> from.</param>
        /// <returns>An instance of a <see cref="Stream"/> from the given <see cref="string"/>.</returns>
        public static Stream GetStream(this string str)
        {
            if (str == null)
            {
                return null;
            }

            return str.ToStream();
        }

        /// <summary>
        /// Create a <see cref="Stream"/> from a <see cref="string"/>.
        /// </summary>
        /// <param name="str">The <see cref="string"/> you want to create a <see cref="Stream"/> from.</param>
        /// <returns>An instance of a <see cref="Stream"/> from the given <see cref="string"/>.</returns>
        public static Stream ToStream(this string str)
        {
            if (str == null)
            {
                return null;
            }

            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }
    }
}