namespace AMCode.Sql.Select
{
    /// <summary>
    /// A factory class for creating a <see cref="ISelectClause"/> object.
    /// </summary>
    public class SelectClauseFactory : ISelectClauseFactory
    {
        /// <inheritdoc/>
        public ISelectClause Create() => new SelectClause();
    }
}