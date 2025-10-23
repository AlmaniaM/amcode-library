using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AMCode.Common.FilterStructures
{
    /// <summary>
    /// A class representing a filter.
    /// </summary>
    public class Filter : IFilter
    {
        /// <summary>
        /// Create an instance of the <see cref="Filter"/> class.
        /// </summary>
        public Filter() { }

        /// <summary>
        /// Create an instance of the <see cref="Filter"/> class.
        /// </summary>
        [JsonConstructor]
        public Filter(FilterName filterName, FilterName filterIdname, List<FilterItem> filterItems)
        {
            FilterName = filterName;
            FilterIdName = filterIdname;
            FilterItems = filterItems.Cast<IFilterItem>().ToList();
        }

        /// <summary>
        /// The name of this filter.
        /// </summary>
        public IFilterName FilterName { get; set; }

        /// <summary>
        /// A <see cref="IList{T}"/> of <see cref="IFilterItem"/>s.
        /// </summary>
        public IList<IFilterItem> FilterItems { get; set; }

        /// <summary>
        /// The name of this filters' ID filter counterpart.
        /// </summary>
        public IFilterName FilterIdName { get; set; }

        /// <summary>
        /// Whether or not this filter is required.
        /// </summary>
        public bool Required { get; set; }
    }
}