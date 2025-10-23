using System;
using System.Collections.Generic;
using System.Globalization;
using AMCode.Common.FilterStructures;
using AMCode.Common.Util;
using AMCode.Sql.Extensions.FilterItems;

namespace AMCode.Sql.Components.Where.Comparers
{
    /// <summary>
    /// A class designed to compare two <see cref="IFilterItem"/> objects.
    /// </summary>
    public class CompareDateTimeFilterItem : IComparer<IFilterItem>
    {
        private readonly bool isIdFilter;
        private readonly string dateFormat;

        /// <summary>
        /// Create an instance of the <see cref="CompareDateTimeFilterItem"/> class.
        /// </summary>
        /// <param name="dateFormat">The date format to use when parsing the date string.</param>
        /// <param name="isIdFilter">Whether or not this is an ID filter.</param>
        public CompareDateTimeFilterItem(string dateFormat, bool isIdFilter)
        {
            this.dateFormat = dateFormat;
            this.isIdFilter = isIdFilter;
        }

        /// <summary>
        /// Compare two <see cref="IFilterItem"/> objects whose <see cref="IFilterItem.FilterVal"/> or <see cref="IFilterItem.FilterId"/>
        /// values are dates.
        /// </summary>
        /// <param name="leftFilterItem">The left <see cref="IFilterItem"/> to compare.</param>
        /// <param name="rightFilterItem">The right <see cref="IFilterItem"/> to compare.</param>
        /// <returns>An <see cref="int"/> representing the size of the left <see cref="IFilterItem"/> compared to the right.
        /// If 1 then it's larger, if 0 then they are equal, if -1 then it's smaller.</returns>
        public int Compare(IFilterItem leftFilterItem, IFilterItem rightFilterItem)
        {
            var leftDateString = leftFilterItem.GetValue(isIdFilter);
            var rightDateString = rightFilterItem.GetValue(isIdFilter);

            var leftDateParsed = DateTime.TryParseExact(leftDateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var leftDate);
            var rightValueParsed = DateTime.TryParseExact(rightDateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var rightDate);

            if (!leftDateParsed || !rightValueParsed)
            {
                var dateString = !leftDateParsed ? leftDateString : rightDateString;
                var header = ExceptionUtil.CreateExceptionHeader<IFilterItem, IFilterItem, int>(Compare);
                throw new ArgumentException($"{header} Error: Cannot parse the give date. Date was \"{dateString}\".");
            }

            return leftDate.CompareTo(rightDate);
        }
    }
}