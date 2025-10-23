using System.Collections.Generic;
using System.Linq;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Extensions.FilterItems;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to help build Filter BETWEEN value1 AND value2 conditions.
    /// </summary>
    public class FilterBetweenConditionBuilder : FilterConditionBuilder, IFilterConditionBuilder
    {
        private readonly IComparerFactory comparerFactory;
        private readonly bool greaterOrEqualAsDefault;

        /// <summary>
        /// Create an instance of the <see cref="FilterBetweenConditionBuilder"/> class.
        /// </summary>
        /// <param name="filter">The <see cref="IFilter"/> to use for the filter in clause.</param>
        /// <param name="comparerFactory">An <see cref="IComparerFactory"/> for creating an <see cref="IComparer{T}"/> of type
        /// <see cref="IFilterItem"/> for ordering values when building the BETWEEN clause values.</param>
        /// <param name="alias">A <see cref="string"/> alias to attach to the column names.</param>
        public FilterBetweenConditionBuilder(IFilter filter, IComparerFactory comparerFactory, string alias)
            : base(filter, alias)
        {
            this.comparerFactory = comparerFactory;
            greaterOrEqualAsDefault = true;
        }

        /// <summary>
        /// Create an instance of the <see cref="FilterBetweenConditionBuilder"/> class.
        /// </summary>
        /// <param name="filter">The <see cref="IFilter"/> to use for the filter in clause.</param>
        /// <param name="comparerFactory">An <see cref="IComparerFactory"/> for creating an <see cref="IComparer{T}"/> of type
        /// <see cref="IFilterItem"/> for ordering values when building the BETWEEN clause values.</param>
        /// <param name="alias">A <see cref="string"/> alias to attach to the column names.</param>
        /// <param name="greaterOrEqualAsDefault">If <c>true</c> then "greater-than-equal" will be used if only one value exists. If <c>false</c>
        /// then "less-than-equal" will be used if only one value exists from the <see cref="IFilterConditionOrganizer.AddFilterCondition(IWhereClauseBuilder)"/>
        /// invocation.</param>
        public FilterBetweenConditionBuilder(IFilter filter, IComparerFactory comparerFactory, string alias, bool greaterOrEqualAsDefault)
            : base(filter, alias)
        {
            this.comparerFactory = comparerFactory;
            this.greaterOrEqualAsDefault = greaterOrEqualAsDefault;
        }

        /// <summary>
        /// Creates a filter BETWEEN clause. Example: "filterName BETWEEN 1 AND 2" or "filterName BETWEEN '02/03/2022' AND '02/30/2022'".
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
            var filterItemsWithoutNoneValues = filter.FilterItems.Where(filterItem => !filterItem.IsNoneValue(isIdFilter)).ToList();

            if (filterItemsWithoutNoneValues.Count == 0)
            {
                return default;
            }

            return new FilterConditionSection
            {
                FilterCondition = new FilterBetweenCondition(
                    filterName,
                    new FilterBetweenConditionValuesBuilder(filterItemsWithoutNoneValues, comparerFactory.Create(filter, isIdFilter), isIdFilter),
                    alias,
                    greaterOrEqualAsDefault),
                OperatorSeparator = "AND"
            };
        }
    }
}