using System.Collections.Generic;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.OrderBy
{
    /// <summary>
    /// An interface that represents an ORDER BY clause.
    /// </summary>
    public interface IOrderByClauseCommand : IClauseCommand
    {
        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="string"/> ORDER BY expressions.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of ORDER BY expressions.</returns>
        IEnumerable<string> GetOrderByExpressions();

        /// <summary>
        /// Set a ORDER BY expression. You cannot have commas at the beginning or end of string. All whitespace will
        /// be trimmed at the starting and ending of the ORDER BY expression.
        /// </summary>
        /// <param name="expression">A ORDER BY expression.</param>
        void SetOrderByExpression(string expression);
    }
}