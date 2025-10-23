using AMCode.Common.Extensions.Strings;

namespace AMCode.Sql.Helpers
{
    internal class AliasUtils
    {
        /// <summary>
        /// Append a dot to the end of the alias if it doesn't have one.
        /// </summary>
        /// <param name="alias">The <see cref="string"/> alias to check.</param>
        /// <returns>The alias with a dot (.) appended to it if id didn't have one. If null, <see cref="string.Empty"/>, whitespace,
        /// or already has a dot then the original alias will be returned.</returns>
        public static string GetUpdatedAlias(string alias)
        {
            if (alias.IsNullEmptyOrWhiteSpace())
            {
                return alias;
            }

            if (alias[alias.Length - 1] == '.')
            {
                return alias;
            }

            return $"{alias}.";
        }
    }
}