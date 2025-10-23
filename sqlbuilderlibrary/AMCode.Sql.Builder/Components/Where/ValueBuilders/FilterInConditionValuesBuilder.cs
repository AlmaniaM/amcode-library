using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Extensions.FilterItems;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Extensions.FilterItems;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to create the "(...values)" section of a WHERE clause "Filter IN (...values)" condition section.
    /// </summary>
    public class FilterInConditionValuesBuilder : IFilterConditionValueBuilder
    {
        private readonly IList<IFilterItem> filterItems;
        private readonly bool isIdFilter;

        /// <summary>
        /// Create an instance of the <see cref="FilterInConditionValuesBuilder"/> class.
        /// </summary>
        /// <param name="filterItems">The <see cref="IList{T}"/> of <see cref="IFilterItem"/>s to build the WHERE clause
        /// condition section values from.</param>
        /// <param name="isIdFilter">Whether or not the <see cref="IFilterItem"/>s have an <see cref="IFilterItem.FilterId"/> value.</param>
        public FilterInConditionValuesBuilder(IList<IFilterItem> filterItems, bool isIdFilter)
        {
            this.filterItems = filterItems;
            this.isIdFilter = isIdFilter;
        }

        /// <inheritdoc/>
        public int Count => filterItems.Where(filterItem => !filterItem.IsNoneValue(isIdFilter)).Count();

        /// <summary>
        /// Creates an IN clause section for a single filter. 
        /// 
        /// Example:
        /// if <seealso cref="IFilter"/> is an ItemID then the generated filter will
        /// be -> ItemID IN ('1', '2', '3') etc...
        /// 
        /// </summary>
        /// <returns>A string representing a single field IN ('', '', '', etc...) clause.</returns>
        public string CreateFilterConditionValue()
        {
            if (filterItems.Count == 1)
            {
                if (filterItems.Contains(FilterItemValueTypes.None, isIdFilter))
                {
                    return string.Empty;
                }
            }

            var filterValues = filterItems
                .Where(filterItem => !filterItem.IsNoneValue(isIdFilter))
                .Select(filterItem => filterItem.SanitizeFilterValue(isIdFilter)).ToList();

            // If no filter values were produced then return an empty string.
            if (filterValues.Count == 0)
            {
                return string.Empty;
            }

            var filterValuesString = string.Join(",", filterValues);
            return filterValuesString;
        }

        /// <summary>
        /// Check if there are any filter values.
        /// </summary>
        /// <returns>Will be true if there are any values. And false if not.</returns>
        public bool HasValues() => filterItems?.Count > 0;
    }
}