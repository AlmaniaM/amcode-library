using System.Collections.Generic;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.OrderBy.Models;

namespace AMCode.Sql.OrderBy
{
    /// <summary>
    /// An interface representing an ORDER BY clause.
    /// </summary>
    public interface IOrderByClause
    {
        /// <summary>
        /// Create an <see cref="IOrderByClauseCommand"/> object.
        /// </summary>
        /// <param name="sortProviders">The <see cref="IEnumerable{T}"/> of <see cref="ISortProvider"/>s to build the
        /// <see cref="IOrderByClauseCommand"/> object with.</param>
        /// <returns>An <see cref="IOrderByClauseCommand"/> object.</returns>
        IOrderByClauseCommand CreateClause(IEnumerable<ISortProvider> sortProviders);

        /// <summary>
        /// Create an <see cref="IOrderByClauseCommand"/> object.
        /// </summary>
        /// <param name="sortProviders">The <see cref="IEnumerable{T}"/> of <see cref="ISortProvider"/>s to build the
        /// <see cref="IOrderByClauseCommand"/> object with.</param>
        /// <param name="getSortFormatterName">A custom <see cref="GetSortFormatterNameFunction"/> function for retrieving named
        /// <see cref="IColumnSortFormatter"/>s that you've stored.</param>
        /// <returns>An <see cref="IOrderByClauseCommand"/> object.</returns>
        IOrderByClauseCommand CreateClause(IEnumerable<ISortProvider> sortProviders, GetSortFormatterNameFunction getSortFormatterName);
    }
}