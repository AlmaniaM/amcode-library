using AMCode.Common.Extensions.FilterItems;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Extensions.FilterItems
{
    /// <summary>
    /// A static class designed to hold extension methods for <see cref="IFilterItem"/> objects.
    /// </summary>
    public static class FilterItemExtensions
    {
        /// <summary>
        /// Get the <see cref="IFilterItem.FilterVal"/> if <paramref name="isIdFilter"/> is false
        /// or <see cref="IFilterItem.FilterId"/> if <paramref name="isIdFilter"/> is true.
        /// </summary>
        /// <param name="filterItem">The <see cref="IFilterItem"/> object to read the value from.</param>
        /// <param name="isIdFilter">Whether or not the filter is an ID filter.</param>
        /// <returns>A <see cref="string"/> value.</returns>
        public static string GetValue(this IFilterItem filterItem, bool isIdFilter)
            => isIdFilter ? filterItem?.FilterId : filterItem?.FilterVal;

        /// <summary>
        /// Check to see if the <see cref="IFilterItem"/> value is of type <see cref="FilterItemValueTypes.None"/>.
        /// </summary>
        /// <param name="filterItem">The <see cref="IFilterItem"/> object to read the value from.</param>
        /// <param name="isIdFilter">Whether or not the filter is an ID filter.</param>
        /// <returns>True if the main value is of type <see cref="FilterItemValueTypes.None"/>. False if not.</returns>
        public static bool IsNoneValue(this IFilterItem filterItem, bool isIdFilter)
            => filterItem.Contains(FilterItemValueTypes.None, isIdFilter);

        /// <summary>
        /// Standardized method for creating a filter item value for a WHERE clause.
        /// </summary>
        /// <param name="filterItem">The <see cref="IFilterItem"/> object to read the value from.</param>
        /// <param name="isIdFilter">Whether or not the filter is an ID filter.</param>
        /// <returns>A <see cref="string"/> value ready for the WHERE clause.</returns>
        public static string SanitizeFilterValue(this IFilterItem filterItem, bool isIdFilter)
        {
            // Use the filter ID if it's available instead of the filterVal which could be a description
            if (isIdFilter && !string.IsNullOrEmpty(filterItem.FilterId))
            {
                if (int.TryParse($"{filterItem.FilterId}", out var intValId))
                {
                    return $"{intValId}";
                }
                else if (decimal.TryParse($"{filterItem.FilterId}", out var decimalValId))
                {
                    return $"{decimalValId}";
                }
                else
                {
                    return $"'{filterItem.FilterId}'";
                }
            }

            // Used to account for quotes (') in the filter values that are strings
            var newFilterValue = filterItem.FilterVal != null ? filterItem.FilterVal.Replace("'", "''") : filterItem.FilterVal;
            return $"'{newFilterValue}'";
        }
    }
}