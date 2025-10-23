namespace AMCode.Sql.Select
{
    /// <summary>
    /// An interface representing a factory that creates an <see cref="ISelectClause"/> object.
    /// </summary>
    public interface ISelectClauseFactory
    {
        /// <summary>
        /// Create an instance of a <see cref="ISelectClause"/> object.
        /// </summary>
        /// <returns>A <see cref="ISelectClause"/> object.</returns>
        ISelectClause Create();
    }
}