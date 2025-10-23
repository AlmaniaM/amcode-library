namespace AMCode.Sql.From
{
    /// <summary>
    /// A factory class for creating a <see cref="IFromClause"/> object.
    /// </summary>
    public class FromClauseFactory : IFromClauseFactory
    {
        /// <inheritdoc/>
        public IFromClause Create() => new FromClause();
    }
}