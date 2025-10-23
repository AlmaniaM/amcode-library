using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands;

namespace AMCode.Sql.OrderBy
{
    /// <summary>
    /// A class designed for creating an ORDER BY clause command.
    /// </summary>
    public class OrderByClauseCommand : ColumnExpressionCommandBase, IOrderByClauseCommand
    {
        /// <summary>
        /// The ORDER BY command that's prepended to the clause.
        /// </summary>
        public static readonly string CommandName = "ORDER BY";

        /// <summary>
        /// Create an instance of the <see cref="OrderByClauseCommand"/> class.
        /// </summary>
        public OrderByClauseCommand() : base() { }

        /// <summary>
        /// Create an instance of the <see cref="OrderByClauseCommand"/> class.
        /// </summary>
        /// <param name="orderByExpressions">Provide an <see cref="IEnumerable{T}"/> of <see cref="string"/> ORDER BY expressions.</param>
        public OrderByClauseCommand(IEnumerable<string> orderByExpressions) : base(orderByExpressions.ToList()) { }

        /// <inheritdoc/>
        public IEnumerable<string> GetOrderByExpressions() => Expressions;

        /// <summary>
        /// Creates an ORDER BY clause.
        /// </summary>
        /// <inheritdoc/>
        public override string CreateCommand() => base.CreateCommand();

        /// <summary>
        /// An ORDER BY clause command.
        /// </summary>
        public override string CommandType => CommandName;

        /// <summary>
        /// A comma separated string of ORDER BY expressions.
        /// </summary>
        public override string GetCommandValue() => base.GetCommandValue();

        /// <inheritdoc/>
        public override bool IsValid => base.IsValid;

        /// <inheritdoc/>
        public override string InvalidCommandMessage
        {
            get
            {
                var cleanExpressions = GetCleanExpressions().ToList();

                if (cleanExpressions.Count == 0)
                {
                    return "There are no ORDER BY expressions to build an ORDER BY clause with.";
                }

                if (cleanExpressions.Count != Expressions.Count)
                {
                    var invalidExpressions = string.Join(", ", GetInvalidExpressions().Select(expression => expression is null ? "'null'" : $"'{expression}'"));
                    return $"There are invalid ORDER BY expressions. Values are {invalidExpressions}.";
                }

                return string.Empty;
            }
        }

        /// <inheritdoc/>
        public void SetOrderByExpression(string expression) => Expressions.Add(expression);

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();
    }
}