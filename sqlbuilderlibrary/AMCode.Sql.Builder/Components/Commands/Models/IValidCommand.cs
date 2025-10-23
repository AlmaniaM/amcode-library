namespace AMCode.Sql.Commands.Models
{
    /// <summary>
    /// An interface designed for validating commands.
    /// </summary>
    public interface IValidCommand
    {
        /// <summary>
        /// Whether or not the command can be successfully constructed.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// A message describing why the command is invalid.
        /// </summary>
        string InvalidCommandMessage { get; }
    }
}