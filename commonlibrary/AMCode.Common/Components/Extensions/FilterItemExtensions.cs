using System;
using System.Collections.Generic;
using AMCode.Common.FilterStructures;

namespace AMCode.Common.Extensions.FilterItems
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="IFilterItem"/> <see cref="Type"/>.
    /// </summary>
    public static class FilterItemExtensions
    {
        /// <summary>
        /// Checks the list of <seealso cref="IFilterItem"/> objects to see if the provided <seealso cref="string"/> 
        /// value exists in any of them.
        /// </summary>
        /// <param name="filterItems"></param>
        /// <param name="val"></param>
        /// <param name="hasIdValues"></param>
        /// <returns></returns>
        public static bool Contains(this IList<IFilterItem> filterItems, string val, bool hasIdValues)
        {
            if (filterItems.Count == 0)
            {
                return false;
            }

            for (var i = 0; i < filterItems.Count; i++)
            {
                var fi = filterItems[i];

                if (fi.Contains(val, hasIdValues))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks the <seealso cref="IFilterItem"/> object to see if the provided <seealso cref="string"/> 
        /// value exists in it.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="val">The <seealso cref="string"/> value to search for.</param>
        /// <param name="hasIdColumn">Set to true if you know that the <seealso cref="IFilterItem"/> has a value of the 
        /// filterIdVal property.</param>
        /// <returns></returns>
        public static bool Contains(this IFilterItem fi, string val, bool hasIdColumn)
        {
            bool stringEqualsIgnoreCase(string s1, string s2) => string.Equals(s1, s2, StringComparison.InvariantCultureIgnoreCase);

            if (hasIdColumn)
            {
                return stringEqualsIgnoreCase(fi.FilterId, val);
            }
            else
            {
                return stringEqualsIgnoreCase(fi.FilterVal, val);
            }
        }

        /// <summary>
        /// Convert this <see cref="IFilterItem"/> into a <see cref="FilterItem"/> instance.
        /// </summary>
        /// <param name="filterItem">A <see cref="IFilterItem"/> object.</param>
        /// <returns>An instance of a <see cref="FilterItem"/> class.</returns>
        public static FilterItem ToFilterItem(this IFilterItem filterItem)
            => new FilterItem
            {
                Disabled = filterItem?.Disabled ?? false,
                FilterId = filterItem?.FilterId,
                FilterVal = filterItem?.FilterVal,
                Selected = filterItem?.Selected ?? false
            };
    }
}