using AMCode.Sql.Commands.Models;
using AMCode.Sql.GroupBy;
using AMCode.Sql.OrderBy;
using AMCode.Sql.Select;

namespace AMCode.Sql.Commands
{
    /// <summary>
    /// An interface designed for creating a complete SELECT clause.
    /// </summary>
    public interface ISelectCommand : ICommandBase, IClauseCommand
    {
        /// <summary>
        /// Whether or not this command should be closed off with a semicolon (;).
        /// </summary>
        bool EndCommand { get; set; }

        /// <summary>
        /// Holds the GROUP BY comma separated columns string.
        /// </summary>
        IGroupByClauseCommand GroupBy { get; set; }

        /// <summary>
        /// Holds the row limit number.
        /// </summary>
        int? Limit { get; set; }

        /// <summary>
        /// Holds the number of rows to fetch.
        /// </summary>
        int? Offset { get; set; }

        /// <summary>
        /// Holds the ORDER BY comma separated columns string.
        /// </summary>
        IOrderByClauseCommand OrderBy { get; set; }

        /// <summary>
        /// Holds the comma separated columns for the SELECT clause.
        /// </summary>
        ISelectClauseCommand Select { get; set; }
    }
}