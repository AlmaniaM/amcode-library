using System;
using AMCode.Common.FilterStructures;

namespace AMCode.Common.Extensions.FilterNames
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="IFilterName"/> <see cref="Type"/>.
    /// </summary>
    public static class FilterNameExtensions
    {
        /// <summary>
        /// Convert this <see cref="IFilterName"/> into a <see cref="FilterName"/> instance.
        /// </summary>
        /// <param name="filterName">A <see cref="IFilterName"/> object.</param>
        /// <returns>An instance of a <see cref="FilterName"/> class.</returns>
        public static FilterName ToFilterName(this IFilterName filterName)
            => new FilterName
            {
                DisplayName = filterName?.DisplayName,
                FieldName = filterName?.FieldName
            };
    }
}