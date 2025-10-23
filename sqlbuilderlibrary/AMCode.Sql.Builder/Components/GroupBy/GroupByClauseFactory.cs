namespace AMCode.Sql.GroupBy
{
    /// <summary>
    /// A factory class which creates the an <see cref="IGroupByClause"/> object.
    /// </summary>
    public class GroupByClauseFactory : IGroupByClauseFactory
    {
        /// <inheritdoc/>
        public IGroupByClause Create() => new GroupByClause();
    }
}