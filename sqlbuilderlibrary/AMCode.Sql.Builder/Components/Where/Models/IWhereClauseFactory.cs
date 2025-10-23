namespace AMCode.Sql.Where
{
    /// <summary>
    /// An interface representing factory for creating an <see cref="IWhereClause"/> object.
    /// </summary>
    public interface IWhereClauseFactory
    {
        /// <summary>
        /// Create an instance of a <see cref="IWhereClause"/> object.
        /// </summary>
        /// <returns>A <see cref="IWhereClause"/> object.</returns>
        IWhereClause Create();
    }
}