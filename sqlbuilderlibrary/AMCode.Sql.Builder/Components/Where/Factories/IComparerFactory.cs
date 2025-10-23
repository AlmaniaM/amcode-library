using System.Collections.Generic;
using AMCode.Common.FilterStructures;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface designed to be a factory for creating an <see cref="IComparer{T}"/> of type <see cref="IFilterItem"/>.
    /// </summary>
    public interface IComparerFactory
    {
        /// <summary>
        /// Create a <see cref="IComparer{T}"/> of type <see cref="IFilterItem"/>.
        /// </summary>
        /// <param name="filter">The filter the comparer is for.</param>
        /// <param name="isIdFilter">Whether or not the filter is an ID filter.</param>
        /// <returns>An <see cref="IComparer{T}"/> of type <see cref="IFilterItem"/>.</returns>
        IComparer<IFilterItem> Create(IFilter filter, bool isIdFilter);
    }
}