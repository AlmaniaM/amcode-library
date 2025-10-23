namespace AMCode.Sql.GroupBy
{
    /// <summary>
    /// An interface that represents a factory which creates an <see cref="IGroupByClause"/>.
    /// </summary>
    public interface IGroupByClauseFactory
    {
        /// <summary>
        /// Create an instance of a <see cref="IGroupByClause"/> object.
        /// </summary>
        /// <returns>A <see cref="IGroupByClause"/> object.</returns>
        IGroupByClause Create();
    }
}