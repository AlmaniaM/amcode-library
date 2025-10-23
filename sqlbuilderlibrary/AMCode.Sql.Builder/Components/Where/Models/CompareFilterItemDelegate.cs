using AMCode.Common.FilterStructures;

namespace AMCode.Sql.Where.Internal
{
    /// <summary>
    /// An interface designed to compare the size of two <see cref="IFilterItem"/> objects.
    /// </summary>
    public interface IFilterItemComparer
    {
        /// <summary>
        /// Compare the size of each <see cref="IFilterItem"/> and return an <see cref="int"/> indicating
        /// if the first one is larger, equal, or smaller.
        /// </summary>
        /// <param name="leftFilterItem">The left-hand side <see cref="IFilterItem"/> object.</param>
        /// <param name="rightFilterItem">The right-hand side <see cref="IFilterItem"/> object.</param>
        /// <param name="isIdFilter">Whether or not the filter is an ID filter.</param>
        /// <returns>An <see cref="int"/> representing the size comparison of the left-hand <see cref="IFilterItem"/>
        /// to the right-hand side. If 1 then it's larger, 0 it's equal, and -1 then it's smaller.</returns>
        int Compare(IFilterItem leftFilterItem, IFilterItem rightFilterItem, bool isIdFilter);
    }
}