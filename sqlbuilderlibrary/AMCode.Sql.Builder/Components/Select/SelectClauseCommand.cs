using System.Collections.Generic;
using System.Linq;
using AMCode.Sql.Commands;

namespace AMCode.Sql.Select
{
    /// <summary>
    /// A class designed to represent a SELECT clause.
    /// </summary>
    public class SelectClauseCommand : ColumnExpressionCommandBase, ISelectClauseCommand
    {
        /// <summary>
        /// The SELECT command that's prepended to the clause.
        /// </summary>
        public static readonly string CommandName = "SELECT";

        /// <summary>
        /// Create an instance of the <see cref="SelectClauseCommand"/> class.
        /// </summary>
        public SelectClauseCommand() : base() { }

        /// <summary>
        /// Create an instance of the <see cref="SelectClauseCommand"/> class.
        /// </summary>
        /// <param name="queryExpressions">Provide an <see cref="IEnumerable{T}"/> of <see cref="string"/> column query expressions.</param>
        public SelectClauseCommand(IEnumerable<string> queryExpressions)
            : base(queryExpressions.ToList()) { }

        /// <summary>
        /// A SELECT clause command.
        /// </summary>
        public override string CommandType => CommandName;

        /// <inheritdoc/>
        public override string InvalidCommandMessage
        {
            get
            {
                var cleanExpressions = GetCleanExpressions().ToList();

                if (cleanExpressions.Count == 0)
                {
                    return "There are no query expressions to build a SELECT clause with.";
                }

                if (cleanExpressions.Count != Expressions.Count)
                {
                    var invalidExpressions = string.Join(", ", GetInvalidExpressions().Select(expression => expression is null ? "'null'" : $"'{expression}'"));
                    return $"There are invalid query expressions. Values are {invalidExpressions}.";
                }

                return string.Empty;
            }
        }

        /// <inheritdoc/>
        public override bool IsValid => base.IsValid;

        /// <inheritdoc/>
        public IEnumerable<string> GetQueryExpressions() => Expressions;

        /// <summary>
        /// Creates a SELECT clause.
        /// </summary>
        /// <inheritdoc/>
        public override string CreateCommand() => base.CreateCommand();

        /// <summary>
        /// A comma separated string of column query expressions.
        /// </summary>
        public override string GetCommandValue() => base.GetCommandValue();

        /// <inheritdoc/>
        public void AddQueryExpression(string expression) => Expressions.Add(expression);

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();

        /// <inheritdoc/>
        public void SetQueryExpressions(IEnumerable<string> queryExpressions)
        {
            foreach (var queryExpression in queryExpressions)
            {
                Expressions.Add(queryExpression);
            }
        }
    }
}