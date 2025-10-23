namespace AMCode.Common.FilterStructures
{
    /// <summary>
    /// A class designed to hold information for a single filter option.
    /// </summary>
    public class FilterItem : IFilterItem
    {
        /// <summary>
        /// The filter value.
        /// </summary>
		public string FilterVal { get; set; }

        /// <summary>
        /// The filter ID value if it exists.
        /// </summary>
		public string FilterId { get; set; }

        /// <summary>
        /// Whether or not the filter has been selected.
        /// </summary>
		public bool Selected { get; set; }

        /// <summary>
        /// Whether or not the filter is active.
        /// </summary>
		public bool Disabled { get; set; }
    }
}