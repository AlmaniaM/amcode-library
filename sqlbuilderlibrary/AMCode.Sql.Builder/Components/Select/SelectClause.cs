using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands.Models;
using AMCode.Sql.Select.Extensions;

namespace AMCode.Sql.Select
{
    /// <summary>
    /// A class designed for creating a SELECT clause.
    /// </summary>
    public class SelectClause : ISelectClause
    {
        /// <summary>
        /// Create an <see cref="IEnumerable{T}"/> of <see cref="string"/>s. Each <see cref="string"/> represents
        /// a query in the SELECT clause.
        /// </summary>
        /// <param name="queryExpressionProviders">The <see cref="IEnumerable{T}"/> of <see cref="IGetQueryExpression"/>s to build the SELECT clause
        /// queries with.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of SELECT clause <see cref="string"/> queries.</returns>
        public ISelectClauseCommand CreateClause(IEnumerable<IGetQueryExpression> queryExpressionProviders)
        {
            if (queryExpressionProviders is null)
            {
                return default;
            }

            return new SelectClauseCommand(queryExpressionProviders
                .Where(queryProvider => queryProvider.IsVisible)
                .Select(column => column.GetExpression()));
        }

        /// <summary>
        /// Create an <see cref="IEnumerable{T}"/> of <see cref="string"/>s. Each <see cref="string"/> represents
        /// a query in the SELECT clause.
        /// </summary>
        /// <param name="queryExpressionProviders">The <see cref="IEnumerable{T}"/> of <see cref="IGetQueryExpression"/>s to build the SELECT clause
        /// queries with.</param>
        /// <param name="getQueryExpressionName">A custom <see cref="GetQueryExpressionNameFunction"/> function for retrieving named
        /// queries that you've stored.</param>
        /// <exception cref="NoGetQueryExpressionNameFunctionProvidedException"></exception>
        /// <returns>An <see cref="IEnumerable{T}"/> of SELECT clause <see cref="string"/> queries.</returns>
        public ISelectClauseCommand CreateClause(IEnumerable<IGetQueryExpression> queryExpressionProviders, GetQueryExpressionNameFunction getQueryExpressionName)
        {
            if (queryExpressionProviders is null)
            {
                return default;
            }

            if (getQueryExpressionName is null)
            {
                throw new NoGetQueryExpressionNameFunctionProvidedException(
                    $"[{nameof(SelectClause)}][{nameof(CreateClause)}]({nameof(queryExpressionProviders)}, {nameof(getQueryExpressionName)})",
                    string.Empty
                );
            }

            return new SelectClauseCommand(queryExpressionProviders
                .Where(queryProvider => queryProvider.IsVisible)
                .Select(column => column.GetExpression(getQueryExpressionName(column)))
            );
        }
    }
}