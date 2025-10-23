namespace AMCode.Sql.Where.Models
{
    /// <summary>
    /// Represents the different types of WHERE clause builders.
    /// </summary>
    public enum WhereClauseBuilderType
    {
        /// <summary>
        /// A WHERE clause builder for global filters.
        /// </summary>
        GlobalFilters,
        /// <summary>
        /// A WHERE clause builder for regular data queries.
        /// </summary>
        Data
    }
}