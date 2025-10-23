using System.Collections.Generic;
using AMCode.Common.Extensions.Strings;

namespace AMCode.Sql.Commands.Extensions
{
    static class CommandExtensions
    {
        /// <summary>
        /// Ends a command by inserting a semicolon as the last command.
        /// </summary>
        /// <param name="list">The list to add the semicolon to.</param>
        /// <param name="endCommand">Whether or not to insert a semicolon (;).</param>
        /// <returns>An updated <see cref="IList{T}"/> of <see cref="string"/> commands.</returns>
        public static IList<string> EndCommand(this IList<string> list, bool endCommand)
        {
            if (list is null || list.Count == 0)
            {
                return list;
            }

            if (!endCommand)
            {
                return list;
            }

            list.Add(";");

            return list;
        }

        /// <summary>
        /// Adds a command to the list.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of <see cref="string"/>s to update.</param>
        /// <param name="command">The command value to add.</param>
        /// <returns>An updated <see cref="IList{T}"/> of <see cref="string"/> commands.</returns>
        public static IList<string> AddCommand(this IList<string> list, string command)
        {
            if (command.IsNullEmptyOrWhiteSpace())
            {
                return list;
            }

            list.Add(command);

            return list;
        }

        /// <summary>
        /// Adds a command to the list.
        /// </summary>
        /// <param name="list">The <see cref="IList{T}"/> of <see cref="string"/>s to update.</param>
        /// <param name="key">The command key to add. If the command value already contains the key then this key
        /// will not be added.</param>
        /// <param name="value">The command value to add.</param>
        /// <returns>An updated <see cref="IList{T}"/> of <see cref="string"/> commands.</returns>
        public static IList<string> AddCommand(this IList<string> list, string key, string value)
        {
            if (value.IsNullEmptyOrWhiteSpace())
            {
                return list;
            }

            if (value.Length >= key.Length && value.Substring(0, key.Length).ToLowerInvariant().Equals(key.ToLowerInvariant()))
            {
                list.Add(value);
            }
            else
            {
                list.Add($"{key} {value}");
            }

            return list;
        }
    }
}