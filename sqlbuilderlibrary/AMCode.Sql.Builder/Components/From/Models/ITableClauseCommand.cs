using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.From
{
    /// <summary>
    /// An interface designed to represent a table name.
    /// </summary>
    public interface ITableClauseCommand : IClauseCommand
    {
        /// <summary>
        /// A <see cref="string"/> alias for the table.
        /// </summary>
        string Alias { get; set; }

        /// <summary>
        /// A <see cref="string"/> schema name to use.
        /// </summary>
        string Schema { get; set; }

        /// <summary>
        /// A <see cref="string"/> table name.
        /// </summary>
        string TableName { get; set; }
    }
}