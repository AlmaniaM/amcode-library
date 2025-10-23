namespace AMCode.Common.FilterStructures
{
    /// <summary>
    /// An interface representing a filter name.
    /// </summary>
    public interface IFilterName
    {
        /// <summary>
        /// The display name that a user sees.
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// The field name for data access.
        /// </summary>
        string FieldName { get; set; }
    }
}