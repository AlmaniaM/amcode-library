namespace AMCode.Common.FilterStructures
{
    /// <summary>
    /// A class designed to hold the name information for a filter.
    /// </summary>
    public class FilterName : IFilterName
    {
        /// <summary>
        /// The display name that a user sees.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The field name for data access.
        /// </summary>
        public string FieldName { get; set; }
    }
}