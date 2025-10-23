using System.Text;
using AMCode.Common.Extensions.Strings;
using AMCode.Sql.Commands;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.From
{
    /// <summary>
    /// A class designed to build the FROM clause.
    /// </summary>
    public class FromClauseCommand : IFromClauseCommand
    {
        private string subQueryAlias;
        private IClauseCommand fromCommand;

        /// <summary>
        /// Create an instance of the <see cref="FromClauseCommand"/> class.
        /// </summary>
        public FromClauseCommand() { }

        /// <summary>
        /// Create an instance of the <see cref="FromClauseCommand"/> class.
        /// </summary>
        /// <param name="schema">Provide a <see cref="string"/> schema for the table.</param>
        /// <param name="tableName">Provide a <see cref="string"/> table name.</param>
        public FromClauseCommand(string schema, string tableName)
            : this(schema, tableName, string.Empty)
        { }

        /// <summary>
        /// Create an instance of the <see cref="FromClauseCommand"/> class.
        /// </summary>
        /// <param name="schema">Provide a <see cref="string"/> schema for the table.</param>
        /// <param name="tableName">Provide a <see cref="string"/> table name.</param>
        /// <param name="alias">Provide a <see cref="string"/> alias for the table.</param>
        public FromClauseCommand(string schema, string tableName, string alias)
        {
            fromCommand = new TableClauseCommand
            {
                Alias = alias,
                Schema = schema,
                TableName = tableName
            };
        }

        /// <summary>
        /// Create an instance of the <see cref="FromClauseCommand"/> class.
        /// </summary>
        /// <param name="selectClauseCommand">An <see cref="ISelectCommand"/> object to use as the FROM subquery clause.</param>
        /// <param name="subQueryAlias">An alias to give to the subquery.</param>
        public FromClauseCommand(ISelectCommand selectClauseCommand, string subQueryAlias)
        {
            fromCommand = selectClauseCommand;
            this.subQueryAlias = subQueryAlias;
        }

        /// <inheritdoc/>
        public string CommandType => "FROM";

        /// <inheritdoc/>
        public bool IsValid => fromCommand.IsValid;

        /// <inheritdoc/>
        public string InvalidCommandMessage
        {
            get
            {
                if (fromCommand is null)
                {
                    return "The inner from command is null";
                }

                if (fromCommand.IsValid)
                {
                    return string.Empty;
                }

                return fromCommand.InvalidCommandMessage;
            }
        }

        /// <inheritdoc/>
        public string CreateCommand()
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            return new StringBuilder()
                .Append(CommandType)
                .Append(' ')
                .Append(GetCommandValue())
                .ToString();
        }

        /// <inheritdoc/>
        public string GetCommandValue()
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            if (fromCommand.CommandType.Equals("TABLE"))
            {
                return createTableCommand();
            }
            else
            {
                return createSubqueryCommand();
            }
        }

        /// <inheritdoc/>
        public string GetFrom() => CreateCommand();

        /// <inheritdoc/>
        public void SetFrom(string schema, string tableName)
        {
            fromCommand = new TableClauseCommand
            {
                Schema = schema,
                TableName = tableName
            };
        }

        /// <inheritdoc/>
        public void SetFrom(string schema, string tableName, string alias)
        {
            fromCommand = new TableClauseCommand
            {
                Alias = alias,
                Schema = schema,
                TableName = tableName
            };
        }

        /// <inheritdoc/>
        public void SetFrom(ISelectCommand selectCommand, string subQueryAlias)
        {
            fromCommand = selectCommand;
            this.subQueryAlias = subQueryAlias;
        }

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();

        /// <summary>
        /// Create a FROM (SubQuery...) command.
        /// </summary>
        /// <returns>A <see cref="string"/> FROM (SubQuery...) command.</returns>
        private string createSubqueryCommand()
        {
            return new StringBuilder()
                .AppendLine("(")
                .AppendLine(fromCommand.CreateCommand())
                .Append(")")
                .Append(subQueryAlias.IsNullEmptyOrWhiteSpace() ? string.Empty : $" AS {subQueryAlias}")
                .ToString();
        }

        /// <summary>
        /// Create a FROM TableName command.
        /// </summary>
        /// <returns>A <see cref="string"/> FROM TableName command.</returns>
        private string createTableCommand()
        {
            return new StringBuilder()
                .Append(fromCommand.CreateCommand())
                .ToString();
        }
    }
}