using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Extensions.StringExtensions.Vertica;

namespace AMCode.Sql.Extensions.EnumerableExtensions.Vertica
{
    /// <summary>
    /// A class containing extension methods for the <see cref="IEnumerable{T}"/> type.
    /// </summary>
    public static class EnumerableExtensionsVertica
    {
        /// <summary>
        /// Sanitizes an <see cref="IEnumerable{T}"/> of <see cref="string"/> value. The values are then usable as a string literals in the Vertica DB.
        /// </summary>
        /// <param name="stringList">The <see cref="IEnumerable{T}"/> of <see cref="string"/>s you want to sanitize.</param>
        /// <returns>If the <see cref="IEnumerable{T}"/> is null or empty then <see cref="Enumerable.Empty{TResult}"/>
        /// will be returned. If not, then the <see cref="string"/>s will be surrounded by double dollar signs on both sides.</returns>
        public static IEnumerable<string> Sanitize(this IEnumerable<string> stringList)
        {
            if (stringList is null || stringList.Count() == 0)
            {
                return Enumerable.Empty<string>();
            }

            return stringList.Select(str => str.Sanitize());
        }
    }
}