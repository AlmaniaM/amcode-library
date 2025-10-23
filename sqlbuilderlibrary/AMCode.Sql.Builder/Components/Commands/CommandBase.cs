using System.Collections.Generic;
using System.Text;
using AMCode.Sql.Commands.Extensions;
using AMCode.Sql.From;
using AMCode.Sql.Where;

namespace AMCode.Sql.Commands
{
    /// <summary>
    /// A class designed to represent the base requirements of a DB command.
    /// </summary>
    public class CommandBase : ICommandBase
    {
        /// <summary>
        /// Create an instance of the <see cref="CommandBase"/> class.
        /// </summary>
        public CommandBase() { }

        /// <summary>
        /// A copy constructor that creates an instance of the <see cref="CommandBase"/> class.
        /// </summary>
        /// <param name="command">An <see cref="ICommandBase"/> class to copy properties from.</param>
        public CommandBase(ICommandBase command)
        {
            From = command.From;
            Where = command.Where;
        }

        /// <inheritdoc/>
        public string CommandType => "SELECTBASE";

        /// <inheritdoc/>
        public IFromClauseCommand From { get; set; }

        /// <inheritdoc/>
        public int IndentLevel { get; set; } = 0;

        /// <inheritdoc/>
        public virtual string InvalidCommandMessage
        {
            get
            {
                if (IsValid)
                {
                    return string.Empty;
                }

                if (From is null)
                {
                    return $"You must provide a valid table. Current table value is 'null'.";
                }

                return From.InvalidCommandMessage;
            }
        }

        /// <inheritdoc/>
        public virtual bool IsValid => !(From is null) && From.IsValid;

        /// <inheritdoc/>
        public IWhereClauseCommand Where { get; set; }

        /// <inheritdoc/>
        public virtual string CreateCommand() => CreateCommand(IndentLevel);

        /// <param name="indentLevel">The number of levels to indent the command.</param>
        /// <inheritdoc cref="CreateCommand()"/>
        public virtual string CreateCommand(int indentLevel)
        {
            if (!IsValid)
            {
                return string.Empty;
            }

            var commandStrings = new List<string>()
                .AddCommand(From?.CreateCommand())
                .AddCommand(Where?.CreateCommand());

            return CompileCommands(commandStrings, false, indentLevel);
        }

        /// <inheritdoc/>
        public string GetCommandValue() => CreateCommand();

        /// <inheritdoc/>
        public override string ToString() => CreateCommand();

        /// <summary>
        /// Compiles the provided list of commands into an executable string.
        /// </summary>
        /// <param name="commands">A <see cref="IList{T}"/> of <see cref="string"/> commands to compile.</param>
        /// <param name="hasEnd">Whether or not the commands list has a command terminator semicolon.</param>
        /// <param name="indentLevel">The indentation level.</param>
        /// <returns>An executable <see cref="string"/>.</returns>
        protected string CompileCommands(IList<string> commands, bool hasEnd, int indentLevel = 0)
        {
            var stringBuilder = new StringBuilder();
            var indentString = new string('\t', indentLevel);

            for (var i = 0; i < commands.Count; i++)
            {
                var command = commands[i];

                if (i < commands.Count - (hasEnd ? 2 : 1))
                {
                    stringBuilder.AppendLine($"{indentString}{command}");
                }
                else
                {
                    stringBuilder.Append($"{indentString}{command}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}