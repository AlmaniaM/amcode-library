namespace AMCode.Sql.From
{
    /// <summary>
    /// An interface representing a factory that creates an <see cref="IFromClause"/> object.
    /// </summary>
    public interface IFromClauseFactory
    {
        /// <summary>
        /// Create an instance of a <see cref="IFromClause"/> object.
        /// </summary>
        /// <returns>A <see cref="IFromClause"/> object.</returns>
        IFromClause Create();
    }
}