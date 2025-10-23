namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface representing a filter condition builder.
    /// </summary>
    public interface IFilterConditionOrganizer
    {
        /// <summary>
        /// Create a Filter IN condition clause and add it to the provided <see cref="IWhereClauseBuilder"/>.
        /// </summary>
        /// <param name="whereClause">A <see cref="IWhereClauseBuilder"/> object to add the newly
        /// created <see cref="IFilterCondition"/></param>
        void AddFilterCondition(IWhereClauseBuilder whereClause);
    }
}