using System.Collections.Generic;
using System.Linq;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Extensions.FilterItems;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to create the "value1 AND value2" section of a WHERE clause "Filter BETWEEN ...values" condition section.
    /// </summary>
    public class FilterBetweenConditionValuesBuilder : IFilterConditionValueBuilder
    {
        private readonly IComparer<IFilterItem> compareFilter;
        private readonly IList<IFilterItem> filterItems;
        private readonly bool isIdFilter;

        /// <summary>
        /// Create an instance of the <see cref="FilterInConditionValuesBuilder"/> class.
        /// </summary>
        /// <param name="filterItems">The <see cref="IList{T}"/> of <see cref="IFilterItem"/>s to build the WHERE clause
        /// condition section values from.</param>
        /// <param name="compareFilter">An <see cref="IComparer{T}"/> of <see cref="IFilterItem"/> objects.</param>
        /// <param name="isIdFilter">Whether or not the <see cref="IFilterItem"/>s have an <see cref="IFilterItem.FilterId"/> value.</param>
        public FilterBetweenConditionValuesBuilder(IList<IFilterItem> filterItems, IComparer<IFilterItem> compareFilter, bool isIdFilter)
        {
            this.filterItems = filterItems;
            this.compareFilter = compareFilter;
            this.isIdFilter = isIdFilter;
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                var validFilterItems = this.validFilterItems;
                var count = validFilterItems.Count;

                return count > 1 ? 2 : count;
            }
        }

        /// <summary>
        /// Creates a BETWEEN clause section for a single filter. 
        /// 
        /// Example:
        /// if <seealso cref="IFilter"/> is a SalesDate then the generated filter will
        /// be -> SalesDate BETWEEN '2022-03-01' AND '2022-03-07'
        /// 
        /// </summary>
        /// <returns>A string representing a single field BETWEEN 'value1' AND 'value2' clause.</returns>
        public string CreateFilterConditionValue()
        {
            var validFilterItems = this.validFilterItems;

            if (validFilterItems.Count == 0)
            {
                return string.Empty;
            }

            if (validFilterItems.Count == 1)
            {
                return validFilterItems.First().SanitizeFilterValue(isIdFilter);
            }

            var orderedFilterItems = validFilterItems.OrderBy(filterItem => filterItem, compareFilter).ToList();
            var leftFilterItem = orderedFilterItems.First();
            var rightFilterItem = orderedFilterItems.Last();

            return $"{leftFilterItem.SanitizeFilterValue(isIdFilter)} AND {rightFilterItem.SanitizeFilterValue(isIdFilter)}";
        }

        /// <summary>
        /// Check if there are any filter values.
        /// </summary>
        /// <returns>Will be true if there are any values. And false if not.</returns>
        public bool HasValues() => validFilterItems.Count > 0;

        /// <summary>
        /// Filter out any bad values.
        /// </summary>
        private List<IFilterItem> validFilterItems => filterItems.Where(filterItem => !filterItem.IsNoneValue(isIdFilter)).ToList() ?? new List<IFilterItem>();
    }
}