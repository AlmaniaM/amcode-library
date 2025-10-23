using System.Collections.Generic;
using System.Text;
using AMCode.Sql.Commands.Extensions;
using AMCode.Sql.GroupBy;
using AMCode.Sql.OrderBy;
using AMCode.Sql.Select;

namespace AMCode.Sql.Commands
{
    /// <summary>
    /// A class designed to create a SELECT command.
    /// </summary>
    public class SelectCommand : CommandBase, ISelectCommand
    {
        /// <summary>
        /// Create an instance of the <see cref="SelectCommand"/> class.
        /// </summary>
        public SelectCommand() : base() { }

        /// <summary>
        /// A copy constructor to create an instance of the <see cref="SelectCommand"/> class.
        /// </summary>
        /// <param name="command">The <see cref="ISelectCommand"/> to copy.</param>
        public SelectCommand(ISelectCommand command) : base(command)
        {
            EndCommand = command.EndCommand;
            From = command.From;
            GroupBy = command.GroupBy;
            IndentLevel = command.IndentLevel;
            Limit = command.Limit;
            Offset = command.Offset;
            OrderBy = command.OrderBy;
            Select = command.Select;
        }

        /// <inheritdoc/>
        public new string CommandType => "SELECT";

        /// <inheritdoc/>
        public bool EndCommand { get; set; } = false;

        /// <summary>
        /// Creates an executable SELECT command.
        /// </summary>
        /// <inheritdoc/>
        public override string CreateCommand()
        {
            var fromWhere = base.CreateCommand(IndentLevel == 0 ? IndentLevel : IndentLevel - 1);

            if (!IsValid)
            {
                return string.Empty;
            }

            var commandStrings = new List<string>()
                .AddCommand(Select.CreateCommand())
                .AddCommand(fromWhere)
                .AddCommand(GroupBy?.CreateCommand())
                .AddCommand(OrderBy?.CreateCommand())
                .AddCommand("OFFSET", Offset.ToString())
                .AddCommand("LIMIT", Limit.ToString())
                .EndCommand(EndCommand);

            return CompileCommands(commandStrings, EndCommand, IndentLevel);
        }

        /// <inheritdoc/>
        public IGroupByClauseCommand GroupBy { get; set; }

        /// <inheritdoc/>
        public override bool IsValid
        {
            get
            {
                var baseIsValid = base.IsValid;
                var selectIsValid = Select.IsValid;
                return baseIsValid && selectIsValid;
            }
        }

        /// <inheritdoc/>
        public override string InvalidCommandMessage
        {
            get
            {
                var sb = new StringBuilder();

                var baseIsValid = base.IsValid;
                var selectIsValid = IsValid;

                if (!baseIsValid || !selectIsValid)
                {
                    sb.Append("Cannot construct SELECT clause. Error(s): ");
                }

                if (!baseIsValid)
                {
                    sb.Append(base.InvalidCommandMessage).Append(", ");
                }

                if (!selectIsValid)
                {
                    sb.Append(Select.InvalidCommandMessage);
                }

                return sb.ToString();
            }
        }

        /// <inheritdoc/>
        public int? Limit { get; set; }

        /// <inheritdoc/>
        public int? Offset { get; set; }

        /// <inheritdoc/>
        public IOrderByClauseCommand OrderBy { get; set; }

        /// <inheritdoc/>
        public ISelectClauseCommand Select { get; set; }

        /// <inheritdoc/>
        public new string GetCommandValue() => CreateCommand();

        /// <inheritdoc cref="CreateCommand"/>
        public override string ToString() => CreateCommand();
    }
}