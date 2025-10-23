namespace AMCode.Sql.OrderBy
{
    /// <summary>
    /// An interface that represents an <see cref="IOrderByClause"/> factory.
    /// </summary>
    public interface IOrderByClauseFactory
    {
        /// <summary>
        /// Create an instance of a <see cref="IOrderByClause"/> object.
        /// </summary>
        /// <returns>A <see cref="IOrderByClause"/> object.</returns>
        IOrderByClause Create();
    }
}