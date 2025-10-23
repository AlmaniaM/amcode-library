using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.Commands
{
    /// <summary>
    /// A class designed to act as a base class for creating column expressions.
    /// </summary>
    public abstract class ColumnExpressionCommandBase : IClauseCommand
    {
        /// <summary>
        /// An <see cref="IList{T}"/> of expressions.
        /// </summary>
        protected IList<string> Expressions { get; set; }

        /// <summary>
        /// Create an instance of an <see cref="ColumnExpressionCommandBase"/> class.
        /// </summary>
        public ColumnExpressionCommandBase()
        {
            Expressions = new List<string>();
        }

        /// <summary>
        /// Create an instance of an <see cref="ColumnExpressionCommandBase"/> class.
        /// </summary>
        /// <param name="expressions">A <see cref="IList{T}"/> of <see cref="string"/> expressions.</param>
        public ColumnExpressionCommandBase(IList<string> expressions)
        {
            Expressions = expressions;
        }

        /// <inheritdoc/>
        public abstract string CommandType { get; }

        /// <inheritdoc/>
        public virtual bool IsValid
        {
            get
            {
                var cleanExpressions = GetCleanExpressions().ToList();
                return cleanExpressions.Count > 0 && cleanExpressions.Count == Expressions.Count;
            }
        }

        /// <inheritdoc/>
        public abstract string InvalidCommandMessage { get; }

        /// <summary>
        /// A comma separated string of expressions.
        /// </summary>
        public virtual string GetCommandValue() => string.Join(", ", GetCleanExpressions());

        /// <inheritdoc/>
        public virtual string CreateCommand()
        {
            if (Expressions.Count == 0 || !IsValid)
            {
                return string.Empty;
            }

            return new StringBuilder()
                .Append(CommandType)
                .Append(" ")
                .Append(GetCommandValue())
                .ToString();
        }

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();

        /// <summary>
        /// Get all valid expressions.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of valid <see cref="string"/> expressions.</returns>
        protected virtual IEnumerable<string> GetCleanExpressions()
            => Expressions
                .Where(expression => expression != null)
                .Where(expression =>
                {
                    var trimmedExpression = expression.Trim();
                    return trimmedExpression.Length > 0
                        && trimmedExpression[0] != ','
                        && trimmedExpression[trimmedExpression.Length - 1] != ',';
                })
                .Select(expression => expression.Trim());

        /// <summary>
        /// Get all invalid expressions.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of invalid <see cref="string"/> expressions.</returns>
        protected virtual IEnumerable<string> GetInvalidExpressions()
            => Expressions
                .Where(expression =>
                {
                    if (expression is null)
                    {
                        return true;
                    }

                    var trimmedExpression = expression.Trim();
                    return trimmedExpression.Length == 0
                        || trimmedExpression[0] == ','
                        || trimmedExpression[trimmedExpression.Length - 1] == ',';
                });
    }
}