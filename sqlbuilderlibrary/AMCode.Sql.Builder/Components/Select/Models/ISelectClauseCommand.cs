using System.Collections.Generic;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.Select
{
    /// <summary>
    /// An interface representing a SELECT clause command object.
    /// </summary>
    public interface ISelectClauseCommand : IClauseCommand
    {
        /// <summary>
        /// Set a query expression. You cannot have commas at the beginning or end of the string. All whitespace will
        /// be trimmed at the starting and ending of the query expression.
        /// </summary>
        /// <param name="expression">A column query expression.</param>
        void AddQueryExpression(string expression);

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="string"/> column query expressions.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of column query expressions.</returns>
        IEnumerable<string> GetQueryExpressions();

        /// <summary>
        /// Set a collection of query expressions. You cannot have commas at the beginning or end of a <see cref="string"/>. All whitespace will
        /// be trimmed at the starting and ending of each query expression.
        /// </summary>
        /// <param name="queryExpressions">A collection of column query expression.</param>
        void SetQueryExpressions(IEnumerable<string> queryExpressions);
    }
}