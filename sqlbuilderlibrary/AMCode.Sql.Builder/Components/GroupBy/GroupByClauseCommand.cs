using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands;

namespace AMCode.Sql.GroupBy
{
    /// <summary>
    /// A class designed to represent a GROUP BY clause as a command object.
    /// </summary>
    public class GroupByClauseCommand : ColumnExpressionCommandBase, IGroupByClauseCommand
    {
        /// <summary>
        /// The GROUP BY command that's prepended to the clause.
        /// </summary>
        public static readonly string CommandName = "GROUP BY";

        /// <summary>
        /// Create an instance of the <see cref="GroupByClauseCommand"/> class.
        /// </summary>
        public GroupByClauseCommand() : base() { }

        /// <summary>
        /// Create an instance of the <see cref="GroupByClauseCommand"/> class.
        /// </summary>
        /// <param name="groupByExpressions">Provide an <see cref="IEnumerable{T}"/> of <see cref="string"/> GROUP BY expressions.</param>
        public GroupByClauseCommand(IEnumerable<string> groupByExpressions) : base(groupByExpressions.ToList()) { }

        /// <inheritdoc/>
        public IEnumerable<string> GetGroupByExpressions() => Expressions;

        /// <summary>
        /// Creates a GROUP BY clause.
        /// </summary>
        /// <inheritdoc/>
        public override string CreateCommand() => base.CreateCommand();

        /// <summary>
        /// A GROUP BY clause command.
        /// </summary>
        public override string CommandType => CommandName;

        /// <summary>
        /// A comma separated string of GROUP BY expressions.
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
                    return "There are no GROUP BY expressions to build a GROUP BY clause with.";
                }

                if (cleanExpressions.Count != Expressions.Count)
                {
                    var invalidExpressions = string.Join(", ", GetInvalidExpressions().Select(expression => expression is null ? "'null'" : $"'{expression}'"));
                    return $"There are invalid GROUP BY expressions. Values are {invalidExpressions}.";
                }

                return string.Empty;
            }
        }

        /// <inheritdoc/>
        public void SetGroupByExpression(string expression) => Expressions.Add(expression);

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();
    }
}