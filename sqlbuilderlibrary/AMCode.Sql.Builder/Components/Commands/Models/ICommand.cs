namespace AMCode.Sql.Commands.Models
{
    /// <summary>
    /// An interface representing a base clause command object.
    /// </summary>
    public interface ICommand : IValidCommand
    {
        /// <summary>
        /// Create a command.
        /// </summary>
        /// <returns>A compiled command. If the command cannot be
        /// properly constructed then it will return <see cref="string.Empty"/>.</returns>
        string CreateCommand();

        /// <summary>
        /// The type of command this is.
        /// </summary>
        string CommandType { get; }

        /// <summary>
        /// The value to go along with the type of command this is.
        /// </summary>
        /// <returns>A <see cref="string"/> value to go along with the type of command this is.</returns>
        string GetCommandValue();

        /// <inheritdoc cref="CreateCommand"/>
        string ToString();
    }
}