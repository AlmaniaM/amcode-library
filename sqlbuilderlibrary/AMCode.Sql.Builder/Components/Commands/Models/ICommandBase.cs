using AMCode.Sql.Commands.Models;
using AMCode.Sql.From;
using AMCode.Sql.Where;

namespace AMCode.Sql.Commands
{
    /// <summary>
    /// An base interface designed for creating DB commands.
    /// </summary>
    public interface ICommandBase : ICommand, IValidCommand
    {
        /// <summary>
        /// The level of indentation this command sits at.
        /// </summary>
        int IndentLevel { get; set; }

        /// <summary>
        /// Holds the FROM clause command.
        /// </summary>
        IFromClauseCommand From { get; set; }

        /// <summary>
        /// Holds the WHERE clause comma separated filters.
        /// </summary>
        IWhereClauseCommand Where { get; set; }
    }
}