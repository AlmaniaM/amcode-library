using System.Text;

namespace AMCode.Sql.Extensions.StringExtensions.Vertica
{
    /// <summary>
    /// A class which contains extension methods for <see cref="string"/>s in the Vertica DB realm.
    /// </summary>
    public static class StringExtensionsVertica
    {
        /// <summary>
        /// Sanitizes a <see cref="string"/> value to be usable as a string literal in the Vertica DB.
        /// </summary>
        /// <param name="str">The <see cref="string"/> you want to sanitize.</param>
        /// <returns>If the <see cref="string"/> is null or <see cref="string.Empty"/> then <see cref="string.Empty"/>
        /// will be returned. If not, then the <see cref="string"/> will be surrounded by double dollar signs on both sides.</returns>
        public static string Sanitize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return new StringBuilder()
                .Append("$$")
                .Append(str)
                .Append("$$")
                .ToString();
        }
    }
}