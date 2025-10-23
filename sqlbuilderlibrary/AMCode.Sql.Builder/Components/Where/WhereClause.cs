using System;
using System.Collections.Generic;
using System.Linq;
using AMCode.Common.Extensions.Enumerables;
using AMCode.Common.Extensions.Strings;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// A class to create a SQL WHERE clause builder.
    /// </summary>
    public class WhereClause : IWhereClause
    {
        private readonly IFilterConditionOrganizerFactory filterConditionBuilderFactory;
        private readonly IWhereClauseBuilderFactory whereClauseBuilderFactory;
        private readonly WhereClauseBuilderType whereClauseBuilderType;

        /// <summary>
        /// Create an instance of the <see cref="WhereClause"/> class.
        /// </summary>
        /// <param name="whereClauseBuilderFactory">An instance of a <see cref="IWhereClauseBuilderFactory"/>.</param>
        /// <param name="filterConditionBuilderFactory">An instance of a <see cref="IFilterConditionOrganizerFactory"/>.</param>
        /// <param name="whereClauseBuilderType">The type of WHERE clause you're constructing.</param>
        public WhereClause(IWhereClauseBuilderFactory whereClauseBuilderFactory, IFilterConditionOrganizerFactory filterConditionBuilderFactory, WhereClauseBuilderType whereClauseBuilderType)
        {
            this.filterConditionBuilderFactory = filterConditionBuilderFactory;
            this.whereClauseBuilderFactory = whereClauseBuilderFactory;
            this.whereClauseBuilderType = whereClauseBuilderType;
        }

        /// <inheritdoc/>
        public IWhereClauseCommand CreateClause(IList<IFilter> selectedFilters) => createWhereClause(selectedFilters, string.Empty, string.Empty);

        /// <inheritdoc/>
        public IWhereClauseCommand CreateClause(IList<IFilter> selectedFilters, string alias) => createWhereClause(selectedFilters, string.Empty, alias);

        /// <inheritdoc/>
        public IWhereClauseCommand CreateClause(IWhereClauseParam selectedFiltersParam) => CreateClause(selectedFiltersParam, string.Empty);

        /// <inheritdoc/>
        public IWhereClauseCommand CreateClause(IWhereClauseParam selectedFiltersParam, string alias) => createWhereClause(selectedFiltersParam, alias);

        /// <summary>
        /// Create a <see cref="IWhereClauseCommand"/> object.
        /// </summary>
        /// <param name="selectedFiltersParam"></param>
        /// <param name="alias">A <see cref="string"/> alias to assign to the SQL WHERE clause column names</param>
        /// <returns>A <see cref="IWhereClauseCommand"/> that you can append to a SQL SELECT query and filter down to the
        /// provided selected <see cref="IList{T}"/> of <see cref="IFilter"/>'s.</returns>
        private IWhereClauseCommand createWhereClause(IWhereClauseParam selectedFiltersParam, string alias)
        {
            if (selectedFiltersParam == null)
            {
                throw new NullReferenceException($"[{nameof(WhereClause)}][{nameof(CreateClause)}]({nameof(selectedFiltersParam)}, {nameof(alias)}) Error: {nameof(selectedFiltersParam)} must not be null.");
            }

            if (selectedFiltersParam.SelectedFilters == null)
            {
                throw new NullReferenceException($"[{nameof(WhereClause)}][{nameof(CreateClause)}]({nameof(selectedFiltersParam)}, {nameof(alias)}) Error: {nameof(selectedFiltersParam.SelectedFilters)} must not be null.");
            }

            if (selectedFiltersParam.LastSelectedFilter.IsNullEmptyOrWhiteSpace())
            {
                throw new NullReferenceException($"[{nameof(WhereClause)}][{nameof(CreateClause)}]({nameof(selectedFiltersParam)}, {nameof(alias)}) Error: {nameof(selectedFiltersParam.LastSelectedFilter)} must not be null, empty, or white space.");
            }

            return createWhereClause(selectedFiltersParam.SelectedFilters, selectedFiltersParam.LastSelectedFilter, alias);
        }

        /// <summary>
        /// Create a <see cref="IWhereClauseCommand"/> object based on the provided list of <see cref="IFilter"/>'s.
        /// </summary>
        /// <param name="selectedFilters">A <see cref="IList{T}"/> of <see cref="IFilter"/> that should be included in the WHERE clause.</param>
        /// <param name="lastSelectedFilter">The name of the <see cref="IFilter"/> that was last selected.</param>
        /// <param name="alias">A <see cref="string"/> alias to assign to the SQL WHERE clause column names</param>
        /// <returns>A <see cref="IWhereClauseCommand"/> object that you can append to a SQL SELECT query and filter down to the
        /// provided selected <see cref="IList{T}"/> of <see cref="IFilter"/>'s.</returns>
        private IWhereClauseCommand createWhereClause(IList<IFilter> selectedFilters, string lastSelectedFilter, string alias)
        {
            if (!selectedFilters.Any())
            {
                return default;
            }

            var whereClauseBuilder = whereClauseBuilderFactory.Create(whereClauseBuilderType);

            selectedFilters
                .Select(filter => filterConditionBuilderFactory.Create(filter, lastSelectedFilter, whereClauseBuilderType, alias))
                .ForEach((IFilterConditionOrganizer conditionBuilder) => conditionBuilder.AddFilterCondition(whereClauseBuilder));

            return whereClauseBuilder.CreateWhereClause();
        }
    }
}