namespace AMCode.Sql.OrderBy
{
    /// <summary>
    /// A factory class designed for creating instances of <see cref="IOrderByClause"/> objects.
    /// </summary>
    public class OrderByClauseFactory : IOrderByClauseFactory
    {
        /// <inheritdoc/>
        public IOrderByClause Create() => new OrderByClause();
    }
}