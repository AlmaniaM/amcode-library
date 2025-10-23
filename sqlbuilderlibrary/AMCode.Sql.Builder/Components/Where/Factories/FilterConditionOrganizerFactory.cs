using AMCode.Common.FilterStructures;
using AMCode.Common.Util;
using AMCode.Sql.Where.Exceptions;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// A factory class designed to create an <see cref="IFilterConditionOrganizer"/> object.
    /// </summary>
    public class FilterConditionOrganizerFactory : IFilterConditionOrganizerFactory
    {
        /// <summary>
        /// Create an instance of an object implementing <see cref="IFilterConditionOrganizer"/>.
        /// </summary>
        /// <param name="filter">The <see cref="IFilter"/> to build the "Filter IN" section.</param>
        /// <param name="lastSelectedFilter">A <see cref="string"/> representing the last selected filter.</param>
        /// <param name="whereClauseBuilderType">The <see cref="WhereClauseBuilderType"/>.</param>
        /// <param name="alias">A <see cref="string"/> alias to attach to the column names.</param>
        /// <exception cref="NoSuchFilterConditionBuilderException"></exception>
        /// <returns>A <see cref="string"/> "Filter IN" section.</returns>
        public IFilterConditionOrganizer Create(IFilter filter, string lastSelectedFilter, WhereClauseBuilderType whereClauseBuilderType, string alias)
        {
            if (whereClauseBuilderType == WhereClauseBuilderType.Data)
            {
                return new DefaultFilterConditionOrganizer(new FilterInConditionBuilder(filter, lastSelectedFilter, alias));
            }
            else if (whereClauseBuilderType == WhereClauseBuilderType.GlobalFilters)
            {
                return new GlobalFiltersFilterConditionOrganizer(new FilterInConditionBuilder(filter, lastSelectedFilter, alias), filter, lastSelectedFilter);
            }
            else
            {
                var header = ExceptionUtil.CreateExceptionHeader<IFilter, string, WhereClauseBuilderType, string, IFilterConditionOrganizer>(Create);
                throw new NoSuchFilterConditionBuilderException(header, $"{whereClauseBuilderType}");
            }
        }
    }
}