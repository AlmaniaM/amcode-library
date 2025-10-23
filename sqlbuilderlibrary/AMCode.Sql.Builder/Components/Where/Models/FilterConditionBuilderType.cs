using AMCode.Sql.Where.Internal;

namespace AMCode.Sql.Where
{
    /// <summary>
    /// Used for determining which <see cref="IFilterConditionBuilder"/> to use.
    /// </summary>
    public enum FilterConditionBuilderType
    {
        /// <summary>
        /// Use the <see cref="FilterBetweenConditionBuilder"/> class.
        /// </summary>
        Between,
        /// <summary>
        /// Use the <see cref="FilterInConditionBuilder"/> class.
        /// </summary>
        In
    }
}