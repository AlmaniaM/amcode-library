using AMCode.Sql.Commands;

namespace AMCode.Sql.From
{
    /// <summary>
    /// A class designed for building a FROM clause command.
    /// </summary>
    public class FromClause : IFromClause
    {
        /// <inheritdoc/>
        public IFromClauseCommand CreateClause(string schema, string tableName)
            => CreateClause(schema, tableName, string.Empty);

        /// <inheritdoc/>
        public IFromClauseCommand CreateClause(string schema, string tableName, string alias)
            => new FromClauseCommand(schema, tableName, alias);

        /// <inheritdoc/>
        public IFromClauseCommand CreateClause(ISelectCommand selectCommand, string subQueryAlias)
            => new FromClauseCommand(selectCommand, subQueryAlias);
    }
}