using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to build an IN clause inside the WHERE clause for global filters.
    /// </summary>
    public class GlobalFiltersFilterConditionOrganizer : IFilterConditionOrganizer
    {
        private readonly IFilter filter;
        private readonly IFilterConditionBuilder filterConditionBuilder;
        private readonly string lastSelectedFilterName;

        /// <summary>
        /// Create an instance of the <see cref="GlobalFiltersFilterConditionOrganizer"/> class. This class will build
        /// an IN clause for the provided <see cref="IFilter"/>.
        /// </summary>
        /// <param name="filterConditionBuilder">Provide an <see cref="IFilterConditionBuilder"/> for creating
        /// the <see cref="IFilterConditionSection"/>s.</param>
        /// <param name="filter">An <see cref="IFilter"/> object to build the IN clause from.</param>
        /// <param name="lastSelectedFilterName">The field name of the last selected filter.</param>
        public GlobalFiltersFilterConditionOrganizer(IFilterConditionBuilder filterConditionBuilder, IFilter filter, string lastSelectedFilterName)
        {
            this.filterConditionBuilder = filterConditionBuilder;
            this.filter = filter;
            this.lastSelectedFilterName = lastSelectedFilterName;
        }

        /// <summary>
        /// Creates a complete filter IN clause. Example: "filterName IN (...values)" or
        /// "(filterName IN (...values) OR filterName IS NULL) OR (filterName IN (...values) OR filterName IS NULL)" or "filterName IS NULL".
        /// </summary>
        /// <param name="whereClauseBuilder">An <see cref="IWhereClauseBuilder"/> object that a "Filter IN" clause will be added to.</param>
        public void AddFilterCondition(IWhereClauseBuilder whereClauseBuilder)
        {
            var filterConditionSection = filterConditionBuilder.CreateFilterClause();
            addFilterCondition(whereClauseBuilder, filterConditionSection);
        }

        /// <summary>
        /// Add a <see cref="IFilterConditionSection"/> to the provided <see cref="IWhereClauseBuilder"/>. If the current filter is the
        /// also the last selected filter than it will add itself as the <see cref="FilterConditionSectionType.Default"/> and
        /// <see cref="FilterConditionSectionType.LastSelected"/> options. Otherwise, only as the <see cref="FilterConditionSectionType.Default"/> option.
        /// </summary>
        /// <param name="whereClauseBuilder">The <see cref="IWhereClauseBuilder"/> object to add a <see cref="IFilterConditionSection"/> to.</param>
        /// <param name="filterConditionSection">The <see cref="IFilterConditionSection"/> to add.</param>
        private void addFilterCondition(IWhereClauseBuilder whereClauseBuilder, IFilterConditionSection filterConditionSection)
        {
            var filterName = filterConditionBuilder.GetFilterName(filter);

            // Add to the where clause object.
            whereClauseBuilder.AddFilterCondition(filterConditionSection, FilterConditionSectionType.Default);
            if (filterName.Equals(lastSelectedFilterName))
            {
                whereClauseBuilder.AddFilterCondition(filterConditionSection, FilterConditionSectionType.LastSelected);
            }
        }
    }
}