namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface representing a filter condition value builder.
    /// </summary>
    public interface IFilterConditionValueBuilder
    {
        /// <summary>
        /// The number of values in the condition.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Creates a string the represents the values of a WHERE clause condition section. For example,
        /// the values would be something like Filter IN "(...values)".
        /// </summary>
        /// <returns>A <see cref="string"/> WHERE clause condition section.</returns>
        string CreateFilterConditionValue();
    }
}