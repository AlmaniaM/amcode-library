using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Internal;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// An interface representing a factory for building filter condition builders.
    /// </summary>
    public interface IFilterConditionOrganizerFactory
    {
        /// <summary>
        /// Create an instance of an object implementing <see cref="IFilterConditionOrganizer"/>.
        /// </summary>
        /// <param name="filter">The <see cref="IFilter"/> to build the "Filter IN" section.</param>
        /// <param name="lastSelectedFilter">A <see cref="string"/> representing the last selected filter.</param>
        /// <param name="whereClauseBuilderType">The <see cref="WhereClauseBuilderType"/>.</param>
        /// <param name="alias">A <see cref="string"/> alias to attach to the column names.</param>
        /// <returns>A <see cref="string"/> "Filter IN" section.</returns>
        IFilterConditionOrganizer Create(IFilter filter, string lastSelectedFilter, WhereClauseBuilderType whereClauseBuilderType, string alias);
    }
}