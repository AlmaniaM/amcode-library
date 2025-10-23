using System.Collections.Generic;

namespace AMCode.Common.FilterStructures
{
    /// <summary>
    /// Represents one filter object with many values.
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// The name of this filters' ID filter counterpart.
        /// </summary>
        IFilterName FilterIdName { get; set; }

        /// <summary>
        /// A <see cref="IList{T}"/> of <see cref="IFilterItem"/>s.
        /// </summary>
        IList<IFilterItem> FilterItems { get; set; }

        /// <summary>
        /// The name of this filter.
        /// </summary>
        IFilterName FilterName { get; set; }

        /// <summary>
        /// Whether or not this filter is required.
        /// </summary>
        bool Required { get; set; }
    }
}