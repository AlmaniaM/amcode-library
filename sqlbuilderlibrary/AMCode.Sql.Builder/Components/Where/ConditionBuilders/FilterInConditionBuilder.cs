using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Extensions.FilterItems;
using AMCode.Common.Extensions.Filters;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to help build Filter IN (...values) conditions.
    /// </summary>
    public class FilterInConditionBuilder : FilterConditionBuilder, IFilterConditionBuilder
    {
        /// <summary>
        /// The name of the last selected filter.
        /// </summary>
        protected readonly string lastSelectedFilterName;

        /// <summary>
        /// Create an instance of the <see cref="FilterInConditionBuilder"/> class.
        /// </summary>
        /// <param name="filter">The <see cref="IFilter"/> to use for the filter in clause.</param>
        /// <param name="lastSelectedFilterName">The field name of the last selected filter.</param>
        /// <param name="alias">A <see cref="string"/> alias to attach to the column names.</param>
        public FilterInConditionBuilder(IFilter filter, string lastSelectedFilterName, string alias)
            : base(filter, alias)
        {
            this.lastSelectedFilterName = lastSelectedFilterName;
        }

        /// <summary>
        /// Creates a filter in clause. Example: "filterName IN (1,2,3)" or "filterName IN ('value1', 'value''2', 'value3')".
        /// </summary>
        /// <param name="filterName">The <see cref="string"/> name of the filter.</param>
        /// <param name="filterItems">A <see cref="IList{T}"/> of <see cref="IFilterItem"/>'s to use for values.</param>
        /// <param name="isIdFilter">True if this is an ID filter. False if not.</param>
        /// <param name="alias">A <see cref="string"/> alias to append to the filter name.</param>
        /// <returns>A <see cref="IFilterConditionSection"/> object updated with the provided information.</returns>
        protected override IFilterConditionSection CreateFilterCondition(string filterName, IList<IFilterItem> filterItems, bool isIdFilter, string alias)
        {
            if (filterItems == null || filterItems.Count == 0)
            {
                return default;
            }

            // Get all filters that aren't the none (-) value
            var filterItemsWithNoNoneValues = filter.FilterItems.Where(filterItem => !filterItem.Contains(FilterItemValueTypes.None, filter.FilterIdName != null)).ToList();

            if (filterItemsWithNoNoneValues.Count == 0)
            {
                return default;
            }

            return new FilterConditionSection
            {
                FilterCondition = new FilterInCondition(filterName, new FilterInConditionValuesBuilder(filterItemsWithNoNoneValues, isIdFilter), alias),
                OperatorSeparator = "AND"
            };
        }
    }
}