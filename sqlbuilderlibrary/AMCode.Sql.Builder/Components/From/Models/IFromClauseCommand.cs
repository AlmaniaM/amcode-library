using AMCode.Sql.Commands;
using AMCode.Sql.Commands.Models;

namespace AMCode.Sql.From
{
    /// <summary>
    /// An interface designed to represent a FROM clause.
    /// </summary>
    public interface IFromClauseCommand : IClauseCommand
    {
        /// <summary>
        /// Get the FROM source.
        /// </summary>
        /// <returns>A <see cref="string"/> representing the FROM clause source.</returns>
        string GetFrom();

        /// <summary>
        /// Set the table to select from.
        /// </summary>
        /// <param name="schema">The schema of the table.</param>
        /// <param name="tableName">The table name.</param>
        void SetFrom(string schema, string tableName);

        /// <summary>
        /// Set the table to select from.
        /// </summary>
        /// <param name="schema">The schema of the table.</param>
        /// <param name="tableName">The table name.</param>
        /// <param name="alias">The alias to give the table.</param>
        void SetFrom(string schema, string tableName, string alias);

        /// <summary>
        /// Set the table to select from.
        /// </summary>
        /// <param name="selectCommand">The <see cref="ISelectCommand"/> to use as the source.</param>
        /// <param name="subQueryAlias">An alias to give to the subquery.</param>
        void SetFrom(ISelectCommand selectCommand, string subQueryAlias);
    }
}