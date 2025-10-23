using System;
using AMCode.Common.Extensions.FilterItems;
using AMCode.Common.Extensions.Strings;
using AMCode.Common.FilterStructures;
using Newtonsoft.Json;

namespace AMCode.Common.Extensions.Filters
{
    /// <summary>
    /// A static class designed to only hold Extension Methods for the <see cref="IFilter"/> <see cref="Type"/>.
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Clone an <see cref="IFilter"/> object.
        /// </summary>
        /// <param name="filter">The filter you want to check for a specific value.</param>
        /// <returns>An <see cref="IFilter"/> deep copy of the provided <see cref="IFilter"/> object.</returns>
        public static IFilter Clone(this IFilter filter)
        {
            var filterString = JsonConvert.SerializeObject(
                filter,
                Formatting.None,
                new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                });
            var filterType = filter.GetType();
            var newFilter = JsonConvert.DeserializeObject(filterString, filterType) as IFilter;
            return newFilter;
        }

        /// <summary>
        /// Checks the list of <seealso cref="IFilter"/> objects to see if the provided <seealso cref="string"/> 
        /// value exists in any of them.
        /// </summary>
        /// <param name="filter">The filter you want to check for a specific value.</param>
        /// <param name="value">The <see cref="string"/> value you are looking for.</param>
        /// <returns>Will return true if the <see cref="IFilter"/> contains the <paramref name="value"/>.
        /// Otherwise, it'll return false.</returns>
        public static bool Contains(this IFilter filter, string value)
        {
            if (filter.FilterItems == null)
            {
                return false;
            }

            if (filter.FilterItems.Count == 0)
            {
                return false;
            }

            var hasIdColumn = filter.FilterIdName != null;

            for (var i = 0; i < filter.FilterItems.Count; i++)
            {
                var fi = filter.FilterItems[i];

                if (fi.Contains(value, hasIdColumn))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check to see if this <seealso cref="IFilter"/> object's field name or id name (if it exists) matches the provided string value;
        /// </summary>
        /// <param name="filter">The instance of the <seealso cref="Filter"/> object you want to compare against the string value.</param>
        /// <param name="filterName">A string name you want to compare against the <seealso cref="IFilter"/> object name values.</param>
        /// <param name="checkIdOnly"></param>
        /// <returns></returns>
        public static bool IsFilter(this IFilter filter, string filterName, bool checkIdOnly)
        {
            if (checkIdOnly)
            {
                if (filter.FilterIdName == null)
                {
                    return false;
                }

                var isFilter = filter.FilterIdName.FieldName.EqualsIgnoreCase(filterName);
                return isFilter;
            }

            return filter.IsFilter(filterName);
        }

        /// <summary>
        /// Check to see if this <seealso cref="IFilter"/> object's field name or id name (if it exists) matches the provided string value;
        /// </summary>
        /// <param name="filter">The instance of the <seealso cref="IFilter"/> object you want to compare against the string value.</param>
        /// <param name="filterName">A string name you want to compare against the <seealso cref="IFilter"/> object name values.</param>
        /// <returns></returns>
        public static bool IsFilter(this IFilter filter, string filterName)
        {
            var isFilterName = filterName.EqualsIgnoreCase(filter.FilterName.FieldName);
            var isFilterIdName = false;
            if (filter.FilterIdName != null && !isFilterName)
            {
                isFilterIdName = filterName.EqualsIgnoreCase(filter.FilterIdName.FieldName);
            }

            return isFilterIdName || isFilterName;
        }
    }
}