namespace AMCode.Sql.Where.Models
{
    /// <summary>
    /// Represents different WHERE clause construction types.
    /// </summary>
    public enum FilterConditionSectionType
    {
        /// <summary>
        /// Filter behavior for this condition type will render filters in the where clause with
        /// the last selected filter values first followed by and 'OR' clause and then the
        /// rest of the filters.
        /// </summary>
        LastSelected,

        /// <summary>
        /// Filter behavior for this condition type will render all filter values normally
        /// where each filter is rendered as filterName IN (...values) OR filterName IN (...values) OR etc...
        /// </summary>
        Default
    }
}