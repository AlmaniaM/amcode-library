using System.Collections.Generic;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.Select
{
    /// <summary>
    /// An interface representing a SELECT clause builder.
    /// </summary>
    public interface ISelectClause
    {
        /// <summary>
        /// Create a <see cref="ISelectClauseCommand"/> object.
        /// </summary>
        /// <param name="queryExpressionProviders">The <see cref="IEnumerable{T}"/> of <see cref="IGetQueryExpression"/>s
        /// to build the <see cref="ISelectClauseCommand"/> object with.</param>
        /// <returns>An <see cref="ISelectClauseCommand"/> object.</returns>
        ISelectClauseCommand CreateClause(IEnumerable<IGetQueryExpression> queryExpressionProviders);

        /// <summary>
        /// Create a <see cref="ISelectClauseCommand"/> object.
        /// </summary>
        /// <param name="queryExpressionProviders">The <see cref="IEnumerable{T}"/> of <see cref="IGetQueryExpression"/>s
        /// to build the <see cref="ISelectClauseCommand"/> object with.</param>
        /// <param name="getQueryExpressionName">A custom <see cref="GetQueryExpressionNameFunction"/> function for retrieving named
        /// queries that you've stored.</param>
        /// <returns>An <see cref="ISelectClauseCommand"/> object.</returns>
        ISelectClauseCommand CreateClause(IEnumerable<IGetQueryExpression> queryExpressionProviders, GetQueryExpressionNameFunction getQueryExpressionName);
    }
}