using AMCode.Sql.Commands;

namespace AMCode.Sql.From
{
    /// <summary>
    /// An interface designed for creating a FROM clause.
    /// </summary>
    public interface IFromClause
    {
        /// <summary>
        /// Create an instance of the <see cref="FromClauseCommand"/> class.
        /// </summary>
        /// <param name="schema">Provide a <see cref="string"/> schema for the table.</param>
        /// <param name="tableName">Provide a <see cref="string"/> table name.</param>
        IFromClauseCommand CreateClause(string schema, string tableName);

        /// <summary>
        /// Create an instance of the <see cref="FromClauseCommand"/> class.
        /// </summary>
        /// <param name="schema">Provide a <see cref="string"/> schema for the table.</param>
        /// <param name="tableName">Provide a <see cref="string"/> table name.</param>
        /// <param name="alias">Provide a <see cref="string"/> alias for the table.</param>
        IFromClauseCommand CreateClause(string schema, string tableName, string alias);

        /// <summary>
        /// Create an instance of the <see cref="FromClauseCommand"/> class.
        /// </summary>
        /// <param name="selectClauseCommand">An <see cref="ISelectCommand"/> object to use as the FROM subquery clause.</param>
        /// <param name="subQueryAlias">An alias to give to the subquery.</param>
        IFromClauseCommand CreateClause(ISelectCommand selectClauseCommand, string subQueryAlias);
    }
}