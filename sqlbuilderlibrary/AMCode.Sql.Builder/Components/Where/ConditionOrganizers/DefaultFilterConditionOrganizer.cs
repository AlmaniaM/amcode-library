using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// A class designed to build filter in clause sections with default behavior.
    /// </summary>
    public class DefaultFilterConditionOrganizer : IFilterConditionOrganizer
    {
        private readonly IFilterConditionBuilder filterConditionBuilder;

        /// <summary>
        /// Create an instance of the <see cref="DefaultFilterConditionOrganizer"/> class.
        /// </summary>
        /// <param name="filterConditionBuilder">Provide an <see cref="IFilterConditionBuilder"/> for creating
        /// the <see cref="IFilterConditionSection"/>s.</param>
        public DefaultFilterConditionOrganizer(IFilterConditionBuilder filterConditionBuilder)
        {
            this.filterConditionBuilder = filterConditionBuilder;
        }

        /// <summary>
        /// Creates a complete filter IN clause. Example: "filterName IN (...values)" or
        /// "(filterName IN (...values) OR filterName IS NULL)" or "filterName IS NULL".
        /// </summary>
        /// <param name="whereClauseBuilder">An <see cref="IWhereClauseBuilder"/> object that a "Filter IN" clause will be added to.</param>
        public void AddFilterCondition(IWhereClauseBuilder whereClauseBuilder)
        {
            var filterConditionSection = filterConditionBuilder.CreateFilterClause();
            addFilterCondition(whereClauseBuilder, filterConditionSection);
        }

        /// <summary>
        /// Add a the <see cref="IFilterConditionSection"/> to the provided <see cref="IWhereClauseBuilder"/> object
        /// using the <see cref="FilterConditionSectionType.Default"/> options.
        /// </summary>
        /// <param name="whereClauseBuilder">The <see cref="IWhereClauseBuilder"/> object to add a <see cref="IFilterConditionSection"/> to.</param>
        /// <param name="filterConditionSection">The <see cref="IFilterConditionSection"/> to add.</param>
        private void addFilterCondition(IWhereClauseBuilder whereClauseBuilder, IFilterConditionSection filterConditionSection)
            => whereClauseBuilder.AddFilterCondition(filterConditionSection, FilterConditionSectionType.Default);
    }
}