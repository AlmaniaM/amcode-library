using System.Collections.Generic;
using AMCode.Common.FilterStructures;
using AMCode.Sql.Where.Models;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// An interface representing a WHERE clause builder.
    /// </summary>
    public interface IWhereClause
    {
        /// <summary>
        /// Create a <see cref="IWhereClauseCommand"/> object based on the provided list of <see cref="IFilter"/>'s.
        /// </summary>
        /// <param name="selectedFilters">A <see cref="IList{T}"/> of <see cref="IFilter"/> that should be included in the WHERE clause.</param>
        /// <returns>A <see cref="IWhereClauseCommand"/> object that you can append to a SQL SELECT query and filter down to the
        /// provided selected <see cref="IList{T}"/> of <see cref="IFilter"/>'s.</returns>
        IWhereClauseCommand CreateClause(IList<IFilter> selectedFilters);

        /// <summary>
        /// Create a <see cref="IWhereClauseCommand"/> object based on the provided list of <see cref="IFilter"/>'s.
        /// </summary>
        /// <param name="selectedFilters">A <see cref="IList{T}"/> of <see cref="IFilter"/> that should be included in the WHERE clause.</param>
        /// <param name="alias">A <see cref="string"/> alias to assign to the SQL WHERE clause column names</param>
        /// <returns>A <see cref="IWhereClauseCommand"/> object that you can append to a SQL SELECT query and filter down to the
        /// provided selected <see cref="IList{T}"/> of <see cref="IFilter"/>'s.</returns>
        IWhereClauseCommand CreateClause(IList<IFilter> selectedFilters, string alias);

        /// <summary>
        /// Create a <see cref="IWhereClauseCommand"/> object based on the provided list of <see cref="IFilter"/>'s.
        /// </summary>
        /// <param name="whereClauseParam">An instance of a <see cref="IWhereClauseParam"/> with selected filters
        /// and the last selected filter.</param>
        /// <returns>A <see cref="IWhereClauseCommand"/> object that you can append to a SQL SELECT query and filter down to the
        /// provided selected <see cref="IList{T}"/> of <see cref="IFilter"/>'s.</returns>
        IWhereClauseCommand CreateClause(IWhereClauseParam whereClauseParam);

        /// <summary>
        /// Create a <see cref="IWhereClauseCommand"/> object based on the provided list of <see cref="IFilter"/>'s.
        /// </summary>
        /// <param name="whereClauseParam">An instance of a <see cref="IWhereClauseParam"/> with selected filters
        /// and the last selected filter.</param>
        /// <param name="alias">A <see cref="string"/> alias to assign to the WHERE clause column names.</param>
        /// <returns>A <see cref="IWhereClauseCommand"/> object that you can append to a SQL SELECT query and filter down to the
        /// provided selected <see cref="IList{T}"/> of <see cref="IFilter"/>'s.</returns>
        IWhereClauseCommand CreateClause(IWhereClauseParam whereClauseParam, string alias);
    }
}