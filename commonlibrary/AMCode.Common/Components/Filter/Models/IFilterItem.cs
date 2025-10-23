namespace AMCode.Common.FilterStructures
{
    /// <summary>
    /// A class designed to hold information for a single filter item.
    /// </summary>
    public interface IFilterItem
    {
        /// <summary>
        /// The filter ID value if it exists.
        /// </summary>
        string FilterId { get; set; }

        /// <summary>
        /// The filter value.
        /// </summary>
        string FilterVal { get; set; }

        /// <summary>
        /// Whether or not the filter has been selected.
        /// </summary>
        bool Selected { get; set; }

        /// <summary>
        /// Whether or not the filter is active.
        /// </summary>
        bool Disabled { get; set; }
    }
}