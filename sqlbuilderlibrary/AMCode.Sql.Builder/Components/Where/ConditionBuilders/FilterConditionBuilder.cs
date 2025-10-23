using System.Collections.Generic;
using AMCode.Common.Extensions.Filters;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to help build Filter BETWEEN value1 AND value2 conditions.
    /// </summary>
    public abstract class FilterConditionBuilder : IFilterConditionBuilder
    {
        /// <summary>
        /// The alias to append to the column name.
        /// </summary>
        protected string alias;

        /// <summary>
        /// The <see cref="IFilter"/> that's currently being worked on.
        /// </summary>
        protected readonly IFilter filter;

        /// <summary>
        /// Create an instance of the <see cref="FilterConditionBuilder"/> class.
        /// </summary>
        /// <param name="filter">The <see cref="IFilter"/> to use for the filter in clause.</param>
        /// <param name="alias">A <see cref="string"/> alias to attach to the column names.</param>
        public FilterConditionBuilder(IFilter filter, string alias)
        {
            this.filter = filter;
            this.alias = alias;
        }

        /// <inheritdoc/>
        public IFilterConditionSection CreateFilterClause()
        {
            // Prepare the surrounding parentheses for a filter that contains 'None' filter which is a null in the back-end
            var filterContainsNoneValue = filter.Contains(FilterItemValueTypes.None);
            var filterCount = filter.FilterItems.Count;
            var filterName = GetFilterName(filter);

            // Builds a filterName CLAUSE ...values clause or a default filter condition section.
            var filterInCondition = CreateFilterCondition(filterName, filter?.FilterItems, filter?.FilterIdName != null, alias);

            // Builds a filterName IS NULL clause or a default filter condition section.
            var filterIsNoneCondition = CreateFilterIsNoneCondition(filterName, filterCount, filterContainsNoneValue, alias);

            // Combines both clauses into a complete filter-in clause
            var filterConditionSection = MergeFilterConditions(filterInCondition, filterIsNoneCondition);

            return filterConditionSection;
        }

        /// <summary>
        /// Creates a filter clause. Example: "filterName CLAUSE ...values".
        /// </summary>
        /// <param name="filterName">The <see cref="string"/> name of the filter.</param>
        /// <param name="filterItems">A <see cref="IList{T}"/> of <see cref="IFilterItem"/>'s to use for values.</param>
        /// <param name="isIdFilter">True if this is an ID filter. False if not.</param>
        /// <param name="alias">A <see cref="string"/> alias to append to the filter name.</param>
        /// <returns>A <see cref="IFilterConditionSection"/> object updated with the provided information.</returns>
        protected abstract IFilterConditionSection CreateFilterCondition(string filterName, IList<IFilterItem> filterItems, bool isIdFilter, string alias);

        /// <inheritdoc/>
        public string GetFilterName(IFilter filter) => filter.FilterIdName != null ? filter.FilterIdName.FieldName : filter.FilterName.FieldName;

        /// <summary>
        /// Set the alias to attach to filter names.
        /// </summary>
        /// <param name="alias"></param>
        public void SetAlias(string alias) => this.alias = alias;

        /// <summary>
        /// Gets a completed filter name in clause.
        /// </summary>
        /// <param name="filterClause">A <see cref="IFilterConditionSection"/> instance if it has values to return. Otherwise, a
        /// default of <see cref="IFilterConditionSection"/>.</param>
        /// <param name="filterIsNoneClause">A <see cref="IFilterConditionSection"/> instance if it has values to return. Otherwise, a
        /// default of <see cref="IFilterConditionSection"/>.</param>
        /// <returns>A <see cref="IFilterConditionSection"/> object updated with the provided information.</returns>
        protected IFilterConditionSection MergeFilterConditions(IFilterConditionSection filterClause, IFilterConditionSection filterIsNoneClause)
        {
            if (filterClause == default(IFilterConditionSection) && filterIsNoneClause == default(IFilterConditionSection))
            {
                return default;
            }
            // If no none (-) value exists then just return a "filterName CLAUSE ...values" clause
            if (filterIsNoneClause == default(IFilterConditionSection))
            {
                return filterClause.Clone();
            } // If only a single none (-) value exists then just return a "filterName IS NULL" clause
            else if (filterIsNoneClause != default(IFilterConditionSection) && filterClause == default(IFilterConditionSection))
            {
                return filterIsNoneClause.Clone();
            } // Otherwise, return a "(filterName CLAUSE ...values OR filterName IS NULL)" clause
            else
            {
                return new FilterConditionSection
                {
                    FilterCondition = filterClause.FilterCondition.Clone(),
                    FilterSections = new List<IFilterConditionSection> { filterIsNoneClause.Clone() },
                    OperatorSeparator = "AND"
                };
            }
        }

        /// <summary>
        /// Creates a filter is null clause. Example: "filterName IS NULL" or "OR filterName IS NULL".
        /// </summary>
        /// <param name="filterName">The string name of the filter to use as the column name.</param>
        /// <param name="filterItemCount">The total number of filters available</param>
        /// <param name="hasNullValue">True if the list of filters contains a <see cref="FilterItemValueTypes.None"/> value. False, otherwise.</param>
        /// <param name="alias">A <see cref="string"/> alias to append to the filter name.</param>
        /// <returns>A <see cref="IFilterConditionSection"/> object updated with the provided information.</returns>
        protected IFilterConditionSection CreateFilterIsNoneCondition(string filterName, int filterItemCount, bool hasNullValue, string alias)
        {
            if (!hasNullValue)
            {
                return default;
            }

            return new FilterConditionSection
            {
                FilterCondition = new FilterIsCondition(filterName, () => "NULL", alias),
                OperatorSeparator = filterItemCount == 1 ? string.Empty : "OR"
            };
        }
    }
}