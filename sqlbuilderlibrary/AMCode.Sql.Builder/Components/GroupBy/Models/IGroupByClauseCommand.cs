using System.Collections.Generic;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.GroupBy
{
    /// <summary>
    /// An interface the represents a GROUP BY clause command.
    /// </summary>
    public interface IGroupByClauseCommand : IClauseCommand
    {
        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="string"/> GROUP BY expressions.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="string"/> GROUP BY expressions.</returns>
        IEnumerable<string> GetGroupByExpressions();

        /// <summary>
        /// Set a GROUP BY expression. You cannot have commas at the beginning or end of string. All whitespace will
        /// be trimmed at the starting and ending of the GROUP BY expression.
        /// </summary>
        /// <param name="expression">A GROUP BY expression.</param>
        void SetGroupByExpression(string expression);
    }
}